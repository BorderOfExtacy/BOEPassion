using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.Counters;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Objects.Vehicles;
using Sims3.SimIFace;
using Sims3.Store.Objects;
using Sims3.UI;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Objects;

namespace S3_Passion.BOE_Lovemaking
{
    [Persistable]
    public class PassionTarget
    {
        public const int MaxMinInvalid = 0;

        public GameObject Object;

        public PassionType ObjectType;

        public Vector3 Location = Vector3.Empty;

        public Vector3 Forward = Vector3.Empty;

        public bool PassthroughDisabled = false;

        [PersistableStatic]
        private static Dictionary<PassionType, int> mMinSimsList;

        [PersistableStatic]
        private static Dictionary<PassionType, int> mMaxSimsList;

        protected Dictionary<PartArea, Part> mParts;

        public static Dictionary<PassionType, int> MinSimsList
        {
            get
            {
                if (mMinSimsList == null)
                {
                    mMinSimsList = new Dictionary<PassionType, int>();
                }
                return mMinSimsList;
            }
        }

        public int MinSims
        {
            get
            {
                return GetMinSims(ObjectType);
            }
        }

        public static Dictionary<PassionType, int> MaxSimsList
        {
            get
            {
                if (mMaxSimsList == null)
                {
                    mMaxSimsList = new Dictionary<PassionType, int>();
                }
                return mMaxSimsList;
            }
        }

        public int MaxSims
        {
            get
            {
                return GetMaxSims(ObjectType);
            }
        }

        public Dictionary<PartArea, Part> Parts
        {
            get
            {
                if (mParts == null)
                {
                    mParts = new Dictionary<PartArea, Part>();
                }
                return mParts;
            }
        }

        public Part this[PartArea area]
        {
            get
            {
                if (Parts.ContainsKey(area) && Parts[area] != null)
                {
                    return Parts[area];
                }
                return null;
            }
        }

        public bool HasObject
        {
            get
            {
                return Object != null;
            }
        }

        public bool IsValid
        {
            get
            {
                return ObjectType != null && Location != Vector3.Empty;
            }
        }

        public int Count
        {
            get
            {
                int num = 0;
                foreach (Part value in Parts.Values)
                {
                    num += value.Players.Count;
                }
                return num;
            }
        }

        public int Remaining
        {
            get
            {
                int num = 0;
                foreach (Part value in Parts.Values)
                {
                    num += value.Remaining;
                }
                return num;
            }
        }

