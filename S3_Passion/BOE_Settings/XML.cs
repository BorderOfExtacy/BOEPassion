using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using S3_Passion;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_UI;
using Sims3.Gameplay;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI.GameEntry;

namespace S3_Passion.BOE_Settings
{
    public class XML
    {
        public class File
        {
            protected XmlDocument mHandle;

            public XmlDocument Handle
            {
                get
                {
                    return mHandle;
                }
                set
                {
                    mHandle = value;
                }
            }

            public bool IsValid
            {
                get
                {
                    return Handle != null;
                }
            }

            public Node this[string id]
            {
                get
                {
                    if (IsValid && !string.IsNullOrEmpty(id))
                    {
                        foreach (XmlNode childNode in Handle.ChildNodes)
                        {
                            if (childNode.NodeType == XmlNodeType.Element && childNode.Name == id)
                            {
                                return Node.Create(childNode);
                            }
                        }
                    }
                    return null;
                }
            }

            public static File Read(string filename)
            {
                ResourceKey key = ResourceKey.CreateXMLKey(filename, 0u);
                return Read(key);
            }

            public static File Read(ResourceKey key)
            {
                if (key != ResourceKey.kInvalidResourceKey)
                {
                    XmlDocument handle = Simulator.LoadFromResourceKey(key);
                    return new File(handle);
                }
                return new File();
            }

            public File()
            {
            }

            public File(XmlDocument handle)
            {
                Handle = handle;
            }
        }

        public class Node
        {
            protected XmlNode mHandle;

            protected Dictionary<string, string> mValues = new Dictionary<string, string>();

            public XmlNode Handle
            {
                get
                {
                    return mHandle;
                }
                set
                {
                    mHandle = value;
                    if (mHandle == null)
                    {
                        return;
                    }
                    mValues.Clear();
                    foreach (XmlNode childNode in mHandle.ChildNodes)
                    {
                        if (childNode.NodeType == XmlNodeType.Element)
                        {
                            if (mValues.ContainsKey(childNode.Name))
                            {
                                mValues[childNode.Name] = childNode.InnerText;
                            }
                            else
                            {
                                mValues.Add(childNode.Name, childNode.InnerText);
                            }
                        }
                    }
                }
            }

            public bool IsValid
            {
                get
                {
                    return mHandle != null;
                }
            }

            public string Value
            {
                get
                {
                    if (IsValid)
                    {
                        return mHandle.InnerText;
                    }
                    return string.Empty;
                }
            }

            public string this[string key]
            {
                get
                {
                    if (!string.IsNullOrEmpty(key) && mValues.ContainsKey(key))
                    {
                        return mValues[key];
                    }
                    return string.Empty;
                }
            }

            public static Node Create(XmlNode handle)
            {
                return new Node(handle);
            }

            public Node()
            {
            }

            public Node(XmlNode handle)
            {
                Handle = handle;
            }

            public string GetAttribute(string name)
            {
                if (Handle != null)
                {
                    XmlAttributeCollection attributes = Handle.Attributes;
                    if (attributes != null)
                    {
                        XmlAttribute xmlAttribute = (XmlAttribute)attributes.GetNamedItem(name);
                        if (xmlAttribute != null)
                        {
                            return xmlAttribute.Value;
                        }
                    }
                }
                return string.Empty;
            }

            public Node GetMatchingNode(string key)
            {
                if (IsValid && !string.IsNullOrEmpty(key))
                {
                    foreach (XmlNode childNode in mHandle.ChildNodes)
                    {
                        if (childNode.NodeType == XmlNodeType.Element && childNode.Name == key)
                        {
                            return Create(childNode);
                        }
                    }
                }
                return null;
            }

            public List<Node> GetMatchingNodes(string key)
            {
                List<Node> list = new List<Node>();
                if (IsValid && !string.IsNullOrEmpty(key))
                {
                    foreach (XmlNode childNode in mHandle.ChildNodes)
                    {
                        if (childNode.NodeType == XmlNodeType.Element && childNode.Name == key)
                        {
                            list.Add(Create(childNode));
                        }
                    }
                }
                return list;
            }
        }

