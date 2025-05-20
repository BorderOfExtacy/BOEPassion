using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;

namespace S3_Passion
{
    class AddMarkers
    {

        public static void AsexualToggle(Sim sim, bool AddOrRemove)
        {
            if (AddOrRemove)
            {
                sim.TraitManager.AddElement((TraitNames)6177560411462291097uL);
                PassionCommon.SystemMessage("imdyingsquirtle");
            }
            else
            {
                sim.TraitManager.RemoveElement((TraitNames)6177560411462291097uL);
                PassionCommon.SystemMessage("bulnosaur");
            }
        }

        public static void AbstinentToggle(Sim sim, bool AddOrRemove)
        {
            if (AddOrRemove)
            {
                sim.TraitManager.AddElement((TraitNames)2214287488174702228uL);
                PassionCommon.SystemMessage("imdyingsquirtle");
            }
            else
            {
                sim.TraitManager.RemoveElement((TraitNames)2214287488174702228uL);
                PassionCommon.SystemMessage("bulnosaur");
            }
        }

        public static void HypersexualToggle(Sim sim, bool AddOrRemove)
        {
            if (AddOrRemove)
            {
                sim.TraitManager.AddElement((TraitNames)5711695705602619160uL);
                PassionCommon.SystemMessage("imdyingsquirtle");
            }
            else
            {
                sim.TraitManager.RemoveElement((TraitNames)5711695705602619160uL);
                PassionCommon.SystemMessage("bulnosaur");
            }
        }

    }
}
