using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion.BOE_Lovemaking;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion.BOE_Lovemaking
{
    [Persistable]
    public class SequenceInstance
    {
        protected int mCurrent = 0;

        protected long mStart = 0L;

        public string Name = string.Empty;

        public bool Repeat = true;

        public bool Continue = false;

        protected SequenceItem[] mItems;

        public Position First
        {
            get
            {
                if (mItems.Length != 0)
                {
                    return mItems[0].Position;
                }
                return null;
            }
        }

        public Position Current
        {
            get
            {
                if (mCurrent < mItems.Length)
                {
                    return mItems[mCurrent].Position;
                }
                return null;
            }
        }

        public bool Started
        {
            get
            {
                return mStart > 0;
            }
        }

        public static SequenceInstance Create(Sequence sequence)
        {
            SequenceInstance sequenceInstance = new SequenceInstance();
            if (sequence != null)
            {
                sequenceInstance.mItems = new SequenceItem[sequence.Items.Length];
                Array.Copy(sequence.Items, sequenceInstance.mItems, sequence.Items.Length);
                sequenceInstance.Name = sequence.Name;
                sequenceInstance.Repeat = sequence.Repeat;
                sequenceInstance.Continue = sequence.Continue;
            }
            return sequenceInstance;
        }

        public Position Start()
        {
            mStart = SimClock.CurrentTicks;
            return First;
        }

        public Position Next()
        {
            Position result = null;
            if (mCurrent < mItems.Length && mItems[mCurrent] != null)
            {
                if (SimClock.CurrentTicks > mStart + mItems[mCurrent].Length)
                {
                    mStart = SimClock.CurrentTicks;
                    mCurrent++;
                }
                if (Repeat && mCurrent >= mItems.Length)
                {
                    mCurrent = 0;
                }
                if (mCurrent < mItems.Length && mItems[mCurrent] != null)
                {
                    result = mItems[mCurrent].Position;
                }
            }
            else
            {
                mCurrent = 0;
            }
            return result;
        }
    }
}
