using System;
using System.Collections.Generic;
using PassionCore = Passion.S3_Passion.Core.Passion;
using Passion.Sims3.Gameplay.Objects.HobbiesSkills;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
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
using Sims3.Gameplay.Objects.Toys;
using Sims3.Gameplay.Objects.Vehicles;
using Sims3.Gameplay.Pools;
using Sims3.SimIFace;
using Sims3.Store.Objects;
using Sims3.UI;

namespace Passion.S3_Passion
{
	[Persistable]
	public class PassionType
	{
		private static readonly Type Sim = typeof(Sim);

		private static readonly Type GameObject = typeof(GameObject);

		public const string BdsmFurniture = "Sims3.Gameplay.Objects.Decorations.StockadeFurniture,ClassStockadeFurniture";

		public const string BondageFrame = "Sims3.Gameplay.Objects.Decorations.Mimics.CrossWood,Cross_Wood";

		public const string RestraintFurniture = "Sims3.Gameplay.Objects.RestraintFurniture,RestraintFurniture";

		public const bool NoUseList = false;

		public const bool UseList = true;

		public const bool Parts = true;

		public const ulong PrimoCoffeeTableNameKey = 15946527783677180907uL;

		public const ulong AltaraCoffeeTableNameKey = 2598734301466468606uL;

		private static readonly Dictionary<ulong, PassionType> Overrides = new Dictionary<ulong, PassionType>();

		private Type _mType;

		private bool _mIsSim;

		private bool _mIsGameObject;

		private bool _mNeedsUseList;

		private bool _mNeedsParts;

		public Type Type
		{
			get
			{
				return _mType;
			}
		}

		public bool IsValid
		{
			get
			{
				return Type != null;
			}
		}

		public bool IsGameObject
		{
			get
			{
				return _mIsGameObject;
			}
		}

		public bool IsSim
		{
			get
			{
				return _mIsSim;
			}
		}

		public bool NeedsUseList
		{
			get
			{
				return _mNeedsUseList;
			}
		}

		public bool NeedsParts
		{
			get
			{
				return _mNeedsParts;
			}
		}

		public string Name
		{
			get
			{
				if (IsValid)
				{
					return Type.Name;
				}
				return "null";
			}
		}

