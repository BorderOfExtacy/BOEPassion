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
	public class CastEnsorcelCharm : MagicWand.CastSpell
	{
		public class Definition : InteractionDefinition<Sim, Sim, CastEnsorcelCharm>, IOverridesVisualType, IHasTraitIcon, IHasMenuPathIcon
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
				return new string[1] { MagicWand.LocalizeString(isFemale, "CastCharm", new object[0]) + Localization.Ellipsis };
			}

			public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString("Ensorcel Spell");
			}

			public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (target.SimDescription.IsEP11Bot)
				{
					return false;
				}
				if (target.SimDescription.IsBonehilda)
				{
					return false;
				}
				if (target.Household == a.Household)
				{
					return false;
				}
				if (target.BuffManager.HasElement(BuffNames.WeddingDay))
				{
					return false;
				}
				if (target.SimDescription.IsPregnant)
				{
					return false;
				}
				if (target.BuffManager.HasElement(BuffNames.Ensorcelled))
				{
					return false;
				}
				if (target.Posture.Satisfies(CommodityKind.SwimmingInPool, null))
				{
					return false;
				}
				if (a.Posture.Satisfies(CommodityKind.SwimmingInPool, null))
				{
					return false;
				}
				if (target.SimDescription.ChildOrBelow)
				{
					return false;
				}
				if (target.SimDescription.IsServicePerson)
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
				if (Actor == Target)
				{
					return "ep7WandSpellSunSelf_main";
				}
				return "ep7WandSpellSun_main";
			}
		}

		public override string EpicFailVfxName
		{
			get
			{
				return "ep7WandSpellSunFail_main";
			}
		}

		public override string HitVfxName
		{
			get
			{
				return "ep7WandSpellSunHit_main";
			}
		}

		public override void DrainMotives()
		{
			mWand.DrainMotive(Actor, CommodityKind.MagicFatigue, 0f - MagicWand.CastGoodLuckCharm.kMotiveDrain);
		}

		public override void OnSpellSuccess()
		{
			Target.BuffManager.AddElement(BuffNames.Excited, Origin.FromSpell);
			EventTracker.SendEvent(EventTypeId.kCastCharm, Actor, Target);
			SpellcastingSkill spellcastingSkill = Actor.SkillManager.GetElement(SkillNames.Spellcasting) as SpellcastingSkill;
			if (spellcastingSkill != null)
			{
				spellcastingSkill.UsedLightMagicSpell(Actor, Target);
				EventTracker.SendEvent(EventTypeId.kEnsorcelSim, Actor, Target);
				EndCommodityUpdates(true);
				Target.BuffManager.AddElement(BuffNames.Ensorcelled, Origin.None);
			}
		}

		public override void OnSpellEpicFailure()
		{
			Actor.BuffManager.AddElement(BuffNames.ImminentNemesis, Origin.FromSpell);
		}
	}
}
