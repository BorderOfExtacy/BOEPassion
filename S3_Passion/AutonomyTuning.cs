using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
    class PassionAutonomyTuning
    {
        // number that needs to be beaten
        [Tunable] public static int bCheckThreshold;

        // libido value bonuses
        [Tunable] public static int ZeroPercentLibidoBonus;
        [Tunable] public static int OnePercentLibidoBonus;
        [Tunable] public static int TwoPercentLibidoBonus;
        [Tunable] public static int ThreePercentLibidoBonus;
        [Tunable] public static int FourPercentLibidoBonus;
        [Tunable] public static int FivePercentLibidoBonus;
        [Tunable] public static int SixPercentLibidoBonus;
        [Tunable] public static int SevenPercentLibidoBonus;
        [Tunable] public static int EightPercentLibidoBonus;
        [Tunable] public static int NinePercentLibidoBonus;
        [Tunable] public static int TenPercentLibidoBonus;

        //hypersexual bonus
        [Tunable] public static int bHypersexualBonus;

        //default lot score
        [Tunable] public static int bDefaultPublicScore;


        // xml reader for lot types

        public class PassionLots
        {
            public string LotTypeName;
            public bool LotAxis;
            public int LotScore;
        }

        public static List<PassionLots> PassionLotList = new List<PassionLots>();

        public static void LoadPassionLotAutonomy(string fileName)
        {

            XmlDbData xmlDbData = XmlDbData.ReadData("BOE_LotAutonomy");
            if (xmlDbData != null)
            {
                foreach (XmlDbRow row in xmlDbData.Tables["Lot"].Rows)
                {

                    string LotNameXML = row.GetString("LotTypeName");
                    bool LotAxisXML = row.GetBool("LotAxis");
                    int LotScoreXML = row.GetInt("LotScore");

                    CreateLotEntry(LotNameXML, LotAxisXML, LotScoreXML);
                }
            }
            else
            {
                PassionCommon.SystemMessage("girl you fucking broke it (it being the lot autonomy tuning. couldn't read the FUCKING xml)");
            }
        }

        public static void CreateLotEntry(string lottype, bool lotaxis, int lotscore)
        {
            PassionLots lot = new PassionLots();
            lot.LotTypeName = lottype;
            lot.LotAxis = lotaxis;
            lot.LotScore = lotscore;
            PassionLots item = lot;
            PassionLotList.Add(item);
        }

        // xml reader for traits

        public class PassionTraitAutonomy
        {
            public string TraitName;
            public bool TraitAxis;
            public int TraitScore;
        }

        public static List<PassionTraitAutonomy> PassionAutoTraitList = new List<PassionTraitAutonomy>();

        public static void LoadPassionTraitAutonomy(string fileName)
        {

            XmlDbData xmlDbData = XmlDbData.ReadData("BOE_TraitAutonomy");
            if (xmlDbData != null)
            {
                foreach (XmlDbRow row in xmlDbData.Tables["TraitEntry"].Rows)
                {

                    string TraitNameXML = row.GetString("TraitName");
                    bool TraitAxisXML = row.GetBool("TraitAxis");
                    int TraitScoreXML = row.GetInt("TraitScore");

                    CreateTraitAutomyEntry(TraitNameXML, TraitAxisXML, TraitScoreXML);
                }
            }
            else
            {
                PassionCommon.SystemMessage("girl you fucking broke it (it being the lot autonomy tuning. couldn't read the FUCKING xml)");
            }
        }

        public static void CreateTraitAutomyEntry(string traitname, bool traitaxis, int traitscore)
        {
            PassionTraitAutonomy trait = new PassionTraitAutonomy();
            trait.TraitName = traitname;
            trait.TraitAxis = traitaxis;
            trait.TraitScore = traitscore;
            PassionTraitAutonomy item = trait;
            PassionAutoTraitList.Add(item);
        }

        public static void Unload()
        {
            PassionLotList.Clear();
            PassionAutoTraitList.Clear();
        }

    }
}
