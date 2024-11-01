using System;
using System.Collections.Generic;
using System.Xml;
using S3_Passion;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.ChildAndTeenUpdates;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.Enums;
using Sims3.UI;

namespace BorderOfExtacy
{
    public static class BOECommon
    {
        public const CommodityKind JC_COMMODITYKIND = (CommodityKind)286210343;

        private static XmlDbData mXdb;


        public static void InitMotive ()
        {
            AddEnumValue<CommodityKind>("TraitJapCulture", 286210343);
            LoadMotive();
        }

        	
        private static void ParseEnumValues<T>(string table, string nameCol, string enumCol) where T : struct
        {
            if (!mXdb.Tables.ContainsKey(table))
            {
                return;
            }
            XmlDbTable xmlDbTable = mXdb.Tables[table];
            foreach (XmlDbRow row in xmlDbTable.Rows)
            {
                string @string = row.GetString(nameCol);
                if (string.IsNullOrEmpty(@string))
                {
                    continue;
                }
                string result = row.GetString(enumCol);
                switch (Type.GetTypeCode(Enum.GetUnderlyingType(typeof(T))))
                {
                case TypeCode.Int32:
                {
                    int result2;
                    if (int.TryParse(result, out result2))
                    {
                        AddEnumValue<T>(@string, result2);
                    }
                    break;
                }
                case TypeCode.UInt32:
                {
                    uint result3;
                    if (uint.TryParse(result, out result3))
                    {
                        AddEnumValue<T>(@string, result3);
                    }
                    break;
                }
                case TypeCode.UInt64:
                    if (ParserFunctions.GetHexChars(result.Split('=')[1], out result))
                    {
                        ulong num = Convert.ToUInt64(result, 16);
                        AddEnumValue<T>(@string, num);
                    }
                    break;
                }
            }
        }


        public static void AddEnumValue<T>(string key, object value) where T : struct
        {
            Type typeFromHandle = typeof(T);
            EnumParser value2;
            if (!ParserFunctions.sCaseInsensitiveEnumParsers.TryGetValue(typeFromHandle, out value2))
            {
                value2 = new EnumParser(typeFromHandle, true);
                ParserFunctions.sCaseInsensitiveEnumParsers.Add(typeFromHandle, value2);
            }
            EnumParser value3;
            if (!ParserFunctions.sCaseSensitiveEnumParsers.TryGetValue(typeFromHandle, out value3))
            {
                value3 = new EnumParser(typeFromHandle, false);
                ParserFunctions.sCaseSensitiveEnumParsers.Add(typeFromHandle, value3);
            }
            if (!value2.mLookup.ContainsKey(key.ToLowerInvariant()) && !value3.mLookup.ContainsKey(key))
            {
                value2.mLookup.Add(key.ToLowerInvariant(), value);
                value3.mLookup.Add(key, value);
            }
        }

        public static void AddMotive(Sim sim)
        {
            if (sim != null && !sim.Motives.HasMotive((CommodityKind)286210343))
            {
                if (!sim.mMotiveTuning.ContainsKey(286210343))
                {
                    sim.mMotiveTuning.Add(286210343, MotiveTuning.GetTuning((CommodityKind)286210343));
                }
                sim.Motives.CreateMotive((CommodityKind)286210343);
            }
        }

        public static void LoadMotive()
        {
            XmlDocument xmlDocument = Simulator.LoadXML("ArsilJapCultureMotive");
            if (xmlDocument != null)
            {
                MotiveTuning.LoadTuning(xmlDocument);
            }
        }

    }
}