using System.Collections.Generic;
using System.Xml;
using S3_Passion;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Lovemaking;
using S3_Passion.BOE_Settings;
using S3_Passion.BOE_UI;
using S3_Passion.BOE_STD;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;

namespace S3_Passion.BOE_Settings
{


    [Persistable]
    public class PersistableSettings
    {
        public enum Setting
        {
            AutonomyChance,
            AutonomyActive,
            AutonomyPublic,
            AutonomyNotify,
            EndWhenFull,
            PregnancyRisk,
            PregnancyMethod,
            PregnancyMale,
            STDSimmunity,
            ConsentPercentage,
            StrapOnMode,
            NakedShower,
            GetSoft,
            GetHard,
            STD,
            RelationshipGain,
            RelationshipLoss,
            Teen,
            CanReject,
            Incest,
            Reactions,
            Label,
            SoloLabel,
            ActiveLabel,
            Motives,
            InitialCategory,
            Outfit,
            Sequences,
            Positions,
            Eligibility,
            Reset,
            ExportImport,
            ExportImportSequence,
            MaxLength,
            RandomizationOptions,
            Jealousy,
            Moodlets,
            ActiveAlwaysAccepts,
            PolyamorousJealousy,
            ExludeInvalidPositions,
            StripperAutonomy,
            BroadCasterEnable,
            FemaleUseStrapOn,
            ObjectAnimation,
            UseCondom,
            RemoveConom,
            CondomIsBroken,
            PassionFuckSession,
            CondomBrakeChance,
            FemalePublicHair,
            VampireInteractions,
            ChildrenOut,
            PetsOut,
            EldersOut,
            VampiresVisitAtNight,
            VampireHypnoSex,
            EvilFighter,
            RemoveVisualOverride
        }

        public class SettingItem
        {
            public Setting Name;

            public object Value;

            public SettingItem(Setting name, object value)
            {
                Name = name;
                Value = value;
            }
        }

        public int AutonomyChance;

        public bool AutonomyActive;

        public bool AutonomyPublic;

        public long AutonomyLength;

        public bool AutonomyNotify;

        public bool EndWhenFull;

        public bool ExcludeInvalidPositions;

        public bool BroadCasterEnable;

        public bool FemaleUseStrapOn;

        public bool StripperAutonomy;

        public int InitialCategory;

        public int PregnancyRisk;

        public PregnancyMethod PregnancyMethod;

        public bool PregnancyMale;

        public STDImmunity STDSimmunity;

        public long MaxLength;

        public long RandomizationLength;

        public RandomizationOptions RandomizationOptions;

        public float ConsentPercentage;

        public float ConsentMinimum;

        public float ConsentMaximum;

        public float RelationshipGain;

        public float RelationshipLoss;

        public bool ActiveAlwaysAccepts;

        public bool StrapOnMode;

        public bool NakedShower;

        public bool GetSoft;

        public bool GetHard;

        public bool STD;

        public bool CanReject;

        public bool Teen;

        public bool Incest;

        public bool Reactions;

        public bool Jealousy;

        public bool PolyamorousJealousy;

        public bool LibidoBuff = false;

        public bool WoohooBuff = true;

        public string Label;

        public string SoloLabel;

        public string ActiveLabel;

        public PassionMotives Motives;

        public OutfitCategories Outfit;

        public List<IGameObject> UsedTables;

        public string ObjectAnimation;

        public bool UseCondom = false;

        public bool RemoveCondom = false;

        public bool CondomIsBroken = false;

        public bool PassionFuckSession = false;

        public int CondomBrakeChance = 0;

        public bool VampireInteractions;

        public bool ChildrenOut;

        public bool PetsOut;

        public bool EldersOut;

        public bool VampiresVisitAtNight;

        public bool VampireHypnoSex;

        public bool EvilFighter;

        public bool RemoveVisualOverride;

        public PersistableSettings()
        {
            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            AutonomyChance = 25;
            AutonomyLength = 2220L;
            AutonomyActive = true;
            AutonomyPublic = true;
            AutonomyNotify = true;
            EndWhenFull = false;
            ExcludeInvalidPositions = false;
            BroadCasterEnable = true;
            FemaleUseStrapOn = true;
            StripperAutonomy = true;
            PregnancyRisk = 0;
            PregnancyMethod = PregnancyMethod.Disabled;
            PregnancyMale = false;
            Outfit = OutfitCategories.Naked;
            Motives = PassionMotives.PassionStandard;
            InitialCategory = 1;
            MaxLength = 0L;
            RandomizationLength = 2222L;
            RandomizationOptions = RandomizationOptions.PositionsAndSequences;
            STDSimmunity = STDImmunity.Immune;
            ConsentPercentage = 0.01f;
            ConsentMinimum = 0.01f;
            ConsentMaximum = 0.99f;
            RelationshipGain = 15f;
            RelationshipLoss = -8f;
            ActiveAlwaysAccepts = true;
            STD = false;
            GetSoft = true;
            GetHard = true;
            StrapOnMode = true;
            NakedShower = true;
            Teen = true;
            CanReject = true;
            Incest = true;
            Reactions = true;
            Jealousy = false;
            PolyamorousJealousy = false;
            LibidoBuff = true;
            WoohooBuff = true;
            Label = "Sex";
            ActiveLabel = "having sex with";
            SoloLabel = "Masturbation";
            ObjectAnimation = null;
            UseCondom = false;
            RemoveCondom = false;
            CondomIsBroken = false;
            PassionFuckSession = false;
            CondomBrakeChance = 0;
            VampireInteractions = true;
            ChildrenOut = false;
            PetsOut = false;
            EldersOut = false;
            VampiresVisitAtNight = true;
            VampireHypnoSex = true;
            EvilFighter = true;
            RemoveVisualOverride = true;
        }