		public static void Load()
		{
			Unload<global::Sims3.Gameplay.Objects.Vehicles.Car>();
			Unload<FixerCar>();
			Unload<FixerCar.FixerCarFixed>();
			Unload<BoatSpeedBoat>();
			Unload<BoatSpeedFishingBoat>();
			Unload<BoatWaterScooter>();
			Load<Sim>();
			Load<Floor>();
			Load<Rug>(true);
			Load<Desk>(true);
			Load<TableEnd>();
			Load<TableDining1x1>();
			Load<TableDining2x1>();
			Load<TableDining3x1>();
			Load<TableCoffee>();
			Load<TableBar>();
			Load<PicnicTable>();
			Load<SaunaClassic>(true, true);
			Load<CounterIsland>(true);
			Load<Counter>(true);
			Load<Fridge>(true);
			Load<Loveseat>(false, true);
			Load<Sofa>(false, true);
			Load<Shower>(true);
			Load<ShowerOutdoor>(true);
			Load<ToiletStall>(true);
			Load<ShowerPublic_Dance>(true);
			Load<ShowerTub>(true, true);
			Load<CornerBathtub>(true, true);
			Load<Bathtub>(true, true);
			Load<BedSingle>(false, true);
			Load<BedDouble>(false, true);
			Load<Altar>(false, true);
			Load<Urnstone>(false, true);
			Load<ChairLiving>(true, true);
			Load<ChairDining>(true, true);
			Load<ChairLounge>(true, true);
			Load<ChairSectional>(true, true);
			Load<RockingChair>(true, true);
			Load<Urinal>(true);
			Load<Toilet>(true, true);
			Load<BrainEnhancingMachine>(true, true);
			Load<HotTub4Seated>(true, true);
			Load<HotTubGrotto>(true, true);
			Load<MassageTable>(true, true);
			Load<Window>(true);
			Load<Windows>(true);
			Load<Bicycle>(true, true);
			Load<DoorSingle>(true);
			Load<CarSports>(true, true);
			Load<CarExpensive1>(true, true);
			Load<CarExpensive2>(true, true);
			Load<CarHatchback>(true, true);
			Load<CarUsed1>(true, true);
			Load<CarUsed2>(true, true);
			Load<CarNormal1>(true, true);
			Load<CarVan4door>(true, true);
			Load<CarPickup2door>(true, true);
			Load<CarSedan>(true, true);
			Load<CarHighSocietyOpen>(true, true);
			Load<CarHighSocietyVintage>(true, true);
			Load<CarLuxuryExotic>(true, true);
			Load<CarLuxurySport>(true, true);
			Load<FixerCar>(true, true);
			Load<FixerCar.FixerCarFixed>(true, true);
			Load<MotorcycleRacing>(true, true);
			Load<MotorcycleChopper>(true, true);
			Load<BoatSpeedBoat>(true, true);
			Load<BoatRowBoat>(true, true);
			Load<BoatSpeedFishingBoat>(true, true);
			Load<BoatWaterScooter>(true, true);
			Load<AdultMagicBroom>(true, true);
			Load<ModerateAdultBroom>(true, true);
			Load<ExpensiveAdultBroom>(true, true);
			Load<SculptureFloorGunShow>();
			Load<WashingMachine>();
			Load<Dryer>();
			Load<Scarecrow>();
			Load<HauntedHouse>();
			Load<ScienceResearchStation>();
			Load<HotTubBase>();
			Load<Podium>();
			Load<MechanicalBull>();
			Load<PoolTable>();
			Load<PoolLadder>();
			Load<Telescope>(true);
			Load<SwingSet>(true);
			Load<WorkoutBench>(true);
			Load<Stove>(true);
			Load<SleepPodFuture>(true);
			Load("Sims3.Gameplay.Objects.Decorations.StockadeFurniture,ClassStockadeFurniture");
			Load("Sims3.Gameplay.Objects.Decorations.Mimics.CrossWood,Cross_Wood");
			Load<Sybian>(true);
			Load("Sims3.Gameplay.Objects.RestraintFurniture,RestraintFurniture");
			Load<FenceRedwood_Gate>(true);
			Load<SinkCounter>(true);
			Load<Sink>(true);
			Load<Pot>(true);
			Load<KissingBooth>(true);
			Load<JungleGymLookOutTower>(true);
			Load<Igloo>(true);
			Load<ArcadeMachineBase>(true);
			Load<DollHouse>(true);
			Load<Trampoline>(true);
			Load<Toilet>(true);
			Load<HayBaleSquare>(true);
			Load<Bar>(true);
			Load<BarProfessional>(true);
			Load<BarCandy>(true);
			Load<BalletBarre>(true);
			Overrides.Add(15946527783677180907uL, GetSupportedType<TableCoffee>());
			Overrides.Add(2598734301466468606uL, GetSupportedType<TableCoffee>());
		}

		private static void Load(string name)
		{
			Load(name, false, false);
		}

		public static void Load(string name, bool use)
		{
			Load(name, use, false);
		}

		private static void Load(string name, bool use, bool parts)
		{
			try
			{
				Type type = Type.GetType(name);
				if (type != null)
				{
					Load(type, use, parts);
				}
			}
			catch
			{
				// ignored
			}
		}

		private static void Load<T>()
		{
			Load<T>(false, false);
		}

		private static void Load<T>(bool use)
		{
			Load<T>(use, false);
		}

		private static void Load<T>(bool use, bool parts)
		{
			Load(typeof(T), use, parts);
		}

		public static void Load(Type type)
		{
			Load(type, false, false);
		}

		public static void Load(Type type, bool use)
		{
			Load(type, use, false);
		}

