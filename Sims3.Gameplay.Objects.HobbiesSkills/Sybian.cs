using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CustomContent;

namespace Sims3.Gameplay.Objects.HobbiesSkills
{
	public class Sybian : GameObject, IHasRouteRadius, IGameObject, IScriptObject, IScriptLogic, IHasScriptProxy, IObjectUI, IExportableContent, INonPurgeableFromNPCInventory
	{
		public class Pickup : PutInInventory
		{
			public sealed class PickupDefinition : Definition
			{
				protected override string GetInteractionName(Sim actor, IGameObject target, InteractionObjectPair iop)
				{
					return Localization.LocalizeString("Gameplay/Abstracts/ScriptObject/PutInInventory:InteractionName");
				}

				protected override bool Test(Sim actor, IGameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return actor != null && actor.IsHuman && !actor.SimDescription.ChildOrBelow && actor.Inventory != null && base.Test(actor, target, isAutonomous, ref greyedOutTooltipCallback);
				}
			}

			public static readonly PickupDefinition PickupSingleton = new PickupDefinition();
		}

		public new static string sLocalizationKey = "Sims3.Gameplay.Objects.HobbiesSkills".Substring(6).Replace('.', '/') + "/Sybian";

		public static float kEnvironmentToLookAtInterestingnessMultiplier = 0.2f;

		public static LookAtTuning sLookAtTuning = new LookAtTuning();

		public override LookAtTuning LookAtTuning
		{
			get
			{
				return sLookAtTuning;
			}
		}

		public float CarryRouteToObjectRadius
		{
			get
			{
				return 0.45f;
			}
		}

		public new static string LocalizeString(string name, params object[] parameters)
		{
			return Localization.LocalizeString(sLocalizationKey + ":" + name, parameters);
		}

		public override bool PackForMovingOverride()
		{
			return true;
		}

		public override int GetLookAtInterestingness(Sim watcher)
		{
			float environmentScore = GetEnvironmentScore(watcher);
			float num = environmentScore * kEnvironmentToLookAtInterestingnessMultiplier;
			return (int)num;
		}

		public override void OnStartup()
		{
			base.OnStartup();
			AddComponent<ItemComponent>(new object[1] { ItemComponent.SimInventoryItem });
			AddInteraction(PutInInventory.Singleton);
		}
	}
}
