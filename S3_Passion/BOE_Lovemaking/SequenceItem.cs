using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion.BOE_Core;
using Sims3.SimIFace;

namespace S3_Passion.BOE_Lovemaking
{
    [Persistable]
    public class SequenceItem
    {
        public int Index;

        public long Length;

        public string Key;

        public Position Position
        {
            get
            {
                if (PassionBase.Positions.ContainsKey(Key) && PassionBase.Positions[Key] != null)
                {
                    return PassionBase.Positions[Key];
                }
                foreach (Position value in PassionBase.Positions.Values)
                {
                    if (value != null && Key == value.Clip)
                    {
                        Key = value.Key;
                        return value;
                    }
                }
                return null;
            }
        }

        public bool IsValid
        {
            get
            {
                return Position != null;
            }
        }

        public SequenceItem()
        {
            Key = string.Empty;
            Index = 0;
            Length = 1L;
        }

        public SequenceItem(string key, long length, int index)
        {
            Key = key;
            Length = length;
            Index = index;
        }
    }
}