		private static void Load(Type type, bool use, bool parts)
		{
			if (type == null)
			{
				return;
			}
			if (PassionCore.LoadedTypes.ContainsKey(type.Name))
			{
				if (PassionCore.LoadedTypes[type.Name] == null)
				{
					PassionCore.LoadedTypes[type.Name] = Create(type, use, parts);
				}
			}
			else
			{
				PassionCore.LoadedTypes.Add(type.Name, Create(type, use, parts));
			}
		}

		private static void Unload<T>()
		{
			Unload(typeof(T));
		}

		private static void Unload(Type type)
		{
			Unload(type == null ? "null" : type.Name);
		}

		private static void Unload(string name)
		{
			if (PassionCore.LoadedTypes.ContainsKey(name))
			{
				PassionCore.LoadedTypes.Remove(name);
			}
		}

		public static void Unload()
		{
			PassionCore.LoadedTypes.Clear();
		}

		public static bool IsLoaded(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				return PassionCore.LoadedTypes.ContainsKey(name) && PassionCore.LoadedTypes[name] != null;
			}
			return false;
		}

		public static bool IsLoaded(Type type)
		{
			return type != null && IsLoaded(type.Name);
		}

		public static bool IsSupported(IGameObject obj)
		{
			return IsSupported(obj as GameObject);
		}

		public static bool IsSupported(GameObject obj)
		{
			return GetSupportedType(obj) != null;
		}

		public static PassionType GetSupportedType(GameObject obj)
		{
			PassionType passionType;
			if (obj == null) return null;
			ulong nameKey = obj.GetNameKey();
			PassionType @override;
			if (Overrides.TryGetValue(nameKey, out @override))
			{
				passionType = @override;
			}
			else
			{
				passionType = GetSupportedType(obj.GetType());
				if (passionType != null) return passionType;
				foreach (PassionType value in PassionCore.LoadedTypes.Values)
				{
					if (!value.IsValid || !value.Type.IsInstanceOfType(obj)) continue;
					passionType = value;
					break;
				}
			}
			return passionType;
		}

		private static PassionType GetSupportedType<T>()
		{
			return GetSupportedType(typeof(T));
		}

		public static PassionType GetSupportedType(Type type)
		{
			if (type != null && PassionCore.LoadedTypes.ContainsKey(type.Name))
			{
				return PassionCore.LoadedTypes[type.Name];
			}
			return null;
		}

		public static PassionType GetSupportedType(string name)
		{
			if (!string.IsNullOrEmpty(name) && PassionCore.LoadedTypes.ContainsKey(name))
			{
				return PassionCore.LoadedTypes[name];
			}
			return null;
		}

		private static PassionType Create(Type type, bool use, bool parts)
		{
			PassionType passionType = new PassionType();
			passionType._mType = type;
			passionType._mNeedsUseList = use;
			passionType._mNeedsParts = parts;
			if (type == null) return passionType;
			if (type.IsSubclassOf(GameObject))
			{
				passionType._mIsGameObject = true;
			}
			if (type == Sim)
			{
				passionType._mIsSim = true;
			}
			return passionType;
		}

		public bool Is(GameObject obj)
		{
			return IsValid && Type.IsInstanceOfType(obj);
		}

		public bool Is(string name)
		{
			return Name == name;
		}

		public bool Is(string fullname, bool recursion)
		{
			Type type = Type.GetType(fullname);
			return Is(type, recursion);
		}

		public bool Is<T>()
		{
			return Is(typeof(T), true);
		}

		public bool Is<T>(bool recursion)
		{
			return Is(typeof(T), recursion);
		}

		public bool Is(Type type)
		{
			return Is(type, true);
		}

		private bool Is(Type type, bool recursion)
		{
			if (IsValid && type != null)
			{
				return Type == type || Type.IsSubclassOf(type) || (recursion && type.IsSubclassOf(Type));
			}
			return !IsValid && type == null;
		}
	}
}
