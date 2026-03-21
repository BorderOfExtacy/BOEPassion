using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Lovemaking;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.CAS;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;

namespace S3_Passion.BOE_Debug
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



    




    public static void GetBottomCASP2(Sim sim, Player player)
        {

            SimDescription simDescription = sim.SimDescription;


            CASPart[] simlist = simDescription.GetOutfit(OutfitCategories.Naked, 0).Parts;

            OutfitParsePreLoop2(simlist, player);

        }

        public static void OutfitParsePreLoop2(CASPart[] parts, Player player)
        {
            try
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    OutfitParseLoop2(parts[i], player);
                }
            }
            catch
            {
                PassionCommon.SystemMessage("it spoded on preloop :(");
            }
        }

        public static void OutfitParseLoop2(CASPart part, Player player)
        {
            try
            {
                if (part.BodyType == BodyTypes.UpperBody)
                {

                    ResourceKey coolhash = part.Key;

                    player.nudeTopRK = coolhash.ToString();
                    //PassionCommon.SystemMessage("bottom found! its hash is \n" + coolhash);
                }
                else
                {
                    //PassionCommon.SystemMessage("part parsed isnt lower bodypart, skipping");
                }
            }
            catch
            {
                PassionCommon.SystemMessage("it spoded on loop :(");
            }
        }



    }



}
