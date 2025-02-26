using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CelebritySystem;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion.Interactions
{
    public class VampireAttack : SocialInteraction
    {
        [DoesntRequireTuning]
        private sealed class Definition : InteractionDefinition<Sim, Sim, VampireAttack>, IHasTraitIcon, IHasMenuPathIcon
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
                return Localization.LocalizeString(target.IsFemale, "Vampire Attack", target.SimDescription,
                    actor.SimDescription);
            }

            public override string[] GetPath(bool isFemale)
            {
                return new string[] { Localization.LocalizeString(isFemale, "Vampire...") };
            }

            public override bool Test(Sim a, Sim target, bool isAutonomous,
                ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return GameUtils.IsInstalled(ProductVersion.EP7) && target != a && !target.IsActiveSim &&
                       !target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) &&
                       !target.SimDescription.IsGhost && !target.SimDescription.ChildOrBelow &&
                       !target.SimDescription.IsPet && a.SimDescription.IsVampire && !a.SimDescription.TeenOrBelow &&
                       a.BuffManager.HasElement(BuffNames.VampiricVigor) &&
                       !a.Posture.Satisfies(CommodityKind.SwimmingInPool, null) &&
                       PassionCore.Settings.VampireInteractions;
            }
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        private static readonly ResourceKey VampireIconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

        public override bool Run()
        {
            //Definition definition = InteractionDefinition as Definition;
            Actor.SynchronizationLevel = Sim.SyncLevel.NotStarted;
            Target.SynchronizationLevel = Sim.SyncLevel.NotStarted;
            Target.InteractionQueue.CancelAllInteractions();
            if (BeginSocialInteraction(new SocialInteractionB.Definition(), false, 1f, true))
            {
                Actor.RouteTurnToFace(Target.Position);
                Target.RouteTurnToFace(Actor.Position);
                Target.LookAtManager.DisableLookAts();
                Actor.LookAtManager.DisableLookAts();
                StandardEntry();
                BeginCommodityUpdates();
                EnterStateMachine("PracticeFight", "Enter", "x");
                SetActor("y", Target);
                AnimateSim("Fight");
                AnimateSim("Exit");
                FinishLinkedInteraction(true);
                EndCommodityUpdates(true);
                WaitForSyncComplete(0.5f);
                StandardExit();
            }

            Actor.InteractionQueue.PushAsContinuation(
                VampireAttacked.Singleton.CreateInstance(Target, Actor,
                    new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
            Cleanup();
            return true;
        }

        public static void VampVictBuffs(Sim vamp, Sim vic)
        {
            vic.BuffManager.AddElement(BuffNames.Weakened, Origin.FromProvidingVampireNutrients);
            vic.BuffManager.AddElement(BuffNames.Bitten, Origin.FromProvidingVampireNutrients);
        }

        public static void VampireDrinkSuccess(Sim actor)
        {
            if (actor.Motives.IsFullEnoughForStuffedBuff())
            {
                actor.BuffManager.AddElement(BuffNames.Stuffed, Origin.FromCarnivorousBehavior);
            }

            SetMaxMotive(actor, CommodityKind.VampireThirst);
            if (actor.LotCurrent.IsCommunityLot)
            {
                DisgracefulActionEvent e = new DisgracefulActionEvent(EventTypeId.kSimCommittedDisgracefulAction, actor,
                    DisgracefulActionType.BiteSomeoneInPublic);
                EventTracker.SendEvent(e);
            }

            if (actor.TraitManager.HasElement(TraitNames.Vegetarian))
            {
                actor.BuffManager.AddElement(BuffNames.Nauseous, Origin.FromCarnivorousBehavior);
            }
            else
            {
                actor.BuffManager.AddElement(BuffNames.Sated, Origin.FromReceivingVampireNutrients);
            }
        }

        private static void SetMaxMotive(Sim actor, CommodityKind type)
        {
            actor.Motives.SetValue(type, 200f);
        }

        public override ThumbnailKey GetIconKey()
        {
            return new ThumbnailKey(VampireIconResourceKey, ThumbnailSize.Large);
        }
    }
}