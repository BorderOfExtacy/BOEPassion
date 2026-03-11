using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Debug;
using S3_Passion.BOE_Lovemaking;
using S3_Passion.BOE_Objects;
using S3_Passion.BOE_Settings;
using S3_Passion.BOE_UI;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Pools;
using Sims3.Gameplay.Situations;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;
using Sims3.UI;

namespace S3_Passion.BOE_Interaction
{
    public class Interactions
    {
        internal sealed class UseSimForPassion : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, UseSimForPassion>, IHasTraitIcon, IHasMenuPathIcon
            {
                public ResourceKey GetTraitIcon(Sim actor, GameObject target)
                {
                    return Interactions.GetTraitIcon(actor, target);
                }

                public ResourceKey GetPathIcon(Sim actor, GameObject target)
                {
                    return Interactions.GetPathIcon(actor, target);
                }

                public override string[] GetPath(bool bPath)
                {
                    return RomancePath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(actor) && target != null)
                    {
                        Player player = PassionBase.GetPlayer(actor);
                        Player player2 = PassionBase.GetPlayer(target);
                        if (player.IsActive || player.IsInPool || player.IsWatching || player2.IsActive || player2.IsInPool || !PassionBase.IsValid(player2.Actor) || player.Actor != player2.Actor && !PersistableSettings.Settings.ActiveAlwaysAccepts && !player.WillPassion(player2))
                        {
                            return false;
                        }
                        return true;
                    }
                    return false;
                }

