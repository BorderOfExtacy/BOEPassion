using System;
using System.Collections.Generic;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Objects.RabbitHoles;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace Passion.S3_Passion
{
	[Persistable]
	public class CustomCareer : PassionCommon
	{
		[Persistable]
		public class School : global::Sims3.Gameplay.Careers.School, ICustomCareer
		{
			public const OccupationNames Id = (OccupationNames)16696807201930100829uL;

			public const string BaseLocalizationPrefix = "Gameplay/Excel/Careers/CareerList:";

			public override Tuning SchoolTuning
			{
				get
				{
					School school = ((OwnerDescription != null && !OwnerDescription.Child) ? (CareerManager.GetStaticCareer(OccupationNames.SchoolHigh) as School) : (CareerManager.GetStaticCareer(OccupationNames.SchoolElementary) as School));
					if (school != null)
					{
						return school.SchoolTuning;
					}
					return null;
				}
			}

			public static School Create()
			{
				return new School(null, null, null);
			}

			public static void Load()
			{
				try
				{
					if (CareerManager.GetStaticOccupation((OccupationNames)16696807201930100829uL) == null)
					{
						School career = Create();
						CareerManager.AddStaticOccupation(career);
					}
				}
				catch (Exception ex)
				{
					PassionCommon.BufferMessage(ex.Message);
				}
			}

			public School()
			{
			}

			public School(XmlDbRow myRow, XmlDbTable levelTable, XmlDbTable eventDataTable)
			{
				Initialize();
			}

			public virtual void Initialize()
			{
				base.AvailableInFutureWorld = true;
				SharedData = new CareerSharedData();
				mbIsValid = true;
				mCareerGuid = (OccupationNames)16696807201930100829uL;
				SharedData.Name = "S3_CustomCareer.School.Name";
				SharedData.OvertimeHours = 0f;
				SharedData.MissWorkPerfPerHour = 0f;
				SharedData.OvertimePerfPerHour = 0f;
				SharedData.MaxPerfFlowPerHour = 0f;
				SharedData.MinPerfFlowPerHour = 0f;
				SharedData.MaxPerfFlowPerHourMaxLevel = 0f;
				SharedData.MinPerfFlowPerHourMaxLevel = 0f;
				SharedData.BonusAmountInHours = 0f;
				SharedData.RaisePercent = 0f;
				SharedData.RaisePercentMaxLevel = 0f;
				SharedData.RaiseChanceMin = 0f;
				SharedData.RaiseChanceMax = 0f;
				SharedData.HolidayBonusChance = 0f;
				SharedData.HolidayBonusSalaryPercentage = 0f;
				SharedData.FunPerHourWhileWorking = 0f;
				SharedData.MinCoworkers = 0;
				SharedData.MaxCoworkers = 0;
				SharedData.Text_JobOffer = "S3_CustomCareer.School.JobOffer";
				SharedData.Text_Retirement = "S3_CustomCareer.School.Retirement";
				SharedData.Text_BranchOffer = string.Empty;
				SharedData.SpeechBalloonImage = "Gameplay/Excel/Careers/CareerList:Career School";
				SharedData.WorkInteractionIcon = "Gameplay/Excel/Careers/CareerList:w_simple_school_career_s";
				SharedData.Topic = "Learn School Career";
				SharedData.LearnedInfoText = "School";
				SharedData.ProductVersion = ProductVersion.BaseGame;
				SharedData.Category = CareerCategory.School;
				SharedData.DreamsAndPromisesIcon = "w_school";
				SharedData.ToneDefinitions = new List<CareerToneDefinition>();
				SharedData.Locations = new List<CareerLocation>();
				SharedData.CareerEventList = new List<EventDaily>();
				SharedData.CareerLevels = new Dictionary<string, Dictionary<int, CareerLevel>>();
				XmlDbData xmlDbData = XmlDbData.ReadData("CustomCareer.PrivateSchool");
				if (xmlDbData == null || !xmlDbData.Tables.ContainsKey("PrivateSchool"))
				{
					return;
				}
				foreach (XmlDbRow row in xmlDbData.Tables["PrivateSchool"].Rows)
				{
					PassionCommon.BufferMessage("row populated");
					try
					{
						CareerLevel careerLevel = new CareerLevel(row, "PrivateSchool", ProductVersion.BaseGame);
						PassionCommon.BufferMessage("CareerLevel created, Level:" + careerLevel.Level);
						if (base.CareerLevels.ContainsKey(careerLevel.BranchName))
						{
							PassionCommon.BufferMessage("Branch key already exists, setting the existing index)");
							base.CareerLevels[careerLevel.BranchName].Add(careerLevel.Level, careerLevel);
							base.CareerLevels[careerLevel.BranchName][careerLevel.Level - 1].NextLevels.Add(careerLevel);
							careerLevel.LastLevel = base.CareerLevels[careerLevel.BranchName][careerLevel.Level - 1];
						}
						else
						{
							PassionCommon.BufferMessage("Branch key does not exist, creating");
							try
							{
								Dictionary<int, CareerLevel> dictionary = new Dictionary<int, CareerLevel>();
								dictionary.Add(careerLevel.Level, careerLevel);
								base.CareerLevels.Add(careerLevel.BranchName, dictionary);
								foreach (Dictionary<int, CareerLevel> value in base.CareerLevels.Values)
								{
									int key = careerLevel.Level - 1;
									if (value.ContainsKey(key) && careerLevel.BranchSource.Equals(value[key].BranchName))
									{
										value[key].NextLevels.Add(careerLevel);
										careerLevel.LastLevel = value[key];
										break;
									}
								}
							}
							catch (Exception ex)
							{
								PassionCommon.BufferMessage("Unable to create CareerLevel tables.");
								PassionCommon.BufferMessage("Error: " + ex.Message);
							}
						}
						if (base.Level1 == null && careerLevel.Level == 1)
						{
							SharedData.Level1 = careerLevel;
						}
						if (careerLevel.Level > HighestLevel)
						{
							SharedData.HighestLevelOfThisCareer = careerLevel.Level;
						}
					}
					catch (Exception ex2)
					{
						PassionCommon.BufferClear();
						PassionCommon.BufferLine(ex2.Message);
						PassionCommon.BufferLine(ex2.StackTrace);
						PassionCommon.BufferMessage();
					}
				}
			}

			public virtual void GetLocations()
			{
				CityHall[] objects = global::Sims3.Gameplay.Queries.GetObjects<CityHall>();
				foreach (CityHall rabbitHole in objects)
				{
					CareerLocation careerLocation = new CareerLocation(rabbitHole, this);
				}
			}

			public override bool CareerAgeTest(SimDescription sim)
			{
				return sim.Child || sim.Teen;
			}

			public override void OnDispose()
			{
				base.OnDispose();
			}
		}

		public static void Load()
		{
		}

		public static void GetLocations()
		{
		}
	}
}
