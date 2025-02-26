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
    public class VampireMakePeeThemselves : SocialInteraction
    {
        [DoesntRequireTuning]
        public sealed class Definition : InteractionDefinition<Sim, Sim, VampireMakePeeThemselves>, IHasTraitIcon,
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
                return Localization.LocalizeString(target.IsFemale, "Vampire Make Sim Pee Themselves",
                    target.SimDescription, actor.SimDescription);
            }

            public override string[] GetPath(bool isFemale)
            {
                return new string[] { Localization.LocalizeString(isFemale, "Vampire...") };
            }

            public override bool Test(Sim a, Sim target, bool isAutonomous,
                ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return GameUtils.IsInstalled(ProductVersion.EP7) && target != a && !target.IsActiveSim &&
                       !target.SimDescription.IsServicePerson &&
                       !target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) &&
                       !target.SimDescription.IsGhost && !target.SimDescription.ChildOrBelow &&
                       !target.SimDescription.IsPet && a.SimDescription.IsVampire && !a.SimDescription.TeenOrBelow &&
                       !a.Posture.Satisfies(CommodityKind.SwimmingInPool, null) &&
                       !target.TraitManager.HasElement(TraitNames.SuperVampire) &&
                       PassionCore.Settings.VampireInteractions;
            }
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        public static ResourceKey VampireIconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

        public override bool Run()
        {
            //Definition definition = base.InteractionDefinition as Definition;
            Actor.SynchronizationLevel = Sim.SyncLevel.NotStarted;
            Target.SynchronizationLevel = Sim.SyncLevel.NotStarted;
            Target.InteractionQueue.CancelAllInteractions();
            if (!BeginSocialInteraction(new SocialInteractionB.Definition(), false, 1f, true)) return false;
            Actor.RouteTurnToFace(Target.Position);
            Target.RouteTurnToFace(Actor.Position);
            Target.LookAtManager.DisableLookAts();
            Actor.LookAtManager.DisableLookAts();
            StandardEntry();
            BeginCommodityUpdates();
            StartSocialContext();
            AcquireStateMachine("intimidate");
            EnterStateMachine("intimidate", "Enter", "x", "y");
            SetActor("x", Actor);
            SetActor("y", Target);
            Target.Motives.SetValue(CommodityKind.Bladder, -200f);
            AnimateJoinSims("Enter");
            AnimateJoinSims("Afraid");
            FinishLinkedInteraction(true);
            EndCommodityUpdates(true);
            WaitForSyncComplete(0.5f);
            StandardExit();
            Target.BuffManager.AddElement(BuffNames.Terrified, Origin.FromUnknown);
            Target.BuffManager.AddElement(BuffNames.WalkOfShame, Origin.FromUnknown);
            return true;
        }

        public override ThumbnailKey GetIconKey()
        {
            return new ThumbnailKey(VampireIconResourceKey, ThumbnailSize.Large);
        }
    }
}