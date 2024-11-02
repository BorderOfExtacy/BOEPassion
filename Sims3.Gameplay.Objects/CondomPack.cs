using System.Collections.Generic;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects.RabbitHoles;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CustomContent;

namespace Sims3.Gameplay.Objects
{
	public class CondomPack : GameObject, IHasRouteRadius, IGameObject, IScriptObject, IScriptLogic, IHasScriptProxy, IObjectUI, IExportableContent, INonPurgeableFromNPCInventory
	{
		public class Pickup : PutInInventory
		{
			public sealed class PickupDefinition : Definition
			{
				public override bool Test(Sim actor, IGameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return actor != null && actor.IsHuman && !actor.SimDescription.ChildOrBelow && actor.Inventory != null && base.Test(actor, target, isAutonomous, ref greyedOutTooltipCallback);
				}
			}

			public static readonly PickupDefinition PickupSingleton = new PickupDefinition();

			public override bool Run()
			{
				if (Actor.Inventory == null)
				{
					return false;
				}
				Actor.PlaySoloAnimation("a2o_object_genericSwipe_x", true);
				Actor.Inventory.TryToAdd(Target);
				return true;
			}
		}

		public static readonly ResourceKey PackResourceKey = ResourceKey.FromString("0x319E4F1D-0x00000000-0x000000004F57910E");

		public static readonly ResourceKey PackProductKey = ResourceKey.FromString("0x2F7D0004-0x00000000-0xF213936389DF2C17");

		public static readonly string kCatalogName = "CatalogObjects/CondomPack";

		public static readonly int kPrice = 15;

		public static readonly ThumbnailKey thumb = new ThumbnailKey(PackProductKey, ThumbnailSize.Medium, 0u, 0u);

		public static readonly string storeUIItemID = "CatalogDB_DriedFoodHigh";

		public static readonly StoreItem item = new StoreItem(Localization.LocalizeString(kCatalogName), kPrice, PackResourceKey, thumb, storeUIItemID, OnCreateCondomPack, OnProcessCondomPack, null);

		public static List<StoreItem> list = Grocery.mItemDictionary["All"];

		public static List<StoreItem> list2 = Grocery.mItemDictionary["Home"];

		public float CarryRouteToObjectRadius
		{
			get
			{
				return 0.25f;
			}
		}

		private static ObjectGuid OnCreateCondomPack(object customData, ref Simulator.ObjectInitParameters initData, Quality quality)
		{
			IGameObject gameObject = GlobalFunctions.CreateObjectOutOfWorld(PackResourceKey);
			if (gameObject == null)
			{
				return ObjectGuid.InvalidObjectGuid;
			}
			return gameObject.ObjectId;
		}

		private static void OnProcessCondomPack(object customData, IGameObject item)
		{
		}

		public void CondomPackInStore()
		{
			if (list != null && !list.Contains(item))
			{
				list.Add(item);
			}
			if (list2 != null && !list2.Contains(item))
			{
				list2.Add(item);
			}
		}

		public override bool StacksWith(IGameObject other)
		{
			return other is CondomPack && base.StacksWith(other);
		}

		public override void OnStartup()
		{
			base.OnStartup();
			AddComponent<ItemComponent>(new object[1] { ItemComponent.SimInventoryItem });
			RemoveAllInteractions();
			AddInteraction(PutInInventory.Singleton);
			CondomPackInStore();
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
