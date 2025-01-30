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
	public class CastUglyCharm : MagicWand.CastSpell
	{
		public class Definition : InteractionDefinition<Sim, Sim, CastUglyCharm>, IOverridesVisualType, IHasTraitIcon, IHasMenuPathIcon
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
				return Localization.LocalizeString("Make Ugly Spell");
			}

			protected override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (target.SimDescription.IsEP11Bot)
				{
					return false;
				}
				if (target.SimDescription.IsBonehilda)
				{
					return false;
				}
				if (target.BuffManager.HasElement(BuffNames.InnerBeauty))
				{
					return false;
				}
				if (target.Posture.Satisfies(CommodityKind.SwimmingInPool, null))
				{
					return false;
				}
				if (target.SimDescription.ChildOrBelow)
				{
					return false;
				}
				if (target == a)
				{
					return false;
				}
				return a.SkillManager.GetSkillLevel(SkillNames.Spellcasting) >= MagicWand.kSpellLevels[10] && MagicWand.CastSpell.CommonSpellTests(a, target, isAutonomous, ref greyedOutTooltipCallback);
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
		public static float kMotiveDrain = 40f;

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
				return "Good Luck";
			}
		}

		public override string SuccessVfxName
		{
			protected get
			{
				if (Actor == Target)
				{
					return "ep7WandSpellSunSelf_main";
				}
				return "ep7WandSpellSun_main";
			}
		}

		public override string EpicFailVfxName
		{
			protected get
			{
				return "ep7WandSpellSunFail_main";
			}
		}

		public override string HitVfxName
		{
			protected get
			{
				return "ep7WandSpellSunHit_main";
			}
		}

		protected override void DrainMotives()
		{
			mWand.DrainMotive(Actor, CommodityKind.MagicFatigue, 0f - MagicWand.CastGoodLuckCharm.kMotiveDrain);
		}

		protected override void OnSpellSuccess()
		{
			Target.BuffManager.AddElement(BuffNames.Excited, Origin.FromSpell);
			EventTracker.SendEvent(EventTypeId.kCastCharm, Actor, Target);
			SpellcastingSkill spellcastingSkill = Actor.SkillManager.GetElement(SkillNames.Spellcasting) as SpellcastingSkill;
			if (spellcastingSkill != null)
			{
				spellcastingSkill.UsedLightMagicSpell(Actor, Target);
				EndCommodityUpdates(true);
				Target.BuffManager.AddElement(BuffNames.InnerBeauty, Origin.None);
			}
		}

		protected override void OnSpellEpicFailure()
		{
			Actor.BuffManager.AddElement(BuffNames.ImminentNemesis, Origin.FromSpell);
		}
	}
}
