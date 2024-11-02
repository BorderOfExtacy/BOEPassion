using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.SimIFace;
using Sims3.SimIFace.CustomContent;

namespace Sims3.Gameplay.Objects
{
	public class SingleCondom : GameObject, IHasRouteRadius, IGameObject, IScriptObject, IScriptLogic, IHasScriptProxy, IObjectUI, IExportableContent, INonPurgeableFromNPCInventory
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
		}

		public static readonly ResourceKey kResourceKey = ResourceKey.FromString("0x319E4F1D-0x00000000-0x000000007FBC752D");

		public float CarryRouteToObjectRadius
		{
			get
			{
				return 0.25f;
			}
		}

		public override void OnStartup()
		{
			base.OnStartup();
			AddComponent<ItemComponent>(new object[1] { ItemComponent.SimInventoryItem });
			RemoveAllInteractions();
			AddInteraction(PutInInventory.Singleton);
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
