using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion.BOE_Lovemaking;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.VideoRecording;
using Sims3.Store.Objects;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Settings;
using S3_Passion.BOE_STD;

namespace S3_Passion.BOE_Lovemaking
{
    [Persistable]
    public class Part
    {
        public int MaxSims;

        public int MinSims;

        public long LastSwitch;

        public PartArea Area;

        public Vector3 Forward;

        public Vector3 Location;

        public string CurrentPositionKey;

        public SequenceInstance CurrentSequence;

        public static bool BroWeAreSwitching = false;

        public static bool HahaFirst = false;

        public PassionTarget Target;

        public Player Initiator;

        public VisualEffect VisualEffect;

        public ObjectSound SoundEffect;

        protected SafeDictionary<ulong, Player> mPlayers;

        protected bool mPositionChanged = false;

        public SafeDictionary<ulong, Player> Players
        {
            get
            {
                if (mPlayers == null)
                {
                    mPlayers = new SafeDictionary<ulong, Player>();
                }
                return mPlayers;
            }
        }

        public Position Position
        {
            get
            {
                return PassionBase.GetPosition(CurrentPositionKey);
            }
        }

        public PassionType Type
        {
            get
            {
                return Target != null ? Target.ObjectType : null;
            }
        }

        public bool PositionChanged
        {
            get
            {
                if (mPositionChanged)
                {
                    mPositionChanged = false;
                    return true;
                }
                return false;
            }
            set
            {
                mPositionChanged = value;
            }
        }

        public bool HasRoom
        {
            get
            {
                return Players.Count < MaxSims;
            }
        }

        public bool HasTarget
        {
            get
            {
                return Target != null;
            }
        }

        public bool HasPosition
        {
            get
            {
                return Position != null;
            }
        }

        public bool HasSequence
        {
            get
            {
                return CurrentSequence != null;
            }
        }

        public bool HasInitiator
        {
            get
            {
                return Initiator != null;
            }
        }

