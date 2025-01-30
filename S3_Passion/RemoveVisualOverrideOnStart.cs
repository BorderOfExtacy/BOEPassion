using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
	public class RemoveVisualOverrideOnStart : ImmediateInteraction<Sim, Sim>
	{
		[DoesntRequireTuning]
		private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, RemoveVisualOverrideOnStart>
		{
			protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString(actor.IsFemale, "Remove Pale Skin", target.SimDescription, actor.SimDescription);
			}

			public override string[] GetPath(bool isFemale)
			{
				return new string[1] { Localization.LocalizeString(isFemale, "Vampire...") };
			}

			protected override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return true;
			}
		}

		public static readonly InteractionDefinition Singleton = new Definition();

		protected override bool Run()
		{
			try
			{
				if (Target.SimDescription.IsVampire)
				{
					Target.InteractionQueue.CancelAllInteractions();
					World.ObjectSetVisualOverride(Target.ObjectId, eVisualOverrideTypes.None, null);
					Target.SimDescription.OccultManager.UpdateOccultUI();
					World.ObjectSetVisualOverride(Target.ObjectId, eVisualOverrideTypes.None, null);
					Target.SimDescription.OccultManager.UpdateOccultUI();
				}
			}
			catch
			{
			}
			return true;
		}
	}
}