        public class Attribute
        {
            private string Name = string.Empty;

            private string Value = string.Empty;

            public Attribute(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string ToXML()
            {
                return Name + "=\"" + Value + "\"";
            }
        }

        public class Element
        {
            public string Name = string.Empty;

            public string Value = string.Empty;

            public bool IsComment = false;

            protected List<Attribute> mAttributes = new List<Attribute>();

            protected List<Element> mElements = new List<Element>();

            public List<Attribute> Attributes
            {
                get
                {
                    if (mAttributes == null)
                    {
                        mAttributes = new List<Attribute>();
                    }
                    return mAttributes;
                }
            }

            public List<Element> ChildElements
            {
                get
                {
                    if (mElements == null)
                    {
                        mElements = new List<Element>();
                    }
                    return mElements;
                }
            }

            public static Element Create(string name)
            {
                return Create(name, string.Empty, null);
            }

            public static Element Create(string name, List<Attribute> attributes)
            {
                return Create(name, string.Empty, attributes);
            }

            public static Element Create(string name, string value)
            {
                return Create(name, value, null);
            }

            public static Element Create(string name, string value, List<Attribute> attributes)
            {
                Element element = new Element();
                element.Name = name;
                element.Value = value;
                if (attributes != null)
                {
                    element.Attributes.AddRange(attributes);
                }
                return element;
            }

            public Element AddChild(string name)
            {
                return AddChild(name, string.Empty);
            }

            public Element AddChild(string name, string value)
            {
                return AddChild(Create(name, value));
            }

            public Element AddChild(Element child)
            {
                if (child != null)
                {
                    ChildElements.Add(child);
                }
                return child;
            }

            public Element AddComment(string text)
            {
                Element element = AddChild("Comment", text);
                element.IsComment = true;
                return element;
            }

            public Attribute AddAttribute(string key, string value)
            {
                Attribute attribute = null;
                if (!string.IsNullOrEmpty(key))
                {
                    attribute = new Attribute(key, value);
                    Attributes.Add(attribute);
                }
                return attribute;
            }

            public string ToXML()
            {
                return ToXML(string.Empty);
            }

            protected string ToXML(string depth)
            {
                string empty = string.Empty;
                string depth2 = depth + "  ";
                if (IsComment)
                {
                    return depth + "<!-- " + Value + " -->";
                }
                empty = empty + depth + "<" + Name;
                if (Attributes.Count > 0)
                {
                    foreach (Attribute attribute in Attributes)
                    {
                        empty = empty + " " + attribute.ToXML();
                    }
                }
                empty += ">";
                if (ChildElements.Count > 0)
                {
                    empty += PassionCommon.NewLine;
                    foreach (Element childElement in ChildElements)
                    {
                        empty += childElement.ToXML(depth2);
                    }
                    empty += depth;
                }
                else
                {
                    empty += Value;
                }
                return empty + "</" + Name + ">" + PassionCommon.NewLine;
            }
        }

        public const string XMLDeclaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        public const string PassionPrefix = "PassionSettingsExport_";

        public static bool WriteToPackage(Element root)
        {
            return WriteToPackage(root, null);
        }