        public static void Show()
        {
            bool flag = true;
            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Setting"), null, 250));
            list.Add(new ObjectPicker.HeaderInfo(PassionCommon.Localize("S3_Passion.Terms.Value"), null, 200));
            while (flag)
            {
                // settings UI display
                List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
                List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();
                ObjectPicker.RowInfo rowInfo = null;
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Reset, true), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.ResetToDefaults")));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(" "));
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.ExportImport, true), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("* " + PassionCommon.Localize("S3_Passion.Terms.SettingsFile") + " *"));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.ImportExport")));
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.ExportImportSequence, true), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("** " + PassionCommon.Localize("S3_Passion.Terms.Sequence.SettingsFile") + " **"));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Sequence.ImportExportSequence")));
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.AutonomyChance, Settings.AutonomyChance), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("* " + PassionCommon.Localize("S3_Passion.Terms.Autonomy") + " *"));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(Settings.AutonomyChance > 0 ? Settings.AutonomyChance + "%, " + PassionCommon.TicksToMinutes(Settings.AutonomyLength) + " Min." : PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Eligibility, true), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("* " + PassionCommon.Localize("S3_Passion.Terms.Eligibility") + " *"));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("A " + (Settings.Incest ? PassionCommon.Localize("S3_Passion.Terms.Enabled") : string.Empty) + ", T " + (Settings.Teen ? PassionCommon.Localize("S3_Passion.Terms.Enabled") : string.Empty)));
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Positions, PassionBase.Positions), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("* " + PassionCommon.Localize("S3_Passion.Terms.Positions") + " *"));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("(" + PassionBase.Positions.Count + ")"));
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Sequences, PassionBase.Sequences), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("* " + PassionCommon.Localize("S3_Passion.Terms.Sequences") + " *"));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("(" + PassionBase.Sequences.Count + ")"));
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.EndWhenFull, Settings.EndWhenFull), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.EndWhenFull")));
                if (Settings.EndWhenFull)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.ExludeInvalidPositions, Settings.ExcludeInvalidPositions), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.ExcludeInvalidPositions")));
                if (Settings.ExcludeInvalidPositions)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.ChildrenOut, Settings.ChildrenOut), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.ChildrenOut")));
                if (Settings.ChildrenOut)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.PetsOut, Settings.PetsOut), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.PetsOut")));
                if (Settings.PetsOut)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.EldersOut, Settings.EldersOut), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.EldersOut")));
                if (Settings.EldersOut)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.BroadCasterEnable, Settings.BroadCasterEnable), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.BroadCasterEnable")));
                if (Settings.BroadCasterEnable)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.FemaleUseStrapOn, Settings.FemaleUseStrapOn), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.FemaleUseStrapOn")));
                if (Settings.FemaleUseStrapOn)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.StripperAutonomy, Settings.StripperAutonomy), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.StripperAutonomy")));
                if (Settings.StripperAutonomy)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.InitialCategory, Settings.InitialCategory), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.InitialCategory")));
                if (Settings.InitialCategory == 1024)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Choose")));
                }
                else if (Settings.InitialCategory == 12)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Fuck")));
                }
                else if (Settings.InitialCategory == 48)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Masturbate")));
                }
                else if (Settings.InitialCategory == 2)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Oral")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Any")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Jealousy, Settings.Jealousy), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Jealousy")));
                if (Settings.Jealousy)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled") + (Settings.PolyamorousJealousy ? "*" : "")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Label, Settings.Label), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Label")));
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(Settings.Label + "/" + Settings.ActiveLabel + "/" + Settings.SoloLabel));
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.MaxLength, Settings.MaxLength), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.MaxLength")));
                if (Settings.MaxLength < 10)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Infinite")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.TicksToMinutes(Settings.MaxLength) + " Minutes"));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Moodlets, true), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Moodlets")));
                if (Settings.LibidoBuff && Settings.WoohooBuff)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Libido") + ", " + PassionCommon.Localize("S3_Passion.Terms.WoohooBuff")));
                }
                else if (Settings.LibidoBuff)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Libido")));
                }
                else if (Settings.WoohooBuff)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.WoohooBuff")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.None")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Motives, Settings.Motives), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Motives")));
                if (Settings.Motives == PassionMotives.NoDecay)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.NoDecay")));
                }
                else if (Settings.Motives == PassionMotives.PassionStandard)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.PassionStandard")));
                }
                else if (Settings.Motives == PassionMotives.Freeze)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Freeze")));
                }
                else if (Settings.Motives == PassionMotives.MaxAll)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.MaxAll")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.EADefault")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Outfit, Settings.Outfit), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Outfit")));
                if (Settings.Outfit == OutfitCategories.Naked)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Naked")));
                }
                else if (Settings.Outfit == OutfitCategories.Sleepwear)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Sleepwear") + (Settings.NakedShower ? "*" : "")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.PreferredOnly") + (Settings.NakedShower ? "*" : "")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.PregnancyMethod, Settings.PregnancyMethod), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Pregnancy")));
                if (Settings.PregnancyMethod == PregnancyMethod.ByCategory)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Category") + " (" + Settings.PregnancyRisk + "%)"));
                }
                else if (Settings.PregnancyMethod == PregnancyMethod.ByPosition)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Position") + " (" + Settings.PregnancyRisk + "%)"));
                }
                else if (Settings.PregnancyMethod == PregnancyMethod.KWSystem)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.KWSystem") + " (" + Settings.PregnancyRisk + "%)"));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.UseCondom, Settings.UseCondom), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.UseCondom")));
                if (Settings.UseCondom)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.CondomBrakeChance") + " (" + Settings.CondomBrakeChance + "%)"));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.RandomizationOptions, Settings.RandomizationOptions), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Randomization")));
                if ((RandomizationOptions.PositionsAndSequences & Settings.RandomizationOptions) == RandomizationOptions.PositionsAndSequences)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Positions") + "/" + PassionCommon.Localize("S3_Passion.Terms.Sequences") + (Settings.RandomizationLength > 0 ? " (" + PassionCommon.TicksToMinutes(Settings.RandomizationLength) + " min.)" : string.Empty) + (PassionCommon.Match(RandomizationOptions.SameCategory, Settings.RandomizationOptions) ? "*" : string.Empty)));
                }
                else if (PassionCommon.Match(RandomizationOptions.Sequences, Settings.RandomizationOptions))
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Sequences") + (Settings.RandomizationLength > 0 ? " (" + PassionCommon.TicksToMinutes(Settings.RandomizationLength) + " min.)" : string.Empty) + (PassionCommon.Match(RandomizationOptions.SameCategory, Settings.RandomizationOptions) ? "*" : string.Empty)));
                }
                else if (PassionCommon.Match(RandomizationOptions.Positions, Settings.RandomizationOptions))
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Positions") + (Settings.RandomizationLength > 0 ? " (" + PassionCommon.TicksToMinutes(Settings.RandomizationLength) + " min.)" : string.Empty) + (PassionCommon.Match(RandomizationOptions.SameCategory, Settings.RandomizationOptions) ? "*" : string.Empty)));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("  "));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.GetHard, Settings.GetHard), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.PenisAddOn")));
                if (Settings.GetHard)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.HardBefore") + (Settings.GetSoft ? ", " + PassionCommon.Localize("S3_Passion.Terms.SoftAfter") + (Settings.StrapOnMode ? "*" : string.Empty) : string.Empty)));
                }
                else if (Settings.GetSoft)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.SoftAfter") + (Settings.StrapOnMode ? "*" : string.Empty)));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Neither")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.CanReject, Settings.CanReject), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Rejection")));
                if (Settings.CanReject && Settings.RelationshipLoss == -15f)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled") + " (--)"));
                }
                else if (Settings.CanReject && Settings.RelationshipLoss == -8f)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled") + " (-)"));
                }
                else if (Settings.CanReject)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.RelationshipGain, Settings.RelationshipGain), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.RelationshipGain")));
                if (Settings.RelationshipGain == 15f)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled") + " (++)"));
                }
                else if (Settings.RelationshipGain == 8f)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled") + " (+)"));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.STD, Settings.STD), new List<ObjectPicker.ColumnInfo>());
                rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.STDs")));
                if (Settings.STD)
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Enabled")));
                }
                else
                {
                    rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Disabled")));
                }
                list3.Add(rowInfo);
                ObjectPicker.TabInfo tabInfo = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.General"), list3);
                if (tabInfo.RowInfo.Count >= 0)
                {
                    list2.Add(tabInfo);
                }
                List<ObjectPicker.RowInfo> list4 = MenuList.Show(PassionCommon.Localize("S3_Passion.Terms.Settings") + "\rBORDEROFEXTACY_Passion, Version " + PassionCommon.Version + "", PassionCommon.Localize("S3_Passion.Terms.Ok"), PassionCommon.Localize("S3_Passion.Terms.Cancel"), list2, list);
                if (list4 == null || list4.Count <= 0)
                {
                    break;
                }
                SettingItem settingItem;
                try
                {
                    settingItem = list4[0].Item as SettingItem;
                }
                catch
                {
                    settingItem = null;
                }
                if (settingItem == null)
                {
                    continue;
                }
                string text = null;
                switch (settingItem.Name)
                {
                    case Setting.Reset:
                        if (!PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.ResetToDefaultsConfirmation"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
                        {
                            break;
                        }
                        foreach (Player value in PassionBase.AllPlayers.Values)
                        {
                            value.Reset();
                        }
                        PassionBase.AllPlayers.Clear();
                        foreach (PassionTarget value2 in PassionBase.AllTargets.Values)
                        {
                            value2.Object = null;
                        }
                        PassionBase.AllTargets.Clear();
                        if (PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.ResetSequences"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
                        {
                            PassionBase.Sequences.Clear();
                        }
                        PassionBase.ReloadDefaultPositions();
                        PassionCommon.CleanMoodlets(CustomBuff.Names.Libido);
                        Settings.ResetToDefaults();
                        break;
                    case Setting.MaxLength:
                        text = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.MaxLength"), PassionCommon.Localize("S3_Passion.Terms.MaxLengthText"), PassionCommon.TicksToMinutes(Settings.MaxLength).ToString());
                        if (text != null)
                        {
                            int num4 = PassionCommon.Int(text);
                            if (num4 < 10)
                            {
                                num4 = 0;
                            }
                            Settings.MaxLength = PassionCommon.MinutesToTicks(num4);
                        }
                        break;
                    case Setting.AutonomyChance:
                        {
                            text = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.Autonomy"), PassionCommon.Localize("S3_Passion.Terms.AutonomyText"), Settings.AutonomyChance.ToString());
                            if (text == null)
                            {
                                break;
                            }
                            int num5 = PassionCommon.Int(text);
                            if (num5 < 0)
                            {
                                num5 = 0;
                            }
                            if (num5 > 100)
                            {
                                num5 = 100;
                            }
                            Settings.AutonomyChance = num5;
                            if (num5 <= 0)
                            {
                                break;
                            }
                            text = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.AutonomyLength"), PassionCommon.Localize("S3_Passion.Terms.AutonomyLengthText"), PassionCommon.TicksToMinutes(Settings.AutonomyLength).ToString());
                            if (text != null)
                            {
                                int num6 = PassionCommon.Int(text);
                                if (num6 < 10)
                                {
                                    num6 = 10;
                                }
                                Settings.AutonomyLength = PassionCommon.MinutesToTicks(num6);
                            }
                            Settings.AutonomyPublic = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.AutonomyPublicText"), Settings.AutonomyPublic, PassionCommon.Localize("S3_Passion.Terms.Enabled"), PassionCommon.Localize("S3_Passion.Terms.Disabled"));
                            Settings.AutonomyActive = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.AutonomyActiveText"), Settings.AutonomyActive, PassionCommon.Localize("S3_Passion.Terms.Enabled"), PassionCommon.Localize("S3_Passion.Terms.Disabled"));
                            Settings.AutonomyNotify = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.AutonomyNotifyText"), Settings.AutonomyNotify, PassionCommon.Localize("S3_Passion.Terms.Enabled"), PassionCommon.Localize("S3_Passion.Terms.Disabled"));
                            break;
                        }
                    case Setting.Eligibility:
                        Settings.Incest = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.AllowNearRelation"), Settings.Incest, PassionCommon.Localize("S3_Passion.Terms.Enabled"), PassionCommon.Localize("S3_Passion.Terms.Disabled"));
                        if (PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.AllowTeen"), Settings.Teen, PassionCommon.Localize("S3_Passion.Terms.Enabled"), PassionCommon.Localize("S3_Passion.Terms.Disabled")))
                        {
                            Settings.Teen = true;
                            text = "1";
                            if (text != null)
                            {
                                float num7 = PassionCommon.Float(text) / 100f;
                                if (num7 > Settings.ConsentMaximum)
                                {
                                    num7 = Settings.ConsentMaximum;
                                }
                                if (num7 < Settings.ConsentMinimum)
                                {
                                    num7 = Settings.ConsentMinimum;
                                }
                                Settings.ConsentPercentage = num7;
                            }
                        }
                        else
                        {
                            Settings.Teen = false;
                        }
                        break;
                    case Setting.EndWhenFull:
                        {
                            GenericDialog.OptionList<bool> optionList8 = new GenericDialog.OptionList<bool>();
                            optionList8.Add(PassionCommon.Localize("S3_Passion.Terms.Enabled"), true);
                            optionList8.Add(PassionCommon.Localize("S3_Passion.Terms.Disabled"), false);
                            Settings.EndWhenFull = GenericDialog.Ask(optionList8, PassionCommon.Localize("S3_Passion.Terms.EndWhenFull"));
                            break;
                        }
                    case Setting.ExludeInvalidPositions:
                        Settings.ExcludeInvalidPositions = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.ExcludeInvalidPositionsText"), Settings.ExcludeInvalidPositions, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.VampireInteractions:
                        Settings.VampireInteractions = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.VampireInteractions"), Settings.VampireInteractions, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.ChildrenOut:
                        Settings.ChildrenOut = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.ChildrenOut"), Settings.ChildrenOut, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.PetsOut:
                        Settings.PetsOut = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.PetsOut"), Settings.PetsOut, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.EldersOut:
                        Settings.EldersOut = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.EldersOut"), Settings.EldersOut, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.VampiresVisitAtNight:
                        Settings.VampiresVisitAtNight = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.VampiresVisitAtNight"), Settings.VampiresVisitAtNight, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.VampireHypnoSex:
                        Settings.VampireHypnoSex = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.VampireHypnoSex"), Settings.VampireHypnoSex, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.EvilFighter:
                        Settings.EvilFighter = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.EvilFighter"), Settings.EvilFighter, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.RemoveVisualOverride:
                        Settings.RemoveVisualOverride = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.RemoveVisualOverride"), Settings.RemoveVisualOverride, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.BroadCasterEnable:
                        Settings.BroadCasterEnable = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms. BroadCasterEnableText"), Settings.BroadCasterEnable, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.FemaleUseStrapOn:
                        Settings.FemaleUseStrapOn = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.FemaleUseStrapOnText"), Settings.FemaleUseStrapOn, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        Settings.ExcludeInvalidPositions = false;
                        PassionBase.ReloadDefaultPositions();
                        break;
                    case Setting.StripperAutonomy:
                        Settings.StripperAutonomy = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.StripperAutonomyText"), Settings.StripperAutonomy, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        break;
                    case Setting.Jealousy:
                        Settings.Jealousy = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.JealousyText"), Settings.Jealousy, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        if (Settings.Jealousy)
                        {
                            Settings.PolyamorousJealousy = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.PolyamorousJealousyText"), Settings.PolyamorousJealousy, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        }
                        break;
                    case Setting.Label:
                        {
                            string text7 = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.PassionLabel"), PassionCommon.Localize("S3_Passion.Terms.PassionLabelText"), Settings.Label);
                            if (!string.IsNullOrEmpty(text7))
                            {
                                Settings.Label = text7;
                            }
                            string text8 = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.PassionActiveLabel"), PassionCommon.Localize("S3_Passion.Terms.PassionActiveLabelText"), Settings.ActiveLabel);
                            if (!string.IsNullOrEmpty(text8))
                            {
                                Settings.ActiveLabel = text8;
                            }
                            string text9 = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.PassionSoloLabel"), PassionCommon.Localize("S3_Passion.Terms.PassionSoloLabelText"), Settings.SoloLabel);
                            if (!string.IsNullOrEmpty(text9))
                            {
                                Settings.SoloLabel = text9;
                            }
                            break;
                        }
                    case Setting.Moodlets:
                        {
                            bool flag12 = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.LibidoBuffText"), Settings.LibidoBuff, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                            if (flag12 && !Settings.LibidoBuff)
                            {
                                PassionCommon.ApplyRandomMoodlet(CustomBuff.Names.Libido);
                            }
                            else if (!flag12 && Settings.LibidoBuff)
                            {
                                PassionCommon.CleanMoodlets(CustomBuff.Names.Libido);
                            }
                            Settings.LibidoBuff = flag12;
                            bool flag13 = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.WoohooBuffText"), Settings.WoohooBuff, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                            if (!flag13 && Settings.WoohooBuff)
                            {
                                PassionCommon.CleanMoodlets(CustomBuff.Names.Woohoo);
                            }
                            Settings.WoohooBuff = flag13;
                            break;
                        }
                    case Setting.RelationshipGain:
                        {
                            GenericDialog.OptionList<float> optionList16 = new GenericDialog.OptionList<float>();
                            optionList16.Add(PassionCommon.Localize("S3_Passion.Terms.None"), 0f);
                            optionList16.Add("+", 8f);
                            optionList16.Add("++", 15f);
                            Settings.RelationshipGain = GenericDialog.Ask(optionList16, PassionCommon.Localize("S3_Passion.Terms.RelationshipGainText"));
                            break;
                        }
                    case Setting.CanReject:
                        if (PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.CanRejectText"), Settings.CanReject, PassionCommon.Localize("S3_Passion.Terms.Enabled"), PassionCommon.Localize("S3_Passion.Terms.Disabled")))
                        {
                            Settings.ActiveAlwaysAccepts = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.ActiveAlwaysAccepts"), Settings.ActiveAlwaysAccepts, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                            Settings.CanReject = true;
                            GenericDialog.OptionList<float> optionList13 = new GenericDialog.OptionList<float>();
                            optionList13.Add(PassionCommon.Localize("S3_Passion.Terms.None"), 0f);
                            optionList13.Add("-", -8f);
                            optionList13.Add("--", -15f);
                            Settings.RelationshipLoss = GenericDialog.Ask(optionList13, PassionCommon.Localize("S3_Passion.Terms.RelationshipLossText"));
                        }
                        else
                        {
                            Settings.CanReject = false;
                        }
                        break;
                    case Setting.GetHard:
                        Settings.GetHard = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.HardBeforeText"), Settings.GetHard, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        Settings.GetSoft = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.SoftAfterText"), Settings.GetSoft, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        if (Settings.GetSoft)
                        {
                            Settings.StrapOnMode = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.StrapOnMode"), Settings.StrapOnMode, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                        }
                        break;
                    case Setting.PregnancyMethod:
                        {
                            GenericDialog.OptionList<PregnancyMethod> optionList17 = new GenericDialog.OptionList<PregnancyMethod>();
                            optionList17.Add(PassionCommon.Localize("S3_Passion.Terms.Enabled") + " (" + PassionCommon.Localize("S3_Passion.Terms.Category") + ")", PregnancyMethod.ByCategory);
                            optionList17.Add(PassionCommon.Localize("S3_Passion.Terms.Enabled") + " (" + PassionCommon.Localize("S3_Passion.Terms.Position") + ")", PregnancyMethod.ByPosition);
                            optionList17.Add(PassionCommon.Localize("S3_Passion.Terms.Enabled") + " (" + PassionCommon.Localize("S3_Passion.Terms.KWSystem") + ")", PregnancyMethod.KWSystem);
                            optionList17.Add(PassionCommon.Localize("S3_Passion.Terms.Disabled"), PregnancyMethod.Disabled);
                            Settings.PregnancyMethod = GenericDialog.Ask(optionList17, PassionCommon.Localize("S3_Passion.Terms.Pregnancy"));
                            if (Settings.PregnancyMethod != 0)
                            {
                                // mpreg settings
                                // look into this later to add lesbian pregnancy too
                                Settings.PregnancyMale = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.MaleMale"), Settings.PregnancyMale, PassionCommon.Localize("S3_Passion.Terms.Enabled"), PassionCommon.Localize("S3_Passion.Terms.Disabled"));
                                text = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.PregnancyRisk"), PassionCommon.Localize("S3_Passion.Terms.PregnancyRiskText"), Settings.PregnancyRisk.ToString());
                                if (text != null)
                                {
                                    int num10 = PassionCommon.Int(text);
                                    if (num10 > 100)
                                    {
                                        num10 = 100;
                                    }
                                    if (num10 < 0)
                                    {
                                        num10 = 0;
                                    }
                                    if (num10 == 0)
                                    {
                                        Settings.PregnancyMethod = PregnancyMethod.Disabled;
                                    }
                                    else
                                    {
                                        Settings.PregnancyRisk = num10;
                                    }
                                }
                            }
                            else if (Settings.PregnancyMethod == PregnancyMethod.Disabled && !Settings.STD && Settings.UseCondom)
                            {
                                Settings.PregnancyRisk = 0;
                                Settings.UseCondom = false;
                                Settings.CondomBrakeChance = 0;
                                Settings.CondomIsBroken = false;
                            }
                            else if (Settings.PregnancyMethod == PregnancyMethod.Disabled && Settings.STD && Settings.UseCondom)
                            {
                                Settings.PregnancyRisk = 0;
                            }
                            break;
                        }
                    case Setting.UseCondom:
                        if (Settings.PregnancyMethod != 0 || Settings.STD)
                        {
                            Settings.UseCondom = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.UseCondom"), Settings.UseCondom, PassionCommon.Localize("S3_Passion.Terms.Enabled"), PassionCommon.Localize("S3_Passion.Terms.Disabled"));
                            if (!Settings.UseCondom)
                            {
                                break;
                            }
                            text = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.CondomBrakeChance"), PassionCommon.Localize("S3_Passion.Terms.CondomBrakeChanceText"), Settings.CondomBrakeChance.ToString());
                            if (text != null)
                            {
                                int num = PassionCommon.Int(text);
                                if (num > 100)
                                {
                                    num = 100;
                                }
                                if (num < 0)
                                {
                                    num = 0;
                                }
                                if (num == 0)
                                {
                                    Settings.UseCondom = false;
                                }
                                else
                                {
                                    Settings.CondomBrakeChance = num;
                                }
                                Settings.CondomIsBroken = false;
                            }
                        }
                        else if (Settings.PregnancyMethod == PregnancyMethod.Disabled && !Settings.STD)
                        {
                            Settings.UseCondom = false;
                            PassionCommon.SystemMessage(PassionCommon.Localize("Pregnancy and STDs settings are\r disabled! There's no need for condoms."));
                        }
                        else if (Settings.PregnancyMethod != PregnancyMethod.KWSystem && Settings.PregnancyMethod != 0)
                        {
                            Settings.UseCondom = false;
                            PassionCommon.SystemMessage(PassionCommon.Localize("Use Condoms Setting works only with \r -Passion Choice- Pregnancy Setting."));
                        }
                        break;
                    case Setting.STD:
                        {
                            GenericDialog.OptionList<bool> optionList14 = new GenericDialog.OptionList<bool>();
                            optionList14.Add(PassionCommon.Localize("S3_Passion.Terms.Enabled"), true);
                            optionList14.Add(PassionCommon.Localize("S3_Passion.Terms.Disabled"), false);
                            bool flag11 = GenericDialog.Ask(optionList14, PassionCommon.Localize("S3_Passion.Terms.STDs"));
                            if (flag11)
                            {
                                GenericDialog.OptionList<STDImmunity> optionList15 = new GenericDialog.OptionList<STDImmunity>();
                                optionList15.Add(PassionCommon.Localize("S3_Passion.Terms.Immunity"), STDImmunity.Immune);
                                optionList15.Add(PassionCommon.Localize("S3_Passion.Terms.Resistance"), STDImmunity.Resistant);
                                optionList15.Add(PassionCommon.Localize("S3_Passion.Terms.NoEffect"), STDImmunity.Vulnerable);
                                Settings.STDSimmunity = GenericDialog.Ask(optionList15, PassionCommon.Localize("S3_Passion.Terms.STDSimmunityEffect"));
                            }
                            if (flag11 && !Settings.STD)
                            {
                                BOE_STD.STD.AddRandomToAll(PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.STDSeedActiveFamily"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")), PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.STDSeedMessages"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")));
                            }
                            else if (!flag11 && Settings.STD)
                            {
                                BOE_STD.STD.RemoveFromAll();
                            }
                            Settings.STD = flag11;
                            if (!Settings.STD && Settings.PregnancyMethod == PregnancyMethod.Disabled)
                            {
                                Settings.UseCondom = false;
                                Settings.CondomBrakeChance = 0;
                                Settings.CondomIsBroken = false;
                            }
                            break;
                        }
                    case Setting.Outfit:
                        {
                            GenericDialog.OptionList<OutfitCategories> optionList12 = new GenericDialog.OptionList<OutfitCategories>();
                            optionList12.Add(PassionCommon.Localize("S3_Passion.Terms.Naked"), OutfitCategories.Naked);
                            optionList12.Add(PassionCommon.Localize("S3_Passion.Terms.Sleepwear"), OutfitCategories.Sleepwear);
                            optionList12.Add(PassionCommon.Localize("S3_Passion.Terms.PreferredOnly"), OutfitCategories.None);
                            Settings.Outfit = GenericDialog.Ask(optionList12, PassionCommon.Localize("S3_Passion.Terms.WhichOutfit"));
                            if (Settings.Outfit != OutfitCategories.Naked)
                            {
                                Settings.NakedShower = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.NakedShower"), Settings.NakedShower, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                            }
                            break;
                        }
                    case Setting.Motives:
                        {
                            GenericDialog.OptionList<PassionMotives> optionList11 = new GenericDialog.OptionList<PassionMotives>();
                            optionList11.Add(PassionCommon.Localize("S3_Passion.Terms.EADefault"), PassionMotives.EADefault);
                            optionList11.Add(PassionCommon.Localize("S3_Passion.Terms.PassionStandard"), PassionMotives.PassionStandard);
                            optionList11.Add(PassionCommon.Localize("S3_Passion.Terms.NoDecay"), PassionMotives.NoDecay);
                            optionList11.Add(PassionCommon.Localize("S3_Passion.Terms.Freeze"), PassionMotives.Freeze);
                            optionList11.Add(PassionCommon.Localize("S3_Passion.Terms.MaxAll"), PassionMotives.MaxAll);
                            Settings.Motives = GenericDialog.Ask(optionList11, PassionCommon.Localize("S3_Passion.Terms.MotiveSatisfaction"));
                            break;
                        }
                    case Setting.InitialCategory:
                        {
                            GenericDialog.OptionList<int> optionList10 = new GenericDialog.OptionList<int>();
                            optionList10.Add(PassionCommon.Localize("S3_Passion.Terms.Choose"), 1024);
                            optionList10.Add(PassionCommon.Localize("S3_Passion.Terms.Fuck"), 12);
                            optionList10.Add(PassionCommon.Localize("S3_Passion.Terms.Oral"), 2);
                            optionList10.Add(PassionCommon.Localize("S3_Passion.Terms.Masturbate"), 48);
                            optionList10.Add(PassionCommon.Localize("S3_Passion.Terms.Any"), 1);
                            Settings.InitialCategory = GenericDialog.Ask(optionList10, PassionCommon.Localize("S3_Passion.Terms.InitialCategory"));
                            break;
                        }
                    case Setting.Positions:
                        {
                            bool flag10 = true;
                            while (flag10)
                            {
                                GenericDialog.OptionList<string> optionList9 = new GenericDialog.OptionList<string>();
                                optionList9.Add(PassionCommon.Localize("S3_Passion.Terms.AddPosition"), PassionCommon.Localize("S3_Passion.Terms.AddPosition"));
                                optionList9.Add(PassionCommon.Localize("S3_Passion.Terms.ReloadDefaults"), PassionCommon.Localize("S3_Passion.Terms.ReloadDefaults"));
                                foreach (string xMLFile in PassionBase.XMLFiles)
                                {
                                    int num8 = PositionsCounter(xMLFile);
                                    int num9 = 38 - xMLFile.Length;
                                    string text10 = "";
                                    if (num8 != 0)
                                    {
                                        for (int j = 0; j < num9; j++)
                                        {
                                            text10 += "_";
                                        }
                                        optionList9.Add("\n" + xMLFile + " " + text10 + " " + num8.ToString().Trim() + "\r", xMLFile);
                                    }
                                }
                                string text11 = GenericDialog.Ask(optionList9, PassionCommon.Localize("Positions Found ") + ": " + PassionBase.Positions.Count, true);
                                if (text11 == PassionCommon.Localize("S3_Passion.Terms.AddPosition"))
                                {
                                    string text12 = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.AddPositionFile"), PassionCommon.Localize("S3_Passion.Terms.AddPositionFileText"), string.Empty);
                                    if (!string.IsNullOrEmpty(text12))
                                    {
                                        PassionBase.XMLFiles.Add(text12);
                                        PassionBase.ReloadPositions();
                                    }
                                    continue;
                                }
                                if (text11 == PassionCommon.Localize("S3_Passion.Terms.ReloadDefaults"))
                                {
                                    PassionBase.ReloadDefaultPositions();
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(text11))
                                {
                                    if (PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.DeleteConfirm1") + text11 + PassionCommon.Localize("S3_Passion.Terms.DeleteConfirm2"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
                                    {
                                        PassionBase.XMLFiles.Remove(text11);
                                        PassionBase.ReloadPositions();
                                    }
                                    continue;
                                }
                                flag10 = false;
                                break;
                            }
                            break;
                        }
                    case Setting.RandomizationOptions:
                        {
                            GenericDialog.OptionList<RandomizationOptions> optionList7 = new GenericDialog.OptionList<RandomizationOptions>();
                            optionList7.Add(PassionCommon.Localize("S3_Passion.Terms.Positions"), RandomizationOptions.Positions);
                            optionList7.Add(PassionCommon.Localize("S3_Passion.Terms.Sequences"), RandomizationOptions.Sequences);
                            optionList7.Add(PassionCommon.Localize("S3_Passion.Terms.Both"), RandomizationOptions.PositionsAndSequences);
                            Settings.RandomizationOptions = GenericDialog.Ask(optionList7, PassionCommon.Localize("S3_Passion.Terms.RandomizationOptions"));
                            if (PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.RandomizationSameCategory"), true, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
                            {
                                Settings.RandomizationOptions |= RandomizationOptions.SameCategory;
                            }
                            if (PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.RandomizationLengthPrompt"), true, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
                            {
                                string text6 = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.Randomization"), PassionCommon.Localize("S3_Passion.Terms.RandomizationLength"), PassionCommon.TicksToMinutes(Settings.RandomizationLength).ToString());
                                if (text6 != null)
                                {
                                    int num3 = PassionCommon.Int(text6);
                                    if (num3 < 1)
                                    {
                                        num3 = 1;
                                    }
                                    if (num3 > 100000)
                                    {
                                        num3 = 100000;
                                    }
                                    Settings.RandomizationLength = PassionCommon.MinutesToTicks(num3);
                                }
                            }
                            else
                            {
                                Settings.RandomizationLength = 0L;
                            }
                            break;
                        }
                    case Setting.Sequences:
                        {
                            bool flag2 = true;
                            while (flag2)
                            {
                                GenericDialog.OptionList<Sequence.DialogEntry> optionList3 = new GenericDialog.OptionList<Sequence.DialogEntry>();
                                optionList3.Add(PassionCommon.Localize("S3_Passion.Terms.AddSequence"), Sequence.DialogEntry.Get(PassionCommon.Localize("S3_Passion.Terms.AddSequence"), null));
                                foreach (Sequence sequence2 in PassionBase.Sequences)
                                {
                                    string text4 = "\"" + sequence2.Name + "\"";
                                    optionList3.Add(text4, Sequence.DialogEntry.Get(text4, sequence2));
                                }
                                Sequence.DialogEntry dialogEntry = GenericDialog.Ask(optionList3, PassionCommon.Localize("S3_Passion.Terms.Sequences"), true);
                                if (dialogEntry != null)
                                {
                                    bool flag3 = false;
                                    bool flag4 = false;
                                    Sequence sequence = null;
                                    if (dialogEntry.Sequence == null)
                                    {
                                        text = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.NewSequence"), PassionCommon.Localize("S3_Passion.Terms.NewSequenceText"), string.Empty);
                                        if (text == null)
                                        {
                                            break;
                                        }
                                        sequence = Sequence.Create();
                                        sequence.Name = text;
                                        PassionBase.Sequences.Add(sequence);
                                        flag3 = true;
                                    }
                                    else
                                    {
                                        sequence = dialogEntry.Sequence;
                                        GenericDialog.OptionList<int> optionList4 = new GenericDialog.OptionList<int>();
                                        optionList4.Add(PassionCommon.Localize("S3_Passion.Terms.Edit"), 1);
                                        optionList4.Add(PassionCommon.Localize("S3_Passion.Terms.Delete"), 2);
                                        optionList4.Add(PassionCommon.Localize("S3_Passion.Terms.Cancel"), 0);
                                        switch (GenericDialog.Ask(optionList4, PassionCommon.Localize("S3_Passion.Terms.SequenceEdit") + " \"" + sequence.Name + "\"?"))
                                        {
                                            case 1:
                                                flag3 = true;
                                                break;
                                            case 2:
                                                flag4 = true;
                                                break;
                                        }
                                    }
                                    if (flag4 && sequence != null)
                                    {
                                        PassionBase.Sequences.Remove(sequence);
                                    }
                                    else
                                    {
                                        if (!flag3 || sequence == null)
                                        {
                                            continue;
                                        }
                                        bool flag5 = true;
                                        while (flag5)
                                        {
                                            GenericDialog.OptionList<Sequence.DialogListItem> optionList5 = new GenericDialog.OptionList<Sequence.DialogListItem>();
                                            optionList5.Add(PassionCommon.Localize("S3_Passion.Terms.AddPosition"), Sequence.DialogListItem.Get(PassionCommon.Localize("S3_Passion.Terms.AddPosition"), null));
                                            for (int i = 0; i < sequence.Items.Length; i++)
                                            {
                                                if (sequence.Items[i] != null)
                                                {
                                                    Position position = sequence.Items[i].Position;
                                                    string text5 = "(" + sequence.Items[i].Index + ") " + (position != null ? PassionCommon.Localize(position.Name) : sequence.Items[i].Key);
                                                    optionList5.Add(text5, Sequence.DialogListItem.Get(text5, sequence.Items[i]));
                                                }
                                            }
                                            Sequence.DialogListItem dialogListItem = GenericDialog.Ask(optionList5, PassionCommon.Localize("S3_Passion.Terms.Editing") + " \"" + sequence.Name + "\"", true);
                                            if (dialogListItem != null)
                                            {
                                                bool flag6 = false;
                                                bool flag7 = false;
                                                bool flag8 = false;
                                                bool flag9 = false;
                                                SequenceItem sequenceItem = dialogListItem.Value;
                                                if (sequenceItem == null)
                                                {
                                                    flag6 = true;
                                                    flag7 = true;
                                                    flag8 = true;
                                                    sequenceItem = new SequenceItem();
                                                }
                                                else
                                                {
                                                    GenericDialog.OptionList<int> optionList6 = new GenericDialog.OptionList<int>();
                                                    optionList6.Add(PassionCommon.Localize("S3_Passion.Terms.ChangePosition"), 1);
                                                    optionList6.Add(PassionCommon.Localize("S3_Passion.Terms.ChangeLength"), 2);
                                                    optionList6.Add(PassionCommon.Localize("S3_Passion.Terms.MoveUp"), 3);
                                                    optionList6.Add(PassionCommon.Localize("S3_Passion.Terms.MoveDown"), 4);
                                                    optionList6.Add(PassionCommon.Localize("S3_Passion.Terms.Remove"), 5);
                                                    optionList6.Add(PassionCommon.Localize("S3_Passion.Terms.Cancel"), 0);
                                                    switch (GenericDialog.Ask(optionList6, PassionCommon.Localize(sequenceItem.Position.Name)))
                                                    {
                                                        case 1:
                                                            flag7 = true;
                                                            break;
                                                        case 2:
                                                            flag8 = true;
                                                            break;
                                                        case 3:
                                                            sequence.MoveUp(sequenceItem);
                                                            break;
                                                        case 4:
                                                            sequence.MoveDown(sequenceItem);
                                                            break;
                                                        case 5:
                                                            flag9 = true;
                                                            break;
                                                    }
                                                }
                                                if (flag9)
                                                {
                                                    sequence.Remove(sequenceItem.Index);
                                                    continue;
                                                }
                                                if (flag7)
                                                {
                                                    Position position2 = Position.ChooseSequencePositionDialog(sequence.SupportedTypes, sequence.MinSims, sequence.MaxSims);
                                                    if (position2 != null)
                                                    {
                                                        sequenceItem.Key = position2.Key;
                                                    }
                                                }
                                                if (flag8)
                                                {
                                                    text = PickString.Show(PassionCommon.Localize(sequenceItem.Position.Name), PassionCommon.Localize("S3_Passion.Terms.SetLength"), PassionCommon.TicksToMinutes(sequenceItem.Length).ToString());
                                                    if (text != null)
                                                    {
                                                        int num2 = PassionCommon.Int(text);
                                                        if (num2 < 1)
                                                        {
                                                            num2 = 1;
                                                        }
                                                        if (num2 > 100000)
                                                        {
                                                            num2 = 100000;
                                                        }
                                                        sequenceItem.Length = PassionCommon.MinutesToTicks(num2);
                                                    }
                                                }
                                                if (flag6)
                                                {
                                                    sequence.Add(sequenceItem);
                                                }
                                                continue;
                                            }
                                            sequence.Repeat = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.RepeatAll"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                                            if (!sequence.Repeat)
                                            {
                                                sequence.Continue = PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.ContinuePassion"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No"));
                                            }
                                            flag5 = false;
                                            break;
                                        }
                                    }
                                    continue;
                                }
                                flag2 = false;
                                break;
                            }
                            break;
                        }
                    case Setting.ExportImport:
                        {
                            GenericDialog.OptionList<string> optionList2 = new GenericDialog.OptionList<string>();
                            optionList2.Add(PassionCommon.Localize("S3_Passion.Terms.Import"), "Import");
                            optionList2.Add(PassionCommon.Localize("S3_Passion.Terms.Export"), "Export");
                            string text3 = GenericDialog.Ask(optionList2, PassionCommon.Localize("S3_Passion.Terms.SettingsFile"));
                            if (text3 != null)
                            {
                                if (text3 == "Import")
                                {
                                    Import();
                                }
                                else if (text3 == "Export")
                                {
                                    Export();
                                }
                            }
                            break;
                        }
                    case Setting.ExportImportSequence:
                        {
                            GenericDialog.OptionList<string> optionList = new GenericDialog.OptionList<string>();
                            optionList.Add(PassionCommon.Localize("S3_Passion.Terms.Sequence.Import"), "Import");
                            optionList.Add(PassionCommon.Localize("S3_Passion.Terms.Sequence.Export"), "Export");
                            string text2 = GenericDialog.Ask(optionList, PassionCommon.Localize("S3_Passion.Terms.Sequence.SettingsFile"));
                            if (text2 != null)
                            {
                                if (text2 == "Import")
                                {
                                    ImportSequence();
                                }
                                else if (text2 == "Export")
                                {
                                    ExportSequence();
                                }
                            }
                            break;
                        }
                    default:
                        flag = false;
                        return;
                }
            }
        }

        public static void Export()
        {
            Export(null);
        }

        // settings export
        public static void Export(string name)
        {
            try
            {
                BOE_Settings.XML.Element element = XML.Element.Create("Passion");
                XML.Element element2 = element.AddChild("Settings");
                element2.AddChild("ActiveLabel", Settings.ActiveLabel);
                element2.AddChild("AutonomyActive", Settings.AutonomyActive.ToString());
                element2.AddChild("AutonomyChance", Settings.AutonomyChance.ToString());
                element2.AddChild("AutonomyLength", Settings.AutonomyLength.ToString());
                element2.AddChild("AutonomyNotify", Settings.AutonomyNotify.ToString());
                element2.AddChild("AutonomyPublic", Settings.AutonomyPublic.ToString());
                element2.AddChild("ConsentPercentage", Settings.ConsentPercentage.ToString());
                element2.AddChild("EndWhenFull", Settings.EndWhenFull.ToString());
                element2.AddChild("GetHard", Settings.GetHard.ToString());
                element2.AddChild("GetSoft", Settings.GetSoft.ToString());
                element2.AddChild("InitialCategory", Settings.InitialCategory.ToString());
                element2.AddChild("Jealousy", Settings.Jealousy.ToString());
                element2.AddChild("Label", Settings.Label.ToString());
                element2.AddChild("MaxLength", Settings.MaxLength.ToString());
                element2.AddChild("Motives", Settings.Motives.ToString());
                element2.AddChild("NakedShower", Settings.NakedShower.ToString());
                element2.AddChild("NearRelation", Settings.Incest.ToString());
                element2.AddChild("Outfit", Settings.Outfit.ToString());
                element2.AddChild("PregnancyMale", Settings.PregnancyMale.ToString());
                element2.AddChild("PregnancyMethod", Settings.PregnancyMethod.ToString());
                element2.AddChild("PregnancyRisk", Settings.PregnancyRisk.ToString());
                element2.AddChild("CondomBrakeChance", Settings.CondomBrakeChance.ToString());
                int randomizationOptions = (int)Settings.RandomizationOptions;
                element2.AddChild("RandomizationOptions", randomizationOptions.ToString());
                element2.AddChild("RandomizationLength", Settings.RandomizationLength.ToString());
                element2.AddChild("Rejection", Settings.CanReject.ToString());
                element2.AddChild("RelationshipGain", Settings.RelationshipGain.ToString());
                element2.AddChild("RelationshipLoss", Settings.RelationshipLoss.ToString());
                element2.AddChild("SoloLabel", Settings.SoloLabel.ToString());
                element2.AddChild("STD", Settings.STD.ToString());
                element2.AddChild("STDSimmunity", Settings.STDSimmunity.ToString());
                element2.AddChild("StrapOnMode", Settings.StrapOnMode.ToString());
                element2.AddChild("Teen", Settings.Teen.ToString());
                XML.Element element3 = element.AddChild("Positions");
                foreach (string xMLFile in PassionBase.XMLFiles)
                {
                    element3.AddChild("Position", xMLFile);
                }
                if (PassionBase.Sequences.Count > 0)
                {
                    XML.Element element4 = element.AddChild("Sequences");
                    foreach (Sequence sequence in PassionBase.Sequences)
                    {
                        XML.Element element5 = element4.AddChild("Sequence");
                        element5.AddChild("Name", sequence.Name);
                        element5.AddChild("Repeat", sequence.Repeat.ToString());
                        element5.AddChild("Continue", sequence.Continue.ToString());
                        element5.AddChild("Categories", sequence.Categories.ToString());
                        if (sequence.Items.Length == 0)
                        {
                            continue;
                        }
                        XML.Element element6 = element5.AddChild("Items");
                        for (int i = 0; i < sequence.Items.Length; i++)
                        {
                            if (sequence.Items[i] != null)
                            {
                                XML.Element element7 = element6.AddChild("Item");
                                element7.AddChild("Name", PassionCommon.Localize(sequence.Items[i].Key));
                                element7.AddChild("Key", sequence.Items[i].Key);
                                element7.AddChild("Index", sequence.Items[i].Index.ToString());
                                element7.AddChild("Length", sequence.Items[i].Length.ToString());
                            }
                        }
                    }
                }
                if (XML.WriteToPackage(element, name) && string.IsNullOrEmpty(name))
                {
                    PassionCommon.SystemMessage(PassionCommon.Localize("S3_Passion.Terms.ExportSucceeded"));
                }
            }
            catch
            {
            }
        }

        public static void Import()
        {
            Import(null);
        }

        // import settings
        public static void Import(string name)
        {
            XML.File file = XML.ReadFromPackage(name);
            if (file == null)
            {
                return;
            }
            XML.Node node = file["Passion"];
            if (node == null)
            {
                return;
            }
            XML.Node matchingNode = node.GetMatchingNode("Settings");
            if (matchingNode != null)
            {
                Settings.ActiveLabel = matchingNode["ActiveLabel"];
                Settings.AutonomyActive = PassionCommon.Bool(matchingNode["AutonomyActive"]);
                Settings.AutonomyChance = PassionCommon.Int(matchingNode["AutonomyChance"]);
                Settings.AutonomyLength = PassionCommon.Long(matchingNode["AutonomyLength"]);
                Settings.AutonomyNotify = PassionCommon.Bool(matchingNode["AutonomyNotify"]);
                Settings.AutonomyPublic = PassionCommon.Bool(matchingNode["AutonomyPublic"]);
                Settings.ConsentPercentage = PassionCommon.Float(matchingNode["ConsentPercentage"]);
                Settings.EndWhenFull = PassionCommon.Bool(matchingNode["EndWhenFull"]);
                Settings.GetHard = PassionCommon.Bool(matchingNode["GetHard"]);
                Settings.GetSoft = PassionCommon.Bool(matchingNode["GetSoft"]);
                Settings.InitialCategory = PassionCommon.Int(matchingNode["InitialCategory"]);
                Settings.Jealousy = PassionCommon.Bool(matchingNode["Jealousy"]);
                Settings.Label = matchingNode["Label"];
                Settings.MaxLength = PassionCommon.Long(matchingNode["MaxLength"]);
                switch (matchingNode["Motives"])
                {
                    case "EADefault":
                        Settings.Motives = PassionMotives.EADefault;
                        break;
                    case "NoDecay":
                        Settings.Motives = PassionMotives.NoDecay;
                        break;
                    case "Freeze":
                        Settings.Motives = PassionMotives.Freeze;
                        break;
                    case "MaxAll":
                        Settings.Motives = PassionMotives.MaxAll;
                        break;
                    default:
                        Settings.Motives = PassionMotives.PassionStandard;
                        break;
                }
                Settings.NakedShower = PassionCommon.Bool(matchingNode["NakedShower"]);
                Settings.Incest = PassionCommon.Bool(matchingNode["NearRelation"]);
                switch (matchingNode["Outfit"])
                {
                    case "None":
                        Settings.Outfit = OutfitCategories.None;
                        break;
                    case "Sleepwear":
                        Settings.Outfit = OutfitCategories.Sleepwear;
                        break;
                    default:
                        Settings.Outfit = OutfitCategories.Naked;
                        break;
                }
                Settings.RandomizationOptions = (RandomizationOptions)PassionCommon.Int(matchingNode["RandomizationOptions"]);
                Settings.RandomizationLength = PassionCommon.Long(matchingNode["RandomizationLength"]);
                Settings.PregnancyMale = PassionCommon.Bool(matchingNode["PregnancyMale"]);
                switch (matchingNode["PregnancyMethod"])
                {
                    case "ByCategory":
                        Settings.PregnancyMethod = PregnancyMethod.ByCategory;
                        break;
                    case "ByPosition":
                        Settings.PregnancyMethod = PregnancyMethod.ByPosition;
                        break;
                    case "KWSystem":
                        Settings.PregnancyMethod = PregnancyMethod.KWSystem;
                        break;
                    default:
                        Settings.PregnancyMethod = PregnancyMethod.Disabled;
                        break;
                }
                Settings.PregnancyRisk = PassionCommon.Int(matchingNode["PregnancyRisk"]);
                Settings.CondomBrakeChance = PassionCommon.Int(matchingNode["CondomBrakeChance"]);
                Settings.CanReject = PassionCommon.Bool(matchingNode["Rejection"]);
                Settings.RelationshipGain = PassionCommon.Float(matchingNode["RelationshipGain"]);
                Settings.RelationshipLoss = PassionCommon.Float(matchingNode["RelationshipLoss"]);
                Settings.SoloLabel = matchingNode["SoloLabel"];
                Settings.STD = PassionCommon.Bool(matchingNode["STD"]);
                string text = matchingNode["STDSimmunity"];
                if (!(text == "Resistant"))
                {
                    if (text == "Vulnerable")
                    {
                        Settings.STDSimmunity = STDImmunity.Vulnerable;
                    }
                    else
                    {
                        Settings.STDSimmunity = STDImmunity.Immune;
                    }
                }
                else
                {
                    Settings.STDSimmunity = STDImmunity.Resistant;
                }
                Settings.StrapOnMode = PassionCommon.Bool(matchingNode["StrapOnMode"]);
                Settings.Teen = PassionCommon.Bool(matchingNode["Teen"]);
            }
            XML.Node matchingNode2 = node.GetMatchingNode("Positions");
            if (matchingNode2 != null)
            {
                List<XML.Node> matchingNodes = matchingNode2.GetMatchingNodes("Position");
                if (matchingNodes != null && matchingNodes.Count > 0)
                {
                    PassionBase.XMLFiles.Clear();
                    foreach (XML.Node item in matchingNodes)
                    {
                        PassionBase.XMLFiles.Add(item.Handle.InnerText);
                    }
                }
                PassionBase.ReloadPositions();
            }
            XML.Node matchingNode3 = node.GetMatchingNode("Sequences");
            if (matchingNode3 == null)
            {
                return;
            }
            foreach (XML.Node matchingNode5 in matchingNode3.GetMatchingNodes("Sequence"))
            {
                Sequence sequence = null;
                foreach (Sequence sequence2 in PassionBase.Sequences)
                {
                    if (sequence2.Name == matchingNode5["Name"])
                    {
                        sequence = sequence2;
                        break;
                    }
                }
                if (sequence == null)
                {
                    sequence = new Sequence();
                    sequence.Name = matchingNode5["Name"];
                    PassionBase.Sequences.Add(sequence);
                }
                if (sequence == null)
                {
                    continue;
                }
                sequence.Repeat = PassionCommon.Bool(matchingNode5["Repeat"]);
                sequence.Continue = PassionCommon.Bool(matchingNode5["Continue"]);
                sequence.Categories = PassionCommon.Int(matchingNode5["Categories"]);
                XML.Node matchingNode4 = matchingNode5.GetMatchingNode("Items");
                if (matchingNode4 != null)
                {
                    List<XML.Node> matchingNodes2 = matchingNode4.GetMatchingNodes("Item");
                    if (matchingNodes2 != null)
                    {
                        sequence.RestoreItems(matchingNodes2);
                    }
                }
            }
        }

        public static void ImportSequence()
        {
            ImportSequence(null);
        }

        public static void ImportSequence(string name)
        {
            XML.File file = XML.ReadFromPackage(name);
            if (file == null)
            {
                return;
            }
            XML.Node node = file["PassionSequences"];
            if (node == null)
            {
                return;
            }
            XML.Node matchingNode = node.GetMatchingNode("Sequences");
            if (matchingNode == null)
            {
                return;
            }
            foreach (XML.Node matchingNode3 in matchingNode.GetMatchingNodes("Sequence"))
            {
                Sequence sequence = null;
                foreach (Sequence sequence2 in PassionBase.Sequences)
                {
                    if (sequence2.Name == matchingNode3["Name"])
                    {
                        sequence = sequence2;
                        break;
                    }
                }
                if (sequence == null)
                {
                    sequence = new Sequence();
                    sequence.Name = matchingNode3["Name"];
                    PassionBase.Sequences.Add(sequence);
                }
                if (sequence == null)
                {
                    continue;
                }
                sequence.Repeat = PassionCommon.Bool(matchingNode3["Repeat"]);
                sequence.Continue = PassionCommon.Bool(matchingNode3["Continue"]);
                sequence.Categories = PassionCommon.Int(matchingNode3["Categories"]);
                XML.Node matchingNode2 = matchingNode3.GetMatchingNode("Items");
                if (matchingNode2 != null)
                {
                    List<XML.Node> matchingNodes = matchingNode2.GetMatchingNodes("Item");
                    if (matchingNodes != null)
                    {
                        sequence.RestoreItems(matchingNodes);
                    }
                }
            }
        }

        public static void ExportSequence()
        {
            ExportSequence(null);
        }

        public static void ExportSequence(string name)
        {
            try
            {
                XML.Element element = XML.Element.Create("PassionSequences");
                if (PassionBase.Sequences.Count > 0)
                {
                    XML.Element element2 = element.AddChild("Sequences");
                    foreach (Sequence sequence in PassionBase.Sequences)
                    {
                        XML.Element element3 = element2.AddChild("Sequence");
                        element3.AddChild("Name", sequence.Name);
                        element3.AddChild("Repeat", sequence.Repeat.ToString());
                        element3.AddChild("Continue", sequence.Continue.ToString());
                        element3.AddChild("Categories", sequence.Categories.ToString());
                        if (sequence.Items.Length == 0)
                        {
                            continue;
                        }
                        XML.Element element4 = element3.AddChild("Items");
                        for (int i = 0; i < sequence.Items.Length; i++)
                        {
                            if (sequence.Items[i] != null)
                            {
                                XML.Element element5 = element4.AddChild("Item");
                                element5.AddChild("Name", PassionCommon.Localize(sequence.Items[i].Key));
                                element5.AddChild("Key", sequence.Items[i].Key);
                                element5.AddChild("Index", sequence.Items[i].Index.ToString());
                                element5.AddChild("Length", sequence.Items[i].Length.ToString());
                            }
                        }
                    }
                }
                if (XML.WriteToPackage(element, name) && string.IsNullOrEmpty(name))
                {
                    PassionCommon.SystemMessage(PassionCommon.Localize("S3_Passion.Terms.Sequence.ExportSucceeded"));
                }
            }
            catch
            {
            }
        }

        public static int PositionsCounter(string file)
        {
            int num = 0;
            int num2 = 0;
            try
            {
                ResourceKey key = new ResourceKey(ResourceUtils.HashString64(file), 53690476u, 0u);
                XmlReader reader = Simulator.ReadXml(key);
                XmlDbData xmlDbData = XmlDbData.XmlDbDataFast.Create(reader);
                XmlDbTable value;
                if (xmlDbData.Tables.TryGetValue("Position", out value))
                {
                    num = value.Rows.Count;
                }
            }
            catch
            {
            }
            try
            {
                if (num == 0)
                {
                    XML.File file2 = XML.Create(file);
                    XML.Node node = file2["WooHooStages"];
                    if (node != null)
                    {
                        foreach (XML.Node matchingNode in node.GetMatchingNodes("WooHooStage"))
                        {
                            num2++;
                            num = num2;
                        }
                    }
                }
            }
            catch
            {
            }
            return num;
        }
        
        
    

    public static PersistableSettings Settings
        {
            get
            {
                if (PassionBase.mSettings == null)
                {
                    PassionBase.mSettings = new PersistableSettings();
                }
                return PassionBase.mSettings;
            }
        }

    } }
