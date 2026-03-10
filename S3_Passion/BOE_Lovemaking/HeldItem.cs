using S3_Passion.BOE_Core;
using Sims3.Gameplay.Abstracts;
using Sims3.SimIFace;

namespace S3_Passion.BOE_Lovemaking
{
    [Persistable]
    public class HeldItem
    {
        public enum ItemSlots : uint
        {
            RightHand = 1557334703u,
            LeftHand = 3703456078u,
            RightRing = 4005035538u,
            LeftEarring = 811482909u,
            RightEarring = 1447611514u,
            RightShortSleeveIn = 4009816123u,
            RightShortSleeveOut = 1651179094u,
            RightToe = 3543879632u,
            LeftToe = 2783384923u,
            RightPantsLeg = 3793686282u,
            LeftRing = 3445785831u,
            LeftCarry = 1318179674u,
            L_Bracelet = 2669835805u,
            RightCarry = 646490825u,
            LeftShortSleeveOut = 4158665743u,
            RightAnkle = 732601729u,
            LeftPantsLeg = 3422414079u,
            LeftAnkle = 298896906u,
            RightBracelet = 1848931864u,
            Glasses = 2747942486u,
            LeftShortSleeveIn = 312766088u,
            BackNeck = 1159156198u,
            Mouth = 453825779u,
            Spine0 = 3764909017u,
            PlumbBobSlot = 671527531u,
            HorseSaddle = 2342056608u,
            Dome = 2705289959u,
            None = 0u
        }

        public ResourceKey Key = ResourceKey.kInvalidResourceKey;

        public Vector3 Location = Vector3.Empty;

        public Vector3 Facing = Vector3.Empty;

        public float Angle = 0f;

        public uint Slot = 0u;

        protected ObjectGuid mID = ObjectGuid.InvalidObjectGuid;

        protected GameObject mObject = null;

        protected Player mHoldingPlayer = null;

        public ObjectGuid ID
        {
            get
            {
                return mID;
            }
            set
            {
                mID = value;
            }
        }

        public GameObject Object
        {
            get
            {
                if (mObject == null && Key != ResourceKey.kInvalidResourceKey && HoldingPlayer != null && HoldingPlayer.IsValid)
                {
                    GenerateObject();
                }
                return mObject;
            }
            set
            {
                Destroy();
                mObject = value;
                if (mObject != null)
                {
                    mID = mObject.ObjectId;
                }
            }
        }

        public Player HoldingPlayer
        {
            get
            {
                return mHoldingPlayer;
            }
            set
            {
                mHoldingPlayer = value;
                if (mObject != null)
                {
                    Slots.AttachToSlot(mID, HoldingPlayer.Actor.ObjectId, Slot, false, ref Location, ref Facing, Angle);
                }
                else
                {
                    GenerateObject();
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return Key != ResourceKey.kInvalidResourceKey && Object != null && !Object.HasBeenDestroyed;
            }
        }

        public void Copy(Player player)
        {
            if (player != null)
            {
                Create(player, Key, Location, Facing, Angle, Slot);
            }
        }

        public static HeldItem Create(Player player, ResourceKey key, Vector3 location, Vector3 facing, float angle, uint slot)
        {
            if (player != null)
            {
                HeldItem heldItem = Create(key, location, facing, angle, slot);
                heldItem.HoldingPlayer = player;
                player.HeldItem = heldItem;
                return heldItem;
            }
            return null;
        }

        public static HeldItem Create(string objectkey, Vector3 location, Vector3 facing, float angle, uint slot)
        {
            ResourceKey key = ResourceKey.kInvalidResourceKey;
            try
            {
                key = ResourceKey.FromString(objectkey);
            }
            catch
            {
            }
            return Create(key, location, facing, angle, slot);
        }

        public static HeldItem Create(ResourceKey key, Vector3 location, Vector3 facing, float angle, uint slot)
        {
            HeldItem heldItem = new HeldItem();
            heldItem.Key = key;
            heldItem.Location = location;
            heldItem.Facing = facing;
            heldItem.Angle = angle;
            heldItem.Slot = slot;
            return heldItem;
        }

        public void Grab()
        {
            if (mObject != null)
            {
                Destroy();
            }
            GenerateObject();
        }

        protected void GenerateObject()
        {
            try
            {
                Simulator.ObjectInitParameters initData = new Simulator.ObjectInitParameters(0uL, Location, 0, Facing, HiddenFlags.Nothing);
                mID = Simulator.CreateObject(Key, null, initData);
                IScriptProxy proxy = Simulator.GetProxy(mID);
                mObject = proxy != null ? proxy.Target as GameObject : null;
                Slots.AttachToSlot(mID, HoldingPlayer.Actor.ObjectId, Slot, false, ref Location, ref Facing, Angle);
            }
            catch
            {
                if (PassionCommon.Testing)
                {
                    PassionCommon.SystemMessage("Error generating Held Item.");
                }
            }
        }

        public void Release()
        {
            Destroy();
        }

        protected void Destroy()
        {
            try
            {
                ObjectGuid objectId = mID;
                if (mObject != null)
                {
                    objectId = mObject.ObjectId;
                }
                if (objectId != ObjectGuid.InvalidObjectGuid)
                {
                    World.RemoveObjectFromObjectManager(objectId);
                    Simulator.DestroyObject(objectId);
                    mObject = null;
                }
            }
            catch
            {
            }
        }
    }
}
