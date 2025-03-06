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

namespace Passion.S3_Passion.Interactions
{
    public class CastUglyCharm : MagicWand.CastSpell
    {
        private const string TraitIconKey = "trait_SpellcastingTalent_s";
        private const string InteractionName = "Make Ugly Spell";
        private const string SuccessVfxSelf = "ep7WandSpellSunSelf_main";
        private const string SuccessVfx = "ep7WandSpellSun_main";
        private const string EpicFailVfx = "ep7WandSpellSunFail_main";
        private const string HitVfx = "ep7WandSpellSunHit_main";

        private class Definition : InteractionDefinition<Sim, Sim, CastUglyCharm>, IOverridesVisualType, IHasTraitIcon,
            IHasMenuPathIcon
        {
            public InteractionVisualTypes GetVisualType
            {
                get { return InteractionVisualTypes.Trait; }
            }

            public override string[] GetPath(bool isFemale)
            {
                return new string[]
                    { MagicWand.LocalizeString(isFemale, "CastCharm", new object[0]) + Localization.Ellipsis };
            }

            public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
            {
                return Localization.LocalizeString(InteractionName);
            }

            public override bool Test(Sim a, Sim target, bool isAutonomous,
                ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return IsValidTarget(a, target, ref greyedOutTooltipCallback) &&
                       a.SkillManager.GetSkillLevel(SkillNames.Spellcasting) >= MagicWand.kSpellLevels[10];
            }

            private bool IsValidTarget(Sim a, Sim target, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                if (target.SimDescription.IsEP11Bot || target.SimDescription.IsBonehilda ||
                    target.BuffManager.HasElement(BuffNames.InnerBeauty) ||
                    target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) ||
                    target.SimDescription.ChildOrBelow || target == a)
                {
                    return false;
                }

                return a.SkillManager.GetSkillLevel(SkillNames.Spellcasting) >= MagicWand.kSpellLevels[10] &&
                       MagicWand.CastSpell.CommonSpellTests(a, target, false, ref greyedOutTooltipCallback);
            }

            public ResourceKey GetTraitIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey(TraitIconKey,
                    ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7));
            }

            public ResourceKey GetPathIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey(TraitIconKey,
                    ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7));
            }
        }

        [Tunable] [TunableComment("Amount that this spell drains from motives.  Range: 0-200")]
        private const float KMotiveDrain = 40f;

        public static InteractionDefinition Singleton = new Definition();

        public override MagicWand.SpellType TypeOfSpell
        {
            get { return MagicWand.SpellType.GoodLuckCharm; }
        }

        public override string JazzStateName
        {
            get { return "Good Luck"; }
        }

        public override string SuccessVfxName
        {
            get { return Actor == Target ? SuccessVfxSelf : SuccessVfx; }
        }

        public override string EpicFailVfxName
        {
            get { return EpicFailVfx; }
        }

        public override string HitVfxName
        {
            get { return HitVfx; }
        }

        public override void DrainMotives()
        {
            mWand.DrainMotive(Actor, CommodityKind.MagicFatigue, 0f - KMotiveDrain);
        }

        public override void OnSpellSuccess()
        {
            ApplySuccessEffects();
        }

        private void ApplySuccessEffects()
        {
            Target.BuffManager.AddElement(BuffNames.Excited, Origin.FromSpell);
            EventTracker.SendEvent(EventTypeId.kCastCharm, Actor, Target);
            UpdateSpellcastingSkill();
            Target.BuffManager.AddElement(BuffNames.InnerBeauty, Origin.None);
        }

        private void UpdateSpellcastingSkill()
        {
            SpellcastingSkill spellcastingSkill =
                Actor.SkillManager.GetElement(SkillNames.Spellcasting) as SpellcastingSkill;
            if (spellcastingSkill != null)
            {
                spellcastingSkill.UsedLightMagicSpell(Actor, Target);
                EndCommodityUpdates(true);
            }
        }

        public override void OnSpellEpicFailure()
        {
            Actor.BuffManager.AddElement(BuffNames.ImminentNemesis, Origin.FromSpell);
        }
    }
}