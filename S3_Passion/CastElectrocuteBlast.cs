using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Alchemy;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI.Hud;

namespace S3_Passion
{
	public class CastElectrocuteBlast : MagicWand.CastSpell, IInteractionInstance
	{
		public class Definition : InteractionDefinition<Sim, Sim, CastElectrocuteBlast>, IOverridesVisualType, IHasTraitIcon, IHasMenuPathIcon
		{
			public InteractionVisualTypes GetVisualType
			{
				get
				{
					return InteractionVisualTypes.Trait;
				}
			}

			public override string[] GetPath(bool isFemale)
			{
				return new string[1] { MagicWand.LocalizeString(isFemale, "CastSpell", new object[0]) + Localization.Ellipsis };
			}

			public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString("Electrocution Blast");
			}

			public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (a == target || !a.SimDescription.IsWitch)
				{
					return false;
				}
				if (a.SkillManager.GetSkillLevel(SkillNames.Spellcasting) < MagicWand.kSpellLevels[4])
				{
					return false;
				}
				if (target.SimDescription.IsPregnant)
				{
					greyedOutTooltipCallback = InteractionInstance.CreateTooltipCallback(Localization.LocalizeString(target.IsFemale, "Gameplay/Actors/Sim:PregnantFailure"));
					return false;
				}
				if (a.Posture.Satisfies(CommodityKind.SwimmingInPool, null) || target.SimDescription.IsBonehilda || target.SimDescription.IsPet || target.SimDescription.ChildOrBelow || target.Posture.Satisfies(CommodityKind.SwimmingInPool, null))
				{
					return false;
				}
				if (target.SimDescription.IsServicePerson)
				{
					return true;
				}
				return MagicWand.CastSpell.CommonSpellTests(a, target, isAutonomous, ref greyedOutTooltipCallback);
			}

			public ResourceKey GetTraitIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_SpellcastingTalent_s", ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7));
			}

			public ResourceKey GetPathIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_SpellcastingTalent_s", ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7));
			}
		}

		[Tunable]
		[TunableComment("Amount that this spell drains from motives.  Range: 0-200")]
		public static float kMotiveDrain = 10f;

		[Tunable]
		[TunableComment("Delay time for the sim to do put self out interaction")]
		public static float kPutSelfOutDelay = 10f;

		public static InteractionDefinition Singleton = new Definition();

		public override MagicWand.SpellType TypeOfSpell
		{
			get
			{
				return MagicWand.SpellType.GoodLuckCharm;
			}
		}

		public override string JazzStateName
		{
			get
			{
				return "Good Luck";
			}
		}

		public override string SuccessVfxName
		{
			get
			{
				return "ep7WandSpellLuckSelf_main";
			}
		}

		public override string EpicFailVfxName
		{
			get
			{
				return "ep7WandSpellLuckFail_main";
			}
		}

		public override string HitVfxName
		{
			get
			{
				return "ep7WandSpellLuckHit_main";
			}
		}

		public override void DrainMotives()
		{
			mWand.DrainMotive(Actor, CommodityKind.MagicFatigue, 0f - MagicWand.CastGoodLuckCharm.kMotiveDrain);
		}

		public override void OnSpellSuccess()
		{
			FireManager.SimShockedBy(Target, Actor);
			Target.BuffManager.AddElement(BuffNames.BeingBioDrained, Origin.FromSpell);
			Actor.Motives.ChangeValue(CommodityKind.Fun, MagicWand.CastGoodLuckCharm.kMotiveDrain);
			if (Target.SimDescription.YoungAdultOrAbove)
			{
				Target.PlaySoloAnimation("a_die_electrocution_x");
				PassionCommon.Wait(15u);
				Target.PlaySoloAnimation("a_sleeponFloor_getUp_x");
			}
			else if (Target.SimDescription.Teen)
			{
				Target.PlaySoloAnimation("t_die_electrocution_x");
				PassionCommon.Wait(15u);
				Target.PlaySoloAnimation("t_sleeponFloor_getUp_x");
			}
			Target.BuffManager.AddElement(BuffNames.BeingBioDrained, Origin.FromSpell);
			Target.BuffManager.RemoveElement(BuffNames.FrozenSolid);
			Target.BuffManager.RemoveElement(BuffNames.SoCold);
			Target.Motives.EnergyFromFullToSleepy();
			Target.Motives.SetValue(CommodityKind.Energy, -90f);
			EventTracker.SendEvent(EventTypeId.kSimFellAsleep, Target, null);
		}

		public override void OnSpellEpicFailure()
		{
		}

		public override void ShowEpicFailVfx(StateMachineClient sender, IEvent evt)
		{
			base.ShowEpicFailVfx(sender, evt);
			Actor.BuffManager.AddElement(BuffNames.BeingBioDrained, Origin.FromSpell);
		}
	}
}