                public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
                {
                    Sim sim = parameters.Actor as Sim;
                    Sim sim2 = parameters.Target as Sim;
                    if (sim == sim2)
                    {
                        List<Sim> availablePartners = Player.GetAvailablePartners(parameters.Actor as Sim);
                        NumSelectableRows = availablePartners.Count;
                        PopulateSimPicker(ref parameters, out listObjs, out headers, availablePartners, false);
                    }
                    else
                    {
                        listObjs = null;
                        headers = null;
                        NumSelectableRows = 0;
                    }
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                Player player2 = PassionBase.GetPlayer(Target);
                Vector3 location = player2.Location;
                Vector3 forward = player2.Forward;
                if (player.IsValid)
                {
                    if (player2 != null && player2.IsValid)
                    {
                        try
                        {
                            GameObject[] objects = Sims3.Gameplay.Queries.GetObjects<GameObject>(player2.Location, 3f);
                            if (objects != null && objects.Length != 0)
                            {
                                GameObject[] array = objects;
                                foreach (GameObject gameObject in array)
                                {
                                    if (gameObject == null || gameObject is Sim)
                                    {
                                        continue;
                                    }
                                    PartData partData = gameObject.PartComponent != null ? gameObject.PartComponent.GetPartSimIsIn(player2.Actor) : null;
                                    if (partData != null || gameObject.IsActorUsingMe(player2.Actor))
                                    {
                                        location = player.Location;
                                        forward = player.Forward;
                                        if (gameObject is Bed || gameObject is Couch || gameObject is ChairLiving || gameObject is ChairLounge || gameObject is ChairSectional)
                                        {
                                            PassionTarget target = PassionBase.GetTarget(gameObject);
                                            if (target != null && target.HasObject && !target.IsOccupiedByOtherSims(player2.Actor))
                                            {
                                                if (!player.Route(player2, 5f))
                                                {
                                                    location = player.Location;
                                                    forward = player.Forward;
                                                    break;
                                                }
                                                Part part = target.ChooseSectionDialog();
                                                if (part != null)
                                                {
                                                    if (partData != null)
                                                    {
                                                        partData.SetContainedSim(null, target.Object.PartComponent);
                                                    }
                                                    if (target.Object.ActorsUsingMe.Contains(player2.Actor))
                                                    {
                                                        target.Object.RemoveFromUseList(player2.Actor);
                                                    }
                                                    try
                                                    {
                                                        AnimationUtil.StopAllAnimation(gameObject);
                                                    }
                                                    catch
                                                    {
                                                    }
                                                    part.Add(player);
                                                    part.Add(player2);
                                                    player2.SpinDisabled = true;
                                                    player2.ForceStartLoop();
                                                    player2.ExitPoint = player.Location;
                                                    player2.SpinDisabled = false;
                                                    return player.DirectStartLoop();
                                                }
                                                break;
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            location = player.Location;
                            forward = player.Forward;
                        }
                    }
                    if (player.Join(PassionBase.GetTarget(location, forward)))
                    {
                        if (player2.IsValid)
                        {
                            player.DirectTargeted = player2.Actor != player.Actor;
                            if (!player.DirectTargeted)
                            {
                                player.SetPartnersToCheck(GetSelectedObjectsAsSims());
                            }
                            else
                            {
                                List<Sim> list = new List<Sim>();
                                list.Add(player2.Actor);
                                player.SetPartnersToCheck(list);
                            }
                        }
                        if (player.PartnersToCheckCount > 0)
                        {
                            foreach (Player item in player.PartnersToCheck)
                            {
                                if (item != null)
                                {
                                    player.Actor.InteractionQueue.AddNext(AskToPassion.Singleton.CreateInstance(item.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                                }
                            }
                        }
                        else
                        {
                            player.StartLoop();
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        internal sealed class UseObjectForPassion : Interaction<Sim, IGameObject>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, IGameObject, UseObjectForPassion>, IHasTraitIcon, IHasMenuPathIcon
            {
                public ResourceKey GetTraitIcon(Sim actor, GameObject target)
                {
                    return Interactions.GetTraitIcon(actor, target);
                }

                public ResourceKey GetPathIcon(Sim actor, GameObject target)
                {
                    return Interactions.GetPathIcon(actor, target);
                }

                public override string[] GetPath(bool bPath)
                {
                    return RomancePath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PersistableSettings.Settings.SoloLabel;
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(actor) && target != null && (!(target is Sim) || actor == target))
                    {
                        Player player = PassionBase.GetPlayer(actor);
                        PassionTarget target2 = PassionBase.GetTarget(target);
                        if (!player.IsActive && !player.IsWatching && target2 != null && target2.MinSims == 1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                PassionTarget target = PassionBase.GetTarget(Target);
                PassionTarget target2 = target;
                if (target.ObjectType.Name.ToString() == "Rug")
                {
                    GameObject[] objects = Sims3.Gameplay.Queries.GetObjects<GameObject>(target.Location, 1f);
                    if (objects != null && objects.Length != 0)
                    {
                        GameObject[] array = objects;
                        foreach (GameObject gameObject in array)
                        {
                            if (gameObject == null || gameObject is Sim)
                            {
                                continue;
                            }
                            target2 = PassionBase.GetTarget(gameObject);
                            try
                            {
                                if (target2.ObjectType.Name.ToString() == "Couch" || target2.ObjectType.Name.ToString() == "Sofa" || target2.ObjectType.Name.ToString() == "Loveseat" || target2.ObjectType.Name.ToString().Contains("Chair") || target2.ObjectType.Name.ToString().Contains("Table") || target2.ObjectType.Name.ToString().Contains("Bed") || target2.ObjectType.Name.ToString() == "Altar")
                                {
                                    target = PassionBase.GetTarget(gameObject);
                                    break;
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                if (target.IsOccupied || target.IsOccupiedByOtherSims(Actor))
                {
                    player.PlayRouteFailure();
                    return false;
                }
                if (player.Join(target))
                {
                    if (Actor == Target)
                    {
                        player.StartLoop();
                    }
                    else
                    {
                        player.Actor.InteractionQueue.PushAsContinuation(RouteToPassion.Singleton.CreateInstance(Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
                    }
                    return true;
                }
                return false;
            }
        }

        internal sealed class UseObjectForPassionWithSim : Interaction<Sim, IGameObject>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, IGameObject, UseObjectForPassionWithSim>, IHasTraitIcon, IHasMenuPathIcon
            {
                public ResourceKey GetTraitIcon(Sim actor, GameObject target)
                {
                    return Interactions.GetTraitIcon(actor, target);
                }

                public ResourceKey GetPathIcon(Sim actor, GameObject target)
                {
                    return Interactions.GetPathIcon(actor, target);
                }

                public override string[] GetPath(bool bPath)
                {
                    return RomancePath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(actor))
                    {
                        Player player = PassionBase.GetPlayer(actor);
                        PassionTarget target2 = PassionBase.GetTarget(target);
                        if (player.IsActive || player.IsWatching || target2.MaxSims < 2)
                        {
                            return false;
                        }
                        return true;
                    }
                    return false;
                }

                public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
                {
                    List<Sim> availablePartners = Player.GetAvailablePartners(parameters.Actor as Sim);
                    NumSelectableRows = availablePartners.Count;
                    PopulateSimPicker(ref parameters, out listObjs, out headers, availablePartners, false);
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                PassionTarget target = PassionBase.GetTarget(Target);
                PassionTarget target2 = target;
                if (target.ObjectType.Name.ToString() == "Rug")
                {
                    GameObject[] objects = Sims3.Gameplay.Queries.GetObjects<GameObject>(target.Location, 1f);
                    if (objects != null && objects.Length != 0)
                    {
                        GameObject[] array = objects;
                        foreach (GameObject gameObject in array)
                        {
                            if (gameObject == null || gameObject is Sim)
                            {
                                continue;
                            }
                            target2 = PassionBase.GetTarget(gameObject);
                            try
                            {
                                if (target2.ObjectType.Name.ToString().Contains("Table") || target2.ObjectType.Name.ToString() == "Couch" || target2.ObjectType.Name.ToString() == "Sofa" || target2.ObjectType.Name.ToString() == "Loveseat" || target2.ObjectType.Name.ToString().Contains("Chair") || target2.ObjectType.Name.ToString() == "Fridge" || target2.ObjectType.Name.ToString().Contains("Bed") || target2.ObjectType.Name.ToString() == "Stove" || target2.ObjectType.Name.ToString() == "Counter" || target2.ObjectType.Name.ToString() == "Toilet" || target2.ObjectType.Name.ToString().Contains("Shower") || target2.ObjectType.Name.ToString() == "Altar" || target2.ObjectType.Name.ToString() == "DoorSingle")
                                {
                                    target = PassionBase.GetTarget(gameObject);
                                    break;
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                if (target.IsOccupied)
                {
                    player.PlayRouteFailure();
                    return false;
                }
                if (player.Join(target))
                {
                    player.SetPartnersToCheck(GetSelectedObjectsAsSims());
                    if (player.PartnersToCheckCount > 0)
                    {
                        foreach (Player item in player.PartnersToCheck)
                        {
                            if (item != null)
                            {
                                player.Actor.InteractionQueue.AddNext(AskToPassion.Singleton.CreateInstance(item.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                            }
                        }
                    }
                    else
                    {
                        player.Leave();
                    }
                    return true;
                }
                return false;
            }
        }

        internal sealed class AskToPassion : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, AskToPassion>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.AskingTo") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                Player player2 = PassionBase.GetPlayer(Target);
                return player.Invite(player2, this);
            }
        }

        internal sealed class BeAskedToPassion : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, BeAskedToPassion>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.BeingAskedTo") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                Player player2 = PassionBase.GetPlayer(Target);
                return player.Respond(player2);
            }
        }

        internal sealed class RouteToPassion : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private class Definition : InteractionDefinition<Sim, Sim, RouteToPassion>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.HeadingTo") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                return PassionBase.GetPlayer(Actor).Route();
            }
        }

        internal sealed class BeginPassion : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, BeginPassion>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize(PassionCommon.Localize("S3_Passion.Terms.Beginning") + " " + PersistableSettings.Settings.Label);
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                return PassionBase.GetPlayer(Actor).StartLoop();
            }
        }


        // passionloop int
        internal sealed class PassionLoop : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, PassionLoop>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    if (actor != null)
                    {
                        Player player = PassionBase.GetPlayer(actor);
                        if (player.IsValid && player.HasPart && player.Part.HasPosition)
                        {
                            return PersistableSettings.Settings.ActiveLabel + PassionCommon.NewLine + PassionCommon.Localize(player.Part.Position.Name);
                        }
                    }
                    return PersistableSettings.Settings.ActiveLabel;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override void Init(ref InteractionInstanceParameters parameters)
            {
                parameters.Priority = new InteractionPriority(InteractionPriorityLevel.High);
                base.Init(ref parameters);
            }

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                return PassionBase.GetPlayer(Actor).DoLoop();
            }
        }

        internal sealed class SwitchWith : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, SwitchWith>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.SwitchWith") + " " + target.Name;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && actor != target)
                    {
                        Player player = PassionBase.GetPlayer(actor);
                        Player player2 = PassionBase.GetPlayer(target);
                        if (player.IsActive && player.HasPart && player2.IsActive && player2.HasPart)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                Player player2 = PassionBase.GetPlayer(Target);
                Part.BroWeAreSwitching = true;
                player.SwitchBuffer = false;
                player2.SwitchBuffer = false;
                PassionBase.SwitchPlayerPartner = Target;
                PassionBase.SwitchPlayerActor = Actor;
                if (player.SwitchBuffer || player2.SwitchBuffer)
                {
                    return false;
                }
                else
                {
                    player.Switch(player2);
                    return true;
                }

            }
        }

        internal sealed class SwitchRoute : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, SwitchRoute>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.SwitchingWith") + " " + (target != null ? target.Name : "?");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                if (Actor != null && Target != null)
                {
                    Player player = PassionBase.GetPlayer(Actor);
                    Player player2 = PassionBase.GetPlayer(Target);
                    if (player.SwitchBuffer)
                    {
                        return false;
                    }
                    else
                    {
                        if (player.IsValid && player2.IsValid)
                        {
                            player.CanSwitch = true;
                            player.SwitchBuffer = true;

                            player.ActiveLeaveJoin = true;
                            if (player.Join(player.SwitchPart))
                            {
                                player.Route();
                            }
                        }
                        player.EndSwitch();
                        return true;
                    }

                }
                return false;
            }
        }

        public sealed class Embarrassed : Interaction<Sim, Sim>
        {
            private sealed class Definition : InteractionDefinition<Sim, Sim, Embarrassed>
            {
                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Embarrassed");
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override void Init(ref InteractionInstanceParameters parameters)
            {
                parameters.Priority = new InteractionPriority(InteractionPriorityLevel.Privacy, 0f);
                base.Init(ref parameters);
            }

            public override bool Run()
            {
                bool flag = PrivacySituation.RouteToAdjacentRoom(Actor);
                if (!flag && Actor.InteractionQueue.GetNextInteraction() == null)
                {
                    Actor.RouteTurnToFace(Target.Position);
                    Actor.PlayRouteFailure();
                }
                return flag;
            }
        }

        internal sealed class MoveTo : ImmediateInteraction<Sim, IGameObject>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, IGameObject, MoveTo>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.MoveTo");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && !(target is Sim) && PassionType.IsSupported(target))
                    {
                        Player player = PassionBase.GetPlayer(actor);
                        PassionTarget target2 = PassionBase.GetTarget(target);
                        if (player.IsActive && !target2.IsOccupied)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                PassionTarget target = PassionBase.GetTarget(Target);
                if (target != null && target.IsValid && target.HasObject)
                {
                    player.Actor.InteractionQueue.AddNext(MoveRoute.Singleton.CreateInstance(target.Object, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                    player.ActiveLeaveJoin = true;
                    player.Stop();
                    return true;
                }
                return false;
            }
        }

        internal sealed class MoveGroupTo : ImmediateInteraction<Sim, IGameObject>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, IGameObject, MoveGroupTo>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.MoveGroupTo");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && !(target is Sim) && PassionType.IsSupported(target))
                    {
                        Player player = PassionBase.GetPlayer(actor);
                        PassionTarget target2 = PassionBase.GetTarget(target);
                        if (player.IsActive && !target2.IsOccupied && player.HasPart && player.Part.Remaining > 1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                PassionTarget target = PassionBase.GetTarget(Target);
                if (target != null && target.IsValid && target.HasObject && player.IsValid && player.HasPart)
                {
                    Part switchPart = null;
                    PartArea key = PassionTarget.ChooseSectionDialog(target.ObjectType);
                    if (target.Parts.ContainsKey(key))
                    {
                        switchPart = target.Parts[key];
                    }
                    foreach (Player value in player.Part.Players.Values)
                    {
                        if (value != player)
                        {
                            value.SwitchPart = switchPart;
                            value.Actor.InteractionQueue.AddNext(MoveGroupRoute.Singleton.CreateInstance(target.Object, value.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                            value.ActiveLeaveJoin = true;
                            value.Stop();
                        }
                    }
                    player.SwitchPart = switchPart;
                    player.Actor.InteractionQueue.AddNext(MoveGroupRoute.Singleton.CreateInstance(target.Object, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                    player.ActiveLeaveJoin = true;
                    player.Stop();
                    return true;
                }
                return false;
            }
        }

        internal sealed class MoveRoute : Interaction<Sim, IGameObject>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, IGameObject, MoveRoute>
            {
                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Moving");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                if (Actor != null && Target != null)
                {
                    Player player = PassionBase.GetPlayer(Actor);
                    PassionTarget target = PassionBase.GetTarget(Target);
                    if (player.IsValid && target.HasObject && player.Join(target))
                    {
                        player.Route();
                    }
                    return true;
                }
                return false;
            }
        }

        internal sealed class MoveGroupRoute : Interaction<Sim, IGameObject>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, IGameObject, MoveGroupRoute>
            {
                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Moving");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                if (Actor != null && Target != null)
                {
                    Player player = PassionBase.GetPlayer(Actor);
                    PassionTarget target = PassionBase.GetTarget(Target);
                    if (player.IsValid && target.HasObject && player.HasSwitchPart && player.Join(player.SwitchPart))
                    {
                        player.Route();
                    }
                    player.EndSwitch();
                    return true;
                }
                return false;
            }
        }

        internal sealed class JoinPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, JoinPassion>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Join") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(actor))
                    {
                        try
                        {
                            Player player = PassionBase.GetPlayer(actor);
                            Player player2 = PassionBase.GetPlayer(target);
                            if (!player.IsActive && player2.IsActive && player2.HasPart && player2.Part.HasRoom && player.WillPassion(player2.Part))
                            {
                                return true;
                            }
                        }
                        catch
                        {
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                Player player2 = PassionBase.GetPlayer(Target);
                if (player.IsValid && player2.IsValid && player2.HasPart && player2.Part.HasTarget && player2.Part.HasRoom)
                {
                    player.Join(player2.Part);
                    Actor.InteractionQueue.AddNext(RouteToPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                }
                return true;
            }
        }

        internal sealed class ActiveJoinPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ActiveJoinPassion>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Join") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(actor))
                    {
                        try
                        {
                            Player player = PassionBase.GetPlayer(actor);
                            Player player2 = PassionBase.GetPlayer(target);
                            if (player.IsSolo && player2.IsActive && player.Actor != player2.Actor && player2.HasPart && player2.Part.HasRoom && player.WillPassion(player2.Part))
                            {
                                return true;
                            }
                        }
                        catch
                        {
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                Player player2 = PassionBase.GetPlayer(Target);
                if (player.IsSolo && player2.IsValid && player2.HasPart && player2.Part.HasTarget && player2.Part.HasRoom)
                {
                    player.ActiveLeave = true;
                    player.Stop();
                    player.Actor.InteractionQueue.AddNext(DelayedJoinPassion.Singleton.CreateInstance(player2.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                }
                return true;
            }
        }

        internal sealed class DelayedJoinPassion : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, DelayedJoinPassion>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Join") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                if (Actor != null && Target != null)
                {
                    Player player = PassionBase.GetPlayer(Actor);
                    Player player2 = PassionBase.GetPlayer(Target);
                    if (player.IsValid && player2.HasPart && player2.Part.HasRoom && player.WillPassion(player2.Part))
                    {
                        player.ActiveJoin = true;
                        if (player.Join(player2.Part))
                        {
                            player.Route();
                        }
                        else
                        {
                            player.ActiveLeaveJoin = false;
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        internal sealed class AskToJoinPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, AskToJoinPassion>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.AsktoJoin") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        try
                        {
                            Player player = PassionBase.GetPlayer(actor);
                            Player player2 = PassionBase.GetPlayer(target);
                            if (player.IsActive && !player2.IsActive && player.HasPart && player.Part.HasRoom && player2.WillPassion(player.Part))
                            {
                                return true;
                            }
                        }
                        catch
                        {
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Target.InteractionQueue.CancelAllInteractions();
                Target.InteractionQueue.AddNext(JoinPassion.Singleton.CreateInstance(Actor, Target, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                return true;
            }
        }

        internal sealed class AskToPassionOther : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, AskToPassionOther>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.AsktoPassionOther") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        try
                        {
                            if (actor != null && target != null && actor != target && !PassionBase.GetPlayer(target).IsActive)
                            {
                                if (!PersistableSettings.Settings.CanReject || actor.HasTrait(TraitNames.MasterOfSeduction))
                                {
                                    return true;
                                }
                                Relationship relationship = actor.SimDescription.GetRelationship(target.SimDescription, false);
                                if (relationship != null && relationship.LTR.Liking > 70f)
                                {
                                    return true;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    return false;
                }

                public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
                {
                    Sim item = parameters.Actor as Sim;
                    Sim sim = parameters.Target as Sim;
                    List<Sim> availablePartners = Player.GetAvailablePartners(sim);
                    if (availablePartners.Contains(item))
                    {
                        availablePartners.Remove(item);
                    }
                    NumSelectableRows = availablePartners.Count;
                    PopulateSimPicker(ref parameters, out listObjs, out headers, availablePartners, false);
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Target);
                PassionTarget target = player.GetNearbySupportedTarget();
                if (target == null)
                {
                    target = PassionBase.GetTarget(player.Actor);
                }
                if (player.Join(target))
                {
                    player.SetPartnersToCheck(GetSelectedObjectsAsSims());
                    if (player.PartnersToCheckCount > 0)
                    {
                        foreach (Player item in player.PartnersToCheck)
                        {
                            if (item != null)
                            {
                                item.Actor.InteractionQueue.CancelAllInteractions();
                                player.Actor.InteractionQueue.AddNext(AskToPassion.Singleton.CreateInstance(item.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                            }
                        }
                    }
                    else
                    {
                        Target.InteractionQueue.CancelAllInteractions();
                        Target.InteractionQueue.AddNext(RouteToPassion.Singleton.CreateInstance(Actor, Target, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                    }
                    return true;
                }
                return false;
            }
        }

        internal sealed class AskToSoloPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, AskToSoloPassion>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Suggest") + " " + PersistableSettings.Settings.SoloLabel;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        try
                        {
                            if (actor != null && target != null && actor != target && !PassionBase.GetPlayer(target).IsActive)
                            {
                                if (!PersistableSettings.Settings.CanReject || actor.HasTrait(TraitNames.MasterOfSeduction))
                                {
                                    return true;
                                }
                                Relationship relationship = actor.SimDescription.GetRelationship(target.SimDescription, false);
                                if (relationship != null && relationship.LTR.Liking > 70f)
                                {
                                    return true;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Target);
                PassionTarget target = player.GetNearbySupportedTarget();
                if (target == null)
                {
                    target = PassionBase.GetTarget(player.Actor);
                }
                if (player.Join(target))
                {
                    player.Actor.InteractionQueue.CancelAllInteractions();
                    if (target.ObjectType.Is<Floor>())
                    {
                        player.ForceStartLoopImmediate();
                    }
                    else
                    {
                        player.Actor.InteractionQueue.AddNext(RouteToPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                    }
                    return true;
                }
                return false;
            }
        }

        internal sealed class AutoSoloPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, AutoSoloPassion>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Suggest") + " " + PersistableSettings.Settings.SoloLabel;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    try
                    {
                        if (PassionBase.IsValid(target) && actor != null && target != null && actor != target && !PassionBase.GetPlayer(target).IsActive)
                        {
                            Relationship relationship = actor.SimDescription.GetRelationship(target.SimDescription, false);
                        }
                    }
                    catch
                    {
                    }
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Target);
                PassionTarget target = player.GetNearbySupportedTarget();
                if (target == null)
                {
                    target = PassionBase.GetTarget(player.Actor);
                }
                if (player.Join(target))
                {
                    player.Actor.InteractionQueue.CancelAllInteractions();
                    if (target.ObjectType.Is<Floor>())
                    {
                        player.ForceStartLoopImmediate();
                    }
                    else
                    {
                        player.Actor.InteractionQueue.AddNext(RouteToPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                    }
                    return true;
                }
                return false;
            }
        }

        internal sealed class StopPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, StopPassion>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Stop");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        Player player = PassionBase.GetPlayer(target);
                        if (player.IsActive)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Target);
                if (player != null)
                {
                    player.ActiveLeaveJoin = false;
                    player.Stop();
                }
                return true;
            }
        }

        internal sealed class StopAllPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, StopAllPassion>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.StopAll");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        Player player = PassionBase.GetPlayer(target);
                        if (player.IsActive)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Target);
                if (player.HasPart)
                {
                    player.Part.StopAllPlayers();
                }
                return true;
            }
        }

        internal sealed class AskToWatchPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, AskToWatchPassion>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.AsktoWatch") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        try
                        {
                            Player player = PassionBase.GetPlayer(actor);
                            Player player2 = PassionBase.GetPlayer(target);
                            if (player.IsActive && !player2.IsActive && !player2.IsWatching && !player.Actor.SimDescription.ChildOrBelow && player.Actor.SimDescription.IsHuman)
                            {
                                return true;
                            }
                        }
                        catch
                        {
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Target.InteractionQueue.AddNext(WatchPassion.Singleton.CreateInstance(Actor, Target, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                return true;
            }
        }

        internal sealed class WatchPassion : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, WatchPassion>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Watch") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    Player player = PassionBase.GetPlayer(a);
                    Player player2 = PassionBase.GetPlayer(target);
                    if (player.IsValid && !player.IsActive && player2.IsValid && player2.IsActive && !player.Actor.SimDescription.ChildOrBelow && player.Actor.SimDescription.IsHuman)
                    {
                        return true;
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Actor.InteractionQueue.AddNext(WatchLoop.Singleton.CreateInstance(Target, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                return true;
            }
        }

        internal sealed class WatchLoop : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, WatchLoop>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Watch") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                PassionBase.GetPlayer(Actor).Watch(Target);
                return true;
            }
        }

        internal sealed class WatchMasturbate : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, WatchMasturbate>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PersistableSettings.Settings.SoloLabel;
                }

                public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    Player player = PassionBase.GetPlayer(a);
                    if (player.IsWatching && PassionBase.IsValid(a) && a == target && !player.Actor.SimDescription.ChildOrBelow && player.Actor.SimDescription.IsHuman)
                    {
                        return true;
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                PassionTarget target = PassionBase.GetTarget(Actor);
                if (player.Join(target))
                {
                    player.State = PassionState.Routing;
                    player.Actor.InteractionQueue.AddNext(BeginPassion.Singleton.CreateInstance(Actor, Actor, new InteractionPriority(InteractionPriorityLevel.High), false, true));
                    player.Actor.InteractionQueue.CancelInteraction(player.Actor.CurrentInteraction, true);
                }
                return true;
            }
        }



        internal sealed class SetPreferredOutfit : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, SetPreferredOutfit>
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.SetPreferredOutfit");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        Player player = PassionBase.GetPlayer(target);
                        if (!player.IsActive)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                PassionBase.GetPlayer(Target).PreferOutfit();
                return true;
            }
        }

        internal sealed class ClearPreferredOutfit : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ClearPreferredOutfit>
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.ClearPreferredOutfit");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        Player player = PassionBase.GetPlayer(target);
                        if (!player.IsActive && player.HasPreferredOutfit)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                PassionBase.GetPlayer(Target).ClearPreferredOutfit();
                return true;
            }
        }

        internal sealed class ChangePosition : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ChangePosition>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.ChangePosition");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        Player player = PassionBase.GetPlayer(target);
                        if (player.IsActive)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                PassionBase.GetPlayer(Target).ChangePosition();
                return true;
            }
        }

        internal sealed class PassionSettingsMenu : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, PassionSettingsMenu>
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.SettingsMenu");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target) && !PassionBase.GetPlayer(target).IsActive)
                    {
                        return true;
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                PersistableSettings.Show();
                return true;
            }
        }

        internal sealed class PassionSettingsMenuActive : ImmediateInteraction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, PassionSettingsMenuActive>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.SettingsMenu");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(target) && PassionBase.GetPlayer(target).IsActive)
                    {
                        return true;
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                PersistableSettings.Show();
                return true;
            }
        }

        internal sealed class ResetMe : ImmediateInteraction<Sim, IGameObject>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, IGameObject, ResetMe>
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.ResetMe");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous)
                    {
                        GameObject gameObject = target as GameObject;
                        ulong value = gameObject.ObjectId.Value;
                        if (gameObject is Sim && PassionBase.IsValid(gameObject as Sim))
                        {
                            if (PassionBase.AllPlayers.ContainsKey(value) && PassionBase.AllPlayers[value] != null && !PassionBase.AllPlayers[value].IsActive)
                            {
                                return true;
                            }
                            return false;
                        }
                        if (gameObject != null && PassionBase.AllTargets.ContainsKey(gameObject.ObjectId.Value))
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                PassionBase.ResetMe(Target);
                return true;
            }
        }

        internal sealed class ResetMeActive : ImmediateInteraction<Sim, IGameObject>
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, IGameObject, ResetMeActive>
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.ResetMe");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous)
                    {
                        GameObject gameObject = target as GameObject;
                        ulong value = gameObject.ObjectId.Value;
                        if (gameObject is Sim && PassionBase.IsValid(gameObject as Sim))
                        {
                            if (PassionBase.AllPlayers.ContainsKey(value) && PassionBase.AllPlayers[value] != null && PassionBase.AllPlayers[value].IsActive)
                            {
                                return true;
                            }
                            return false;
                        }
                        if (gameObject != null && PassionBase.AllTargets.ContainsKey(gameObject.ObjectId.Value))
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                PassionBase.ResetMe(Target);
                return true;
            }
        }

        internal sealed class Report : ImmediateInteraction<Sim, IGameObject>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, IGameObject, Report>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Report");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return PassionCommon.Testing;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                PassionCommon.DumpMessages();
                PassionCommon.BufferClear();
                int num = 0;
                foreach (Player value2 in PassionBase.AllPlayers.Values)
                {
                    if (value2.IsValid)
                    {
                        if (num != 0)
                        {
                            PassionCommon.BufferLine("  ------------------");
                        }
                        PassionCommon.BufferLine(" Name: " + value2.Actor.Name);
                        PassionCommon.BufferLine("  State: " + value2.State);
                        PassionCommon.BufferLine("  CanAnimate: " + value2.CanAnimate);
                        PassionCommon.BufferLine("  HasPart: " + value2.HasPart);
                        if (value2.HasPart)
                        {
                            PassionCommon.BufferLine("   Part.Initiator: " + (value2.Part.HasInitiator ? value2.Part.Initiator.Name : ""));
                            PassionCommon.BufferLine("   Part.Count: " + value2.Part.Count);
                            PassionCommon.BufferLine("   Part.Players.Count: " + value2.Part.Players.Count);
                        }
                        PassionCommon.BufferLine("  PositionIndex: " + value2.PositionIndex);
                        if (++num > 6)
                        {
                            num = 0;
                            PassionCommon.SystemMessage();
                        }
                    }
                }
                if (!PassionCommon.MessageBufferEmpty)
                {
                    PassionCommon.SystemMessage();
                }
                if (PassionBase.AllTargets.Count > 0)
                {
                    num = 0;
                    foreach (KeyValuePair<ulong, PassionTarget> allTarget in PassionBase.AllTargets)
                    {
                        if (allTarget.Value != null)
                        {
                            if (allTarget.Value.HasObject)
                            {
                                if (num != 0)
                                {
                                    PassionCommon.BufferLine("  ------------------");
                                }
                                PassionTarget value = allTarget.Value;
                                if (allTarget.Value.HasObject)
                                {
                                    PassionCommon.BufferLine(" Name: " + value.Object.GetLocalizedName());
                                }
                                if (allTarget.Value.ObjectType != null)
                                {
                                    PassionCommon.BufferLine(" Type: " + allTarget.Value.ObjectType.Name);
                                }
                                else
                                {
                                    PassionCommon.BufferLine(" Type: null");
                                }
                                PassionCommon.BufferLine(" Parts (" + value.Parts.Count + "):");
                                foreach (Part value3 in value.Parts.Values)
                                {
                                    PassionCommon.BufferLine("  " + value3.Area);
                                    PassionCommon.BufferLine("   Players (" + value3.Players.Count + "):");
                                    foreach (Player value4 in value3.Players.Values)
                                    {
                                        if (value4.IsValid)
                                        {
                                            PassionCommon.BufferLine("    " + value4.Actor.Name + (value4.IsInitiator ? " (I)" : ""));
                                        }
                                    }
                                }
                                if (++num > 6)
                                {
                                    num = 0;
                                    PassionCommon.SystemMessage();
                                }
                            }
                            else
                            {
                                PassionCommon.BufferLine("Target Object Reference Missing:" + allTarget.Key);
                            }
                        }
                        else
                        {
                            PassionCommon.BufferLine("Target Missing:" + allTarget.Key);
                        }
                    }
                    if (!PassionCommon.MessageBufferEmpty)
                    {
                        PassionCommon.SystemMessage();
                    }
                }
                return true;
            }
        }

        internal sealed class Test : ImmediateInteraction<Sim, IGameObject>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, IGameObject, Test>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Buffer Position XML");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return PassionCommon.Testing;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public static int Index = 0;

            public override bool Run()
            {
                Sim sim = Target as Sim;
                if (Test2.Exporting && Test2.Root != null && sim != null)
                {
                    Player player = PassionBase.GetPlayer(sim);
                    if (player.IsValid && player.HasPart && player.Part.HasPosition)
                    {
                        try
                        {
                            XML.Element element = Test2.Root.AddChild("Position");
                            Position position = player.Part.Position;
                            int count = player.Part.Count;
                            element.AddComment("Localized Name:  \"" + PickString.Show(PassionCommon.Localize("New Localized Name"), "", "") + "\"");
                            element.AddChild("Key", position.Key);
                            element.AddChild("Name", position.Name);
                            element.AddChild("Creator", position.Creator);
                            string text = string.Empty;
                            foreach (PassionType value2 in position.SupportedTypes.Values)
                            {
                                if (text != string.Empty)
                                {
                                    text += ",";
                                }
                                text += value2.Name;
                            }
                            element.AddChild("Targets", text);
                            element.AddChild("Tones", PickString.Show(PassionCommon.Localize("Tones"), PassionCommon.Localize("Currently-valid Tones are:") + "Romantic, Intense, Rough, Disinterested, Dominating, Toy", "Any"));
                            element.AddChild("Categories", PickString.Show(PassionCommon.Localize("Categories"), PassionCommon.Localize("Currently-valid Categories are:") + " None, Any, Foreplay, Oral, Anal, Vaginal, Hands, Feet, Breasts, Masturbate (Hands/Feet), Fuck (Vaginal/Anal)", "Any"));
                            element.AddChild("MinSims", position.MinSims.ToString());
                            element.AddChild("MaxSims", position.MaxSims.ToString());
                            XML.Element element2 = element.AddChild("AnimationSets");
                            XML.Element element3 = element2.AddChild("Set");
                            element3.AddAttribute("Players", count.ToString());
                            foreach (Player value3 in player.Part.Players.Values)
                            {
                                XML.Element element4 = element3.AddChild("Player");
                                element4.AddAttribute("Index", value3.PositionIndex.ToString());
                                if (value3.HeldItem != null)
                                {
                                    XML.Element element5 = element4.AddChild("HeldItem");
                                    XML.Element element6 = element5.AddChild("ObjectKey", string.Format("{0:x8}-{1:x8}-{2:x16}", value3.HeldItem.Key.TypeId, value3.HeldItem.Key.GroupId, value3.HeldItem.Key.InstanceId));
                                    XML.Element element7 = element5.AddChild("Location");
                                    element7.AddAttribute("X", value3.HeldItem.Location.x.ToString());
                                    element7.AddAttribute("Y", value3.HeldItem.Location.y.ToString());
                                    element7.AddAttribute("Z", value3.HeldItem.Location.z.ToString());
                                    XML.Element element8 = element5.AddChild("Facing");
                                    element8.AddAttribute("X", value3.HeldItem.Facing.x.ToString());
                                    element8.AddAttribute("Y", value3.HeldItem.Facing.y.ToString());
                                    element8.AddAttribute("Z", value3.HeldItem.Facing.z.ToString());
                                    element5.AddChild("Angle", player.HeldItem.Angle.ToString());
                                    HeldItem.ItemSlots slot = (HeldItem.ItemSlots)player.HeldItem.Slot;
                                    element5.AddChild("Slot", slot.ToString());
                                }
                                XML.Element element9 = element4.AddChild("Clip", position.GetAnimation(value3));
                                string value = PickString.Show(PassionCommon.Localize("Requirement"), PassionCommon.Localize("What clip requirements are there for ") + value3.Name + "?", "");
                                if (!string.IsNullOrEmpty(value))
                                {
                                    element9.Attributes.Add(new XML.Attribute("Required", value));
                                }
                            }
                            PassionCommon.SystemMessage("Position Buffered");
                        }
                        catch
                        {
                            PassionCommon.SystemMessage("Couldn't export file.");
                        }
                    }
                    else
                    {
                        PassionCommon.SystemMessage("Nothing to report.");
                    }
                }
                return true;
            }
        }

        internal sealed class Test2 : ImmediateInteraction<Sim, IGameObject>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, IGameObject, Test2>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Toggle Export");
                }

                public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return PassionCommon.Testing;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public static bool Exporting = false;

            public static XML.Element Root = null;

            public override bool Run()
            {
                Exporting = !Exporting;
                if (Exporting)
                {
                    PassionCommon.SystemMessage("Buffering Export Data");
                    Root = XML.Element.Create("Passion");
                }
                else
                {
                    if (XML.WriteToFile(Root, "PassionPosition"))
                    {
                        PassionCommon.SystemMessage("Successfully Exported Data");
                    }
                    else
                    {
                        PassionCommon.SystemMessage("Unable to Export Data");
                    }
                    Root = null;
                }
                return true;
            }
        }

        internal sealed class Reassure : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, Reassure>
            {
                public override string[] GetPath(bool bPath)
                {
                    return RomancePath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.Reassure");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return target != null && target.BuffManager.HasElement((BuffNames)5912255412026328145uL);
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                Player player2 = PassionBase.GetPlayer(Target);
                Actor.SynchronizationTarget = player2.Actor;
                Actor.SynchronizationRole = Sim.SyncRole.Initiator;
                Actor.SynchronizationLevel = Sim.SyncLevel.Started;
                InteractionInstance entry = LinkedInteractionInstance = BeReassured.Singleton.CreateInstance(Actor, Target, new InteractionPriority(InteractionPriorityLevel.High), false, true);
                player2.Actor.InteractionQueue.Add(entry);
                if (Actor.WaitForSynchronizationLevelWithSim(player2.Actor, Sim.SyncLevel.Started, 60f) && player.Route(player2))
                {
                    Actor.SynchronizationLevel = Sim.SyncLevel.Routed;
                    if (Actor.WaitForSynchronizationLevelWithSim(player2.Actor, Sim.SyncLevel.Routed, 60f))
                    {
                        Actor.RouteTurnToFace(player2.Actor.Position);
                        Actor.SynchronizationLevel = Sim.SyncLevel.Committed;
                        if (Actor.WaitForSynchronizationLevelWithSim(player2.Actor, Sim.SyncLevel.Committed, 60f))
                        {
                            Actor.PlaySoloAnimation("a2a_soc_Neutral_Apologize_Neutral_Neutral_x");
                            player2.Actor.BuffManager.RemoveElement((BuffNames)5912255412026328145uL);
                            player.UpdateRelationship(player2, 8f);
                        }
                    }
                }
                return true;
            }
        }

        internal sealed class BeReassured : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private sealed class Definition : InteractionDefinition<Sim, Sim, BeReassured>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.BeReassured");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                Player player2 = PassionBase.GetPlayer(Target);
                Actor.SynchronizationTarget = player2.Actor;
                Actor.SynchronizationRole = Sim.SyncRole.Receiver;
                Actor.SynchronizationLevel = Sim.SyncLevel.Started;
                if (Actor.WaitForSynchronizationLevelWithSim(player2.Actor, Sim.SyncLevel.Started, 60f))
                {
                    Actor.SynchronizationLevel = Sim.SyncLevel.Routed;
                    if (Actor.WaitForSynchronizationLevelWithSim(player2.Actor, Sim.SyncLevel.Routed, 60f))
                    {
                        Actor.RouteTurnToFace(player2.Actor.Position);
                        Actor.SynchronizationLevel = Sim.SyncLevel.Committed;
                        if (Actor.WaitForSynchronizationLevelWithSim(player2.Actor, Sim.SyncLevel.Committed, 60f))
                        {
                            Actor.PlaySoloAnimation("a_soc_idle_amorous_lookDown_x");
                        }
                    }
                }
                return true;
            }
        }

        // NEW INTERACTIONS YAYE

        internal sealed class AddAsexualTrait : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, AddAsexualTrait>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Add Asexual Marker");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (target.HasTrait((TraitNames)6177560411462291097uL))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {
                OnAddAsexual(Target);
                // PassionCommon.Message("the interacton didnt shit itself");
                return true;
            }

