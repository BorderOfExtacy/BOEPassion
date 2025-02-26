using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion.Interactions
{
    internal sealed class VampireReleaseMindControl : SocialInteraction
    {
        [DoesntRequireTuning]
        private sealed class Definition : InteractionDefinition<Sim, Sim, VampireReleaseMindControl>, IHasTraitIcon,
            IHasMenuPathIcon
        {
            public ResourceKey GetPathIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
            }

            public ResourceKey GetTraitIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
            }

            public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
            {
                return Localization.LocalizeString(target.IsFemale, "Release Mind Control", target.SimDescription,
                    actor.SimDescription);
            }

            public override string[] GetPath(bool bPath)
            {
                return new string[] { Localization.LocalizeString("Vampire...") };
            }

            public override bool Test(Sim actor, Sim target, bool isAutonomous,
                ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return actor.SimDescription.IsVampire && actor.TraitManager.HasElement(TraitNames.SuperVampire) &&
                       target.BuffManager.HasElement(BuffNames.Ensorcelled) &&
                       GameUtils.IsInstalled(ProductVersion.EP7) &&
                       PassionCore.Settings.VampireInteractions &&
                       !actor.Posture.Satisfies(CommodityKind.SwimmingInPool, null);
            }
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        private static readonly ResourceKey VampireIconResourceKey =
            new ResourceKey(11874435978993716202uL, 796721156u, 0u);

        public override ThumbnailKey GetIconKey()
        {
            return new ThumbnailKey(VampireIconResourceKey, ThumbnailSize.Large);
        }

        public override bool Run()
        {
            Actor.RouteTurnToFace(Target.Position);
            Target.BuffManager.RemoveElement(BuffNames.Ensorcelled);
            StandardEntry();
            BeginCommodityUpdates();
            Actor.PlaySoloAnimation("a_genie_clap_x", true, ProductVersion.EP6);
            EndCommodityUpdates(true);
            VisualEffect visualEffect = VisualEffect.Create("ep6geniespellenchantsparks");
            visualEffect.ParentTo(Target, Sim.FXJoints.Head);
            visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
            Target.BuffManager.AddElement(BuffNames.KnockedOut, Origin.FromUnfriendlyVampire);
            Cleanup();
            return true;
        }
    }
}