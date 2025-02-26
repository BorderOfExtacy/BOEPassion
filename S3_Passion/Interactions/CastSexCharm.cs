using System.Text;
using Passion.S3_Passion.Utilities;
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
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion.Interactions
{
    public class CastSexCharm : MagicWand.CastSpell
    {
        private class Definition : InteractionDefinition<Sim, Sim, CastSexCharm>, IOverridesVisualType, IHasTraitIcon,
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
                return Localization.LocalizeString("Sex Spell");
            }

            public override bool Test(Sim a, Sim target, bool isAutonomous,
                ref GreyedOutTooltipCallback greyedOutTooltipCallback)
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

                if (a.Posture.Satisfies(CommodityKind.SwimmingInPool, null) ||
                    target.Posture.Satisfies(CommodityKind.SwimmingInPool, null))
                {
                    return false;
                }

                if (target.SimDescription.IsServicePerson)
                {
                    return true;
                }

                return a.SkillManager.GetSkillLevel(SkillNames.Spellcasting) >= MagicWand.kSpellLevels[1] &&
                       MagicWand.CastSpell.CommonSpellTests(a, target, isAutonomous, ref greyedOutTooltipCallback);
            }

            public ResourceKey GetTraitIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_SpellcastingTalent_s",
                    ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7));
            }

            public ResourceKey GetPathIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_SpellcastingTalent_s",
                    ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7));
            }
        }

        [Tunable] [TunableComment("Amount that this spell drains from motives.  Range: 0-200")]
        public static float KMotiveDrain = 5f;

        public static InteractionDefinition Singleton = new Definition();

        public override MagicWand.SpellType TypeOfSpell
        {
            get { return MagicWand.SpellType.GoodLuckCharm; }
        }

        public override string JazzStateName
        {
            get { return "Love Charm"; }
        }

        public override string SuccessVfxName
        {
            get
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
            get { return "ep7WandSpellLoveFail_main"; }
        }

        public override string HitVfxName
        {
            get { return "ep7WandSpellLoveHit_main"; }
        }

        public override void DrainMotives()
        {
            mWand.DrainMotive(Actor, CommodityKind.MagicFatigue, 0f - MagicWand.CastLoveCharm.kMotiveDrain);
        }

        public override void OnSpellSuccess()
        {
            Target.BuffManager.AddElement(BuffNames.Excited, Origin.FromSpell);
            EventTracker.SendEvent(EventTypeId.kCastCharm, Actor, Target);
            SpellcastingSkill spellcastingSkill =
                Actor.SkillManager.GetElement(SkillNames.Spellcasting) as SpellcastingSkill;
            if (spellcastingSkill == null)
            {
                return;
            }

            spellcastingSkill.UsedLightMagicSpell(Actor, Target);
            PassionCore.Player player = PassionCore.GetPlayer(Target);
            PassionCore.Player player2 = PassionCore.GetPlayer(Actor);
            PassionCore.Settings.AutonomyNotify = true;
            Target.InteractionQueue.AddAfterCheckingForDuplicates(
                PassionCore.Interactions.UseSimForPassion.Singleton.CreateInstance(Actor, Target,
                    new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
            Target.BuffManager.AddElement(BuffNames.Obsessed, Origin.FromSpell);
            Target.BuffManager.RemoveElement(BuffNames.ImminentNemesis);
            if (PassionCore.Settings.AutonomyChance <= 0 || !PassionCore.Settings.AutonomyNotify)
            {
                return;
            }

            player.IsAutonomous = true;
            player2.IsAutonomous = true;
            if (Target != Actor)
            {
                try
                {
                    StringBuilder stringBuilder =
                        new StringBuilder(PassionCommon.Localize("S3_Passion.Terms.AutonomyNotifySimMessage"));
                    stringBuilder.Replace("[player]", player.Name);
                    stringBuilder.Replace("[label]", PassionCore.Settings.ActiveLabel.ToLower());
                    stringBuilder.Replace("[partner]", player2.Name);
                    stringBuilder.Replace("[address]", player.Actor.LotCurrent.Name);
                    PassionCommon.SimMessage(stringBuilder.ToString(), player.Actor, player2.Actor);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public override void OnSpellEpicFailure()
        {
            Actor.BuffManager.AddElement(BuffNames.ImminentNemesis, Origin.FromSpell);
        }
    }
}