using System.Text;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Alchemy;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
	public class CastSexCharm : MagicWand.CastSpell
	{
		public class Definition : InteractionDefinition<Sim, Sim, CastSexCharm>, IOverridesVisualType, IHasTraitIcon, IHasMenuPathIcon
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
				return new string[1] { MagicWand.LocalizeString(isFemale, "CastCharm") + Localization.Ellipsis };
			}

			protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString("Sex Spell");
			}

			protected override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (target.SimDescription.IsBonehilda)
				{
					return false;
				}
				if (target.SimDescription.IsPregnant)
				{
					return false;
				}
				if (target.SimDescription.IsEP11Bot)
				{
					return false;
				}
				if (target.SimDescription.ChildOrBelow)
				{
					return false;
				}
				if (a.Posture.Satisfies(CommodityKind.SwimmingInPool, null) || target.Posture.Satisfies(CommodityKind.SwimmingInPool, null))
				{
					return false;
				}
				if (target.SimDescription.IsServicePerson)
				{
					return true;
				}
				return a.SkillManager.GetSkillLevel(SkillNames.Spellcasting) >= MagicWand.kSpellLevels[1] && MagicWand.CastSpell.CommonSpellTests(a, target, isAutonomous, ref greyedOutTooltipCallback);
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
		public static float kMotiveDrain = 5f;

		public static InteractionDefinition Singleton = new Definition();

		public override MagicWand.SpellType TypeOfSpell
		{
			protected get
			{
				return MagicWand.SpellType.GoodLuckCharm;
			}
		}

		public override string JazzStateName
		{
			protected get
			{
				return "Love Charm";
			}
		}

		public override string SuccessVfxName
		{
			protected get
			{
				if (Actor == Target)
				{
					return "ep7WandSpellLoveSelf_main";
				}
				return "ep7WandSpellLove_main";
			}
		}

		public override string EpicFailVfxName
		{
			protected get
			{
				return "ep7WandSpellLoveFail_main";
			}
		}

		public override string HitVfxName
		{
			protected get
			{
				return "ep7WandSpellLoveHit_main";
			}
		}

		protected override void DrainMotives()
		{
			mWand.DrainMotive(Actor, CommodityKind.MagicFatigue, 0f - MagicWand.CastLoveCharm.kMotiveDrain);
		}

		protected override void OnSpellSuccess()
		{
			Target.BuffManager.AddElement(BuffNames.Excited, Origin.FromSpell);
			EventTracker.SendEvent(EventTypeId.kCastCharm, Actor, Target);
			SpellcastingSkill spellcastingSkill = Actor.SkillManager.GetElement(SkillNames.Spellcasting) as SpellcastingSkill;
			if (spellcastingSkill == null)
			{
				return;
			}
			spellcastingSkill.UsedLightMagicSpell(Actor, Target);
			Passion.Player player = Passion.GetPlayer(Target);
			Passion.Player player2 = Passion.GetPlayer(Actor);
			Passion.Settings.AutonomyNotify = true;
			Target.InteractionQueue.AddAfterCheckingForDuplicates(Passion.Interactions.UseSimForPassion.Singleton.CreateInstance(Actor, Target, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
			Target.BuffManager.AddElement(BuffNames.Obsessed, Origin.FromSpell);
			Target.BuffManager.RemoveElement(BuffNames.ImminentNemesis);
			if (Passion.Settings.AutonomyChance <= 0 || !Passion.Settings.AutonomyNotify)
			{
				return;
			}
			player.IsAutonomous = true;
			player2.IsAutonomous = true;
			if (Target != Actor)
			{
				try
				{
					StringBuilder stringBuilder = new StringBuilder(PassionCommon.Localize("S3_Passion.Terms.AutonomyNotifySimMessage"));
					stringBuilder.Replace("[player]", player.Name);
					stringBuilder.Replace("[label]", Passion.Settings.ActiveLabel.ToLower());
					stringBuilder.Replace("[partner]", player2.Name);
					stringBuilder.Replace("[address]", player.Actor.LotCurrent.Name);
					PassionCommon.SimMessage(stringBuilder.ToString(), player.Actor, player2.Actor);
				}
				catch
				{
				}
			}
		}

		protected override void OnSpellEpicFailure()
		{
			Actor.BuffManager.AddElement(BuffNames.ImminentNemesis, Origin.FromSpell);
		}
	}
}