        public static bool WriteToPackage(Element root, string name)
        {
            if (root != null)
            {
                string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + PassionCommon.NewLine;
                text += root.ToXML();
                if (string.IsNullOrEmpty(name))
                {
                    bool flag = true;
                    string text2 = null;
                    name = string.Empty;
                    while (flag)
                    {
                        try
                        {
                            flag = false;
                            text2 = PickString.Show(PassionCommon.Localize("S3_Passion.Terms.ExportFile"), PassionCommon.Localize("S3_Passion.Terms.ExportFileText"), DateTime.Now.ToString("MM-dd-yyyy (hh:mm)"));
                            if (string.IsNullOrEmpty(text2))
                            {
                                return false;
                            }
                            name = "PassionSettingsExport_" + text2;
                            BinModel.Singleton.PopulateExportBin();
                            foreach (IExportBinContents item in new List<IExportBinContents>(BinModel.Singleton.ExportBinContents))
                            {
                                if (item != null && !string.IsNullOrEmpty(item.HouseholdName) && item.HouseholdName.ToLower() == name.ToLower())
                                {
                                    BinModel.Singleton.DeleteFromExportBin(item.ContentId);
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    BinModel.Singleton.PopulateExportBin();
                    foreach (IExportBinContents item2 in new List<IExportBinContents>(BinModel.Singleton.ExportBinContents))
                    {
                        if (item2 != null && !string.IsNullOrEmpty(item2.HouseholdName) && item2.HouseholdName.ToLower() == name.ToLower())
                        {
                            BinModel.Singleton.DeleteFromExportBin(item2.ContentId);
                            break;
                        }
                    }
                }
                Household household = Household.Create();
                household.SetName(name);
                household.BioText = text;
                BinModel.Singleton.AddToExportBin(household);
                household.Destroy();
                return true;
            }
            return false;
        }

        public static File ReadFromPackage()
        {
            return ReadFromPackage(null);
        }

        public static File ReadFromPackage(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                bool flag = true;
                bool flag2 = false;
                while (flag)
                {
                    BinModel.Singleton.PopulateExportBin();
                    GenericDialog.OptionList<IExportBinContents> optionList = new GenericDialog.OptionList<IExportBinContents>();
                    foreach (IExportBinContents item in new List<IExportBinContents>(BinModel.Singleton.ExportBinContents))
                    {
                        if (item != null && item.HouseholdName != null && item.HouseholdName.StartsWith("PassionSettingsExport_"))
                        {
                            optionList.Add(item.HouseholdName.Replace("PassionSettingsExport_", string.Empty), item);
                        }
                    }
                    if (optionList.Count > 0)
                    {
                        flag2 = true;
                        IExportBinContents exportBinContents = GenericDialog.Ask(optionList, PassionCommon.Localize("S3_Passion.Terms.ImportFile"), true);
                        if (exportBinContents == null)
                        {
                            break;
                        }
                        GenericDialog.OptionList<string> optionList2 = new GenericDialog.OptionList<string>();
                        optionList2.Add(PassionCommon.Localize("S3_Passion.Terms.Ok"), "Ok");
                        optionList2.Add(PassionCommon.Localize("S3_Passion.Terms.Remove"), "Remove");
                        string text = GenericDialog.Ask(optionList2, "\"" + exportBinContents.HouseholdName + "\"");
                        if (!(text == "Remove"))
                        {
                            if (text == "Ok")
                            {
                                XmlDocument xmlDocument = new XmlDocument();
                                xmlDocument.LoadXml(exportBinContents.HouseholdBio);
                                return new File(xmlDocument);
                            }
                        }
                        else
                        {
                            BinModel.Singleton.DeleteFromExportBin(exportBinContents.ContentId);
                        }
                        continue;
                    }
                    if (!flag2)
                    {
                        PassionCommon.SystemMessage(PassionCommon.Localize("S3_Passion.Terms.NoFilesFound"));
                    }
                    break;
                }
            }
            else
            {
                BinModel.Singleton.PopulateExportBin();
                foreach (IExportBinContents item2 in new List<IExportBinContents>(BinModel.Singleton.ExportBinContents))
                {
                    if (item2 != null && !string.IsNullOrEmpty(item2.HouseholdName) && item2.HouseholdName.ToLower() == name.ToLower())
                    {
                        XmlDocument xmlDocument2 = new XmlDocument();
                        xmlDocument2.LoadXml(item2.HouseholdBio);
                        return new File(xmlDocument2);
                    }
                }
            }
            return null;
        }

        public static bool WriteToFile(Element root, string prefix)
        {
            try
            {
                if (root != null)
                {
                    uint fileHandle = 0u;
                    Simulator.CreateExportFile(ref fileHandle, prefix);
                    if (fileHandle != 0)
                    {
                        CustomXmlWriter customXmlWriter = new CustomXmlWriter(fileHandle);
                        customXmlWriter.WriteToBuffer("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + PassionCommon.NewLine);
                        customXmlWriter.WriteToBuffer(root.ToXML());
                        customXmlWriter.WriteEndDocument();
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        public static File Create(string filename)
        {
            return File.Read(filename);
        }
    }
}
