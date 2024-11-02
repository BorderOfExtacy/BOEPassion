using System.Collections.Generic;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.HobbiesSkills;
using Sims3.SimIFace;
using Sims3.UI;

namespace S3_Passion
{
	internal sealed class AskToUseSybian : ImmediateInteraction<Sim, Sybian>
	{
		[DoesntRequireTuning]
		private sealed class Definition : ImmediateInteractionDefinition<Sim, Sybian, AskToUseSybian>
		{
			public override string GetInteractionName(Sim actor, Sybian target, InteractionObjectPair interaction)
			{
				return PassionCommon.Localize("S3_Passion.Terms.Suggest") + " " + Passion.Settings.SoloLabel;
			}

			public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
			{
				NumSelectableRows = 1;
				PopulateSimPicker(ref parameters, out listObjs, out headers, CollectPeople(parameters.Actor as Sim, parameters.Actor.LotCurrent), false);
			}

			public override bool Test(Sim actor, Sybian target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return actor.SimDescription.TeenOrAbove && actor.SimDescription.IsHuman && target.UseCount == 0;
			}
		}

		public static readonly InteractionDefinition Singleton = new Definition();

		public override bool Run()
		{
			Sim sim = GetSelectedObject() as Sim;
			sim.InteractionQueue.AddNext(Passion.Interactions.UseObjectForPassion.Singleton.CreateInstance(Target, sim, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
			return true;
		}

		private static List<Sim> CollectPeople(Sim actor, Lot targetLot)
		{
			List<Sim> list = new List<Sim>();
			foreach (Sim sim in targetLot.GetSims())
			{
				if (actor != sim && sim.SimDescription.TeenOrAbove && sim.SimDescription.IsHuman && !sim.LotCurrent.IsWorldLot)
				{
					list.Add(sim);
				}
			}
			return list;
		}
	}
}