        public bool IsOccupied
        {
            get
            {
                if (HasObject)
                {
                    foreach (Part value in Parts.Values)
                    {
                        if (!value.IsOccupied)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        public static PassionTarget Create(GameObject obj)
        {
            PassionTarget target = new PassionTarget();
            if (obj != null)
            {
                target.Object = obj;
                target.ObjectType = PassionType.GetSupportedType(obj);
                target.Location = obj.Position;
                target.Forward = obj.ForwardVector;
                target.CreateParts();
            }
            return target;
        }

        public static PassionTarget Create(PassionType type, Vector3 location, Vector3 forward)
        {
            PassionTarget target = new PassionTarget();
            target.ObjectType = type;
            target.Location = location;
            target.Forward = forward;
            target.CreateParts();
            return target;
        }

        public static void Unload()
        {
            mMinSimsList.Clear();
            mMinSimsList = null;
            mMaxSimsList.Clear();
            mMaxSimsList = null;
        }

        public static int GetMinSims(PassionType type)
        {
            if (MinSimsList.ContainsKey(type))
            {
                return MinSimsList[type];
            }
            return 0;
        }

        public static int GetMaxSims(PassionType type)
        {
            if (MaxSimsList.ContainsKey(type))
            {
                return MaxSimsList[type];
            }
            return 0;
        }

        public static void RegisterMinMaxSims(Position position)
        {
            foreach (PassionType value in position.SupportedTypes.Values)
            {
                if (!MinSimsList.ContainsKey(value))
                {
                    MinSimsList.Add(value, position.MinSims);
                }
                else if (position.MinSims < MinSimsList[value])
                {
                    MinSimsList[value] = position.MinSims;
                }
                if (!MaxSimsList.ContainsKey(value))
                {
                    MaxSimsList.Add(value, position.MaxSims);
                }
                else if (position.MaxSims > MaxSimsList[value])
                {
                    MaxSimsList[value] = position.MaxSims;
                }
            }
        }

        public static void ClearMinMaxSims()
        {
            mMinSimsList.Clear();
            mMinSimsList = null;
            mMaxSimsList.Clear();
            mMaxSimsList = null;
        }

        public bool IsOccupiedByOtherSims(Sim exclude)
        {
            if (HasObject)
            {
                foreach (Part value in Parts.Values)
                {
                    if (!value.IsOccupiedByOtherSims(exclude))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public Part CreatePart(PartArea area, Vector3 location, Vector3 forward)
        {
            return Part.Create(area, location, forward);
        }

        public Part CreatePart(PartArea area)
        {
            return Part.Create(area, Location, Forward);
        }

        public Part CreatePart()
        {
            return Part.Create(PartArea.Middle, Location, Forward);
        }

        public void CreateParts()
        {
            if (ObjectType == null || !ObjectType.IsValid)
            {
                return;
            }
            if (ObjectType.Is<BedDouble>())
            {
                try
                {
                    Part part = CreatePart(PartArea.Left);
                    Part part2 = CreatePart(PartArea.Middle);
                    Part part3 = CreatePart(PartArea.Right);
                    part.Location.Set(Location.x + Forward.z * 0f - Forward.z * 0.4f, Location.y, Location.z + Forward.z * 0f + Forward.x * 0.4f);
                    part3.Location.Set(Location.x + Forward.z * 0f - Forward.z * -0.4f, Location.y, Location.z + Forward.z * 0f + Forward.x * -0.4f);
                    AddPart(part);
                    AddPart(part2);
                    AddPart(part3);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<TableDining3x1>())
            {
                try
                {
                    Part part4 = CreatePart(PartArea.Left);
                    Part part5 = CreatePart(PartArea.Right);
                    part4.Location.Set(Location.x + Forward.z * 0f - Forward.z * 0.5f, Location.y, Location.z + Forward.z * 0f + Forward.x * 0.5f);
                    part5.Location.Set(Location.x + Forward.z * 0f - Forward.z * -0.5f, Location.y, Location.z + Forward.z * 0f + Forward.x * -0.5f);
                    AddPart(part4);
                    AddPart(part5);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<Sofa>())
            {
                try
                {
                    Part part6 = CreatePart();
                    part6.Location.Set(Location.x + Forward.z * 0f - Forward.z * 0.5f, Location.y, Location.z + Forward.z * 0f + Forward.x * 0.5f);
                    AddPart(part6);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<MassageTable>())
            {
                try
                {
                    Part part7 = CreatePart();
                    part7.Location.Set(Location.x + Forward.z * 0f - Forward.z * 0f, Location.y, Location.z + Forward.z * 0f + Forward.x * 0f);
                    AddPart(part7);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<HotTub4Seated>())
            {
                try
                {
                    Part part8 = CreatePart(PartArea.Top);
                    Part part9 = CreatePart(PartArea.Left, Location, -Forward);
                    AddPart(part8);
                    AddPart(part9);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<HotTubGrotto>())
            {
                try
                {
                    Part part10 = CreatePart(PartArea.Left);
                    Part part11 = CreatePart(PartArea.Middle);
                    Part part12 = CreatePart(PartArea.Right);
                    AddPart(part10);
                    AddPart(part11);
                    AddPart(part12);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<CornerBathtub>(false))
            {
                try
                {
                    Part part13 = CreatePart();
                    part13.Location.Set(Location.x + Forward.z * 0f - Forward.z * 0.5f, Location.y, Location.z + Forward.z * 0f + Forward.x * 0.5f);
                    AddPart(part13);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<CounterIsland>(false))
            {
                try
                {
                    Part part14 = CreatePart(PartArea.Middle, new Vector3(Location.x - Forward.x * 0.25f, Location.y, Location.z), -Forward);
                    AddPart(part14);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<SinkCounter>(false))
            {
                try
                {
                    Part part15 = CreatePart(PartArea.Middle);
                    float x = Location.x + Forward.z * 0f + Forward.x * 0.25f;
                    float num = Location.y / 1.0522f;
                    float num2 = Location.y - num;
                    float y = num + (num2 - 1.0335f);
                    float z = Location.z;
                    Vector3 location = new Vector3(x, y, z);
                    part15 = CreatePart(PartArea.Middle, location, Forward);
                    AddPart(part15);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<Sink>(false))
            {
                try
                {
                    Part part16 = CreatePart(PartArea.Middle, new Vector3(Location.x + Forward.z * 0f - Forward.x * 0.25f, Location.y, Location.z), Forward);
                    AddPart(part16);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<CarExpensive1>(false))
            {
                try
                {
                    Part part17 = CreatePart(PartArea.Middle, new Vector3(Location.x + Forward.z * 0f + Forward.x * 0.25f, Location.y, Location.z), Forward);
                    AddPart(part17);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<BoatSpeedBoat>() || ObjectType.Is<BoatSpeedFishingBoat>())
            {
                try
                {
                    float num3 = 1f + Forward.x;
                    float num4 = 1f + Forward.z;
                    float x2 = 0f;
                    float z2 = 0f;
                    float num5 = 0f;
                    float num6 = 0f;
                    float num7 = 0f;
                    float num8 = 0f;
                    float num9 = 0f;
                    Part part18 = CreatePart(PartArea.Middle);
                    if (num3 > 1f && num4 > 1f && num3 > num4 && Forward.x > Forward.z || num3 < 1f && num4 < 1f && num3 < num4 && 0f - Forward.x > 0f - Forward.z)
                    {
                        num5 = Location.x - (Forward.z * 0f + Forward.x * 8.5E-08f) + Forward.z;
                        num6 = Location.z - (Forward.z * 0f - Forward.x * -8.5E-08f) - Forward.z;
                        num7 = Location.x - num5;
                        num8 = Location.z - num6;
                        if (num3 > 1f && num4 > 1f && num3 > num4 && Forward.x > Forward.z)
                        {
                            num9 = Forward.x + Forward.z;
                            x2 = num5 + (num9 * 0.242f + num7);
                            z2 = num6 - (0.242f / num9 - num8);
                        }
                        if (num3 < 1f && num4 < 1f && num3 < num4 && 0f - Forward.x > 0f - Forward.z)
                        {
                            num9 = 0f - Forward.x - Forward.z;
                            x2 = num5 - (num9 * 0.242f - num7);
                            z2 = num6 + (0.242f / num9 + num8);
                        }
                    }
                    else if (num3 > 1f && num4 < 1f && num3 > num4 && Forward.x < 0f - Forward.z || num3 < 1f && num4 > 1f && num3 < num4 && 0f - Forward.x < Forward.z)
                    {
                        num5 = Location.x - (Forward.x * 0f - Forward.z * -8.5E-08f) - Forward.x;
                        num7 = Location.x - num5;
                        if (num3 < 1f && num4 > 1f && num3 < num4 && 0f - Forward.x < Forward.z)
                        {
                            num9 = 0f - (Forward.x - Forward.z);
                            x2 = num5 + num9 * (0.242f + num7);
                            float num10 = Location.z * 1.000397f;
                            float num11 = Location.z - num10;
                            z2 = num10 - (num11 + 0.0857f);
                        }
                        if (num3 > 1f && num4 < 1f && num3 > num4 && Forward.x < 0f - Forward.z)
                        {
                            num9 = Forward.x - Forward.z;
                            x2 = num5 - (num9 * 0.242f - num7);
                            float num12 = Location.z * 0.999603f;
                            float num13 = Location.z - num12;
                            z2 = num12 - (num13 - 0.0857f);
                        }
                    }
                    else if (num3 > 1f && num4 < 1f && num3 > num4 && Forward.x > 0f - Forward.z || num3 < 1f && num4 > 1f && num3 < num4 && 0f - Forward.x > Forward.z)
                    {
                        num5 = Location.x - (Forward.z * 0f + Forward.x * 8.5E-08f) + Forward.z + Forward.x / 1.2f;
                        num6 = Location.z - (Forward.z * 0f - Forward.x * -8.5E-08f) + Forward.z + Forward.x / 2.95f;
                        num7 = Location.x - num5;
                        num8 = Location.z - num6;
                        if (num3 < 1f && num4 > 1f && num3 < num4 && 0f - Forward.x > Forward.z)
                        {
                            num9 = 0f - Forward.x + Forward.z;
                            x2 = num5 - num9 * (0.1992f - num7);
                            z2 = num6 + (0.2244f / num9 + num8);
                        }
                        if (num3 > 1f && num4 < 1f && num3 > num4 && Forward.x > 0f - Forward.z)
                        {
                            num9 = Forward.x - Forward.z;
                            x2 = num5 - num9 * (-0.1992f - num7);
                            z2 = num6 - (0.2244f / num9 - num8);
                        }
                    }
                    else if (num3 > 1f && num4 > 1f && num3 < num4 && Forward.x < Forward.z || num3 <= 1f && num4 < 1f && num3 > num4 && 0f - Forward.x < 0f - Forward.z)
                    {
                        num5 = Location.x - (Forward.x * 0f - Forward.z * -8.5E-08f) - Forward.x + Forward.z / 1.05f;
                        num6 = Location.z - (Forward.x * 0f + Forward.z * 8.5E-08f) + Forward.x + Forward.z / 2.3f;
                        num7 = Location.x - num5;
                        num8 = Location.z - num6;
                        if (num3 > 1f && num4 > 1f && num3 < num4 && Forward.x < Forward.z)
                        {
                            num9 = Forward.x + Forward.z;
                            x2 = num5 + (num9 * 0.301f + num7);
                            z2 = num6 - (-0.14235f / num9 - num8);
                        }
                        if (num3 <= 1f && num4 < 1f && num3 > num4 && 0f - Forward.x < 0f - Forward.z)
                        {
                            num9 = 0f - Forward.x - Forward.z;
                            x2 = num5 - (num9 * 0.301f - num7);
                            z2 = num6 - (0.14235f / num9 - num8);
                        }
                    }
                    float num14 = Location.y / 1.015f;
                    float num15 = Location.y - num14;
                    float y2 = num14 + (num15 - 0.1889601f);
                    Vector3 location2 = new Vector3(x2, y2, z2);
                    part18 = CreatePart(PartArea.Middle, location2, Forward);
                    AddPart(part18);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<BoatWaterScooter>())
            {
                try
                {
                    float num16 = 1f + Forward.x;
                    float num17 = 1f + Forward.z;
                    float x3 = 0f;
                    float num18 = 0f;
                    float num19 = 0f;
                    float z3 = 0f;
                    float num20 = 0f;
                    float num21 = 0f;
                    float num22 = 0f;
                    Part part19 = CreatePart(PartArea.Middle);
                    if (num16 > 1f && num17 > 1f && num16 > num17 && Forward.x > Forward.z || num16 < 1f && num17 < 1f && num16 < num17 && 0f - Forward.x > 0f - Forward.z)
                    {
                        if (num16 > 1f && num17 > 1f && num16 > num17 && Forward.x > Forward.z)
                        {
                            num18 = Location.x / (1f + (Forward.x + Forward.z) / Location.x);
                            num19 = Location.x - num18;
                            num20 = Location.z / (1f + (Forward.x + Forward.z) / Location.z);
                            num21 = Location.z - num20;
                            float num23 = 4f;
                            float num24 = 2.329f;
                            if (Location.x > 200f)
                            {
                                num23 = 16f;
                                num24 = 2.66f;
                            }
                            num22 = Forward.x + Forward.z;
                            x3 = num18 + (num22 * (0f - (Forward.x + Forward.z) / num23) + num19);
                            z3 = num20 + ((Forward.x + Forward.z) * num24 / num22 - num21);
                        }
                        if (num16 < 1f && num17 < 1f && num16 < num17 && 0f - Forward.x > 0f - Forward.z)
                        {
                            num18 = Location.x / (1f + (Forward.x + Forward.z) / Location.x);
                            num19 = Location.x - num18;
                            num20 = Location.z / (1f + (Forward.x + Forward.z) / Location.z);
                            num21 = Location.z - num20;
                            float num25 = 4f;
                            float num26 = 2.329f;
                            if (Location.x > 200f)
                            {
                                num25 = 16f;
                                num26 = 2.66f;
                            }
                            num22 = 0f - Forward.x - Forward.z;
                            x3 = num18 - (num22 * (0f - (0f - Forward.x - Forward.z) / num25) - num19);
                            z3 = num20 - ((0f - Forward.x - Forward.z) * num26 / num22 + num21);
                        }
                    }
                    else if (num16 > 1f && num17 < 1f && num16 > num17 && Forward.x < 0f - Forward.z || num16 < 1f && num17 > 1f && num16 < num17 && 0f - Forward.x < Forward.z)
                    {
                        if (num16 < 1f && num17 > 1f && num16 < num17 && 0f - Forward.x < Forward.z)
                        {
                            num20 = Location.z / 1.0018f;
                            num21 = Location.z - num20;
                            z3 = num20 + (num21 - 0.3921923f);
                            num18 = Location.x / 0.9998f;
                            num19 = Location.x - num18;
                            x3 = num18 - (num19 + 0.021165773f);
                        }
                        if (num16 > 1f && num17 < 1f && num16 > num17 && Forward.x < 0f - Forward.z)
                        {
                            num20 = Location.z / 0.9982032f;
                            num21 = Location.z - num20;
                            z3 = num20 + (num21 + 0.3921923f);
                            num18 = Location.x / 1.0002f;
                            num19 = Location.x - num18;
                            x3 = num18 - (num19 - 0.021165773f);
                        }
                    }
                    else if (num16 > 1f && num17 < 1f && num16 > num17 && Forward.x > 0f - Forward.z || num16 < 1f && num17 > 1f && num16 < num17 && 0f - Forward.x > Forward.z)
                    {
                        if (num16 > 1f && num17 < 1f && num16 > num17 && Forward.x > 0f - Forward.z)
                        {
                            num18 = Location.x / 1.00365f;
                            num19 = Location.x - num18;
                            x3 = num18 + (num19 - 0.3810976f);
                            if (Location.x < Location.z)
                            {
                                z3 = Location.z / 0.9991f;
                            }
                            else if (Location.x > Location.z)
                            {
                                z3 = Location.z / 1.00005f;
                            }
                        }
                        if (num16 < 1f && num17 > 1f && num16 < num17 && 0f - Forward.x > Forward.z)
                        {
                            num18 = Location.x / 0.996363f;
                            num19 = Location.x - num18;
                            x3 = num18 + (num19 + 0.3810976f);
                            if (Location.x < Location.z)
                            {
                                z3 = Location.z / 1.000901f;
                            }
                            else if (Location.x > Location.z)
                            {
                                z3 = Location.z / 0.99995f;
                            }
                        }
                    }
                    else if (num16 > 1f && num17 > 1f && num16 < num17 && Forward.x < Forward.z || num16 <= 1f && num17 < 1f && num16 > num17 && 0f - Forward.x < 0f - Forward.z)
                    {
                        if (num16 > 1f && num17 > 1f && num16 < num17 && Forward.x < Forward.z)
                        {
                            if (Location.x < Location.z)
                            {
                                x3 = Location.x / 1.00176f;
                                z3 = Location.z / 1.001425f;
                            }
                            else if (Location.x > Location.z)
                            {
                                x3 = Location.x / 0.99996054f;
                                z3 = Location.z / 1.0005107f;
                            }
                        }
                        if (num16 <= 1f && num17 < 1f && num16 > num17 && 0f - Forward.x < 0f - Forward.z)
                        {
                            if (Location.x < Location.z)
                            {
                                x3 = Location.x / 0.9982425f;
                                z3 = Location.z / 0.9982425f;
                            }
                            else if (Location.x > Location.z)
                            {
                                x3 = Location.x / 1.00004f;
                                z3 = Location.z / 0.9994896f;
                            }
                        }
                    }
                    float num27 = Location.y / 1.008f;
                    float num28 = Location.y - num27;
                    float y3 = num27 + (num28 - 0.1014786f);
                    Vector3 location3 = new Vector3(x3, y3, z3);
                    part19 = CreatePart(PartArea.Middle, location3, Forward);
                    AddPart(part19);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<FixerCar>(false))
            {
                try
                {
                    Part part20 = CreatePart(PartArea.Middle, new Vector3(Location.x + Forward.z * 0f - Forward.x * 0.55f, Location.y, Location.z + Forward.z * 0f + Forward.x * 0.3f), Forward);
                    AddPart(part20);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<FixerCar.FixerCarFixed>(false))
            {
                try
                {
                    Part part21 = CreatePart(PartArea.Middle, new Vector3(Location.x + Forward.z * 0f - Forward.x * 0.55f, Location.y, Location.z + Forward.z * 0f + Forward.x * 0.3f), Forward);
                    AddPart(part21);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<Fridge>())
            {
                try
                {
                    Part part22 = CreatePart(PartArea.Middle, Location, Forward);
                    AddPart(part22);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<Motorcycle>())
            {
                try
                {
                    Part part23 = CreatePart(PartArea.Middle, Location, Forward);
                    AddPart(part23);
                    return;
                }
                catch
                {
                    return;
                }
            }
            if (ObjectType.Is<Bicycle>())
            {
                try
                {
                    Part part24 = CreatePart(PartArea.Middle, Location, -Forward);
                    AddPart(part24);
                    return;
                }
                catch
                {
                    return;
                }
            }
            try
            {
                AddPart(CreatePart());
            }
            catch
            {
            }
        }

        public void AddPart(Part part)
        {
            part.Target = this;
            part.RefreshMinMaxSims();
            if (Parts.ContainsKey(part.Area))
            {
                Parts[part.Area] = part;
            }
            else
            {
                Parts.Add(part.Area, part);
            }
        }

        public bool Add(Player player)
        {
            PartArea key = ChooseSectionDialog(ObjectType);
            if (Parts.ContainsKey(key) && Parts[key] != null)
            {
                return Parts[key].Add(player);
            }
            return false;
        }

        public void DisablePassthrough(Player player)
        {
            try
            {
                if (player != null && player.IsValid && HasObject && (Object is IHasPortalComponent || Object is IPortalConnectionObject))
                {
                    PortalComponent portalComponent = Object.PortalComponent;
                    if (portalComponent != null)
                    {
                        bool preferredLaneSelected = false;
                        portalComponent.LockRoutingLane(player.Actor, Object.Position, false, Route.RouteOption.None, out preferredLaneSelected);
                        PassthroughDisabled = true;
                    }
                }
            }
            catch
            {
            }
        }

        public void EnablePassthrough()
        {
            try
            {
                if (HasObject && (Object is IHasPortalComponent || Object is IPortalConnectionObject))
                {
                    PortalComponent portalComponent = Object.PortalComponent;
                    if (portalComponent != null)
                    {
                        portalComponent.FreeAllRoutingLanes();
                        PassthroughDisabled = false;
                    }
                }
            }
            catch
            {
            }
        }

        public Part ChooseSectionDialog()
        {
            PartArea key = ChooseSectionDialog(ObjectType);
            if (Parts.ContainsKey(key))
            {
                return Parts[key];
            }
            return null;
        }

        public static PartArea ChooseSectionDialog(PassionType PT)
        {
            PartArea result = PartArea.Middle;
            if (PT != null && PT.IsValid)
            {
                if (PT.Is<BedDouble>())
                {
                    switch (ThreeButtonDialog.Show(PassionCommon.Localize("S3_Passion.Terms.WhatPartOfBed"), PassionCommon.Localize("S3_Passion.Terms.LeftSide"), PassionCommon.Localize("S3_Passion.Terms.Center"), PassionCommon.Localize("S3_Passion.Terms.RightSide")))
                    {
                        case ThreeButtonDialog.ButtonPressed.FirstButton:
                            result = PartArea.Left;
                            break;
                        case ThreeButtonDialog.ButtonPressed.ThirdButton:
                            result = PartArea.Right;
                            break;
                    }
                }
                else if (PT.Is<HotTubGrotto>())
                {
                    switch (ThreeButtonDialog.Show(PassionCommon.Localize("S3_Passion.Terms.WhatPartOfHotTub"), PassionCommon.Localize("S3_Passion.Terms.LeftSide"), PassionCommon.Localize("S3_Passion.Terms.Center"), PassionCommon.Localize("S3_Passion.Terms.RightSide")))
                    {
                        case ThreeButtonDialog.ButtonPressed.FirstButton:
                            result = PartArea.Left;
                            break;
                        case ThreeButtonDialog.ButtonPressed.ThirdButton:
                            result = PartArea.Right;
                            break;
                    }
                }
                else if (PT.Is<HotTub4Seated>())
                {
                    result = !TwoButtonDialog.Show(PassionCommon.Localize("S3_Passion.Terms.WhatPartOfHotTub"), PassionCommon.Localize("S3_Passion.Terms.Top"), PassionCommon.Localize("S3_Passion.Terms.Bottom")) ? PartArea.Bottom : PartArea.Top;
                }
                else if (PT.Is<TableDining3x1>())
                {
                    result = !TwoButtonDialog.Show(PassionCommon.Localize("S3_Passion.Terms.WhatPartOfTable"), PassionCommon.Localize("S3_Passion.Terms.LeftSide"), PassionCommon.Localize("S3_Passion.Terms.RightSide")) ? PartArea.Right : PartArea.Left;
                }
            }
            return result;
        }
    }
}
