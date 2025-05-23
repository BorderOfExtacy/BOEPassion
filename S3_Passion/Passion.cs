using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using S3_Passion.PassionDance;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.Gameplay.Objects.Counters;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.Gameplay.Objects.Decorations.Mimics;
using Sims3.Gameplay.Objects.Door;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Objects.Entertainment;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.HobbiesSkills;
using Sims3.Gameplay.Objects.HobbiesSkills.BrainEnhancingMachine;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Objects.Tables;
using Sims3.Gameplay.Objects.Vehicles;
using Sims3.Gameplay.Pools;
using Sims3.Gameplay.Seasons;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Situations;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.ThoughtBalloons;
using Sims3.Gameplay.TimeTravel;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.SimIFace.RouteDestinations;
using Sims3.Store.Objects;
using Sims3.UI;
using Sims3.UI.CAS;
using Sims3.UI.Controller;
using Sims3.UI.GameEntry;


namespace S3_Passion
{
	public class Passion : PassionCommon
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
				RandomizationLength = 60L;
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
					rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn((Settings.AutonomyChance > 0) ? (Settings.AutonomyChance + "%, " + PassionCommon.TicksToMinutes(Settings.AutonomyLength) + " Min.") : PassionCommon.Localize("S3_Passion.Terms.Disabled")));
					list3.Add(rowInfo);
					rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Eligibility, true), new List<ObjectPicker.ColumnInfo>());
					rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("* " + PassionCommon.Localize("S3_Passion.Terms.Eligibility") + " *"));
					rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("A " + (Settings.Incest ? PassionCommon.Localize("S3_Passion.Terms.Enabled") : string.Empty) + ", T " + (Settings.Teen ? PassionCommon.Localize("S3_Passion.Terms.Enabled") : string.Empty)));
					list3.Add(rowInfo);
					rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Positions, Positions), new List<ObjectPicker.ColumnInfo>());
					rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("* " + PassionCommon.Localize("S3_Passion.Terms.Positions") + " *"));
					rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("(" + Positions.Count + ")"));
					list3.Add(rowInfo);
					rowInfo = new ObjectPicker.RowInfo(new SettingItem(Setting.Sequences, Sequences), new List<ObjectPicker.ColumnInfo>());
					rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("* " + PassionCommon.Localize("S3_Passion.Terms.Sequences") + " *"));
					rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn("(" + Sequences.Count + ")"));
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
						rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Positions") + "/" + PassionCommon.Localize("S3_Passion.Terms.Sequences") + ((Settings.RandomizationLength > 0) ? (" (" + PassionCommon.TicksToMinutes(Settings.RandomizationLength) + " min.)") : string.Empty) + (PassionCommon.Match(RandomizationOptions.SameCategory, Settings.RandomizationOptions) ? "*" : string.Empty)));
					}
					else if (PassionCommon.Match(RandomizationOptions.Sequences, Settings.RandomizationOptions))
					{
						rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Sequences") + ((Settings.RandomizationLength > 0) ? (" (" + PassionCommon.TicksToMinutes(Settings.RandomizationLength) + " min.)") : string.Empty) + (PassionCommon.Match(RandomizationOptions.SameCategory, Settings.RandomizationOptions) ? "*" : string.Empty)));
					}
					else if (PassionCommon.Match(RandomizationOptions.Positions, Settings.RandomizationOptions))
					{
						rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.Positions") + ((Settings.RandomizationLength > 0) ? (" (" + PassionCommon.TicksToMinutes(Settings.RandomizationLength) + " min.)") : string.Empty) + (PassionCommon.Match(RandomizationOptions.SameCategory, Settings.RandomizationOptions) ? "*" : string.Empty)));
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
						rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(PassionCommon.Localize("S3_Passion.Terms.HardBefore") + (Settings.GetSoft ? (", " + PassionCommon.Localize("S3_Passion.Terms.SoftAfter") + (Settings.StrapOnMode ? "*" : string.Empty)) : string.Empty)));
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
					List<ObjectPicker.RowInfo> list4 = MenuList.Show(PassionCommon.Localize("S3_Passion.Terms.Settings") + "\rPassion Version ( " + PassionCommon.Version + " )", PassionCommon.Localize("S3_Passion.Terms.Ok"), PassionCommon.Localize("S3_Passion.Terms.Cancel"), list2, list);
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
						foreach (Player value in AllPlayers.Values)
						{
							value.Reset();
						}
						AllPlayers.Clear();
						foreach (Target value2 in AllTargets.Values)
						{
							value2.Object = null;
						}
						AllTargets.Clear();
						if (PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.ResetSequences"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
						{
							Sequences.Clear();
						}
						ReloadDefaultPositions();
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
						ReloadDefaultPositions();
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
							S3_Passion.STD.AddRandomToAll(PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.STDSeedActiveFamily"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")), PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.STDSeedMessages"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")));
						}
						else if (!flag11 && Settings.STD)
						{
							S3_Passion.STD.RemoveFromAll();
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
							foreach (string xMLFile in XMLFiles)
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
							string text11 = GenericDialog.Ask(optionList9, PassionCommon.Localize("Positions Found ") + ": " + Positions.Count, true);
							if (text11 == PassionCommon.Localize("S3_Passion.Terms.AddPosition"))
							{
								string text12 = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.AddPositionFile"), PassionCommon.Localize("S3_Passion.Terms.AddPositionFileText"), string.Empty);
								if (!string.IsNullOrEmpty(text12))
								{
									XMLFiles.Add(text12);
									ReloadPositions();
								}
								continue;
							}
							if (text11 == PassionCommon.Localize("S3_Passion.Terms.ReloadDefaults"))
							{
								ReloadDefaultPositions();
								continue;
							}
							if (!string.IsNullOrEmpty(text11))
							{
								if (PickBoolean.Show(PassionCommon.Localize("S3_Passion.Terms.DeleteConfirm1") + text11 + PassionCommon.Localize("S3_Passion.Terms.DeleteConfirm2"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
								{
									XMLFiles.Remove(text11);
									ReloadPositions();
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
							foreach (Sequence sequence2 in Sequences)
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
									Sequences.Add(sequence);
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
									Sequences.Remove(sequence);
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
												string text5 = "(" + sequence.Items[i].Index + ") " + ((position != null) ? PassionCommon.Localize(position.Name) : sequence.Items[i].Key);
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
					XML.Element element = XML.Element.Create("Passion");
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
					foreach (string xMLFile in XMLFiles)
					{
						element3.AddChild("Position", xMLFile);
					}
					if (Sequences.Count > 0)
					{
						XML.Element element4 = element.AddChild("Sequences");
						foreach (Sequence sequence in Sequences)
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
						XMLFiles.Clear();
						foreach (XML.Node item in matchingNodes)
						{
							XMLFiles.Add(item.Handle.InnerText);
						}
					}
					ReloadPositions();
				}
				XML.Node matchingNode3 = node.GetMatchingNode("Sequences");
				if (matchingNode3 == null)
				{
					return;
				}
				foreach (XML.Node matchingNode5 in matchingNode3.GetMatchingNodes("Sequence"))
				{
					Sequence sequence = null;
					foreach (Sequence sequence2 in Sequences)
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
						Sequences.Add(sequence);
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
					foreach (Sequence sequence2 in Sequences)
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
						Sequences.Add(sequence);
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
					if (Sequences.Count > 0)
					{
						XML.Element element2 = element.AddChild("Sequences");
						foreach (Sequence sequence in Sequences)
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
		}

		public class XML
		{
			public class File
			{
				protected XmlDocument mHandle;

				public XmlDocument Handle
				{
					get
					{
						return mHandle;
					}
					set
					{
						mHandle = value;
					}
				}

				public bool IsValid
				{
					get
					{
						return Handle != null;
					}
				}

				public Node this[string id]
				{
					get
					{
						if (IsValid && !string.IsNullOrEmpty(id))
						{
							foreach (XmlNode childNode in Handle.ChildNodes)
							{
								if (childNode.NodeType == XmlNodeType.Element && childNode.Name == id)
								{
									return Node.Create(childNode);
								}
							}
						}
						return null;
					}
				}

				public static File Read(string filename)
				{
					ResourceKey key = ResourceKey.CreateXMLKey(filename, 0u);
					return Read(key);
				}

				public static File Read(ResourceKey key)
				{
					if (key != ResourceKey.kInvalidResourceKey)
					{
						XmlDocument handle = Simulator.LoadFromResourceKey(key);
						return new File(handle);
					}
					return new File();
				}

				public File()
				{
				}

				public File(XmlDocument handle)
				{
					Handle = handle;
				}
			}

			public class Node
			{
				protected XmlNode mHandle;

				protected Dictionary<string, string> mValues = new Dictionary<string, string>();

				public XmlNode Handle
				{
					get
					{
						return mHandle;
					}
					set
					{
						mHandle = value;
						if (mHandle == null)
						{
							return;
						}
						mValues.Clear();
						foreach (XmlNode childNode in mHandle.ChildNodes)
						{
							if (childNode.NodeType == XmlNodeType.Element)
							{
								if (mValues.ContainsKey(childNode.Name))
								{
									mValues[childNode.Name] = childNode.InnerText;
								}
								else
								{
									mValues.Add(childNode.Name, childNode.InnerText);
								}
							}
						}
					}
				}

				public bool IsValid
				{
					get
					{
						return mHandle != null;
					}
				}

				public string Value
				{
					get
					{
						if (IsValid)
						{
							return mHandle.InnerText;
						}
						return string.Empty;
					}
				}

				public string this[string key]
				{
					get
					{
						if (!string.IsNullOrEmpty(key) && mValues.ContainsKey(key))
						{
							return mValues[key];
						}
						return string.Empty;
					}
				}

				public static Node Create(XmlNode handle)
				{
					return new Node(handle);
				}

				public Node()
				{
				}

				public Node(XmlNode handle)
				{
					Handle = handle;
				}

				public string GetAttribute(string name)
				{
					if (Handle != null)
					{
						XmlAttributeCollection attributes = Handle.Attributes;
						if (attributes != null)
						{
							XmlAttribute xmlAttribute = (XmlAttribute)attributes.GetNamedItem(name);
							if (xmlAttribute != null)
							{
								return xmlAttribute.Value;
							}
						}
					}
					return string.Empty;
				}

				public Node GetMatchingNode(string key)
				{
					if (IsValid && !string.IsNullOrEmpty(key))
					{
						foreach (XmlNode childNode in mHandle.ChildNodes)
						{
							if (childNode.NodeType == XmlNodeType.Element && childNode.Name == key)
							{
								return Create(childNode);
							}
						}
					}
					return null;
				}

				public List<Node> GetMatchingNodes(string key)
				{
					List<Node> list = new List<Node>();
					if (IsValid && !string.IsNullOrEmpty(key))
					{
						foreach (XmlNode childNode in mHandle.ChildNodes)
						{
							if (childNode.NodeType == XmlNodeType.Element && childNode.Name == key)
							{
								list.Add(Create(childNode));
							}
						}
					}
					return list;
				}
			}

			public class Attribute
			{
				private string Name = string.Empty;

				private string Value = string.Empty;

				public Attribute(string name, string value)
				{
					Name = name;
					Value = value;
				}

				public string ToXML()
				{
					return Name + "=\"" + Value + "\"";
				}
			}

			public class Element
			{
				public string Name = string.Empty;

				public string Value = string.Empty;

				public bool IsComment = false;

				protected List<Attribute> mAttributes = new List<Attribute>();

				protected List<Element> mElements = new List<Element>();

				public List<Attribute> Attributes
				{
					get
					{
						if (mAttributes == null)
						{
							mAttributes = new List<Attribute>();
						}
						return mAttributes;
					}
				}

				public List<Element> ChildElements
				{
					get
					{
						if (mElements == null)
						{
							mElements = new List<Element>();
						}
						return mElements;
					}
				}

				public static Element Create(string name)
				{
					return Create(name, string.Empty, null);
				}

				public static Element Create(string name, List<Attribute> attributes)
				{
					return Create(name, string.Empty, attributes);
				}

				public static Element Create(string name, string value)
				{
					return Create(name, value, null);
				}

				public static Element Create(string name, string value, List<Attribute> attributes)
				{
					Element element = new Element();
					element.Name = name;
					element.Value = value;
					if (attributes != null)
					{
						element.Attributes.AddRange(attributes);
					}
					return element;
				}

				public Element AddChild(string name)
				{
					return AddChild(name, string.Empty);
				}

				public Element AddChild(string name, string value)
				{
					return AddChild(Create(name, value));
				}

				public Element AddChild(Element child)
				{
					if (child != null)
					{
						ChildElements.Add(child);
					}
					return child;
				}

				public Element AddComment(string text)
				{
					Element element = AddChild("Comment", text);
					element.IsComment = true;
					return element;
				}

				public Attribute AddAttribute(string key, string value)
				{
					Attribute attribute = null;
					if (!string.IsNullOrEmpty(key))
					{
						attribute = new Attribute(key, value);
						Attributes.Add(attribute);
					}
					return attribute;
				}

				public string ToXML()
				{
					return ToXML(string.Empty);
				}

				protected string ToXML(string depth)
				{
					string empty = string.Empty;
					string depth2 = depth + "  ";
					if (IsComment)
					{
						return depth + "<!-- " + Value + " -->";
					}
					empty = empty + depth + "<" + Name;
					if (Attributes.Count > 0)
					{
						foreach (Attribute attribute in Attributes)
						{
							empty = empty + " " + attribute.ToXML();
						}
					}
					empty += ">";
					if (ChildElements.Count > 0)
					{
						empty += PassionCommon.NewLine;
						foreach (Element childElement in ChildElements)
						{
							empty += childElement.ToXML(depth2);
						}
						empty += depth;
					}
					else
					{
						empty += Value;
					}
					return empty + "</" + Name + ">" + PassionCommon.NewLine;
				}
			}

			public const string XMLDeclaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

			public const string PassionPrefix = "PassionSettingsExport_";

			public static bool WriteToPackage(Element root)
			{
				return WriteToPackage(root, null);
			}

			public static bool WriteToPackage(Element root, string name)
			{
				if (root != null)
				{
					string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + PassionCommon.NewLine;
					text += root.ToXML();
					if (string.IsNullOrEmpty(name))
					{
						bool flag = true;
						string text2 = null;
						name = string.Empty;
						while (flag)
						{
							try
							{
								flag = false;
								text2 = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.ExportFile"), PassionCommon.Localize("S3_Passion.Terms.ExportFileText"), DateTime.Now.ToString("MM-dd-yyyy (hh:mm)"));
								if (string.IsNullOrEmpty(text2))
								{
									return false;
								}
								name = "PassionSettingsExport_" + text2;
								Sims3.Gameplay.BinModel.Singleton.PopulateExportBin();
								foreach (IExportBinContents item in new List<IExportBinContents>(Sims3.Gameplay.BinModel.Singleton.ExportBinContents))
								{
									if (item != null && !string.IsNullOrEmpty(item.HouseholdName) && item.HouseholdName.ToLower() == name.ToLower())
									{
										Sims3.Gameplay.BinModel.Singleton.DeleteFromExportBin(item.ContentId);
										break;
									}
								}
							}
							catch
							{
							}
						}
					}
					else
					{
						Sims3.Gameplay.BinModel.Singleton.PopulateExportBin();
						foreach (IExportBinContents item2 in new List<IExportBinContents>(Sims3.Gameplay.BinModel.Singleton.ExportBinContents))
						{
							if (item2 != null && !string.IsNullOrEmpty(item2.HouseholdName) && item2.HouseholdName.ToLower() == name.ToLower())
							{
								Sims3.Gameplay.BinModel.Singleton.DeleteFromExportBin(item2.ContentId);
								break;
							}
						}
					}
					Household household = Household.Create();
					household.SetName(name);
					household.BioText = text;
					Sims3.Gameplay.BinModel.Singleton.AddToExportBin(household);
					household.Destroy();
					return true;
				}
				return false;
			}

			public static File ReadFromPackage()
			{
				return ReadFromPackage(null);
			}

			public static File ReadFromPackage(string name)
			{
				if (string.IsNullOrEmpty(name))
				{
					bool flag = true;
					bool flag2 = false;
					while (flag)
					{
						Sims3.Gameplay.BinModel.Singleton.PopulateExportBin();
						GenericDialog.OptionList<IExportBinContents> optionList = new GenericDialog.OptionList<IExportBinContents>();
						foreach (IExportBinContents item in new List<IExportBinContents>(Sims3.Gameplay.BinModel.Singleton.ExportBinContents))
						{
							if (item != null && item.HouseholdName != null && item.HouseholdName.StartsWith("PassionSettingsExport_"))
							{
								optionList.Add(item.HouseholdName.Replace("PassionSettingsExport_", string.Empty), item);
							}
						}
						if (optionList.Count > 0)
						{
							flag2 = true;
							IExportBinContents exportBinContents = GenericDialog.Ask(optionList, PassionCommon.Localize("S3_Passion.Terms.ImportFile"), true);
							if (exportBinContents == null)
							{
								break;
							}
							GenericDialog.OptionList<string> optionList2 = new GenericDialog.OptionList<string>();
							optionList2.Add(PassionCommon.Localize("S3_Passion.Terms.Ok"), "Ok");
							optionList2.Add(PassionCommon.Localize("S3_Passion.Terms.Remove"), "Remove");
							string text = GenericDialog.Ask(optionList2, "\"" + exportBinContents.HouseholdName + "\"");
							if (!(text == "Remove"))
							{
								if (text == "Ok")
								{
									XmlDocument xmlDocument = new XmlDocument();
									xmlDocument.LoadXml(exportBinContents.HouseholdBio);
									return new File(xmlDocument);
								}
							}
							else
							{
								Sims3.Gameplay.BinModel.Singleton.DeleteFromExportBin(exportBinContents.ContentId);
							}
							continue;
						}
						if (!flag2)
						{
							PassionCommon.SystemMessage(PassionCommon.Localize("S3_Passion.Terms.NoFilesFound"));
						}
						break;
					}
				}
				else
				{
					Sims3.Gameplay.BinModel.Singleton.PopulateExportBin();
					foreach (IExportBinContents item2 in new List<IExportBinContents>(Sims3.Gameplay.BinModel.Singleton.ExportBinContents))
					{
						if (item2 != null && !string.IsNullOrEmpty(item2.HouseholdName) && item2.HouseholdName.ToLower() == name.ToLower())
						{
							XmlDocument xmlDocument2 = new XmlDocument();
							xmlDocument2.LoadXml(item2.HouseholdBio);
							return new File(xmlDocument2);
						}
					}
				}
				return null;
			}

			public static bool WriteToFile(Element root, string prefix)
			{
				try
				{
					if (root != null)
					{
						uint fileHandle = 0u;
						Simulator.CreateExportFile(ref fileHandle, prefix);
						if (fileHandle != 0)
						{
							CustomXmlWriter customXmlWriter = new CustomXmlWriter(fileHandle);
							customXmlWriter.WriteToBuffer("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + PassionCommon.NewLine);
							customXmlWriter.WriteToBuffer(root.ToXML());
							customXmlWriter.WriteEndDocument();
							return true;
						}
					}
				}
				catch
				{
				}
				return false;
			}

			public static File Create(string filename)
			{
				return File.Read(filename);
			}
		}

		public class SectionalManager : IAlarmOwner
		{
			public const float LoadDelay = 2f;

			public static SectionalManager Singleton = new SectionalManager();

			public static AlarmHandle UpdateAlarm = AlarmHandle.kInvalidHandle;

			public static void TriggerUpdate()
			{
				if (UpdateAlarm == AlarmHandle.kInvalidHandle)
				{
					UpdateAlarm = AlarmManager.Global.AddAlarm(2f, TimeUnit.Minutes, CheckSectionals, "Sectional Furniture Check", AlarmType.NeverPersisted, Singleton);
				}
			}

			public static void CheckSectionals()
			{
				try
				{
					ChairSectional[] objects = Sims3.Gameplay.Queries.GetObjects<ChairSectional>();
					if (objects != null)
					{
						ChairSectional[] array = objects;
						foreach (ChairSectional chairSectional in array)
						{
							if (chairSectional.Section != Sims3.Gameplay.Objects.Seating.Section.CornerConcave)
							{
								chairSectional.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
								chairSectional.AddInteraction(Interactions.UseObjectForPassionWithSim.Singleton, true);
								chairSectional.AddInteraction(Interactions.ResetMe.Singleton, true);
								chairSectional.AddInteraction(Interactions.ResetMeActive.Singleton, true);
								chairSectional.AddInteraction(Interactions.MoveTo.Singleton, true);
								chairSectional.AddInteraction(Interactions.MoveGroupTo.Singleton, true);
							}
							else
							{
								chairSectional.RemoveInteractionByType(Interactions.UseObjectForPassion.Singleton);
								chairSectional.RemoveInteractionByType(Interactions.UseObjectForPassionWithSim.Singleton);
								chairSectional.RemoveInteractionByType(Interactions.ResetMe.Singleton);
								chairSectional.RemoveInteractionByType(Interactions.ResetMeActive.Singleton);
								chairSectional.RemoveInteractionByType(Interactions.MoveTo.Singleton);
								chairSectional.RemoveInteractionByType(Interactions.MoveGroupTo.Singleton);
							}
						}
					}
				}
				catch
				{
				}
				UpdateAlarm = AlarmHandle.kInvalidHandle;
			}
		}

		public class Autonomy
		{
			public static ListenerAction HappendAtMassage(Event e)
			{
				return ListenerAction.Keep;
			}

			// nude/stripper dance autonomy
			public static ListenerAction DanceNude2Music(Event e)
			{
				ResourceKey key = ResourceKey.FromString("0x02DC343F-0x08000000-0x475CA79579FF223E");
				if (World.ResourceExists(key) && Settings.StripperAutonomy)
				{
					try
					{
					// if the autonomy chance is higher than the RNG number
						if (Settings.AutonomyChance > 0 && RandomUtil.GetInt(0, 99) < Settings.AutonomyChance)
						{
							Sim sim = e.Actor as Sim;
							GameObject gameObject = e.TargetObject as GameObject;
							int num = sim.LotCurrent.GetAllActorsCount();
							uint num2 = sim.LotCurrent.CountObjects<SculptureFloorGunShow>();
							foreach (Sim allActor in sim.LotCurrent.GetAllActors())
							{
								Player player = GetPlayer(allActor);
								if (allActor != null && !allActor.LotCurrent.IsWorldLot && (player.Actor.SimDescription.ChildOrBelow || !player.Actor.SimDescription.IsHuman))
								{
									num--;
								}
							}
							if (Settings.AutonomyActive)
							{
								Player player2 = GetPlayer(sim);
								SimDescription simDescription = sim.SimDescription;
								if (player2.IsValid && !simDescription.ChildOrBelow && num >= 3 && num2 != 0 && RandomUtil.CoinFlip())
								{
									if (num2 <= 2)
									{
										if (RandomUtil.CoinFlip())
										{
											PassionCommon.Wait(1800u);
										}
										else
										{
											PassionCommon.Wait(1200u);
										}
									}
									else if (num2 > 2)
									{
										if (RandomUtil.CoinFlip())
										{
											PassionCommon.Wait(900u);
										}
										else
										{
											PassionCommon.Wait(600u);
										}
									}
									List<string> list = new List<string>();
									foreach (Sim sim2 in sim.LotCurrent.GetSims())
									{
										if (sim2.SimDescription.IsServicePerson && !(sim2.Service is Butler) && !(sim2.Service is Maid))
										{
											list.Add(sim2.Name.ToString());
										}
									}
									string randomObjectFromList;
									if (list.Count > 0)
									{
										randomObjectFromList = RandomUtil.GetRandomObjectFromList(list);
									}
									else
									{
										foreach (Sim sim3 in sim.LotCurrent.GetSims())
										{
											if (sim3.SimDescription.TeenOrAbove && sim3.SimDescription.IsHuman && !sim3.LotCurrent.IsWorldLot)
											{
												list.Add(sim3.Name.ToString());
											}
										}
										randomObjectFromList = RandomUtil.GetRandomObjectFromList(list);
									}
									foreach (Sim sim4 in sim.LotCurrent.GetSims())
									{
										if (sim4.Name.ToString() == randomObjectFromList)
										{
											sim = sim4;
											player2 = GetPlayer(sim);
										}
									}
									player2.IsAutonomous = true;
									List<SculptureFloorGunShow> list2 = new List<SculptureFloorGunShow>(Sims3.Gameplay.Queries.GetObjects<SculptureFloorGunShow>(sim.LotCurrent, sim.RoomId));
									if (list2.Count > 0)
									{
										foreach (SculptureFloorGunShow item in list2)
										{
											if (item.UseCount < 1)
											{
												gameObject = item;
												break;
											}
											gameObject = null;
										}
									}
									if (sim != null && gameObject != null && RandomUtil.CoinFlip() && !sim.SimDescription.Elder && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != StripperPole.WatchStrip.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.UseObjectForPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.UseSimForPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.ActiveJoinPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.JoinPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.AskToJoinPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.AskToSoloPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.AskToPassionOther.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.AskToWatchPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.WatchPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.WatchMasturbate.Singleton)
									{
										if (sim.SimDescription.IsServicePerson && !(sim.Service is Butler) && !(sim.Service is Maid) && sim.CurrentOutfitCategory != OutfitCategories.Career)
										{
											MyOutfit = sim.CurrentOutfitCategory;
											sim.SwitchToOutfitWithoutSpin(OutfitCategories.Career, 0);
										}
										sim.InteractionQueue.AddAfterCheckingForDuplicates(StripperPole.AutoDance.Singleton.CreateInstance(gameObject, sim, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
									}
								}
							}
						}
					}
					catch
					{
					}
					return ListenerAction.Keep;
				}
				return ListenerAction.Keep;
			}

			// masturbate while watching passion tv
			public static ListenerAction WhenWatchTV(Event e)
			{
				ResourceKey key = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x8DC278D813275705");
				ResourceKey key2 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0xEFD91C1084B3E2DA");
				ResourceKey key3 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x8647C1AAD74E30F7");
				ResourceKey key4 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0xDC118FA318BD0EE4");
				ResourceKey key5 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x046C240AEA28A251");
				ResourceKey key6 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x875667F1FFDDF916");
				ResourceKey key7 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0xCC02BFCB7272D603");
				ResourceKey key8 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x7649533123BEF070");
				if (World.ResourceExists(key) && World.ResourceExists(key2) && World.ResourceExists(key3) && World.ResourceExists(key4) && World.ResourceExists(key5) && World.ResourceExists(key6) && World.ResourceExists(key7) && World.ResourceExists(key8))
				{
					try
					{
						if (Settings.AutonomyChance > 0 && RandomUtil.GetInt(0, 99) < Settings.AutonomyChance)
						{
							Sim sim = e.Actor as Sim;
							Sim sim2 = e.TargetObject as Sim;
							if (TVChannelData.GetChannelNames(TVChannelLevel.Level1).Contains("Gameplay/Excel/TV/TVChannel:Channel_1") && TVChannelData.GetChannelNames(TVChannelLevel.Level1).Contains("Gameplay/Excel/TV/TVChannel:Channel_2") && TVChannelData.GetChannelNames(TVChannelLevel.Level1).Contains("Gameplay/Excel/TV/TVChannel:Channel_3") && TVChannelData.GetChannelNames(TVChannelLevel.Level1).Contains("Gameplay/Excel/TV/TVChannel:Channel_4") && (Settings.AutonomyActive || (sim.Household != Household.ActiveHousehold && sim2.Household != Household.ActiveHousehold)) && (Settings.AutonomyPublic || sim.LotCurrent.LotType == LotType.Residential))
							{
								Player player = GetPlayer(sim);
								SimDescription simDescription = sim.SimDescription;
								if (player.IsValid && !player.IsActive && !simDescription.ChildOrBelow && (player.Actor.InteractionQueue.GetHeadInteraction().ToString() == PassionCommon.Localize("Gameplay/Objects/Electronics/TV/WatchTV:WatchTVInteractionName") || player.Actor.InteractionQueue.GetHeadInteraction().ToString() == PassionCommon.Localize("Gameplay/Objects/Electronics/TV/WatchTVAutonomously:WatchTVInteractionName")))
								{
									player.IsAutonomous = true;
									ObjectGuid objectId = player.Actor.CurrentInteraction.Target.ObjectId;
									IGameObject target = player.Actor.CurrentInteraction.Target;
									// from what i can tell...
									// this is effectively, if the sim is aroused, wait and then auto masturbate. the more aroused they are, the shorter the wait

									//100%
									if (player.Actor.BuffManager.HasElement((BuffNames)2922253427052633003uL))
									{
										PassionCommon.Wait(400);
										player.Actor.InteractionQueue.CancelAllInteractions();
										player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
									}
									// 90%
									else if (player.Actor.BuffManager.HasElement((BuffNames)13147589483235469726uL))
									{
										PassionCommon.Wait(600);
										player.Actor.InteractionQueue.CancelAllInteractions();
										player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
									}
									// 80%
									else if (player.Actor.BuffManager.HasElement((BuffNames)14041574305464178967uL))
									{
										PassionCommon.Wait(800);
										player.Actor.InteractionQueue.CancelAllInteractions();
										player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
									}
									// 70%
									else if (player.Actor.BuffManager.HasElement((BuffNames)16251613925768384549uL))
									{
										PassionCommon.Wait(1000);
										player.Actor.InteractionQueue.CancelAllInteractions();
										player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
									}
									// 60%
									else if (player.Actor.BuffManager.HasElement((BuffNames)2917472750494117670uL))
									{
										PassionCommon.Wait(1500);
										player.Actor.InteractionQueue.CancelAllInteractions();
										player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
									}
								}
							}
						}
					}
					catch
					{
					}
					return ListenerAction.Keep;
				}
				return ListenerAction.Remove;
			}

			// passioncheck -- check to **initiate** autonomous passion
			// this runs through 'willpassion' for both sims, so see that for like. conditions n shit
			public static ListenerAction PassionCheck(Event e)
			{
				try
				{
					// im scared
					// PassionCommon.SystemMessage("passioncheck fired alright");

					Sim guy = e.Actor as Sim;
					Sim guy2 = e.TargetObject as Sim;

					ShortTermContext sTC = Relationship.GetSTC(guy, guy2);

					// if the current convo stc is romantic
					// otherwise fuck you
					if (sTC.IsRomantic)
					{

						int ChargeThreshold = 0;


						Player playerguy = GetPlayer(guy);
						Player playerguy2 = GetPlayer(guy2);

						// add 10 to their charge
						if (guy.TraitManager.HasElement((TraitNames)5711695705602619160uL))
						{
							playerguy.PassionCharge += 25;
						}
						else
						{
                            playerguy.PassionCharge += 10;
                        }
                        // PassionCommon.SystemMessage("sim1 preroll charge is" + playerguy.PassionCharge);
                        if (guy2.TraitManager.HasElement((TraitNames)5711695705602619160uL))
                        {
                            playerguy2.PassionCharge += 25;
                        }
                        else
                        {
                            playerguy2.PassionCharge += 10;
                        }
                        // PassionCommon.SystemMessage("sim2 preroll charge is" + playerguy2.PassionCharge);

                        ChargeThreshold = RandomUtil.GetInt(0, 100);
						// PassionCommon.SystemMessage("charge threshold is" + ChargeThreshold);

						if (playerguy.PassionCharge >= ChargeThreshold)
						{
							Libido.IncreaseUrgency(guy);
							playerguy.PassionCharge = 0;
							// PassionCommon.SystemMessage("sim1 increased libido");
						}
						if (playerguy2.PassionCharge >= ChargeThreshold)
						{
							Libido.IncreaseUrgency(guy2);
							playerguy2.PassionCharge = 0;
							// PassionCommon.SystemMessage("sim2 increased libido");
						}


						// if autonomychance is higher than random, check continues.
						// refactor this so it takes libido into account as well?
						if (Settings.AutonomyChance > 0 && RandomUtil.GetInt(0, 99) < Settings.AutonomyChance)
						{
							Sim sim = e.Actor as Sim;
							Sim sim2 = e.TargetObject as Sim;
							if (!(sim.Posture is RelaxingPosture) && !(sim.Posture is SittingPosture) && Player.CanPassion(sim, sim2) && Player.WillPassion(sim, sim2) && Player.WillPassion(sim2, sim) && (Settings.AutonomyActive || (sim.Household != Household.ActiveHousehold && sim2.Household != Household.ActiveHousehold)))
							{
								Player player = GetPlayer(sim);
								Player player2 = GetPlayer(sim2);
								if (player.IsValid && player2.IsValid && !player.IsActive && !player2.IsActive)
								{

									Target target = player.GetNearbySupportedTarget();
									if (target == null)
									{
										target = GetTarget(sim2);
									}
									if (target != null && target.IsValid)
									{
										Part part = null;
										if (target.Parts.Count > 0)
										{
											part = RandomUtil.GetRandomObjectFromList(new List<Part>(target.Parts.Values));
										}
										if (part != null && player.Join(part))
										{
											player.IsAutonomous = true;
											player2.IsAutonomous = true;
											player.Actor.InteractionQueue.CancelAllInteractions();
											player.Actor.InteractionQueue.AddNext(Interactions.AskToPassion.Singleton.CreateInstance(player2.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
											if (Settings.AutonomyNotify)
											{
												try
												{
													StringBuilder stringBuilder = new StringBuilder(Localize("S3_Passion.Terms.AutonomyNotifySimMessage"));
													stringBuilder.Replace("[player]", player.Name);
													stringBuilder.Replace("[label]", Settings.ActiveLabel.ToLower());
													stringBuilder.Replace("[partner]", player2.Name);
													stringBuilder.Replace("[address]", player.Actor.LotCurrent.Name);
													SimMessage(stringBuilder.ToString(), player.Actor, player2.Actor);
												}
												catch
												{
												}
											}
										}
									}
									else if (Testing)
									{
										SystemMessage("No valid target found for Autonomy for " + player.Name + " & " + player2.Name);
									}

								}
							}
						}

					}
					else
					{
						// SystemMessage("context isnt romantic. fuck you.");
					}
				}
				catch
				{
                    // SystemMessage("uh oh");
                }
                return ListenerAction.Keep;
            }

			// jealousy/cheating check
			public static void JealousyCheck(Sim witness, ReactionBroadcaster rb)
			{
				try
				{
					// if jealousy is off or there are no witnesses
					if (!Settings.Jealousy || witness == null || !(rb.BroadcastingObject is Sim))
					{
						return;
					}
					Player player = GetPlayer(rb.BroadcastingObject as Sim);
					if (player == null || !player.IsInitiator || !player.HadPartner || witness == player.Actor)
					{
						return;
					}
					bool flag = Settings.PolyamorousJealousy;
					List<Sim> list = new List<Sim>();
					List<Sim> list2 = new List<Sim>();
					foreach (Player value in player.Part.Players.Values)
					{
						if (value == null || !value.IsValid)
						{
							continue;
						}
						bool flag2 = value.Actor.HasTrait(TraitNames.NoJealousy);
						if (!flag2)
						{
							Relationship relationship = Relationship.Get(witness, value.Actor, false);
							if (relationship != null)
							{
								LTRData lTRData = LTRData.Get(relationship.LTR.CurrentLTR);
								if (witness.Partner != value.Actor.SimDescription && !lTRData.IsRomantic)
								{
									flag = false;
								}
							}
							else
							{
								flag = false;
							}
						}
						if (!list.Contains(value.Actor))
						{
							list.Add(value.Actor);
						}
						if (witness == value.Actor)
						{
							return;
						}
						if (!flag2 && !list2.Contains(value.Actor))
						{
							list2.Add(value.Actor);
						}
					}
					if (flag || list2.Count < 1)
					{
						return;
					}
					List<Sim> list3 = new List<Sim>();
					foreach (Sim item in list2)
					{
						Relationship relationship2 = Relationship.Get(witness, item, false);
						if (relationship2 != null)
						{
							LTRData lTRData2 = LTRData.Get(relationship2.LTR.CurrentLTR);
							if (witness.Partner == item.SimDescription || lTRData2.IsRomantic)
							{
								SocialComponent.OnIWasCheatedOn(witness, item.SimDescription, player.Actor.SimDescription, JealousyLevel.High);
								RomanceVisibilityState.PushAccuseSimOfBetrayal(witness, item);
								continue;
							}
						}
						if (!Settings.PolyamorousJealousy && item.Partner != null && item.Partner.CreatedSim != null && !list.Contains(item.Partner.CreatedSim) && witness.Genealogy.IsBloodRelated(item.Partner.Genealogy))
						{
							SocialComponent.OnSomeoneICareAboutWasCheatedOn(witness, item.Partner, item.SimDescription, player.Actor.SimDescription, JealousyLevel.High);
						}
					}
				}
				catch
				{
					if (PassionCommon.Testing)
					{
						PassionCommon.SystemMessage("Error during JealousyCheck");
					}
				}
			}
		}

		[Persistable]

		// sim stats
		public class Player
		{
			public ulong ID;

			public PassionState State;

            public object SimStrapon = PassionGenitals.SimStraponList.UNSET;

			public string SimGenitalType;

			public string SimJunkBaseCASP;

			public string SimErectSIMO;

            public bool IsActive;

			public bool IsInPlace;

			public bool IsAutonomous;

			public bool DirectTargeted;

			public bool ActiveLeave;

			public bool ActiveJoin;

			public bool SpinDisabled;

			public bool HasPreferredOutfit;

			public bool CanAnimate;

			public bool CanSwitch;

			public bool AreWeSwitching;

			public long StartTime;

			public int NumberAccepted;

			public Part Part;

			public int PositionIndex;

			public int PassionCharge;

			public string BufferedAnimation;

			public string SwitchRoleBuffer;

			public string BufferedTargetAnimation;

			public string BufferedObjectAnimation;

			public HeldItem HeldItem;

			public HeldItem HeldItemWithNoKey;

			public Player Partner;

			public Part SwitchPart;

			public Vector3 ExitPoint = Vector3.Empty;

			public OutfitCategories PreferredOutfitCategory;

			public OutfitCategories PreviousOutfitCategory;

			public RejectionReasons RejectionReason;

			public int PreferredOutfitIndex;

			public int PreviousOutfitIndex;

			public int PartnersToCheckCount = 0;

			public List<Player> PotentialImpregnators = new List<Player>();

			public List<Player> PartnersUpdated = new List<Player>();

			public List<Player> PartnersToCheck = new List<Player>();

			protected ReactionBroadcaster JealousyBroadcaster;

			protected Sim mActor;

			public SimDescription mDescription;

			protected float mHeightModifier = 0f;

			public Sim.SwitchOutfitHelper mSwitchOutfitHelper;

			public Sim Actor
			{
				get
				{
					if (mActor == null && mDescription != null)
					{
						mActor = mDescription.CreatedSim;
					}
					return mActor;
				}
				set
				{
					mActor = value;
					if (mActor != null)
					{
						mDescription = mActor.SimDescription;
						ID = mActor.ObjectId.Value;
					}
					else
					{
						mDescription = null;
						ID = 0uL;
					}
				}
			}

			public SimDescription Description
			{
				get
				{
					if (mDescription == null && mActor != null)
					{
						mDescription = mActor.SimDescription;
					}
					return mDescription;
				}
				set
				{
					mDescription = value;
					if (mDescription != null)
					{
						mActor = mDescription.CreatedSim;
						ID = mActor.ObjectId.Value;
					}
					else
					{
						mActor = null;
						ID = 0uL;
					}
				}
			}

			public float HeightModifier
			{
				get
				{
					return mHeightModifier;
				}
			}

			public string Name
			{
				get
				{
					if (IsValid)
					{
						return Actor.Name;
					}
					return "Player (No Actor)";
				}
			}

			public uint Flags
			{
				get
				{
					if (IsValid)
					{
						return Actor.SimDescription.SimFlags;
					}
					return 0u;
				}
			}

			public Vector3 Location
			{
				get
				{
					if (IsValid)
					{
						return Actor.Position;
					}
					return Vector3.Empty;
				}
				set
				{
					if (IsValid)
					{
						Actor.SetPosition(value);
					}
				}
			}

			public Vector3 Forward
			{
				get
				{
					if (IsValid)
					{
						return Actor.ForwardVector;
					}
					return Vector3.Empty;
				}
				set
				{
					if (IsValid)
					{
						Actor.SetForward(value);
					}
				}
			}

			public bool ActiveLeaveJoin
			{
				get
				{
					return ActiveLeave && ActiveJoin;
				}
				set
				{
					ActiveLeave = value;
					ActiveJoin = value;
				}
			}

			public OutfitCategories Outfit
			{
				get
				{
					if (IsValid)
					{
						return Actor.CurrentOutfitCategory;
					}
					return OutfitCategories.None;
				}
				set
				{
					SetOutfit(value);
				}
			}

			public Player PlayerInteractedWith
			{
				get
				{
					Player result = null;
					if (HasPart && Part.HasPosition && Part.Position.InteractsWith.ContainsKey(PositionIndex))
					{
						foreach (Player value in Part.Players.Values)
						{
							if (value != null && value.PositionIndex == Part.Position.InteractsWith[PositionIndex])
							{
								result = value;
								break;
							}
						}
					}
					return result;
				}
			}

			public List<Player> Partners
			{
				get
				{
					List<Player> list = new List<Player>();
					if (HasPart)
					{
						foreach (Player value in Part.Players.Values)
						{
							if (value != this)
							{
								list.Add(value);
							}
						}
					}
					return list;
				}
			}

			public bool HadPartner
			{
				get
				{
					return Partner != null && Partner.IsValid;
				}
			}

			public bool IsValid
			{
				get
				{
					return Actor != null && !Actor.HasBeenDestroyed && !Actor.TraitManager.HasElement((TraitNames)2214287488174702228uL);
				}
			}

			public bool IsInitiator
			{
				get
				{
					if (IsValid && HasPart && Part.Initiator.IsValid && Part.Initiator.Actor == Actor)
					{
						return true;
					}
					return false;
				}
			}

			public bool IsStopping
			{
				get
				{
					return State == PassionState.Stopping || State == PassionState.Leaving;
				}
			}

			public bool IsWatching
			{
				get
				{
					return State == PassionState.Watching;
				}
			}

			public bool IsSolo
			{
				get
				{
					return IsValid && IsActive && HasPart && Part.Players.Count == 1;
				}
			}

			public bool IsInValidObject
			{
				get
				{
					return !(Actor.CurrentInteraction.Target is Sim) && Actor.CurrentInteraction.Target.IsActorUsingMe(Actor) && PassionType.IsSupported(Actor.CurrentInteraction.Target);
				}
			}

			public bool IsInPool
			{
				get
				{
					if (IsValid && Actor.Posture is SwimmingInPool)
					{
						return true;
					}
					return false;
				}
			}

			// if youre dying you cant fuck :pensive:
			public bool IsInMotiveDesperation
			{
				get
				{
					if (IsValid && Actor.Motives != null && Actor.Motives.InMotiveDistress && (Settings.Motives == PassionMotives.EADefault || Settings.Motives == PassionMotives.PassionStandard) && (Actor.Motives.GetValue(CommodityKind.Energy) < -90f || Actor.Motives.GetValue(CommodityKind.Bladder) < -90f || Actor.Motives.GetValue(CommodityKind.Hunger) < -90f || Actor.Motives.GetValue(CommodityKind.MermaidDermalHydration) < -90f || Actor.Motives.GetValue(CommodityKind.VampireThirst) < -90f || Actor.Motives.GetValue(CommodityKind.AlienBrainPower) < -90f))
					{
						return true;
					}
					return false;
				}
			}

			// i think this is the timeout in between autonomous passion allowances?
			public bool IsTimedOut
			{
				get
				{
					if (StartTime != 0L && HasPart && (!Part.HasSequence || Part.CurrentSequence.Repeat))
					{
						if ((Settings.MaxLength > 0 && StartTime + Settings.MaxLength < SimClock.CurrentTicks) || (IsAutonomous && StartTime + Settings.AutonomyLength < SimClock.CurrentTicks))
						{
							return true;
						}
						return false;
					}
					return false;
				}
			}

			public bool IsFull
			{
				get
				{
					if (Settings.EndWhenFull && IsValid && Settings.Motives != PassionMotives.Freeze && Actor.Motives.GetMotiveValue(CommodityKind.Fun) >= 100f && (!HadPartner || Actor.Motives.GetMotiveValue(CommodityKind.Social) >= 100f) && (!HasPart || !Part.HasTarget || !Part.Target.HasObject || (!(Part.Target.Object is IShowerable) && !(Part.Target.Object is IBathtub)) || Actor.Motives.GetMotiveValue(CommodityKind.Hygiene) >= 100f))
					{
						return true;
					}
					return false;
				}
			}

			public bool ShouldStop
			{
				get
				{
					if (State == PassionState.Stopping || State == PassionState.Leaving)
					{
						return true;
					}
					if (Actor.HasExitReason())
					{
						return true;
					}
					return false;
				}
			}

			public bool HasPart
			{
				get
				{
					return Part != null;
				}
			}

			public bool HasSwitchPart
			{
				get
				{
					return SwitchPart != null;
				}
			}

			public bool HasCigarette
			{
				get
				{
					return PassionCommon.HasPart(Actor, SimPart.Cigarette);
				}
			}



			public Sim.SwitchOutfitHelper SwitchOutfitHelper
			{
				get
				{
					return mSwitchOutfitHelper;
				}
				set
				{
					if (mSwitchOutfitHelper != value)
					{
						if (mSwitchOutfitHelper != null)
						{
							mSwitchOutfitHelper.Dispose();
						}
						mSwitchOutfitHelper = value;
					}
				}
			}

			public static Player Create(Sim sim)
			{
				Player player = new Player();
				player.Actor = sim;
				player.RefreshHeightModifier(sim);
                player.SimStrapon = PassionGenitals.SimStraponList.UNSET;
				player.SimGenitalType = "";
				player.SimJunkBaseCASP = "";
				player.SimErectSIMO = "";
                player.PositionIndex = 0;
				player.PassionCharge = 0;
				player.CanAnimate = false;
				player.CanSwitch = false;
				player.AreWeSwitching = false;
				player.IsAutonomous = false;
				player.DirectTargeted = false;
				player.ActiveJoin = false;
				player.NumberAccepted = 0;
				player.Partner = null;
				player.SwitchPart = null;
				player.StartTime = 0L;
				player.HasPreferredOutfit = false;
				player.PreferredOutfitCategory = OutfitCategories.None;
				player.PreferredOutfitIndex = 0;
				player.PreviousOutfitCategory = OutfitCategories.None;
				player.RejectionReason = RejectionReasons.None;
				player.PreviousOutfitIndex = 0;
				player.BufferedAnimation = string.Empty;
				player.SwitchRoleBuffer = string.Empty;
				return player;
			}

			public void SetPartnersToCheck(List<Sim> partners)
			{
				PartnersToCheck.Clear();
				if (partners == null)
				{
					return;
				}
				foreach (Sim partner in partners)
				{
					PartnersToCheck.Add(GetPlayer(partner));
				}
				PartnersToCheckCount = PartnersToCheck.Count;
			}

			public void RefreshHeightModifier(Sim sim)
			{
				if (IsValid && sim.SimDescription.Teen)
				{
					mHeightModifier = SimPart.Types.Height.TeenMorph(sim);
				}
				else
				{
					mHeightModifier = 0f;
				}
			}

			public void SetOutfit(OutfitCategories category)
			{
				SetOutfit(category, 0);
			}

			public void SetOutfit(OutfitCategories category, int index)
			{
				if (IsValid && !Actor.IsSimBot && !Actor.IsEP11Bot && (index != Actor.CurrentOutfitIndex || category != Actor.CurrentOutfitCategory))
				{
					if (index < 0 || index >= Actor.SimDescription.GetOutfitCount(category))
					{
						index = 0;
					}
					if (SpinDisabled)
					{
						Actor.SwitchToOutfitWithoutSpin(category, index);
					}
					else
					{
						Actor.SwitchToOutfitWithSpin(category, index);
					}
				}
			}

			public void RevertOutfit()
			{
				SetOutfit(PreviousOutfitCategory, PreviousOutfitIndex);
			}

			public void PreferOutfit()
			{
				if (IsValid)
				{
					PreferredOutfitCategory = Actor.CurrentOutfitCategory;
					PreferredOutfitIndex = Actor.CurrentOutfitIndex;
					HasPreferredOutfit = true;
				}
			}

			public bool UsePreferredOutfit()
			{
				if (HasPreferredOutfit)
				{
					try
					{
						SetOutfit(PreferredOutfitCategory, PreferredOutfitIndex);
						return true;
					}
					catch
					{
						ClearPreferredOutfit();
					}
				}
				return false;
			}

			public void ClearPreferredOutfit()
			{
				HasPreferredOutfit = false;
				PreferredOutfitCategory = OutfitCategories.None;
				PreferredOutfitIndex = 0;
			}

			public void SaveOutfit()
			{
				if (IsValid)
				{
					PreviousOutfitCategory = Actor.CurrentOutfitCategory;
					PreviousOutfitIndex = Actor.CurrentOutfitIndex;
				}
			}



			public void ClearBuffer()
			{
				BufferedAnimation = string.Empty;
			}

			public bool CanPassion(Sim partner)
			{
				return CanPassion(Actor, partner);
			}

			public static bool CanPassion(Sim sim, Sim partner)
			{
				return IsValid(sim) && IsValid(partner) && (!sim.IsRelated(partner) || Settings.Incest);
			}

			public bool WillPassion(Player partner)
			{
				return IsValid && partner != null && partner.IsValid && WillPassion(Actor, partner.Actor);
			}

			public bool WillPassion(Part part)
			{
				if (IsValid && part != null)
				{
					int num = 0;
					Sim[] array = new Sim[part.Players.Count];
					try
					{
						foreach (Player value in part.Players.Values)
						{
							if (value.IsValid)
							{
								array[num] = value.Actor;
							}
							num++;
						}
					}
					catch
					{
					}
					return WillPassion(Actor, array);
				}
				return false;
			}

			public static bool WillPassion(Sim sim, Sim target)
			{
				return WillPassion(sim, new Sim[1] { target });
			}


			// tests if a sim will ACCEPT a passion request or not
			// modify this to include multiplers for new libido system
			// from what i can see.. higher the number, more likely to accept. lets yoloswag
			public static bool WillPassion(Sim sim, Sim[] targets)
			{
				// if rejection is disabled
				if (!Settings.CanReject)
				{
					return true;
				}

				bool result = false;

				
				if (sim != null && targets != null && targets.Length != 0)
				{
					int num = PassionAutonomyTuning.CheckThreshold;

                    bool RejectedLowRel = false;
                    bool RejectedLowLibido = false;
                    bool RejectedInPublic = false;
					bool RejectedIsBlocked = false;

                    // trait based math for sim1 AKA initiator
                    // the partner sim has to 'beat' this number, so here the 'goal' is lower
                    // actually i dont think this is ever. fucking used since both initiator and partner are passed thru this?
                    //  if (sim.HasTrait(TraitNames.Inappropriate))
                    //{
                    //	num = 0;
                    //}
                    //else if (sim.HasTrait(TraitNames.PartyAnimal))
                    //{
                    //	num = 50;
                    //}
                    //else if (sim.HasTrait(TraitNames.Shy))
                    //{
                    //	num = 100;
                    //}

                    Dictionary<Sim, int> dictionary = new Dictionary<Sim, int>();
					foreach (Sim sim2 in targets)
					{
						// if theres no other sims, ignore this
						if (sim2 == null || sim2 == sim)
						{
							continue;
						}

						int num2 = 0;

						Relationship relationship = sim.GetRelationship(sim2, false);

						if (relationship != null)
						{
							// num2 is how many LTR points sim2 has with sim1
							int LTRValue = (int)relationship.LTR.Liking;

							LTRValue /= 2;

							num2 += LTRValue; 

							if (num2 >= 10)
							{
								RejectedLowRel = true;
							}

							// refactor this?
							if (relationship.AreRomantic())
							{
								if (sim.HasTrait(TraitNames.HopelessRomantic))
								{
									num2 += 15;
								}
								if (relationship.LTR.HasInteractionBit(LongTermRelationship.InteractionBits.Kissed))
								{
									num2 += 10;
								}
							}
						}

						// trait based math for sim2
						if (sim2.HasTrait(TraitNames.Flirty))
						{
							num2 += 5;
						}
						else if (sim2.HasTrait(TraitNames.Unflirty))
						{
							num2 -= 10;
						}
						if (sim2.HasTrait(TraitNames.Attractive))
						{
							num2 += 10;
						}
						if (GameUtils.IsInstalled(ProductVersion.EP1) && sim2.HasTrait(TraitNames.EyeCandy))
						{
							num2 += 20;
						}
						if (GameUtils.IsInstalled(ProductVersion.EP9) && sim2.HasTrait(TraitNames.Irresistible))
						{
							num2 += 40;
						}
						if (GameUtils.IsInstalled(ProductVersion.EP3) && sim2.HasTrait(TraitNames.MasterOfSeduction))
						{
							num2 += 100;
						}

						// how high is sim2's libido?
						// time for the worst ifelse tree evar
						BuffManager buffManager = sim2.BuffManager;

                        // 0%
                        if (buffManager.HasElement((BuffNames)2248271455579464240uL))
                        {
                            RejectedLowLibido = true;
                        }
                        // 10%
                        else if (buffManager.HasElement((BuffNames)2913570583726353694uL))
						{
							num2 += 5;
							RejectedLowLibido = true;
						}
						// 20%
						else if (buffManager.HasElement((BuffNames)2922268820215428064uL))
						{
							num2 += 10;
                            RejectedLowLibido = true;
                        }
						// 30%
						else if (buffManager.HasElement((BuffNames)3097843141287298166uL))
						{
							num2 += 15;
						}
						// 40%
						else if (buffManager.HasElement((BuffNames)8198323707617122614uL))
						{
							num2 += 20;
						}
						// 50%
						else if (buffManager.HasElement((BuffNames)8200297330989383022uL))
						{
							num2 += 25;
						}
                        // 60%
                        else if (buffManager.HasElement((BuffNames)2917472750494117670uL))
                        {
                            num2 += 30;
                        }
                        // 70%
                        else if (buffManager.HasElement((BuffNames)16251613925768384549uL))
                        {
                            num2 += 35;
                        }
                        // 80%
                        else if (buffManager.HasElement((BuffNames)14041574305464178967uL))
                        {
                            num2 += 40;
                        }
                        // 90%
                        else if (buffManager.HasElement((BuffNames)13147589483235469726uL))
                        {
                            num2 += 45;
                        }
                        // 100%
                        else if (buffManager.HasElement((BuffNames)2922253427052633003uL))
                        {
                            num2 += 50;
                        }


                        if (!sim.SimDescription.NotOpposedToRomanceWithGender(sim2.SimDescription.Gender))
						{
							num2 -= 60;
						}


						// extra modifiers for new traits

						// asexual - denies autonomy
						if (sim2.HasTrait((TraitNames)6177560411462291097uL) && GetPlayer(sim2).IsAutonomous)
						{
							num2 -= 9999;
							RejectedIsBlocked = true;

						}
                        // abstinent - denies ALL
                        if (sim2.HasTrait((TraitNames)2214287488174702228uL))
                        {
                            num2 -= 9999;
							RejectedIsBlocked = true;
                        }
                        // hypersexual - more willing
                        if (sim2.HasTrait((TraitNames)5711695705602619160uL))
                        {
                            num2 += 50;
                        }

						// modifiers based on lot type

						Lot SimCurrentLot = sim2.LotCurrent;

                        // if residential
                        if (sim2.LotCurrent.LotType == LotType.Residential)
						{
							num2 += 10;
						}
						else
						{
                            foreach (PassionAutonomyTuning.PassionLots lot in PassionAutonomyTuning.PassionLotList)
                            {
                                Lot.MetaAutonomyType LotType = (Lot.MetaAutonomyType)lot.LotTypeEnum;
                                bool LotAxis = lot.LotAxis;
                                int LotScore = lot.LotScore;

                                try
                                {
                                    if (SimCurrentLot.mMetaAutonomyType == LotType)
                                    {
                                        if (LotAxis)
                                        {
                                            num2 += LotScore;
                                        }
                                        else
                                        {
                                            num2 -= LotScore;
											RejectedInPublic = true;
                                        }
                                    }
                                }
                                catch
                                {
                                    Message("something in the lot autonomy check Broke lmao");
                                }

                            }
                        }

						if (RejectedIsBlocked == true)
						{
							GetPlayer(sim2).RejectionReason = RejectionReasons.IsBlocked;
						}
						else if (RejectedInPublic == true)
						{
							GetPlayer(sim2).RejectionReason = RejectionReasons.InPublic;
						}
						else if (RejectedLowLibido == true)
						{
							GetPlayer(sim2).RejectionReason = RejectionReasons.LowLibido;
						}
						else if (RejectedLowRel == true)
						{
							GetPlayer(sim2).RejectionReason = RejectionReasons.LowLTR;
						}

							dictionary.Add(sim2, num2);
					}
					int num3 = 0;
					if (dictionary.Count > 0)
					{
						foreach (int value in dictionary.Values)
						{
							num3 += value;
						}
						if (num3 > 0)
						{
							num3 /= dictionary.Count;
						}
					}
					num3 += Modules.GetPassionScoringModifier(sim, targets);

					if (num3 >= num)
					{
						result = true;
					}

				}
				return result;
			}
			// end if a sim will ACCEPT passion


			public void DegradeRelationship(Player partner)
			{
				UpdateRelationship(partner, Settings.RelationshipLoss);
			}

			public void ImproveRelationship(Player partner)
			{
				UpdateRelationship(partner, Settings.RelationshipGain);
			}

			public void UpdateRelationship(Player partner, float amount)
			{
				if (!IsValid || partner == null || !partner.IsValid)
				{
					return;
				}
				Relationship relationship = Relationship.Get(Actor, partner.Actor, true);
				if (relationship == null || amount == 0f)
				{
					return;
				}
				VisualEffect visualEffect = null;
				VisualEffect visualEffect2 = null;
				string text = string.Empty;
				if (amount > 8f)
				{
					text = "socialPlusPlusFx";
				}
				else if (amount > 0f)
				{
					text = "socialPlusFx";
				}
				else if (amount < -8f)
				{
					text = "socialMinusMinusFx";
				}
				else if (amount < 0f)
				{
					text = "socialMinusFx";
				}
				if (text != string.Empty)
				{
					visualEffect = VisualEffect.Create(text);
					visualEffect2 = VisualEffect.Create(text);
					if (visualEffect != null && visualEffect2 != null)
					{
						if (amount < 0f)
						{
							visualEffect.SetEffectColorScale(1f, 0f, 0f);
							visualEffect2.SetEffectColorScale(1f, 0f, 0f);
						}
						Actor.ParentHeadlineFx(visualEffect);
						partner.Actor.ParentHeadlineFx(visualEffect2);
						visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
						visualEffect2.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
					}
				}
				relationship.LTR.UpdateLiking(amount);
			}

			public void ImproveRelationships()
			{
				if (!HasPart || !(Settings.RelationshipGain > 0f))
				{
					return;
				}
				foreach (Player value in Part.Players.Values)
				{
					if (value != null && value != this && !PartnersUpdated.Contains(value) && !value.PartnersUpdated.Contains(this))
					{
						PartnersUpdated.Add(value);
						ImproveRelationship(value);
					}
				}
			}

			public void PlayRouteFailure()
			{
				if (IsValid)
				{
					ThoughtBalloonManager.BalloonData balloonData = new ThoughtBalloonManager.BalloonData("t_balloon_routefail");
					balloonData.BalloonType = ThoughtBalloonTypes.kSpeechBalloon;
					balloonData.mPriority = ThoughtBalloonPriority.High;
					balloonData.LowAxis = ThoughtBalloonAxis.kDislike;
					Actor.ThoughtBalloonManager.ShowBalloon(balloonData);
					Actor.PlaySoloAnimation(Position.Animation.Prefix.Age(Actor) + "_routeFail_standing_x");
				}
			}

			public void PlayJoinFailure(Sim unliked)
			{
				if (IsValid && unliked != null)
				{
					ThoughtBalloonManager.BalloonData balloonData = new ThoughtBalloonManager.BalloonData(unliked.GetThumbnailKey());
					balloonData.BalloonType = ThoughtBalloonTypes.kThoughtBalloon;
					balloonData.mPriority = ThoughtBalloonPriority.High;
					balloonData.LowAxis = ThoughtBalloonAxis.kDislike;
					Actor.ThoughtBalloonManager.ShowBalloon(balloonData);
					Actor.PlaySoloAnimation(Position.Animation.Prefix.Age(Actor) + "_react_view_hate_x");
				}
			}

			public List<Sim> GetAvailablePartners()
			{
				return GetAvailablePartners(Actor);
			}

			public static List<Sim> GetAvailablePartners(Sim sim)
			{
				List<Sim> list = new List<Sim>();
				if (sim != null)
				{
					foreach (Sim sim2 in sim.LotCurrent.GetSims())
					{
						if (sim2 != sim && CanPassion(sim, sim2) && !sim.LotCurrent.IsWorldLot)
						{
							list.Add(sim2);
						}
					}
				}
				return list;
			}

			public Target GetNearbySupportedTarget()
			{
				return GetTarget(GetNearbySupportedObject());
			}

			public GameObject GetNearbySupportedObject()
			{
				if (IsValid)
				{
					List<Type> randomList = new List<Type>(PassionCommon.PreferredTypes);
					for (int i = 0; i < PassionCommon.PreferredTypes.Count; i++)
					{
						Type randomObjectFromList = RandomUtil.GetRandomObjectFromList(randomList);
						GameObject[] array = Sims3.SimIFace.Queries.GetObjects(randomObjectFromList, Actor.LotCurrent.LotId, Actor.RoomId) as GameObject[];
						if (array == null)
						{
							continue;
						}
						GameObject[] array2 = array;
						foreach (GameObject gameObject in array2)
						{
							if (gameObject != null && !gameObject.HasBeenDestroyed && HasRoom(gameObject))
							{
								return gameObject;
							}
						}
					}
					GameObject[] objects = Sims3.Gameplay.Queries.GetObjects<GameObject>(Actor.LotCurrent, Actor.RoomId);
					GameObject[] array3 = objects;
					foreach (GameObject gameObject2 in array3)
					{
						if (gameObject2 != null && !gameObject2.HasBeenDestroyed)
						{
							PassionType supportedType = PassionType.GetSupportedType(gameObject2);
							if (supportedType != null && !supportedType.Is<Windows>() && !supportedType.Is<ChairDining>() && !PassionCommon.PreferredTypes.Contains(supportedType.Type) && HasRoom(gameObject2))
							{
								return gameObject2;
							}
						}
					}
				}
				return null;
			}


			// this can be considered the 'true' start of the loop i think, because
			// asking to passion leads to this section, and it runs down the list from there?
			public bool Invite(Player partner, Interaction<Sim, Sim> interaction)
			{
				if (partner != null && partner.IsValid)
				{
					if (partner.Actor.IsSleeping)
					{
						partner.Actor.InteractionQueue.CancelInteraction(partner.Actor.InteractionQueue.GetCurrentInteraction(), true);
						PassionCommon.Wait(30);
					}
					State = PassionState.None;
					Actor.SynchronizationTarget = partner.Actor;
					Actor.SynchronizationRole = Sim.SyncRole.Initiator;
					Actor.SynchronizationLevel = Sim.SyncLevel.Started;
					InteractionInstance entry = (interaction.LinkedInteractionInstance = Interactions.BeAskedToPassion.Singleton.CreateInstance(Actor, partner.Actor, new InteractionPriority(InteractionPriorityLevel.High), false, true));
					partner.Actor.InteractionQueue.Add(entry);
					if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Started, 60f) && Route(partner))
					{
						Actor.SynchronizationLevel = Sim.SyncLevel.Routed;
						if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Routed, 60f))
						{
							Actor.RouteTurnToFace(partner.Actor.Position);
							if (Settings.ActiveAlwaysAccepts || WillPassion(partner))
							{
								State = PassionState.Accept;
							}
							else
							{
								State = PassionState.Deny;
							}
							Actor.SynchronizationLevel = Sim.SyncLevel.Committed;
							if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Committed, 60f))
							{
								if (State == PassionState.Accept && partner.State == PassionState.Accept)
								{
									if (Actor.Posture.AllowsNormalSocials() && !(Actor.Posture is SwimmingInPool))
									{
										PlayInviteSuccess(partner);
									}
									NumberAccepted++;
								}
								else
								{
									if (Actor.Posture.AllowsNormalSocials() && !(Actor.Posture is SwimmingInPool))
									{
										PlayInviteFailure(partner);
									}
									DegradeRelationship(partner);

									// send message as to why the invite failed if we have its reason logged
									// make this non-global for final, its global rn for testing lol
									if (GetPlayer(partner.Actor).RejectionReason == RejectionReasons.IsBlocked)
									{
                                        PassionCommon.SimMessage(PassionCommon.Localize("Sorry! I'm... not into that kind of thing.").ToString(), partner.Actor);
                                    }
                                    else if (GetPlayer(partner.Actor).RejectionReason == RejectionReasons.InPublic)
                                    {
                                        PassionCommon.SimMessage(PassionCommon.Localize("Uh, no way. Not in public!").ToString(), partner.Actor);
                                    }
                                    else if (GetPlayer(partner.Actor).RejectionReason == RejectionReasons.LowLibido)
                                    {
                                        PassionCommon.SimMessage(PassionCommon.Localize("I'm not really in the mood for that, not right now.").ToString(), partner.Actor);
                                    }
                                    else if (GetPlayer(partner.Actor).RejectionReason == RejectionReasons.LowLTR)
                                    {
                                        PassionCommon.SimMessage(PassionCommon.Localize("Look, I really don't know you well enough to do that.").ToString(), partner.Actor);
                                    }


                                    Leave();
                                    
                                    return false;
                                }
							}
						}
					}
					if (--PartnersToCheckCount < 1)
					{
						if (DirectTargeted)
						{
							DirectStartLoop();
						}
						else if (NumberAccepted > 0)
						{
							Actor.InteractionQueue.PushAsContinuation(Interactions.RouteToPassion.Singleton.CreateInstance(partner.Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
						}
						else
						{
							Leave();
						}
					}
					return true;
				}
				return false;
			}

			public void PlayInviteSuccess(Player partner)
			{
				if (!IsValid || partner == null || !partner.IsValid)
				{
					return;
				}
				string animationName = "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_x";
				ProductVersion productVersion = ProductVersion.BaseGame;
				if (Actor.SimDescription.Teen || partner.Actor.SimDescription.Teen)
				{
					animationName = ((!RandomUtil.CoinFlip()) ? "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_x" : "a2a_soc_Neutral_RevealSecret_Friendly_Neutral_x");
				}
				else if (GameUtils.IsInstalled(ProductVersion.EP3) && Actor.HasTrait(TraitNames.MasterOfSeduction))
				{
					if (GameUtils.IsInstalled(ProductVersion.EP9))
					{
						animationName = "a2a_soc_neutral_heatOfTheMomentKiss_amorous_neutral_x";
						productVersion = ProductVersion.EP9;
					}
					else
					{
						animationName = "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_x";
					}
				}
				else if (GameUtils.IsInstalled(ProductVersion.EP9) && Actor.HasTrait(TraitNames.Irresistible))
				{
					animationName = "a2a_soc_irresistible_wink_accept_x";
					productVersion = ProductVersion.EP9;
				}
				Actor.PlaySoloAnimation(animationName, true, productVersion, true);
			}

			public void PlayInviteFailure(Player partner)
			{
				if (IsValid && partner != null && partner.IsValid)
				{
					string animationName = "a2a_soc_Amorous_flirtHitOn_Neutral_Neutral_x";
					ProductVersion productVersion = ProductVersion.BaseGame;
					Actor.PlaySoloAnimation(animationName, true, productVersion, true);
				}
			}

			public bool Respond(Player partner)
			{
				if (partner != null && partner.IsValid)
				{
					State = PassionState.None;
					Actor.SynchronizationTarget = partner.Actor;
					Actor.SynchronizationRole = Sim.SyncRole.Receiver;
					Actor.SynchronizationLevel = Sim.SyncLevel.Started;
					if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Started, 600f))
					{
						Actor.SynchronizationLevel = Sim.SyncLevel.Routed;
						if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Routed, 60f))
						{
							Actor.RouteTurnToFace(partner.Actor.Position);
							if (WillPassion(partner))
							{
								State = PassionState.Accept;
							}
							else
							{
								State = PassionState.Deny;
							}
							Actor.SynchronizationLevel = Sim.SyncLevel.Committed;
							if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Committed, 60f))
							{
								if (State == PassionState.Accept && partner.State == PassionState.Accept)
								{
									if (Actor.Posture.AllowsNormalSocials() && !(Actor.Posture is SwimmingInPool))
									{
										PlayRespondSuccess(partner);
									}
									Join(partner.Part);
									if (partner.DirectTargeted)
									{
										DirectStartLoop();
									}
									else
									{
										Actor.InteractionQueue.PushAsContinuation(Interactions.RouteToPassion.Singleton.CreateInstance(partner.Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
									}
									return true;
								}
								if (Actor.Posture.AllowsNormalSocials() && !(Actor.Posture is SwimmingInPool))
								{
									PlayRespondFailure(partner);
								}
								Reset();
							}
						}
					}
				}
				return false;
			}

			public void PlayRespondSuccess(Player partner)
			{
				if (!IsValid || partner == null || !partner.IsValid)
				{
					return;
				}
				string animationName = "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_y";
				ProductVersion productVersion = ProductVersion.BaseGame;
				if (Actor.SimDescription.Teen || partner.Actor.SimDescription.Teen)
				{
					animationName = "a2a_soc_Neutral_RevealSecret_Friendly_Neutral_y";
				}
				else if (GameUtils.IsInstalled(ProductVersion.EP3) && partner.Actor.HasTrait(TraitNames.MasterOfSeduction))
				{
					if (GameUtils.IsInstalled(ProductVersion.EP9))
					{
						animationName = "a2a_soc_neutral_heatOfTheMomentKiss_amorous_neutral_y";
						productVersion = ProductVersion.EP9;
					}
					else
					{
						animationName = "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_y";
					}
				}
				else if (GameUtils.IsInstalled(ProductVersion.EP9) && partner.Actor.HasTrait(TraitNames.Irresistible))
				{
					animationName = "a2a_soc_irresistible_wink_accept_y";
					productVersion = ProductVersion.EP9;
				}
				Actor.PlaySoloAnimation(animationName, true, productVersion, true);
			}

			public void PlayRespondFailure(Player partner)
			{
				if (IsValid && partner != null && partner.IsValid)
				{
					string animationName = "a2a_soc_Amorous_flirtHitOn_Neutral_Neutral_y";
					ProductVersion productVersion = ProductVersion.BaseGame;
					Actor.PlaySoloAnimation(animationName, true, productVersion, true);
				}
			}

			public float GetDistanceTo(IGameObject obj)
			{
				if (IsValid && obj != null)
				{
					return Actor.GetDistanceToObject(obj);
				}
				return float.PositiveInfinity;
			}

			public float GetDistanceTo(Vector3 point)
			{
				return PassionCommon.GetDistanceBetween(Actor, point);
			}

			public bool Route()
			{
				if (RouteToTarget())
				{
					Actor.InteractionQueue.PushAsContinuation(Interactions.BeginPassion.Singleton.CreateInstance(Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
					return true;
				}
				return Leave();
			}

			public bool Route(Player partner, float range)
			{
				if (partner != null && partner.IsValid)
				{
					Route.RouteOption[] additionalRouteOptions = new Route.RouteOption[1] { Sims3.SimIFace.Route.RouteOption.MakeDynamicObjectAdjustments };
					try
					{
						if (Actor.RoutingComponent.RouteToObjectRadialRange(partner.Actor, 1.5f, range, additionalRouteOptions))
						{
							return true;
						}
					}
					catch
					{
					}
					return Actor.RoutingComponent.RouteToObjectRadialRange(partner.Actor, 1.5f, range);
				}
				return false;
			}

			public bool Route(Player partner)
			{
				if (IsValid && partner != null && partner.IsValid)
				{
					State = PassionState.Routing;
					if (Actor.RouteToDynamicObjectRadius(partner.Actor, Actor.GetSocialRadiusWith(partner.Actor), null))
					{
						return true;
					}
				}
				return false;
			}

			public bool RouteToTarget()
			{
				if (IsValid && HasPart && Part.HasTarget)
				{
					State = PassionState.Routing;
					if (Part.Target.HasObject)
					{
						if (Part.Type.Is("CrossWood"))
						{
							if (Actor.RouteToPointRadialRange(Part.Target.Object.GetPositionOfSlot(Door.RoutingSlots.Door0_Front), 0.1f, 1.5f))
							{
								return true;
							}
							if (Actor.RouteToPointRadialRange(Part.Target.Object.GetPositionOfSlot(Door.RoutingSlots.Door0_Front), 0.1f, 3f))
							{
								return true;
							}
						}
						if (Part.Type.Is<PoolLadder>())
						{
							if (Actor.RouteToPointRadialRange(Part.Target.Object.GetPositionOfSlot((Slot)1986264130u), 0.1f, 1.5f))
							{
								return true;
							}
							if (Actor.RouteToPointRadialRange(Part.Target.Object.GetPositionOfSlot((Slot)1986264130u), 0.1f, 3f))
							{
								return true;
							}
						}
						Route.RouteOption[] additionalRouteOptions = new Route.RouteOption[1] { Sims3.SimIFace.Route.RouteOption.MakeDynamicObjectAdjustments };
						try
						{
							if (Actor.RoutingComponent.RouteToObjectRadialRange(Part.Target.Object, 0.5f, 2.5f, additionalRouteOptions))
							{
								return true;
							}
						}
						catch
						{
						}
						try
						{
							if (Actor.RoutingComponent.RouteToObjectRadialRange(Part.Target.Object, 0.5f, 5f, additionalRouteOptions))
							{
								return true;
							}
						}
						catch
						{
						}
					}
					try
					{
						if (Actor.RouteToPointRadialRange(Part.Target.Location, 0.5f, 2.5f))
						{
							return true;
						}
					}
					catch
					{
					}
					try
					{
						if (Actor.RouteToPointRadialRange(Part.Target.Location, 0.5f, 5f))
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

			public bool RouteInWater(Vector3 point)
			{
				return RouteInWater(point, 1f, 2f, 1.4f);
			}

			public bool RouteInWater(Vector3 point, float min, float max, float spacing)
			{
				if (point != Vector3.Invalid && point != Vector3.Empty)
				{
					RadialRangeDestination radialRangeDestination = new RadialRangeDestination();
					radialRangeDestination.mCenterPoint = point;
					radialRangeDestination.mfMinRadius = min;
					radialRangeDestination.mfMaxRadius = max;
					radialRangeDestination.mfPreferredSpacing = spacing;
					radialRangeDestination.ScoreFunctionWeights[(uint)(UIntPtr)2uL] = 1f;
					Route route = Actor.CreateRoute();
					route.SetOption(Sims3.SimIFace.Route.RouteOption.EnableWaterPlanning, true);
					route.SetOption(Sims3.SimIFace.Route.RouteOption.DoNotEmitDegenerateRoutesForRadialRangeGoals, true);
					route.SetOption(Sims3.SimIFace.Route.RouteOption.DoLineOfSightCheckUserOverride, false);
					route.SetOption(Sims3.SimIFace.Route.RouteOption.CheckForFootprintsNearGoals, false);
					route.SetOption2(Sims3.SimIFace.Route.RouteOption2.EnablePlanningAsBoat, false);
					route.AddDestination(radialRangeDestination);
					if (route.Plan().Succeeded() && Actor.DoRoute(route))
					{
						return true;
					}
				}
				return false;
			}

			public bool Join(Target target)
			{
				if (target != null && target.IsValid && !target.IsOccupied)
				{
					return target.Add(this);
				}
				return false;
			}

			public bool Join(Part part)
			{
				if (part != null && part.HasRoom)
				{
					return part.Add(this);
				}
				return false;
			}

			public void UpdateLocation()
			{
				if (Actor != null && Part != null)
				{
					Forward = Part.Forward;
					Vector3 location = Part.Location;
					location.y += HeightModifier;
					Location = location;
					IsInPlace = true;
				}
			}

			public void SetPosture()
			{
				if (IsValid)
				{
					if (HasPart && Part.HasTarget && Part.Target.HasObject)
					{
						PassionPosture.Set(Actor, Part.Target.Object);
					}
					else
					{
						PassionPosture.Set(Actor);
					}
				}
			}

			public void ClearPosture()
			{
				if (IsValid)
				{
					PassionPosture.Revert(Actor);
				}
			}

			public void ChangePosition()
			{
				if (HasPart)
				{
					Part.ChangePosition();
				}
			}

			public void Animate()
			{
				CanAnimate = false;
				State = PassionState.Animating;
				Animate(BufferedAnimation);
				if (State != PassionState.Leaving && State != PassionState.Stopping)
				{
					State = PassionState.Ready;
				}
			}

			public void Animate(string clip)
			{
				Animate(clip, ProductVersion.Undefined);
			}

			public void Animate(string clip, ProductVersion version)
			{
				if (!IsValid)
				{
					return;
				}
				try
				{
					StateMachineClient stateMachineClient = StateMachineClient.Acquire(Actor, "passion_generic");
					if (Part.Target.Object != null && Settings.ObjectAnimation != null)
					{
						stateMachineClient.SetParameter("ObjectAnimationName", Settings.ObjectAnimation, version);
						stateMachineClient.SetActor("obj", Part.Target.Object);
					}
					stateMachineClient.SetParameter("AnimationName", clip, version);
					stateMachineClient.SetActor(PassionCommon.X, Actor);
					stateMachineClient.EnterState(PassionCommon.X, "Enter");
					stateMachineClient.RequestState(PassionCommon.X, "Play Animation");
					stateMachineClient.RequestState(PassionCommon.X, "Exit");
				}
				catch
				{
				}
			}

			// motive updates while passioning
			public void BeginMotiveUpdates()
			{
				if (!IsValid || !HasPart)
				{
					return;
				}
				InteractionInstance currentInteraction = Actor.CurrentInteraction;
				if (currentInteraction == null)
				{
					return;
				}
				if (Settings.Motives == PassionMotives.MaxAll)
				{
					Actor.Motives.MaxEverything();
				}
				else
				{
					if (Settings.Motives == PassionMotives.EADefault)
					{
						return;
					}
					bool flag = Settings.Motives == PassionMotives.NoDecay || Settings.Motives == PassionMotives.Freeze;
					float num = 0f;
					float num2 = 0f;
					float num3 = 0f;
					float num4 = 0f;
					float num5 = 0f;
					float num6 = 0f;
					if (Settings.Motives != PassionMotives.Freeze)
					{
						num4 = 0f;
						num3 = 0f;
						num6 = 0f;
						num = 100f;
						if (Settings.Motives != PassionMotives.NoDecay)
						{
							num2 = -40f;
							num5 = -20f;
						}
						if (Part.HasTarget)
						{
							Target target = Part.Target;
							if (target.HasObject)
							{
								if (target.Object is IShowerable)
								{
									num2 = 600f;
								}
								else if (target.Object is IHotTub || target.Object is IBathtub)
								{
									num2 = 400f;
								}
							}
						}
						if (HadPartner)
						{
							num = 300f;
							num6 = 300f;
						}
					}
					// refactor this maybe because idfk whats going on
					if (num != 0f || flag)
					{
						currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Fun, num, false, num, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
					}
					if (num2 != 0f || flag)
					{
						currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Hygiene, num2, false, num2, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
					}
					if (num3 != 0f || flag)
					{
						currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Bladder, num3, false, num3, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
					}
					if (num4 != 0f || flag)
					{
						currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Hunger, num4, false, num4, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
					}
					if (num5 != 0f || flag)
					{
						currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Energy, num5, false, num5, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
					}
					if (num6 != 0f || flag)
					{
						currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Social, num6, false, num6, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
					}
				}
			}

			public void EndMotiveUpdates()
			{
				if (IsValid)
				{
					InteractionInstance currentInteraction = Actor.CurrentInteraction;
					if (currentInteraction != null && Settings.Motives != 0 && Settings.Motives != PassionMotives.MaxAll)
					{
						currentInteraction.EndCommodityUpdates(true);
					}
				}
			}

			public void RecalculateMotiveUpdates()
			{
				if (IsValid && IsActive)
				{
					EndMotiveUpdates();
					BeginMotiveUpdates();
					Modules.LoopProcessing(Actor);
				}
			}

			// broadcast when people start passion
			// DONE -- REWRITE THE DIALOG FOR THE LOVE OF FUCKING GOD
			public void BroadcastPassionStart()
			{
				try
				{
					if (!IsValid || !IsActive || Actor.LotCurrent == null || Actor.LotCurrent.IsWorldLot)
					{
						return;
					}
					foreach (Sim allActor in Actor.LotCurrent.GetAllActors())
					{
						if (allActor == null || allActor.RoomId != Actor.RoomId || (allActor.CurrentInteraction != null && allActor.CurrentInteraction.Target is RabbitHole) || allActor.SimDescription.ChildOrBelow || allActor.SimDescription.IsPet || !CanPassion(allActor))
						{
							continue;
						}
						Player player = GetPlayer(allActor);
						if (!player.IsValid || player.IsActive || player.State != 0)
						{
							continue;
						}
						// START REFACTOR

						// if witness is party animal
						if (player.Actor.HasTrait(TraitNames.PartyAnimal))
						{
							player.Actor.PlayReaction(ReactionTypes.PumpFist, Actor, ReactionSpeed.Immediate);
							player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
							// set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
							if (Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
							{
								PassionCommon.SimMessage(PassionCommon.Localize("WOOOOOO YEAH!! You guys know how to PARTY HARD! LITERALLY!").ToString(), player.Actor);
							}
						}
						// if witness is flirty
						else if (player.Actor.HasTrait(TraitNames.Flirty))
						{
							player.Actor.PlayReaction(ReactionTypes.Giggle, Actor, ReactionSpeed.Immediate);
							player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
							// set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
							if (Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
							{
								PassionCommon.SimMessage(PassionCommon.Localize("Ooh la la! Who could deny such a show?").ToString(), player.Actor);
							}
						}
						// if witness is daredevil
						else if (player.Actor.HasTrait(TraitNames.Daredevil))
						{
							player.Actor.PlayReaction(ReactionTypes.PumpFist, Actor, ReactionSpeed.Immediate);
							player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
							// set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
							if (Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
							{
								PassionCommon.SimMessage(PassionCommon.Localize("Livin' it on the edge, huh? Cheers, bro!").ToString(), player.Actor);
							}
						}
						// if witness is diva
						else if (GameUtils.IsInstalled(ProductVersion.EP6) && player.Actor.HasTrait(TraitNames.Diva))
						{
							player.Actor.PlayReaction(ReactionTypes.Fascinated, Actor, ReactionSpeed.Immediate);
							player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
							if (Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
							{
								PassionCommon.SimMessage(PassionCommon.Localize("Wow, what a bold move...! I'm so intruiged...!").ToString(), player.Actor);
							}
						}
						// general reactions
						else if (Settings.AutonomyChance > 0 && (Settings.AutonomyActive || !player.Actor.LotHome.IsActive) && (Settings.AutonomyPublic || player.Actor.LotCurrent.LotType == LotType.Residential) && RandomUtil.GetInt(0, 99) < Settings.AutonomyChance)
						{
							player.IsAutonomous = true;
							player.Actor.InteractionQueue.AddNext(Interactions.WatchLoop.Singleton.CreateInstance(Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
							// add coinflip to reduce spam in crowded areas
							if (Settings.AutonomyNotify && RandomUtil.CoinFlip())
							{
								if (PassionCommon.RandomReactionNeg == ReactionTypes.Awkward)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Um... could you people get a room?").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.Embarrassed)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Stop it!! That's too freaky for here!").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.Boo)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Boo! Boooooo!! Cut it out!").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.Inappropriate)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Ergh, no thanks. Do that somewhere else.").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.FreakOut)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Is this seriously happening?! Here? In PUBLIC?!").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.ThrowUp)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("What the hell?! I'm gonna be sick...").ToString(), player.Actor);
								}
							}
							if (player.Actor.InteractionQueue.Count > 1)
							{
								player.Actor.InteractionQueue.CancelInteraction(player.Actor.CurrentInteraction, false);
							}
						}

						// END REFACTOR
					}
				}
				catch
				{
				}
			}

			// ????? this is literally the same as above. whats going on here
			public void BroadcastPassionResult()
			{
				try
				{
					if (!IsValid || !IsActive || Actor.LotCurrent == null || Actor.LotCurrent.IsWorldLot)
					{
						return;
					}
					foreach (Sim allActor in Actor.LotCurrent.GetAllActors())
					{
						if (allActor == null || allActor.RoomId != Actor.RoomId || (allActor.CurrentInteraction != null && allActor.CurrentInteraction.Target is RabbitHole) || !CanPassion(allActor))
						{
							continue;
						}
						Player player = GetPlayer(allActor);
						if (!player.IsValid || player.IsActive || player.State != 0)
						{
							continue;
						}
						// START REFACTOR

						// if witness is party animal
						if (player.Actor.HasTrait(TraitNames.PartyAnimal))
						{
							player.Actor.PlayReaction(ReactionTypes.PumpFist, Actor, ReactionSpeed.Immediate);
							player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
							// set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
							if (Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
							{
								PassionCommon.SimMessage(PassionCommon.Localize("WOOOOOO YEAH!! You guys know how to PARTY HARD! LITERALLY!").ToString(), player.Actor);
							}
						}
						// if witness is flirty
						else if (player.Actor.HasTrait(TraitNames.Flirty))
						{
							player.Actor.PlayReaction(ReactionTypes.Giggle, Actor, ReactionSpeed.Immediate);
							player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
							// set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
							if (Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
							{
								PassionCommon.SimMessage(PassionCommon.Localize("Ooh la la! Who could deny such a show?").ToString(), player.Actor);
							}
						}
						// if witness is daredevil
						else if (player.Actor.HasTrait(TraitNames.Daredevil))
						{
							player.Actor.PlayReaction(ReactionTypes.PumpFist, Actor, ReactionSpeed.Immediate);
							player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
							// set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
							if (Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
							{
								PassionCommon.SimMessage(PassionCommon.Localize("Livin' it on the edge, huh? Cheers, bro!").ToString(), player.Actor);
							}
						}
						// if witness is diva
						else if (GameUtils.IsInstalled(ProductVersion.EP6) && player.Actor.HasTrait(TraitNames.Diva))
						{
							player.Actor.PlayReaction(ReactionTypes.Fascinated, Actor, ReactionSpeed.Immediate);
							player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
							if (Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
							{
								PassionCommon.SimMessage(PassionCommon.Localize("Wow, what a bold move...! I'm so intruiged...!").ToString(), player.Actor);
							}
						}
						// general reactions
						else if (Settings.AutonomyChance > 0 && (Settings.AutonomyActive || !player.Actor.LotHome.IsActive) && (Settings.AutonomyPublic || player.Actor.LotCurrent.LotType == LotType.Residential) && RandomUtil.GetInt(0, 99) < Settings.AutonomyChance)
						{
							player.IsAutonomous = true;
							player.Actor.InteractionQueue.AddNext(Interactions.WatchLoop.Singleton.CreateInstance(Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
							// add coinflip to reduce spam in crowded areas
							if (Settings.AutonomyNotify && RandomUtil.CoinFlip())
							{
								if (PassionCommon.RandomReactionNeg == ReactionTypes.Awkward)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Um... could you people get a room?").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.Embarrassed)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Stop it!! That's too freaky for here!").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.Boo)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Boo! Boooooo!! Cut it out!").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.Inappropriate)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Ergh, no thanks. Do that somewhere else...").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.FreakOut)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Is this seriously happening?! Here? In PUBLIC?!").ToString(), player.Actor);
								}
								if (PassionCommon.RandomReactionNeg == ReactionTypes.ThrowUp)
								{
									PassionCommon.SimMessage(PassionCommon.Localize("What the hell?! I'm gonna be sick...").ToString(), player.Actor);
								}
							}
							if (player.Actor.InteractionQueue.Count > 1)
							{
								player.Actor.InteractionQueue.CancelInteraction(player.Actor.CurrentInteraction, false);
							}
						}

						// END REFACTOR
					}
				}
				catch
				{
				}
			}

			public void StartJealousyBroadcast()
			{
				if (JealousyBroadcaster == null)
				{
					JealousyBroadcaster = new ReactionBroadcaster(Actor, Conversation.ReactToSocialParams, Autonomy.JealousyCheck);
				}
			}

			public void StopJealousyBroadcast()
			{
				if (JealousyBroadcaster != null)
				{
					JealousyBroadcaster.Dispose();
					JealousyBroadcaster = null;
				}
			}

			public bool ForceStartLoopImmediate()
			{
				if (IsValid)
				{
					State = PassionState.Routing;
					Actor.SetIsSleeping(false);
					Actor.InteractionQueue.CancelAllInteractions();
					Actor.InteractionQueue.AddNext(Interactions.BeginPassion.Singleton.CreateInstance(Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
					return true;
				}
				return false;
			}

			public bool ForceStartLoop()
			{
				if (IsValid)
				{
					Actor.SetIsSleeping(false);
					Actor.InteractionQueue.CancelAllInteractions();
					State = PassionState.Routing;
					return StartLoop();
				}
				return false;
			}

			public bool DirectStartLoop()
			{
				State = PassionState.Routing;
				return StartLoop();
			}


			// START THE PASSION LOOP
            public bool StartLoop()
			{
				if (IsValid && HasPart)
				{
					if (Part.Count >= Part.MaxSims)
					{
						Leave();
					}
					ExitPoint = new Vector3(Actor.Position);
					if (!ActiveJoin)
					{
						SaveOutfit();
						if (Settings.NakedShower && HasPart && Part.HasTarget && Part.Target.HasObject && (Part.Target.Object is IShowerable || Part.Target.Object is IBathtub))
						{
							Outfit = OutfitCategories.Naked;
						}
						else if (!UsePreferredOutfit())
						{
							if (Settings.Outfit == OutfitCategories.Naked && Outfit != OutfitCategories.Naked)
							{
								Outfit = OutfitCategories.Naked;
							}
							else if (Settings.Outfit == OutfitCategories.Sleepwear && Outfit != OutfitCategories.Sleepwear)
							{
								Outfit = OutfitCategories.Sleepwear;
							}
						}

					}
					else
					{
						ActiveJoin = false;
					}
					SetPosture();
					Actor.LookAtManager.DisableLookAts();
					Modules.PreProcessing(Actor);
					State = PassionState.Ready;
					PositionIndex = Part.Players.Count;
					Part.CurrentPositionInvalidate();
					Part.StartVisualEffects();
					Part.StartSoundEffects();
					Actor.InteractionQueue.PushAsContinuation(Interactions.PassionLoop.Singleton.CreateInstance(Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
					return true;
				}
				return false;
			}

			// animations for objects
			public string Animation2Obj(string anim)
			{
				Settings.ObjectAnimation = null;
				if (anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutramakout_01") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1"))
				{
					Settings.ObjectAnimation = "a_L666_carsports3";
				}
				else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_01") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1") && !Part.Type.Is("BoatSpeedBoat"))
				{
					Settings.ObjectAnimation = "a_L666_carsports4";
				}
				else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_02") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1") && !Part.Type.Is("BoatSpeedBoat"))
				{
					Settings.ObjectAnimation = "a_L666_carsports";
				}
				else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_03") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1") && !Part.Type.Is("BoatSpeedBoat"))
				{
					Settings.ObjectAnimation = "a_L666_carsports2";
				}
				else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_threesomecarfmf_01o") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1"))
				{
					Settings.ObjectAnimation = "a_L666_threesomecarfmfcaranime";
				}
				else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_threesomecarfmf_02o") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1"))
				{
					Settings.ObjectAnimation = "a_L666_threesomecarfmfcaranime2";
				}
				else if (anim.Contains("KW_Amra72_Animations.a2a_(Am)Car_Blowjob_01") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1") && !Part.Type.Is("BoatSpeedBoat"))
				{
					Settings.ObjectAnimation = "a_L666_carsports4";
				}
				else if (!anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutramakout_01") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_01") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_02") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_03") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_threesomecarfmf_01o") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_threesomecarfmf_02o") && !anim.Contains("KW_Amra72_Animations.a2a_(Am)Car_Blowjob_01") && Part.Position.ObjectAnimation != null)
				{
					Settings.ObjectAnimation = Part.Position.ObjectAnimation;
				}
				return Settings.ObjectAnimation;
			}

			// THE BIG BOY, WHERE IT ALL HAPPENS
			public bool DoLoop()
			{
				OutfitCategories previousOutfitCategory = PreviousOutfitCategory;
				int previousOutfitIndex = PreviousOutfitIndex;
				string text = null;
				string text2 = null;
				Settings.RemoveCondom = false;
				try
				{
					ResetAnimationObject(Part.Target.Object);
					if (Part.Target.ObjectType.Name.ToString() == "Sybian")
					{
						Part.Target.Object.RemoveInteractionByType(AskToUseSybian.Singleton);
						Part.Target.Object.RemoveInteractionByType(Interactions.UseObjectForPassion.Singleton);
						Part.Target.Object.RemoveInteractionByType(Interactions.ResetMe.Singleton);
						Part.Target.Object.RemoveInteractionByType(Interactions.ResetMeActive.Singleton);
						Part.Target.Object.RemoveInteractionByType(PutInInventory.Singleton);
					}
				}
				catch
				{
				}
				if (IsValid)
				{
					IsActive = true;
                    try
					{
						if (Actor.SimDescription.Teen)
						{
							RefreshHeightModifier(Actor);
						}
					}
					catch
					{
					}
					SimDescription simDescription = Actor.SimDescription;

					//
					// check to see what junk a sim has
					// im sorry if any competent programmers see this
					// edit: is it less scary now. please clap
					//

					
                   foreach (PassionBody.BodyShop body in PassionBody.coolbodyshop)
					{
                        string PartName = body.Name;
                        string PartType = body.GenitalType;
                        string PartBase = body.BaseCASP;
                        string PartErect = body.ErectSIMO;

                        if (simDescription.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString(PartBase)) != null)
						{
                           // PassionCommon.SystemMessage("WE FOUND A MATCH BESTIES!!!!!\n" + PartName);
							GetPlayer(Actor).SimGenitalType = PartType;
                            GetPlayer(Actor).SimJunkBaseCASP = PartBase;
                            GetPlayer(Actor).SimErectSIMO = PartErect;
                        }

                    }


					SimDescription simDescription2 = Actor.SimDescription;
					SimDescription simDescription3 = Actor.SimDescription;
					SimDescription simDescription4 = Actor.SimDescription;
					SimDescription simDescription5 = Actor.SimDescription;
                    SimDescription simDescription6 = Actor.SimDescription;
                    SimDescription simDescription7 = Actor.SimDescription;
                    SimDescription simDescription8 = Actor.SimDescription;
                    SimDescription simDescription9 = Actor.SimDescription;
                    SimDescription simDescription10 = Actor.SimDescription;
                    SimDescription simDescription11 = Actor.SimDescription;
                    SimDescription simDescription12 = Actor.SimDescription;
                    SimDescription simDescription13 = Actor.SimDescription;
                    SimDescription simDescription14 = Actor.SimDescription;
                    SimDescription simDescription15 = Actor.SimDescription;
                    SimDescription simDescription16 = Actor.SimDescription;
                    SimDescription simDescription17 = Actor.SimDescription;
                    SimDescription simDescription18 = Actor.SimDescription;
                    SimDescription simDescription19 = Actor.SimDescription;
                    SimDescription simDescription20 = Actor.SimDescription;

                    StartTime = SimClock.CurrentTicks;
					Modules.LoopProcessing(Actor);
					if (Settings.BroadCasterEnable && IsInitiator)
					{
						BroadcastPassionStart();
					}
					if (Settings.BroadCasterEnable)
					{
						BroadcastPassionResult();
					}
					while (IsValid && HasPart)
					{
						Actor.AddInteraction(CumOnFace.Singleton, true);
						Actor.AddInteraction(CumOnTits.Singleton, true);
						Actor.AddInteraction(CumOnButt.Singleton, true);
						Actor.AddInteraction(CumOnBelly.Singleton, true);
						Actor.AddInteraction(CumOnRightHand.Singleton, true);
						Actor.AddInteraction(CumOnLeftHand.Singleton, true);
						Actor.AddInteraction(CumOnRightFoot.Singleton, true);
						Actor.AddInteraction(CumOnLeftFoot.Singleton, true);
						Actor.AddInteraction(CumOnThigh.Singleton, true);
						CumInteractions = true;
						try
						{
							// get the animation for the object (mostly cars)
							Animation2Obj(Part.Position.Name.ToString());
						}
						catch
						{
						}
						try
						{
							foreach (Sim allActor in Actor.LotCurrent.GetAllActors())
							{
								Player player = GetPlayer(allActor);
								if (allActor != null && allActor.RoomId == Actor.RoomId && !Actor.LotCurrent.IsWorldLot && !allActor.SimDescription.IsServicePerson && allActor != Actor && (allActor.SimDescription.ChildOrBelow || allActor.SimDescription.IsPet || (allActor != Actor && allActor.SimDescription.Elder && Settings.AutonomyChance > 0)))
								{
									if (allActor.InteractionQueue.Count <= 1 && Settings.ChildrenOut && allActor.SimDescription.ChildOrBelow)
									{
										allActor.PlayReaction(ReactionTypes.Embarrassed, Actor, ReactionSpeed.Immediate);
										allActor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
									}
									else if (allActor.InteractionQueue.Count <= 1 && Settings.PetsOut && allActor.SimDescription.IsPet)
									{
										allActor.PlayReaction(ReactionTypes.Embarrassed, Actor, ReactionSpeed.Immediate);
										allActor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
									}
									else if (allActor.InteractionQueue.Count <= 1 && Settings.EldersOut && allActor.SimDescription.Elder)
									{
										allActor.PlayReaction(ReactionTypes.Embarrassed, Actor, ReactionSpeed.Immediate);
										allActor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
									}
								}
							}
						}
						catch
						{
						}
						if (CanAnimate)
						{
							text = Part.Position.Name.ToString();
							Animate();
						}
						if (text != text2 && HeldItem != null)
						{
							HeldItem.Release();
							HeldItem = null;
						}
						if (ShouldStop)
						{
							if (HeldItem != null)
							{
								HeldItem.Release();
								HeldItem = null;
							}
							Settings.PassionFuckSession = false;
							Settings.CondomIsBroken = false;
							Settings.RemoveCondom = true;
							try
							{
								// penis removal if the sim should stop
								if (GetPlayer(Actor).SimGenitalType == "penis" || GetPlayer(Actor).SimGenitalType == "both")
								{
                                    SwitchToPeener(Actor, false);
                                }
								else if (GetPlayer(Partner.Actor).SimGenitalType == "penis" || GetPlayer(Partner.Actor).SimGenitalType == "both")
                                {
									SwitchToPeener(Partner.Actor, false);
                                }

                                // remove condom
                                if (GetPlayer(Actor).SimGenitalType == "penis" || GetPlayer(Actor).SimGenitalType == "both")
                                {
                                    WearCondom(Actor, false);
                                }
                                else if (GetPlayer(Partner.Actor).SimGenitalType == "penis" || GetPlayer(Partner.Actor).SimGenitalType == "both")
                                {
                                    WearCondom(Partner.Actor, false);
                                }

                            }
							catch
							{
							}
							previousOutfitCategory = PreviousOutfitCategory;
							previousOutfitIndex = PreviousOutfitIndex;
							try
							{
								// remove strap
								if (GetPlayer(Actor).SimGenitalType == "vagina" || GetPlayer(Actor).SimGenitalType == "neither")
                                {
									SwitchToStrapon(Actor, false);
								}
								else if (GetPlayer(Actor).SimGenitalType == "vagina" || GetPlayer(Actor).SimGenitalType == "neither")
                                {
									SwitchToStrapon(Partner.Actor, false);
								}

                            }
							catch
							{
							}
							try
							{
								CumInteractions = false;
								Actor.RemoveInteractionByType(CumOnFace.Singleton);
								Actor.RemoveInteractionByType(CumOnTits.Singleton);
								Actor.RemoveInteractionByType(CumOnButt.Singleton);
								Actor.RemoveInteractionByType(CumOnBelly.Singleton);
								Actor.RemoveInteractionByType(CumOnRightHand.Singleton);
								Actor.RemoveInteractionByType(CumOnLeftHand.Singleton);
								Actor.RemoveInteractionByType(CumOnRightFoot.Singleton);
								Actor.RemoveInteractionByType(CumOnLeftFoot.Singleton);
								Actor.RemoveInteractionByType(CumOnThigh.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnFace.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnTits.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnButt.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnBelly.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnRightHand.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnLeftHand.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnRightFoot.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnLeftFoot.Singleton);
								Partner.Actor.RemoveInteractionByType(CumOnThigh.Singleton);
							}
							catch
							{
							}
							try
							{
								CumInteractions = false;
								foreach (Sim allActor2 in Actor.LotCurrent.GetAllActors())
								{
									Player player2 = GetPlayer(allActor2);
									if ((allActor2 != null && allActor2.RoomId == Actor.RoomId) || (allActor2 != null && allActor2.RoomId == Partner.Actor.RoomId))
									{
										allActor2.RemoveInteractionByType(CumOnFace.Singleton);
										allActor2.RemoveInteractionByType(CumOnTits.Singleton);
										allActor2.RemoveInteractionByType(CumOnButt.Singleton);
										allActor2.RemoveInteractionByType(CumOnBelly.Singleton);
										allActor2.RemoveInteractionByType(CumOnRightHand.Singleton);
										allActor2.RemoveInteractionByType(CumOnLeftHand.Singleton);
										allActor2.RemoveInteractionByType(CumOnRightFoot.Singleton);
										allActor2.RemoveInteractionByType(CumOnLeftFoot.Singleton);
										allActor2.RemoveInteractionByType(CumOnThigh.Singleton);
									}
								}
							}
							catch
							{
							}
							try
							{
								if (Part.Target.ObjectType.Name.ToString() == "Sybian")
								{
									Part.Target.Object.AddInteraction(AskToUseSybian.Singleton, true);
									Part.Target.Object.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
									Part.Target.Object.AddInteraction(Interactions.ResetMe.Singleton, true);
									Part.Target.Object.AddInteraction(Interactions.ResetMeActive.Singleton, true);
									Part.Target.Object.AddInteraction(PutInInventory.Singleton, true);
								}
							}
							catch
							{
							}
							break;
						}
						List<Player> ready;
						if (IsInitiator && Part.CanAnimate(out ready) && Part.CheckSequence())
						{
							try
							{
								if (SwitchPlayerActor != null && SwitchPlayerActor == Actor && SwitchPlayerPartner != null && SwitchPlayerPartner == Partner.Actor)
								{
									Player player3 = GetPlayer(Actor);
									Player player4 = GetPlayer(Partner.Actor);
									player3.Switch(player4);
								}
							}
							catch
							{
							}
							if (Part.PositionChanged)
							{
								if (Part.Position == null)
								{
									break;
								}
								text2 = Part.Position.Name.ToString();
								if (HeldItem != null)
								{
									HeldItem.Release();
									HeldItem = null;
								}
								if (Part.Position != null)
								{
									Part.Position.GiveHeldItems(ready);
								}
							}
							else if (!Part.PositionChanged)
							{
								if (HeldItem != null)
								{
									HeldItem.Release();
									HeldItem = null;
								}
								if (Part.Position != null)
								{
									Part.Position.GiveHeldItems(ready);
								}
							}
							if (Part.Players.Count != Part.Position.MaxSims && Part.Position.MaxSims.ToString() != null && Part.Position.MaxSims == 0)
							{
								Part.Position.MaxSims = 1;
							}
							else
							{
								if (Part.Position.MaxSims.ToString() == null || Part.Position.MaxSims == 0 || Part.Position == null)
								{
									break;
								}
								// if its a masturbation action, ie one sim with one max sim
								if (Part.Players.Count == 1 && Part.Position.MaxSims == 1)
								{
									Settings.CondomIsBroken = true;
									Settings.PassionFuckSession = true;
									try
									{
										
										//
										// if strapon IS meant to be used, sim has vagina or null
										if ((GetPlayer(Actor).SimGenitalType == "vagina" || GetPlayer(Actor).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
										{
											SwitchToStrapon(Actor, true);
										}
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(Actor).SimGenitalType == "vagina" || GetPlayer(Actor).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
										{
											SwitchToStrapon(Actor, false);
										}
										//end branch
										//

                                        // if actor has a penis
                                        if (GetPlayer(Actor).SimGenitalType == "penis" || GetPlayer(Actor).SimGenitalType == "both")
										{
											SwitchToPeener(Actor, true);
                                        }
										// end branch
										//

									}
									catch
									{
									}
								}

								// ELSE if theres 2 sims, with 2 max sims
								else if (Part.Players.Count == 2 && Part.Position.MaxSims == 2)
								{
									Sim sim = null;
									Sim sim2 = null;
									try
									{
										foreach (Player item in ready)
										{
											if (item.PositionIndex == 1)
											{
												sim = item.Actor;
												continue;
											}
											if (item.PositionIndex == 2)
											{
												sim2 = item.Actor;
												continue;
											}
											break;
										}
									}
									catch
									{
									}
									simDescription3 = sim.SimDescription;
									simDescription4 = sim2.SimDescription;
									try
									{
                                        // do i really have to repeat this code for every fucking participant. what the hell

                                        // SIM 1 STRAP START
                                        if ((GetPlayer(sim).SimGenitalType == "vagina" || GetPlayer(sim).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim).SimGenitalType == "vagina" || GetPlayer(sim).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim, false);
                                        }

                                        // SIM 1 STRAP END

                                        // SIM 2 STRAP START
                                        if ((GetPlayer(sim2).SimGenitalType == "vagina" || GetPlayer(sim2).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim2, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim2).SimGenitalType == "vagina" || GetPlayer(sim2).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim2, false);
                                        }
                                        // SIM 2 STRAP END

                                        simDescription3 = sim.SimDescription;
										simDescription4 = sim2.SimDescription;

                                        // cock processing

                                        // SIM 1 PEEN START
                                        if (GetPlayer(sim).SimGenitalType == "penis" || GetPlayer(sim).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim, true);
                                        }
                                        // SIM 1 PEEN END

                                        // SIM 2 PEEN START
                                        if (GetPlayer(sim2).SimGenitalType == "penis" || GetPlayer(sim2).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim2, true);
                                        }
                                        // SIM 2 PEEN END
                                    }
                                    catch
									{
									}
									try
									{
										// see if sim should NOT be wearing condom
										if (!Settings.UseCondom || Settings.CondomIsBroken)
										{
											if (GetPlayer(Actor).SimGenitalType == "penis" || GetPlayer(Actor).SimGenitalType == "both")
                                            {
												WearCondom(Actor, false);
											}
                                            // check the partner too
                                            if (GetPlayer(Partner.Actor).SimGenitalType == "penis" || GetPlayer(Partner.Actor).SimGenitalType == "both")
                                            {
												WearCondom(Partner.Actor, false);
											}
										}
										// see if sim SHOULD be wearing condom
										if (Settings.UseCondom && !Settings.CondomIsBroken)
										{
                                            if (GetPlayer(Actor).SimGenitalType == "penis" || GetPlayer(Actor).SimGenitalType == "both")
                                            {
												WearCondom(Actor, true);
											}
											else if (GetPlayer(Partner.Actor).SimGenitalType == "penis" || GetPlayer(Partner.Actor).SimGenitalType == "both")
                                            {
												WearCondom(Partner.Actor, true);
											}
										}
									}
									catch
									{
									}
								}

								// if theres 3 sims (kill me)
								else if (Part.Players.Count == 3 && Part.Position.MaxSims == 3)
								{
									Sim sim3 = null;
									Sim sim4 = null;
									Sim sim5 = null;
									try
									{
										foreach (Player item2 in ready)
										{
											if (item2.PositionIndex == 1)
											{
												sim3 = item2.Actor;
												continue;
											}
											if (item2.PositionIndex == 2)
											{
												sim4 = item2.Actor;
												continue;
											}
											if (item2.PositionIndex == 3)
											{
												sim5 = item2.Actor;
												continue;
											}
											break;
										}
									}
									catch
									{
									}
									simDescription3 = sim3.SimDescription;
									simDescription4 = sim4.SimDescription;
									simDescription5 = sim5.SimDescription;
									try
									{
                                        // SIM 3 STRAP START
                                        if ((GetPlayer(sim3).SimGenitalType == "vagina" || GetPlayer(sim3).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim3, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim3).SimGenitalType == "vagina" || GetPlayer(sim3).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim3, false);
                                        }
                                        // SIM 3 STRAP END

                                        // SIM 4 STRAP START
                                        if ((GetPlayer(sim4).SimGenitalType == "vagina" || GetPlayer(sim4).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim4, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim4).SimGenitalType == "vagina" || GetPlayer(sim4).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim4, false);
                                        }
                                        // SIM 4 STRAP END

                                        // SIM 5 STRAP START
                                        if ((GetPlayer(sim5).SimGenitalType == "vagina" || GetPlayer(sim5).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim5, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim5).SimGenitalType == "vagina" || GetPlayer(sim5).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim5, false);
                                        }
                                        // SIM 5 STRAP END

                                        //
                                        // cock time
                                        //

                                        // SIM 3 PEEN START
                                        if (GetPlayer(sim3).SimGenitalType == "penis" || GetPlayer(sim3).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim3, true);
                                        }
                                        // SIM 3 PEEN END

                                        // SIM 4 PEEN START
                                        if (GetPlayer(sim4).SimGenitalType == "penis" || GetPlayer(sim4).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim4, true);
                                        }
                                        // SIM 4 PEEN END

                                        // SIM 5 PEEN START
                                        if (GetPlayer(sim5).SimGenitalType == "penis" || GetPlayer(sim5).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim5, true);
                                        }
                                        // SIM 5 PEEN END


                                        // three sim condom processing

                                        if (Settings.UseCondom && !Settings.CondomIsBroken)
										{
                                            if (GetPlayer(sim3).SimGenitalType == "penis" || GetPlayer(sim3).SimGenitalType == "both")
                                            {
												WearCondom(sim3, true);
											}
                                            if (GetPlayer(sim4).SimGenitalType == "penis" || GetPlayer(sim4).SimGenitalType == "both")
                                            {
												WearCondom(sim4, true);
											}
                                            if (GetPlayer(sim5).SimGenitalType == "penis" || GetPlayer(sim5).SimGenitalType == "both")
                                            {
												WearCondom(sim5, true);
											}
										}
										else if (!Settings.UseCondom || Settings.CondomIsBroken)
										{
                                            if (GetPlayer(sim3).SimGenitalType == "penis" || GetPlayer(sim3).SimGenitalType == "both")
                                            {
												WearCondom(sim3, false);
											}
                                            if (GetPlayer(sim4).SimGenitalType == "penis" || GetPlayer(sim4).SimGenitalType == "both")
                                            {
												WearCondom(sim4, false);
											}
                                            if (GetPlayer(sim5).SimGenitalType == "penis" || GetPlayer(sim5).SimGenitalType == "both")
                                            {
												WearCondom(sim5, false);
											}
										}
									}
									catch
									{
									}
								}
								else if (Part.Players.Count == 4 && Part.Position.MaxSims == 4)
								{
									Sim sim6 = null;
									Sim sim7 = null;
									Sim sim8 = null;
									Sim sim9 = null;
									try
									{
										foreach (Player item3 in ready)
										{
											if (item3.PositionIndex == 1)
											{
												sim6 = item3.Actor;
												continue;
											}
											if (item3.PositionIndex == 2)
											{
												sim7 = item3.Actor;
												continue;
											}
											if (item3.PositionIndex == 3)
											{
												sim8 = item3.Actor;
												continue;
											}
											if (item3.PositionIndex == 4)
											{
												sim9 = item3.Actor;
												continue;
											}
											break;
										}
									}
									catch
									{
									}
                                    simDescription6 = sim6.SimDescription;
                                    simDescription7 = sim7.SimDescription;
                                    simDescription8 = sim8.SimDescription;
                                    simDescription9 = sim9.SimDescription;
                                    try
									{

                                        // SIM 6 STRAP START
                                        if ((GetPlayer(sim6).SimGenitalType == "vagina" || GetPlayer(sim6).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim6, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim6).SimGenitalType == "vagina" || GetPlayer(sim6).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim6, false);
                                        }
                                        // SIM 6 STRAP END

                                        // SIM 7 STRAP START
                                        if ((GetPlayer(sim7).SimGenitalType == "vagina" || GetPlayer(sim7).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim7, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim7).SimGenitalType == "vagina" || GetPlayer(sim7).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim7, false);
                                        }
                                        // SIM 7 STRAP END

                                        // SIM 8 STRAP START
                                        if ((GetPlayer(sim8).SimGenitalType == "vagina" || GetPlayer(sim8).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim8, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim8).SimGenitalType == "vagina" || GetPlayer(sim8).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim8, false);
                                        }
                                        // SIM 8 STRAP END

                                        // SIM 9 STRAP START
                                        if ((GetPlayer(sim9).SimGenitalType == "vagina" || GetPlayer(sim9).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim9, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim9).SimGenitalType == "vagina" || GetPlayer(sim9).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim9, false);
                                        }
                                        // SIM 9 STRAP END

                                        // SIM 6 PEEN START
                                        if (GetPlayer(sim6).SimGenitalType == "penis" || GetPlayer(sim6).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim6, true);
                                        }
                                        // SIM 6 PEEN END

                                        // SIM 7 PEEN START
                                        if (GetPlayer(sim7).SimGenitalType == "penis" || GetPlayer(sim7).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim7, true);
                                        }
                                        // SIM 7 PEEN END

                                        // SIM 8 PEEN START
                                        if (GetPlayer(sim8).SimGenitalType == "penis" || GetPlayer(sim8).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim8, true);
                                        }
                                        // SIM 8 PEEN END

                                        // SIM 9 PEEN START
                                        if (GetPlayer(sim9).SimGenitalType == "penis" || GetPlayer(sim9).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim9, true);
                                        }
                                        // SIM 9 PEEN END


                                        // condom processing

                                        if (Settings.UseCondom && !Settings.CondomIsBroken)
                                        {
                                            if (GetPlayer(sim6).SimGenitalType == "penis" || GetPlayer(sim6).SimGenitalType == "both")
                                            {
                                                WearCondom(sim6, true);
                                            }
                                            if (GetPlayer(sim7).SimGenitalType == "penis" || GetPlayer(sim7).SimGenitalType == "both")
                                            {
                                                WearCondom(sim7, true);
                                            }
                                            if (GetPlayer(sim8).SimGenitalType == "penis" || GetPlayer(sim8).SimGenitalType == "both")
                                            {
                                                WearCondom(sim8, true);
                                            }
                                            if (GetPlayer(sim9).SimGenitalType == "penis" || GetPlayer(sim9).SimGenitalType == "both")
                                            {
                                                WearCondom(sim9, true);
                                            }
                                        }
                                        else if (!Settings.UseCondom || Settings.CondomIsBroken)
                                        {
                                            if (GetPlayer(sim6).SimGenitalType == "penis" || GetPlayer(sim6).SimGenitalType == "both")
                                            {
                                                WearCondom(sim6, false);
                                            }
                                            if (GetPlayer(sim7).SimGenitalType == "penis" || GetPlayer(sim7).SimGenitalType == "both")
                                            {
                                                WearCondom(sim7, false);
                                            }
                                            if (GetPlayer(sim8).SimGenitalType == "penis" || GetPlayer(sim8).SimGenitalType == "both")
                                            {
                                                WearCondom(sim8, false);
                                            }
                                            if (GetPlayer(sim9).SimGenitalType == "penis" || GetPlayer(sim9).SimGenitalType == "both")
                                            {
                                                WearCondom(sim9, false);
                                            }
                                        }

                                    }
                                    catch
									{
									}
								}
								else if (Part.Players.Count == 5 && Part.Position.MaxSims == 5)
								{
									Sim sim10 = null;
									Sim sim11 = null;
									Sim sim12 = null;
									Sim sim13 = null;
									Sim sim14 = null;
									try
									{
										foreach (Player item4 in ready)
										{
											if (item4.PositionIndex == 1)
											{
												sim10 = item4.Actor;
												continue;
											}
											if (item4.PositionIndex == 2)
											{
												sim11 = item4.Actor;
												continue;
											}
											if (item4.PositionIndex == 3)
											{
												sim12 = item4.Actor;
												continue;
											}
											if (item4.PositionIndex == 4)
											{
												sim13 = item4.Actor;
												continue;
											}
											if (item4.PositionIndex == 5)
											{
												sim14 = item4.Actor;
												continue;
											}
											break;
										}
									}
									catch
									{
									}
                                    simDescription10 = sim10.SimDescription;
                                    simDescription11 = sim11.SimDescription;
                                    simDescription12 = sim12.SimDescription;
                                    simDescription13 = sim13.SimDescription;
                                    simDescription14 = sim14.SimDescription;
                                    try
									{

                                        // SIM 10 STRAP START
                                        if ((GetPlayer(sim10).SimGenitalType == "vagina" || GetPlayer(sim10).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim10, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim10).SimGenitalType == "vagina" || GetPlayer(sim10).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim10, false);
                                        }
                                        // SIM 10 STRAP END

                                        // SIM 11 STRAP START
                                        if ((GetPlayer(sim11).SimGenitalType == "vagina" || GetPlayer(sim11).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim11, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim11).SimGenitalType == "vagina" || GetPlayer(sim11).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim11, false);
                                        }
                                        // SIM 11 STRAP END

                                        // SIM 12 STRAP START
                                        if ((GetPlayer(sim12).SimGenitalType == "vagina" || GetPlayer(sim12).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim12, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim12).SimGenitalType == "vagina" || GetPlayer(sim12).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim12, false);
                                        }
                                        // SIM 12 STRAP END

                                        // SIM 13 STRAP START
                                        if ((GetPlayer(sim13).SimGenitalType == "vagina" || GetPlayer(sim13).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim13, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim13).SimGenitalType == "vagina" || GetPlayer(sim13).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim13, false);
                                        }
                                        // SIM 13 STRAP END

                                        // SIM 14 STRAP START
                                        if ((GetPlayer(sim14).SimGenitalType == "vagina" || GetPlayer(sim14).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim14, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim14).SimGenitalType == "vagina" || GetPlayer(sim14).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim14, false);
                                        }
                                        // SIM 14 STRAP END

                                        // SIM 10 PEEN START
                                        if (GetPlayer(sim10).SimGenitalType == "penis" || GetPlayer(sim10).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim10, true);
                                        }
                                        // SIM 10 PEEN END

                                        // SIM 11 PEEN START
                                        if (GetPlayer(sim11).SimGenitalType == "penis" || GetPlayer(sim11).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim11, true);
                                        }
                                        // SIM 11 PEEN END

                                        // SIM 12 PEEN START
                                        if (GetPlayer(sim12).SimGenitalType == "penis" || GetPlayer(sim12).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim12, true);
                                        }
                                        // SIM 12 PEEN END

                                        // SIM 13 PEEN START
                                        if (GetPlayer(sim13).SimGenitalType == "penis" || GetPlayer(sim13).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim13, true);
                                        }
                                        // SIM 13 PEEN END

                                        // SIM 14 PEEN START
                                        if (GetPlayer(sim14).SimGenitalType == "penis" || GetPlayer(sim14).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim14, true);
                                        }
                                        // SIM 14 PEEN END


                                        // condom processing

                                        if (Settings.UseCondom && !Settings.CondomIsBroken)
                                        {
                                            if (GetPlayer(sim10).SimGenitalType == "penis" || GetPlayer(sim10).SimGenitalType == "both")
                                            {
                                                WearCondom(sim10, true);
                                            }
                                            if (GetPlayer(sim11).SimGenitalType == "penis" || GetPlayer(sim11).SimGenitalType == "both")
                                            {
                                                WearCondom(sim11, true);
                                            }
                                            if (GetPlayer(sim12).SimGenitalType == "penis" || GetPlayer(sim12).SimGenitalType == "both")
                                            {
                                                WearCondom(sim12, true);
                                            }
                                            if (GetPlayer(sim13).SimGenitalType == "penis" || GetPlayer(sim13).SimGenitalType == "both")
                                            {
                                                WearCondom(sim13, true);
                                            }
                                            if (GetPlayer(sim14).SimGenitalType == "penis" || GetPlayer(sim14).SimGenitalType == "both")
                                            {
                                                WearCondom(sim14, true);
                                            }
                                        }
                                        else if (!Settings.UseCondom || Settings.CondomIsBroken)
                                        {
                                            if (GetPlayer(sim10).SimGenitalType == "penis" || GetPlayer(sim10).SimGenitalType == "both")
                                            {
                                                WearCondom(sim10, false);
                                            }
                                            if (GetPlayer(sim11).SimGenitalType == "penis" || GetPlayer(sim11).SimGenitalType == "both")
                                            {
                                                WearCondom(sim11, false);
                                            }
                                            if (GetPlayer(sim12).SimGenitalType == "penis" || GetPlayer(sim12).SimGenitalType == "both")
                                            {
                                                WearCondom(sim12, false);
                                            }
                                            if (GetPlayer(sim13).SimGenitalType == "penis" || GetPlayer(sim13).SimGenitalType == "both")
                                            {
                                                WearCondom(sim13, false);
                                            }
                                            if (GetPlayer(sim14).SimGenitalType == "penis" || GetPlayer(sim14).SimGenitalType == "both")
                                            {
                                                WearCondom(sim14, false);
                                            }
                                        }

                                    }
									catch
									{
									}
								}
								else if (Part.Players.Count == 6 && Part.Position.MaxSims == 6)
								{
									Sim sim15 = null;
									Sim sim16 = null;
									Sim sim17 = null;
									Sim sim18 = null;
									Sim sim19 = null;
									Sim sim20 = null;
									try
									{
										foreach (Player item5 in ready)
										{
											if (item5.PositionIndex == 1)
											{
												sim15 = item5.Actor;
												continue;
											}
											if (item5.PositionIndex == 2)
											{
												sim16 = item5.Actor;
												continue;
											}
											if (item5.PositionIndex == 3)
											{
												sim17 = item5.Actor;
												continue;
											}
											if (item5.PositionIndex == 4)
											{
												sim18 = item5.Actor;
												continue;
											}
											if (item5.PositionIndex == 5)
											{
												sim19 = item5.Actor;
												continue;
											}
											if (item5.PositionIndex == 6)
											{
												sim20 = item5.Actor;
												continue;
											}
											break;
										}
									}
									catch
									{
									}
                                    simDescription15 = sim15.SimDescription;
                                    simDescription16 = sim16.SimDescription;
                                    simDescription17 = sim17.SimDescription;
                                    simDescription18 = sim18.SimDescription;
                                    simDescription19 = sim19.SimDescription;
                                    simDescription20 = sim20.SimDescription;
                                    try
									{

                                        // SIM 15 STRAP START
                                        if ((GetPlayer(sim15).SimGenitalType == "vagina" || GetPlayer(sim15).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim15, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim15).SimGenitalType == "vagina" || GetPlayer(sim15).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim15, false);
                                        }
                                        // SIM 15 STRAP END

                                        // SIM 16 STRAP START
                                        if ((GetPlayer(sim16).SimGenitalType == "vagina" || GetPlayer(sim16).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim16, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim16).SimGenitalType == "vagina" || GetPlayer(sim16).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim16, false);
                                        }
                                        // SIM 16 STRAP END

                                        // SIM 17 STRAP START
                                        if ((GetPlayer(sim17).SimGenitalType == "vagina" || GetPlayer(sim17).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim17, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim17).SimGenitalType == "vagina" || GetPlayer(sim17).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim17, false);
                                        }
                                        // SIM 17 STRAP END

                                        // SIM 18 STRAP START
                                        if ((GetPlayer(sim18).SimGenitalType == "vagina" || GetPlayer(sim18).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim18, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim18).SimGenitalType == "vagina" || GetPlayer(sim18).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim18, false);
                                        }
                                        // SIM 18 STRAP END

                                        // SIM 19 STRAP START
                                        if ((GetPlayer(sim19).SimGenitalType == "vagina" || GetPlayer(sim19).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim19, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim19).SimGenitalType == "vagina" || GetPlayer(sim19).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim19, false);
                                        }
                                        // SIM 19 STRAP END

                                        // SIM 20 STRAP START
                                        if ((GetPlayer(sim20).SimGenitalType == "vagina" || GetPlayer(sim20).SimGenitalType == "neither") && Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim20, true);
                                        }
                                        // if strapon isn't meant to be used
                                        else if ((GetPlayer(sim20).SimGenitalType == "vagina" || GetPlayer(sim20).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                        {
                                            SwitchToStrapon(sim20, false);
                                        }
                                        // SIM 20 STRAP END

                                        // SIM 15 PEEN START
                                        if (GetPlayer(sim15).SimGenitalType == "penis" || GetPlayer(sim15).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim15, true);
                                        }
                                        // SIM 15 PEEN END

                                        // SIM 16 PEEN START
                                        if (GetPlayer(sim16).SimGenitalType == "penis" || GetPlayer(sim16).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim16, true);
                                        }
                                        // SIM 16 PEEN END

                                        // SIM 17 PEEN START
                                        if (GetPlayer(sim17).SimGenitalType == "penis" || GetPlayer(sim17).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim17, true);
                                        }
                                        // SIM 17 PEEN END

                                        // SIM 18 PEEN START
                                        if (GetPlayer(sim18).SimGenitalType == "penis" || GetPlayer(sim18).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim18, true);
                                        }
                                        // SIM 18 PEEN END

                                        // SIM 19 PEEN START
                                        if (GetPlayer(sim19).SimGenitalType == "penis" || GetPlayer(sim19).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim19, true);
                                        }
                                        // SIM 19 PEEN END

                                        // SIM 20 PEEN START
                                        if (GetPlayer(sim20).SimGenitalType == "penis" || GetPlayer(sim20).SimGenitalType == "both")
                                        {
                                            SwitchToPeener(sim20, true);
                                        }
                                        // SIM 20 PEEN END


                                        // condom processing

                                        if (Settings.UseCondom && !Settings.CondomIsBroken)
                                        {
                                            if (GetPlayer(sim15).SimGenitalType == "penis" || GetPlayer(sim15).SimGenitalType == "both")
                                            {
                                                WearCondom(sim15, true);
                                            }
                                            if (GetPlayer(sim16).SimGenitalType == "penis" || GetPlayer(sim16).SimGenitalType == "both")
                                            {
                                                WearCondom(sim16, true);
                                            }
                                            if (GetPlayer(sim17).SimGenitalType == "penis" || GetPlayer(sim17).SimGenitalType == "both")
                                            {
                                                WearCondom(sim17, true);
                                            }
                                            if (GetPlayer(sim18).SimGenitalType == "penis" || GetPlayer(sim18).SimGenitalType == "both")
                                            {
                                                WearCondom(sim18, true);
                                            }
                                            if (GetPlayer(sim19).SimGenitalType == "penis" || GetPlayer(sim19).SimGenitalType == "both")
                                            {
                                                WearCondom(sim19, true);
                                            }
                                            if (GetPlayer(sim20).SimGenitalType == "penis" || GetPlayer(sim20).SimGenitalType == "both")
                                            {
                                                WearCondom(sim20, true);
                                            }
                                        }
                                        else if (!Settings.UseCondom || Settings.CondomIsBroken)
                                        {
                                            if (GetPlayer(sim15).SimGenitalType == "penis" || GetPlayer(sim15).SimGenitalType == "both")
                                            {
                                                WearCondom(sim15, false);
                                            }
                                            if (GetPlayer(sim16).SimGenitalType == "penis" || GetPlayer(sim16).SimGenitalType == "both")
                                            {
                                                WearCondom(sim16, false);
                                            }
                                            if (GetPlayer(sim17).SimGenitalType == "penis" || GetPlayer(sim17).SimGenitalType == "both")
                                            {
                                                WearCondom(sim17, false);
                                            }
                                            if (GetPlayer(sim18).SimGenitalType == "penis" || GetPlayer(sim18).SimGenitalType == "both")
                                            {
                                                WearCondom(sim18, false);
                                            }
                                            if (GetPlayer(sim19).SimGenitalType == "penis" || GetPlayer(sim19).SimGenitalType == "both")
                                            {
                                                WearCondom(sim19, false);
                                            }
                                            if (GetPlayer(sim20).SimGenitalType == "penis" || GetPlayer(sim20).SimGenitalType == "both")
                                            {
                                                WearCondom(sim20, false);
                                            }
                                        }

                                    }
									catch
									{
									}
								}
							}
							foreach (Player item6 in ready)
							{
								item6.CanAnimate = true;
							}
						}
						PassionCommon.Wait();
					}
					ImproveRelationships();
					EndMotiveUpdates();
				}
				if (HasPart)
				{
					Leave();
					Reset();
				}
				try
				{
					//get out of nakey outfit, take off peen and condom

						SwitchToPeener(Actor, false);
						if (previousOutfitCategory != OutfitCategories.Naked)
						{
							Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
						}
						SwitchToPeener(Partner.Actor, false);
						if (previousOutfitCategory != OutfitCategories.Naked)
						{
							Partner.Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
						}
						WearCondom(Actor, false);
						if (previousOutfitCategory != OutfitCategories.Naked)
						{
							Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
						}
						WearCondom(Partner.Actor, false);
						if (previousOutfitCategory != OutfitCategories.Naked)
						{
							Partner.Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
						}
						SwitchToStrapon(Actor, false);
						if (previousOutfitCategory != OutfitCategories.Naked)
						{
							Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
						}
						SwitchToStrapon(Partner.Actor, false);
						if (previousOutfitCategory != OutfitCategories.Naked)
						{
							Partner.Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
						}
                }
				catch
				{
				}
				try
				{
					CumInteractions = false;
					Actor.RemoveInteractionByType(CumOnFace.Singleton);
					Actor.RemoveInteractionByType(CumOnTits.Singleton);
					Actor.RemoveInteractionByType(CumOnButt.Singleton);
					Actor.RemoveInteractionByType(CumOnBelly.Singleton);
					Actor.RemoveInteractionByType(CumOnRightHand.Singleton);
					Actor.RemoveInteractionByType(CumOnLeftHand.Singleton);
					Actor.RemoveInteractionByType(CumOnRightFoot.Singleton);
					Actor.RemoveInteractionByType(CumOnLeftFoot.Singleton);
					Actor.RemoveInteractionByType(CumOnThigh.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnFace.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnTits.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnButt.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnBelly.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnRightHand.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnLeftHand.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnRightFoot.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnLeftFoot.Singleton);
					Partner.Actor.RemoveInteractionByType(CumOnThigh.Singleton);
				}
				catch
				{
				}
				try
				{
					if (Part.Target.ObjectType.Name.ToString() == "Sybian")
					{
						Part.Target.Object.AddInteraction(AskToUseSybian.Singleton, true);
						Part.Target.Object.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
						Part.Target.Object.AddInteraction(Interactions.ResetMe.Singleton, true);
						Part.Target.Object.AddInteraction(Interactions.ResetMeActive.Singleton, true);
						Part.Target.Object.AddInteraction(PutInInventory.Singleton, true);
					}
				}
				catch
				{
				}
				return false;
			}

			// apply condom to sim
			public bool WearCondom(Sim PlayerSim, bool CondomOnDick)
			{
				ResourceKey key = ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8");
				SimDescription simDescription = PlayerSim.SimDescription;
				if (Settings.UseCondom && !World.ResourceExists(key))
				{
					PassionCommon.SystemMessage("Condom Accessories not found. Setting is now disabled!");
					Settings.UseCondom = false;
					Settings.CondomIsBroken = false;
					return false;
				}
				try
				{
					if (CondomOnDick && !Settings.CondomIsBroken && Settings.UseCondom)
					{
						SimDescription simDescription2 = PlayerSim.SimDescription;
						if (simDescription2.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8")) == null)
						{
							if (!RandomUtil.CoinFlip())
							{
								SimOutfit uniform = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0x28A1DEBB221B6632"));
								SimOutfit resultOutfit;
								if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), uniform, simDescription2, "RedCondom", out resultOutfit))
								{
									simDescription2.AddOutfit(resultOutfit, OutfitCategories.Naked, true);
									SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
									SwitchOutfitHelper.Start();
									SwitchOutfitHelper.Wait(false);
									try
									{
										PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
									}
									catch
									{
									}
									return true;
								}
							}
							else if (RandomUtil.CoinFlip())
							{
								SimOutfit uniform2 = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0x51E7AC8E736EDB12"));
								SimOutfit resultOutfit2;
								if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), uniform2, simDescription2, "GreenCondom", out resultOutfit2))
								{
									simDescription2.AddOutfit(resultOutfit2, OutfitCategories.Naked, true);
									SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
									SwitchOutfitHelper.Start();
									SwitchOutfitHelper.Wait(false);
									try
									{
										PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit2, 0);
									}
									catch
									{
									}
									return true;
								}
							}
							else
							{
								SimOutfit uniform3 = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0xFCB5B98600AE92C8"));
								SimOutfit resultOutfit3;
								if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), uniform3, simDescription2, "WhiteCondom", out resultOutfit3))
								{
									simDescription2.AddOutfit(resultOutfit3, OutfitCategories.Naked, true);
									SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
									SwitchOutfitHelper.Start();
									SwitchOutfitHelper.Wait(false);
									try
									{
										PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit3, 0);
									}
									catch
									{
									}
									return true;
								}
							}
						}
					}
					else if (!CondomOnDick)
					{
						SimDescription simDescription3 = PlayerSim.SimDescription;
						if ((simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1 && simDescription3.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8")) != null && Settings.CondomIsBroken) || (simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1 && simDescription3.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8")) != null && !Settings.UseCondom) || (simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1 && simDescription3.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8")) != null && Settings.RemoveCondom))
						{
							if (simDescription3.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x0603B3F0BE3C7883")) == null)
							{
								while (simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1)
								{
									simDescription3.RemoveOutfit(OutfitCategories.Naked, 0, true);
								}
								try
								{
									PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
								}
								catch
								{
								}
							}
							return false;
						}
					}
				}
				catch
				{
				}
				return false;
			}


			// attach strapon to dickless sim
			public bool SwitchToStrapon(Sim PlayerSim, bool AddRemove)
			{
				SimDescription simDescription = PlayerSim.SimDescription;

					try
					{
						// if straps are disabled (aka for cowards)
						if (!Settings.FemaleUseStrapOn)
						{
							return false;
						}
						// if we're adding it
						if (AddRemove)
						{
							SimDescription simDescription2 = PlayerSim.SimDescription;
							if (simDescription2.GetOutfitCount(OutfitCategories.Naked) == 1)
							{
							// generate new outfit
							SimOutfit uniform = null;

                            // if sim is female
                            if (simDescription.IsFemale)
                            {
                                uniform = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0xF32C06036EFA3D8E"));
                            }
                            // if sim is male
                            else
                            {
                                uniform = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0xF32C06036EFA3D85"));
                            }

                            SimOutfit resultOutfit;
								if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), uniform, simDescription2, "Strapon", out resultOutfit))
								{
									simDescription2.AddOutfit(resultOutfit, OutfitCategories.Naked, true);
									SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
									SwitchOutfitHelper.Start();
									SwitchOutfitHelper.Wait(false);
									try
									{
										PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
									}
									catch
									{
									}
									return true;
								}
							}
						}
						// end strap addition

						// if we're removing it
						else if (!AddRemove)
						{
							SimDescription simDescription3 = PlayerSim.SimDescription;
							if (simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1)
							{
								while (simDescription3.GetOutfitCount(OutfitCategories.Naked) > 1)
								{
									simDescription3.RemoveOutfit(OutfitCategories.Naked, 0, true);
								}
								try
								{
									PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
								}
								catch
								{
								}
								return false;
							}
						}
						// end strap removal
					}
					catch
					{
					}
				return false;
			}

			// add spaceman peener to sims with peens
			//... so thats what that meant
			public bool SwitchToPeener(Sim PlayerSim, bool AddIt)
			{

						// if we're adding it
						if (AddIt)
						{
							SimDescription simDescription = PlayerSim.SimDescription;
					string ErectPeen = GetPlayer(PlayerSim).SimErectSIMO;
                    SimOutfit uniform = null;
                    uniform = new SimOutfit(ResourceKey.FromString(ErectPeen));
					//
					//


                    
								SimOutfit resultOutfit;
								if (OutfitUtils.TryApplyUniformToOutfit(simDescription.GetOutfit(OutfitCategories.Naked, 0), uniform, simDescription, "ErectPenis", out resultOutfit))
								{
									simDescription.AddOutfit(resultOutfit, OutfitCategories.Naked, true);
									SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
									SwitchOutfitHelper.Start();
									SwitchOutfitHelper.Wait(false);
									try
									{
										PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
									}
									catch
									{
									}
								}
								return true;
							
						}
						// end peen addition

						// if we're removing it
						else if (!AddIt)
						{
                    SimDescription simDescription2 = PlayerSim.SimDescription;
                    if (simDescription2.GetOutfitCount(OutfitCategories.Naked) != 1)
                    {
                        while (simDescription2.GetOutfitCount(OutfitCategories.Naked) > 1)
                        {
                            simDescription2.RemoveOutfit(OutfitCategories.Naked, 0, true);
                        }
                        try
                        {
                            PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
                        }
                        catch
                        {
                        }
                        return false;
                    }
                }
				// end peen removal

				return false;


			}

			public void OnAnimationCompleted(StateMachineClient sender, IEvent evt)
			{
				if (State != PassionState.Leaving && State != PassionState.Stopping)
				{
					State = PassionState.Ready;
				}
			}

			public void Watch(Sim sim)
			{
				Watch(GetPlayer(sim));
			}

			// watch passion interaction
			// double check this to make sure that this action only runs for sims who'd be into it
			public void Watch(Player target)
			{
				if (!IsValid || !target.IsValid || !target.IsActive || !Actor.RouteToObjectRadialRange(target.Actor, 1.5f, 3f) || !target.IsValid || !target.IsActive)
				{
					return;
				}
				// shitty math time
				Actor.RouteTurnToFace(target.Actor.Position);
				State = PassionState.Watching;
				InteractionInstance currentInteraction = Actor.CurrentInteraction;
				currentInteraction.StandardEntry();
				currentInteraction.BeginCommodityUpdates();
				currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Fun, 100f, false, 100f, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
				Libido.IncreaseUrgency(Actor);
				long num = SimClock.CurrentTicks + RandomUtil.GetInt(600, 1200);
				long num2 = SimClock.CurrentTicks + RandomUtil.GetInt(30, 50);
				while (IsWatching && Actor.HasNoExitReason() && target.IsActive && (!currentInteraction.Autonomous || SimClock.CurrentTicks < num))
				{
					if (SimClock.CurrentTicks > num2)
					{
						// if a sim is a party animal or is the RNG check passes a sim who is watching may join
						// ...rewrite this
						if (Settings.AutonomyChance > 0 && (Actor.HasTrait(TraitNames.PartyAnimal) || RandomUtil.GetInt(0, 299) < Settings.AutonomyChance))
						{
							if (IsAutonomous && target.HasPart && target.Part.IsAutonomous && target.Part.HasRoom && WillPassion(target.Part) && Join(target.Part))
							{
								currentInteraction.EndCommodityUpdates(true);
								DirectStartLoop();
								return;
							}
							if (Join(GetTarget(Actor)))
							{
								currentInteraction.EndCommodityUpdates(true);
								DirectStartLoop();
								return;
							}
						}
						Actor.PlayReaction(PassionCommon.RandomReactionPos, target.Actor, ReactionSpeed.ImmediateWithoutOverlay);
						num2 = SimClock.CurrentTicks + RandomUtil.GetInt(75, 125);
					}
					PassionCommon.Wait(10);
				}
				currentInteraction.EndCommodityUpdates(true);
				currentInteraction.StandardExit();
				if (IsWatching)
				{
					State = PassionState.None;
				}
			}

			// switch position?
			// things without prefix infer the actor, partner is player2 aka the other guy
			public void Switch(Player partner)
			{
				if (partner == null || !IsValid || !partner.IsValid || !HasPart || !partner.HasPart)
				{
					return;
				}
				if (Part.BroWeAreSwitching == false)
				{
					return;
				}
				Part part = Part;
				Part part2 = partner.Part;
                int positionIndex = PositionIndex;
				int positionIndex2 = partner.PositionIndex;
				if (part == part2)
				{
					PositionIndex = positionIndex2;
					partner.PositionIndex = positionIndex;
					BufferedAnimation = Part.Position.GetAnimation(this);
					partner.BufferedAnimation = Part.Position.GetAnimation(partner);
				}
				else if (part.Target == part2.Target)
				{
					Vector3 exitPoint = ExitPoint;
					Vector3 exitPoint2 = partner.ExitPoint;
					PositionIndex = positionIndex2;
					partner.PositionIndex = positionIndex;
					ExitPoint = exitPoint2;
					partner.ExitPoint = exitPoint;
					IsInPlace = false;
					partner.IsInPlace = false;
					Join(part2);
					partner.Join(part);
					part.Players.Remove(ID);
					if (part.Initiator == this)
					{
						part.SwapInitiator(partner);
					}
					part2.Players.Remove(partner.ID);
					if (part2.Initiator == partner)
					{
						part2.SwapInitiator(this);
					}
					BufferedAnimation = part2.Position.GetAnimation(this);
					partner.BufferedAnimation = part.Position.GetAnimation(partner);
				}
				else
				{
					SwitchPart = part2;
					partner.SwitchPart = part;
					Actor.InteractionQueue.AddNext(Interactions.SwitchRoute.Singleton.CreateInstance(partner.Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
					partner.Actor.InteractionQueue.AddNext(Interactions.SwitchRoute.Singleton.CreateInstance(Actor, partner.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
					ActiveLeaveJoin = true;
					partner.ActiveLeaveJoin = true;
					Stop();
					partner.Stop();
				}
			}

			public void EndSwitch()
			{
				CanSwitch = false;
				SwitchPart = null;


            }

			// stop interaction (but dont actually leave it?)
			public void Stop(ExitReason reason)
			{
				if (IsValid)
				{
					Actor.AddExitReason(reason);
				}
				Stop();
			}

			public void Stop()
			{
				State = PassionState.Stopping;
			}

			// leave the passion interaction
			public bool Leave()
			{
				State = PassionState.Leaving;
				CanAnimate = false;
				try
				{
					Actor.LookAtManager.EnableLookAts();
					ClearPosture();
					if (Settings.LibidoBuff)
					{
                        Libido.SatisfactionCalc(Actor, Partner.Actor);
					}
					Part.BroWeAreSwitching = false;
					RegisterWoohoo();
                    try
                    {
                        if (!ActiveLeave && !CanSwitch && Settings.GetSoft && (!Settings.StrapOnMode || (GetPlayer(Actor).SimGenitalType == "penis" || GetPlayer(Actor).SimGenitalType == "both")))
                        {
							SwitchToPeener(Actor, false);
                        }
                    }
                    catch
                    {
                    }
                    if (IsInPlace && ExitPoint != Vector3.Empty && ExitPoint != Vector3.Invalid)
					{
						Location = ExitPoint;
					}
					if (!ActiveLeave && !CanSwitch && (Settings.Outfit != 0 || HasPreferredOutfit))
					{
						RevertOutfit();
					}
					Modules.PostProcessing(Actor);
					ActiveLeave = false;
				}
				catch
				{
				}
				try
				{
					if (HasPart)
					{
						Part.Remove(this);
					}
				}
				catch
				{
				}
				Reset();
				return false;
			}

			public void RegisterWoohoo()
			{
				if (!IsValid || !HadPartner)
				{
					return;
				}
				if (HasPart && Part.HasTarget && Part.Target.HasObject && Part.Target.ObjectType.IsGameObject)
				{
					EventTracker.SendEvent(new WooHooEvent(EventTypeId.kWooHooed, Actor, Partner.Actor, Part.Target.Object));
				}
				else
				{
					EventTracker.SendEvent(new WooHooEvent(EventTypeId.kWooHooed, Actor, Partner.Actor, Partner.Actor));
				}
				if (Actor.SimDescription.HadFirstWooHoo)
				{
					return;
				}
				// first time buff
				Actor.SimDescription.SetFirstWooHoo();
				if (Settings.WoohooBuff)
				{
					if (Actor.BuffManager.HasElement((BuffNames)9944098001884692765uL))
					{
						Actor.BuffManager.RemoveElement((BuffNames)9944098001884692765uL);
					}
					Actor.BuffManager.AddElement((BuffNames)9944098001884692765uL, Origin.None);
				}
				EventTracker.SendEvent(EventTypeId.kHadFirstWoohoo, Actor, Partner.Actor);
			}

			// reset all da shit!!!!!!!!!!!
			public void Reset()
			{
				StopJealousyBroadcast();
				Part = null;
				IsActive = false;
				IsInPlace = false;
				IsAutonomous = false;
				CanAnimate = false;
				CanSwitch = false;
				SpinDisabled = false;
				DirectTargeted = false;
				StartTime = 0L;
				NumberAccepted = 0;
				PositionIndex = 0;
				PartnersToCheckCount = 0;
				PreviousOutfitIndex = 0;
				PreviousOutfitCategory = OutfitCategories.None;
				PotentialImpregnators = new List<Player>();
				PartnersToCheck = new List<Player>();
				PartnersUpdated = new List<Player>();
				ExitPoint = Vector3.Empty;
				State = PassionState.None;
				BufferedAnimation = string.Empty;
				BufferedTargetAnimation = string.Empty;
				BufferedObjectAnimation = string.Empty;
				HeldItem = null;
				Partner = null;
				Settings.PassionFuckSession = false;
				Settings.CondomIsBroken = false;
				Settings.RemoveCondom = true;
				SwitchPlayerActor = null;
				SwitchPlayerPartner = null;
			}
		}

		[Persistable]
		public class Target
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

			public static Target Create(GameObject obj)
			{
				Target target = new Target();
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

			public static Target Create(PassionType type, Vector3 location, Vector3 forward)
			{
				Target target = new Target();
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
						if ((num3 > 1f && num4 > 1f && num3 > num4 && Forward.x > Forward.z) || (num3 < 1f && num4 < 1f && num3 < num4 && 0f - Forward.x > 0f - Forward.z))
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
						else if ((num3 > 1f && num4 < 1f && num3 > num4 && Forward.x < 0f - Forward.z) || (num3 < 1f && num4 > 1f && num3 < num4 && 0f - Forward.x < Forward.z))
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
						else if ((num3 > 1f && num4 < 1f && num3 > num4 && Forward.x > 0f - Forward.z) || (num3 < 1f && num4 > 1f && num3 < num4 && 0f - Forward.x > Forward.z))
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
						else if ((num3 > 1f && num4 > 1f && num3 < num4 && Forward.x < Forward.z) || (num3 <= 1f && num4 < 1f && num3 > num4 && 0f - Forward.x < 0f - Forward.z))
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
						if ((num16 > 1f && num17 > 1f && num16 > num17 && Forward.x > Forward.z) || (num16 < 1f && num17 < 1f && num16 < num17 && 0f - Forward.x > 0f - Forward.z))
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
						else if ((num16 > 1f && num17 < 1f && num16 > num17 && Forward.x < 0f - Forward.z) || (num16 < 1f && num17 > 1f && num16 < num17 && 0f - Forward.x < Forward.z))
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
						else if ((num16 > 1f && num17 < 1f && num16 > num17 && Forward.x > 0f - Forward.z) || (num16 < 1f && num17 > 1f && num16 < num17 && 0f - Forward.x > Forward.z))
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
						else if ((num16 > 1f && num17 > 1f && num16 < num17 && Forward.x < Forward.z) || (num16 <= 1f && num17 < 1f && num16 > num17 && 0f - Forward.x < 0f - Forward.z))
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
						result = ((!TwoButtonDialog.Show(PassionCommon.Localize("S3_Passion.Terms.WhatPartOfHotTub"), PassionCommon.Localize("S3_Passion.Terms.Top"), PassionCommon.Localize("S3_Passion.Terms.Bottom"))) ? PartArea.Bottom : PartArea.Top);
					}
					else if (PT.Is<TableDining3x1>())
					{
						result = ((!TwoButtonDialog.Show(PassionCommon.Localize("S3_Passion.Terms.WhatPartOfTable"), PassionCommon.Localize("S3_Passion.Terms.LeftSide"), PassionCommon.Localize("S3_Passion.Terms.RightSide"))) ? PartArea.Right : PartArea.Left);
					}
				}
				return result;
			}
		}

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

			public Target Target;

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
					return GetPosition(CurrentPositionKey);
				}
			}

			public PassionType Type
			{
				get
				{
					return (Target != null) ? Target.ObjectType : null;
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
							if ((part != null || part2 != null) && ((part != null && part.ContainedSim != null) || (part2 != null && part2.ContainedSim != null)))
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
						if ((part != null || part2 != null) && ((part != null && part.ContainedSim != null && part.ContainedSim != excluded) || (part2 != null && part2.ContainedSim != null && part2.ContainedSim != excluded)))
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
					if (value != null && (value.State == PassionState.Ready || value.State == PassionState.Animating || (value.State == PassionState.Routing && value.GetDistanceTo(Target as IGameObject) < 5f)))
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
				bool flag = PassionCommon.Match(Settings.RandomizationOptions, RandomizationOptions.SameCategory);
				int num = ((Settings.InitialCategory != 1024) ? Settings.InitialCategory : 12);
				int num2 = (HasPosition ? Position.Categories : num);
				int penises = 0;
				int vaginas = 0;
                GetSexCharacteristics(out penises, out vaginas);
				if (Part.BroWeAreSwitching == true)
				{
                    IPositionChoice randomValidPosition = Position.GetRandomValidPosition(Type, Count, penises, vaginas, flag ? num2 : num, true);
                }
				else
				{
                    IPositionChoice randomValidPosition = Position.GetRandomValidPosition(Type, Count, penises, vaginas, flag ? num2 : num, false);
                    if (randomValidPosition == null && !flag)
                    {
                        randomValidPosition = Position.GetRandomValidPosition(Type, Count, penises, vaginas);
                    }
                    if (randomValidPosition != null)
                    {
                        SmartSetPosition(randomValidPosition);
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
				if ((position != null && position != Position) || HasSequence)
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
				if (Settings.ExcludeInvalidPositions)
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
								Position.Animation.Slot slot = ((set != null) ? set.GetSlot(i) : null);
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
									else if (player == null || (item.PositionIndex == i && player.SimGenitalType != "penis"))
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
									else if (player == null || (item.PositionIndex == i && player.SimGenitalType == "penis"))

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
				if (Settings.RandomizationLength > 0)
				{
					if (LastSwitch == 0)
					{
						LastSwitch = SimClock.CurrentTicks;
					}
					else if (LastSwitch + Settings.RandomizationLength < SimClock.CurrentTicks)
					{
						GetRandomValidPosition();
					}
				}
				return true;
			}

			public void SortAndGetPosition()
			{
				if (Settings.InitialCategory == 1024)
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
					else if (value.State == PassionState.Animating || (value.State == PassionState.Routing && value.GetDistanceTo(Location) < 5f))
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
					if (Settings.UseCondom && Settings.CondomBrakeChance > 0 && HasPosition && World.ResourceExists(key) && World.ResourceExists(key2))
					{
						foreach (Player value in Players.Values)
						{
							if (!value.IsValid || !value.IsActive || value.IsStopping || Settings.CondomIsBroken)
							{
								continue;
							}
							if (value.SimGenitalType == "penis" && !value.Actor.Inventory.ContainsType(typeof(CondomPack), 1) && !value.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !Settings.PassionFuckSession)
							{
								Settings.PassionFuckSession = true;
								try
								{
									PassionCommon.SimMessage(PassionCommon.Localize("Fuck! I don't have any condoms!").ToString(), value.Actor);
									PassionCommon.Wait();
								}
								catch
								{
								}
								Settings.CondomIsBroken = true;
								return true;
							}
							if (value.SimGenitalType == "penis" && value.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !Settings.PassionFuckSession)
							{
								Settings.PassionFuckSession = true;
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
							else if (value.SimGenitalType == "penis" && value.Actor.Inventory.ContainsType(typeof(CondomPack), 1) && !value.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !Settings.PassionFuckSession)
							{
								Settings.PassionFuckSession = true;
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
								if (value2.SimGenitalType == "penis" && !value2.Actor.Inventory.ContainsType(typeof(CondomPack), 1) && !value2.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !Settings.PassionFuckSession)
								{
									Settings.PassionFuckSession = true;
									try
									{
										PassionCommon.SimMessage(PassionCommon.Localize("Fuck! I don't have any condoms!").ToString(), value2.Actor);
										PassionCommon.Wait();
									}
									catch
									{
									}
									Settings.CondomIsBroken = true;
									return true;
								}
								if (value2.SimGenitalType == "penis" && value2.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !Settings.PassionFuckSession)
								{
									Settings.PassionFuckSession = true;
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
								else if (value2.SimGenitalType == "penis" && value2.Actor.Inventory.ContainsType(typeof(CondomPack), 1) && !value2.Actor.Inventory.ContainsType(typeof(SingleCondom), 1) && !Settings.PassionFuckSession)
								{
									Settings.PassionFuckSession = true;
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
									if (((value2.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 4)) || (value2.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 8)) || (value2.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 12))) && RandomUtil.RandomChance(Settings.CondomBrakeChance))
									{
										try
										{
											PassionCommon.SimMessage(PassionCommon.Localize("Shit! Condom broke!").ToString(), value2.Actor);
											PassionCommon.Wait();
										}
										catch
										{
										}
										Settings.CondomIsBroken = true;
										return true;
									}
								}
								catch
								{
								}
								try
								{
									if (((value.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 4)) || (value.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 8)) || (value.SimGenitalType == "penis" && PassionCommon.Match(Position.Categories, 12))) && RandomUtil.RandomChance(Settings.CondomBrakeChance))
									{
										try
										{
											PassionCommon.SimMessage(PassionCommon.Localize("Shit! Condom broke!").ToString(), value.Actor);
											PassionCommon.Wait();
										}
										catch
										{
										}
										Settings.CondomIsBroken = true;
										return true;
									}
								}
								catch
								{
								}
							}
						}
					}
					else if (!Settings.UseCondom || World.ResourceExists(key) || World.ResourceExists(key2))
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
					if (Settings.PregnancyMethod == PregnancyMethod.Disabled || Settings.PregnancyRisk <= 0 || !HasPosition)
					{
						return;
					}
					foreach (Player value in Players.Values)
					{
						if (!value.IsValid || !value.IsActive || value.IsStopping || value.Actor.SimDescription.IsPregnant || (!value.Actor.IsFemale && !Settings.PregnancyMale))
						{
							continue;
						}
						foreach (Player value2 in Players.Values)
						{
							if (!value2.IsActive || value2.Actor == value.Actor || (value2.SimGenitalType != "penis" && (Settings.PregnancyMethod != PregnancyMethod.ByPosition || !Position.PossibleSameSexPregnancy) && Settings.PregnancyMethod != PregnancyMethod.KWSystem) || ((Settings.PregnancyMethod != PregnancyMethod.ByCategory || !PassionCommon.Match(Position.Categories, 12)) && (Settings.PregnancyMethod != PregnancyMethod.ByPosition || !Position.PossiblePregnancy) && (Settings.PregnancyMethod != PregnancyMethod.KWSystem || !value.Actor.IsFemale || !PassionCommon.Match(Position.Categories, 4)) && (Settings.PregnancyMethod != PregnancyMethod.KWSystem || !value.Actor.IsMale || !PassionCommon.Match(Position.Categories, 8)) && (Settings.PregnancyMethod != PregnancyMethod.KWSystem || !PassionCommon.Match(Position.Categories, 12))))
							{
								continue;
							}
							if (RandomUtil.RandomChance(Settings.PregnancyRisk) && !value.Actor.SimDescription.IsPregnant && !value2.Actor.SimDescription.IsPregnant && value.Actor.SimDescription.IsFemale && value2.Actor.SimDescription.IsMale)
							{
								PassionCommon.Impregnate(value.Actor, value2.Actor);
								break;
							}
							if (RandomUtil.RandomChance(Settings.PregnancyRisk) && !value2.Actor.SimDescription.IsPregnant && value.Actor.SimDescription.IsMale && value2.Actor.SimDescription.IsFemale)
							{
								PassionCommon.Impregnate(value2.Actor, value.Actor);
								break;
							}
							if ((RandomUtil.RandomChance(Settings.PregnancyRisk) && !value.Actor.SimDescription.IsPregnant && !value2.Actor.SimDescription.IsPregnant && value.Actor.SimDescription.IsFemale && value.SimGenitalType != "penis" && value2.Actor.SimDescription.IsFemale && value2.SimGenitalType != "penis") || (RandomUtil.RandomChance(Settings.PregnancyRisk) && !value.Actor.SimDescription.IsPregnant && !value2.Actor.SimDescription.IsPregnant && value.Actor.SimDescription.IsFemale && value.SimGenitalType != "penis" && value2.Actor.SimDescription.IsFemale && value2.SimGenitalType == "penis"))
							{
								if (Settings.PregnancyMale)
								{
									PassionCommon.Impregnate(value.Actor, value2.Actor);
									break;
								}
							}
							else if (((RandomUtil.RandomChance(Settings.PregnancyRisk) && value.Actor.SimDescription.IsMale && value2.Actor.SimDescription.IsFemale && value2.SimGenitalType == "penis") || (RandomUtil.RandomChance(Settings.PregnancyRisk) && Settings.PregnancyMale && value.Actor.SimDescription.IsMale && value2.Actor.SimDescription.IsMale)) && Settings.PregnancyMale)
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
					value.BufferedAnimation = ((clipData != null) ? clipData.Clip : string.Empty);
					value.RecalculateMotiveUpdates();
					if (!Settings.UseCondom && Settings.STD)
					{
						STD.Process(value);
					}
					else if (Settings.UseCondom && Settings.STD && Settings.CondomIsBroken)
					{
						STD.Process(value);
					}
					if (Settings.Jealousy && value.IsInitiator)
					{
						value.StartJealousyBroadcast();
					}
				}
			}
		}

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
						text = text + ((text.Length > 0) ? ", " : "") + PassionCommon.Localize("S3_Passion.Terms.Oral");
					}
					if (PassionCommon.Match(i, 48))
					{
						text = text + ((text.Length > 0) ? ", " : "") + PassionCommon.Localize("S3_Passion.Terms.Manual");
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
										PassionCommon.BufferLine("    Slot " + slot.Key + ": " + ((slot.Value != null) ? "!null" : "null"));
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

					public ClipData Add(string clip, CASAgeGenderFlags flags, bool penis, bool vagina)
					{
						return Add(new ClipData(clip, flags, penis, vagina));
					}

					public ClipData Add(ClipData clipdata)
					{
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
						foreach (ClipData clip in Clips)
						{
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
								foreach (ClipData clip in Clips)
								{
									if (clip != null && clip.IsValid)
									{
										if (((player.SimGenitalType == "penis" && clip.NeedsPenis) || (player.SimGenitalType == "vagina" && clip.NeedsVagina)) && clip.CompareFlags(player.Flags))
										{
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
						return new ClipData(clip1 + AWPosition[position] + clip2 + ((participants == 3) ? "o" : string.Empty), position == 1, false);
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

					public ClipData(string clip, CASAgeGenderFlags flags)
					{
						Initialize(clip, flags, false, false, false);
					}

					public ClipData(string clip, bool penis, bool vagina)
					{
						Initialize(clip, CASAgeGenderFlags.None, penis, vagina, false);
					}

					public ClipData(string clip, CASAgeGenderFlags flags, bool penis, bool vagina)
					{
						Initialize(clip, flags, penis, vagina, false);
					}

					public ClipData(string clip, CASAgeGenderFlags flags, bool penis, bool vagina, bool breasts)
					{
						Initialize(clip, flags, penis, vagina, breasts);
					}

					public void Initialize(string clip, CASAgeGenderFlags flags, bool penis, bool vagina, bool breasts)
					{
						Clip = clip;
						Flags = flags;
						NeedsPenis = penis;
						NeedsVagina = vagina;
						NeedsBreasts = breasts;
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
							return (!checkPenis) ? Get((int)sim.SimDescription.Gender) : ((!PassionCommon.HasPart(sim, SimPart.Penis)) ? Get(8192) : Get(4096));
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
												Vector3 location = ((matchingNode3 != null) ? new Vector3(PassionCommon.Float(matchingNode3.GetAttribute("X")), PassionCommon.Float(matchingNode3.GetAttribute("Y")), PassionCommon.Float(matchingNode3.GetAttribute("Z"))) : Vector3.Empty);
												XML.Node matchingNode4 = matchingNode2.GetMatchingNode("Facing");
												Vector3 facing = ((matchingNode4 != null) ? new Vector3(PassionCommon.Float(matchingNode4.GetAttribute("X")), PassionCommon.Float(matchingNode4.GetAttribute("Y")), PassionCommon.Float(matchingNode4.GetAttribute("Z"))) : Vector3.Empty);
												float angle = PassionCommon.Float(matchingNode2["Angle"]);
												uint slot2 = 3703456078u;
												try
												{
													slot2 = (string.IsNullOrEmpty(matchingNode2["Slot"]) ? 3703456078u : ((uint)Enum.Parse(typeof(HeldItem.ItemSlots), matchingNode2["Slot"], true)));
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
												slot.Add(matchingNode10.Value, cASAgeGenderFlags, penis, vagina);
											}
										}
										set.Update();
									}
								}
								position.RegisterMinMaxSims();
								if (Positions.ContainsKey(position.Key))
								{
									Positions[position.Key] = position;
								}
								else
								{
									Positions.Add(position.Key, position);
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
								foreach (XML.Node item in matchingNodes)
								{
									int num7 = PassionCommon.Int(item.GetAttribute("Id")) + 1;
									position2.AddHardCodedAnimation(num7, item["Animation"]);
									string text5 = item["Genders"].ToLower();
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
									bool flag = text5.Contains("male") && (!text5.Contains("female") || PassionCommon.Bool(item["UseStrapon"]));
									bool vagina2 = text5 == "female";
									if (PassionCommon.Bool(item["UseStrapon"]))
									{
										if (position2.PutOnStraOn.ContainsKey(num7))
										{
											position2.PutOnStraOn[num7] = true;
										}
										else
										{
											position2.PutOnStraOn.Add(num7, true);
										}
									}
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
										slot3.Add(item["Animation"], CASAgeGenderFlags.None, flag, vagina2);
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
										position2.AddSupportedType<Sims3.Gameplay.Objects.Environment.Scarecrow>();
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
								if (Positions.ContainsKey(position2.Key))
								{
									Positions[position2.Key] = position2;
								}
								else
								{
									Positions.Add(position2.Key, position2);
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
								if (Positions.ContainsKey(text7))
								{
									Positions[text7] = position3;
								}
								else
								{
									Positions.Add(text7, position3);
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
				Target.RegisterMinMaxSims(this);
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
                    if (PassionCommon.Match(Settings.RandomizationOptions, RandomizationOptions.Positions))
                    {
                        list.AddRange(GetValidPositions(type, participants, penises, vaginas, category));
                    }
                    if (PassionCommon.Match(Settings.RandomizationOptions, RandomizationOptions.Sequences))
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
					foreach (Sequence sequence in Sequences)
					{
						if (sequence.CanUseWith(participants) && sequence.CanUseWith(type) && sequence.IsCategory(categories))
						{
							list.Add(sequence);
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
					foreach (Position value in Positions.Values)
					{
						if (value.CanUseWith(participants) && value.CanUseWith(type) && value.IsCategory(categories) && (!Settings.ExcludeInvalidPositions || ((value.Sets.ContainsKey(participants) || value.Sets[participants] != null) && penises >= value.Sets[participants].PenisesNeeded && vaginas >= value.Sets[participants].VaginasNeeded)))
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
					foreach (Position value in Positions.Values)
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
				if (Settings.ExcludeInvalidPositions && Sets.ContainsKey(participants) && Sets[participants] != null)
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
				list2.Add(new ObjectPicker.TabInfo("shop_all_r2", PassionCommon.Localize("S3_Passion.Terms.All"), PositionListForDialog(type, participants, penises, vaginas)));
				ObjectPicker.TabInfo tabInfo = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.Masturbate"), PositionListForDialog(type, participants, penises, vaginas, 16));
				if (tabInfo.RowInfo.Count >= 0)
				{
					list2.Add(tabInfo);
				}
				ObjectPicker.TabInfo tabInfo2 = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.Oral"), PositionListForDialog(type, participants, penises, vaginas, 2));
				if (tabInfo2.RowInfo.Count >= 0)
				{
					list2.Add(tabInfo2);
				}
				ObjectPicker.TabInfo tabInfo3 = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.Vaginal"), PositionListForDialog(type, participants, penises, vaginas, 4));
				if (tabInfo3.RowInfo.Count >= 0)
				{
					list2.Add(tabInfo3);
				}
				ObjectPicker.TabInfo tabInfo4 = new ObjectPicker.TabInfo("shop_skill_r2", PassionCommon.Localize("S3_Passion.Terms.Anal"), PositionListForDialog(type, participants, penises, vaginas, 8));
				if (tabInfo4.RowInfo.Count >= 0)
				{
					list2.Add(tabInfo4);
				}
				List<ObjectPicker.RowInfo> list3 = MenuList.Show(PassionCommon.Localize("S3_Passion.Terms.ChangePosition"), PassionCommon.Localize("S3_Passion.Terms.Ok"), PassionCommon.Localize("S3_Passion.Terms.Cancel"), list2, list);
				if (list3 != null && list3.Count > 0)
				{
					try
					{
						result = list3[0].Item as IPositionChoice;
					}
					catch
					{
					}
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
				foreach (Sequence sequence in Sequences)
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
				foreach (Position value in Positions.Values)
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
				foreach (Position value in Positions.Values)
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
						text = text + ((text.Length > 0) ? ", " : "") + value2.Name;
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

		[Persistable]
		public class Sequence : IPositionChoice
		{
			public class DialogEntry
			{
				public string Name;

				public Sequence Sequence;

				public static DialogEntry Get(string name, Sequence sequence)
				{
					return new DialogEntry(name, sequence);
				}

				public DialogEntry(string name, Sequence sequence)
				{
					Name = name;
					Sequence = sequence;
				}
			}

			public class DialogListItem
			{
				public string Name;

				public SequenceItem Value;

				public static DialogListItem Get(string name, SequenceItem value)
				{
					return new DialogListItem(name, value);
				}

				public DialogListItem(string name, SequenceItem value)
				{
					Name = name;
					Value = value;
				}
			}

			public string Name;

			public int Tones;

			public int Categories;

			public Dictionary<string, PassionType> SupportedTypes;

			public int MinSims;

			public int MaxSims;

			public bool Repeat;

			public bool Continue;

			protected SequenceItem[] mItems;

			public SequenceItem[] Items
			{
				get
				{
					if (mItems == null)
					{
						mItems = new SequenceItem[0];
					}
					return mItems;
				}
			}

			public static Sequence Create()
			{
				return Create(string.Empty, 1);
			}

			public static Sequence Create(string initial)
			{
				return Create(initial, 1);
			}

			public static Sequence Create(string initial, int length)
			{
				Sequence sequence = new Sequence();
				if (!string.IsNullOrEmpty(initial))
				{
					sequence.Add(initial, length, 0);
				}
				return sequence;
			}

			public Sequence()
			{
				Name = string.Empty;
				Tones = 1;
				Categories = 255;
				SupportedTypes = new Dictionary<string, PassionType>(LoadedTypes);
				MaxSims = int.MaxValue;
				MinSims = 0;
				Repeat = true;
			}

			public void RestoreItems(List<XML.Node> itemset)
			{
				if (itemset == null || itemset.Count <= 0)
				{
					return;
				}
				mItems = new SequenceItem[itemset.Count];
				MaxSims = int.MaxValue;
				MinSims = 0;
				foreach (XML.Node item in itemset)
				{
					if (string.IsNullOrEmpty(item["Index"]))
					{
						continue;
					}
					int num = PassionCommon.Int(item["Index"]);
					if (num < 0 || num >= mItems.Length)
					{
						continue;
					}
					SequenceItem sequenceItem = new SequenceItem(item["Key"], PassionCommon.Long(item["Length"]), num);
					mItems[num] = sequenceItem;
					if (sequenceItem.Position != null)
					{
						if (sequenceItem.Position.MaxSims < MaxSims)
						{
							MaxSims = sequenceItem.Position.MaxSims;
						}
						if (sequenceItem.Position.MinSims > MinSims)
						{
							MinSims = sequenceItem.Position.MinSims;
						}
					}
				}
				CleanSupportedTypes();
			}

			public bool IsCategory(int category)
			{
				return category == 1 || Categories == 1 || PassionCommon.Match(category, Categories);
			}

			public bool CanUseWith(PassionType type, int participants, int categories)
			{
				if (type == null)
				{
					return false;
				}
				if (!SupportedTypes.ContainsKey(type.Name))
				{
					return false;
				}
				if (participants > 0 && (participants < MinSims || participants > MaxSims))
				{
					return false;
				}
				if (categories != 0 && !IsCategory(categories))
				{
					return false;
				}
				return true;
			}

			public bool CanUseWith(int participants)
			{
				return participants >= MinSims && participants <= MaxSims;
			}

			public bool CanUseWith(PassionType type)
			{
				return type != null && SupportedTypes.ContainsKey(type.Name);
			}

			public static void CleanSequences()
			{
				foreach (Sequence sequence in Sequences)
				{
					sequence.CleanSupportedTypes();
				}
			}

			public void CleanSupportedTypes()
			{
				Dictionary<string, PassionType> dictionary = new Dictionary<string, PassionType>(LoadedTypes);
				SequenceItem[] items = Items;
				foreach (SequenceItem sequenceItem in items)
				{
					if (!sequenceItem.IsValid)
					{
						continue;
					}
					Position position = sequenceItem.Position;
					foreach (PassionType item in new List<PassionType>(dictionary.Values))
					{
						if (!position.SupportedTypes.ContainsKey(item.Name))
						{
							dictionary.Remove(item.Name);
						}
					}
				}
				SupportedTypes = dictionary;
			}

			public void Add(string key, long length, int index)
			{
				Add(new SequenceItem(key, length, index));
			}

			public void Add(SequenceItem item)
			{
				if (item == null)
				{
					return;
				}
				Position position = item.Position;
				if (position != null)
				{
					Categories &= position.Categories;
					if (position.MaxSims < MaxSims)
					{
						MaxSims = position.MaxSims;
					}
					if (position.MinSims > MinSims)
					{
						MinSims = position.MinSims;
					}
				}
				int num = Items.Length;
				SequenceItem[] array = new SequenceItem[Items.Length + 1];
				for (int i = 0; i < num; i++)
				{
					array[i] = Items[i];
				}
				item.Index = num;
				array[num] = item;
				mItems = array;
				CleanSupportedTypes();
			}

			public void Set(SequenceItem item, int index)
			{
				if (index >= 0 && Items.Length > index)
				{
					Items[index] = item;
					if (item != null)
					{
						item.Index = index;
					}
				}
			}

			public void Swap(int s, int t)
			{
				int num = Items.Length;
				if (s >= 0 && t >= 0 && s < num && t < num)
				{
					SequenceItem item = Items[s];
					SequenceItem item2 = Items[t];
					Set(item, t);
					Set(item2, s);
				}
			}

			public void MoveUp(SequenceItem item)
			{
				if (item != null)
				{
					MoveUp(item.Index);
				}
			}

			public void MoveUp(int s)
			{
				if (s > 0)
				{
					Swap(s, s - 1);
				}
			}

			public void MoveDown(SequenceItem item)
			{
				if (item != null)
				{
					MoveDown(item.Index);
				}
			}

			public void MoveDown(int s)
			{
				if (s < Items.Length - 1)
				{
					Swap(s, s + 1);
				}
			}

			public void Clear()
			{
				Categories = 1;
				SupportedTypes = new Dictionary<string, PassionType>(LoadedTypes);
				MaxSims = int.MaxValue;
				MinSims = 0;
				mItems = null;
			}

			public void Remove(int index)
			{
				if (Items.Length >= 1 && index >= 0 && index < Items.Length)
				{
					int num = Items.Length - 1;
					for (int i = index; i < num; i++)
					{
						Set(Items[i + 1], i);
					}
					if (Items.Length > 1)
					{
						SequenceItem[] destinationArray = new SequenceItem[num];
						Array.Copy(Items, destinationArray, num);
						mItems = destinationArray;
					}
					else
					{
						mItems = new SequenceItem[0];
					}
				}
			}
		}

		[Persistable]
		public class SequenceInstance
		{
			protected int mCurrent = 0;

			protected long mStart = 0L;

			public string Name = string.Empty;

			public bool Repeat = true;

			public bool Continue = false;

			protected SequenceItem[] mItems;

			public Position First
			{
				get
				{
					if (mItems.Length != 0)
					{
						return mItems[0].Position;
					}
					return null;
				}
			}

			public Position Current
			{
				get
				{
					if (mCurrent < mItems.Length)
					{
						return mItems[mCurrent].Position;
					}
					return null;
				}
			}

			public bool Started
			{
				get
				{
					return mStart > 0;
				}
			}

			public static SequenceInstance Create(Sequence sequence)
			{
				SequenceInstance sequenceInstance = new SequenceInstance();
				if (sequence != null)
				{
					sequenceInstance.mItems = new SequenceItem[sequence.Items.Length];
					Array.Copy(sequence.Items, sequenceInstance.mItems, sequence.Items.Length);
					sequenceInstance.Name = sequence.Name;
					sequenceInstance.Repeat = sequence.Repeat;
					sequenceInstance.Continue = sequence.Continue;
				}
				return sequenceInstance;
			}

			public Position Start()
			{
				mStart = SimClock.CurrentTicks;
				return First;
			}

			public Position Next()
			{
				Position result = null;
				if (mCurrent < mItems.Length && mItems[mCurrent] != null)
				{
					if (SimClock.CurrentTicks > mStart + mItems[mCurrent].Length)
					{
						mStart = SimClock.CurrentTicks;
						mCurrent++;
					}
					if (Repeat && mCurrent >= mItems.Length)
					{
						mCurrent = 0;
					}
					if (mCurrent < mItems.Length && mItems[mCurrent] != null)
					{
						result = mItems[mCurrent].Position;
					}
				}
				else
				{
					mCurrent = 0;
				}
				return result;
			}
		}

		[Persistable]
		public class SequenceItem
		{
			public int Index;

			public long Length;

			public string Key;

			public Position Position
			{
				get
				{
					if (Positions.ContainsKey(Key) && Positions[Key] != null)
					{
						return Positions[Key];
					}
					foreach (Position value in Positions.Values)
					{
						if (value != null && Key == value.Clip)
						{
							Key = value.Key;
							return value;
						}
					}
					return null;
				}
			}

			public bool IsValid
			{
				get
				{
					return Position != null;
				}
			}

			public SequenceItem()
			{
				Key = string.Empty;
				Index = 0;
				Length = 1L;
			}

			public SequenceItem(string key, long length, int index)
			{
				Key = key;
				Length = length;
				Index = index;
			}
		}

		[Persistable]
		public class HeldItem
		{
			public enum ItemSlots : uint
			{
				RightHand = 1557334703u,
				LeftHand = 3703456078u,
				RightRing = 4005035538u,
				LeftEarring = 811482909u,
				RightEarring = 1447611514u,
				RightShortSleeveIn = 4009816123u,
				RightShortSleeveOut = 1651179094u,
				RightToe = 3543879632u,
				LeftToe = 2783384923u,
				RightPantsLeg = 3793686282u,
				LeftRing = 3445785831u,
				LeftCarry = 1318179674u,
				L_Bracelet = 2669835805u,
				RightCarry = 646490825u,
				LeftShortSleeveOut = 4158665743u,
				RightAnkle = 732601729u,
				LeftPantsLeg = 3422414079u,
				LeftAnkle = 298896906u,
				RightBracelet = 1848931864u,
				Glasses = 2747942486u,
				LeftShortSleeveIn = 312766088u,
				BackNeck = 1159156198u,
				Mouth = 453825779u,
				Spine0 = 3764909017u,
				PlumbBobSlot = 671527531u,
				HorseSaddle = 2342056608u,
				Dome = 2705289959u,
				None = 0u
			}

			public ResourceKey Key = ResourceKey.kInvalidResourceKey;

			public Vector3 Location = Vector3.Empty;

			public Vector3 Facing = Vector3.Empty;

			public float Angle = 0f;

			public uint Slot = 0u;

			protected ObjectGuid mID = ObjectGuid.InvalidObjectGuid;

			protected GameObject mObject = null;

			protected Player mHoldingPlayer = null;

			public ObjectGuid ID
			{
				get
				{
					return mID;
				}
				set
				{
					mID = value;
				}
			}

			public GameObject Object
			{
				get
				{
					if (mObject == null && Key != ResourceKey.kInvalidResourceKey && HoldingPlayer != null && HoldingPlayer.IsValid)
					{
						GenerateObject();
					}
					return mObject;
				}
				set
				{
					Destroy();
					mObject = value;
					if (mObject != null)
					{
						mID = mObject.ObjectId;
					}
				}
			}

			public Player HoldingPlayer
			{
				get
				{
					return mHoldingPlayer;
				}
				set
				{
					mHoldingPlayer = value;
					if (mObject != null)
					{
						Slots.AttachToSlot(mID, HoldingPlayer.Actor.ObjectId, Slot, false, ref Location, ref Facing, Angle);
					}
					else
					{
						GenerateObject();
					}
				}
			}

			public bool IsValid
			{
				get
				{
					return Key != ResourceKey.kInvalidResourceKey && Object != null && !Object.HasBeenDestroyed;
				}
			}

			public void Copy(Player player)
			{
				if (player != null)
				{
					Create(player, Key, Location, Facing, Angle, Slot);
				}
			}

			public static HeldItem Create(Player player, ResourceKey key, Vector3 location, Vector3 facing, float angle, uint slot)
			{
				if (player != null)
				{
					HeldItem heldItem = Create(key, location, facing, angle, slot);
					heldItem.HoldingPlayer = player;
					player.HeldItem = heldItem;
					return heldItem;
				}
				return null;
			}

			public static HeldItem Create(string objectkey, Vector3 location, Vector3 facing, float angle, uint slot)
			{
				ResourceKey key = ResourceKey.kInvalidResourceKey;
				try
				{
					key = ResourceKey.FromString(objectkey);
				}
				catch
				{
				}
				return Create(key, location, facing, angle, slot);
			}

			public static HeldItem Create(ResourceKey key, Vector3 location, Vector3 facing, float angle, uint slot)
			{
				HeldItem heldItem = new HeldItem();
				heldItem.Key = key;
				heldItem.Location = location;
				heldItem.Facing = facing;
				heldItem.Angle = angle;
				heldItem.Slot = slot;
				return heldItem;
			}

			public void Grab()
			{
				if (mObject != null)
				{
					Destroy();
				}
				GenerateObject();
			}

			protected void GenerateObject()
			{
				try
				{
					Simulator.ObjectInitParameters initData = new Simulator.ObjectInitParameters(0uL, Location, 0, Facing, HiddenFlags.Nothing);
					mID = Simulator.CreateObject(Key, null, initData);
					IScriptProxy proxy = Simulator.GetProxy(mID);
					mObject = ((proxy != null) ? (proxy.Target as GameObject) : null);
					Slots.AttachToSlot(mID, HoldingPlayer.Actor.ObjectId, Slot, false, ref Location, ref Facing, Angle);
				}
				catch
				{
					if (PassionCommon.Testing)
					{
						PassionCommon.SystemMessage("Error generating Held Item.");
					}
				}
			}

			public void Release()
			{
				Destroy();
			}

			protected void Destroy()
			{
				try
				{
					ObjectGuid objectId = mID;
					if (mObject != null)
					{
						objectId = mObject.ObjectId;
					}
					if (objectId != ObjectGuid.InvalidObjectGuid)
					{
						World.RemoveObjectFromObjectManager(objectId);
						Simulator.DestroyObject(objectId);
						mObject = null;
					}
				}
				catch
				{
				}
			}
		}

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
						return Settings.Label;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(actor) && target != null)
						{
							Player player = GetPlayer(actor);
							Player player2 = GetPlayer(target);
							if (player.IsActive || player.IsInPool || player.IsWatching || player2.IsActive || player2.IsInPool || !IsValid(player2.Actor) || (player.Actor != player2.Actor && !Settings.ActiveAlwaysAccepts && !player.WillPassion(player2)))
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
					Player player = GetPlayer(Actor);
					Player player2 = GetPlayer(Target);
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
										PartData partData = ((gameObject.PartComponent != null) ? gameObject.PartComponent.GetPartSimIsIn(player2.Actor) : null);
										if (partData != null || gameObject.IsActorUsingMe(player2.Actor))
										{
											location = player.Location;
											forward = player.Forward;
											if (gameObject is Bed || gameObject is Couch || gameObject is ChairLiving || gameObject is ChairLounge || gameObject is ChairSectional)
											{
												Target target = GetTarget(gameObject);
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
						if (player.Join(GetTarget(location, forward)))
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
						return Settings.SoloLabel;
					}

					public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(actor) && target != null && (!(target is Sim) || actor == target))
						{
							Player player = GetPlayer(actor);
							Target target2 = GetTarget(target);
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
					Player player = GetPlayer(Actor);
					Target target = GetTarget(Target);
					Target target2 = target;
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
								target2 = GetTarget(gameObject);
								try
								{
									if (target2.ObjectType.Name.ToString() == "Couch" || target2.ObjectType.Name.ToString() == "Sofa" || target2.ObjectType.Name.ToString() == "Loveseat" || target2.ObjectType.Name.ToString().Contains("Chair") || target2.ObjectType.Name.ToString().Contains("Table") || target2.ObjectType.Name.ToString().Contains("Bed") || target2.ObjectType.Name.ToString() == "Altar")
									{
										target = GetTarget(gameObject);
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
						return Settings.Label;
					}

					public override bool Test(Sim actor, IGameObject target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(actor))
						{
							Player player = GetPlayer(actor);
							Target target2 = GetTarget(target);
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
					Player player = GetPlayer(Actor);
					Target target = GetTarget(Target);
					Target target2 = target;
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
								target2 = GetTarget(gameObject);
								try
								{
									if (target2.ObjectType.Name.ToString().Contains("Table") || target2.ObjectType.Name.ToString() == "Couch" || target2.ObjectType.Name.ToString() == "Sofa" || target2.ObjectType.Name.ToString() == "Loveseat" || target2.ObjectType.Name.ToString().Contains("Chair") || target2.ObjectType.Name.ToString() == "Fridge" || target2.ObjectType.Name.ToString().Contains("Bed") || target2.ObjectType.Name.ToString() == "Stove" || target2.ObjectType.Name.ToString() == "Counter" || target2.ObjectType.Name.ToString() == "Toilet" || target2.ObjectType.Name.ToString().Contains("Shower") || target2.ObjectType.Name.ToString() == "Altar" || target2.ObjectType.Name.ToString() == "DoorSingle")
									{
										target = GetTarget(gameObject);
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
						return PassionCommon.Localize("S3_Passion.Terms.AskingTo") + " " + Settings.Label;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						return true;
					}
				}

				public static readonly InteractionDefinition Singleton = new Definition();

				public override bool Run()
				{
					Player player = GetPlayer(Actor);
					Player player2 = GetPlayer(Target);
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
						return PassionCommon.Localize("S3_Passion.Terms.BeingAskedTo") + " " + Settings.Label;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						return true;
					}
				}

				public static readonly InteractionDefinition Singleton = new Definition();

				public override bool Run()
				{
					Player player = GetPlayer(Actor);
					Player player2 = GetPlayer(Target);
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
						return PassionCommon.Localize("S3_Passion.Terms.HeadingTo") + " " + Settings.Label;
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
					return GetPlayer(Actor).Route();
				}
			}

			internal sealed class BeginPassion : Interaction<Sim, Sim>
			{
				[DoesntRequireTuning]
				private sealed class Definition : InteractionDefinition<Sim, Sim, BeginPassion>
				{
					public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
					{
						return PassionCommon.Localize(PassionCommon.Localize("S3_Passion.Terms.Beginning") + " " + Settings.Label);
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
					return GetPlayer(Actor).StartLoop();
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
							Player player = GetPlayer(actor);
							if (player.IsValid && player.HasPart && player.Part.HasPosition)
							{
								return Settings.ActiveLabel + PassionCommon.NewLine + PassionCommon.Localize(player.Part.Position.Name);
							}
						}
						return Settings.ActiveLabel;
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
					return GetPlayer(Actor).DoLoop();
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
							Player player = GetPlayer(actor);
							Player player2 = GetPlayer(target);
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
					Player player = GetPlayer(Actor);
					Player player2 = GetPlayer(Target);
                    Part.BroWeAreSwitching = true;
                    SwitchPlayerPartner = Target;
					SwitchPlayerActor = Actor;
					player.Switch(player2);
					return true;
				}
			}

			internal sealed class SwitchRoute : Interaction<Sim, Sim>
			{
				[DoesntRequireTuning]
				private sealed class Definition : InteractionDefinition<Sim, Sim, SwitchRoute>
				{
					public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
					{
						return PassionCommon.Localize("S3_Passion.Terms.SwitchingWith") + " " + ((target != null) ? target.Name : "?");
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
						Player player = GetPlayer(Actor);
						Player player2 = GetPlayer(Target);
						if (player.IsValid && player2.IsValid)
						{
							player.CanSwitch = true;
							int num = 0;
							while (!player2.CanSwitch && player.Actor.HasNoExitReason() && ++num < 270)
							{
								PassionCommon.Wait(10u);
							}
							player.ActiveLeaveJoin = true;
							if (player.Join(player.SwitchPart))
							{
								player.Route();
							}
						}
						player.EndSwitch();
                        return true;
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
							Player player = GetPlayer(actor);
							Target target2 = GetTarget(target);
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
					Player player = GetPlayer(Actor);
					Target target = GetTarget(Target);
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
							Player player = GetPlayer(actor);
							Target target2 = GetTarget(target);
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
					Player player = GetPlayer(Actor);
					Target target = GetTarget(Target);
					if (target != null && target.IsValid && target.HasObject && player.IsValid && player.HasPart)
					{
						Part switchPart = null;
						PartArea key = Passion.Target.ChooseSectionDialog(target.ObjectType);
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
						Player player = GetPlayer(Actor);
						Target target = GetTarget(Target);
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
						Player player = GetPlayer(Actor);
						Target target = GetTarget(Target);
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
						return PassionCommon.Localize("S3_Passion.Terms.Join") + " " + Settings.Label;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(actor))
						{
							try
							{
								Player player = GetPlayer(actor);
								Player player2 = GetPlayer(target);
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
					Player player = GetPlayer(Actor);
					Player player2 = GetPlayer(Target);
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
						return PassionCommon.Localize("S3_Passion.Terms.Join") + " " + Settings.Label;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(actor))
						{
							try
							{
								Player player = GetPlayer(actor);
								Player player2 = GetPlayer(target);
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
					Player player = GetPlayer(Actor);
					Player player2 = GetPlayer(Target);
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
						return PassionCommon.Localize("S3_Passion.Terms.Join") + " " + Settings.Label;
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
						Player player = GetPlayer(Actor);
						Player player2 = GetPlayer(Target);
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
						return PassionCommon.Localize("S3_Passion.Terms.AsktoJoin") + " " + Settings.Label;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(target))
						{
							try
							{
								Player player = GetPlayer(actor);
								Player player2 = GetPlayer(target);
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
						return PassionCommon.Localize("S3_Passion.Terms.AsktoPassionOther") + " " + Settings.Label;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(target))
						{
							try
							{
								if (actor != null && target != null && actor != target && !GetPlayer(target).IsActive)
								{
									if (!Settings.CanReject || actor.HasTrait(TraitNames.MasterOfSeduction))
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
					Player player = GetPlayer(Target);
					Target target = player.GetNearbySupportedTarget();
					if (target == null)
					{
						target = GetTarget(player.Actor);
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
						return PassionCommon.Localize("S3_Passion.Terms.Suggest") + " " + Settings.SoloLabel;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(target))
						{
							try
							{
								if (actor != null && target != null && actor != target && !GetPlayer(target).IsActive)
								{
									if (!Settings.CanReject || actor.HasTrait(TraitNames.MasterOfSeduction))
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
					Player player = GetPlayer(Target);
					Target target = player.GetNearbySupportedTarget();
					if (target == null)
					{
						target = GetTarget(player.Actor);
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
						return PassionCommon.Localize("S3_Passion.Terms.Suggest") + " " + Settings.SoloLabel;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						try
						{
							if (IsValid(target) && actor != null && target != null && actor != target && !GetPlayer(target).IsActive)
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
					Player player = GetPlayer(Target);
					Target target = player.GetNearbySupportedTarget();
					if (target == null)
					{
						target = GetTarget(player.Actor);
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
						if (!IsAutonomous && IsValid(target))
						{
							Player player = GetPlayer(target);
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
					Player player = GetPlayer(Target);
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
						if (!IsAutonomous && IsValid(target))
						{
							Player player = GetPlayer(target);
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
					Player player = GetPlayer(Target);
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
						return PassionCommon.Localize("S3_Passion.Terms.AsktoWatch") + " " + Settings.Label;
					}

					public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(target))
						{
							try
							{
								Player player = GetPlayer(actor);
								Player player2 = GetPlayer(target);
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
						return PassionCommon.Localize("S3_Passion.Terms.Watch") + " " + Settings.Label;
					}

					public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						Player player = GetPlayer(a);
						Player player2 = GetPlayer(target);
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
						return PassionCommon.Localize("S3_Passion.Terms.Watch") + " " + Settings.Label;
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
					GetPlayer(Actor).Watch(Target);
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
						return Settings.SoloLabel;
					}

					public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						Player player = GetPlayer(a);
						if (player.IsWatching && IsValid(a) && a == target && !player.Actor.SimDescription.ChildOrBelow && player.Actor.SimDescription.IsHuman)
						{
							return true;
						}
						return false;
					}
				}

				public static readonly InteractionDefinition Singleton = new Definition();

				public override bool Run()
				{
					Player player = GetPlayer(Actor);
					Target target = GetTarget(Actor);
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
						if (!IsAutonomous && IsValid(target))
						{
							Player player = GetPlayer(target);
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
					GetPlayer(Target).PreferOutfit();
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
						if (!IsAutonomous && IsValid(target))
						{
							Player player = GetPlayer(target);
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
					GetPlayer(Target).ClearPreferredOutfit();
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
						if (!IsAutonomous && IsValid(target))
						{
							Player player = GetPlayer(target);
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
					GetPlayer(Target).ChangePosition();
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
						if (!IsAutonomous && IsValid(target) && !GetPlayer(target).IsActive)
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
						if (!IsAutonomous && IsValid(target) && GetPlayer(target).IsActive)
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
							if (gameObject is Sim && IsValid(gameObject as Sim))
							{
								if (AllPlayers.ContainsKey(value) && AllPlayers[value] != null && !AllPlayers[value].IsActive)
								{
									return true;
								}
								return false;
							}
							if (gameObject != null && AllTargets.ContainsKey(gameObject.ObjectId.Value))
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
					ResetMe(Target);
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
							if (gameObject is Sim && IsValid(gameObject as Sim))
							{
								if (AllPlayers.ContainsKey(value) && AllPlayers[value] != null && AllPlayers[value].IsActive)
								{
									return true;
								}
								return false;
							}
							if (gameObject != null && AllTargets.ContainsKey(gameObject.ObjectId.Value))
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
					ResetMe(Target);
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
					foreach (Player value2 in AllPlayers.Values)
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
					if (AllTargets.Count > 0)
					{
						num = 0;
						foreach (KeyValuePair<ulong, Target> allTarget in AllTargets)
						{
							if (allTarget.Value != null)
							{
								if (allTarget.Value.HasObject)
								{
									if (num != 0)
									{
										PassionCommon.BufferLine("  ------------------");
									}
									Target value = allTarget.Value;
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
						Player player = GetPlayer(sim);
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
					Player player = GetPlayer(Actor);
					Player player2 = GetPlayer(Target);
					Actor.SynchronizationTarget = player2.Actor;
					Actor.SynchronizationRole = Sim.SyncRole.Initiator;
					Actor.SynchronizationLevel = Sim.SyncLevel.Started;
					InteractionInstance entry = (LinkedInteractionInstance = BeReassured.Singleton.CreateInstance(Actor, Target, new InteractionPriority(InteractionPriorityLevel.High), false, true));
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
					Player player = GetPlayer(Actor);
					Player player2 = GetPlayer(Target);
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
						return Settings.Label;
					}

					public override bool Test(Sim actor, PoolLadder target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
					{
						if (!IsAutonomous && IsValid(actor))
						{
							Player player = GetPlayer(actor);
							Target target2 = GetTarget(target);
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
					Player player = GetPlayer(Actor);
					Target target = GetTarget(Target);
					if (player.Join(target))
					{
						foreach (Sim selectedObjectsAsSim in GetSelectedObjectsAsSims())
						{
							Player player2 = GetPlayer(selectedObjectsAsSim);
							if (player2.IsValid && player2.WillPassion(player) && (Settings.ActiveAlwaysAccepts || player.WillPassion(player2)) && player2.Join(target))
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
						return PassionCommon.Localize("S3_Passion.Terms.HeadingTo") + " " + Settings.Label;
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
					Player player = GetPlayer(Actor);
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

		public static bool InitialLoad = true;

		public static EventListener OnNewSimAdded;

		public static EventListener OnNewSimAged;

		public static EventListener OnNewObjectAdded;

		public static EventListener OnNewObjectAddedInInventory;

		public static EventListener OnNewDay;

		public static EventListener OnAnyFlirt;

		public static EventListener OnFlowerKiss;

		public static EventListener OnHadFirstKiss;

		public static EventListener OnSimKissed;

		public static EventListener OnHadFirstRomance;

		public static EventListener OnHighlyVisibleVirtuousRomance;

		public static EventListener OnMultipleVisibleRomances;

		public static EventListener OnPerformedRomanticSingagram;

		public static EventListener OnTerrain;

		public static EventListener OnSimInstantiated;

		public static EventListener OnGotMassage;

		public static EventListener ONWatchedTv;

		public static EventListener OnDance2Music;

		public static OutfitCategories MyOutfit;

		public static Sim SwitchPlayerActor = null;

		public static Sim SwitchPlayerPartner = null;

		public static bool CumInteractions = false;

		[PersistableStatic]
		protected static PersistableSettings mSettings;

		public static PersistableSettings SettingsBackup = null;

		[PersistableStatic]
		protected static Dictionary<ulong, Player> mAllPlayers;

		public static Dictionary<ulong, Player> AllPlayersBackup = null;

		[PersistableStatic]
		protected static Dictionary<ulong, Target> mAllTargets;

		[PersistableStatic]
		protected static List<string> mXMLFiles;

		public static List<string> XMLFilesBackup = null;

		[PersistableStatic]
		protected static Dictionary<string, PassionType> mLoadedTypes;

		public static Dictionary<string, PassionType> LoadedTypesBackup = null;

		[PersistableStatic]
		protected static Dictionary<string, Position> mPositions;

		public static Dictionary<string, Position> PositionsBackup = null;

		[PersistableStatic]
		protected static List<Sequence> mSequences;

		public static List<Sequence> SequencesBackup = null;

		public static int GuestVampiresInLot = 0;

		public static string GuestVampireName = "Null";

		public static PersistableSettings Settings
		{
			get
			{
				if (mSettings == null)
				{
					mSettings = new PersistableSettings();
				}
				return mSettings;
			}
		}

		public static Dictionary<ulong, Player> AllPlayers
		{
			get
			{
				if (mAllPlayers == null)
				{
					mAllPlayers = new Dictionary<ulong, Player>();
				}
				return mAllPlayers;
			}
		}

		public static Dictionary<ulong, Target> AllTargets
		{
			get
			{
				if (mAllTargets == null)
				{
					mAllTargets = new Dictionary<ulong, Target>();
				}
				return mAllTargets;
			}
		}

		public static List<string> XMLFiles
		{
			get
			{
				if (mXMLFiles == null)
				{
					mXMLFiles = new List<string>(PassionCommon.DefaultXMLFiles);
				}
				return mXMLFiles;
			}
		}

		public static Dictionary<string, PassionType> LoadedTypes
		{
			get
			{
				if (mLoadedTypes == null)
				{
					mLoadedTypes = new Dictionary<string, PassionType>();
				}
				return mLoadedTypes;
			}
		}

		public static Dictionary<string, Position> Positions
		{
			get
			{
				if (mPositions == null)
				{
					mPositions = new Dictionary<string, Position>();
				}
				return mPositions;
			}
		}

		public static List<Sequence> Sequences
		{
			get
			{
				if (mSequences == null)
				{
					mSequences = new List<Sequence>();
				}
				return mSequences;
			}
		}

		// PRELOAD MODSTUFF
		public static void Preload()
		{
			if (PassionCommon.Testing)
			{
				PassionCommon.BufferMessage("Preload Fired");
			}
			CustomTrait.Load();
			CustomBuff.Load();
			CustomCareer.Load();
		}




		
		// LOAD THE MOD

		public static void Load(object sender, EventArgs e)
		{
			try
			{
				if (PassionCommon.Testing)
				{
					PassionCommon.BufferMessage("Load Fired");
				}
				if (InitialLoad)
				{
					PassionType.Load();
				}
				PassionBody.LoadBodies("bababooey");
				Load(Sims3.Gameplay.Queries.GetGlobalObjects<Sim>());
				foreach (PassionType value in LoadedTypes.Values)
				{
					if (value != null && !value.IsSim && value.IsGameObject)
					{
						Array objects = Sims3.SimIFace.Queries.GetObjects(value.Type);
						if (objects is GameObject[])
						{
							Load(objects as GameObject[]);
						}
					}
				}
				FenceRedwood_Gate[] objects3 = Sims3.Gameplay.Queries.GetObjects<FenceRedwood_Gate>();
				foreach (FenceRedwood_Gate fenceRedwood_Gate in objects3)
				{
					if (fenceRedwood_Gate != null && fenceRedwood_Gate.GetNameKey() == 5329506766876672555L)
					{
						fenceRedwood_Gate.AddInteraction(GloryHole.ServiceStrangers.Singleton, true);
						fenceRedwood_Gate.AddInteraction(GloryHole.GetSucked.Singleton, true);
					}
				}
				SculptureFloorGunShow[] objects4 = Sims3.Gameplay.Queries.GetObjects<SculptureFloorGunShow>();
				foreach (SculptureFloorGunShow sculptureFloorGunShow in objects4)
				{
					if (sculptureFloorGunShow != null && sculptureFloorGunShow.GetNameKey() == 13788670039724095860uL)
					{
						sculptureFloorGunShow.AddInteraction(StripperPole.Dance.Singleton, true);
						sculptureFloorGunShow.AddInteraction(StripperPole.Dance2.Singleton, true);
						sculptureFloorGunShow.AddInteraction(StripperPole.AskToDanceOnPole.Singleton, true);
						sculptureFloorGunShow.AddInteraction(StripperPole.AskToDanceOnPole2.Singleton, true);
						sculptureFloorGunShow.AddInteraction(StripperPole.WatchStrip.Singleton, true);
					}
				}
				DanceFloor[] objects5 = Sims3.Gameplay.Queries.GetObjects<DanceFloor>();
				foreach (DanceFloor danceFloor in objects5)
				{
					danceFloor.AddInteraction(CustomDance.NightFeaver.Singleton, true);
					danceFloor.AddInteraction(CustomDance.DiscoDance.Singleton, true);
					danceFloor.AddInteraction(CustomDance.Hifiraver.Singleton, true);
					danceFloor.AddInteraction(CustomDance.JunjoufFighter.Singleton, true);
					danceFloor.AddInteraction(CustomDance.TiktTok.Singleton, true);
					danceFloor.AddInteraction(CustomDance.Valenti.Singleton, true);
				}
				Sybian[] objects6 = Sims3.Gameplay.Queries.GetObjects<Sybian>();
				foreach (Sybian sybian in objects6)
				{
					sybian.AddInteraction(AskToUseSybian.Singleton, true);
					sybian.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
					sybian.AddInteraction(Interactions.ResetMe.Singleton, true);
					sybian.AddInteraction(Interactions.ResetMeActive.Singleton, true);
				}
				foreach (Sim actor in LotManager.Actors)
				{
					if (!actor.SimDescription.IsPet && !actor.SimDescription.IsWildAnimal)
					{
						actor.InteractionQueue.Add(Interactions.StopAllPassion.Singleton.CreateInstance(actor, actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
					}
				}
			}
			catch
			{
			}
			PersistableSettings.Import("PassionSettingsBackup");
			if (Settings.RandomizationOptions == RandomizationOptions.None)
			{
				Settings.RandomizationOptions = RandomizationOptions.PositionsAndSequences;
				mPositions = new Dictionary<string, Position>();
			}
			Position.Create(XMLFiles);
			CustomBuff.AddInteractions();
			CustomCareer.GetLocations();
			StartListening();
			Modules.StartListeners();
			InitialLoad = false;
			ReloadDefaultPositions();
		}

		public static ListenerAction Load(Event e)
		{
			try
			{
				GameObject gameObject = e.TargetObject as GameObject;
				if (gameObject is FenceRedwood_Gate && gameObject.GetNameKey() == 5329506766876672555L)
				{
					gameObject.AddInteraction(GloryHole.ServiceStrangers.Singleton, true);
					gameObject.AddInteraction(GloryHole.GetSucked.Singleton, true);
				}
				else if (gameObject is SculptureFloorGunShow && gameObject.GetNameKey() == 13788670039724095860uL)
				{
					gameObject.AddInteraction(StripperPole.Dance.Singleton, true);
					gameObject.AddInteraction(StripperPole.Dance2.Singleton, true);
					gameObject.AddInteraction(StripperPole.AskToDanceOnPole.Singleton, true);
					gameObject.AddInteraction(StripperPole.AskToDanceOnPole2.Singleton, true);
					gameObject.AddInteraction(StripperPole.WatchStrip.Singleton, true);
					Load(gameObject);
				}
				else if (gameObject is DanceFloor)
				{
					gameObject.AddInteraction(CustomDance.NightFeaver.Singleton, true);
					gameObject.AddInteraction(CustomDance.DiscoDance.Singleton, true);
					gameObject.AddInteraction(CustomDance.Hifiraver.Singleton, true);
					gameObject.AddInteraction(CustomDance.JunjoufFighter.Singleton, true);
					gameObject.AddInteraction(CustomDance.TiktTok.Singleton, true);
					gameObject.AddInteraction(CustomDance.Valenti.Singleton, true);
				}
				else if (gameObject is Sybian)
				{
					gameObject.AddInteraction(AskToUseSybian.Singleton, true);
					gameObject.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
					gameObject.AddInteraction(Interactions.ResetMe.Singleton, true);
					gameObject.AddInteraction(Interactions.ResetMeActive.Singleton, true);
					Load(gameObject);
				}
				else if (gameObject is BunkBed)
				{
					BunkBedContainer bunkBedContainer = gameObject as BunkBedContainer;
					if (bunkBedContainer != null)
					{
						if (bunkBedContainer.LowerBunk != null)
						{
							Load(bunkBedContainer.LowerBunk);
						}
						if (bunkBedContainer.UpperBunk != null)
						{
							Load(bunkBedContainer.UpperBunk);
						}
					}
				}
				else if (gameObject is ChairSectional)
				{
					SectionalManager.TriggerUpdate();
				}
				else if (PassionType.IsSupported(gameObject))
				{
					Load(gameObject);
				}
			}
			catch
			{
			}
			return ListenerAction.Keep;
		}

		private static void Load(GameObject[] objs)
		{
			for (int i = 0; i < objs.Length; i++)
			{
				Load(objs[i]);
			}
		}

		private static void Load(GameObject obj)
		{
			if (obj is Sim)
			{
				Sim sim = obj as Sim;
				// add ints if sim is valid for passion
				if (IsValid(sim, true))
				{
					RefreshPlayer(sim);
					sim.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
					sim.AddInteraction(Interactions.UseSimForPassion.Singleton, true);
					sim.AddInteraction(Interactions.SwitchWith.Singleton, true);
					sim.AddInteraction(Interactions.ChangePosition.Singleton, true);
					sim.AddInteraction(Interactions.StopPassion.Singleton, true);
					sim.AddInteraction(Interactions.StopAllPassion.Singleton, true);
					sim.AddInteraction(Interactions.ActiveJoinPassion.Singleton, true);
					sim.AddInteraction(Interactions.JoinPassion.Singleton, true);
					sim.AddInteraction(Interactions.AskToJoinPassion.Singleton, true);
					sim.AddInteraction(Interactions.AskToSoloPassion.Singleton, true);
					sim.AddInteraction(Interactions.AskToPassionOther.Singleton, true);
					sim.AddInteraction(Interactions.AskToWatchPassion.Singleton, true);
					sim.AddInteraction(Interactions.WatchPassion.Singleton, true);
					sim.AddInteraction(Interactions.WatchMasturbate.Singleton, true);
					sim.AddInteraction(Interactions.SetPreferredOutfit.Singleton, true);
					sim.AddInteraction(Interactions.ClearPreferredOutfit.Singleton, true);
					sim.AddInteraction(Interactions.PassionSettingsMenu.Singleton, true);
					sim.AddInteraction(Interactions.PassionSettingsMenuActive.Singleton, true);
					sim.AddInteraction(Interactions.Reassure.Singleton, true);
                    sim.AddInteraction(Interactions.AddAbstinentTrait.Singleton, true);
                    sim.AddInteraction(Interactions.AddAsexualTrait.Singleton, true);
                    sim.AddInteraction(Interactions.AddHypersexualTrait.Singleton, true);
                    sim.AddInteraction(Interactions.RemoveAbstinentTrait.Singleton, true);
                    sim.AddInteraction(Interactions.RemoveAsexualTrait.Singleton, true);
                    sim.AddInteraction(Interactions.RemoveHypersexualTrait.Singleton, true);
                    sim.AddInteraction(Interactions.ResetMe.Singleton, true);
					sim.AddInteraction(Interactions.ResetMeActive.Singleton, true);
					sim.AddInteraction(StripperPole.Strip.Singleton, true);
					sim.AddInteraction(StripperPole.DanceOnPoleStop.Singleton, true);
					sim.AddInteraction(CastSexCharm.Singleton, true);
					sim.AddInteraction(CastEnsorcelCharm.Singleton, true);
					sim.AddInteraction(CastElectrocuteBlast.Singleton, true);
					sim.AddInteraction(WitchReleaseMindControl.Singleton, true);
					sim.AddInteraction(CastUglyCharm.Singleton, true);
				}
			}
			else if (obj != null)
			{
				RefreshTarget(obj);
				obj.AddInteraction(Interactions.MoveTo.Singleton, true);
				obj.AddInteraction(Interactions.MoveGroupTo.Singleton, true);
				obj.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
				obj.AddInteraction(Interactions.UseObjectForPassionWithSim.Singleton, true);
				obj.AddInteraction(Interactions.ResetMe.Singleton, true);
				obj.AddInteraction(Interactions.ResetMeActive.Singleton, true);
				obj.AddInteraction(CastConvert2Toy.Singleton, true);
				if (obj is Car || obj is FixerCar || obj is FixerCar.FixerCarFixed || obj is Boat || obj is BoatWaterScooter || obj is BoatSpeedBoat || obj is BoatSpeedFishingBoat || obj is Rug || obj is Desk || obj is TableEnd || obj is TableDining1x1 || obj is TableDining2x1 || obj is TableDining3x1 || obj is TableCoffee || obj is TableBar || obj is SaunaClassic || obj is CounterIsland || obj is Counter || obj is Fridge || obj is Loveseat || obj is Sofa || obj is Shower || obj is ShowerOutdoor || obj is ToiletStall || obj is ShowerPublic_Dance || obj is ShowerTub || obj is CornerBathtub || obj is Bathtub || obj is BedSingle || obj is BedDouble || obj is Altar || obj is ChairLiving || obj is ChairDining || obj is ChairLounge || obj is ChairSectional || obj is RockingChair || obj is Urinal || obj is Toilet || obj is BrainEnhancingMachine || obj is HotTub4Seated || obj is HotTubGrotto || obj is MassageTable || obj is Windows || obj is Bicycle || obj is DoorSingle || obj is CarSports || obj is CarExpensive1 || obj is CarExpensive2 || obj is CarHatchback || obj is CarUsed1 || obj is CarUsed2 || obj is CarNormal1 || obj is CarVan4door || obj is CarPickup2door || obj is CarSedan || obj is CarHighSocietyOpen || obj is CarHighSocietyVintage || obj is CarLuxuryExotic || obj is CarLuxurySport || obj is MotorcycleRacing || obj is MotorcycleChopper || obj is BoatRowBoat || obj is AdultMagicBroom || obj is ModerateAdultBroom || obj is ExpensiveAdultBroom || obj is SculptureFloorGunShow || obj is WashingMachine || obj is Dryer || obj is PoolTable || obj is PoolLadder || obj is WorkoutBench || obj is Stove || obj is Sybian || obj is FenceRedwood_Gate || obj is SinkCounter || obj is Sink || obj is Pot || obj is PicnicTable || obj is Urnstone || obj is KissingBooth || obj is Telescope || obj is Sims3.Gameplay.Objects.Environment.Scarecrow || obj is HauntedHouse || obj is ScienceResearchStation || obj is HotTubBase || obj is Podium || obj is MechanicalBull)
				{
					try
					{
						obj.RemoveInteractionByType(CastConvert2Toy.Singleton);
					}
					catch
					{
					}
				}
				if (obj is Urinal)
				{
					obj.AddInteraction(FemaleUseUrinal.Singleton, true);
				}
				else if (obj is ShowerPublic_Dance)
				{
					try
					{
						obj.RemoveInteractionByType(ShowerPublic_Dance.TakeShower.Singleton);
					}
					catch
					{
					}
					obj.AddInteraction(TakeNakedShower.Singleton, true);
				}
			}
			Modules.Load(obj);
		}

		public static void UnLoad(object sender, EventArgs e)
		{
			AllPlayersBackup = new Dictionary<ulong, Player>(mAllPlayers);
			foreach (Player value in mAllPlayers.Values)
			{
				if (value != null)
				{
					value.ActiveLeaveJoin = false;
					value.Reset();
				}
			}
			mAllPlayers.Clear();
			mAllPlayers = null;
			mAllTargets.Clear();
			mAllTargets = null;
			PassionBody.Unload();
			STD.ClearData();
			Target.ClearMinMaxSims();
			PersistableSettings.Export("PassionSettingsBackup");
		}

		public static void StartListening()
		{
			//start listening for new events
			OnNewDay = EventTracker.AddListener(EventTypeId.kBecameDaytime, Cleanup);
			OnNewSimAdded = EventTracker.AddListener(EventTypeId.kSimInstantiated, Load);
			OnNewSimAged = EventTracker.AddListener(EventTypeId.kSimAgeTransition, Load);
			OnNewObjectAdded = EventTracker.AddListener(EventTypeId.kBoughtObject, Load);
			OnNewObjectAddedInInventory = EventTracker.AddListener(EventTypeId.kInventoryObjectAdded, Load);
			OnAnyFlirt = EventTracker.AddListener(EventTypeId.kSocialInteraction, Autonomy.PassionCheck);
			ONWatchedTv = EventTracker.AddListener(EventTypeId.kWatchedSportsChannel, Autonomy.WhenWatchTV);
			OnDance2Music = EventTracker.AddListener(EventTypeId.kSimDanced, Autonomy.DanceNude2Music);
			OnGotMassage = EventTracker.AddListener(EventTypeId.kGotMassage, Autonomy.HappendAtMassage);
		}

		public static void StopListening()
		{
			EventTracker.RemoveListener(OnNewDay);
			EventTracker.RemoveListener(OnNewSimAdded);
			EventTracker.RemoveListener(OnNewSimAged);
			EventTracker.RemoveListener(OnNewObjectAdded);
			EventTracker.RemoveListener(OnAnyFlirt);
			EventTracker.RemoveListener(OnSimInstantiated);
			EventTracker.RemoveListener(ONWatchedTv);
			EventTracker.RemoveListener(OnDance2Music);
		}


		public static ListenerAction Cleanup(Event e)
		{
			Cleanup();
			return ListenerAction.Keep;
		}

		public static void Cleanup()
		{
			try
			{
				List<ulong> list = new List<ulong>();
				foreach (KeyValuePair<ulong, Player> allPlayer in AllPlayers)
				{
					if (allPlayer.Value != null && !allPlayer.Value.IsValid)
					{
						list.Add(allPlayer.Key);
					}
				}
				foreach (ulong item in list)
				{
					AllPlayers.Remove(item);
				}
			}
			catch
			{
			}
			try
			{
				List<ulong> list2 = new List<ulong>();
				foreach (KeyValuePair<ulong, Target> allTarget in AllTargets)
				{
					if (allTarget.Value != null && allTarget.Value.Count < 1)
					{
						list2.Add(allTarget.Key);
					}
				}
				foreach (ulong item2 in list2)
				{
					AllTargets.Remove(item2);
				}
			}
			catch
			{
			}
		}

		public static void ReloadPositions()
		{
			mPositions = new Dictionary<string, Position>();
			Position.Create(XMLFiles);
		}

		public static void ReloadDefaultPositions()
		{
			mXMLFiles = new List<string>(PassionCommon.DefaultXMLFiles);
			ReloadPositions();
		}

		public static bool IsValid(Sim sim)
		{
			return IsValid(sim, false);
		}

		public static bool IsValid(Sim sim, bool load)
		{
			if (sim != null && !sim.HasBeenDestroyed && sim.IsHuman && IsValidAge(sim, load))
			{
				return true;
			}
			if (sim != null && !sim.HasBeenDestroyed && sim.IsEP11Bot)
			{
				return true;
			}
			if (sim != null && !sim.HasBeenDestroyed && sim.SimDescription.IsZombie)
			{
				return true;
			}
			return false;
		}

		public static bool IsValidAge(Sim sim)
		{
			return IsValidAge(sim, false);
		}

		public static bool IsValidAge(Sim sim, bool load)
		{
			if (sim != null && sim.SimDescription.TeenOrAbove)
			{
				return true;
			}
			return false;
		}

		public static void RefreshPlayer(Sim sim)
		{
			if (sim == null)
			{
				return;
			}
			ulong value = sim.ObjectId.Value;
			if (AllPlayersBackup != null && AllPlayersBackup.ContainsKey(value) && AllPlayersBackup[value] != null)
			{
				if (AllPlayers.ContainsKey(value))
				{
					AllPlayers[value] = AllPlayersBackup[value];
				}
				else
				{
					AllPlayersBackup.Add(value, AllPlayersBackup[value]);
				}
			}
			if (!AllPlayers.ContainsKey(value))
			{
				return;
			}
			Player player = GetPlayer(sim);
			if (player.HasPart)
			{
				if (!player.Part.Players.ContainsKey(value))
				{
					player.Part.Players.Add(value, player);
				}
				else if (player.Part.Players[value] == null)
				{
					player.Part.Players[value] = player;
				}
				if (player.IsActive)
				{
					player.SetPosture();
				}
			}
		}

		public static Player GetPlayer(Sim sim)
		{
			Player player = null;
			if (sim != null)
			{
				ulong value = sim.ObjectId.Value;
				if (AllPlayers.ContainsKey(value))
				{
					if (AllPlayers[value] != null)
					{
						player = AllPlayers[value];
						player.Actor = sim;
					}
					else
					{
						AllPlayers[value] = Player.Create(sim);
					}
				}
				else
				{
					player = Player.Create(sim);
					AllPlayers.Add(value, player);
				}
			}
			return player;
		}

		public static void RefreshTarget(GameObject obj)
		{
			if (obj == null)
			{
				return;
			}
			ulong value = obj.ObjectId.Value;
			if (AllTargets.ContainsKey(value))
			{
				if (AllTargets[value] == null)
				{
					AllTargets[value] = Target.Create(obj);
				}
				AllTargets[value].Object = obj;
				AllTargets[value].ObjectType = PassionType.GetSupportedType(obj);
			}
		}

		public static Target GetTarget(Vector3 location, Vector3 forward)
		{
			return Target.Create(PassionType.GetSupportedType(Floor.Type), location, forward);
		}

		public static Target GetTarget(IGameObject obj)
		{
			return GetTarget(obj as GameObject);
		}

		public static Target GetTarget(GameObject obj)
		{
			Target target = null;
			if (obj != null)
			{
				if (obj is Sim)
				{
					return GetTarget(obj.Position, obj.ForwardVector);
				}
				ulong value = obj.ObjectId.Value;
				if (AllTargets.ContainsKey(value))
				{
					if (AllTargets[value] != null)
					{
						target = AllTargets[value];
						target.Object = obj;
					}
					else
					{
						target = Target.Create(obj);
						AllTargets[value] = target;
					}
				}
				else
				{
					target = Target.Create(obj);
					AllTargets.Add(value, target);
				}
			}
			return target;
		}

		public static Position GetPosition(string key)
		{
			if (!string.IsNullOrEmpty(key) && Positions.ContainsKey(key))
			{
				return Positions[key];
			}
			return null;
		}

		public static bool HasRoom(IGameObject obj)
		{
			return HasRoom(obj as GameObject);
		}

		public static bool HasRoom(GameObject obj)
		{
			if (obj != null && !obj.HasBeenDestroyed)
			{
				ulong value = obj.ObjectId.Value;
				PassionType supportedType = PassionType.GetSupportedType(obj);
				if (supportedType == null || Target.GetMinSims(supportedType) == 0)
				{
					return false;
				}
				if (AllTargets.ContainsKey(value) && AllTargets[value] != null)
				{
					if (AllTargets[value].IsOccupied)
					{
						return false;
					}
				}
				else
				{
					if (supportedType.NeedsUseList && obj.UseCount > 0)
					{
						return false;
					}
					if (supportedType.NeedsParts)
					{
						foreach (IGameObject containedObject in obj.GetContainedObjectList<IGameObject>(obj.GetContainmentSlots()))
						{
							if (containedObject is Sim)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		public static void ResetMe(IGameObject obj)
		{
			Sim sim = obj as Sim;
			GameObject gameObject = obj as GameObject;
			if (sim != null)
			{
				if (PickBoolean.Show(sim.Name + System.Environment.NewLine + PassionCommon.Localize("S3_Passion.Terms.ResetSimConfirm"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
				{
					Player player = GetPlayer(sim);
					if (player.IsActive)
					{
						player.Leave();
					}
					if (AllPlayers.ContainsKey(player.ID))
					{
						AllPlayers.Remove(player.ID);
					}
				}
			}
			else
			{
				if (gameObject == null || !PickBoolean.Show(gameObject.GetLocalizedName() + System.Environment.NewLine + PassionCommon.Localize("S3_Passion.Terms.ResetObjectConfirm"), false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
				{
					return;
				}
				Target target = GetTarget(gameObject);
				if (target.Count > 0)
				{
					foreach (Part value in target.Parts.Values)
					{
						value.StopAllPlayers();
					}
				}
				if (target.HasObject && AllTargets.ContainsKey(target.Object.ObjectId.Value))
				{
					AllTargets.Remove(target.Object.ObjectId.Value);
				}
			}
		}

		public static void ResetAnimationObject(IGameObject obj)
		{
			GameObject gameObject = obj as GameObject;
			if (gameObject != null)
			{
				Target target = GetTarget(gameObject);
				if (target.HasObject && AllTargets.ContainsKey(target.Object.ObjectId.Value))
				{
					AllTargets.Remove(target.Object.ObjectId.Value);
				}
			}
		}
	}
}