            public void OnAddAsexual(Sim target)
            {
                // PassionCommon.Message("the interacton didnt shit itself for real");
                AddMarkers.AsexualToggle(target, true);
            }
        }

        internal sealed class RemoveAsexualTrait : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, RemoveAsexualTrait>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Remove Asexual Marker");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (target.HasTrait((TraitNames)6177560411462291097uL))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {
                OnRemoveAsexual(Target);
                // PassionCommon.Message("the interacton didnt shit itself");
                return true;
            }

            public void OnRemoveAsexual(Sim target)
            {
                // PassionCommon.Message("the interacton didnt shit itself for real");
                AddMarkers.AsexualToggle(target, false);
            }

        }

        internal sealed class AddHypersexualTrait : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, AddHypersexualTrait>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Add Hypersexual Marker");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (target.HasTrait((TraitNames)5711695705602619160uL))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {
                OnAddHypersexual(Target);
                //  PassionCommon.Message("the interacton didnt shit itself");
                return true;
            }

            public void OnAddHypersexual(Sim target)
            {
                // PassionCommon.Message("the interacton didnt shit itself for real");
                AddMarkers.HypersexualToggle(target, true);
            }
        }

        internal sealed class RemoveHypersexualTrait : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, RemoveHypersexualTrait>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Remove Hypersexual Marker");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (target.HasTrait((TraitNames)5711695705602619160uL))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {
                OnRemoveHypersexual(Target);
                // PassionCommon.Message("the interacton didnt shit itself");
                return true;
            }

            public void OnRemoveHypersexual(Sim target)
            {
                // PassionCommon.Message("the interacton didnt shit itself for real");
                AddMarkers.HypersexualToggle(target, false);
            }
        }

        internal sealed class AddAbstinentTrait : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, AddAbstinentTrait>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Add Abstinent Marker");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (target.HasTrait((TraitNames)2214287488174702228uL))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {
                OnAddAbstinent(Target);
                // PassionCommon.Message("the interacton didnt shit itself");
                return true;
            }

            public void OnAddAbstinent(Sim target)
            {
                // PassionCommon.Message("the interacton didnt shit itself for real");
                AddMarkers.AbstinentToggle(target, true);
            }
        }

        internal sealed class RemoveAbstinentTrait : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, RemoveAbstinentTrait>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Remove Abstinent Marker");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (target.HasTrait((TraitNames)2214287488174702228uL))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {
                OnRemoveAbstinent(Target);
                // PassionCommon.Message("the interacton didnt shit itself");
                return true;
            }

            public void OnRemoveAbstinent(Sim target)
            {
                // PassionCommon.Message("the interacton didnt shit itself for real");
                AddMarkers.AbstinentToggle(target, false);
            }
        }


        internal sealed class ToggleStrapon : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ToggleStrapon>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Toggle strapon");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {

                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        Player player = PassionBase.GetPlayer(target);
                        if (player.IsActive)
                        {
                            if (PassionBase.GetPlayer(target).SimGenitalType == "vagina" || PassionBase.GetPlayer(target).SimGenitalType == "neither")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return false;

                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {

                Player balls = null;

                balls = new Player();

                if (PassionBase.GetPlayer(Target).StrapIsOn)
                {
                    balls.SwitchToStrapon(Target, false);
                    PassionBase.GetPlayer(Target).ForceNoStrap = true;
                    return false;
                }
                else
                {
                    balls.SwitchToStrapon(Target, true);
                    PassionBase.GetPlayer(Target).ForceNoStrap = false;
                    return true;
                }
            }
        }


        internal sealed class ToggleErection : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ToggleErection>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return NoPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("Toggle erection");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {

                    if (!IsAutonomous && PassionBase.IsValid(target))
                    {
                        Player player = PassionBase.GetPlayer(target);
                        if (player.IsActive)
                        {
                            if (PassionBase.GetPlayer(target).SimGenitalType == "penis" || PassionBase.GetPlayer(target).SimGenitalType == "both")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return false;

                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {

                Player balls = null;

                balls = new Player();

                if (PassionBase.GetPlayer(Target).PeenIsErect)
                {
                    balls.SwitchToPeener(Target, false);
                    return false;
                }
                else
                {
                    balls.SwitchToPeener(Target, true);
                    return true;
                }
            }
        }


        internal sealed class DEBUGCheckBottom : ImmediateInteraction<Sim, Sim>, IImmediateInteraction
        {
            [DoesntRequireTuning]
            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, DEBUGCheckBottom>, IImmediateInteractionDefinition
            {
                public override string[] GetPath(bool bPath)
                {
                    return PassionPath;
                }

                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("DEBUG: Check nude bottom hash");
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
            public static readonly InteractionDefinition Singleton = new Definition();


            public override bool Run()
            {

                PenisInspection.GetBottomCASP(Target);
                return true;

            }
        }


        // END NEW INTERACTIONS

        public sealed class UsePoolLadderForPassionWithSim : Interaction<Sim, PoolLadder>
        {
            private sealed class Definition : InteractionDefinition<Sim, PoolLadder, UsePoolLadderForPassionWithSim>, IHasTraitIcon, IHasMenuPathIcon
            {
                public ResourceKey GetTraitIcon(Sim actor, GameObject target)
                {
                    return Interactions.GetTraitIcon(actor, target);
                }

                public ResourceKey GetPathIcon(Sim actor, GameObject target)
                {
                    return Interactions.GetPathIcon(actor, target);
                }

                public override string[] GetPath(bool bPath)
                {
                    return RomancePath;
                }

                public override string GetInteractionName(Sim actor, PoolLadder target, InteractionObjectPair interaction)
                {
                    return PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, PoolLadder target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    if (!IsAutonomous && PassionBase.IsValid(actor))
                    {
                        Player player = PassionBase.GetPlayer(actor);
                        PassionTarget target2 = PassionBase.GetTarget(target);
                        if (!player.IsValid || player.IsActive || player.IsWatching || target2.MaxSims < 2 || !player.IsInPool || player.Actor.Posture.Container != Pool.GetPoolNearestPoint(target.Position))
                        {
                            return false;
                        }
                        return true;
                    }
                    return false;
                }

                public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
                {
                    List<Sim> availablePartners = Player.GetAvailablePartners(parameters.Actor as Sim);
                    NumSelectableRows = availablePartners.Count;
                    PopulateSimPicker(ref parameters, out listObjs, out headers, availablePartners, false);
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                PassionTarget target = PassionBase.GetTarget(Target);
                if (player.Join(target))
                {
                    foreach (Sim selectedObjectsAsSim in GetSelectedObjectsAsSims())
                    {
                        Player player2 = PassionBase.GetPlayer(selectedObjectsAsSim);
                        if (player2.IsValid && player2.WillPassion(player) && (PersistableSettings.Settings.ActiveAlwaysAccepts || player.WillPassion(player2)) && player2.Join(target))
                        {
                            if (player2.IsInPool && player2.Actor.Posture.Container == Pool.GetPoolNearestPoint(Target.Position) && player2.Join(target))
                            {
                                player2.Actor.InteractionQueue.AddNext(RouteToPoolLadderPassion.Singleton.CreateInstance(player2.Actor, player2.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                            }
                            else
                            {
                                player2.Route();
                            }
                        }
                    }
                    if (Target.PoolPortalComponent.RouteToSlotGroup(player.Actor, PoolPortalComponent.RoutingSlotGroup.Inside))
                    {
                        player.DirectStartLoop();
                    }
                }
                return true;
            }
        }

        internal sealed class RouteToPoolLadderPassion : Interaction<Sim, Sim>
        {
            [DoesntRequireTuning]
            private class Definition : InteractionDefinition<Sim, Sim, RouteToPoolLadderPassion>
            {
                public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
                {
                    return PassionCommon.Localize("S3_Passion.Terms.HeadingTo") + " " + PersistableSettings.Settings.Label;
                }

                public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }

            public static readonly InteractionDefinition Singleton = new Definition();

            public override ThumbnailKey GetIconKey()
            {
                return WoohooThumbnail;
            }

            public override bool Run()
            {
                Player player = PassionBase.GetPlayer(Actor);
                if (player.IsValid && player.HasPart)
                {
                    if (player.IsInPool && player.Actor.Posture is SwimmingInPool && player.Part.HasTarget && player.Part.Target.HasObject && player.Actor.Posture.Container == player.Part.Target.Object)
                    {
                        PoolLadder poolLadder = player.Part.Target.Object as PoolLadder;
                        if (poolLadder != null && poolLadder.PoolPortalComponent.RouteToSlotGroup(player.Actor, PoolPortalComponent.RoutingSlotGroup.Inside))
                        {
                            player.DirectStartLoop();
                        }
                    }
                    else
                    {
                        player.Route();
                    }
                }
                else
                {
                    player.Leave();
                }
                return true;
            }
        }

        public static readonly ResourceKey Woohoo = ResourceKey.CreatePNGKey("moodlet_woohoo_s", 0u);

        public static readonly ResourceKey Libido = ResourceKey.CreatePNGKey("moodlet_libido_positive", 0u);

        public static readonly ThumbnailKey WoohooThumbnail = new ThumbnailKey(Woohoo, ThumbnailSize.Medium);

        public static readonly ThumbnailKey LibidoThumbnail = new ThumbnailKey(Libido, ThumbnailSize.Medium);

        public static readonly ResourceKey MasterOfSeduction = ResourceKey.CreatePNGKey("trait_masterofseduction_s", 0u);

        public static readonly ResourceKey Attractive = ResourceKey.CreatePNGKey("trait_attractive_s", 0u);

        public static readonly ResourceKey EyeCandy = ResourceKey.CreatePNGKey("trait_eyecandy_s", 0u);

        public static readonly string[] NoPath = new string[0];

        public static readonly string[] PassionPath = new string[1] { PassionCommon.Localize("S3_Passion.PassionPath") };

        public static readonly string[] RomancePath = new string[1] { PassionCommon.Localize("S3_Passion.RootName") };

        public static ResourceKey GetPathIcon(Sim actor, GameObject target)
        {
            return GetTraitIcon(actor, target);
        }

        public static ResourceKey GetTraitIcon(Sim actor, GameObject target)
        {
            Trait trait = null;
            if (actor != null)
            {
                if (GameUtils.IsInstalled(ProductVersion.EP3) && actor.HasTrait(TraitNames.MasterOfSeduction))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.MasterOfSeduction);
                }
                else if (actor.HasTrait(TraitNames.Shy))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.Shy);
                }
                else if (GameUtils.IsInstalled(ProductVersion.EP9) && actor.HasTrait(TraitNames.Irresistible))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.Irresistible);
                }
                else if (actor.HasTrait(TraitNames.PartyAnimal))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.PartyAnimal);
                }
                else if (GameUtils.IsInstalled(ProductVersion.EP1) && actor.HasTrait(TraitNames.EyeCandy))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.EyeCandy);
                }
                else if (actor.HasTrait(TraitNames.Attractive))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.Attractive);
                }
                else if (actor.HasTrait(TraitNames.HopelessRomantic))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.HopelessRomantic);
                }
                else if (actor.HasTrait(TraitNames.Flirty))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.Flirty);
                }
                else if (actor.HasTrait(TraitNames.Unflirty))
                {
                    trait = TraitManager.GetTraitFromDictionary(TraitNames.Unflirty);
                }
            }
            if (trait != null)
            {
                switch (trait.Guid)
                {
                    case TraitNames.MasterOfSeduction:
                        return MasterOfSeduction;
                    case TraitNames.Attractive:
                        return Attractive;
                    case TraitNames.EyeCandy:
                        return EyeCandy;
                    case TraitNames.Unflirty:
                    case TraitNames.Shy:
                        return trait.DislikePieMenuKey;
                    default:
                        return trait.PieMenuKey;
                }
            }
            return Woohoo;
        }
    }
}
