using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Settings;
using Sims3.SimIFace;

namespace S3_Passion.BOE_Lovemaking
{
    [Persistable]
    public class Sequence : IPositionChoice
    {
        public class DialogEntry
        {
            public string Name;

            public Sequence Sequence;

            public static DialogEntry Get(string name, Sequence sequence)
            {
                return new DialogEntry(name, sequence);
            }

            public DialogEntry(string name, Sequence sequence)
            {
                Name = name;
                Sequence = sequence;
            }
        }

        public class DialogListItem
        {
            public string Name;

            public SequenceItem Value;

            public static DialogListItem Get(string name, SequenceItem value)
            {
                return new DialogListItem(name, value);
            }

            public DialogListItem(string name, SequenceItem value)
            {
                Name = name;
                Value = value;
            }
        }

        public string Name;

        public int Tones;

        public int Categories;

        public Dictionary<string, PassionType> SupportedTypes;

        public int MinSims;

        public int MaxSims;

        public bool Repeat;

        public bool Continue;

        protected SequenceItem[] mItems;

        public SequenceItem[] Items
        {
            get
            {
                if (mItems == null)
                {
                    mItems = new SequenceItem[0];
                }
                return mItems;
            }
        }

        public static Sequence Create()
        {
            return Create(string.Empty, 1);
        }

        public static Sequence Create(string initial)
        {
            return Create(initial, 1);
        }

        public static Sequence Create(string initial, int length)
        {
            Sequence sequence = new Sequence();
            if (!string.IsNullOrEmpty(initial))
            {
                sequence.Add(initial, length, 0);
            }
            return sequence;
        }

        public Sequence()
        {
            Name = string.Empty;
            Tones = 1;
            Categories = 255;
            SupportedTypes = new Dictionary<string, PassionType>(PassionBase.LoadedTypes);
            MaxSims = int.MaxValue;
            MinSims = 0;
            Repeat = true;
        }

        public void RestoreItems(List<XML.Node> itemset)
        {
            if (itemset == null || itemset.Count <= 0)
            {
                return;
            }
            mItems = new SequenceItem[itemset.Count];
            MaxSims = int.MaxValue;
            MinSims = 0;
            foreach (XML.Node item in itemset)
            {
                if (string.IsNullOrEmpty(item["Index"]))
                {
                    continue;
                }
                int num = PassionCommon.Int(item["Index"]);
                if (num < 0 || num >= mItems.Length)
                {
                    continue;
                }
                SequenceItem sequenceItem = new SequenceItem(item["Key"], PassionCommon.Long(item["Length"]), num);
                mItems[num] = sequenceItem;
                if (sequenceItem.Position != null)
                {
                    if (sequenceItem.Position.MaxSims < MaxSims)
                    {
                        MaxSims = sequenceItem.Position.MaxSims;
                    }
                    if (sequenceItem.Position.MinSims > MinSims)
                    {
                        MinSims = sequenceItem.Position.MinSims;
                    }
                }
            }
            CleanSupportedTypes();
        }

        public bool IsCategory(int category)
        {
            return category == 1 || Categories == 1 || PassionCommon.Match(category, Categories);
        }

        public bool CanUseWith(PassionType type, int participants, int categories)
        {
            if (type == null)
            {
                return false;
            }
            if (!SupportedTypes.ContainsKey(type.Name))
            {
                return false;
            }
            if (participants > 0 && (participants < MinSims || participants > MaxSims))
            {
                return false;
            }
            if (categories != 0 && !IsCategory(categories))
            {
                return false;
            }
            return true;
        }

        public bool CanUseWith(int participants)
        {
            return participants >= MinSims && participants <= MaxSims;
        }

        public bool CanUseWith(PassionType type)
        {
            return type != null && SupportedTypes.ContainsKey(type.Name);
        }

        public static void CleanSequences()
        {
            foreach (Sequence sequence in PassionBase.Sequences)
            {
                sequence.CleanSupportedTypes();
            }
        }

        public void CleanSupportedTypes()
        {
            Dictionary<string, PassionType> dictionary = new Dictionary<string, PassionType>(PassionBase.LoadedTypes);
            SequenceItem[] items = Items;
            foreach (SequenceItem sequenceItem in items)
            {
                if (!sequenceItem.IsValid)
                {
                    continue;
                }
                Position position = sequenceItem.Position;
                foreach (PassionType item in new List<PassionType>(dictionary.Values))
                {
                    if (!position.SupportedTypes.ContainsKey(item.Name))
                    {
                        dictionary.Remove(item.Name);
                    }
                }
            }
            SupportedTypes = dictionary;
        }

        public void Add(string key, long length, int index)
        {
            Add(new SequenceItem(key, length, index));
        }

        public void Add(SequenceItem item)
        {
            if (item == null)
            {
                return;
            }
            Position position = item.Position;
            if (position != null)
            {
                Categories &= position.Categories;
                if (position.MaxSims < MaxSims)
                {
                    MaxSims = position.MaxSims;
                }
                if (position.MinSims > MinSims)
                {
                    MinSims = position.MinSims;
                }
            }
            int num = Items.Length;
            SequenceItem[] array = new SequenceItem[Items.Length + 1];
            for (int i = 0; i < num; i++)
            {
                array[i] = Items[i];
            }
            item.Index = num;
            array[num] = item;
            mItems = array;
            CleanSupportedTypes();
        }

        public void Set(SequenceItem item, int index)
        {
            if (index >= 0 && Items.Length > index)
            {
                Items[index] = item;
                if (item != null)
                {
                    item.Index = index;
                }
            }
        }

        public void Swap(int s, int t)
        {
            int num = Items.Length;
            if (s >= 0 && t >= 0 && s < num && t < num)
            {
                SequenceItem item = Items[s];
                SequenceItem item2 = Items[t];
                Set(item, t);
                Set(item2, s);
            }
        }

        public void MoveUp(SequenceItem item)
        {
            if (item != null)
            {
                MoveUp(item.Index);
            }
        }

        public void MoveUp(int s)
        {
            if (s > 0)
            {
                Swap(s, s - 1);
            }
        }

        public void MoveDown(SequenceItem item)
        {
            if (item != null)
            {
                MoveDown(item.Index);
            }
        }

        public void MoveDown(int s)
        {
            if (s < Items.Length - 1)
            {
                Swap(s, s + 1);
            }
        }

        public void Clear()
        {
            Categories = 1;
            SupportedTypes = new Dictionary<string, PassionType>(PassionBase.LoadedTypes);
            MaxSims = int.MaxValue;
            MinSims = 0;
            mItems = null;
        }

        public void Remove(int index)
        {
            if (Items.Length >= 1 && index >= 0 && index < Items.Length)
            {
                int num = Items.Length - 1;
                for (int i = index; i < num; i++)
                {
                    Set(Items[i + 1], i);
                }
                if (Items.Length > 1)
                {
                    SequenceItem[] destinationArray = new SequenceItem[num];
                    Array.Copy(Items, destinationArray, num);
                    mItems = destinationArray;
                }
                else
                {
                    mItems = new SequenceItem[0];
                }
            }
        }
    }
}
