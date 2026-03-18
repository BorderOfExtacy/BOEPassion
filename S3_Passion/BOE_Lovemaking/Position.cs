using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Lovemaking;
using S3_Passion.BOE_Objects;
using S3_Passion.BOE_Settings;
using S3_Passion.BOE_UI;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.Counters;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.Gameplay.Objects.Decorations.Mimics;
using Sims3.Gameplay.Objects.Door;
using Sims3.Gameplay.Objects.Entertainment;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.HobbiesSkills;
using Sims3.Gameplay.Objects.HobbiesSkills.BrainEnhancingMachine;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Objects.Tables;
using Sims3.Gameplay.Objects.Vehicles;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.SimIFace.VideoRecording;
using Sims3.Store.Objects;
using Sims3.UI;


namespace S3_Passion.BOE_Lovemaking
{
    [Persistable]
    public class Position : IPositionChoice
    {
        public class Category
        {
            public const int None = 0;

            public const int Any = 1;

            public const int Oral = 2;

            public const int Vaginal = 4;

            public const int Anal = 8;

            public const int Hands = 16;

            public const int Feet = 32;

            public const int Breasts = 64;

            public const int Foreplay = 128;

            public const int All = 255;

            public const int Masturbate = 48;

            public const int Fuck = 12;

            public const int Choose = 1024;

            public static int FromString(string categories)
            {
                int num = 0;
                categories = categories.Replace(" ", string.Empty);
                string[] array = categories.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    switch (array[i].ToLower())
                    {
                        case "foreplay":
                            num |= 0x80;
                            break;
                        case "hands":
                        case "masturbate":
                            num |= 0x10;
                            break;
                        case "feet":
                            num |= 0x20;
                            break;
                        case "breasts":
                            num |= 0x40;
                            break;
                        case "oral":
                            num |= 2;
                            break;
                        case "vaginal":
                            num |= 4;
                            break;
                        case "anal":
                            num |= 8;
                            break;
                        case "fuck":
                            num |= 0xC;
                            break;
                        case "none":
                        case "any":
                            num |= 1;
                            break;
                    }
                }
                return num;
            }

            public static string ToString(int i)
            {
                if (PassionCommon.Match(i, 1))
                {
                    return PassionCommon.Localize("S3_Passion.Terms.All");
                }
                string text = string.Empty;
                if (PassionCommon.Match(i, 12))
                {
                    text += PassionCommon.Localize("S3_Passion.Terms.Fuck");
                }
                if (PassionCommon.Match(i, 2))
                {
                    text = text + (text.Length > 0 ? ", " : "") + PassionCommon.Localize("S3_Passion.Terms.Oral");
                }
                if (PassionCommon.Match(i, 48))
                {
                    text = text + (text.Length > 0 ? ", " : "") + PassionCommon.Localize("S3_Passion.Terms.Manual");
                }
                if (string.IsNullOrEmpty(text))
                {
                    text = PassionCommon.Localize("S3_Passion.Terms.All");
                }
                return text;
            }
        }

        public class Tone
        {
            public const int None = 0;

            public const int Any = 1;

            public const int Romantic = 2;

            public const int Intense = 4;

            public const int Rough = 12;

            public const int Disinterested = 16;

            public const int Dominating = 32;

            public const int Toy = 64;