        public int Count
        {
            get
            {
                int num = 0;
                foreach (Player value in Players.Values)
                {
                    if (value != null && (value.State == PassionState.Ready || value.State == PassionState.Animating))
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public int Remaining
        {
            get
            {
                int num = 0;
                foreach (Player value in Players.Values)
                {
                    if (value != null && value.State != PassionState.Stopping && value.State != PassionState.Leaving)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public bool IsAutonomous
        {
            get
            {
                if (Players.Count > 0)
                {
                    foreach (Player value in Players.Values)
                    {
                        if (!value.IsAutonomous)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        public bool IsOccupied
        {
            get
            {
                if (Players.Count > 0)
                {
                    return true;
                }
                if (HasTarget && Target.IsValid && Target.HasObject)
                {
                    if (Type.NeedsParts)
                    {
                        PartData part;
                        PartData part2;
                        GetPartsForTarget(Target.Object, Area, out part, out part2);
                        if ((part != null || part2 != null) && (part != null && part.ContainedSim != null || part2 != null && part2.ContainedSim != null))
                        {
                            return true;
                        }
                    }
                    if (Type.NeedsUseList && Target.Object.UseCount > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static Part Create(GameObject obj)
        {
            return Create(obj, PartArea.Middle);
        }

        public static Part Create(GameObject obj, PartArea area)
        {
            if (obj != null)
            {
                return Create(area, obj.Position, obj.ForwardVector);
            }
            return Create(area, Vector3.Empty, Vector3.Empty);
        }

        public static Part Create(Vector3 location, Vector3 forward)
        {
            return Create(PartArea.Middle, location, forward);
        }

        public static Part Create(PartArea area, Vector3 location, Vector3 forward)
        {
            Part part = new Part();
            part.Area = area;
            part.Forward = forward;
            part.Location = location;
            part.CurrentSequence = null;
            part.CurrentPositionKey = string.Empty;
            return part;
        }

        public Part()
        {
            Area = PartArea.Middle;
            Forward = Vector3.Empty;
            Location = Vector3.Empty;
        }

        public bool IsOccupiedByOtherSims(Sim excluded)
        {
            if (Players.Count > 0)
            {
                return true;
            }
            if (HasTarget && Target.IsValid && Target.HasObject)
            {
                if (Type.NeedsParts)
                {
                    PartData part;
                    PartData part2;
                    GetPartsForTarget(Target.Object, Area, out part, out part2);
                    if ((part != null || part2 != null) && (part != null && part.ContainedSim != null && part.ContainedSim != excluded || part2 != null && part2.ContainedSim != null && part2.ContainedSim != excluded))
                    {
                        return true;
                    }
                }
                if (Type.NeedsUseList && Target.Object.UseCount > 0)
                {
                    if (Target.Object.UseCount == 1 && Target.Object.ActorsUsingMe.Contains(excluded))
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public void GetSexCharacteristics(out int penises, out int vaginas)
        {
            penises = 0;
            vaginas = 0;
            foreach (Player value in Players.Values)
            {
                if (value != null)
                {
                    if (value.SimGenitalType == "penis" || value.SimGenitalType == "both")
                    {
                        penises++;
                    }
                    if (value.SimGenitalType == "vagina" || value.SimGenitalType == "both")
                    {
                        vaginas++;
                    }
                }
            }
        }

        public bool CheckActiveParticipants()
        {
            foreach (Player duder in Players.Values)
            {
                if (duder != null)
                {
                    if (duder.Actor.IsInActiveHousehold)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void GetPartsForTarget(GameObject target, PartArea target_part, out PartData part1, out PartData part2)
        {
            part1 = null;
            part2 = null;
            if (target == null || target.PartComponent == null)
            {
                return;
            }
            foreach (PartData part3 in target.PartComponent.GetParts())
            {
                if ((target is ChairDining || target is Toilet) && part3.Area == PartArea.Middle)
                {
                    part1 = part3;
                    break;
                }
                if (target is ChairLiving && part3.Area == PartArea.SitMiddle)
                {
                    part1 = part3;
                    break;
                }
                if (target is ShowerTub && part3.Area == PartArea.SitRight)
                {
                    part1 = part3;
                    break;
                }
                if (target is BedDouble)
                {
                    if (part3.Area == PartArea.Left && target_part != PartArea.Right)
                    {
                        part1 = part3;
                        if (part2 != null)
                        {
                            break;
                        }
                    }
                    else if (part3.Area == PartArea.Right && target_part != 0)
                    {
                        part2 = part3;
                        if (part1 != null)
                        {
                            break;
                        }
                    }
                }
                else if (target is BedSingle || target is BunkBedUpper || target is BunkBedLower)
                {
                    if (part3.Area == PartArea.Left)
                    {
                        part1 = part3;
                        if (part2 != null)
                        {
                            break;
                        }
                    }
                    else if (part3.Area == PartArea.Right)
                    {
                        part2 = part3;
                        if (part1 != null)
                        {
                            break;
                        }
                    }
                }
                else if (target is Loveseat)
                {
                    if (part3.Area == PartArea.SitMiddle)
                    {
                        part1 = part3;
                        if (part2 != null)
                        {
                            break;
                        }
                    }
                    else if (part3.Area == PartArea.SitRight)
                    {
                        part2 = part3;
                        if (part1 != null)
                        {
                            break;
                        }
                    }
                }
                else if (target is Couch)
                {
                    if (part3.Area == PartArea.SitLeft)
                    {
                        part1 = part3;
                        if (part2 != null)
                        {
                            break;
                        }
                    }
                    else if (part3.Area == PartArea.SitMiddle)
                    {
                        part2 = part3;
                        if (part1 != null)
                        {
                            break;
                        }
                    }
                }
                else if (target is HotTub4Seated)
                {
                    if (target_part == PartArea.Top)
                    {
                        if (part3.Area == PartArea.Part_0)
                        {
                            part1 = part3;
                            if (part2 != null)
                            {
                                break;
                            }
                        }
                        else if (part3.Area == PartArea.Part_1)
                        {
                            part2 = part3;
                            if (part1 != null)
                            {
                                break;
                            }
                        }
                    }
                    if (target_part != PartArea.Bottom)
                    {
                        continue;
                    }
                    if (part3.Area == PartArea.Part_2)
                    {
                        part1 = part3;
                        if (part2 != null)
                        {
                            break;
                        }
                    }
                    else if (part3.Area == PartArea.Part_3)
                    {
                        part2 = part3;
                        if (part1 != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if (!(target is HotTubGrotto))
                    {
                        continue;
                    }
                    if (target_part == PartArea.Left)
                    {
                        if (part3.Area == PartArea.Part_0)
                        {
                            part1 = part3;
                            if (part2 != null)
                            {
                                break;
                            }
                        }
                        else if (part3.Area == PartArea.Part_1)
                        {
                            part2 = part3;
                            if (part1 != null)
                            {
                                break;
                            }
                        }
                    }
                    if (target_part == PartArea.Middle)
                    {
                        if (part3.Area == PartArea.Part_2)
                        {
                            part1 = part3;
                            if (part2 != null)
                            {
                                break;
                            }
                        }
                        else if (part3.Area == PartArea.Part_3)
                        {
                            part2 = part3;
                            if (part1 != null)
                            {
                                break;
                            }
                        }
                    }
                    if (target_part != PartArea.Right)
                    {
                        continue;
                    }
                    if (part3.Area == PartArea.Part_4)
                    {
                        part1 = part3;
                        if (part2 != null)
                        {
                            break;
                        }
                    }
                    else if (part3.Area == PartArea.Part_5)
                    {
                        part2 = part3;
                        if (part1 != null)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void Reserve(Player player)
        {
            Initiator = player;
            Reserve();
        }

        public void Reserve()
        {
            if (!HasTarget || !Target.HasObject || !HasInitiator || !Initiator.IsValid)
            {
                return;
            }
            Target.DisablePassthrough(Initiator);
            if (Type.NeedsUseList)
            {
                Target.Object.AddToUseList(Initiator.Actor);
            }
            if (Type.NeedsParts)
            {
                PartData part;
                PartData part2;
                GetPartsForTarget(Target.Object, Area, out part, out part2);
                if (part != null && part.ContainedSim == null)
                {
                    part.SetContainedSim(Initiator.Actor, Target.Object.PartComponent);
                }
                if (part2 != null && part2.ContainedSim == null)
                {
                    part2.SetContainedSim(Initiator.Actor, Target.Object.PartComponent);
                }
            }
        }

        public void Free()
        {
            if (!HasTarget || !Target.HasObject || !HasInitiator || !Initiator.IsValid)
            {
                return;
            }
            Target.EnablePassthrough();
            if (Type.NeedsUseList)
            {
                Target.Object.RemoveFromUseList(Initiator.Actor);
            }
            if (Type.NeedsParts)
            {
                PartData part;
                PartData part2;
                GetPartsForTarget(Target.Object, Area, out part, out part2);
                if (part != null)
                {
                    part.SetContainedSim(null, Target.Object.PartComponent);
                }
                if (part2 != null)
                {
                    part2.SetContainedSim(null, Target.Object.PartComponent);
                }
            }
        }

        public void RefreshMinMaxSims()
        {
            Position.GetMinMaxSims(Type, out MinSims, out MaxSims);
        }

        public bool RefreshInitiator()
        {
            if (HasInitiator)
            {
                Initiator.StopJealousyBroadcast();
                Free();
            }
            Initiator = null;
            foreach (Player value in Players.Values)
            {
                if (value != null && (value.State == PassionState.Ready || value.State == PassionState.Animating || value.State == PassionState.Routing && value.GetDistanceTo(Target as IGameObject) < 5f))
                {
                    Initiator = value;
                }
            }
            if (!HasInitiator)
            {
                foreach (Player value2 in Players.Values)
                {
                    if (value2 != null && value2.State != PassionState.Stopping && value2.State != PassionState.Leaving && value2.State != PassionState.Deny)
                    {
                        Initiator = value2;
                    }
                }
            }
            if (HasInitiator)
            {
                Reserve();
                return true;
            }
            return false;
        }

        public void SwapInitiator(Player player)
        {
            if (HasInitiator)
            {
                Initiator.StopJealousyBroadcast();
                Free();
            }
            Reserve(player);
        }


        public void GetRandomValidPosition()
        {
            bool flag = PassionCommon.Match(PersistableSettings.Settings.RandomizationOptions, RandomizationOptions.SameCategory);
            int num = PersistableSettings.Settings.InitialCategory != 1024 ? PersistableSettings.Settings.InitialCategory : 12;
            int num2 = HasPosition ? Position.Categories : num;
            int penises = 0;
            int vaginas = 0;
            IPositionChoice choice = null;
            GetSexCharacteristics(out penises, out vaginas);
            if (BroWeAreSwitching == true)
            {
                choice = Position.GetRandomValidPosition(Type, Count, penises, vaginas, flag ? num2 : num, true);
            }
            else
            {
                bool isActiveHousehold = CheckActiveParticipants();

                if (HahaFirst && isActiveHousehold)
                {
                    if (PersistableSettings.Settings.ExcludeInvalidPositions)
                    {
                        GetSexCharacteristics(out penises, out vaginas);
                    }
                    choice = Position.ChoosePositionDialog(Type, Count, penises, vaginas);
                    HahaFirst = false;
                }
                else
                {
                    choice = Position.GetRandomValidPosition(Type, Count, penises, vaginas, flag ? num2 : num, false);
                    if (choice == null && !flag)
                    {
                        choice = Position.GetRandomValidPosition(Type, Count, penises, vaginas);
                    }
                }
                if (choice != null)
                {
                    SmartSetPosition(choice);
                }
                else
                {
                    StopAllPlayers();
                }
            }

        }

        public bool Add(Player player)
        {
            if (player != null && player.IsValid)
            {
                if (HasTarget && (!HasInitiator || Remaining < 1))
                {
                    Reserve(player);
                }
                if (Players.ContainsKey(player.ID))
                {
                    Players[player.ID] = player;
                }
                else
                {
                    Players.Add(player.ID, player);
                }
                player.Part = this;
                return true;
            }
            return false;
        }

        public bool SmartSetPosition(IPositionChoice choice)
        {
            if (choice is Sequence)
            {
                SetPosition(choice as Sequence);
            }
            else
            {
                if (!(choice is Position))
                {
                    return false;
                }
                SetPosition(choice as Position);
            }
            return true;
        }

        public void SetPosition(Sequence sequence)
        {
            if (sequence != null)
            {
                mPositionChanged = true;
                CurrentSequence = SequenceInstance.Create(sequence);
                Position first = CurrentSequence.First;
                if (first != null)
                {
                    CurrentPositionKey = first.Key;
                    LastSwitch = SimClock.CurrentTicks;
                    Sort();
                }
            }
            else
            {
                CurrentSequence = null;
            }
        }

        public void SetPosition(Position position)
        {
            SetPosition(position, false);
        }

        public void SetPosition(Position position, bool keepsequence)
        {
            if (position != null && position != Position || HasSequence)
            {
                mPositionChanged = true;
                if (!keepsequence)
                {
                    CurrentSequence = null;
                }
                CurrentPositionKey = position.Key;
                LastSwitch = SimClock.CurrentTicks;
                Sort();
            }
        }

        public bool ChangePosition()
        {
            int penises = 0;
            int vaginas = 0;
            if (PersistableSettings.Settings.ExcludeInvalidPositions)
            {
                GetSexCharacteristics(out penises, out vaginas);
            }
            IPositionChoice choice = Position.ChoosePositionDialog(Type, Count, penises, vaginas);
            return SmartSetPosition(choice);
        }

        public void Sort()
        {
            if (!HasPosition)
            {
                return;
            }
            int count = Count;
            List<Player> values = Players.Values;
            Position.Animation.Set set = Position.GetSet(count);
            if (set != null)
            {
                try
                {
                    for (int i = 1; i <= count; i++)
                    {
                        Player player = null;
                        foreach (Player item in values)
                        {
                            if (item.IsStopping)
                            {
                                continue;
                            }
                            Position.Animation.Slot slot = set != null ? set.GetSlot(i) : null;
                            if (slot != null && slot.NeedsPenis)
                            {
                                if (item.SimGenitalType == "penis" || item.SimGenitalType == "both")
                                {
                                    if (item.PositionIndex == i)
                                    {
                                        player = item;
                                        break;
                                    }
                                    if (player == null || player.SimGenitalType != "penis")
                                    {
                                        player = item;
                                    }
                                }
                                else if (player == null || item.PositionIndex == i && player.SimGenitalType != "penis")
                                {
                                    player = item;
                                }
                            }
                            else if (slot != null && slot.NeedsVagina)
                            {
                                if (item.SimGenitalType == "vagina" || item.SimGenitalType == "vagina")
                                {
                                    if (item.PositionIndex == i)
                                    {
                                        player = item;
                                        break;
                                    }
                                    if (player == null || player.SimGenitalType == "penis")
                                    {
                                        player = item;
                                    }
                                }
                                else if (player == null || item.PositionIndex == i && player.SimGenitalType == "penis")

                                {
                                    player = item;
                                }
                            }
                            else
                            {
                                player = item;
                            }
                        }
                        if (player != null)
                        {
                            player.PositionIndex = i;
                            values.Remove(player);
                        }
                    }
                }
                catch
                {
                    PassionCommon.SystemMessage("Attempted to Sort(), but encountered an error.");
                }
            }
            else if (PassionCommon.Testing)
            {
                PassionCommon.SystemMessage("Attempted to Sort(), but Set was null.");
            }
            UpdateHandling();
            if (BrokenCondom())
            {
                CheckPregnancy();
            }
        }

        public bool CheckSequence()
        {
            if (HasSequence)
            {
                if (!CurrentSequence.Started && CurrentSequence.First != null)
                {
                    SetPosition(CurrentSequence.Start(), true);
                    return true;
                }
                Position position = CurrentSequence.Next();
                if (position == Position)
                {
                    return true;
                }
                if (position != null)
                {
                    SetPosition(position, true);
                    return true;
                }
                if (CurrentSequence.Continue)
                {
                    CurrentPositionInvalidate();
                    return true;
                }
                StopAllPlayers();
                return false;
            }
            if (PersistableSettings.Settings.RandomizationLength > 0)
            {
                if (LastSwitch == 0)
                {
                    LastSwitch = SimClock.CurrentTicks;
                }
                else if (LastSwitch + PersistableSettings.Settings.RandomizationLength < SimClock.CurrentTicks)
                {
                    GetRandomValidPosition();
                }
            }
            return true;
        }

        public void SortAndGetPosition()
        {

            if (PersistableSettings.Settings.InitialCategory == 1024)
            {
                bool flag = true;
                foreach (Player value in Players.Values)
                {
                    if (value.State != PassionState.Stopping && !value.IsAutonomous)
                    {
                        flag = false;
                    }
                }
                if (flag || !ChangePosition())
                {
                    GetRandomValidPosition();
                }
            }
            else
            {
                GetRandomValidPosition();
            }
        }

        public void CurrentPositionInvalidate()
        {
            CurrentPositionKey = string.Empty;
            mPositionChanged = true;
        }

        public bool CanAnimate(out List<Player> ready)
        {
            bool flag = true;
            ready = new List<Player>();
            foreach (Player value in Players.Values)
            {
                if (!value.IsValid)
                {
                    continue;
                }
                if (value.IsInMotiveDesperation)
                {
                    value.Stop(ExitReason.MoodFailure);
                }
                else if (value.IsTimedOut || value.IsFull)
                {
                    value.Stop(ExitReason.Finished);
                }
                else if (value.State == PassionState.Ready)
                {
                    if (!value.IsInPlace)
                    {
                        UpdateLocation(value);
                    }
                    ready.Add(value);
                }
                else if (value.State == PassionState.Animating || value.State == PassionState.Routing && value.GetDistanceTo(Location) < 5f)
                {
                    flag = false;
                }
            }
            if (flag && ready.Count >= MinSims)
            {
                if (!HasPosition)
                {
                    SortAndGetPosition();
                }
                return true;
            }
            return false;
        }

        public void UpdateLocation(Player player)
        {
            if (player == null || !player.IsValid)
            {
                return;
            }
            Vector3 forward = Forward;
            Vector3 location = Location;
            if (HasPosition)
            {
                Position.Animation.ClipData clip = Position.GetClip(player);
                if (clip == null)
                {
                }
            }
            if (player.HeightModifier != 0f)
            {
                location.y += 0.115f;
            }
            else
            {
                location.y += player.HeightModifier;
            }
            player.Location = location;
            player.Forward = forward;
            player.IsInPlace = true;
        }

        public void Remove(Player player)
        {
            if (player == null || !player.IsValid)
            {
                return;
            }
            if (Players.ContainsKey(player.ID))
            {
                Players.Remove(player.ID);
            }
            if (player == Initiator && Players.Count > 0)
            {
                RefreshInitiator();
            }
            player.PositionIndex = 0;
            player.Part = null;
            if (HasTarget && Target.HasObject && Players.Count < 1)
            {
                Free();
                StopVisualEffects();
                StopSoundEffects();
            }
            int penises = 0;
            int vaginas = 0;
            GetSexCharacteristics(out penises, out vaginas);
            if (HasPosition && Position.CanUseWith(Type, Players.Count, penises, vaginas, 1))
            {
                foreach (Player value in Players.Values)
                {
                    if (value != null && value.IsActive)
                    {
                        mPositionChanged = true;
                        value.BufferedAnimation = Position.GetAnimation(value);
                    }
                }
                return;
            }
            CurrentPositionInvalidate();
        }

        public void StartVisualEffects()
        {
            if (!HasTarget || !Target.HasObject || Target.ObjectType == null)
            {
                return;
            }
            try
            {
                if (Target.ObjectType.Is<Shower>() || Target.ObjectType.Is<ShowerOutdoor>() || Target.ObjectType.Is<ShowerPublic_Dance>())
                {
                    VisualEffect = VisualEffect.Create("showerfx");
                    VisualEffect.ParentTo(Target.Object, Slot.FXJoint_0);
                    VisualEffect.Start();
                }
                else if (Target.ObjectType.Is<ShowerTub>())
                {
                    VisualEffect = VisualEffect.Create("bathtubshowerfx");
                    VisualEffect.ParentTo(Target.Object, Slot.FXJoint_2);
                    VisualEffect.Start();
                }
                else if (Target.ObjectType.Is<Altar>() && Target.Object.GetResourceKey().ToString() == "319e4f1d:28000000:000000000098a21e")
                {
                    VisualEffect = VisualEffect.Create("ep3alterbedcandlesburning");
                    VisualEffect.ParentTo(Target.Object, Slot.FXJoint_0);
                    VisualEffect.Start();
                }
            }
            catch
            {
            }
        }

        public void StopVisualEffects()
        {
            if (VisualEffect != null)
            {
                VisualEffect.Stop();
                VisualEffect = null;
            }
        }

        public void StartSoundEffects()
        {
            if (!HasTarget || !Target.HasObject || Target.ObjectType == null)
            {
                return;
            }
            try
            {
                if (Target.ObjectType.Type == typeof(Shower) || Target.ObjectType.Type == typeof(ShowerTub) || Target.ObjectType.Type == typeof(ShowerPublic_Dance))
                {
                    SoundEffect = new ObjectSound(Target.Object.ObjectId, "shower_running_lp");
                    SoundEffect.StartLoop();
                }
            }
            catch
            {
            }
        }

        public void StopSoundEffects()
        {
            if (SoundEffect != null)
            {
                SoundEffect.Stop();
                SoundEffect.Dispose();
                SoundEffect = null;
            }
        }

        public void StopAllPlayers()
        {
            foreach (Player value in Players.Values)
            {
                value.ActiveLeaveJoin = false;
                value.Stop();
            }
        }

        // condom breaks processing
        public bool BrokenCondom()
        {
            try
            {
                ResourceKey key = ResourceKey.FromString("0x319E4F1D-0x00000000-0x000000004F57910E");
                ResourceKey key2 = ResourceKey.FromString("0x319E4F1D-0x00000000-0x000000007FBC752D");
                if (PersistableSettings.Settings.UseCondom && PersistableSettings.Settings.CondomBrakeChance > 0 && HasPosition && World.ResourceExists(key) && World.ResourceExists(key2))
                {
                    foreach (Player value in Players.Values)
                    {
                        if (!value.IsValid || !value.IsActive || value.IsStopping || PersistableSettings.Settings.CondomIsBroken)
                        {
                            continue;
                        }
                        if (value.SimGenitalType == "penis" && !value.Actor.Inventory.ContainsType(typeof(CondomPack), 1) && !value.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !PersistableSettings.Settings.PassionFuckSession)
                        {
                            PersistableSettings.Settings.PassionFuckSession = true;
                            try
                            {
                                PassionCommon.SimMessage(PassionCommon.Localize("Fuck! I don't have any condoms!").ToString(), value.Actor);
                                PassionCommon.Wait();
                            }
                            catch
                            {
                            }
                            PersistableSettings.Settings.CondomIsBroken = true;
                            return true;
                        }
                        if (value.SimGenitalType == "penis" && value.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !PersistableSettings.Settings.PassionFuckSession)
                        {
                            PersistableSettings.Settings.PassionFuckSession = true;
                            foreach (SingleCondom item in value.Actor.Inventory.FindAll<SingleCondom>(true))
                            {
                                if (item != null)
                                {
                                    value.Actor.Inventory.RemoveByForce(item);
                                    item.Destroy();
                                    break;
                                }
                            }
                        }
                        else if (value.SimGenitalType == "penis" && value.Actor.Inventory.ContainsType(typeof(CondomPack), 1) && !value.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !PersistableSettings.Settings.PassionFuckSession)
                        {
                            PersistableSettings.Settings.PassionFuckSession = true;
                            foreach (CondomPack item2 in value.Actor.Inventory.FindAll<CondomPack>(true))
                            {
                                if (item2 != null)
                                {
                                    value.Actor.Inventory.RemoveByForce(item2);
                                    item2.Destroy();
                                    break;
                                }
                            }
                            IGameObject obj2 = GlobalFunctions.CreateObjectOutOfWorld(key2);
                            IGameObject obj3 = GlobalFunctions.CreateObjectOutOfWorld(key2);
                            value.Actor.TryAddObjectToInventory(obj2);
                            value.Actor.TryAddObjectToInventory(obj3);
                        }
                        foreach (Player value2 in Players.Values)
                        {
                            if (!value2.IsActive || value2.Actor == value.Actor)
                            {
                                continue;
                            }
                            if (value2.SimGenitalType == "penis" && !value2.Actor.Inventory.ContainsType(typeof(CondomPack), 1) && !value2.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !PersistableSettings.Settings.PassionFuckSession)
                            {
                                PersistableSettings.Settings.PassionFuckSession = true;
                                try
                                {
                                    PassionCommon.SimMessage(PassionCommon.Localize("Fuck! I don't have any condoms!").ToString(), value2.Actor);
                                    PassionCommon.Wait();
                                }
                                catch
                                {
                                }
                                PersistableSettings.Settings.CondomIsBroken = true;
                                return true;
                            }
                            if (value2.SimGenitalType == "penis" && value2.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !PersistableSettings.Settings.PassionFuckSession)
                            {
                                PersistableSettings.Settings.PassionFuckSession = true;
                                foreach (SingleCondom item3 in value2.Actor.Inventory.FindAll<SingleCondom>(true))
                                {
                                    if (item3 != null)
                                    {
                                        value2.Actor.Inventory.RemoveByForce(item3);
                                        item3.Destroy();
                                        break;
                                    }
                                }
                            }
                            else if (value2.SimGenitalType == "penis" && value2.Actor.Inventory.ContainsType(typeof(CondomPack), 1) && !value2.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !PersistableSettings.Settings.PassionFuckSession)
                            {
                                PersistableSettings.Settings.PassionFuckSession = true;
                                foreach (CondomPack item4 in value2.Actor.Inventory.FindAll<CondomPack>(true))
                                {
                                    if (item4 != null)
                                    {
                                        value2.Actor.Inventory.RemoveByForce(item4);
                                        item4.Destroy();
                                        break;
                                    }
                                }
                                IGameObject obj5 = GlobalFunctions.CreateObjectOutOfWorld(key2);
                                IGameObject obj6 = GlobalFunctions.CreateObjectOutOfWorld(key2);
                                value2.Actor.TryAddObjectToInventory(obj5);
                                value2.Actor.TryAddObjectToInventory(obj6);
                            }
                            try
                            {
                                if ((value2.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 4) || value2.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 8) || value2.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 12)) && RandomUtil.RandomChance(PersistableSettings.Settings.CondomBrakeChance))
                                {
                                    try
                                    {
                                        PassionCommon.SimMessage(PassionCommon.Localize("Shit! Condom broke!").ToString(), value2.Actor);
                                        PassionCommon.Wait();
                                    }
                                    catch
                                    {
                                    }
                                    PersistableSettings.Settings.CondomIsBroken = true;
                                    return true;
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if ((value.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 4) || value.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 8) || value.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 12)) && RandomUtil.RandomChance(PersistableSettings.Settings.CondomBrakeChance))
                                {
                                    try
                                    {
                                        PassionCommon.SimMessage(PassionCommon.Localize("Shit! Condom broke!").ToString(), value.Actor);
                                        PassionCommon.Wait();
                                    }
                                    catch
                                    {
                                    }
                                    PersistableSettings.Settings.CondomIsBroken = true;
                                    return true;
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else if (!PersistableSettings.Settings.UseCondom || World.ResourceExists(key) || World.ResourceExists(key2))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public void CheckPregnancy()
        {
            try
            {
                if (PersistableSettings.Settings.PregnancyMethod == PregnancyMethod.Disabled || PersistableSettings.Settings.PregnancyRisk <= 0 || !HasPosition)
                {
                    return;
                }
                foreach (Player value in Players.Values)
                {
                    if (!value.IsValid || !value.IsActive || value.IsStopping || value.Actor.SimDescription.IsPregnant || !value.Actor.IsFemale && !PersistableSettings.Settings.PregnancyMale)
                    {
                        continue;
                    }
                    foreach (Player value2 in Players.Values)
                    {
                        if (!value2.IsActive || value2.Actor == value.Actor || value2.SimGenitalType != "penis" && (PersistableSettings.Settings.PregnancyMethod != PregnancyMethod.ByPosition || !Position.PossibleSameSexPregnancy) && PersistableSettings.Settings.PregnancyMethod != PregnancyMethod.KWSystem || (PersistableSettings.Settings.PregnancyMethod != PregnancyMethod.ByCategory || !PassionCommon.Match(Position.Categories, 12)) && (PersistableSettings.Settings.PregnancyMethod != PregnancyMethod.ByPosition || !Position.PossiblePregnancy) && (PersistableSettings.Settings.PregnancyMethod != PregnancyMethod.KWSystem || !value.Actor.IsFemale || !PassionCommon.Match(Position.Categories, 4)) && (PersistableSettings.Settings.PregnancyMethod != PregnancyMethod.KWSystem || !value.Actor.IsMale || !PassionCommon.Match(Position.Categories, 8)) && (PersistableSettings.Settings.PregnancyMethod != PregnancyMethod.KWSystem || !PassionCommon.Match(Position.Categories, 12)))
                        {
                            continue;
                        }
                        if (RandomUtil.RandomChance(PersistableSettings.Settings.PregnancyRisk) && !value.Actor.SimDescription.IsPregnant && !value2.Actor.SimDescription.IsPregnant && value.Actor.SimDescription.IsFemale && value2.Actor.SimDescription.IsMale)
                        {
                            PassionCommon.Impregnate(value.Actor, value2.Actor);
                            break;
                        }
                        if (RandomUtil.RandomChance(PersistableSettings.Settings.PregnancyRisk) && !value2.Actor.SimDescription.IsPregnant && value.Actor.SimDescription.IsMale && value2.Actor.SimDescription.IsFemale)
                        {
                            PassionCommon.Impregnate(value2.Actor, value.Actor);
                            break;
                        }
                        if (RandomUtil.RandomChance(PersistableSettings.Settings.PregnancyRisk) && !value.Actor.SimDescription.IsPregnant && !value2.Actor.SimDescription.IsPregnant && value.Actor.SimDescription.IsFemale && value.SimGenitalType != "penis" && value2.Actor.SimDescription.IsFemale && value2.SimGenitalType != "penis" || RandomUtil.RandomChance(PersistableSettings.Settings.PregnancyRisk) && !value.Actor.SimDescription.IsPregnant && !value2.Actor.SimDescription.IsPregnant && value.Actor.SimDescription.IsFemale && value.SimGenitalType != "penis" && value2.Actor.SimDescription.IsFemale && value2.SimGenitalType == "penis")
                        {
                            if (PersistableSettings.Settings.PregnancyMale)
                            {
                                PassionCommon.Impregnate(value.Actor, value2.Actor);
                                break;
                            }
                        }
                        else if ((RandomUtil.RandomChance(PersistableSettings.Settings.PregnancyRisk) && value.Actor.SimDescription.IsMale && value2.Actor.SimDescription.IsFemale && value2.SimGenitalType == "penis" || RandomUtil.RandomChance(PersistableSettings.Settings.PregnancyRisk) && PersistableSettings.Settings.PregnancyMale && value.Actor.SimDescription.IsMale && value2.Actor.SimDescription.IsMale) && PersistableSettings.Settings.PregnancyMale)
                        {
                            PassionCommon.Impregnate(value.Actor, value2.Actor);
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public void UpdateHandling()
        {
            foreach (Player value in Players.Values)
            {
                if (value.IsStopping)
                {
                    continue;
                }
                if (Count > 1 && !value.HadPartner)
                {
                    foreach (Player value2 in Players.Values)
                    {
                        if (value2.IsValid && value2.IsActive && value2 != value && !value2.IsStopping)
                        {
                            value.Partner = value2;
                            break;
                        }
                    }
                }
                Position.Animation.ClipData clipData = null;
                Position.Animation.Set set = Position.GetSet(Count);
                try
                {
                    if (set != null && value != null)
                    {
                        if (value.IsInitiator)
                        {
                            value.BufferedTargetAnimation = set.TargetAnimation;
                        }
                        clipData = set.GetClip(value);
                    }
                    else
                    {
                        clipData = Position.GetClip(value);
                    }
                }
                catch
                {
                    PassionCommon.SystemMessage("Attempted to set Player Animations, but encountered an error.");
                }
                value.BufferedAnimation = clipData != null ? clipData.Clip : string.Empty;
                value.RecalculateMotiveUpdates();
                if (!PersistableSettings.Settings.UseCondom && PersistableSettings.Settings.STD)
                {
                    STD.Process(value.Actor);
                }
                else if (PersistableSettings.Settings.UseCondom && PersistableSettings.Settings.STD && PersistableSettings.Settings.CondomIsBroken)
                {
                    STD.Process(value.Actor);
                }
                if (PersistableSettings.Settings.Jealousy && value.IsInitiator)
                {
                    value.StartJealousyBroadcast();
                }
            }
        }
    }
}
