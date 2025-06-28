using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.CAS;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;

namespace S3_Passion
{
    class PenisInspection
    {



        public static void GetBottomCASP (Sim sim)
        {

            SimDescription simDescription = sim.SimDescription;


            CASPart[] simlist = simDescription.GetOutfit(OutfitCategories.Naked, 0).Parts;

            OutfitParsePreLoop(simlist);

        }

        public static void OutfitParsePreLoop(CASPart[] parts)
        {
            try
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    OutfitParseLoop(parts[i]);
                }
            }
            catch
            {
                PassionCommon.SystemMessage("it spoded on preloop :(");
            }
        }

        public static void OutfitParseLoop(CASPart part)
        {
            try
            {
               if (part.BodyType == BodyTypes.LowerBody)
                {

                   ResourceKey coolhash = part.Key;
                   PassionCommon.SystemMessage("bottom found! its hash is \n" + coolhash);
                }
               else
                {
                    PassionCommon.SystemMessage("part parsed isnt lower bodypart, skipping");
                }
            }
            catch
            {
                PassionCommon.SystemMessage("it spoded on loop :(");
            }
        }



    }
}
