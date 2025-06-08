using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using S3_Passion;
using Sims3.Gameplay.Pools;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;

namespace S3_Passion
{
    class PassionBody
    {

        public class BodyShop
        {
            public string Name;
            public string GenitalType;
            public string BaseCASP;
            public string ErectSIMO;
        }

        public static List<BodyShop> coolbodyshop = new List<BodyShop>();

        public static void LoadBodies(string fileName)
        {

            XmlDbData xmlDbData = XmlDbData.ReadData("BOE_Bodies");
                    if (xmlDbData != null)
                    {
                        foreach (XmlDbRow row in xmlDbData.Tables["Bodypart"].Rows)
                        {

                            string PartName = row.GetString("Name");
                            string PartType = row.GetString("GenitalType");
                            string PartCASP = row.GetString("BaseCASP");
                            string PartSIMO = row.GetString("ErectSIMO");

                            CreateBodyEntry(PartName, PartType, PartCASP, PartSIMO);
                }
            }
            else
            {
                PassionCommon.SystemMessage("girl you fucking broke it (it being the body selector. couldn't read the FUCKING xml)");
            }
        }

        public static void CreateBodyEntry(string name, string genitaltype, string basecasp, string erectsimo)
        {
                BodyShop body = new BodyShop();
                body.Name = name;
                body.GenitalType = genitaltype;
                body.BaseCASP = basecasp;
                body.ErectSIMO = erectsimo;
                BodyShop item = body;
                coolbodyshop.Add(item);
        }

        public static void Unload()
        {
            coolbodyshop.Clear();
        }

    }


    }
