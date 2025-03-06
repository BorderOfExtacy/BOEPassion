using System.Collections.Generic;
using Passion.S3_Passion.Utilities;
using Passion.Sims3.Gameplay.Objects.HobbiesSkills;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.SimIFace;
using Sims3.UI;
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion.Interactions
{
    internal sealed class AskToUseSybian : ImmediateInteraction<Sim, Sybian>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, Sybian, AskToUseSybian>
        {
            public override string GetInteractionName(Sim actor, Sybian target, InteractionObjectPair interaction)
            {
                return PassionCommon.Localize("S3_Passion.Terms.Suggest") + " " + PassionCore.Settings.SoloLabel;
            }

            public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters,
                out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers,
                out int numSelectableRows)
            {
                numSelectableRows = 1;
                PopulateSimPicker(ref parameters, out listObjs, out headers,
                    CollectPeople(parameters.Actor as Sim, parameters.Actor.LotCurrent), false);
            }

            public override bool Test(Sim actor, Sybian target, bool isAutonomous,
                ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return actor.SimDescription.TeenOrAbove && actor.SimDescription.IsHuman && target.UseCount == 0;
            }
        }

        public static readonly InteractionDefinition Singleton = new Definition();

        public override bool Run()
        {
            Sim sim = GetSelectedObject() as Sim;
            if (sim != null)
            {
                sim.InteractionQueue.AddNext(PassionCore.Interactions.UseObjectForPassion.Singleton.CreateInstance(
                    Target,
                    sim, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
            }

            return true;
        }

        private static List<Sim> CollectPeople(Sim actor, Lot targetLot)
        {
            List<Sim> sims = new List<Sim>();
            foreach (Sim sim in targetLot.GetSims())
            {
                if (IsValidSim(actor, sim))
                {
                    sims.Add(sim);
                }
            }

            return sims;
        }

        private static bool IsValidSim(Sim actor, Sim sim)
        {
            return actor != sim && sim.SimDescription.TeenOrAbove && sim.SimDescription.IsHuman &&
                   !sim.LotCurrent.IsWorldLot;
        }
    }
}