using System.Collections.Generic;
using Passion.S3_Passion.Utilities;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.Gameplay.Objects.Decorations.Mimics;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Services;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;
using Queries = Sims3.Gameplay.Queries;
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion.Interactions
{
    public class StripperPole : PassionCommon
    {
        internal class Dance : Interaction<Sim, SculptureFloorGunShow>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, SculptureFloorGunShow, Dance>
            {
                public override string GetInteractionName(Sim actor, SculptureFloorGunShow target,
                    InteractionObjectPair interaction)
                {
                    return Localize("S3_Passion.Terms.Dance").ToString();
                }

                public override bool Test(Sim actor, SculptureFloorGunShow target, bool isAutonomous,
                    ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!isAutonomous && actor != null && target != null && target.UseCount < 1 &&
                        actor.SimDescription.IsHuman && actor.SimDescription.TeenOrAbove)
                    {
                        return true;
                    }

                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                List<Stereo> list = new List<Stereo>(Queries.GetObjects<Stereo>(Actor.LotCurrent, Actor.RoomId));
                Stereo stereo = null;
                foreach (Stereo item in list)
                {
                    if (!item.TurnedOn) continue;
                    stereo = item;
                    break;
                }

                if (!Actor.RouteToObjectRadius(Target, 1f) || Target.UseCount >= 1 || stereo == null) return true;
                Vector3 position = new Vector3(Actor.Position);
                OutfitCategories currentOutfitCategory = Actor.CurrentOutfitCategory;
                int currentOutfitIndex = Actor.CurrentOutfitIndex;
                Target.ClearUseList();
                Target.AddToUseList(Actor);
                Actor.SetPosition(Target.Position);
                Actor.SetForward(Target.ForwardVector);
                int num = 2;
                WatchersHaveRouteToStripper = true;
                GetStripWatchers(Actor, Target);
                while (Actor.HasNoExitReason() &&
                       !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, this, false, Autonomous))
                {
                    _stripperIsOnPole = true;
                    Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(Animations));
                    if (num == 0)
                    {
                        switch (Actor.CurrentOutfitCategory)
                        {
                            case OutfitCategories.Everyday:
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
                                num = 1;
                                break;
                            case OutfitCategories.Swimwear:
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Sleepwear, 0);
                                num = 0;
                                break;
                            case OutfitCategories.Sleepwear:
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
                                break;
                            default:
                            {
                                if (Actor.CurrentOutfitCategory != OutfitCategories.Naked)
                                {
                                    Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
                                    num = 2;
                                }

                                break;
                            }
                        }
                    }

                    if (num > 0)
                    {
                        num--;
                    }

                    Libido.PartialSatisfaction(Actor);
                    Wait(2);
                }

                Target.RemoveFromUseList(Actor);
                Actor.SetPosition(position);
                _stripperIsOnPole = false;
                WatchersHaveRouteToStripper = false;
                Actor.SwitchToOutfitWithoutSpin(currentOutfitCategory, currentOutfitIndex);
                return true;
            }
        }

        internal class Dance2 : Interaction<Sim, SculptureFloorGunShow>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, SculptureFloorGunShow, Dance2>
            {
                public override string GetInteractionName(Sim actor, SculptureFloorGunShow target,
                    InteractionObjectPair interaction)
                {
                    return Localize("S3_Passion.Terms.Dance2").ToString();
                }

                public override bool Test(Sim actor, SculptureFloorGunShow target, bool isAutonomous,
                    ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return !isAutonomous && actor != null && target != null && target.UseCount < 1 &&
                           actor.SimDescription.IsHuman && actor.SimDescription.TeenOrAbove;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                List<Stereo> list = new List<Stereo>(Queries.GetObjects<Stereo>(Actor.LotCurrent, Actor.RoomId));
                Stereo stereo = null;
                foreach (Stereo item in list)
                {
                    if (!item.TurnedOn) continue;
                    stereo = item;
                    break;
                }

                if (!Actor.RouteToObjectRadius(Target, 1f) || Target.UseCount >= 1 || stereo == null) return true;
                Vector3 position = new Vector3(Actor.Position);
                OutfitCategories currentOutfitCategory = Actor.CurrentOutfitCategory;
                int currentOutfitIndex = Actor.CurrentOutfitIndex;
                Target.ClearUseList();
                Target.AddToUseList(Actor);
                float x = Target.Position.x - (Target.ForwardVector.z * 0f + Target.ForwardVector.x * 0.795f);
                float y = Target.Position.y;
                float z = Target.Position.z - (Target.ForwardVector.z * 0f - Target.ForwardVector.z * -0.795f);
                Actor.SetForward(Target.ForwardVector);
                Vector3 position2 = new Vector3(x, y, z);
                Actor.SetPosition(position2);
                int num = 3;
                WatchersHaveRouteToStripper = true;
                GetStripWatchers(Actor, Target);
                while (Actor.HasNoExitReason() &&
                       !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, this, false, Autonomous))
                {
                    _stripperIsOnPole = true;
                    Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(Animations2));
                    if (num == 0)
                    {
                        switch (Actor.CurrentOutfitCategory)
                        {
                            case OutfitCategories.Everyday:
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
                                num = 2;
                                break;
                            case OutfitCategories.Swimwear:
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Sleepwear, 0);
                                num = 1;
                                break;
                            case OutfitCategories.Sleepwear:
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
                                break;
                            default:
                            {
                                if (Actor.CurrentOutfitCategory != OutfitCategories.Naked)
                                {
                                    Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
                                    num = 3;
                                }

                                break;
                            }
                        }
                    }

                    if (num > 0)
                    {
                        num--;
                    }

                    Libido.PartialSatisfaction(Actor);
                    Wait(2);
                }

                Target.RemoveFromUseList(Actor);
                Actor.SetPosition(position);
                _stripperIsOnPole = false;
                WatchersHaveRouteToStripper = false;
                Actor.SwitchToOutfitWithoutSpin(currentOutfitCategory, currentOutfitIndex);
                return true;
            }
        }

        public class AutoDance : Interaction<Sim, SculptureFloorGunShow>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, SculptureFloorGunShow, AutoDance>
            {
                public override string GetInteractionName(Sim actor, SculptureFloorGunShow target,
                    InteractionObjectPair interaction)
                {
                    return Localize("S3_Passion.Terms.Dance").ToString();
                }

                public override bool Test(Sim actor, SculptureFloorGunShow target, bool isAutonomous,
                    ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return !isAutonomous && actor != null && target != null && target.UseCount < 1 &&
                           actor.SimDescription.TeenOrAbove && actor.SimDescription.IsHuman;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                List<Stereo> list = new List<Stereo>(Queries.GetObjects<Stereo>(Actor.LotCurrent, Actor.RoomId));
                Stereo stereo = null;
                foreach (Stereo item in list)
                {
                    if (!item.TurnedOn) continue;
                    stereo = item;
                    break;
                }

                if (Actor.RouteToObjectRadius(Target, 1f) && Target.UseCount < 1 && stereo != null)
                {
                    Vector3 position = new Vector3(Actor.Position);
                    OutfitCategories currentOutfitCategory = Actor.CurrentOutfitCategory;
                    int currentOutfitIndex = Actor.CurrentOutfitIndex;
                    Target.ClearUseList();
                    Target.AddToUseList(Actor);
                    float x = Target.Position.x - (Target.ForwardVector.z * 0f + Target.ForwardVector.x * 0.795f);
                    float y = Target.Position.y;
                    float z = Target.Position.z - (Target.ForwardVector.z * 0f - Target.ForwardVector.z * -0.795f);
                    Actor.SetForward(Target.ForwardVector);
                    Vector3 position2 = new Vector3(x, y, z);
                    Actor.SetPosition(position2);
                    int num = 3;
                    WatchersHaveRouteToStripper = true;
                    GetStripWatchers(Actor, Target);
                    while (Actor.HasNoExitReason() &&
                           !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, this, false, Autonomous))
                    {
                        _stripperIsOnPole = true;
                        Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(Animations2));
                        if (num == 0)
                        {
                            if (Actor.CurrentOutfitCategory == OutfitCategories.Everyday)
                            {
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
                                num = 2;
                            }
                            else if (Actor.CurrentOutfitCategory == OutfitCategories.Swimwear)
                            {
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Sleepwear, 0);
                                num = 1;
                            }
                            else if (Actor.CurrentOutfitCategory == OutfitCategories.Sleepwear)
                            {
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
                                num = 2;
                            }
                            else if (Actor.CurrentOutfitCategory != OutfitCategories.Naked)
                            {
                                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
                                num = 3;
                            }
                            else if (Actor.CurrentOutfitCategory == OutfitCategories.Naked)
                            {
                                break;
                            }
                        }

                        if (num > 0)
                        {
                            num--;
                        }

                        Libido.PartialSatisfaction(Actor);
                        Wait(2u);
                    }

                    Target.RemoveFromUseList(Actor);
                    Actor.SetPosition(position);
                    _stripperIsOnPole = false;
                    WatchersHaveRouteToStripper = false;
                    if (Actor.SimDescription.IsServicePerson && !(Actor.Service is Butler) && !(Actor.Service is Maid))
                    {
                        Actor.SwitchToOutfitWithoutSpin(PassionCore.MyOutfit);
                    }
                    else
                    {
                        Actor.SwitchToOutfitWithoutSpin(currentOutfitCategory, currentOutfitIndex);
                    }
                }

                Sim actor = Actor;
                if (Actor.LotCurrent.CountObjects<DanceFloor>() != 0)
                {
                    Sim sim = Actor;
                    GameObject gameObject = Target;
                    List<DanceFloor> list2 =
                        new List<DanceFloor>(Queries.GetObjects<DanceFloor>(sim.LotCurrent, sim.RoomId));
                    if (!sim.SimDescription.IsServicePerson || sim.Service is Butler || sim.Service is Maid)
                    {
                        foreach (Sim sim2 in sim.LotCurrent.GetSims())
                        {
                            if (sim2.SimDescription.TeenOrAbove && sim2.SimDescription.IsHuman &&
                                !sim2.LotCurrent.IsWorldLot)
                            {
                                sim = sim2;
                            }

                            foreach (DanceFloor item2 in list2)
                            {
                                gameObject = item2;
                            }

                            if (GameUtils.IsInstalled(ProductVersion.EP3) && gameObject != null && sim != null &&
                                !sim.SimDescription.IsServicePerson && RandomUtil.CoinFlip())
                            {
                                sim.InteractionQueue.AddAfterCheckingForDuplicates(
                                    DanceFloor.Dance.Singleton.CreateInstance(gameObject, sim,
                                        new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                            }
                        }
                    }
                }

                Actor = actor;
                return true;
            }
        }

        internal sealed class AskToDanceOnPole : ImmediateInteraction<Sim, SculptureFloorGunShow>
        {
            [DoesntRequireTuning]
            private sealed class
                Definition : ImmediateInteractionDefinition<Sim, SculptureFloorGunShow, AskToDanceOnPole>
            {
                public override string GetInteractionName(Sim actor, SculptureFloorGunShow target,
                    InteractionObjectPair interaction)
                {
                    return Localize("S3_Passion.Terms.AskToDance").ToString();
                }

                public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters,
                    out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers,
                    out int numSelectableRows)
                {
                    numSelectableRows = 1;
                    PopulateSimPicker(ref parameters, out listObjs, out headers,
                        GetThePeople(parameters.Actor as Sim, parameters.Actor.LotCurrent), false);
                }

                public override bool Test(Sim actor, SculptureFloorGunShow target, bool isAutonomous,
                    ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return actor.SimDescription.TeenOrAbove && actor.SimDescription.IsHuman && target.UseCount == 0;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Sim sim = GetSelectedObject() as Sim;
                if (sim != null)
                    sim.InteractionQueue.AddNextIfPossibleAfterCheckingForDuplicates(
                        Dance.Singleton.CreateInstance(Target, sim,
                            new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                return true;
            }
        }

        internal sealed class AskToDanceOnPole2 : ImmediateInteraction<Sim, SculptureFloorGunShow>
        {
            [DoesntRequireTuning]
            private sealed class
                Definition : ImmediateInteractionDefinition<Sim, SculptureFloorGunShow, AskToDanceOnPole2>
            {
                public override string GetInteractionName(Sim actor, SculptureFloorGunShow target,
                    InteractionObjectPair interaction)
                {
                    return Localize("S3_Passion.Terms.AskToDance2").ToString();
                }

                public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters,
                    out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers,
                    out int numSelectableRows)
                {
                    numSelectableRows = 1;
                    PopulateSimPicker(ref parameters, out listObjs, out headers,
                        GetThePeople(parameters.Actor as Sim, parameters.Actor.LotCurrent), false);
                }

                public override bool Test(Sim actor, SculptureFloorGunShow target, bool isAutonomous,
                    ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return actor.SimDescription.TeenOrAbove && actor.SimDescription.IsHuman && target.UseCount == 0;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Sim sim = GetSelectedObject() as Sim;
                if (sim != null)
                    sim.InteractionQueue.AddNextIfPossibleAfterCheckingForDuplicates(
                        Dance2.Singleton.CreateInstance(Target, sim,
                            new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                return true;
            }
        }

        internal sealed class DanceOnPoleStop : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, DanceOnPoleStop>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return Localize("S3_Passion.Terms.StopDance").ToString();
                }

                public override bool Test(Sim actor, Sim target, bool isAutonomous,
                    ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return target.InteractionQueue.GetCurrentInteraction() != null &&
                           (target.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Dance.Singleton ||
                            target.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Dance2.Singleton);
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Target.InteractionQueue.CancelInteraction(Target.InteractionQueue.GetCurrentInteraction().Id,
                    ExitReason.UserCanceled);
                _stripperIsOnPole = false;
                return true;
            }
        }

        internal sealed class Strip : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, Strip>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return Localize("S3_Passion.Terms.Strip").ToString();
                }

                public override bool Test(Sim actor, Sim target, bool isAutonomous,
                    ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    try
                    {
                        if (!isAutonomous && actor == target && actor.SimDescription.IsHuman &&
                            actor.CurrentInteraction != null &&
                            actor.CurrentInteraction.InteractionDefinition == Dance.Singleton)
                        {
                            return true;
                        }

                        if (!isAutonomous && actor == target && actor.SimDescription.IsHuman &&
                            actor.CurrentInteraction != null &&
                            actor.CurrentInteraction.InteractionDefinition == Dance2.Singleton)
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked);
                return true;
            }
        }

        public class WatchStrip : Interaction<Sim, SculptureFloorGunShow>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, SculptureFloorGunShow, WatchStrip>
            {
                public override string GetInteractionName(Sim actor, SculptureFloorGunShow target,
                    InteractionObjectPair interaction)
                {
                    return Localize("S3_Passion.Terms.Watch") + " " + Localize("S3_Passion.Terms.Strip");
                }

                public override bool Test(Sim actor, SculptureFloorGunShow target, bool isAutonomous,
                    ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return !isAutonomous && actor != null && actor != Sim.ActiveActor && target != null &&
                           target.UseCount > 0 && !actor.SimDescription.IsServicePerson && actor.SimDescription.IsHuman;
                }
            }

            private const float KMinRouteDistance = 1f;

            private const float KMaxRouteDistance = 5f;

            private const float KMinTimeBetweenReaction = 4f;

            private const float KMaxTimeBetweenReaction = 5f;

            private float _mTimeSinceLastReaction = 1f;

            private float _mTimeBetweenReaction;

            public WatchStrip MWatchedInteraction;

            [Persistable(false)] private readonly List<ReactionTypes> _mReactions = new List<ReactionTypes>(
                new ReactionTypes[]
                {
                    ReactionTypes.Cheer,
                    ReactionTypes.PumpFist,
                    ReactionTypes.Giggle,
                    ReactionTypes.Fascinated
                });

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                List<Stereo> list = new List<Stereo>(Queries.GetObjects<Stereo>(Actor.LotCurrent, Actor.RoomId));
                Stereo stereo = null;
                foreach (Stereo item in list)
                {
                    if (!item.TurnedOn) continue;
                    stereo = item;
                    break;
                }

                Sim sim = ((Target.ActorsUsingMe.Count > 0) ? Target.ActorsUsingMe[0] : null);
                if (sim == null || stereo == null) return false;
                PassionCore.GetPlayer(Actor).Watch(sim);
                Route route = Actor.CreateRoute();
                route.PlanToPointRadialRange(Target.Position, KMinRouteDistance, KMaxRouteDistance,
                    RouteDistancePreference.NoPreference, RouteOrientationPreference.TowardsObject,
                    Target.LotCurrent.LotId, new int[1] { Target.RoomId });
                if (!Actor.DoRoute(route) ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.UseObjectForPassion.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.UseSimForPassion.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.ActiveJoinPassion.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.JoinPassion.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.AskToJoinPassion.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.AskToSoloPassion.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.AskToPassionOther.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.AskToWatchPassion.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.WatchPassion.Singleton ||
                    Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition ==
                    PassionCore.Interactions.WatchMasturbate.Singleton)
                {
                    return false;
                }

                BeginCommodityUpdates();
                Actor.LoopIdle();
                bool flag = DoLoop(ExitReason.Default, WatchLoop, mCurrentStateMachine);
                EndCommodityUpdates(flag);
                return flag;
            }

            private void WatchLoop(StateMachineClient smc, LoopData ld)
            {
                if (Actor.HasExitReason(ExitReason.Canceled) || !_stripperIsOnPole)
                {
                    Actor.AddExitReason(ExitReason.Finished);
                    return;
                }

                _mTimeSinceLastReaction += ld.mDeltaTime;
                if (!(_mTimeBetweenReaction < _mTimeSinceLastReaction)) return;
                ReactionTypes randomObjectFromList = RandomUtil.GetRandomObjectFromList(_mReactions);
                Actor.PlayReaction(randomObjectFromList, ReactionSpeed.ImmediateWithoutOverlay);
                _mTimeSinceLastReaction = 0f;
                _mTimeBetweenReaction = RandomUtil.GetFloat(KMinTimeBetweenReaction, KMaxTimeBetweenReaction);
            }
        }

        private static bool _stripperIsOnPole;

        protected static bool WatchersHaveRouteToStripper;

        private static readonly List<string> Animations;

        private static readonly List<string> Animations2;

        private static List<Sim> GetThePeople(Sim actor, Lot targetLot)
        {
            List<Sim> list = new List<Sim>();
            foreach (Sim sim in targetLot.GetSims())
            {
                if (actor != sim && sim.SimDescription.TeenOrAbove && sim.SimDescription.IsHuman &&
                    !sim.LotCurrent.IsWorldLot)
                {
                    list.Add(sim);
                }
            }

            return list;
        }

        private static void GetStripWatchers(Sim actor, GameObject pole)
        {
            foreach (Sim sim in actor.LotCurrent.GetSims())
            {
                if (sim != actor && !sim.SimDescription.IsServicePerson)
                {
                    sim.InteractionQueue.AddAfterCheckingForDuplicates(WatchStrip.Singleton.CreateInstance(pole, sim,
                        new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                }
            }
        }

        static StripperPole()
        {
            List<string> list = new List<string>();
            list.Add("a_mmd_doax2_poledance_1");
            list.Add("a_mmd_doax2_poledance_2");
            Animations = list;
            List<string> list2 = new List<string>();
            list2.Add("a2o_DancePole_Practice_Start_x");
            list2.Add("a2o_DancePole_Practice_Loop_1_x");
            list2.Add("a2o_DancePole_Practice_Loop_1_x");
            list2.Add("a2o_DancePole_Practice_Loop_2_x");
            list2.Add("a2o_DancePole_Practice_Loop_2_x");
            list2.Add("a2o_DancePole_Practice_Stop_x");
            Animations2 = list2;
        }
    }
}