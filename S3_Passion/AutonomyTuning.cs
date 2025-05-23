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
        [Tunable] public static int CheckThreshold;

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

        // xml reader for lot types

        public class PassionLots
        {
            public string LotTypeName;
            public int LotTypeEnum;
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
                    int LotEnumXML = row.GetInt("LotTypeEnum");
                    bool LotAxisXML = row.GetBool("LotAxis");
                    int LotScoreXML = row.GetInt("LotScore");

                    CreateLotEntry(LotNameXML, LotEnumXML, LotAxisXML, LotScoreXML);
                }
            }
            else
            {
                PassionCommon.SystemMessage("girl you fucking broke it (it being the lot autonomy tuning. couldn't read the FUCKING xml)");
            }
        }

        public static void CreateLotEntry(string lottype, int lotenum, bool lotaxis, int lotscore)
        {
            PassionLots lot = new PassionLots();
            lot.LotTypeEnum = lotenum;
            lot.LotAxis = lotaxis;
            lot.LotScore = lotscore;
            PassionLots item = lot;
            PassionLotList.Add(item);
        }

        public static void Unload()
        {
            PassionLotList.Clear();
        }

    }
}