            public static int FromString(string tones)
            {
                int num = 0;
                tones = tones.Replace(" ", string.Empty);
                string[] array = tones.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    switch (array[i].ToLower())
                    {
                        case "romantic":
                            num |= 2;
                            break;
                        case "rough":
                            num |= 0xC;
                            break;
                        case "disinterested":
                            num |= 0x10;
                            break;
                        case "intense":
                            num |= 4;
                            break;
                        case "dominating":
                            num |= 0x20;
                            break;
                        case "toy":
                            num |= 0x40;
                            break;
                        case "any":
                            num |= 1;
                            break;
                    }
                }
                return num;
            }
        }

        public class Animation
        {
            [Persistable]
            public class Set
            {
                public int PenisesNeeded = 0;

                public int VaginasNeeded = 0;

                public string TargetAnimation = null;

                public Dictionary<int, Slot> Slots;

                public Set()
                {
                    Slots = new Dictionary<int, Slot>();
                }

                public Slot Add(int i)
                {
                    if (Slots == null)
                    {
                        Slots = new Dictionary<int, Slot>();
                    }
                    if (i < 1)
                    {
                        i = Slots.Count + 1;
                    }
                    Slot slot = new Slot();
                    if (Slots.ContainsKey(i))
                    {
                        Slots[i] = slot;
                    }
                    else
                    {
                        Slots.Add(i, slot);
                    }
                    return slot;
                }

                public void Update()
                {
                    try
                    {
                        PenisesNeeded = 0;
                        VaginasNeeded = 0;
                        int num = 0;
                        int num2 = 0;
                        foreach (Slot value in Slots.Values)
                        {
                            if (value != null)
                            {
                                if (value.NeedsPenis)
                                {
                                    num++;
                                }
                                if (value.NeedsVagina)
                                {
                                    num2++;
                                }
                            }
                        }
                        if (num > PenisesNeeded)
                        {
                            PenisesNeeded = num;
                        }
                        if (num2 > VaginasNeeded)
                        {
                            VaginasNeeded = num2;
                        }
                    }
                    catch
                    {
                    }
                }

                public Slot GetSlot(int slot)
                {
                    if (Slots != null && Slots.ContainsKey(slot))
                    {
                        return Slots[slot];
                    }
                    return null;
                }

                public ClipData GetClip(Player player)
                {
                    try
                    {
                        if (player != null && player.IsValid && Slots != null && Slots.ContainsKey(player.PositionIndex) && Slots[player.PositionIndex] != null)
                        {
                            return Slots[player.PositionIndex].GetClip(player);
                        }
                        if (PassionCommon.Testing)
                        {
                            PassionCommon.BufferClear();
                            PassionCommon.BufferLine("Logic break in Set.GetClip()");
                            if (player == null)
                            {
                                PassionCommon.BufferLine("  Player == null");
                            }
                            else if (!player.IsValid)
                            {
                                PassionCommon.BufferLine("  Player != IsValid");
                            }
                            else if (Slots == null)
                            {
                                PassionCommon.BufferLine("  Slots == null");
                            }
                            else if (!Slots.ContainsKey(player.PositionIndex))
                            {
                                PassionCommon.BufferLine("  Slots does not contain player.PositionIndex (" + player.PositionIndex + ")");
                                PassionCommon.BufferLine("  Slots.Count == " + Slots.Count);
                                foreach (KeyValuePair<int, Slot> slot in Slots)
                                {
                                    PassionCommon.BufferLine("    Slot " + slot.Key + ": " + (slot.Value != null ? "!null" : "null"));
                                }
                            }
                            PassionCommon.SystemMessage();
                        }
                    }
                    catch
                    {
                        PassionCommon.SystemMessage("Error in Set.GetClip()");
                    }
                    return null;
                }

                public void GiveHeldItem(Player player)
                {
                    Slot slot = GetSlot(player.PositionIndex);
                    if (slot != null && slot.HasHeldItem)
                    {
                        try
                        {
                            if (player.HeldItem == null || !player.HeldItem.IsValid || player.HeldItem.Key != slot.HeldItem.Key)
                            {
                                HeldItem.Create(player, slot.HeldItem.Key, slot.HeldItem.Location, slot.HeldItem.Facing, slot.HeldItem.Angle, slot.HeldItem.Slot);
                            }
                        }
                        catch
                        {
                            if (PassionCommon.Testing)
                            {
                                PassionCommon.SystemMessage("Error attempting to retrieve Held Item data.");
                            }
                        }
                        player.BufferedObjectAnimation = slot.HeldItemAnimation;
                    }
                    else
                    {
                        if (player.HeldItem != null)
                        {
                            player.HeldItem.Release();
                        }
                        player.HeldItem = null;
                    }
                }
            }

            [Persistable]
            public class Slot
            {
                public bool NeedsPenis = false;

                public bool NeedsVagina = false;

                public bool NeedsBreasts = false;

                public HeldItem HeldItem = null;

                public string HeldItemAnimation = null;

                public List<ClipData> Clips;

                public bool HasHeldItem
                {
                    get
                    {
                        return HeldItem != null;
                    }
                }

                public Slot()
                {
                    Clips = new List<ClipData>();
                }

                public ClipData Add(string clip, CASAgeGenderFlags flags, bool penis, bool vagina, string nude)
                {
                    // adds per-sim animation data
                    return Add(new ClipData(clip, flags, penis, vagina, nude));
                }

                public ClipData Add(ClipData clipdata)
                {
                    //adds per-sim animation data, but as like, clipdata this time
                    Clips.Add(clipdata);
                    Update();
                    return clipdata;
                }

                public void Update()
                {
                    if (Clips == null || Clips.Count == 0)
                    {
                        NeedsPenis = false;
                        NeedsVagina = false;
                        NeedsBreasts = false;
                        return;
                    }
                    NeedsPenis = true;
                    NeedsVagina = true;
                    NeedsBreasts = true;

                    // for each clip in the clip list
                    foreach (ClipData clip in Clips)
                    {

                        //turns off various bools if the clip doesnt need them?
                        if (!clip.NeedsPenis)
                        {
                            NeedsPenis = false;
                        }
                        if (!clip.NeedsVagina)
                        {
                            NeedsVagina = false;
                        }
                        if (!clip.NeedsBreasts)
                        {
                            NeedsBreasts = false;
                        }

                    }
                }

                public ClipData GetClip(Player player)
                {
                    ClipData result = null;
                    try
                    {
                        if (player != null && player.IsValid)
                        {

                            //for each sim in the clip collection
                            foreach (ClipData clip in Clips)
                            {
                                if (clip != null && clip.IsValid)
                                {
                                    // SAVE THIS: where a position is assigned, i think!!
                                    // if a clip needs a peen and sim has peen OR if a clip has vagene and sim has vagene
                                    // refactor to include if the clip is a top or bottom position - whether genitals or sexual role takes priority should be a setting
                                    if ((player.SimGenitalType == "penis" && clip.NeedsPenis || player.SimGenitalType == "vagina" && clip.NeedsVagina) && clip.CompareFlags(player.Flags))
                                    {

                                        player.UndressLevel = clip.NudeType;
                                        
                                        
                                        return clip;
                                    }
                                    result = clip;
                                }
                            }
                        }
                        else
                        {
                            PassionCommon.SystemMessage("Error: " + PassionCommon.Localize("Logic break in Position Clip"));
                        }
                    }
                    catch
                    {
                    }
                    return result;
                }
            }

            [Persistable]
            public class ClipData
            {
                public static readonly Dictionary<int, string> AWPosition;

                public string Clip;

                public CASAgeGenderFlags Flags;

                public bool NeedsPenis;

                public bool NeedsVagina;

                public bool NeedsBreasts;

                public bool NeedsTeen;

                public string NudeType;

                public bool IsValid
                {
                    get
                    {
                        return Exists(Clip);
                    }
                }

                public static ClipData CreateFromAnimatedWoohoo(string clip1, string clip2, int position, int participants)
                {
                    if (!AWPosition.ContainsKey(position))
                    {
                        position = 1;
                    }
                    return new ClipData(clip1 + AWPosition[position] + clip2 + (participants == 3 ? "o" : string.Empty), position == 1, false, "Fullbody");
                }

                public static bool Exists(string clip)
                {
                    return !string.IsNullOrEmpty(clip) && Sims3.SimIFace.Animation.ClipExists(clip, ProductVersion.BaseGame);
                }

                public bool CompareFlags(uint flags)
                {
                    if (Flags == CASAgeGenderFlags.None)
                    {
                        return true;
                    }
                    return PassionCommon.Match(flags, (uint)Flags);
                }

                public ClipData(string clip, CASAgeGenderFlags flags, string nude)
                {
                    Initialize(clip, flags, false, false, false, nude);
                }

                public ClipData(string clip, bool penis, bool vagina, string nude)
                {
                    Initialize(clip, CASAgeGenderFlags.None, penis, vagina, false, nude);
                }

                public ClipData(string clip, CASAgeGenderFlags flags, bool penis, bool vagina, string nude)
                {
                    Initialize(clip, flags, penis, vagina, false, nude);
                }

                public ClipData(string clip, CASAgeGenderFlags flags, bool penis, bool vagina, bool breasts, string nude)
                {
                    Initialize(clip, flags, penis, vagina, breasts, nude);
                }

                public void Initialize(string clip, CASAgeGenderFlags flags, bool penis, bool vagina, bool breasts, string nude)
                {
                    Clip = clip;
                    Flags = flags;
                    NeedsPenis = penis;
                    NeedsVagina = vagina;
                    NeedsBreasts = breasts;
                    NudeType = nude;
                }

                static ClipData()
                {
                    Dictionary<int, string> dictionary = new Dictionary<int, string>();
                    dictionary.Add(1, "A");
                    dictionary.Add(2, "B");
                    dictionary.Add(3, "C");
                    dictionary.Add(4, "D");
                    dictionary.Add(5, "E");
                    dictionary.Add(6, "F");
                    dictionary.Add(7, "G");
                    dictionary.Add(8, "H");
                    dictionary.Add(9, "I");
                    dictionary.Add(10, "J");
                    dictionary.Add(11, "K");
                    dictionary.Add(12, "L");
                    dictionary.Add(13, "M");
                    dictionary.Add(14, "N");
                    dictionary.Add(15, "O");
                    dictionary.Add(16, "P");
                    dictionary.Add(17, "Q");
                    dictionary.Add(18, "R");
                    dictionary.Add(19, "S");
                    dictionary.Add(20, "T");
                    dictionary.Add(21, "U");
                    dictionary.Add(22, "V");
                    dictionary.Add(23, "W");
                    dictionary.Add(24, "X");
                    dictionary.Add(25, "Y");
                    dictionary.Add(26, "Z");
                    AWPosition = dictionary;
                }
            }

            public class Prefix
            {
                public const int Male = 4096;

                public const int Female = 8192;

                private static readonly Dictionary<int, string> mDictionary;

                public static string AgeGender(Sim sim)
                {
                    return Age(sim) + Gender(sim);
                }

                public static string Age(Sim sim)
                {
                    return Age(sim, false);
                }

                public static string Age(Sim sim, bool teenIsAdult)
                {
                    if (sim != null && !teenIsAdult)
                    {
                        try
                        {
                            return Get((int)sim.SimDescription.Age);
                        }
                        catch
                        {
                        }
                    }
                    return "a";
                }

                public static string Gender(Sim sim)
                {
                    return Gender(sim, true);
                }

                public static string Gender(Sim sim, bool checkPenis)
                {
                    if (sim != null)
                    {
                        return !checkPenis ? Get((int)sim.SimDescription.Gender) : !PassionCommon.HasPart(sim, PassionCommon.SimPart.Penis) ? Get(8192) : Get(4096);
                    }
                    return "";
                }

                public static string Get(Sim sim)
                {
                    return AgeGender(sim);
                }

                public static string Get(int i)
                {
                    if (mDictionary.ContainsKey(i) && mDictionary[i] != null)
                    {
                        return mDictionary[i];
                    }
                    return "";
                }

                static Prefix()
                {
                    Dictionary<int, string> dictionary = new Dictionary<int, string>();
                    dictionary.Add(0, "");
                    dictionary.Add(8, "t");
                    dictionary.Add(16, "a");
                    dictionary.Add(32, "a");
                    dictionary.Add(64, "a");
                    dictionary.Add(4096, "");
                    dictionary.Add(8192, "f");
                    mDictionary = dictionary;
                }
            }
        }

        public string Key;

        public string Name;

        public string NamePrefix;

        public string Creator;

        public int Tones;

        public int Categories;

        public Dictionary<string, PassionType> SupportedTypes;

        public Dictionary<int, bool> NeedsPenis;

        public Dictionary<int, bool> NeedsVagina;

        public Dictionary<int, int> InteractsWith;

        public Dictionary<int, string> SimID;

        public Dictionary<int, string> HardCodedAnimationName;

        public Dictionary<int, Animation.Set> Sets;

        public string Clip;

        public string AnimObject;

        public string ObjectAnimation;

        public string Genders;

        public string ClipPart1;

        public string ClipPart2;

        public bool TeenAnimations;

        public bool FemaleAnimations;

        public bool ComplexAnimations;

        public bool PossiblePregnancy;

        public bool PossibleSameSexPregnancy;

        public bool UseTheWhip;

        public Dictionary<int, bool> PutOnStraOn;

        public Dictionary<int, bool> ActorPosition;

        public float TeenHeightAdjustment;

        public int Dildo;

        public int ButtPlug;

        public int BlackDildo;

        public int DoubleDildo;

        public int Punisher;

        public int AnalTrainer;

        public int CurvedDildo;

        public int Pumpkin;

        protected int mMinSims;

        protected int mMaxSims;

        public int MinSims
        {
            get
            {
                return mMinSims;
            }
            set
            {
                mMinSims = value;
                if (mMinSims > mMaxSims)
                {
                    mMaxSims = mMinSims;
                }
            }
        }

        public int MaxSims
        {
            get
            {
                return mMaxSims;
            }
            set
            {
                mMaxSims = value;
                if (mMaxSims < mMinSims)
                {
                    mMinSims = mMaxSims;
                }
            }
        }

        public static int Create(IEnumerable<string> filenames)
        {
            int num = 0;
            foreach (string filename in filenames)
            {
                num += Create(filename);
            }
            return num;
        }

        public static int Create(string filename)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(filename))
            {
                XML.File file = XML.Create(filename);
                if (file.IsValid)
                {
                    XML.Node node = file["Passion"];
                    if (node != null)
                    {
                        foreach (XML.Node matchingNode7 in node.GetMatchingNodes("Position"))
                        {
                            if (string.IsNullOrEmpty(matchingNode7["Name"]))
                            {
                                continue;
                            }
                            Position position = new Position();
                            position.NamePrefix = "(*) ";
                            position.Key = matchingNode7["Key"];
                            position.Name = matchingNode7["Name"];
                            position.Clip = position.Name;
                            position.Creator = matchingNode7["Creator"];
                            position.MinSims = PassionCommon.Int(matchingNode7["MinSims"]);
                            position.MaxSims = PassionCommon.Int(matchingNode7["MaxSims"]);
                            position.Tones = Tone.FromString(matchingNode7["Tones"]);
                            position.Categories = Category.FromString(matchingNode7["Categories"]);
                            string text = matchingNode7["Targets"];
                            if (!string.IsNullOrEmpty(text))
                            {
                                text = text.Replace(" ", string.Empty);
                                string[] array = text.Split(',');
                                string[] array2 = array;
                                foreach (string text2 in array2)
                                {
                                    string text3 = text2.ToLower();
                                    if (!(text3 == "beds"))
                                    {
                                        if (text3 == "tables")
                                        {
                                            position.AddSupportedType<TableDining1x1>();
                                            position.AddSupportedType<TableDining2x1>();
                                            position.AddSupportedType<TableDining3x1>();
                                            position.AddSupportedType<TableEnd>();
                                        }
                                        else if (PassionType.IsLoaded(text2))
                                        {
                                            position.AddSupportedType(text2);
                                        }
                                    }
                                    else
                                    {
                                        position.AddSupportedType<BedSingle>();
                                        position.AddSupportedType<BedDouble>();
                                    }
                                }
                            }
                            XML.Node matchingNode = matchingNode7.GetMatchingNode("AnimationSets");
                            if (matchingNode != null)
                            {
                                foreach (XML.Node matchingNode8 in matchingNode.GetMatchingNodes("Set"))
                                {
                                    if (matchingNode8 == null)
                                    {
                                        continue;
                                    }
                                    int num2 = PassionCommon.Int(matchingNode8.GetAttribute("Players"));
                                    if (num2 <= 0)
                                    {
                                        continue;
                                    }
                                    if (position.MinSims < 1 || num2 < position.MinSims)
                                    {
                                        position.MinSims = num2;
                                    }
                                    if (position.MaxSims < 1 || num2 > position.MaxSims)
                                    {
                                        position.MaxSims = num2;
                                    }
                                    Animation.Set set = new Animation.Set();
                                    if (position.Sets.ContainsKey(num2))
                                    {
                                        position.Sets[num2] = set;
                                    }
                                    else
                                    {
                                        position.Sets.Add(num2, set);
                                    }
                                    set.TargetAnimation = matchingNode8["TargetAnimation"];
                                    foreach (XML.Node matchingNode9 in matchingNode8.GetMatchingNodes("Player"))
                                    {
                                        int num3 = PassionCommon.Int(matchingNode9.GetAttribute("Index"));
                                        if (num3 <= 0)
                                        {
                                            continue;
                                        }
                                        Animation.Slot slot = set.Add(num3);
                                        XML.Node matchingNode2 = matchingNode9.GetMatchingNode("HeldItem");
                                        if (matchingNode2 != null)
                                        {
                                            XML.Node matchingNode3 = matchingNode2.GetMatchingNode("Location");
                                            Vector3 location = matchingNode3 != null ? new Vector3(PassionCommon.Float(matchingNode3.GetAttribute("X")), PassionCommon.Float(matchingNode3.GetAttribute("Y")), PassionCommon.Float(matchingNode3.GetAttribute("Z"))) : Vector3.Empty;
                                            XML.Node matchingNode4 = matchingNode2.GetMatchingNode("Facing");
                                            Vector3 facing = matchingNode4 != null ? new Vector3(PassionCommon.Float(matchingNode4.GetAttribute("X")), PassionCommon.Float(matchingNode4.GetAttribute("Y")), PassionCommon.Float(matchingNode4.GetAttribute("Z"))) : Vector3.Empty;
                                            float angle = PassionCommon.Float(matchingNode2["Angle"]);
                                            uint slot2 = 3703456078u;
                                            try
                                            {
                                                slot2 = string.IsNullOrEmpty(matchingNode2["Slot"]) ? 3703456078u : (uint)Enum.Parse(typeof(HeldItem.ItemSlots), matchingNode2["Slot"], true);
                                            }
                                            catch
                                            {
                                                if (PassionCommon.Testing)
                                                {
                                                    PassionCommon.BufferMessage("Error parsing Held Item slot for " + position.Name + "(Set:" + num2 + ",Slot:" + num3 + ")");
                                                }
                                            }
                                            slot.HeldItem = HeldItem.Create(matchingNode2["ObjectKey"], location, facing, angle, slot2);
                                            slot.HeldItemAnimation = matchingNode2["Clip"];
                                        }
                                        foreach (XML.Node matchingNode10 in matchingNode9.GetMatchingNodes("Clip"))
                                        {
                                            if (!Animation.ClipData.Exists(matchingNode10.Value))
                                            {
                                                continue;
                                            }
                                            string attribute = matchingNode10.GetAttribute("Required");
                                            float num4 = PassionCommon.Float(matchingNode10.GetAttribute("X"));
                                            float num5 = PassionCommon.Float(matchingNode10.GetAttribute("Y"));
                                            float num6 = PassionCommon.Float(matchingNode10.GetAttribute("Z"));
                                            bool penis = false;
                                            bool vagina = false;
                                            CASAgeGenderFlags cASAgeGenderFlags = CASAgeGenderFlags.None;
                                            if (!string.IsNullOrEmpty(attribute))
                                            {
                                                attribute = attribute.Replace(" ", string.Empty);
                                                string[] array3 = attribute.Split(',');
                                                string[] array4 = array3;

                                                

                                                foreach (string text4 in array4)
                                                {
                                                    switch (text4.ToLower())
                                                    {
                                                        case "penis":
                                                            penis = true;
                                                            break;
                                                        case "vagina":
                                                            vagina = true;
                                                            break;
                                                        case "teen":
                                                            cASAgeGenderFlags |= CASAgeGenderFlags.Teen;
                                                            break;
                                                        default:
                                                            cASAgeGenderFlags = (CASAgeGenderFlags)((uint)cASAgeGenderFlags | (uint)PassionCommon.Int(text4));
                                                            break;
                                                    }
                                                }
                                            }
                                            slot.Add(matchingNode10.Value, cASAgeGenderFlags, penis, vagina, "LowerBody");
                                        }
                                    }
                                    set.Update();
                                }
                            }
                            position.RegisterMinMaxSims();
                            if (PassionBase.Positions.ContainsKey(position.Key))
                            {
                                PassionBase.Positions[position.Key] = position;
                            }
                            else
                            {
                                PassionBase.Positions.Add(position.Key, position);
                            }
                            num++;
                        }
                    }
                    XML.Node node2 = file["WooHooStages"];
                    if (node2 != null)
                    {
                        foreach (XML.Node matchingNode11 in node2.GetMatchingNodes("WooHooStage"))
                        {
                            if (string.IsNullOrEmpty(matchingNode11["Key"]))
                            {
                                continue;
                            }
                            Position position2 = new Position();
                            position2.NamePrefix = "(KW) ";
                            position2.Key = matchingNode11["Key"];
                            position2.Name = position2.Key;
                            position2.Clip = position2.Key;
                            position2.Creator = matchingNode11["Creator"];
                            position2.MinSims = PassionCommon.Int(matchingNode11["MinSims"]);
                            position2.AnimObject = matchingNode11["Objects"];
                            position2.ObjectAnimation = matchingNode11["ObjectAnimation"];
                            SimBuilder simBuilder = new SimBuilder();
                            List<XML.Node> matchingNodes = matchingNode11.GetMatchingNodes("WooHooActor");
                            if (matchingNodes.Count > position2.MinSims)
                            {
                                position2.MaxSims = matchingNodes.Count;
                            }
                            else
                            {
                                position2.MaxSims = position2.MinSims;
                            }


                            // FOR EACH ACTOR
                            foreach (XML.Node item in matchingNodes)
                            {
                                //actor id
                                int num7 = PassionCommon.Int(item.GetAttribute("Id")) + 1;

                                position2.AddHardCodedAnimation(num7, item["Animation"]);

                                // what's inputted in the viable genders
                                string text5 = item["Genders"].ToLower();

                                string nudity = item["NakedFlags"];

                                position2.Genders = item["Genders"];
                                position2.UseTheWhip = PassionCommon.Bool(item["UseWhip"]);
                                if (position2.ActorPosition.ContainsKey(num7))
                                {
                                    position2.ActorPosition[num7] = true;
                                }
                                else
                                {
                                    position2.ActorPosition.Add(num7, true);
                                }

                                // remove this bc idfk what's going on
                                // i think this is where it gathers what genitalia types are needed?

                                // flag = penis (in orig this means male sim or strapon)
                                // if flag is true, it means peenar
                                bool flag = text5.Contains("male") && (!text5.Contains("female") || PassionCommon.Bool(item["UseStrapon"]));

                                // if vagina2 is true, it means vagine
                                bool vagina2 = text5 == "female";

                                //if the position calls for a strapon
                                if (PassionCommon.Bool(item["UseStrapon"]))
                                {
                                    //if the position's strap list includes this actor's key
                                    if (position2.PutOnStraOn.ContainsKey(num7))
                                    {
                                        position2.PutOnStraOn[num7] = true;
                                    }
                                    // else we add it to the list
                                    else
                                    {
                                        position2.PutOnStraOn.Add(num7, true);
                                    }
                                }

                                // if the position calls for a penis
                                if (flag)
                                {
                                    if (position2.NeedsPenis.ContainsKey(num7))
                                    {
                                        position2.NeedsPenis[num7] = true;
                                    }
                                    else
                                    {
                                        position2.NeedsPenis.Add(num7, true);
                                    }
                                }

                                // for each sim in the animation (?)
                                for (int k = position2.MinSims; k <= position2.MaxSims; k++)
                                {
                                    Animation.Set set2;
                                    if (!position2.Sets.ContainsKey(k))
                                    {
                                        set2 = new Animation.Set();
                                        position2.Sets.Add(k, set2);
                                    }
                                    else
                                    {
                                        set2 = position2.Sets[k];
                                    }
                                    Animation.Slot slot3 = set2.Add(num7);

                                    // add the animation for the sim? reminder flag is penis bool, vagina2 is vagina bool
                                    slot3.Add(item["Animation"], CASAgeGenderFlags.None, flag, vagina2, nudity);

                                    //sim accessory processing
                                    //WHYYYYYY is this hardcoded
                                    XML.Node matchingNode5 = item.GetMatchingNode("Accessory");
                                    if (matchingNode5 != null)
                                    {
                                        string empty = string.Empty;
                                        string empty2 = string.Empty;
                                        switch (matchingNode5.GetAttribute("Key"))
                                        {
                                            case "Dildo":
                                                empty = "0x02DC343F-0x00000000-0x780597AC522896FB";
                                                break;
                                            case "WoodenSpoon":
                                                empty = "0x02DC343F-0x00000000-0x000000000000035F";
                                                break;
                                            case "KnifeLarge":
                                                empty = "0x02DC343F-0x00000000-0x000000000000036A";
                                                break;
                                            case "accessorySponge":
                                                empty = "0x02DC343F-0x00000000-0x00000000000003A2";
                                                break;
                                            case "PhoneCell":
                                                empty = "0x02DC343F-0x00000000-0x0000000000000525";
                                                break;
                                            case "PhoneCellProp":
                                                empty = "0x02DC343F-0x00000000-0x0000000000001183";
                                                break;
                                            case "phoneSmartPhone":
                                                empty = "0x02DC343F-0x00000000-0x000000000098DB49";
                                                break;
                                            case "PoolCue":
                                                empty = "0x02DC343F-0x00000000-0x000000000098D433";
                                                break;
                                            case "DragonWhip":
                                                empty = "0x02DC343F-0x00000000-0x000000006C860635";
                                                break;
                                            case "SV-Whip":
                                                empty = "0x02DC343F-0x00000000-0x0000000000DF58B3";
                                                break;
                                            case "ButtPlug":
                                                empty = "0x02DC343F-0x00000000-0x0000000059C1465B";
                                                break;
                                            case "BlackDildo":
                                                empty = "0x02DC343F-0x00000000-0x00000000042C4C21";
                                                break;
                                            case "DoubleDildo":
                                                empty = "0x02DC343F-0x00000000-0x000000000D4B42F6";
                                                break;
                                            case "DoubleButtPlug":
                                                empty = "0x02DC343F-0x00000000-0x000000002734DB7A";
                                                break;
                                            case "HandCuffs":
                                                empty = "0x02DC343F-0x00000000-0x000000003B3BB332";
                                                break;
                                            case "Punisher":
                                                empty = "0x02DC343F-0x00000000-0x000000004E6DFBC4";
                                                break;
                                            case "AnalTrainer":
                                                empty = "0x02DC343F-0x00000000-0x0000000038B3F337";
                                                break;
                                            case "CurvedDildo":
                                                empty = "0x02DC343F-0x00000000-0x00000000590CD0E0";
                                                break;
                                            case "Pumpkin":
                                                empty = "0x02DC343F-0x00000000-0x000000004964A3B5";
                                                break;
                                            case "cameraVideoCheap":
                                                empty = "0x02DC343F-0x38000000-0x000000000098A6AC";
                                                break;
                                            case "Coin":
                                                empty = "0x02DC343F-0x00000000-0x0000000000000A1E";
                                                break;
                                            case "DollhouseDollThree":
                                                empty = "0x02DC343F-0x00000000-0x00000000000009AE";
                                                break;
                                            case "cleaningMop":
                                                empty = "0x02DC343F-0x00000000-0x00000000000004B6";
                                                break;
                                            case "IceCreamCone":
                                                empty = "0x02DC343F-0x00000000-0x000000000098A94F";
                                                break;
                                            case "VideoGameSystemController":
                                                empty = "0x02DC343F-0x00000000-0x0000000000000664";
                                                break;
                                            case "wildflowersSingle":
                                                empty = "0x02DC343F-0x78000000-0x000000000098B06F";
                                                break;
                                            default:
                                                empty = null;
                                                break;
                                        }
                                        Vector3 vector = Vector3.Zero;
                                        Vector3.TryParse(matchingNode5["Offset"], out vector);
                                        Vector3 facing2 = Vector3.Zero;
                                        switch (matchingNode5["Axis"])
                                        {
                                            case "UnitX":
                                                facing2 = Vector3.UnitX;
                                                break;
                                            case "UnitY":
                                                facing2 = Vector3.UnitY;
                                                break;
                                            case "UnitZ":
                                                facing2 = Vector3.UnitZ;
                                                break;
                                        }
                                        uint slot4 = 3703456078u;
                                        float angle2 = PassionCommon.Float(matchingNode5["Angle"]) * (float)Math.PI / 180f;
                                        if (matchingNode5.GetAttribute("Key") != "WoodenSpoon")
                                        {
                                            try
                                            {
                                                slot4 = (uint)Enum.Parse(typeof(HeldItem.ItemSlots), matchingNode5["Slot"], true);
                                            }
                                            catch
                                            {
                                            }
                                            slot3.HeldItem = HeldItem.Create(empty, vector, facing2, angle2, slot4);
                                            slot3.HeldItemAnimation = empty2;
                                        }
                                        else if (matchingNode5.GetAttribute("Key") == "WoodenSpoon")
                                        {
                                            slot4 = 1557334703u;
                                            try
                                            {
                                                slot4 = (uint)Enum.Parse(typeof(HeldItem.ItemSlots), "LeftHand", true);
                                            }
                                            catch
                                            {
                                            }
                                            Vector3.TryParse("0.13,0.03,0.19", out vector);
                                            angle2 = PassionCommon.Float("90") * (float)Math.PI / 180f;
                                            facing2 = Vector3.UnitZ;
                                            empty = "0x02DC343F-0x00000000-0x0000000000000376";
                                            slot3.HeldItem = HeldItem.Create(empty, vector, facing2, angle2, slot4);
                                            slot3.HeldItemAnimation = empty2;
                                        }
                                    }
                                    if (position2.UseTheWhip)
                                    {
                                        float angle3 = 0f;
                                        string empty3 = string.Empty;
                                        string empty4 = string.Empty;
                                        uint slot5 = 3703456078u;
                                        Vector3 vector2 = Vector3.Zero;
                                        Vector3 zero = Vector3.Zero;
                                        try
                                        {
                                            slot5 = (uint)Enum.Parse(typeof(HeldItem.ItemSlots), "RightHand", true);
                                        }
                                        catch
                                        {
                                        }
                                        Vector3.TryParse("-0.01,-0.00,0.08", out vector2);
                                        slot3.HeldItem = HeldItem.Create("0x02DC343F-0x00000000-0x0000000000DF58B3", vector2, zero, angle3, slot5);
                                        slot3.HeldItemAnimation = empty4;
                                    }
                                    slot3.Update();
                                }
                                foreach (Animation.Set value2 in position2.Sets.Values)
                                {
                                    if (value2 != null)
                                    {
                                        value2.Update();
                                    }
                                }
                                XML.Node matchingNode6 = item.GetMatchingNode("Action");
                                if (matchingNode6 == null)
                                {
                                    continue;
                                }
                                int num8 = PassionCommon.Int(matchingNode6["Partner"]) + 1;
                                if (num8 > 0)
                                {
                                    if (position2.InteractsWith.ContainsKey(num7))
                                    {
                                        position2.InteractsWith[num7] = num8;
                                    }
                                    else
                                    {
                                        position2.InteractsWith.Add(num7, num8);
                                    }
                                }
                            }
                            // END FOR EACH ACTOR


                            switch (matchingNode11["Category"].ToLower())
                            {
                                case "anal":
                                    position2.Categories |= 8;
                                    break;
                                case "vaginal":
                                    position2.Categories |= 4;
                                    break;
                                case "oraljob":
                                    position2.Categories |= 2;
                                    break;
                                case "handjob":
                                    position2.Categories |= 16;
                                    break;
                                case "teasing":
                                case "any":
                                    position2.Categories = 1;
                                    break;
                            }
                            string[] array5 = matchingNode11["Objects"].Split(',');
                            foreach (string text6 in array5)
                            {
                                switch (text6)
                                {
                                    case "Bathtub":
                                        position2.AddSupportedType<Bathtub>();
                                        position2.AddSupportedType<ShowerTub>();
                                        break;
                                    case "Shower":
                                    case "ShowerTub":
                                        position2.AddSupportedType<Bathtub>();
                                        position2.AddSupportedType<Shower>();
                                        position2.AddSupportedType<ShowerOutdoor>();
                                        position2.AddSupportedType<ShowerTub>();
                                        position2.AddSupportedType<ShowerPublic_Dance>();
                                        break;
                                    case "Couch":
                                        position2.AddSupportedType<Sofa>();
                                        position2.AddSupportedType<Loveseat>();
                                        break;
                                    case "HotTubBase":
                                        position2.AddSupportedType<HotTubBase>();
                                        position2.AddSupportedType<HotTubGrotto>();
                                        position2.AddSupportedType<HotTub4Seated>();
                                        break;
                                    case "ChairLiving":
                                        position2.AddSupportedType<ChairLiving>();
                                        position2.AddSupportedType<ChairSectional>();
                                        break;
                                    case "ChairLounge":
                                        position2.AddSupportedType<ChairLounge>();
                                        position2.AddSupportedType<SleepPodFuture>();
                                        position2.AddSupportedType<BrainEnhancingMachine>();
                                        break;
                                    case "CarSports":
                                    case "CarExpensive1":
                                    case "CarExpensive2":
                                        position2.AddSupportedType<CarSports>();
                                        position2.AddSupportedType<CarExpensive1>();
                                        position2.AddSupportedType<CarExpensive2>();
                                        break;
                                    case "CarHatchback":
                                        position2.AddSupportedType<CarHatchback>();
                                        break;
                                    case "CarUsed1":
                                        position2.AddSupportedType<CarUsed1>();
                                        break;
                                    case "CarUsed2":
                                        position2.AddSupportedType<CarUsed2>();
                                        break;
                                    case "CarNormal1":
                                        position2.AddSupportedType<CarNormal1>();
                                        break;
                                    case "CarVan4door":
                                        position2.AddSupportedType<CarVan4door>();
                                        break;
                                    case "CarPickup2door":
                                        position2.AddSupportedType<CarPickup2door>();
                                        break;
                                    case "CarSedan":
                                        position2.AddSupportedType<CarSedan>();
                                        break;
                                    case "CarHighSocietyOpen":
                                        position2.AddSupportedType<CarHighSocietyOpen>();
                                        break;
                                    case "CarHighSocietyVintage":
                                        position2.AddSupportedType<CarHighSocietyVintage>();
                                        break;
                                    case "CarLuxuryExotic":
                                        position2.AddSupportedType<CarLuxuryExotic>();
                                        break;
                                    case "CarLuxurySleek":
                                        position2.AddSupportedType<CarLuxurySleek>();
                                        break;
                                    case "CarLuxurySport":
                                        position2.AddSupportedType<CarLuxurySport>();
                                        break;
                                    case "DoorSingle":
                                        position2.AddSupportedType<DoorSingle>();
                                        break;
                                    case "MotorcycleRacing":
                                        position2.AddSupportedType<MotorcycleRacing>();
                                        break;
                                    case "MotorcycleChopper":
                                        position2.AddSupportedType<MotorcycleChopper>();
                                        break;
                                    case "BoatSpeedBoat":
                                        position2.AddSupportedType<BoatSpeedBoat>();
                                        break;
                                    case "BoatRowBoat":
                                        position2.AddSupportedType<BoatRowBoat>();
                                        break;
                                    case "BoatSpeedFishingBoat":
                                        position2.AddSupportedType<BoatSpeedFishingBoat>();
                                        break;
                                    case "BoatWaterScooter":
                                        position2.AddSupportedType<BoatWaterScooter>();
                                        break;
                                    case "AdultMagicBroom":
                                        position2.AddSupportedType<AdultMagicBroom>();
                                        break;
                                    case "ModerateAdultBroom":
                                        position2.AddSupportedType<ModerateAdultBroom>();
                                        break;
                                    case "ExpensiveAdultBroom":
                                        position2.AddSupportedType<ExpensiveAdultBroom>();
                                        break;
                                    case "Counter":
                                    case "CounterIsland":
                                        position2.AddSupportedType<Counter>();
                                        position2.AddSupportedType<CounterIsland>();
                                        break;
                                    case "TableDining":
                                        position2.AddSupportedType<TableEnd>();
                                        position2.AddSupportedType<TableDining1x1>();
                                        position2.AddSupportedType<TableDining2x1>();
                                        position2.AddSupportedType<TableDining3x1>();
                                        break;
                                    case "PicnicTable":
                                        position2.AddSupportedType<PicnicTable>();
                                        break;
                                    case "Urnstone":
                                        position2.AddSupportedType<Urnstone>();
                                        break;
                                    case "KissingBooth":
                                        position2.AddSupportedType<KissingBooth>();
                                        break;
                                    case "Telescope":
                                        position2.AddSupportedType<Telescope>();
                                        break;
                                    case "Scarecrow":
                                        position2.AddSupportedType<Scarecrow>();
                                        break;
                                    case "HauntedHouse":
                                        position2.AddSupportedType<HauntedHouse>();
                                        break;
                                    case "ScienceResearchStation":
                                        position2.AddSupportedType<ScienceResearchStation>();
                                        break;
                                    case "Podium":
                                        position2.AddSupportedType<Podium>();
                                        break;
                                    case "MechanicalBull":
                                        position2.AddSupportedType<MechanicalBull>();
                                        break;
                                    case "Stove":
                                        position2.AddSupportedType<Stove>();
                                        break;
                                    case "FenceRedwood_Gate":
                                        position2.AddSupportedType<FenceRedwood_Gate>();
                                        break;
                                    case "Rug":
                                        position2.AddSupportedType<Rug>();
                                        break;
                                    case "Sybian":
                                        position2.AddSupportedType<Sybian>();
                                        break;
                                    default:
                                        position2.AddSupportedType(text6);
                                        break;
                                }
                            }
                            position2.RegisterMinMaxSims();
                            if (PassionBase.Positions.ContainsKey(position2.Key))
                            {
                                PassionBase.Positions[position2.Key] = position2;
                            }
                            else
                            {
                                PassionBase.Positions.Add(position2.Key, position2);
                            }
                            num++;
                        }
                    }
                    XML.Node node3 = file["AnimatedWoohoo"];
                    if (node3 != null)
                    {
                        foreach (XML.Node matchingNode12 in node3.GetMatchingNodes("Position"))
                        {
                            string text7 = matchingNode12["animName"];
                            if (string.IsNullOrEmpty(text7))
                            {
                                continue;
                            }
                            Position position3 = new Position();
                            string text8 = matchingNode12["animText1"];
                            string text9 = matchingNode12["animText2"];
                            string text10 = text8 + text9;
                            position3.ClipPart1 = text8;
                            position3.ClipPart2 = text9;
                            position3.Clip = text7;
                            position3.Name = text7;
                            position3.Key = text7;
                            if (!string.IsNullOrEmpty(matchingNode12["animCreator"]))
                            {
                                position3.Creator = matchingNode12["animCreator"];
                            }
                            if (!string.IsNullOrEmpty(matchingNode12["category"]))
                            {
                                switch (matchingNode12["category"].ToLower())
                                {
                                    case "fuck":
                                        position3.Categories |= 12;
                                        break;
                                    case "oral":
                                        position3.Categories |= 2;
                                        break;
                                    case "masturbate":
                                        position3.Categories |= 16;
                                        break;
                                    case "any":
                                        position3.Categories = 1;
                                        break;
                                }
                            }
                            if (!string.IsNullOrEmpty(matchingNode12["minSims"]))
                            {
                                position3.MinSims = PassionCommon.Int(matchingNode12["minSims"]);
                            }
                            if (!string.IsNullOrEmpty(matchingNode12["maxSims"]))
                            {
                                position3.MaxSims = PassionCommon.Int(matchingNode12["maxSims"]);
                            }
                            if (!string.IsNullOrEmpty(matchingNode12["teenFixHeight"]))
                            {
                                position3.TeenHeightAdjustment = PassionCommon.Float(matchingNode12["teenFixHeight"]);
                            }
                            if (!string.IsNullOrEmpty(matchingNode12["possiblePregnancy"]))
                            {
                                position3.PossiblePregnancy = PassionCommon.Bool(matchingNode12["possiblePregnancy"]);
                            }
                            if (!string.IsNullOrEmpty(matchingNode12["possibleSameSexPregnancy"]))
                            {
                                position3.PossibleSameSexPregnancy = PassionCommon.Bool(matchingNode12["PossibleSameSexPregnancy"]);
                            }
                            position3.TeenAnimations = PassionCommon.Bool(matchingNode12["hasTeenAnims"]);
                            position3.FemaleAnimations = PassionCommon.Bool(matchingNode12["hasFemaleAnims"]);
                            position3.ComplexAnimations = PassionCommon.Bool(matchingNode12["hasComplexAnims"]);
                            for (int m = position3.MinSims; m <= position3.MaxSims; m++)
                            {
                                try
                                {
                                    string value = matchingNode12["sim" + m + "IDOverwrite"];
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        if (position3.SimID.ContainsKey(m))
                                        {
                                            position3.SimID[m] = value;
                                        }
                                        else
                                        {
                                            position3.SimID.Add(m, value);
                                        }
                                    }
                                    int num9 = PassionCommon.Int(matchingNode12["sim" + m + "InteractsWith"]);
                                    if (num9 > 0)
                                    {
                                        if (position3.InteractsWith.ContainsKey(m))
                                        {
                                            position3.InteractsWith[m] = num9;
                                        }
                                        else
                                        {
                                            position3.InteractsWith.Add(m, num9);
                                        }
                                    }
                                    Animation.Set set3 = new Animation.Set();
                                    position3.Sets.Add(m, set3);
                                    for (int n = 1; n <= m; n++)
                                    {
                                        Animation.Slot slot6 = set3.Add(n);
                                        slot6.Add(Animation.ClipData.CreateFromAnimatedWoohoo(text8, text9, n, m));
                                        slot6.Update();
                                    }
                                    set3.Update();
                                }
                                catch
                                {
                                }
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithSauna"]))
                            {
                                position3.AddSupportedType<SaunaClassic>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithBedSingle"]))
                            {
                                position3.AddSupportedType<BedSingle>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithBedDouble"]))
                            {
                                position3.AddSupportedType<BedDouble>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithAltar"]))
                            {
                                position3.AddSupportedType<Altar>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithMassageTable"]))
                            {
                                position3.AddSupportedType<MassageTable>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithToilet"]))
                            {
                                position3.AddSupportedType<Toilet>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithPole"]))
                            {
                                position3.AddSupportedType<SculptureFloorGunShow>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithCar"]))
                            {
                                position3.AddSupportedType<CarSports>();
                                position3.AddSupportedType<CarExpensive1>();
                                position3.AddSupportedType<CarExpensive2>();
                                position3.AddSupportedType<CarHatchback>();
                                position3.AddSupportedType<CarUsed1>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithFloor"]))
                            {
                                position3.AddSupportedType<Floor>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithWindow"]))
                            {
                                position3.AddSupportedType<Windows>();
                                position3.AddSupportedType<Fridge>();
                                position3.AddSupportedType<SculptureFloorGunShow>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithShowerTub"]))
                            {
                                position3.AddSupportedType<Bathtub>();
                                position3.AddSupportedType<ShowerTub>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithShower"]))
                            {
                                position3.AddSupportedType<Bathtub>();
                                position3.AddSupportedType<Shower>();
                                position3.AddSupportedType<ShowerTub>();
                                position3.AddSupportedType<ShowerOutdoor>();
                                position3.AddSupportedType<ShowerPublic_Dance>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithCounter"]))
                            {
                                position3.AddSupportedType<Counter>();
                                position3.AddSupportedType<CounterIsland>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithTableCoffee"]))
                            {
                                position3.AddSupportedType<TableCoffee>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithDiningTable"]))
                            {
                                position3.AddSupportedType<Desk>();
                                position3.AddSupportedType<TableEnd>();
                                position3.AddSupportedType<TableDining1x1>();
                                position3.AddSupportedType<TableDining2x1>();
                                position3.AddSupportedType<TableDining3x1>();
                                position3.AddSupportedType<Bicycle>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithChair"]))
                            {
                                position3.AddSupportedType<TableCoffee>();
                                position3.AddSupportedType<ChairLiving>();
                                position3.AddSupportedType<ChairDining>();
                                position3.AddSupportedType<ChairSectional>();
                                position3.AddSupportedType<Toilet>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithLounge"]))
                            {
                                position3.AddSupportedType<ChairLounge>();
                                position3.AddSupportedType<SleepPodFuture>();
                                position3.AddSupportedType<BrainEnhancingMachine>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithCouch"]))
                            {
                                position3.AddSupportedType<TableCoffee>();
                                position3.AddSupportedType<Sofa>();
                                position3.AddSupportedType<Loveseat>();
                            }
                            if (PassionCommon.Bool(matchingNode12["canBeUsedWithHottub"]))
                            {
                                position3.AddSupportedType<HotTub4Seated>();
                                position3.AddSupportedType<HotTubGrotto>();
                            }
                            position3.RegisterMinMaxSims();
                            if (PassionBase.Positions.ContainsKey(text7))
                            {
                                PassionBase.Positions[text7] = position3;
                            }
                            else
                            {
                                PassionBase.Positions.Add(text7, position3);
                            }
                            num++;
                        }
                    }
                }
            }
            return num;
        }

        public void AddHardCodedAnimation(int index, string animation)
        {
            if (HardCodedAnimationName == null)
            {
                HardCodedAnimationName = new Dictionary<int, string>();
            }
            if (HardCodedAnimationName.ContainsKey(index))
            {
                HardCodedAnimationName[index] = animation;
            }
            else
            {
                HardCodedAnimationName.Add(index, animation);
            }
        }

        public void AddSupportedType<T>()
        {
            AddSupportedType(PassionType.GetSupportedType(typeof(T)));
        }

        public void AddSupportedType(string name)
        {
            AddSupportedType(PassionType.GetSupportedType(name));
        }

        public void AddSupportedType(PassionType type)
        {
            if (type != null && type.IsValid && !SupportedTypes.ContainsKey(type.Name))
            {
                SupportedTypes.Add(type.Name, type);
            }
        }

        public void RegisterMinMaxSims()
        {
            PassionTarget.RegisterMinMaxSims(this);
        }

        public static IPositionChoice GetRandomValidPosition(PassionType type, int participants, int penises, int vaginas)
        {
            return GetRandomValidPosition(type, participants, penises, vaginas, 1, false);
        }

        public static IPositionChoice GetRandomValidPosition(PassionType type, int participants, int penises, int vaginas, int category, bool IsItASwitch)
        {
            List<IPositionChoice> list = new List<IPositionChoice>();
            if (IsItASwitch == true)
            {
                Part.BroWeAreSwitching = false;
                return null;
            }
            else
            {
                if (PassionCommon.Match(PersistableSettings.Settings.RandomizationOptions, RandomizationOptions.Positions))
                {
                    list.AddRange(GetValidPositions(type, participants, penises, vaginas, category));
                }
                if (PassionCommon.Match(PersistableSettings.Settings.RandomizationOptions, RandomizationOptions.Sequences))
                {
                    list.AddRange(GetValidSequences(type, participants, category));
                }
                if (list.Count > 0)
                {
                    int @int = RandomUtil.GetInt(0, list.Count - 1);
                    try
                    {
                        return list[@int];
                    }
                    catch
                    {
                    }
                }
                return null;
            }

        }

        public static List<IPositionChoice> GetValidSequences(PassionType type, int participants, int categories)
        {
            List<IPositionChoice> list = new List<IPositionChoice>();
            if (type != null && participants > 0)
            {
                foreach (Sequence sequence in PassionBase.Sequences)
                {
                    if (sequence.CanUseWith(participants) && sequence.CanUseWith(type) && sequence.IsCategory(categories))
                    {
                        list.Add((IPositionChoice)sequence);
                    }
                }
            }
            return list;
        }

        public static List<IPositionChoice> GetValidPositions(PassionType type, int participants, int penises, int vaginas, int categories)
        {
            List<IPositionChoice> list = new List<IPositionChoice>();
            if (type != null && participants > 0)
            {
                foreach (Position value in PassionBase.Positions.Values)
                {
                    if (value.CanUseWith(participants) && value.CanUseWith(type) && value.IsCategory(categories) && (!PersistableSettings.Settings.ExcludeInvalidPositions || (value.Sets.ContainsKey(participants) || value.Sets[participants] != null) && penises >= value.Sets[participants].PenisesNeeded && vaginas >= value.Sets[participants].VaginasNeeded))
                    {
                        list.Add(value);
                    }
                }
            }
            return list;
        }

        public static List<Position> GetValidPositions(PassionType type)
        {
            List<Position> list = new List<Position>();
            if (type != null)
            {
                foreach (Position value in PassionBase.Positions.Values)
                {
                    if (value.CanUseWith(type))
                    {
                        list.Add(value);
                    }
                }
            }
            return list;
        }

        public static void GetMinMaxSims(PassionType type, out int MinSims, out int MaxSims)
        {
            int num = int.MaxValue;
            int num2 = 0;
            if (type != null)
            {
                foreach (Position validPosition in GetValidPositions(type))
                {
                    if (validPosition.MinSims < num)
                    {
                        num = validPosition.MinSims;
                    }
                    if (validPosition.MaxSims > num2)
                    {
                        num2 = validPosition.MaxSims;
                    }
                }
            }
            if (num == int.MaxValue)
            {
                num = 1;
            }
            if (num2 == 0)
            {
                num2 = 1;
            }
            MinSims = num;
            MaxSims = num2;
        }

        public Position()
        {
            Key = "";
            Name = "";
            NamePrefix = "";
            Tones = 1;
            Categories = 0;
            SupportedTypes = new Dictionary<string, PassionType>();
            Sets = new Dictionary<int, Animation.Set>();
            Creator = string.Empty;
            mMinSims = 0;
            mMaxSims = 0;
            Clip = string.Empty;
            TeenAnimations = false;
            FemaleAnimations = false;
            ComplexAnimations = false;
            HardCodedAnimationName = null;
            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
            dictionary.Add(1, true);
            dictionary.Add(2, false);
            dictionary.Add(3, false);
            dictionary.Add(4, false);
            dictionary.Add(5, false);
            dictionary.Add(6, false);
            NeedsPenis = dictionary;
            Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
            dictionary2.Add(1, false);
            dictionary2.Add(2, false);
            dictionary2.Add(3, false);
            dictionary2.Add(4, false);
            dictionary2.Add(5, false);
            dictionary2.Add(6, false);
            NeedsVagina = dictionary2;
            Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
            dictionary3.Add(1, 2);
            dictionary3.Add(2, 1);
            dictionary3.Add(3, 1);
            dictionary3.Add(4, 1);
            dictionary3.Add(5, 1);
            dictionary3.Add(6, 1);
            InteractsWith = dictionary3;
            Dictionary<int, string> dictionary4 = new Dictionary<int, string>();
            dictionary4.Add(1, "A");
            dictionary4.Add(2, "B");
            dictionary4.Add(3, "C");
            dictionary4.Add(4, "D");
            dictionary4.Add(5, "E");
            dictionary4.Add(6, "F");
            SimID = dictionary4;
            Dictionary<int, bool> dictionary5 = new Dictionary<int, bool>();
            dictionary5.Add(1, false);
            dictionary5.Add(2, false);
            dictionary5.Add(3, false);
            dictionary5.Add(4, false);
            dictionary5.Add(5, false);
            dictionary5.Add(6, false);
            PutOnStraOn = dictionary5;
            Dictionary<int, bool> dictionary6 = new Dictionary<int, bool>();
            dictionary6.Add(1, false);
            dictionary6.Add(2, false);
            dictionary6.Add(3, false);
            dictionary6.Add(4, false);
            dictionary6.Add(5, false);
            dictionary6.Add(6, false);
            ActorPosition = dictionary6;
        }

        public bool CanUseWith(Dictionary<string, PassionType> types, int minsims, int maxsims)
        {
            return CanUseWith(types, minsims, maxsims, 1);
        }

        public bool CanUseWith(Dictionary<string, PassionType> types, int minsims, int maxsims, int categories)
        {
            if (minsims <= MinSims && maxsims >= MaxSims && IsCategory(categories))
            {
                foreach (PassionType value in types.Values)
                {
                    if (CanUseWith(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanUseWith(PassionType type, int participants, int penises, int vaginas, int categories)
        {
            bool result = true;
            if (type != null && !SupportedTypes.ContainsKey(type.Name))
            {
                result = false;
            }
            if (participants > 0 && (participants < MinSims || participants > MaxSims))
            {
                result = false;
            }
            if (categories != 0 && !IsCategory(categories))
            {
                result = false;
            }
            if (PersistableSettings.Settings.ExcludeInvalidPositions && Sets.ContainsKey(participants) && Sets[participants] != null)
            {
                if (penises < Sets[participants].PenisesNeeded)
                {
                    result = false;
                }
                if (vaginas < Sets[participants].VaginasNeeded)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool CanUseWith(int participants)
        {
            return participants >= MinSims && participants <= MaxSims;
        }

        public bool CanUseWith(PassionType type)
        {
            return type != null && SupportedTypes.ContainsKey(type.Name);
        }

        public void AddType(PassionType type)
        {
            if (type != null && !CanUseWith(type))
            {
                SupportedTypes.Add(type.Name, type);
            }
        }

        public void RemoveType(PassionType type)
        {
            if (type != null && CanUseWith(type))
            {
                SupportedTypes.Remove(type.Name);
            }
        }

        public bool IsCategory(int category)
        {
            return category == 1 || Categories == 1 || (category & Categories) > 0;
        }

        public Animation.Set GetSet(int participants)
        {
            if (Sets != null && Sets.ContainsKey(participants))
            {
                return Sets[participants];
            }
            return null;
        }

        public Animation.Slot GetSlot(int participants, int slot)
        {
            Animation.Set set = GetSet(participants);
            if (set != null)
            {
                return set.GetSlot(slot);
            }
            return null;
        }

        public string GetAnimation(Player player)
        {
            Animation.ClipData clip = GetClip(player);
            if (clip != null)
            {
                return clip.Clip;
            }
            return string.Empty;
        }

        public Animation.ClipData GetClip(Player player)
        {
            try
            {
                if (player != null && player.IsValid && player.HasPart)
                {
                    int count = player.Part.Count;
                    if (Sets.ContainsKey(count) && Sets[count] != null)
                    {
                        return Sets[count].GetClip(player);
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        public void GiveHeldItems(List<Player> players)
        {
            Animation.Set set = GetSet(players.Count);
            if (set == null)
            {
                return;
            }
            foreach (Player player in players)
            {
                if (player == null || !player.IsValid)
                {
                    continue;
                }
                Animation.Slot slot = set.GetSlot(player.PositionIndex);
                if (slot == null)
                {
                    continue;
                }
                if (slot.HasHeldItem)
                {
                    if (World.ResourceExists(ResourceKey.FromString("0x02DC343F-0x00000000-0x00000000590CD0E0")) && World.ResourceExists(ResourceKey.FromString("0x02DC343F-0x00000000-0x0000000059C1465B")) && World.ResourceExists(ResourceKey.FromString("0x02DC343F-0x00000000-0x00000000042C4C21")) && World.ResourceExists(ResourceKey.FromString("0x02DC343F-0x00000000-0x000000000D4B42F6")) && World.ResourceExists(ResourceKey.FromString("0x02DC343F-0x00000000-0x000000002734DB7A")) && World.ResourceExists(ResourceKey.FromString("0x02DC343F-0x00000000-0x000000004E6DFBC4")) && World.ResourceExists(ResourceKey.FromString("0x02DC343F-0x00000000-0x0000000038B3F337")) && World.ResourceExists(ResourceKey.FromString("0x02DC343F-0x00000000-0x000000004964A3B5")) && (slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x780597AC522896FB") || slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x00000000590CD0E0") || slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x0000000059C1465B") || slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x00000000042C4C21") || slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x000000000D4B42F6") || slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x000000002734DB7A") || slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x000000004E6DFBC4") || slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x0000000038B3F337") || slot.HeldItem.Key == ResourceKey.FromString("0x02DC343F-0x00000000-0x000000004964A3B5")))
                    {
                        if (RandomUtil.CoinFlip())
                        {
                            if (RandomUtil.CoinFlip())
                            {
                                if (RandomUtil.CoinFlip())
                                {
                                    slot.HeldItem.Key = ResourceKey.FromString("0x02DC343F-0x00000000-0x780597AC522896FB");
                                }
                                else
                                {
                                    slot.HeldItem.Key = ResourceKey.FromString("0x02DC343F-0x00000000-0x0000000059C1465B");
                                }
                            }
                            else if (RandomUtil.CoinFlip())
                            {
                                slot.HeldItem.Key = ResourceKey.FromString("0x02DC343F-0x00000000-0x00000000042C4C21");
                            }
                            else
                            {
                                slot.HeldItem.Key = ResourceKey.FromString("0x02DC343F-0x00000000-0x000000000D4B42F6");
                            }
                        }
                        else if (RandomUtil.CoinFlip())
                        {
                            if (!RandomUtil.CoinFlip())
                            {
                                slot.HeldItem.Key = ResourceKey.FromString("0x02DC343F-0x00000000-0x000000002734DB7A");
                            }
                            else
                            {
                                slot.HeldItem.Key = ResourceKey.FromString("0x02DC343F-0x00000000-0x000000004E6DFBC4");
                            }
                        }
                        else if (RandomUtil.CoinFlip())
                        {
                            slot.HeldItem.Key = ResourceKey.FromString("0x02DC343F-0x00000000-0x0000000038B3F337");
                        }
                        else
                        {
                            slot.HeldItem.Key = ResourceKey.FromString("0x02DC343F-0x00000000-0x000000004964A3B5");
                        }
                    }
                    if (player.HeldItem == null || player.HeldItem.Key != slot.HeldItem.Key)
                    {
                        slot.HeldItem.Copy(player);
                    }
                }
                else
                {
                    if (player.HeldItem != null)
                    {
                        player.HeldItem.Release();
                    }
                    player.HeldItem = null;
                }
            }
        }


        public static IPositionChoice ChoosePositionDialog(PassionType type, int participants, int penises, int vaginas)
        {
            IPositionChoice result = null;



            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Position"), null, 250));
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Creator"), null, 200));
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Category"), null, 100));
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Participants"), null, 50));
            List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
            list2.Add(new ObjectPicker.TabInfo("boe_img_icon_allanim", PassionCommon.Localize("S3_Passion.Terms.All"), PositionListForDialog(type, participants, penises, vaginas)));
            ObjectPicker.TabInfo tabInfo = new ObjectPicker.TabInfo("boe_img_icon_handjob", PassionCommon.Localize("S3_Passion.Terms.Masturbate"), PositionListForDialog(type, participants, penises, vaginas, 16));
            if (tabInfo.RowInfo.Count >= 0)
            {
                list2.Add(tabInfo);
            }
            ObjectPicker.TabInfo tabInfo2 = new ObjectPicker.TabInfo("boe_img_icon_oral", PassionCommon.Localize("S3_Passion.Terms.Oral"), PositionListForDialog(type, participants, penises, vaginas, 2));
            if (tabInfo2.RowInfo.Count >= 0)
            {
                list2.Add(tabInfo2);
            }
            ObjectPicker.TabInfo tabInfo3 = new ObjectPicker.TabInfo("boe_img_icon_vaginal", PassionCommon.Localize("S3_Passion.Terms.Vaginal"), PositionListForDialog(type, participants, penises, vaginas, 4));
            if (tabInfo3.RowInfo.Count >= 0)
            {
                list2.Add(tabInfo3);
            }
            ObjectPicker.TabInfo tabInfo4 = new ObjectPicker.TabInfo("boe_img_icon_anal", PassionCommon.Localize("S3_Passion.Terms.Anal"), PositionListForDialog(type, participants, penises, vaginas, 8));
            if (tabInfo4.RowInfo.Count >= 0)
            {
                list2.Add(tabInfo4);
            }
            List<ObjectPicker.RowInfo> list3 = MenuList.Show(PassionCommon.Localize("S3_Passion.Terms.ChangePosition"), PassionCommon.Localize("S3_Passion.Terms.Ok"), PassionCommon.Localize("S3_Passion.Terms.Cancel"), list2, list);

            if (list3 != null && list3.Count > 0)
            {
                object wildCard = list3[0].Item;
                try
                {

                    if (wildCard.Equals(6942069))
                    {
                        result = GetRandomValidPosition(type, participants, penises, vaginas);
                    }
                    else
                    {
                        result = list3[0].Item as IPositionChoice;
                    }

                }
                catch
                {
                }
            }
            else
            {
                result = GetRandomValidPosition(type, participants, penises, vaginas);
            }
            return result;
        }

        public static List<ObjectPicker.RowInfo> PositionListForDialog(PassionType type, int participants, int penises, int vaginas)
        {
            return PositionListForDialog(type, participants, penises, vaginas, 1);
        }

        public static List<ObjectPicker.RowInfo> PositionListForDialog(PassionType type, int participants, int penises, int vaginas, int category)
        {
            List<ObjectPicker.RowInfo> list = new List<ObjectPicker.RowInfo>();
            foreach (Sequence sequence in PassionBase.Sequences)
            {
                if (sequence.CanUseWith(type, participants, category))
                {
                    ObjectPicker.RowInfo rowInfo = new ObjectPicker.RowInfo(sequence, new List<ObjectPicker.ColumnInfo>());
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("\"" + sequence.Name + "\""));
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(" -" + PassionCommon.Localize("S3_Passion.Terms.Sequence") + "- "));
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(Category.ToString(sequence.Categories)));
                    if (sequence.MinSims == sequence.MaxSims)
                    {
                        rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(sequence.MinSims.ToString()));
                    }
                    else
                    {
                        rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(sequence.MinSims + "-" + sequence.MaxSims));
                    }
                    list.Add(rowInfo);
                }
            }
            foreach (Position value in PassionBase.Positions.Values)
            {
                if (value.CanUseWith(type, participants, penises, vaginas, category))
                {
                    ObjectPicker.RowInfo rowInfo2 = new ObjectPicker.RowInfo(value, new List<ObjectPicker.ColumnInfo>());
                    rowInfo2.ColumnInfo.Add(new ObjectPicker.TextColumn(value.NamePrefix + PassionCommon.Localize(value.Name)));
                    rowInfo2.ColumnInfo.Add(new ObjectPicker.TextColumn(value.Creator));
                    rowInfo2.ColumnInfo.Add(new ObjectPicker.TextColumn(Category.ToString(value.Categories)));
                    if (value.MinSims == value.MaxSims)
                    {
                        rowInfo2.ColumnInfo.Add(new ObjectPicker.TextColumn(value.MinSims.ToString()));
                    }
                    else
                    {
                        rowInfo2.ColumnInfo.Add(new ObjectPicker.TextColumn(value.MinSims + "-" + value.MaxSims));
                    }
                    list.Add(rowInfo2);
                }
            }

            // add random option (i hope)

            ObjectPicker.RowInfo rowInfo69 = new ObjectPicker.RowInfo(6942069, new List<ObjectPicker.ColumnInfo>());
            rowInfo69.ColumnInfo.Add(new ObjectPicker.TextColumn("((!! == CHOOSE FOR ME ==!!))"));
            rowInfo69.ColumnInfo.Add(new ObjectPicker.TextColumn("Roll the dice!"));
            rowInfo69.ColumnInfo.Add(new ObjectPicker.TextColumn(""));
            rowInfo69.ColumnInfo.Add(new ObjectPicker.TextColumn(""));
            list.Add(rowInfo69);

            // end add random

            return list;
        }

        public static Position ChooseSequencePositionDialog(Dictionary<string, PassionType> types, int minsims, int maxsims)
        {
            Position result = null;
            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Position"), null, 250));
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Creator"), null, 200));
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Types"), null, 200));
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Participants"), null, 50));
            List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
            list2.Add(new ObjectPicker.TabInfo("shop_all_r2", PassionCommon.Localize("S3_Passion.Terms.All"), PositionListForSequenceDialog(types, minsims, maxsims)));
            ObjectPicker.TabInfo tabInfo = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.Masturbate"), PositionListForSequenceDialog(types, minsims, maxsims, 16));
            if (tabInfo.RowInfo.Count >= 0)
            {
                list2.Add(tabInfo);
            }
            ObjectPicker.TabInfo tabInfo2 = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.Oral"), PositionListForSequenceDialog(types, minsims, maxsims, 2));
            if (tabInfo2.RowInfo.Count >= 0)
            {
                list2.Add(tabInfo2);
            }
            ObjectPicker.TabInfo tabInfo3 = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.Vaginal"), PositionListForSequenceDialog(types, minsims, maxsims, 4));
            if (tabInfo3.RowInfo.Count >= 0)
            {
                list2.Add(tabInfo3);
            }
            ObjectPicker.TabInfo tabInfo4 = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.Anal"), PositionListForSequenceDialog(types, minsims, maxsims, 8));
            if (tabInfo4.RowInfo.Count >= 0)
            {
                list2.Add(tabInfo4);
            }
            List<ObjectPicker.RowInfo> list3 = MenuList.Show(PassionCommon.Localize("S3_Passion.Terms.SelectPosition"), PassionCommon.Localize("S3_Passion.Terms.Ok"), PassionCommon.Localize("S3_Passion.Terms.Cancel"), list2, list);
            if (list3 != null && list3.Count > 0)
            {
                try
                {

                    result = list3[0].Item as Position;

                }
                catch
                {
                }
            }
            return result;
        }

        public static List<ObjectPicker.RowInfo> PositionListForSequenceDialog(Dictionary<string, PassionType> types, int minsims, int maxsims)
        {
            return PositionListForSequenceDialog(types, minsims, maxsims, 1);
        }

        public static List<ObjectPicker.RowInfo> PositionListForSequenceDialog(Dictionary<string, PassionType> types, int minsims, int maxsims, int categories)
        {
            List<ObjectPicker.RowInfo> list = new List<ObjectPicker.RowInfo>();
            foreach (Position value in PassionBase.Positions.Values)
            {
                if (!value.CanUseWith(types, minsims, maxsims, categories))
                {
                    continue;
                }
                ObjectPicker.RowInfo rowInfo = new ObjectPicker.RowInfo(value, new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(value.NamePrefix + PassionCommon.Localize(value.Name)));
                string text = string.Empty;
                foreach (PassionType value2 in value.SupportedTypes.Values)
                {
                    text = text + (text.Length > 0 ? ", " : "") + value2.Name;
                }
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(text));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(value.Creator));
                if (value.MinSims == value.MaxSims)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(value.MinSims.ToString()));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(value.MinSims + "-" + value.MaxSims));
                }
                list.Add(rowInfo);
            }
            return list;
        }
    }
}
