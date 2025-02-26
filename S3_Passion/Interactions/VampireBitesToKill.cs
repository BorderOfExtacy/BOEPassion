using Passion.S3_Passion.Utilities;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.CelebritySystem;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI.Controller;
using Sims3.UI.Hud;
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion.Interactions
{
    public class VampireBitesToKill : SocialInteraction
    {
        [DoesntRequireTuning]
        public sealed class Definition : InteractionDefinition<Sim, Sim, VampireBitesToKill>, IHasTraitIcon,
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
                return Localization.LocalizeString(target.IsFemale, "Vampire Bites To Kill", target.SimDescription,
                    actor.SimDescription);
            }

            public override string[] GetPath(bool isFemale)
            {
                return new string[] { Localization.LocalizeString(isFemale, "Vampire...") };
            }

            public override bool Test(Sim a, Sim target, bool isAutonomous,
                ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return GameUtils.IsInstalled(ProductVersion.EP7) && target != a && !a.LotCurrent.IsWorldLot &&
                       !target.IsActiveSim && !target.SimDescription.IsServicePerson &&
                       !target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) &&
                       !target.SimDescription.IsGhost && !target.SimDescription.ChildOrBelow &&
                       !target.SimDescription.IsPet && a.SimDescription.IsVampire &&
                       a.TraitManager.HasElement(TraitNames.SuperVampire) && !a.SimDescription.ChildOrBelow &&
                       !a.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && PassionCore.Settings.VampireInteractions;
            }
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        public static ResourceKey VampireiconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

        public override bool Run()
        {
            bool flag = false;
            //Definition definition = base.InteractionDefinition as Definition;
            Actor.SynchronizationLevel = Sim.SyncLevel.NotStarted;
            Target.SynchronizationLevel = Sim.SyncLevel.NotStarted;
            Target.InteractionQueue.CancelAllInteractions();
            if (BeginSocialInteraction(new SocialInteractionB.Definition(), true, 0.75f, true))
            {
                Target.LookAtManager.DisableLookAts();
                Actor.LookAtManager.DisableLookAts();
                Actor.RouteTurnToFace(Target.Position);
                Target.RouteTurnToFace(Actor.Position);
                StandardEntry();
                BeginCommodityUpdates();
                StartSocialContext();
                AcquireStateMachine("Vampire");
                EnterStateMachine("Vampire", "Enter", "x", "y");
                SetActor("x", Actor);
                SetActor("y", Target);
                EnterSim("Join");
                AnimateJoinSims("Amorus Bite");
                FinishLinkedInteraction(true);
                EndCommodityUpdates(true);
                StandardExit();
                WaitForSyncComplete(2f);
                VampireDrinkSuccess(Actor);
                VampVictBuffs(Actor, Target);
                if (Target.SimDescription.IsFairy)
                {
                    Actor.BuffManager.AddElement(BuffNames.DrankFromAFairy, 1350f,
                        Origin.FromReceivingVampireNutrients);
                }

                EventTracker.SendEvent(EventTypeId.kVampireDrankFromSim, Actor, Target);
                EventTracker.SendEvent(new VampireLifetimeEvent(EventTypeId.kVampireLifetimeEvent, Actor.SimDescription,
                    false, Target.SimDescription.SimDescriptionId));
                PassionCommon.Wait(250);
                Sim actor = Actor;
                Lot lotCurrent = Actor.LotCurrent;
                if ((!Target.SimDescription.IsVampire &&
                     !Target.SimDescription.TraitManager.HasElement(TraitNames.Evil) &&
                     !Actor.SimDescription.TraitManager.HasElement(TraitNames.Evil)) ||
                    (!Target.SimDescription.IsWerewolf &&
                     !Target.SimDescription.TraitManager.HasElement(TraitNames.Evil) &&
                     !Actor.SimDescription.TraitManager.HasElement(TraitNames.Evil)))
                {
                    SetTraits(Actor);
                    if (actor != null)
                    {
                        foreach (Sim sim in lotCurrent.GetSims())
                        {
                            if (!lotCurrent.LotCurrent.IsWorldLot && sim.SimDescription != actor.SimDescription &&
                                !sim.SimDescription.IsPet && !sim.SimDescription.IsWildAnimal)
                            {
                                Relationship relationship =
                                    Relationship.Get(actor.SimDescription, sim.SimDescription, true);
                                if (relationship != null)
                                {
                                    relationship.MakeAcquaintances();
                                    Relationship relationship2 =
                                        Relationship.Get(actor.SimDescription, sim.SimDescription, true);
                                    relationship2.LTR.ForceChangeState(LongTermRelationshipTypes.Enemy);
                                }
                            }
                        }
                    }

                    Actor.BuffManager.RemoveElement(BuffNames.FeelingLucky);
                    Actor.BuffManager.RemoveElement(BuffNames.HeroOfTheCity);
                    Actor.BuffManager.AddElement(BuffNames.FearOfHuman, 1350f, Origin.FromBeingAroundNonFriends);
                    Actor.BuffManager.AddElement(BuffNames.FeelingUnlucky, 1350f, Origin.FromAlone);
                }
                else
                {
                    if (Actor.SimDescription.TraitManager.HasElement(TraitNames.Evil))
                    {
                        if (actor != null)
                        {
                            foreach (Sim sim2 in lotCurrent.GetSims())
                            {
                                if (!lotCurrent.LotCurrent.IsWorldLot && sim2.SimDescription != actor.SimDescription &&
                                    !sim2.SimDescription.IsPet && !sim2.SimDescription.IsWildAnimal)
                                {
                                    Relationship relationship3 = Relationship.Get(actor.SimDescription,
                                        sim2.SimDescription, true);
                                    if (relationship3 != null)
                                    {
                                        relationship3.MakeAcquaintances();
                                        Relationship relationship4 = Relationship.Get(actor.SimDescription,
                                            sim2.SimDescription, true);
                                        relationship4.LTR.ForceChangeState(LongTermRelationshipTypes.Enemy);
                                    }
                                }
                            }
                        }

                        Actor.BuffManager.RemoveElement(BuffNames.FeelingLucky);
                        Actor.BuffManager.RemoveElement(BuffNames.HeroOfTheCity);
                        Actor.BuffManager.AddElement(BuffNames.FearOfHuman, 1350f, Origin.FromBeingAroundNonFriends);
                        Actor.BuffManager.AddElement(BuffNames.FeelingUnlucky, 1350f, Origin.FromAlone);
                    }

                    if ((Target.SimDescription.IsVampire &&
                         Target.SimDescription.TraitManager.HasElement(TraitNames.Evil) &&
                         !Actor.SimDescription.TraitManager.HasElement(TraitNames.Evil)) ||
                        (Target.SimDescription.IsWerewolf &&
                         Target.SimDescription.TraitManager.HasElement(TraitNames.Evil) &&
                         !Actor.SimDescription.TraitManager.HasElement(TraitNames.Evil)))
                    {
                        if (actor != null)
                        {
                            foreach (Sim sim3 in lotCurrent.GetSims())
                            {
                                if (sim3.SimDescription != actor.SimDescription && sim3.SimDescription.IsHuman)
                                {
                                    Relationship relationship5 = Relationship.Get(actor.SimDescription,
                                        sim3.SimDescription, true);
                                    if (relationship5 != null)
                                    {
                                        relationship5.MakeAcquaintances();
                                        Relationship relationship6 = Relationship.Get(actor.SimDescription,
                                            sim3.SimDescription, true);
                                        relationship6.LTR.ForceChangeState(LongTermRelationshipTypes.BestFriend);
                                    }
                                }
                            }
                        }

                        if (Target.SimDescription.IsVampire)
                        {
                            Actor.BuffManager.AddElement(BuffNames.HeroOfTheCity, 1350f, Origin.FromVampire);
                        }
                        else if (Target.SimDescription.IsWerewolf)
                        {
                            Actor.BuffManager.AddElement(BuffNames.HeroOfTheCity, 1350f, Origin.FromWerewolfBite);
                        }
                    }
                }

                flag = true;
            }

            if (flag)
            {
                VampKillsVictim(Actor, Target);
            }

            Cleanup();
            return true;
        }

        public void SetTraits(Sim sim)
        {
            bool flag = sim.TraitManager.HasElement(TraitNames.SuperVampire);
            sim.TraitManager.RemoveAllElements();
            if (flag)
            {
                sim.TraitManager.AddElement(TraitNames.SuperVampire);
            }

            sim.TraitManager.AddElement(TraitNames.HotHeaded);
            sim.TraitManager.AddElement(TraitNames.Unlucky);
            sim.TraitManager.AddElement(TraitNames.Evil);
            sim.TraitManager.AddElement(TraitNames.Athletic);
            sim.TraitManager.AddElement(TraitNames.Charismatic);
            sim.TraitManager.AddElement(TraitNames.Loser);
            sim.TraitManager.AddHiddenElement(TraitNames.VampireHiddenTrait);
            if (sim.OccultManager.HasOccultType(OccultTypes.Fairy))
            {
                sim.TraitManager.AddHiddenElement(TraitNames.FairyHiddenTrait);
            }

            if (sim.OccultManager.HasOccultType(OccultTypes.Witch))
            {
                sim.TraitManager.AddHiddenElement(TraitNames.WitchHiddenTrait);
            }

            if (sim.OccultManager.HasOccultType(OccultTypes.Werewolf))
            {
                sim.TraitManager.AddHiddenElement(TraitNames.LycanthropyWerewolf);
                sim.TraitManager.AddHiddenElement(TraitNames.WerewolfAlphaDog);
            }

            if (sim.OccultManager.HasOccultType(OccultTypes.Genie))
            {
                sim.TraitManager.AddHiddenElement(TraitNames.GenieHiddenTrait);
            }

            if (!sim.IsSelectable)
            {
                Household.RoommateManager.MakeRoommateSelectable(sim.SimDescription);
            }
        }

        public override ThumbnailKey GetIconKey()
        {
            return new ThumbnailKey(VampireiconResourceKey, ThumbnailSize.Large);
        }

        public static void VampKillsVictim(Sim vamp, Sim vic)
        {
            vic.InteractionQueue.CancelAllInteractions();
            if (vic.BuffManager.HasElement(BuffNames.FeelingUnlucky))
            {
                vic.BuffManager.RemoveElement(BuffNames.FeelingUnlucky);
            }

            if (vic.TraitManager.HasElement(TraitNames.Unlucky))
            {
                vic.TraitManager.RemoveElement(TraitNames.Unlucky);
            }

            if (vic.SimDescription.IsVampire || vic.SimDescription.IsWerewolf)
            {
                vic.Kill(SimDescription.DeathType.MummyCurse);
            }
            else
            {
                vic.Kill(SimDescription.DeathType.Starve);
            }
        }

        public static void VampVictBuffs(Sim vamp, Sim vic)
        {
            vic.BuffManager.AddElement(BuffNames.Bitten, Origin.FromProvidingVampireNutrients);
            vic.BuffManager.AddElement(BuffNames.Weakened, Origin.FromProvidingVampireNutrients);
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

        public static void SetMaxMotive(Sim actor, CommodityKind type)
        {
            actor.Motives.SetValue(type, 200f);
        }
    }
}