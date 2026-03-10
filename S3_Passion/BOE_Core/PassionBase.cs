using System;
using System.Collections.Generic;
using S3_Passion.PassionDance;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.CookingObjects;
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
using Sims3.Gameplay.Pools;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.Store.Objects;
using S3_Passion.BOE_Settings;
using S3_Passion.BOE_Lovemaking;
using S3_Passion.BOE_Interaction;
using S3_Passion.BOE_Objects;
using S3_Passion.BOE_Tunables;
using S3_Passion.BOE_STD;
using S3_Passion.BOE_UI;


namespace S3_Passion.BOE_Core
{
	public class PassionBase
	{
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
		public static PersistableSettings mSettings;

		public static PersistableSettings SettingsBackup = null;

		[PersistableStatic]
		protected static Dictionary<ulong, Player> mAllPlayers;

		public static Dictionary<ulong, Player> AllPlayersBackup = null;

		[PersistableStatic]
		protected static Dictionary<ulong, PassionTarget> mAllTargets;

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

		public static Dictionary<ulong, PassionTarget> AllTargets
		{
			get
			{
				if (mAllTargets == null)
				{
					mAllTargets = new Dictionary<ulong, PassionTarget>();
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
			
			CustomBuff.Load();
			
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
                PassionAutonomyTuning.LoadPassionLotAutonomy("bababooey");
                PassionAutonomyTuning.LoadPassionTraitAutonomy("bababooey");
                PassionBody.LoadBodies();
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



			if (PersistableSettings.Settings.RandomizationOptions == RandomizationOptions.None)
			{
				PersistableSettings.Settings.RandomizationOptions = RandomizationOptions.PositionsAndSequences;
				mPositions = new Dictionary<string, Position>();
			}
			Position.Create(XMLFiles);
			CustomBuff.AddInteractions();
			StartListening();
			PassionCommon.Modules.StartListeners();
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
                    sim.AddInteraction(Interactions.ToggleStrapon.Singleton, true);
                    sim.AddInteraction(Interactions.ToggleErection.Singleton, true);
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
                    sim.AddInteraction(Interactions.DEBUGCheckBottom.Singleton, true);
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
				if (obj is Car || obj is FixerCar || obj is FixerCar.FixerCarFixed || obj is Boat || obj is BoatWaterScooter || obj is BoatSpeedBoat || obj is BoatSpeedFishingBoat || obj is Rug || obj is Desk || obj is TableEnd || obj is TableDining1x1 || obj is TableDining2x1 || obj is TableDining3x1 || obj is TableCoffee || obj is TableBar || obj is SaunaClassic || obj is CounterIsland || obj is Counter || obj is Fridge || obj is Loveseat || obj is Sofa || obj is Shower || obj is ShowerOutdoor || obj is ToiletStall || obj is ShowerPublic_Dance || obj is ShowerTub || obj is CornerBathtub || obj is Bathtub || obj is BedSingle || obj is BedDouble || obj is Altar || obj is ChairLiving || obj is ChairDining || obj is ChairLounge || obj is ChairSectional || obj is RockingChair || obj is Urinal || obj is Toilet || obj is BrainEnhancingMachine || obj is HotTub4Seated || obj is HotTubGrotto || obj is MassageTable || obj is Windows || obj is Bicycle || obj is DoorSingle || obj is CarSports || obj is CarExpensive1 || obj is CarExpensive2 || obj is CarHatchback || obj is CarUsed1 || obj is CarUsed2 || obj is CarNormal1 || obj is CarVan4door || obj is CarPickup2door || obj is CarSedan || obj is CarHighSocietyOpen || obj is CarHighSocietyVintage || obj is CarLuxuryExotic || obj is CarLuxurySport || obj is MotorcycleRacing || obj is MotorcycleChopper || obj is BoatRowBoat || obj is AdultMagicBroom || obj is ModerateAdultBroom || obj is ExpensiveAdultBroom || obj is SculptureFloorGunShow || obj is WashingMachine || obj is Dryer || obj is PoolTable || obj is PoolLadder || obj is WorkoutBench || obj is Stove || obj is Sybian || obj is FenceRedwood_Gate || obj is SinkCounter || obj is Sink || obj is Pot || obj is PicnicTable || obj is Urnstone || obj is KissingBooth || obj is Telescope || obj is Scarecrow || obj is HauntedHouse || obj is ScienceResearchStation || obj is HotTubBase || obj is Podium || obj is MechanicalBull)
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
			PassionCommon.Modules.Load(obj);
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
			PassionAutonomyTuning.Unload();
			STD.ClearData();
			PassionTarget.ClearMinMaxSims();
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
			OnAnyFlirt = EventTracker.AddListener(EventTypeId.kSocialInteraction, BOE_Lovemaking.Autonomy.PassionCheck);
			ONWatchedTv = EventTracker.AddListener(EventTypeId.kWatchedSportsChannel, BOE_Lovemaking.Autonomy.WhenWatchTV);
			OnDance2Music = EventTracker.AddListener(EventTypeId.kSimDanced, BOE_Lovemaking.Autonomy.DanceNude2Music);
			OnGotMassage = EventTracker.AddListener(EventTypeId.kGotMassage, BOE_Lovemaking.Autonomy.HappendAtMassage);
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
				foreach (KeyValuePair<ulong, PassionTarget> allTarget in AllTargets)
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
            if (sim.HasTrait((TraitNames)2214287488174702228uL))
			{
				return false;
			}
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
					AllTargets[value] = PassionTarget.Create(obj);
				}
				AllTargets[value].Object = obj;
				AllTargets[value].ObjectType = PassionType.GetSupportedType(obj);
			}
		}

		public static PassionTarget GetTarget(Vector3 location, Vector3 forward)
		{
			return PassionTarget.Create(PassionType.GetSupportedType(Floor.Type), location, forward);
		}

		public static PassionTarget GetTarget(IGameObject obj)
		{
			return GetTarget(obj as GameObject);
		}

		public static PassionTarget GetTarget(GameObject obj)
		{
			PassionTarget target = null;
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
						target = PassionTarget.Create(obj);
						AllTargets[value] = target;
					}
				}
				else
				{
					target = PassionTarget.Create(obj);
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
				if (supportedType == null || PassionTarget.GetMinSims(supportedType) == 0)
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
				PassionTarget target = GetTarget(gameObject);
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
				PassionTarget target = GetTarget(gameObject);
				if (target.HasObject && AllTargets.ContainsKey(target.Object.ObjectId.Value))
				{
					AllTargets.Remove(target.Object.ObjectId.Value);
				}
			}
		}
	}
}
