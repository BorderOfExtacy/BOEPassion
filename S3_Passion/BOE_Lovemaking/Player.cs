using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Debug;
using S3_Passion.BOE_Interaction;
using S3_Passion.BOE_Settings;
using S3_Passion.BOE_Tunables;
using S3_Passion.BOE_UI;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Pools;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Situations;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.ThoughtBalloons;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.SimIFace.RouteDestinations;
using Sims3.SimIFace.VideoRecording;
using Sims3.UI.CAS;

namespace S3_Passion.BOE_Lovemaking
{
    [Persistable]
    public class Player
    {
        public ulong ID;

        public PassionState State;

        public object SimStrapon = PassionGenitals.SimStraponList.UNSET;

        public string SimGenitalType;

        public string SimJunkBaseCASP;

        public string SimErectSIMO;

        public bool IsActive;

        public bool IsInPlace;

        public bool IsAutonomous;

        public bool DirectTargeted;

        public bool ActiveLeave;

        public bool ActiveJoin;

        public bool SpinDisabled;

        public bool ForceNoStrap;

        public bool HasPreferredOutfit;

        public bool CanAnimate;

        public bool AutonomyIsDisabled;

        public bool CanSwitch;

        public bool IsStartingSesh;

        public bool AreWeSwitching;

        public bool StrapIsOn;

        public string nudeTopRK;

        public bool IsNaked;

        public bool PeenIsErect;

        public bool CancelledOnTwitterDotCom;

        public long StartTime;

        public int NumberAccepted;

        public Part Part;

        public int PositionIndex;

        public int PassionCharge;

        public string BufferedAnimation;

        public string SwitchRoleBuffer;

        public string UndressLevel;

        public string BufferedTargetAnimation;

        public string BufferedObjectAnimation;

        public HeldItem HeldItem;

        public HeldItem HeldItemWithNoKey;

        public Player Partner;

        public Part SwitchPart;

        public Vector3 ExitPoint = Vector3.Empty;

        public OutfitCategories PreferredOutfitCategory;

        public OutfitCategories PreviousOutfitCategory;

        public RejectionReasons RejectionReason;

        public int PreferredOutfitIndex;

        public bool SwitchBuffer;

        public int PreviousOutfitIndex;

        public int PartnersToCheckCount = 0;

        public List<Player> PotentialImpregnators = new List<Player>();

        public List<Player> PartnersUpdated = new List<Player>();

        public List<Player> PartnersToCheck = new List<Player>();

        protected ReactionBroadcaster JealousyBroadcaster;

        protected Sim mActor;

        public SimDescription mDescription;

        protected float mHeightModifier = 0f;

        public Sim.SwitchOutfitHelper mSwitchOutfitHelper;

        public Sim Actor
        {
            get
            {
                if (mActor == null && mDescription != null)
                {
                    mActor = mDescription.CreatedSim;
                }
                return mActor;
            }
            set
            {
                mActor = value;
                if (mActor != null)
                {
                    mDescription = mActor.SimDescription;
                    ID = mActor.ObjectId.Value;
                }
                else
                {
                    mDescription = null;
                    ID = 0uL;
                }
            }
        }

        public SimDescription Description
        {
            get
            {
                if (mDescription == null && mActor != null)
                {
                    mDescription = mActor.SimDescription;
                }
                return mDescription;
            }
            set
            {
                mDescription = value;
                if (mDescription != null)
                {
                    mActor = mDescription.CreatedSim;
                    ID = mActor.ObjectId.Value;
                }
                else
                {
                    mActor = null;
                    ID = 0uL;
                }
            }
        }

        public float HeightModifier
        {
            get
            {
                return mHeightModifier;
            }
        }

        public string Name
        {
            get
            {
                if (IsValid)
                {
                    return Actor.Name;
                }
                return "Player (No Actor)";
            }
        }

        public uint Flags
        {
            get
            {
                if (IsValid)
                {
                    return Actor.SimDescription.SimFlags;
                }
                return 0u;
            }
        }

        public Vector3 Location
        {
            get
            {
                if (IsValid)
                {
                    return Actor.Position;
                }
                return Vector3.Empty;
            }
            set
            {
                if (IsValid)
                {
                    Actor.SetPosition(value);
                }
            }
        }

        public Vector3 Forward
        {
            get
            {
                if (IsValid)
                {
                    return Actor.ForwardVector;
                }
                return Vector3.Empty;
            }
            set
            {
                if (IsValid)
                {
                    Actor.SetForward(value);
                }
            }
        }

        public bool ActiveLeaveJoin
        {
            get
            {
                return ActiveLeave && ActiveJoin;
            }
            set
            {
                ActiveLeave = value;
                ActiveJoin = value;
            }
        }

        public OutfitCategories Outfit
        {
            get
            {
                if (IsValid)
                {
                    return Actor.CurrentOutfitCategory;
                }
                return OutfitCategories.None;
            }
            set
            {
                if (!IsNaked)
                {

                    SetOutfit(value);
                    IsNaked = true;

                }
            }
        }

        public Player PlayerInteractedWith
        {
            get
            {
                Player result = null;
                if (HasPart && Part.HasPosition && Part.Position.InteractsWith.ContainsKey(PositionIndex))
                {
                    foreach (Player value in Part.Players.Values)
                    {
                        if (value != null && value.PositionIndex == Part.Position.InteractsWith[PositionIndex])
                        {
                            result = value;
                            break;
                        }
                    }
                }
                return result;
            }
        }

        public List<Player> Partners
        {
            get
            {
                List<Player> list = new List<Player>();
                if (HasPart)
                {
                    foreach (Player value in Part.Players.Values)
                    {
                        if (value != this)
                        {
                            list.Add(value);
                        }
                    }
                }
                return list;
            }
        }

        public bool HadPartner
        {
            get
            {
                return Partner != null && Partner.IsValid;
            }
        }

        public bool IsValid
        {
            get
            {
                return Actor != null && !Actor.HasBeenDestroyed && !Actor.TraitManager.HasElement((TraitNames)2214287488174702228uL);
            }
        }

        public bool IsInitiator
        {
            get
            {
                if (IsValid && HasPart && Part.Initiator.IsValid && Part.Initiator.Actor == Actor)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsStopping
        {
            get
            {
                return State == PassionState.Stopping || State == PassionState.Leaving;
            }
        }

        public bool IsWatching
        {
            get
            {
                return State == PassionState.Watching;
            }
        }

        public bool IsSolo
        {
            get
            {
                return IsValid && IsActive && HasPart && Part.Players.Count == 1;
            }
        }

        public bool IsInValidObject
        {
            get
            {
                return !(Actor.CurrentInteraction.Target is Sim) && Actor.CurrentInteraction.Target.IsActorUsingMe(Actor) && PassionType.IsSupported(Actor.CurrentInteraction.Target);
            }
        }

        public bool IsInPool
        {
            get
            {
                if (IsValid && Actor.Posture is SwimmingInPool)
                {
                    return true;
                }
                return false;
            }
        }

        // if youre dying you cant fuck :pensive:
        public bool IsInMotiveDesperation
        {
            get
            {
                if (IsValid && Actor.Motives != null && Actor.Motives.InMotiveDistress && (PersistableSettings.Settings.Motives == PassionMotives.EADefault || PersistableSettings.Settings.Motives == PassionMotives.PassionStandard) && (Actor.Motives.GetValue(CommodityKind.Energy) < -90f || Actor.Motives.GetValue(CommodityKind.Bladder) < -90f || Actor.Motives.GetValue(CommodityKind.Hunger) < -90f || Actor.Motives.GetValue(CommodityKind.MermaidDermalHydration) < -90f || Actor.Motives.GetValue(CommodityKind.VampireThirst) < -90f || Actor.Motives.GetValue(CommodityKind.AlienBrainPower) < -90f))
                {
                    return true;
                }
                return false;
            }
        }

        // i think this is the timeout in between autonomous passion allowances?
        public bool IsTimedOut
        {
            get
            {
                if (StartTime != 0L && HasPart && (!Part.HasSequence || Part.CurrentSequence.Repeat))
                {
                    if (PersistableSettings.Settings.MaxLength > 0 && StartTime + PersistableSettings.Settings.MaxLength < SimClock.CurrentTicks || IsAutonomous && StartTime + PersistableSettings.Settings.AutonomyLength < SimClock.CurrentTicks)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        public bool IsFull
        {
            get
            {
                if (PersistableSettings.Settings.EndWhenFull && IsValid && PersistableSettings.Settings.Motives != PassionMotives.Freeze && Actor.Motives.GetMotiveValue(CommodityKind.Fun) >= 100f && (!HadPartner || Actor.Motives.GetMotiveValue(CommodityKind.Social) >= 100f) && (!HasPart || !Part.HasTarget || !Part.Target.HasObject || !(Part.Target.Object is IShowerable) && !(Part.Target.Object is IBathtub) || Actor.Motives.GetMotiveValue(CommodityKind.Hygiene) >= 100f))
                {
                    return true;
                }
                return false;
            }
        }

        public bool ShouldStop
        {
            get
            {
                if (State == PassionState.Stopping || State == PassionState.Leaving)
                {
                    return true;
                }
                if (Actor.HasExitReason())
                {
                    return true;
                }
                return false;
            }
        }

        public bool HasPart
        {
            get
            {
                return Part != null;
            }
        }

        public bool HasSwitchPart
        {
            get
            {
                return SwitchPart != null;
            }
        }


        public bool HasCigarette
        {
            get
            {
                return PassionCommon.HasPart(Actor, PassionCommon.SimPart.Cigarette);
            }
        }



        public Sim.SwitchOutfitHelper SwitchOutfitHelper
        {
            get
            {
                return mSwitchOutfitHelper;
            }
            set
            {
                if (mSwitchOutfitHelper != value)
                {
                    if (mSwitchOutfitHelper != null)
                    {
                        mSwitchOutfitHelper.Dispose();
                    }
                    mSwitchOutfitHelper = value;
                }
            }
        }

        public static Player Create(Sim sim)
        {
            Player player = new Player();
            player.Actor = sim;
            player.RefreshHeightModifier(sim);
            player.SimStrapon = PassionGenitals.SimStraponList.UNSET;
            player.SimGenitalType = "";
            player.SimJunkBaseCASP = "";
            player.SimErectSIMO = "";
            player.PositionIndex = 0;
            player.PassionCharge = 0;
            player.CanAnimate = false;
            player.CanSwitch = false;
            player.ForceNoStrap = false;
            player.StrapIsOn = false;
            player.PeenIsErect = false;
            player.CancelledOnTwitterDotCom = false;
            player.IsNaked = false;
            player.SwitchBuffer = false;
            player.AreWeSwitching = false;
            player.IsStartingSesh = false;
            player.IsAutonomous = false;
            player.AutonomyIsDisabled = false;
            player.DirectTargeted = false;
            player.ActiveJoin = false;
            player.NumberAccepted = 0;
            player.UndressLevel = "";
            player.nudeTopRK = "";
            player.Partner = null;
            player.SwitchPart = null;
            player.StartTime = 0L;
            player.HasPreferredOutfit = false;
            player.PreferredOutfitCategory = OutfitCategories.None;
            player.PreferredOutfitIndex = 0;
            player.PreviousOutfitCategory = OutfitCategories.None;
            player.RejectionReason = RejectionReasons.None;
            player.PreviousOutfitIndex = 0;
            player.BufferedAnimation = string.Empty;
            player.SwitchRoleBuffer = string.Empty;
            return player;
        }

        public void SetPartnersToCheck(List<Sim> partners)
        {
            PartnersToCheck.Clear();
            if (partners == null)
            {
                return;
            }
            foreach (Sim partner in partners)
            {
                PartnersToCheck.Add(PassionBase.GetPlayer(partner));
            }
            PartnersToCheckCount = PartnersToCheck.Count;
        }

        public void RefreshHeightModifier(Sim sim)
        {
            if (IsValid && sim.SimDescription.Teen)
            {
                mHeightModifier = PassionCommon.SimPart.Types.Height.TeenMorph(sim);
            }
            else
            {
                mHeightModifier = 0f;
            }
        }

        public void SetOutfit(OutfitCategories category)
        {
            SetOutfit(category, 0);
        }

        public void SetOutfit(OutfitCategories category, int index)
        {
            if (IsValid && !Actor.IsSimBot && !Actor.IsEP11Bot && (index != Actor.CurrentOutfitIndex || category != Actor.CurrentOutfitCategory))
            {
                if (index < 0 || index >= Actor.SimDescription.GetOutfitCount(category))
                {
                    index = 0;
                }
                if (SpinDisabled)
                {
                    Actor.SwitchToOutfitWithoutSpin(category, index);
                }
                else
                {
                    Actor.SwitchToOutfitWithSpin(category, index);
                }
            }
        }

        public void RevertOutfit()
        {
            SetOutfit(PreviousOutfitCategory, PreviousOutfitIndex);
        }

        public void PreferOutfit()
        {
            if (IsValid)
            {
                PreferredOutfitCategory = Actor.CurrentOutfitCategory;
                PreferredOutfitIndex = Actor.CurrentOutfitIndex;
                HasPreferredOutfit = true;
            }
        }

        public bool UsePreferredOutfit()
        {
            if (HasPreferredOutfit)
            {
                try
                {
                    SetOutfit(PreferredOutfitCategory, PreferredOutfitIndex);
                    return true;
                }
                catch
                {
                    ClearPreferredOutfit();
                }
            }
            return false;
        }

        public void ClearPreferredOutfit()
        {
            HasPreferredOutfit = false;
            PreferredOutfitCategory = OutfitCategories.None;
            PreferredOutfitIndex = 0;
        }

        public void SaveOutfit()
        {
            if (IsValid)
            {
                PreviousOutfitCategory = Actor.CurrentOutfitCategory;
                PreviousOutfitIndex = Actor.CurrentOutfitIndex;
            }
        }



        public void ClearBuffer()
        {
            BufferedAnimation = string.Empty;
        }

        public bool CanPassion(Sim partner)
        {
            return CanPassion(Actor, partner);
        }

        public static bool CanPassion(Sim sim, Sim partner)
        {
            return PassionBase.IsValid(sim) && PassionBase.IsValid(partner) && (!sim.IsRelated(partner) || PersistableSettings.Settings.Incest);
        }

        public bool WillPassion(Player partner)
        {
            return IsValid && partner != null && partner.IsValid && WillPassion(Actor, partner.Actor);
        }

        public bool WillPassion(Part part)
        {
            if (IsValid && part != null)
            {
                int num = 0;
                Sim[] array = new Sim[part.Players.Count];
                try
                {
                    foreach (Player value in part.Players.Values)
                    {
                        if (value.IsValid)
                        {
                            array[num] = value.Actor;
                        }
                        num++;
                    }
                }
                catch
                {
                }
                return WillPassion(Actor, array);
            }
            return false;
        }

        public static bool WillPassion(Sim sim, Sim target)
        {
            return WillPassion(sim, new Sim[1] { target });
        }


        // tests if a sim will ACCEPT a passion request or not
        // modify this to include multiplers for new libido system
        // from what i can see.. higher the number, more likely to accept. lets yoloswag
        public static bool WillPassion(Sim sim, Sim[] targets)
        {
            // if rejection is disabled
            if (!PersistableSettings.Settings.CanReject)
            {
                return true;
            }

            bool result = false;


            if (sim != null && targets != null && targets.Length != 0)
            {
                int num = PassionAutonomyTuning.bCheckThreshold;

                bool RejectedLowRel = false;
                bool RejectedLowLibido = false;
                bool RejectedInPublic = false;
                bool RejectedIsBlocked = false;

                // trait based math for sim1 AKA initiator
                // the partner sim has to 'beat' this number, so here the 'goal' is lower
                // actually i dont think this is ever. fucking used since both initiator and partner are passed thru this?
                //  if (sim.HasTrait(TraitNames.Inappropriate))
                //{
                //	num = 0;
                //}
                //else if (sim.HasTrait(TraitNames.PartyAnimal))
                //{
                //	num = 50;
                //}
                //else if (sim.HasTrait(TraitNames.Shy))
                //{
                //	num = 100;
                //}

                Dictionary<Sim, int> dictionary = new Dictionary<Sim, int>();
                foreach (Sim sim2 in targets)
                {
                    // if theres no other sims, ignore this
                    if (sim2 == null || sim2 == sim)
                    {
                        continue;
                    }

                    int num2 = 0;

                    Relationship relationship = sim.GetRelationship(sim2, false);

                    if (relationship != null)
                    {
                        // num2 is how many LTR points sim2 has with sim1
                        int LTRValue = (int)relationship.LTR.Liking;

                        LTRValue /= 2;

                        num2 += LTRValue;

                        if (num2 >= 10)
                        {
                            RejectedLowRel = true;
                        }

                        // refactor this?
                        if (relationship.AreRomantic())
                        {
                            if (sim.HasTrait(TraitNames.HopelessRomantic))
                            {
                                num2 += 15;
                            }
                            if (relationship.LTR.HasInteractionBit(LongTermRelationship.InteractionBits.Kissed))
                            {
                                num2 += 10;
                            }
                        }
                    }

                    // trait based math for sim2
                    foreach (PassionAutonomyTuning.PassionTraitAutonomy trait in PassionAutonomyTuning.PassionAutoTraitList)
                    {
                        TraitNames ParsedTraitName;


                        ParserFunctions.TryParseEnum(trait.TraitName, out ParsedTraitName, TraitNames.Unknown);

                        TraitNames TraitType = ParsedTraitName;


                        try
                        {
                            if (sim2.HasTrait(TraitType))
                            {
                                bool TraitAxis = trait.TraitAxis;
                                int TraitScore = trait.TraitScore;

                                if (TraitAxis)
                                {
                                    num2 += TraitScore;
                                }
                                else
                                {
                                    num2 -= TraitScore;
                                }
                            }
                        }
                        catch
                        {
                            PassionCommon.Message("something in the trait autonomy check Broke lmao");
                        }

                    }

                    // how high is sim2's libido?
                    // time for the worst ifelse tree evar
                    BuffManager buffManager = sim2.BuffManager;

                    // 0%
                    if (buffManager.HasElement((BuffNames)2248271455579464240uL))
                    {
                        num2 += PassionAutonomyTuning.ZeroPercentLibidoBonus;
                        RejectedLowLibido = true;
                    }
                    // 10%
                    else if (buffManager.HasElement((BuffNames)2913570583726353694uL))
                    {
                        num2 += PassionAutonomyTuning.OnePercentLibidoBonus;

                        RejectedLowLibido = true;
                    }
                    // 20%
                    else if (buffManager.HasElement((BuffNames)2922268820215428064uL))
                    {
                        num2 += PassionAutonomyTuning.TwoPercentLibidoBonus;
                        RejectedLowLibido = true;
                    }
                    // 30%
                    else if (buffManager.HasElement((BuffNames)3097843141287298166uL))
                    {
                        num2 += PassionAutonomyTuning.ThreePercentLibidoBonus;
                    }
                    // 40%
                    else if (buffManager.HasElement((BuffNames)8198323707617122614uL))
                    {
                        num2 += PassionAutonomyTuning.FourPercentLibidoBonus;
                    }
                    // 50%
                    else if (buffManager.HasElement((BuffNames)8200297330989383022uL))
                    {
                        num2 += PassionAutonomyTuning.FivePercentLibidoBonus;
                    }
                    // 60%
                    else if (buffManager.HasElement((BuffNames)2917472750494117670uL))
                    {
                        num2 += PassionAutonomyTuning.SixPercentLibidoBonus;
                    }
                    // 70%
                    else if (buffManager.HasElement((BuffNames)16251613925768384549uL))
                    {
                        num2 += PassionAutonomyTuning.SevenPercentLibidoBonus;
                    }
                    // 80%
                    else if (buffManager.HasElement((BuffNames)14041574305464178967uL))
                    {
                        num2 += PassionAutonomyTuning.EightPercentLibidoBonus;
                    }
                    // 90%
                    else if (buffManager.HasElement((BuffNames)13147589483235469726uL))
                    {
                        num2 += PassionAutonomyTuning.NinePercentLibidoBonus;
                    }
                    // 100%
                    else if (buffManager.HasElement((BuffNames)2922253427052633003uL))
                    {
                        num2 += PassionAutonomyTuning.TenPercentLibidoBonus;
                    }



                    // extra modifiers for new traits

                    // asexual - denies autonomy
                    if (sim2.HasTrait((TraitNames)6177560411462291097uL) && PassionBase.GetPlayer(sim2).IsAutonomous)
                    {
                        num2 -= 9999;
                        RejectedIsBlocked = true;

                    }
                    // abstinent - denies ALL
                    if (sim2.HasTrait((TraitNames)2214287488174702228uL))
                    {
                        num2 -= 9999;
                        RejectedIsBlocked = true;
                    }
                    // hypersexual - more willing
                    if (sim2.HasTrait((TraitNames)5711695705602619160uL))
                    {
                        num2 += PassionAutonomyTuning.bHypersexualBonus;
                    }


                    // modifiers based on lot type

                    Lot SimCurrentLot = sim2.LotCurrent;
                    int CoolerLotScore = 0;

                    // if residential
                    if (sim2.LotCurrent.LotType == LotType.Residential)
                    {
                        num2 += 10;
                    }
                    else
                    {


                        foreach (PassionAutonomyTuning.PassionLots lot in PassionAutonomyTuning.PassionLotList)
                        {
                            Lot.MetaAutonomyType ParsedLotName;


                            ParserFunctions.TryParseEnum(lot.LotTypeName, out ParsedLotName, Lot.MetaAutonomyType.None);

                            Lot.MetaAutonomyType LotType = ParsedLotName;


                            try
                            {
                                if (SimCurrentLot.mMetaAutonomyType == LotType)
                                {
                                    bool LotAxis = lot.LotAxis;
                                    CoolerLotScore = lot.LotScore;

                                    if (LotAxis)
                                    {
                                        num2 += CoolerLotScore;
                                    }
                                    else
                                    {
                                        num2 -= CoolerLotScore;
                                        RejectedInPublic = true;
                                    }
                                }
                            }
                            catch
                            {
                                PassionCommon.Message("something in the lot autonomy check Broke lmao");
                            }

                        }
                        // fallback if the lot wasn't checked
                        if (CoolerLotScore == 0)
                        {
                            num2 += PassionAutonomyTuning.bDefaultPublicScore;
                        }
                    }

                    // set rejection reason for notif

                    if (RejectedIsBlocked == true)
                    {
                        PassionBase.GetPlayer(sim2).RejectionReason = RejectionReasons.IsBlocked;
                    }
                    else if (RejectedInPublic == true)
                    {
                        PassionBase.GetPlayer(sim2).RejectionReason = RejectionReasons.InPublic;
                    }
                    else if (RejectedLowLibido == true)
                    {
                        PassionBase.GetPlayer(sim2).RejectionReason = RejectionReasons.LowLibido;
                    }
                    else if (RejectedLowRel == true)
                    {
                        PassionBase.GetPlayer(sim2).RejectionReason = RejectionReasons.LowLTR;
                    }

                    dictionary.Add(sim2, num2);
                }
                int num3 = 0;
                if (dictionary.Count > 0)
                {
                    foreach (int value in dictionary.Values)
                    {
                        num3 += value;
                    }
                    if (num3 > 0)
                    {
                        num3 /= dictionary.Count;
                    }
                }
                num3 += PassionCommon.Modules.GetPassionScoringModifier(sim, targets);

                if (num3 >= num)
                {
                    result = true;
                }

            }
            return result;
        }
        // end if a sim will ACCEPT passion


        public void DegradeRelationship(Player partner)
        {
            UpdateRelationship(partner, PersistableSettings.Settings.RelationshipLoss);
        }

        public void ImproveRelationship(Player partner)
        {
            UpdateRelationship(partner, PersistableSettings.Settings.RelationshipGain);
        }

        public void UpdateRelationship(Player partner, float amount)
        {
            if (!IsValid || partner == null || !partner.IsValid)
            {
                return;
            }
            Relationship relationship = Relationship.Get(Actor, partner.Actor, true);
            if (relationship == null || amount == 0f)
            {
                return;
            }
            VisualEffect visualEffect = null;
            VisualEffect visualEffect2 = null;
            string text = string.Empty;
            if (amount > 8f)
            {
                text = "socialPlusPlusFx";
            }
            else if (amount > 0f)
            {
                text = "socialPlusFx";
            }
            else if (amount < -8f)
            {
                text = "socialMinusMinusFx";
            }
            else if (amount < 0f)
            {
                text = "socialMinusFx";
            }
            if (text != string.Empty)
            {
                visualEffect = VisualEffect.Create(text);
                visualEffect2 = VisualEffect.Create(text);
                if (visualEffect != null && visualEffect2 != null)
                {
                    if (amount < 0f)
                    {
                        visualEffect.SetEffectColorScale(1f, 0f, 0f);
                        visualEffect2.SetEffectColorScale(1f, 0f, 0f);
                    }
                    Actor.ParentHeadlineFx(visualEffect);
                    partner.Actor.ParentHeadlineFx(visualEffect2);
                    visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
                    visualEffect2.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
                }
            }
            relationship.LTR.UpdateLiking(amount);
        }

        public void ImproveRelationships()
        {
            if (!HasPart || !(PersistableSettings.Settings.RelationshipGain > 0f))
            {
                return;
            }
            foreach (Player value in Part.Players.Values)
            {
                if (value != null && value != this && !PartnersUpdated.Contains(value) && !value.PartnersUpdated.Contains(this))
                {
                    PartnersUpdated.Add(value);
                    ImproveRelationship(value);
                }
            }
        }

        public void PlayRouteFailure()
        {
            if (IsValid)
            {
                ThoughtBalloonManager.BalloonData balloonData = new ThoughtBalloonManager.BalloonData("t_balloon_routefail");
                balloonData.BalloonType = ThoughtBalloonTypes.kSpeechBalloon;
                balloonData.mPriority = ThoughtBalloonPriority.High;
                balloonData.LowAxis = ThoughtBalloonAxis.kDislike;
                Actor.ThoughtBalloonManager.ShowBalloon(balloonData);
                Actor.PlaySoloAnimation(Position.Animation.Prefix.Age(Actor) + "_routeFail_standing_x");
            }
        }

        public void PlayJoinFailure(Sim unliked)
        {
            if (IsValid && unliked != null)
            {
                ThoughtBalloonManager.BalloonData balloonData = new ThoughtBalloonManager.BalloonData(unliked.GetThumbnailKey());
                balloonData.BalloonType = ThoughtBalloonTypes.kThoughtBalloon;
                balloonData.mPriority = ThoughtBalloonPriority.High;
                balloonData.LowAxis = ThoughtBalloonAxis.kDislike;
                Actor.ThoughtBalloonManager.ShowBalloon(balloonData);
                Actor.PlaySoloAnimation(Position.Animation.Prefix.Age(Actor) + "_react_view_hate_x");
            }
        }

        public List<Sim> GetAvailablePartners()
        {
            return GetAvailablePartners(Actor);
        }

        public static List<Sim> GetAvailablePartners(Sim sim)
        {
            List<Sim> list = new List<Sim>();
            if (sim != null)
            {
                foreach (Sim sim2 in sim.LotCurrent.GetSims())
                {
                    if (sim2 != sim && CanPassion(sim, sim2) && !sim.LotCurrent.IsWorldLot)
                    {
                        list.Add(sim2);
                    }
                }
            }
            return list;
        }

        public PassionTarget GetNearbySupportedTarget()
        {
            return PassionBase.GetTarget(GetNearbySupportedObject());
        }

        public GameObject GetNearbySupportedObject()
        {
            if (IsValid)
            {
                List<Type> randomList = new List<Type>(PassionCommon.PreferredTypes);
                for (int i = 0; i < PassionCommon.PreferredTypes.Count; i++)
                {
                    Type randomObjectFromList = RandomUtil.GetRandomObjectFromList(randomList);
                    GameObject[] array = Sims3.SimIFace.Queries.GetObjects(randomObjectFromList, Actor.LotCurrent.LotId, Actor.RoomId) as GameObject[];
                    if (array == null)
                    {
                        continue;
                    }
                    GameObject[] array2 = array;
                    foreach (GameObject gameObject in array2)
                    {
                        if (gameObject != null && !gameObject.HasBeenDestroyed && PassionBase.HasRoom(gameObject))
                        {
                            return gameObject;
                        }
                    }
                }
                GameObject[] objects = Sims3.Gameplay.Queries.GetObjects<GameObject>(Actor.LotCurrent, Actor.RoomId);
                GameObject[] array3 = objects;
                foreach (GameObject gameObject2 in array3)
                {
                    if (gameObject2 != null && !gameObject2.HasBeenDestroyed)
                    {
                        PassionType supportedType = PassionType.GetSupportedType(gameObject2);
                        if (supportedType != null && !supportedType.Is<Windows>() && !supportedType.Is<ChairDining>() && !PassionCommon.PreferredTypes.Contains(supportedType.Type) && PassionBase.HasRoom(gameObject2))
                        {
                            return gameObject2;
                        }
                    }
                }
            }
            return null;
        }


        // this can be considered the 'true' start of the loop i think, because
        // asking to passion leads to this section, and it runs down the list from there?
        public bool Invite(Player partner, Interaction<Sim, Sim> interaction)
        {
            if (partner != null && partner.IsValid)
            {
                if (partner.Actor.IsSleeping)
                {
                    partner.Actor.InteractionQueue.CancelInteraction(partner.Actor.InteractionQueue.GetCurrentInteraction(), true);
                    PassionCommon.Wait(30);
                }
                State = PassionState.None;
                Actor.SynchronizationTarget = partner.Actor;
                Actor.SynchronizationRole = Sim.SyncRole.Initiator;
                Actor.SynchronizationLevel = Sim.SyncLevel.Started;
                InteractionInstance entry = interaction.LinkedInteractionInstance = Interactions.BeAskedToPassion.Singleton.CreateInstance(Actor, partner.Actor, new InteractionPriority(InteractionPriorityLevel.High), false, true);
                partner.Actor.InteractionQueue.Add(entry);
                if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Started, 60f) && Route(partner))
                {
                    Actor.SynchronizationLevel = Sim.SyncLevel.Routed;
                    if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Routed, 60f))
                    {
                        Actor.RouteTurnToFace(partner.Actor.Position);
                        if (PersistableSettings.Settings.ActiveAlwaysAccepts || WillPassion(partner))
                        {
                            State = PassionState.Accept;
                        }
                        else
                        {
                            State = PassionState.Deny;
                        }
                        Actor.SynchronizationLevel = Sim.SyncLevel.Committed;
                        if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Committed, 60f))
                        {
                            if (State == PassionState.Accept && partner.State == PassionState.Accept)
                            {
                                if (Actor.Posture.AllowsNormalSocials() && !(Actor.Posture is SwimmingInPool))
                                {
                                    PlayInviteSuccess(partner);
                                }
                                NumberAccepted++;
                            }
                            else
                            {
                                if (Actor.Posture.AllowsNormalSocials() && !(Actor.Posture is SwimmingInPool))
                                {
                                    PlayInviteFailure(partner);
                                }
                                DegradeRelationship(partner);

                                // send message as to why the invite failed if we have its reason logged
                                // make this non-global for final, its global rn for testing lol
                                if (PassionBase.GetPlayer(partner.Actor).RejectionReason == RejectionReasons.IsBlocked)
                                {
                                    PassionCommon.SimMessage(PassionCommon.Localize("Sorry! I'm... not into that kind of thing.").ToString(), partner.Actor);
                                }
                                else if (PassionBase.GetPlayer(partner.Actor).RejectionReason == RejectionReasons.InPublic)
                                {
                                    PassionCommon.SimMessage(PassionCommon.Localize("Uh, no way. Not in public!").ToString(), partner.Actor);
                                }
                                else if (PassionBase.GetPlayer(partner.Actor).RejectionReason == RejectionReasons.LowLibido)
                                {
                                    PassionCommon.SimMessage(PassionCommon.Localize("I'm not really in the mood for that, not right now.").ToString(), partner.Actor);
                                }
                                else if (PassionBase.GetPlayer(partner.Actor).RejectionReason == RejectionReasons.LowLTR)
                                {
                                    PassionCommon.SimMessage(PassionCommon.Localize("Look, I really don't know you well enough to do that.").ToString(), partner.Actor);
                                }


                                Leave();

                                return false;
                            }
                        }
                    }
                }
                if (--PartnersToCheckCount < 1)
                {
                    if (DirectTargeted)
                    {
                        DirectStartLoop();
                    }
                    else if (NumberAccepted > 0)
                    {
                        Actor.InteractionQueue.PushAsContinuation(Interactions.RouteToPassion.Singleton.CreateInstance(partner.Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
                    }
                    else
                    {
                        Leave();
                    }
                }
                return true;
            }
            return false;
        }

        public void PlayInviteSuccess(Player partner)
        {
            if (!IsValid || partner == null || !partner.IsValid)
            {
                return;
            }
            string animationName = "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_x";
            ProductVersion productVersion = ProductVersion.BaseGame;
            if (Actor.SimDescription.Teen || partner.Actor.SimDescription.Teen)
            {
                animationName = !RandomUtil.CoinFlip() ? "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_x" : "a2a_soc_Neutral_RevealSecret_Friendly_Neutral_x";
            }
            else if (GameUtils.IsInstalled(ProductVersion.EP3) && Actor.HasTrait(TraitNames.MasterOfSeduction))
            {
                if (GameUtils.IsInstalled(ProductVersion.EP9))
                {
                    animationName = "a2a_soc_neutral_heatOfTheMomentKiss_amorous_neutral_x";
                    productVersion = ProductVersion.EP9;
                }
                else
                {
                    animationName = "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_x";
                }
            }
            else if (GameUtils.IsInstalled(ProductVersion.EP9) && Actor.HasTrait(TraitNames.Irresistible))
            {
                animationName = "a2a_soc_irresistible_wink_accept_x";
                productVersion = ProductVersion.EP9;
            }
            Actor.PlaySoloAnimation(animationName, true, productVersion, true);
        }

        public void PlayInviteFailure(Player partner)
        {
            if (IsValid && partner != null && partner.IsValid)
            {
                string animationName = "a2a_soc_Amorous_flirtHitOn_Neutral_Neutral_x";
                ProductVersion productVersion = ProductVersion.BaseGame;
                Actor.PlaySoloAnimation(animationName, true, productVersion, true);
            }
        }

        public bool Respond(Player partner)
        {
            if (partner != null && partner.IsValid)
            {
                State = PassionState.None;
                Actor.SynchronizationTarget = partner.Actor;
                Actor.SynchronizationRole = Sim.SyncRole.Receiver;
                Actor.SynchronizationLevel = Sim.SyncLevel.Started;
                if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Started, 600f))
                {
                    Actor.SynchronizationLevel = Sim.SyncLevel.Routed;
                    if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Routed, 60f))
                    {
                        Actor.RouteTurnToFace(partner.Actor.Position);
                        if (WillPassion(partner))
                        {
                            State = PassionState.Accept;
                        }
                        else
                        {
                            State = PassionState.Deny;
                        }
                        Actor.SynchronizationLevel = Sim.SyncLevel.Committed;
                        if (Actor.WaitForSynchronizationLevelWithSim(partner.Actor, Sim.SyncLevel.Committed, 60f))
                        {
                            if (State == PassionState.Accept && partner.State == PassionState.Accept)
                            {
                                if (Actor.Posture.AllowsNormalSocials() && !(Actor.Posture is SwimmingInPool))
                                {
                                    PlayRespondSuccess(partner);
                                }
                                Join(partner.Part);
                                if (partner.DirectTargeted)
                                {
                                    DirectStartLoop();
                                }
                                else
                                {
                                    Actor.InteractionQueue.PushAsContinuation(Interactions.RouteToPassion.Singleton.CreateInstance(partner.Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
                                }
                                return true;
                            }
                            if (Actor.Posture.AllowsNormalSocials() && !(Actor.Posture is SwimmingInPool))
                            {
                                PlayRespondFailure(partner);
                            }
                            Reset();
                        }
                    }
                }
            }
            return false;
        }

        public void PlayRespondSuccess(Player partner)
        {
            if (!IsValid || partner == null || !partner.IsValid)
            {
                return;
            }
            string animationName = "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_y";
            ProductVersion productVersion = ProductVersion.BaseGame;

            // set pre passion outfits
            PassionBase.GetPlayer(Actor).PreviousOutfitCategory = Actor.CurrentOutfitCategory;
            PassionBase.GetPlayer(Actor).PreviousOutfitIndex = Actor.CurrentOutfitIndex;

            partner.PreviousOutfitCategory = partner.Actor.CurrentOutfitCategory;
            partner.PreviousOutfitIndex = partner.Actor.CurrentOutfitIndex;


            if (Actor.SimDescription.Teen || partner.Actor.SimDescription.Teen)
            {
                animationName = "a2a_soc_Neutral_RevealSecret_Friendly_Neutral_y";
            }
            else if (GameUtils.IsInstalled(ProductVersion.EP3) && partner.Actor.HasTrait(TraitNames.MasterOfSeduction))
            {
                if (GameUtils.IsInstalled(ProductVersion.EP9))
                {
                    animationName = "a2a_soc_neutral_heatOfTheMomentKiss_amorous_neutral_y";
                    productVersion = ProductVersion.EP9;
                }
                else
                {
                    animationName = "a2a_soc_Amorous_flirtHitOn_Amorous_Amorous_y";
                }
            }
            else if (GameUtils.IsInstalled(ProductVersion.EP9) && partner.Actor.HasTrait(TraitNames.Irresistible))
            {
                animationName = "a2a_soc_irresistible_wink_accept_y";
                productVersion = ProductVersion.EP9;
            }
            Actor.PlaySoloAnimation(animationName, true, productVersion, true);
        }

        public void PlayRespondFailure(Player partner)
        {
            if (IsValid && partner != null && partner.IsValid)
            {
                string animationName = "a2a_soc_Amorous_flirtHitOn_Neutral_Neutral_y";
                ProductVersion productVersion = ProductVersion.BaseGame;
                Actor.PlaySoloAnimation(animationName, true, productVersion, true);
            }
        }

        public float GetDistanceTo(IGameObject obj)
        {
            if (IsValid && obj != null)
            {
                return Actor.GetDistanceToObject(obj);
            }
            return float.PositiveInfinity;
        }

        public float GetDistanceTo(Vector3 point)
        {
            return PassionCommon.GetDistanceBetween(Actor, point);
        }

        public bool Route()
        {
            if (RouteToTarget())
            {
                Actor.InteractionQueue.PushAsContinuation(Interactions.BeginPassion.Singleton.CreateInstance(Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
                return true;
            }
            return Leave();
        }

        public bool Route(Player partner, float range)
        {
            if (partner != null && partner.IsValid)
            {
                Route.RouteOption[] additionalRouteOptions = new Route.RouteOption[1] { Sims3.SimIFace.Route.RouteOption.MakeDynamicObjectAdjustments };
                try
                {
                    if (Actor.RoutingComponent.RouteToObjectRadialRange(partner.Actor, 1.5f, range, additionalRouteOptions))
                    {
                        return true;
                    }
                }
                catch
                {
                }
                return Actor.RoutingComponent.RouteToObjectRadialRange(partner.Actor, 1.5f, range);
            }
            return false;
        }

        public bool Route(Player partner)
        {
            if (IsValid && partner != null && partner.IsValid)
            {
                State = PassionState.Routing;
                if (Actor.RouteToDynamicObjectRadius(partner.Actor, Actor.GetSocialRadiusWith(partner.Actor), null))
                {
                    return true;
                }
            }
            return false;
        }

        public void CancelTheFuckingInteractionPlease()
        {
            Actor.InteractionQueue.CancelInteraction(Actor.CurrentInteraction, false);
            CancelledOnTwitterDotCom = false;
        }

        public bool RouteToTarget()
        {
            if (IsValid && HasPart && Part.HasTarget)
            {
                State = PassionState.Routing;
                if (Part.Target.HasObject)
                {
                    if (Part.Type.Is("CrossWood"))
                    {
                        if (Actor.RouteToPointRadialRange(Part.Target.Object.GetPositionOfSlot(Door.RoutingSlots.Door0_Front), 0.1f, 1.5f))
                        {
                            return true;
                        }
                        if (Actor.RouteToPointRadialRange(Part.Target.Object.GetPositionOfSlot(Door.RoutingSlots.Door0_Front), 0.1f, 3f))
                        {
                            return true;
                        }
                    }
                    if (Part.Type.Is<PoolLadder>())
                    {
                        if (Actor.RouteToPointRadialRange(Part.Target.Object.GetPositionOfSlot((Slot)1986264130u), 0.1f, 1.5f))
                        {
                            return true;
                        }
                        if (Actor.RouteToPointRadialRange(Part.Target.Object.GetPositionOfSlot((Slot)1986264130u), 0.1f, 3f))
                        {
                            return true;
                        }
                    }
                    Route.RouteOption[] additionalRouteOptions = new Route.RouteOption[1] { Sims3.SimIFace.Route.RouteOption.MakeDynamicObjectAdjustments };
                    try
                    {
                        if (Actor.RoutingComponent.RouteToObjectRadialRange(Part.Target.Object, 0.5f, 2.5f, additionalRouteOptions))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (Actor.RoutingComponent.RouteToObjectRadialRange(Part.Target.Object, 0.5f, 5f, additionalRouteOptions))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                    }
                }
                try
                {
                    if (Actor.RouteToPointRadialRange(Part.Target.Location, 0.5f, 2.5f))
                    {
                        return true;
                    }
                }
                catch
                {
                }
                try
                {
                    if (Actor.RouteToPointRadialRange(Part.Target.Location, 0.5f, 5f))
                    {
                        return true;
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        public bool RouteInWater(Vector3 point)
        {
            return RouteInWater(point, 1f, 2f, 1.4f);
        }

        public bool RouteInWater(Vector3 point, float min, float max, float spacing)
        {
            if (point != Vector3.Invalid && point != Vector3.Empty)
            {
                RadialRangeDestination radialRangeDestination = new RadialRangeDestination();
                radialRangeDestination.mCenterPoint = point;
                radialRangeDestination.mfMinRadius = min;
                radialRangeDestination.mfMaxRadius = max;
                radialRangeDestination.mfPreferredSpacing = spacing;
                radialRangeDestination.ScoreFunctionWeights[(uint)(UIntPtr)2uL] = 1f;
                Route route = Actor.CreateRoute();
                route.SetOption(Sims3.SimIFace.Route.RouteOption.EnableWaterPlanning, true);
                route.SetOption(Sims3.SimIFace.Route.RouteOption.DoNotEmitDegenerateRoutesForRadialRangeGoals, true);
                route.SetOption(Sims3.SimIFace.Route.RouteOption.DoLineOfSightCheckUserOverride, false);
                route.SetOption(Sims3.SimIFace.Route.RouteOption.CheckForFootprintsNearGoals, false);
                route.SetOption2(Sims3.SimIFace.Route.RouteOption2.EnablePlanningAsBoat, false);
                route.AddDestination(radialRangeDestination);
                if (route.Plan().Succeeded() && Actor.DoRoute(route))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Join(PassionTarget target)
        {
            if (target != null && target.IsValid && !target.IsOccupied)
            {
                return target.Add(this);
            }
            return false;
        }

        public bool Join(Part part)
        {
            if (part != null && part.HasRoom)
            {
                return part.Add(this);
            }
            return false;
        }

        public void UpdateLocation()
        {
            if (Actor != null && Part != null)
            {
                Forward = Part.Forward;
                Vector3 location = Part.Location;
                location.y += HeightModifier;
                Location = location;
                IsInPlace = true;
            }
        }

        public void SetPosture()
        {
            if (IsValid)
            {
                if (HasPart && Part.HasTarget && Part.Target.HasObject)
                {
                    PassionPosture.Set(Actor, Part.Target.Object);
                }
                else
                {
                    PassionPosture.Set(Actor);
                }
            }
        }

        public void ClearPosture()
        {
            if (IsValid)
            {
                PassionPosture.Revert(Actor);
            }
        }

        public void ChangePosition()
        {
            if (HasPart)
            {
                Part.ChangePosition();
                CancelledOnTwitterDotCom = true;
            }
        }

        public void Animate()
        {
            CanAnimate = false;
            State = PassionState.Animating;
            Animate(BufferedAnimation);
            if (State != PassionState.Leaving && State != PassionState.Stopping)
            {
                State = PassionState.Ready;
            }
        }

        public void Animate(string clip)
        {
            Animate(clip, ProductVersion.Undefined);
        }

        public void Animate(string clip, ProductVersion version)
        {
            if (!IsValid)
            {
                return;
            }
            try
            {
                StateMachineClient stateMachineClient = StateMachineClient.Acquire(Actor, "passion_generic");
                if (Part.Target.Object != null && PersistableSettings.Settings.ObjectAnimation != null)
                {
                    stateMachineClient.SetParameter("ObjectAnimationName", PersistableSettings.Settings.ObjectAnimation, version);
                    stateMachineClient.SetActor("obj", Part.Target.Object);
                }
                stateMachineClient.SetParameter("AnimationName", clip, version);
                stateMachineClient.SetActor(PassionCommon.X, Actor);
                stateMachineClient.EnterState(PassionCommon.X, "Enter");
                stateMachineClient.RequestState(PassionCommon.X, "Play Animation");
                stateMachineClient.RequestState(PassionCommon.X, "Exit");

            }
            catch
            {
            }
        }

        // motive updates while passioning
        public void BeginMotiveUpdates()
        {
            if (!IsValid || !HasPart)
            {
                return;
            }
            InteractionInstance currentInteraction = Actor.CurrentInteraction;
            if (currentInteraction == null)
            {
                return;
            }
            if (PersistableSettings.Settings.Motives == PassionMotives.MaxAll)
            {
                Actor.Motives.MaxEverything();
            }
            else
            {
                if (PersistableSettings.Settings.Motives == PassionMotives.EADefault)
                {
                    return;
                }
                bool flag = PersistableSettings.Settings.Motives == PassionMotives.NoDecay || PersistableSettings.Settings.Motives == PassionMotives.Freeze;
                float num = 0f;
                float num2 = 0f;
                float num3 = 0f;
                float num4 = 0f;
                float num5 = 0f;
                float num6 = 0f;
                if (PersistableSettings.Settings.Motives != PassionMotives.Freeze)
                {
                    num4 = 0f;
                    num3 = 0f;
                    num6 = 0f;
                    num = 100f;
                    if (PersistableSettings.Settings.Motives != PassionMotives.NoDecay)
                    {
                        num2 = -40f;
                        num5 = -20f;
                    }
                    if (Part.HasTarget)
                    {
                        PassionTarget target = Part.Target;
                        if (target.HasObject)
                        {
                            if (target.Object is IShowerable)
                            {
                                num2 = 600f;
                            }
                            else if (target.Object is IHotTub || target.Object is IBathtub)
                            {
                                num2 = 400f;
                            }
                        }
                    }
                    if (HadPartner)
                    {
                        num = 300f;
                        num6 = 300f;
                    }
                }
                // refactor this maybe because idfk whats going on
                if (num != 0f || flag)
                {
                    currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Fun, num, false, num, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
                }
                if (num2 != 0f || flag)
                {
                    currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Hygiene, num2, false, num2, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
                }
                if (num3 != 0f || flag)
                {
                    currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Bladder, num3, false, num3, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
                }
                if (num4 != 0f || flag)
                {
                    currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Hunger, num4, false, num4, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
                }
                if (num5 != 0f || flag)
                {
                    currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Energy, num5, false, num5, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
                }
                if (num6 != 0f || flag)
                {
                    currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Social, num6, false, num6, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
                }
            }
        }

        public void EndMotiveUpdates()
        {
            if (IsValid)
            {
                InteractionInstance currentInteraction = Actor.CurrentInteraction;
                if (currentInteraction != null && PersistableSettings.Settings.Motives != 0 && PersistableSettings.Settings.Motives != PassionMotives.MaxAll)
                {
                    currentInteraction.EndCommodityUpdates(true);
                }
            }
        }

        public void RecalculateMotiveUpdates()
        {
            if (IsValid && IsActive)
            {
                EndMotiveUpdates();
                BeginMotiveUpdates();
                PassionCommon.Modules.LoopProcessing(Actor);
            }
        }



        // ????? this is literally the same as above. whats going on here
        public void BroadcastPassionResult()
        {
            try
            {
                if (!IsValid || !IsActive || Actor.LotCurrent == null || Actor.LotCurrent.IsWorldLot)
                {
                    return;
                }
                foreach (Sim allActor in Actor.LotCurrent.GetAllActors())
                {
                    if (allActor == null || allActor.RoomId != Actor.RoomId || allActor.CurrentInteraction != null && allActor.CurrentInteraction.Target is RabbitHole || !CanPassion(allActor))
                    {
                        continue;
                    }
                    Player player = PassionBase.GetPlayer(allActor);
                    if (!player.IsValid || player.IsActive || player.State != 0)
                    {
                        continue;
                    }
                    // START REFACTOR

                    // if witness is party animal
                    if (player.Actor.HasTrait(TraitNames.PartyAnimal))
                    {
                        player.Actor.PlayReaction(ReactionTypes.PumpFist, Actor, ReactionSpeed.Immediate);
                        player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
                        // set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
                        if (PersistableSettings.Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
                        {
                            PassionCommon.SimMessage(PassionCommon.Localize("WOOOOOO YEAH!! You guys know how to PARTY HARD! LITERALLY!").ToString(), player.Actor);
                        }
                    }
                    // if witness is flirty
                    else if (player.Actor.HasTrait(TraitNames.Flirty))
                    {
                        player.Actor.PlayReaction(ReactionTypes.Giggle, Actor, ReactionSpeed.Immediate);
                        player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
                        // set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
                        if (PersistableSettings.Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
                        {
                            PassionCommon.SimMessage(PassionCommon.Localize("Ooh la la! Who could deny such a show?").ToString(), player.Actor);
                        }
                    }
                    // if witness is daredevil
                    else if (player.Actor.HasTrait(TraitNames.Daredevil))
                    {
                        player.Actor.PlayReaction(ReactionTypes.PumpFist, Actor, ReactionSpeed.Immediate);
                        player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
                        // set the message to be on a coinflip to potentially cut down on the barrage of notifs when sims screw in a crowded place
                        if (PersistableSettings.Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
                        {
                            PassionCommon.SimMessage(PassionCommon.Localize("Livin' it on the edge, huh? Cheers, bro!").ToString(), player.Actor);
                        }
                    }
                    // if witness is diva
                    else if (GameUtils.IsInstalled(ProductVersion.EP6) && player.Actor.HasTrait(TraitNames.Diva))
                    {
                        player.Actor.PlayReaction(ReactionTypes.Fascinated, Actor, ReactionSpeed.Immediate);
                        player.Actor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
                        if (PersistableSettings.Settings.AutonomyNotify && player.Actor.SimDescription.IsHuman && RandomUtil.CoinFlip())
                        {
                            PassionCommon.SimMessage(PassionCommon.Localize("Wow, what a bold move...! I'm so intruiged...!").ToString(), player.Actor);
                        }
                    }
                    // general reactions
                    else if (PersistableSettings.Settings.AutonomyChance > 0 && (PersistableSettings.Settings.AutonomyActive || !player.Actor.LotHome.IsActive) && (PersistableSettings.Settings.AutonomyPublic || player.Actor.LotCurrent.LotType == LotType.Residential) && RandomUtil.GetInt(0, 99) < PersistableSettings.Settings.AutonomyChance)
                    {
                        player.IsAutonomous = true;
                        player.Actor.InteractionQueue.AddNext(Interactions.WatchLoop.Singleton.CreateInstance(Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                        // add coinflip to reduce spam in crowded areas
                        if (PersistableSettings.Settings.AutonomyNotify && RandomUtil.CoinFlip())
                        {
                            if (PassionCommon.RandomReactionNeg == ReactionTypes.Awkward)
                            {
                                PassionCommon.SimMessage(PassionCommon.Localize("Um... could you people get a room?").ToString(), player.Actor);
                            }
                            if (PassionCommon.RandomReactionNeg == ReactionTypes.Embarrassed)
                            {
                                PassionCommon.SimMessage(PassionCommon.Localize("Stop it!! That's too freaky for here!").ToString(), player.Actor);
                            }
                            if (PassionCommon.RandomReactionNeg == ReactionTypes.Boo)
                            {
                                PassionCommon.SimMessage(PassionCommon.Localize("Boo! Boooooo!! Cut it out!").ToString(), player.Actor);
                            }
                            if (PassionCommon.RandomReactionNeg == ReactionTypes.Inappropriate)
                            {
                                PassionCommon.SimMessage(PassionCommon.Localize("Ergh, no thanks. Do that somewhere else...").ToString(), player.Actor);
                            }
                            if (PassionCommon.RandomReactionNeg == ReactionTypes.FreakOut)
                            {
                                PassionCommon.SimMessage(PassionCommon.Localize("Is this seriously happening?! Here? In PUBLIC?!").ToString(), player.Actor);
                            }
                            if (PassionCommon.RandomReactionNeg == ReactionTypes.ThrowUp)
                            {
                                PassionCommon.SimMessage(PassionCommon.Localize("What the hell?! I'm gonna be sick...").ToString(), player.Actor);
                            }
                        }
                        if (player.Actor.InteractionQueue.Count > 1)
                        {
                            player.Actor.InteractionQueue.CancelInteraction(player.Actor.CurrentInteraction, false);
                        }
                    }

                    // END REFACTOR
                }
            }
            catch
            {
            }
        }

        public void StartJealousyBroadcast()
        {
            if (JealousyBroadcaster == null)
            {
                JealousyBroadcaster = new ReactionBroadcaster(Actor, Conversation.ReactToSocialParams, Autonomy.JealousyCheck);
            }
        }

        public void StopJealousyBroadcast()
        {
            if (JealousyBroadcaster != null)
            {
                JealousyBroadcaster.Dispose();
                JealousyBroadcaster = null;
            }
        }

        public bool ForceStartLoopImmediate()
        {
            if (IsValid)
            {
                State = PassionState.Routing;
                Actor.SetIsSleeping(false);
                Actor.InteractionQueue.CancelAllInteractions();
                Actor.InteractionQueue.AddNext(Interactions.BeginPassion.Singleton.CreateInstance(Actor, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                return true;
            }
            return false;
        }

        public bool ForceStartLoop()
        {
            if (IsValid)
            {
                Actor.SetIsSleeping(false);
                Actor.InteractionQueue.CancelAllInteractions();
                State = PassionState.Routing;
                return StartLoop();
            }
            return false;
        }

        public bool DirectStartLoop()
        {
            State = PassionState.Routing;
            return StartLoop();
        }


        // START THE PASSION LOOP
        public bool StartLoop()
        {
            if (IsValid && HasPart)
            {
                if (Part.Count >= Part.MaxSims)
                {
                    Leave();
                }
                ExitPoint = new Vector3(Actor.Position);
                if (!ActiveJoin)
                {
                    
                }
                else
                {
                    ActiveJoin = false;
                }
                SetPosture();
                Actor.LookAtManager.DisableLookAts();
                PassionCommon.Modules.PreProcessing(Actor);
                State = PassionState.Ready;
                PositionIndex = Part.Players.Count;
                Part.CurrentPositionInvalidate();
                Part.StartVisualEffects();
                Part.StartSoundEffects();
                Part.HahaFirst = true;
                Actor.InteractionQueue.PushAsContinuation(Interactions.PassionLoop.Singleton.CreateInstance(Actor, Actor, new InteractionPriority(InteractionPriorityLevel.High), false, true), true);
                return true;
            }
            return false;
        }

        // animations for objects
        public string Animation2Obj(string anim)
        {
            PersistableSettings.Settings.ObjectAnimation = null;
            if (anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutramakout_01") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1"))
            {
                PersistableSettings.Settings.ObjectAnimation = "a_L666_carsports3";
            }
            else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_01") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1") && !Part.Type.Is("BoatSpeedBoat"))
            {
                PersistableSettings.Settings.ObjectAnimation = "a_L666_carsports4";
            }
            else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_02") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1") && !Part.Type.Is("BoatSpeedBoat"))
            {
                PersistableSettings.Settings.ObjectAnimation = "a_L666_carsports";
            }
            else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_03") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1") && !Part.Type.Is("BoatSpeedBoat"))
            {
                PersistableSettings.Settings.ObjectAnimation = "a_L666_carsports2";
            }
            else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_threesomecarfmf_01o") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1"))
            {
                PersistableSettings.Settings.ObjectAnimation = "a_L666_threesomecarfmfcaranime";
            }
            else if (anim.Contains("KW_L666_Kinky_Animations.a_L666_threesomecarfmf_02o") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1"))
            {
                PersistableSettings.Settings.ObjectAnimation = "a_L666_threesomecarfmfcaranime2";
            }
            else if (anim.Contains("KW_Amra72_Animations.a2a_(Am)Car_Blowjob_01") && Part.Type.Is("CarSports") && !Part.Type.Is("CarExpensive1") && !Part.Type.Is("CarExpensive2") && !Part.Type.Is("CarHatchback") && !Part.Type.Is("CarUsed1") && !Part.Type.Is("BoatSpeedBoat"))
            {
                PersistableSettings.Settings.ObjectAnimation = "a_L666_carsports4";
            }
            else if (!anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutramakout_01") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_01") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_02") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_carmasutracowgrl_03") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_threesomecarfmf_01o") && !anim.Contains("KW_L666_Kinky_Animations.a_L666_threesomecarfmf_02o") && !anim.Contains("KW_Amra72_Animations.a2a_(Am)Car_Blowjob_01") && Part.Position.ObjectAnimation != null)
            {
                PersistableSettings.Settings.ObjectAnimation = Part.Position.ObjectAnimation;
            }
            return PersistableSettings.Settings.ObjectAnimation;
        }

        // THE BIG BOY, WHERE IT ALL HAPPENS
        public bool DoLoop()
        {

            OutfitCategories previousOutfitCategory = PreviousOutfitCategory;
            int previousOutfitIndex = PreviousOutfitIndex;
            string text = null;
            string text2 = null;
            PersistableSettings.Settings.RemoveCondom = false;
            try
            {
                PassionBase.ResetAnimationObject(Part.Target.Object);
                if (Part.Target.ObjectType.Name.ToString() == "Sybian")
                {
                    Part.Target.Object.RemoveInteractionByType(AskToUseSybian.Singleton);
                    Part.Target.Object.RemoveInteractionByType(Interactions.UseObjectForPassion.Singleton);
                    Part.Target.Object.RemoveInteractionByType(Interactions.ResetMe.Singleton);
                    Part.Target.Object.RemoveInteractionByType(Interactions.ResetMeActive.Singleton);
                    Part.Target.Object.RemoveInteractionByType(PutInInventory.Singleton);
                }
            }
            catch
            {
            }
            if (IsValid)
            {
                IsActive = true;
                try
                {
                    if (Actor.SimDescription.Teen)
                    {
                        RefreshHeightModifier(Actor);
                    }
                }
                catch
                {
                }
                SimDescription simDescription = Actor.SimDescription;

                //
                // check to see what junk a sim has
                // im sorry if any competent programmers see this
                // edit: is it less scary now. please clap
                //

                if (Actor.IsMale)
                {
                    PassionBase.GetPlayer(Actor).SimGenitalType = "penis";
                    if (simDescription.YoungAdultOrAdult)
                    {
                        PassionBase.GetPlayer(Actor).SimErectSIMO = PassionBody.FallbackErectPenisAM;
                    }
                    else if (simDescription.Elder)
                    {
                        PassionBase.GetPlayer(Actor).SimErectSIMO = PassionBody.FallbackErectPenisEM;
                    }
                    else if (simDescription.Teen)
                    {
                        PassionBase.GetPlayer(Actor).SimErectSIMO = PassionBody.FallbackErectPenisTM;
                    }
                }
                else
                {
                    PassionBase.GetPlayer(Actor).SimGenitalType = "vagina";
                }


                PenisInspection.GetBottomCASP2(Actor, PassionBase.GetPlayer(Actor));


                foreach (PassionBody.BodyShop body in PassionBody.coolbodyshop)
                {
                    string PartName = body.Name;
                    string PartType = body.GenitalType;
                    string PartBase = body.BaseCASP;
                    string PartErect = body.ErectSIMO;


                    if (simDescription.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString(PartBase)) != null)
                    {
                        // PassionCommon.SystemMessage("WE FOUND A MATCH BESTIES!!!!!\n" + PartName);
                        PassionBase.GetPlayer(Actor).SimGenitalType = PartType;
                        PassionBase.GetPlayer(Actor).SimJunkBaseCASP = PartBase;
                        PassionBase.GetPlayer(Actor).SimErectSIMO = PartErect;
                    }

                }


                SimDescription simDescription2 = Actor.SimDescription;
                SimDescription simDescription3 = Actor.SimDescription;
                SimDescription simDescription4 = Actor.SimDescription;
                SimDescription simDescription5 = Actor.SimDescription;
                SimDescription simDescription6 = Actor.SimDescription;
                SimDescription simDescription7 = Actor.SimDescription;
                SimDescription simDescription8 = Actor.SimDescription;
                SimDescription simDescription9 = Actor.SimDescription;
                SimDescription simDescription10 = Actor.SimDescription;
                SimDescription simDescription11 = Actor.SimDescription;
                SimDescription simDescription12 = Actor.SimDescription;
                SimDescription simDescription13 = Actor.SimDescription;
                SimDescription simDescription14 = Actor.SimDescription;
                SimDescription simDescription15 = Actor.SimDescription;
                SimDescription simDescription16 = Actor.SimDescription;
                SimDescription simDescription17 = Actor.SimDescription;
                SimDescription simDescription18 = Actor.SimDescription;
                SimDescription simDescription19 = Actor.SimDescription;
                SimDescription simDescription20 = Actor.SimDescription;

                StartTime = SimClock.CurrentTicks;
                PassionCommon.Modules.LoopProcessing(Actor);
                if (PersistableSettings.Settings.BroadCasterEnable)
                {
                    BroadcastPassionResult();
                }
                while (IsValid && HasPart)
                {


                    Actor.AddInteraction(CumOnFace.Singleton, true);
                    Actor.AddInteraction(CumOnTits.Singleton, true);
                    Actor.AddInteraction(CumOnButt.Singleton, true);
                    Actor.AddInteraction(CumOnBelly.Singleton, true);
                    Actor.AddInteraction(CumOnRightHand.Singleton, true);
                    Actor.AddInteraction(CumOnLeftHand.Singleton, true);
                    Actor.AddInteraction(CumOnRightFoot.Singleton, true);
                    Actor.AddInteraction(CumOnLeftFoot.Singleton, true);
                    Actor.AddInteraction(CumOnThigh.Singleton, true);
                    PassionBase.CumInteractions = true;
                    try
                    {
                        // get the animation for the object (mostly cars)
                        Animation2Obj(Part.Position.Name.ToString());
                    }
                    catch
                    {
                    }
                    try
                    {
                        foreach (Sim allActor in Actor.LotCurrent.GetAllActors())
                        {
                            Player player = PassionBase.GetPlayer(allActor);
                            if (allActor != null && allActor.RoomId == Actor.RoomId && !Actor.LotCurrent.IsWorldLot && !allActor.SimDescription.IsServicePerson && allActor != Actor && (allActor.SimDescription.ChildOrBelow || allActor.SimDescription.IsPet || allActor != Actor && allActor.SimDescription.Elder && PersistableSettings.Settings.AutonomyChance > 0))
                            {
                                if (allActor.InteractionQueue.Count <= 1 && PersistableSettings.Settings.ChildrenOut && allActor.SimDescription.ChildOrBelow)
                                {
                                    allActor.PlayReaction(ReactionTypes.Embarrassed, Actor, ReactionSpeed.Immediate);
                                    allActor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
                                }
                                else if (allActor.InteractionQueue.Count <= 1 && PersistableSettings.Settings.PetsOut && allActor.SimDescription.IsPet)
                                {
                                    allActor.PlayReaction(ReactionTypes.Embarrassed, Actor, ReactionSpeed.Immediate);
                                    allActor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
                                }
                                else if (allActor.InteractionQueue.Count <= 1 && PersistableSettings.Settings.EldersOut && allActor.SimDescription.Elder)
                                {
                                    allActor.PlayReaction(ReactionTypes.Embarrassed, Actor, ReactionSpeed.Immediate);
                                    allActor.InteractionQueue.AddNext(Interactions.Embarrassed.Singleton.CreateInstance(Actor, allActor, new InteractionPriority(InteractionPriorityLevel.Privacy), true, true));
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    if (CanAnimate)
                    {
                        text = Part.Position.Name.ToString();
                        Animate();
                    }
                    if (text != text2 && HeldItem != null)
                    {
                        HeldItem.Release();
                        HeldItem = null;
                    }
                    if (ShouldStop)
                    {
                        if (HeldItem != null)
                        {
                            HeldItem.Release();
                            HeldItem = null;
                        }
                        PersistableSettings.Settings.PassionFuckSession = false;
                        PersistableSettings.Settings.CondomIsBroken = false;
                        PersistableSettings.Settings.RemoveCondom = true;
                        try
                        {
                            // penis removal if the sim should stop
                            if (PassionBase.GetPlayer(Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Actor).SimGenitalType == "both")
                            {
                                SwitchToPeener(Actor, false);
                            }
                            else if (PassionBase.GetPlayer(Partner.Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Partner.Actor).SimGenitalType == "both")
                            {
                                SwitchToPeener(Partner.Actor, false);
                            }

                            // remove condom
                            if (PassionBase.GetPlayer(Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Actor).SimGenitalType == "both")
                            {
                                WearCondom(Actor, false);
                            }
                            else if (PassionBase.GetPlayer(Partner.Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Partner.Actor).SimGenitalType == "both")
                            {
                                WearCondom(Partner.Actor, false);
                            }

                        }
                        catch
                        {
                        }
                        previousOutfitCategory = PreviousOutfitCategory;
                        previousOutfitIndex = PreviousOutfitIndex;
                        try
                        {
                            // remove strap
                            if (PassionBase.GetPlayer(Actor).SimGenitalType == "vagina" || PassionBase.GetPlayer(Actor).SimGenitalType == "neither")
                            {
                                SwitchToStrapon(Actor, false);
                            }
                            else if (PassionBase.GetPlayer(Actor).SimGenitalType == "vagina" || PassionBase.GetPlayer(Actor).SimGenitalType == "neither")
                            {
                                SwitchToStrapon(Partner.Actor, false);
                            }

                        }
                        catch
                        {
                        }
                        try
                        {
                            PassionBase.CumInteractions = false;
                            Actor.RemoveInteractionByType(CumOnFace.Singleton);
                            Actor.RemoveInteractionByType(CumOnTits.Singleton);
                            Actor.RemoveInteractionByType(CumOnButt.Singleton);
                            Actor.RemoveInteractionByType(CumOnBelly.Singleton);
                            Actor.RemoveInteractionByType(CumOnRightHand.Singleton);
                            Actor.RemoveInteractionByType(CumOnLeftHand.Singleton);
                            Actor.RemoveInteractionByType(CumOnRightFoot.Singleton);
                            Actor.RemoveInteractionByType(CumOnLeftFoot.Singleton);
                            Actor.RemoveInteractionByType(CumOnThigh.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnFace.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnTits.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnButt.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnBelly.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnRightHand.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnLeftHand.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnRightFoot.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnLeftFoot.Singleton);
                            Partner.Actor.RemoveInteractionByType(CumOnThigh.Singleton);
                        }
                        catch
                        {
                        }
                        try
                        {
                            PassionBase.CumInteractions = false;
                            foreach (Sim allActor2 in Actor.LotCurrent.GetAllActors())
                            {
                                Player player2 = PassionBase.GetPlayer(allActor2);
                                if (allActor2 != null && allActor2.RoomId == Actor.RoomId || allActor2 != null && allActor2.RoomId == Partner.Actor.RoomId)
                                {
                                    allActor2.RemoveInteractionByType(CumOnFace.Singleton);
                                    allActor2.RemoveInteractionByType(CumOnTits.Singleton);
                                    allActor2.RemoveInteractionByType(CumOnButt.Singleton);
                                    allActor2.RemoveInteractionByType(CumOnBelly.Singleton);
                                    allActor2.RemoveInteractionByType(CumOnRightHand.Singleton);
                                    allActor2.RemoveInteractionByType(CumOnLeftHand.Singleton);
                                    allActor2.RemoveInteractionByType(CumOnRightFoot.Singleton);
                                    allActor2.RemoveInteractionByType(CumOnLeftFoot.Singleton);
                                    allActor2.RemoveInteractionByType(CumOnThigh.Singleton);
                                }
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            if (Part.Target.ObjectType.Name.ToString() == "Sybian")
                            {
                                Part.Target.Object.AddInteraction(AskToUseSybian.Singleton, true);
                                Part.Target.Object.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
                                Part.Target.Object.AddInteraction(Interactions.ResetMe.Singleton, true);
                                Part.Target.Object.AddInteraction(Interactions.ResetMeActive.Singleton, true);
                                Part.Target.Object.AddInteraction(PutInInventory.Singleton, true);
                            }
                        }
                        catch
                        {
                        }
                        break;
                    }
                    List<Player> ready;
                    if (IsInitiator && Part.CanAnimate(out ready) && Part.CheckSequence())
                    {
                        try
                        {
                            if (PassionBase.SwitchPlayerActor != null && PassionBase.SwitchPlayerActor == Actor && PassionBase.SwitchPlayerPartner != null && PassionBase.SwitchPlayerPartner == Partner.Actor)
                            {
                                Player player3 = PassionBase.GetPlayer(Actor);
                                Player player4 = PassionBase.GetPlayer(Partner.Actor);
                                player3.Switch(player4);
                            }
                        }
                        catch
                        {
                        }
                        if (Part.PositionChanged)
                        {
                            if (Part.Position == null)
                            {
                                break;
                            }
                            text2 = Part.Position.Name.ToString();
                            if (HeldItem != null)
                            {
                                HeldItem.Release();
                                HeldItem = null;
                            }
                            if (Part.Position != null)
                            {
                                Part.Position.GiveHeldItems(ready);
                            }
                        }
                        else if (!Part.PositionChanged)
                        {
                            if (HeldItem != null)
                            {
                                HeldItem.Release();
                                HeldItem = null;
                            }
                            if (Part.Position != null)
                            {
                                Part.Position.GiveHeldItems(ready);
                            }
                        }
                        if (Part.Players.Count != Part.Position.MaxSims && Part.Position.MaxSims.ToString() != null && Part.Position.MaxSims == 0)
                        {
                            Part.Position.MaxSims = 1;
                        }
                        else
                        {
                            if (Part.Position.MaxSims.ToString() == null || Part.Position.MaxSims == 0 || Part.Position == null)
                            {
                                break;
                            }
                            // if its a masturbation action, ie one sim with one max sim
                            if (Part.Players.Count == 1 && Part.Position.MaxSims == 1)
                            {
                                PersistableSettings.Settings.CondomIsBroken = true;
                                PersistableSettings.Settings.PassionFuckSession = true;
                                try
                                {

                                    //
                                    // if strapon IS meant to be used, sim has vagina or null
                                    if ((PassionBase.GetPlayer(Actor).SimGenitalType == "vagina" || PassionBase.GetPlayer(Actor).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(Actor, true);
                                    }
                                    // if strapon isn't meant to be used
                                    // EDIT; changing this to GetNaked
                                    else if ((PassionBase.GetPlayer(Actor).SimGenitalType == "vagina" || PassionBase.GetPlayer(Actor).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(Actor, true);
                                    }
                                    //end branch
                                    //

                                    // if actor has a penis
                                    if (PassionBase.GetPlayer(Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Actor).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(Actor, true);
                                    }
                                    // end branch
                                    //

                                }
                                catch
                                {
                                }
                            }

                            // ELSE if theres 2 sims, with 2 max sims
                            else if (Part.Players.Count == 2 && Part.Position.MaxSims == 2)
                            {
                                Sim sim = null;
                                Sim sim2 = null;
                                try
                                {
                                    foreach (Player item in ready)
                                    {
                                        if (item.PositionIndex == 1)
                                        {
                                            sim = item.Actor;
                                            continue;
                                        }
                                        if (item.PositionIndex == 2)
                                        {
                                            sim2 = item.Actor;
                                            continue;
                                        }
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                                simDescription3 = sim.SimDescription;
                                simDescription4 = sim2.SimDescription;
                                try
                                {
                                    // do i really have to repeat this code for every fucking participant. what the hell
                                    // edit now that i added GetNaked - THIS IS GOING TO SUCK!!!!!!!!!!!

                                    // SIM 1 STRAP START
                                    if ((PassionBase.GetPlayer(sim).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim, true);
                                    }

                                    // SIM 1 STRAP END

                                    // SIM 2 STRAP START
                                    if ((PassionBase.GetPlayer(sim2).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim2).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim2, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim2).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim2).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim2, true);
                                    }
                                    // SIM 2 STRAP END

                                    simDescription3 = sim.SimDescription;
                                    simDescription4 = sim2.SimDescription;

                                    // cock processing

                                    // SIM 1 PEEN START
                                    if (PassionBase.GetPlayer(sim).SimGenitalType == "penis" || PassionBase.GetPlayer(sim).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim, true);
                                    }
                                    // SIM 1 PEEN END

                                    // SIM 2 PEEN START
                                    if (PassionBase.GetPlayer(sim2).SimGenitalType == "penis" || PassionBase.GetPlayer(sim2).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim2, true);
                                    }
                                    // SIM 2 PEEN END
                                }
                                catch
                                {
                                }
                                try
                                {
                                    // see if sim should NOT be wearing condom
                                    if (!PersistableSettings.Settings.UseCondom || PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Actor).SimGenitalType == "both")
                                        {
                                            WearCondom(Actor, false);
                                        }
                                        // check the partner too
                                        if (PassionBase.GetPlayer(Partner.Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Partner.Actor).SimGenitalType == "both")
                                        {
                                            WearCondom(Partner.Actor, false);
                                        }
                                    }
                                    // see if sim SHOULD be wearing condom
                                    if (PersistableSettings.Settings.UseCondom && !PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Actor).SimGenitalType == "both")
                                        {
                                            WearCondom(Actor, true);
                                        }
                                        else if (PassionBase.GetPlayer(Partner.Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Partner.Actor).SimGenitalType == "both")
                                        {
                                            WearCondom(Partner.Actor, true);
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }

                            // if theres 3 sims (kill me)
                            else if (Part.Players.Count == 3 && Part.Position.MaxSims == 3)
                            {
                                Sim sim3 = null;
                                Sim sim4 = null;
                                Sim sim5 = null;
                                try
                                {
                                    foreach (Player item2 in ready)
                                    {
                                        if (item2.PositionIndex == 1)
                                        {
                                            sim3 = item2.Actor;
                                            continue;
                                        }
                                        if (item2.PositionIndex == 2)
                                        {
                                            sim4 = item2.Actor;
                                            continue;
                                        }
                                        if (item2.PositionIndex == 3)
                                        {
                                            sim5 = item2.Actor;
                                            continue;
                                        }
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                                simDescription3 = sim3.SimDescription;
                                simDescription4 = sim4.SimDescription;
                                simDescription5 = sim5.SimDescription;
                                try
                                {
                                    // SIM 3 STRAP START
                                    if ((PassionBase.GetPlayer(sim3).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim3).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim3, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim3).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim3).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim3, true);
                                    }
                                    // SIM 3 STRAP END

                                    // SIM 4 STRAP START
                                    if ((PassionBase.GetPlayer(sim4).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim4).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim4, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim4).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim4).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim4, true);
                                    }
                                    // SIM 4 STRAP END

                                    // SIM 5 STRAP START
                                    if ((PassionBase.GetPlayer(sim5).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim5).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim5, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim5).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim5).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim5, true);
                                    }
                                    // SIM 5 STRAP END

                                    //
                                    // cock time
                                    //

                                    // SIM 3 PEEN START
                                    if (PassionBase.GetPlayer(sim3).SimGenitalType == "penis" || PassionBase.GetPlayer(sim3).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim3, true);
                                    }
                                    // SIM 3 PEEN END

                                    // SIM 4 PEEN START
                                    if (PassionBase.GetPlayer(sim4).SimGenitalType == "penis" || PassionBase.GetPlayer(sim4).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim4, true);
                                    }
                                    // SIM 4 PEEN END

                                    // SIM 5 PEEN START
                                    if (PassionBase.GetPlayer(sim5).SimGenitalType == "penis" || PassionBase.GetPlayer(sim5).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim5, true);
                                    }
                                    // SIM 5 PEEN END


                                    // three sim condom processing

                                    if (PersistableSettings.Settings.UseCondom && !PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(sim3).SimGenitalType == "penis" || PassionBase.GetPlayer(sim3).SimGenitalType == "both")
                                        {
                                            WearCondom(sim3, true);
                                        }
                                        if (PassionBase.GetPlayer(sim4).SimGenitalType == "penis" || PassionBase.GetPlayer(sim4).SimGenitalType == "both")
                                        {
                                            WearCondom(sim4, true);
                                        }
                                        if (PassionBase.GetPlayer(sim5).SimGenitalType == "penis" || PassionBase.GetPlayer(sim5).SimGenitalType == "both")
                                        {
                                            WearCondom(sim5, true);
                                        }
                                    }
                                    else if (!PersistableSettings.Settings.UseCondom || PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(sim3).SimGenitalType == "penis" || PassionBase.GetPlayer(sim3).SimGenitalType == "both")
                                        {
                                            WearCondom(sim3, false);
                                        }
                                        if (PassionBase.GetPlayer(sim4).SimGenitalType == "penis" || PassionBase.GetPlayer(sim4).SimGenitalType == "both")
                                        {
                                            WearCondom(sim4, false);
                                        }
                                        if (PassionBase.GetPlayer(sim5).SimGenitalType == "penis" || PassionBase.GetPlayer(sim5).SimGenitalType == "both")
                                        {
                                            WearCondom(sim5, false);
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                            else if (Part.Players.Count == 4 && Part.Position.MaxSims == 4)
                            {
                                Sim sim6 = null;
                                Sim sim7 = null;
                                Sim sim8 = null;
                                Sim sim9 = null;
                                try
                                {
                                    foreach (Player item3 in ready)
                                    {
                                        if (item3.PositionIndex == 1)
                                        {
                                            sim6 = item3.Actor;
                                            continue;
                                        }
                                        if (item3.PositionIndex == 2)
                                        {
                                            sim7 = item3.Actor;
                                            continue;
                                        }
                                        if (item3.PositionIndex == 3)
                                        {
                                            sim8 = item3.Actor;
                                            continue;
                                        }
                                        if (item3.PositionIndex == 4)
                                        {
                                            sim9 = item3.Actor;
                                            continue;
                                        }
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                                simDescription6 = sim6.SimDescription;
                                simDescription7 = sim7.SimDescription;
                                simDescription8 = sim8.SimDescription;
                                simDescription9 = sim9.SimDescription;
                                try
                                {

                                    // SIM 6 STRAP START
                                    if ((PassionBase.GetPlayer(sim6).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim6).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim6, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim6).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim6).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim6, true);
                                    }
                                    // SIM 6 STRAP END

                                    // SIM 7 STRAP START
                                    if ((PassionBase.GetPlayer(sim7).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim7).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim7, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim7).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim7).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim7, true);
                                    }
                                    // SIM 7 STRAP END

                                    // SIM 8 STRAP START
                                    if ((PassionBase.GetPlayer(sim8).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim8).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim8, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim8).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim8).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim8, true);
                                    }
                                    // SIM 8 STRAP END

                                    // SIM 9 STRAP START
                                    if ((PassionBase.GetPlayer(sim9).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim9).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim9, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim9).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim9).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim9, true);
                                    }
                                    // SIM 9 STRAP END

                                    // SIM 6 PEEN START
                                    if (PassionBase.GetPlayer(sim6).SimGenitalType == "penis" || PassionBase.GetPlayer(sim6).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim6, true);
                                    }
                                    // SIM 6 PEEN END

                                    // SIM 7 PEEN START
                                    if (PassionBase.GetPlayer(sim7).SimGenitalType == "penis" || PassionBase.GetPlayer(sim7).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim7, true);
                                    }
                                    // SIM 7 PEEN END

                                    // SIM 8 PEEN START
                                    if (PassionBase.GetPlayer(sim8).SimGenitalType == "penis" || PassionBase.GetPlayer(sim8).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim8, true);
                                    }
                                    // SIM 8 PEEN END

                                    // SIM 9 PEEN START
                                    if (PassionBase.GetPlayer(sim9).SimGenitalType == "penis" || PassionBase.GetPlayer(sim9).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim9, true);
                                    }
                                    // SIM 9 PEEN END


                                    // condom processing

                                    if (PersistableSettings.Settings.UseCondom && !PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(sim6).SimGenitalType == "penis" || PassionBase.GetPlayer(sim6).SimGenitalType == "both")
                                        {
                                            WearCondom(sim6, true);
                                        }
                                        if (PassionBase.GetPlayer(sim7).SimGenitalType == "penis" || PassionBase.GetPlayer(sim7).SimGenitalType == "both")
                                        {
                                            WearCondom(sim7, true);
                                        }
                                        if (PassionBase.GetPlayer(sim8).SimGenitalType == "penis" || PassionBase.GetPlayer(sim8).SimGenitalType == "both")
                                        {
                                            WearCondom(sim8, true);
                                        }
                                        if (PassionBase.GetPlayer(sim9).SimGenitalType == "penis" || PassionBase.GetPlayer(sim9).SimGenitalType == "both")
                                        {
                                            WearCondom(sim9, true);
                                        }
                                    }
                                    else if (!PersistableSettings.Settings.UseCondom || PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(sim6).SimGenitalType == "penis" || PassionBase.GetPlayer(sim6).SimGenitalType == "both")
                                        {
                                            WearCondom(sim6, false);
                                        }
                                        if (PassionBase.GetPlayer(sim7).SimGenitalType == "penis" || PassionBase.GetPlayer(sim7).SimGenitalType == "both")
                                        {
                                            WearCondom(sim7, false);
                                        }
                                        if (PassionBase.GetPlayer(sim8).SimGenitalType == "penis" || PassionBase.GetPlayer(sim8).SimGenitalType == "both")
                                        {
                                            WearCondom(sim8, false);
                                        }
                                        if (PassionBase.GetPlayer(sim9).SimGenitalType == "penis" || PassionBase.GetPlayer(sim9).SimGenitalType == "both")
                                        {
                                            WearCondom(sim9, false);
                                        }
                                    }

                                }
                                catch
                                {
                                }
                            }
                            else if (Part.Players.Count == 5 && Part.Position.MaxSims == 5)
                            {
                                Sim sim10 = null;
                                Sim sim11 = null;
                                Sim sim12 = null;
                                Sim sim13 = null;
                                Sim sim14 = null;
                                try
                                {
                                    foreach (Player item4 in ready)
                                    {
                                        if (item4.PositionIndex == 1)
                                        {
                                            sim10 = item4.Actor;
                                            continue;
                                        }
                                        if (item4.PositionIndex == 2)
                                        {
                                            sim11 = item4.Actor;
                                            continue;
                                        }
                                        if (item4.PositionIndex == 3)
                                        {
                                            sim12 = item4.Actor;
                                            continue;
                                        }
                                        if (item4.PositionIndex == 4)
                                        {
                                            sim13 = item4.Actor;
                                            continue;
                                        }
                                        if (item4.PositionIndex == 5)
                                        {
                                            sim14 = item4.Actor;
                                            continue;
                                        }
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                                simDescription10 = sim10.SimDescription;
                                simDescription11 = sim11.SimDescription;
                                simDescription12 = sim12.SimDescription;
                                simDescription13 = sim13.SimDescription;
                                simDescription14 = sim14.SimDescription;
                                try
                                {

                                    // SIM 10 STRAP START
                                    if ((PassionBase.GetPlayer(sim10).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim10).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim10, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim10).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim10).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim10, true);
                                    }
                                    // SIM 10 STRAP END

                                    // SIM 11 STRAP START
                                    if ((PassionBase.GetPlayer(sim11).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim11).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim11, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim11).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim11).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim11, true);
                                    }
                                    // SIM 11 STRAP END

                                    // SIM 12 STRAP START
                                    if ((PassionBase.GetPlayer(sim12).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim12).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim12, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim12).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim12).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim12, true);
                                    }
                                    // SIM 12 STRAP END

                                    // SIM 13 STRAP START
                                    if ((PassionBase.GetPlayer(sim13).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim13).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim13, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim13).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim13).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim13, true);
                                    }
                                    // SIM 13 STRAP END

                                    // SIM 14 STRAP START
                                    if ((PassionBase.GetPlayer(sim14).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim14).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim14, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim14).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim14).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim14, true);
                                    }
                                    // SIM 14 STRAP END

                                    // SIM 10 PEEN START
                                    if (PassionBase.GetPlayer(sim10).SimGenitalType == "penis" || PassionBase.GetPlayer(sim10).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim10, true);
                                    }
                                    // SIM 10 PEEN END

                                    // SIM 11 PEEN START
                                    if (PassionBase.GetPlayer(sim11).SimGenitalType == "penis" || PassionBase.GetPlayer(sim11).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim11, true);
                                    }
                                    // SIM 11 PEEN END

                                    // SIM 12 PEEN START
                                    if (PassionBase.GetPlayer(sim12).SimGenitalType == "penis" || PassionBase.GetPlayer(sim12).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim12, true);
                                    }
                                    // SIM 12 PEEN END

                                    // SIM 13 PEEN START
                                    if (PassionBase.GetPlayer(sim13).SimGenitalType == "penis" || PassionBase.GetPlayer(sim13).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim13, true);
                                    }
                                    // SIM 13 PEEN END

                                    // SIM 14 PEEN START
                                    if (PassionBase.GetPlayer(sim14).SimGenitalType == "penis" || PassionBase.GetPlayer(sim14).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim14, true);
                                    }
                                    // SIM 14 PEEN END


                                    // condom processing

                                    if (PersistableSettings.Settings.UseCondom && !PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(sim10).SimGenitalType == "penis" || PassionBase.GetPlayer(sim10).SimGenitalType == "both")
                                        {
                                            WearCondom(sim10, true);
                                        }
                                        if (PassionBase.GetPlayer(sim11).SimGenitalType == "penis" || PassionBase.GetPlayer(sim11).SimGenitalType == "both")
                                        {
                                            WearCondom(sim11, true);
                                        }
                                        if (PassionBase.GetPlayer(sim12).SimGenitalType == "penis" || PassionBase.GetPlayer(sim12).SimGenitalType == "both")
                                        {
                                            WearCondom(sim12, true);
                                        }
                                        if (PassionBase.GetPlayer(sim13).SimGenitalType == "penis" || PassionBase.GetPlayer(sim13).SimGenitalType == "both")
                                        {
                                            WearCondom(sim13, true);
                                        }
                                        if (PassionBase.GetPlayer(sim14).SimGenitalType == "penis" || PassionBase.GetPlayer(sim14).SimGenitalType == "both")
                                        {
                                            WearCondom(sim14, true);
                                        }
                                    }
                                    else if (!PersistableSettings.Settings.UseCondom || PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(sim10).SimGenitalType == "penis" || PassionBase.GetPlayer(sim10).SimGenitalType == "both")
                                        {
                                            WearCondom(sim10, false);
                                        }
                                        if (PassionBase.GetPlayer(sim11).SimGenitalType == "penis" || PassionBase.GetPlayer(sim11).SimGenitalType == "both")
                                        {
                                            WearCondom(sim11, false);
                                        }
                                        if (PassionBase.GetPlayer(sim12).SimGenitalType == "penis" || PassionBase.GetPlayer(sim12).SimGenitalType == "both")
                                        {
                                            WearCondom(sim12, false);
                                        }
                                        if (PassionBase.GetPlayer(sim13).SimGenitalType == "penis" || PassionBase.GetPlayer(sim13).SimGenitalType == "both")
                                        {
                                            WearCondom(sim13, false);
                                        }
                                        if (PassionBase.GetPlayer(sim14).SimGenitalType == "penis" || PassionBase.GetPlayer(sim14).SimGenitalType == "both")
                                        {
                                            WearCondom(sim14, false);
                                        }
                                    }

                                }
                                catch
                                {
                                }
                            }
                            else if (Part.Players.Count == 6 && Part.Position.MaxSims == 6)
                            {
                                Sim sim15 = null;
                                Sim sim16 = null;
                                Sim sim17 = null;
                                Sim sim18 = null;
                                Sim sim19 = null;
                                Sim sim20 = null;
                                try
                                {
                                    foreach (Player item5 in ready)
                                    {
                                        if (item5.PositionIndex == 1)
                                        {
                                            sim15 = item5.Actor;
                                            continue;
                                        }
                                        if (item5.PositionIndex == 2)
                                        {
                                            sim16 = item5.Actor;
                                            continue;
                                        }
                                        if (item5.PositionIndex == 3)
                                        {
                                            sim17 = item5.Actor;
                                            continue;
                                        }
                                        if (item5.PositionIndex == 4)
                                        {
                                            sim18 = item5.Actor;
                                            continue;
                                        }
                                        if (item5.PositionIndex == 5)
                                        {
                                            sim19 = item5.Actor;
                                            continue;
                                        }
                                        if (item5.PositionIndex == 6)
                                        {
                                            sim20 = item5.Actor;
                                            continue;
                                        }
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                                simDescription15 = sim15.SimDescription;
                                simDescription16 = sim16.SimDescription;
                                simDescription17 = sim17.SimDescription;
                                simDescription18 = sim18.SimDescription;
                                simDescription19 = sim19.SimDescription;
                                simDescription20 = sim20.SimDescription;
                                try
                                {

                                    // SIM 15 STRAP START
                                    if ((PassionBase.GetPlayer(sim15).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim15).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim15, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim15).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim15).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim15, true);
                                    }
                                    // SIM 15 STRAP END

                                    // SIM 16 STRAP START
                                    if ((PassionBase.GetPlayer(sim16).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim16).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim16, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim16).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim16).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim16, true);
                                    }
                                    // SIM 16 STRAP END

                                    // SIM 17 STRAP START
                                    if ((PassionBase.GetPlayer(sim17).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim17).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim17, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim17).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim17).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim17, true);
                                    }
                                    // SIM 17 STRAP END

                                    // SIM 18 STRAP START
                                    if ((PassionBase.GetPlayer(sim18).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim18).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim18, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim18).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim18).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim18, true);
                                    }
                                    // SIM 18 STRAP END

                                    // SIM 19 STRAP START
                                    if ((PassionBase.GetPlayer(sim19).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim19).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim19, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim19).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim19).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim19, true);
                                    }
                                    // SIM 19 STRAP END

                                    // SIM 20 STRAP START
                                    if ((PassionBase.GetPlayer(sim20).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim20).SimGenitalType == "neither") && PersistableSettings.Settings.FemaleUseStrapOn && Part.Position.PutOnStraOn[1])
                                    {
                                        SwitchToStrapon(sim20, true);
                                    }
                                    // if strapon isn't meant to be used
                                    else if ((PassionBase.GetPlayer(sim20).SimGenitalType == "vagina" || PassionBase.GetPlayer(sim20).SimGenitalType == "neither") && !Part.Position.PutOnStraOn[1])
                                    {
                                        GetNaked(sim20, true);
                                    }
                                    // SIM 20 STRAP END

                                    // SIM 15 PEEN START
                                    if (PassionBase.GetPlayer(sim15).SimGenitalType == "penis" || PassionBase.GetPlayer(sim15).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim15, true);
                                    }
                                    // SIM 15 PEEN END

                                    // SIM 16 PEEN START
                                    if (PassionBase.GetPlayer(sim16).SimGenitalType == "penis" || PassionBase.GetPlayer(sim16).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim16, true);
                                    }
                                    // SIM 16 PEEN END

                                    // SIM 17 PEEN START
                                    if (PassionBase.GetPlayer(sim17).SimGenitalType == "penis" || PassionBase.GetPlayer(sim17).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim17, true);
                                    }
                                    // SIM 17 PEEN END

                                    // SIM 18 PEEN START
                                    if (PassionBase.GetPlayer(sim18).SimGenitalType == "penis" || PassionBase.GetPlayer(sim18).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim18, true);
                                    }
                                    // SIM 18 PEEN END

                                    // SIM 19 PEEN START
                                    if (PassionBase.GetPlayer(sim19).SimGenitalType == "penis" || PassionBase.GetPlayer(sim19).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim19, true);
                                    }
                                    // SIM 19 PEEN END

                                    // SIM 20 PEEN START
                                    if (PassionBase.GetPlayer(sim20).SimGenitalType == "penis" || PassionBase.GetPlayer(sim20).SimGenitalType == "both")
                                    {
                                        SwitchToPeener(sim20, true);
                                    }
                                    // SIM 20 PEEN END


                                    // condom processing

                                    if (PersistableSettings.Settings.UseCondom && !PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(sim15).SimGenitalType == "penis" || PassionBase.GetPlayer(sim15).SimGenitalType == "both")
                                        {
                                            WearCondom(sim15, true);
                                        }
                                        if (PassionBase.GetPlayer(sim16).SimGenitalType == "penis" || PassionBase.GetPlayer(sim16).SimGenitalType == "both")
                                        {
                                            WearCondom(sim16, true);
                                        }
                                        if (PassionBase.GetPlayer(sim17).SimGenitalType == "penis" || PassionBase.GetPlayer(sim17).SimGenitalType == "both")
                                        {
                                            WearCondom(sim17, true);
                                        }
                                        if (PassionBase.GetPlayer(sim18).SimGenitalType == "penis" || PassionBase.GetPlayer(sim18).SimGenitalType == "both")
                                        {
                                            WearCondom(sim18, true);
                                        }
                                        if (PassionBase.GetPlayer(sim19).SimGenitalType == "penis" || PassionBase.GetPlayer(sim19).SimGenitalType == "both")
                                        {
                                            WearCondom(sim19, true);
                                        }
                                        if (PassionBase.GetPlayer(sim20).SimGenitalType == "penis" || PassionBase.GetPlayer(sim20).SimGenitalType == "both")
                                        {
                                            WearCondom(sim20, true);
                                        }
                                    }
                                    else if (!PersistableSettings.Settings.UseCondom || PersistableSettings.Settings.CondomIsBroken)
                                    {
                                        if (PassionBase.GetPlayer(sim15).SimGenitalType == "penis" || PassionBase.GetPlayer(sim15).SimGenitalType == "both")
                                        {
                                            WearCondom(sim15, false);
                                        }
                                        if (PassionBase.GetPlayer(sim16).SimGenitalType == "penis" || PassionBase.GetPlayer(sim16).SimGenitalType == "both")
                                        {
                                            WearCondom(sim16, false);
                                        }
                                        if (PassionBase.GetPlayer(sim17).SimGenitalType == "penis" || PassionBase.GetPlayer(sim17).SimGenitalType == "both")
                                        {
                                            WearCondom(sim17, false);
                                        }
                                        if (PassionBase.GetPlayer(sim18).SimGenitalType == "penis" || PassionBase.GetPlayer(sim18).SimGenitalType == "both")
                                        {
                                            WearCondom(sim18, false);
                                        }
                                        if (PassionBase.GetPlayer(sim19).SimGenitalType == "penis" || PassionBase.GetPlayer(sim19).SimGenitalType == "both")
                                        {
                                            WearCondom(sim19, false);
                                        }
                                        if (PassionBase.GetPlayer(sim20).SimGenitalType == "penis" || PassionBase.GetPlayer(sim20).SimGenitalType == "both")
                                        {
                                            WearCondom(sim20, false);
                                        }
                                    }

                                }
                                catch
                                {
                                }
                            }
                        }
                        foreach (Player item6 in ready)
                        {
                            item6.CanAnimate = true;
                        }
                    }

                    PassionCommon.Wait();

                }
                ImproveRelationships();
                EndMotiveUpdates();
            }
            if (HasPart)
            {
                Leave();
                Reset();
            }
            try
            {
                //get out of nakey outfit, take off peen and condom

                SwitchToPeener(Actor, false);
                if (previousOutfitCategory != OutfitCategories.Naked)
                {
                    Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
                }
                SwitchToPeener(Partner.Actor, false);
                if (previousOutfitCategory != OutfitCategories.Naked)
                {
                    Partner.Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
                }
                WearCondom(Actor, false);
                if (previousOutfitCategory != OutfitCategories.Naked)
                {
                    Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
                }
                WearCondom(Partner.Actor, false);
                if (previousOutfitCategory != OutfitCategories.Naked)
                {
                    Partner.Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
                }
                SwitchToStrapon(Actor, false);
                if (previousOutfitCategory != OutfitCategories.Naked)
                {
                    Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
                }
                SwitchToStrapon(Partner.Actor, false);
                if (previousOutfitCategory != OutfitCategories.Naked)
                {
                    Partner.Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
                }
                GetNaked(Actor, false);
                if (previousOutfitCategory != OutfitCategories.Naked)
                {
                    Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
                }
                GetNaked(Partner.Actor, false);
                if (previousOutfitCategory != OutfitCategories.Naked)
                {
                    Partner.Actor.SwitchToOutfitWithoutSpin(previousOutfitCategory, previousOutfitIndex);
                }
            }
            catch
            {
            }
            try
            {
                PassionBase.CumInteractions = false;
                Actor.RemoveInteractionByType(CumOnFace.Singleton);
                Actor.RemoveInteractionByType(CumOnTits.Singleton);
                Actor.RemoveInteractionByType(CumOnButt.Singleton);
                Actor.RemoveInteractionByType(CumOnBelly.Singleton);
                Actor.RemoveInteractionByType(CumOnRightHand.Singleton);
                Actor.RemoveInteractionByType(CumOnLeftHand.Singleton);
                Actor.RemoveInteractionByType(CumOnRightFoot.Singleton);
                Actor.RemoveInteractionByType(CumOnLeftFoot.Singleton);
                Actor.RemoveInteractionByType(CumOnThigh.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnFace.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnTits.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnButt.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnBelly.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnRightHand.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnLeftHand.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnRightFoot.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnLeftFoot.Singleton);
                Partner.Actor.RemoveInteractionByType(CumOnThigh.Singleton);
            }
            catch
            {
            }
            try
            {
                if (Part.Target.ObjectType.Name.ToString() == "Sybian")
                {
                    Part.Target.Object.AddInteraction(AskToUseSybian.Singleton, true);
                    Part.Target.Object.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
                    Part.Target.Object.AddInteraction(Interactions.ResetMe.Singleton, true);
                    Part.Target.Object.AddInteraction(Interactions.ResetMeActive.Singleton, true);
                    Part.Target.Object.AddInteraction(PutInInventory.Singleton, true);
                }
            }
            catch
            {
            }
            return false;
        }

        // apply condom to sim
        public bool WearCondom(Sim PlayerSim, bool CondomOnDick)
        {
            ResourceKey key = ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8");
            SimDescription simDescription = PlayerSim.SimDescription;
            if (PersistableSettings.Settings.UseCondom && !World.ResourceExists(key))
            {
                PassionCommon.SystemMessage("Condom Accessories not found. Setting is now disabled!");
                PersistableSettings.Settings.UseCondom = false;
                PersistableSettings.Settings.CondomIsBroken = false;
                return false;
            }
            try
            {
                if (CondomOnDick && !PersistableSettings.Settings.CondomIsBroken && PersistableSettings.Settings.UseCondom)
                {
                    SimDescription simDescription2 = PlayerSim.SimDescription;
                    if (simDescription2.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8")) == null)
                    {
                        if (!RandomUtil.CoinFlip())
                        {
                            SimOutfit uniform = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0x28A1DEBB221B6632"));
                            SimOutfit resultOutfit;
                            if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), uniform, simDescription2, "RedCondom", out resultOutfit))
                            {
                                simDescription2.AddOutfit(resultOutfit, OutfitCategories.Naked, true);
                                SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
                                SwitchOutfitHelper.Start();
                                SwitchOutfitHelper.Wait(false);
                                try
                                {
                                    PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
                                }
                                catch
                                {
                                }
                                return true;
                            }
                        }
                        else if (RandomUtil.CoinFlip())
                        {
                            SimOutfit uniform2 = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0x51E7AC8E736EDB12"));
                            SimOutfit resultOutfit2;
                            if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), uniform2, simDescription2, "GreenCondom", out resultOutfit2))
                            {
                                simDescription2.AddOutfit(resultOutfit2, OutfitCategories.Naked, true);
                                SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
                                SwitchOutfitHelper.Start();
                                SwitchOutfitHelper.Wait(false);
                                try
                                {
                                    PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit2, 0);
                                }
                                catch
                                {
                                }
                                return true;
                            }
                        }
                        else
                        {
                            SimOutfit uniform3 = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0xFCB5B98600AE92C8"));
                            SimOutfit resultOutfit3;
                            if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), uniform3, simDescription2, "WhiteCondom", out resultOutfit3))
                            {
                                simDescription2.AddOutfit(resultOutfit3, OutfitCategories.Naked, true);
                                SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
                                SwitchOutfitHelper.Start();
                                SwitchOutfitHelper.Wait(false);
                                try
                                {
                                    PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit3, 0);
                                }
                                catch
                                {
                                }
                                return true;
                            }
                        }
                    }
                }
                else if (!CondomOnDick)
                {
                    SimDescription simDescription3 = PlayerSim.SimDescription;
                    if (simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1 && simDescription3.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8")) != null && PersistableSettings.Settings.CondomIsBroken || simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1 && simDescription3.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8")) != null && !PersistableSettings.Settings.UseCondom || simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1 && simDescription3.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x5BF7D41C6F2D94E8")) != null && PersistableSettings.Settings.RemoveCondom)
                    {
                        if (simDescription3.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x0603B3F0BE3C7883")) == null)
                        {
                            while (simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1)
                            {
                                simDescription3.RemoveOutfit(OutfitCategories.Naked, 0, true);
                            }
                            try
                            {
                                PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
                            }
                            catch
                            {
                            }
                        }
                        return false;
                    }
                }
            }
            catch
            {
            }
            return false;
        }


        // attach strapon to dickless sim
        public bool SwitchToStrapon(Sim PlayerSim, bool AddRemove)
        {
            SimDescription simDescription = PlayerSim.SimDescription;

            try
            {
                // if straps are disabled (aka for cowards)
                if (!PersistableSettings.Settings.FemaleUseStrapOn)
                {
                    return false;
                }
                // if strapon has been force toggled off
                if (PassionBase.GetPlayer(PlayerSim).ForceNoStrap)
                {
                    return false;
                }
                // if we're adding it
                if (AddRemove)
                {


                    SimDescription simDescription2 = PlayerSim.SimDescription;
                    if (simDescription2.GetOutfitCount(OutfitCategories.Naked) == 1)
                    {
                        // generate new outfit
                        SimOutfit uniform = null;

                        // if sim is female
                        if (simDescription.IsFemale)
                        {
                            uniform = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0xF32C06036EFA3D8E"));
                        }
                        // if sim is male
                        else
                        {
                            uniform = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0xF32C06036EFA3D85"));
                        }

                        SimOutfit resultOutfit;
                        if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), uniform, simDescription2, "Strapon", out resultOutfit))
                        {
                            simDescription2.AddOutfit(resultOutfit, OutfitCategories.Naked, true);
                            SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
                            SwitchOutfitHelper.Start();
                            SwitchOutfitHelper.Wait(false);
                            try
                            {
                                PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
                            }
                            catch
                            {
                            }
                            PassionBase.GetPlayer(PlayerSim).StrapIsOn = true;
                            return true;
                        }
                    }
                }
                // end strap addition

                // if we're removing it
                else if (!AddRemove)
                {
                    SimDescription simDescription3 = PlayerSim.SimDescription;
                    if (simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1)
                    {
                        while (simDescription3.GetOutfitCount(OutfitCategories.Naked) > 1)
                        {
                            simDescription3.RemoveOutfit(OutfitCategories.Naked, 0, true);
                        }
                        try
                        {
                            PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
                        }
                        catch
                        {
                        }
                        PassionBase.GetPlayer(PlayerSim).StrapIsOn = false;
                        return false;
                    }
                }
                // end strap removal
            }
            catch
            {
            }
            return false;
        }

        // add spaceman peener to sims with peens
        //... so thats what that meant
        public bool SwitchToPeener(Sim PlayerSim, bool AddIt)
        {

            if (PassionBase.GetPlayer(PlayerSim).SimGenitalType == "penis" || PassionBase.GetPlayer(PlayerSim).SimGenitalType == "both")
            {

                // if we're adding it
                if (AddIt && !PassionBase.GetPlayer(PlayerSim).PeenIsErect)
                {
                    SimDescription simDescription = PlayerSim.SimDescription;
                    string ErectPeen = PassionBase.GetPlayer(PlayerSim).SimErectSIMO;
                    CASPart junk;
                    junk = new CASPart(ResourceKey.FromString(ErectPeen));
                    CASPart NakeyTop;
                    NakeyTop = new CASPart(ResourceKey.FromString(PassionBase.GetPlayer(PlayerSim).nudeTopRK));

                    SimBuilder simBuilder = new SimBuilder();
                    simBuilder.UseCompression = true;

                    // get current outfit
                    OutfitUtils.SetOutfit(simBuilder, PlayerSim.CurrentOutfit, simDescription);


                    // figure out our nudity type (if this doesnt work im going to fucking kill someone)
                    if (PassionBase.GetPlayer(PlayerSim).UndressLevel == "LowerBody")
                    {
                        CASPart[] parts = PlayerSim.CurrentOutfit.Parts;
                        for (int i = 0; i < parts.Length; i++)
                        {
                            CASPart part = parts[i];
                            if (part.BodyType == BodyTypes.LowerBody)
                            {
                                simBuilder.RemovePart(part);
                            }
                            // if theyre wearing fullbody we wanna remove that too and and the top :p
                            else if (part.BodyType == BodyTypes.FullBody)
                            {
                                simBuilder.RemovePart(part);
                                simBuilder.AddPart(NakeyTop);
                            }
                        }
                    }
                    else if (PassionBase.GetPlayer(PlayerSim).UndressLevel == "UpperBody")
                    {
                        CASPart[] parts = PlayerSim.CurrentOutfit.Parts;
                        for (int i = 0; i < parts.Length; i++)
                        {
                            CASPart part = parts[i];
                            if (part.BodyType == BodyTypes.UpperBody)
                            {
                                simBuilder.RemovePart(part);
                            }
                        }
                    }
                    else
                    {
                        CASPart[] parts = PlayerSim.CurrentOutfit.Parts;
                        for (int i = 0; i < parts.Length; i++)
                        {
                            CASPart part = parts[i];
                            if (part.BodyType == BodyTypes.FullBody)
                            {
                                simBuilder.RemovePart(part);
                            }
                        }
                    }


                    CASPart part2 = junk;
                    // add the dong
                    simBuilder.AddPart(part2);

                    // special checks if undress type is fullbody or top, so we can add the sim's top too


                    if (PassionBase.GetPlayer(PlayerSim).UndressLevel != "LowerBody")
                    {
                        simBuilder.AddPart(NakeyTop);
                    }



                    ResourceKey key = simBuilder.CacheOutfit("BOE_Erect" + simDescription.SimDescriptionId);
                    SimOutfit uniform = new SimOutfit(key);


                    //if (OutfitUtils.TryApplyUniformToOutfit(simDescription.GetOutfit(OutfitCategories.Naked, 0), uniform, simDescription, "imdying", out resultOutfit))
                    //{
                    simDescription.AddOutfit(uniform, OutfitCategories.Naked, 0);
                    SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
                    SwitchOutfitHelper.Start();
                    SwitchOutfitHelper.Wait(false);
                    try
                    {
                        PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, uniform, 0);
                    }
                    catch
                    {
                    }

                    PassionBase.GetPlayer(PlayerSim).PeenIsErect = true;
                    return true;
                    //}
                }
                // end peen addition

                // if we're removing it
                else if (!AddIt)
                {
                    SimDescription simDescription2 = PlayerSim.SimDescription;
                    if (simDescription2.GetOutfitCount(OutfitCategories.Naked) != 1)
                    {
                        while (simDescription2.GetOutfitCount(OutfitCategories.Naked) > 1)
                        {
                            simDescription2.RemoveOutfit(OutfitCategories.Naked, 0, true);
                        }
                        try
                        {
                            PlayerSim.SwitchToOutfitWithoutSpin(PreviousOutfitCategory, PreviousOutfitIndex);
                        }
                        catch
                        {
                        }
                        return false;
                    }
                }
                // end peen removal
            }
            return false;


        }

        // im just copying this without changing the internal terms bc im lazy
        // surely this won't haunt me layer (clueless)
        public bool GetNaked(Sim PlayerSim, bool AddIt)
        {
            if (PassionBase.GetPlayer(PlayerSim).SimGenitalType == "vagina" || PassionBase.GetPlayer(PlayerSim).SimGenitalType == "neither")
            {
                // if we're adding it
                if (AddIt && !PassionBase.GetPlayer(PlayerSim).PeenIsErect)
                {
                    SimDescription simDescription = PlayerSim.SimDescription;

                    // this is their bottom mesh, not peenar
                    string ErectPeen = PassionBase.GetPlayer(PlayerSim).SimJunkBaseCASP;
                    CASPart junk;
                    junk = new CASPart(ResourceKey.FromString(ErectPeen));

                    CASPart NakeyTop;
                    NakeyTop = new CASPart(ResourceKey.FromString(PassionBase.GetPlayer(PlayerSim).nudeTopRK));

                    SimBuilder simBuilder = new SimBuilder();
                    simBuilder.UseCompression = true;

                    // get current outfit
                    OutfitUtils.SetOutfit(simBuilder, PlayerSim.CurrentOutfit, simDescription);


                    // figure out our nudity type (if this doesnt work im going to fucking kill someone)
                    if (PassionBase.GetPlayer(PlayerSim).UndressLevel == "LowerBody")
                    {
                        CASPart[] parts = PlayerSim.CurrentOutfit.Parts;
                        for (int i = 0; i < parts.Length; i++)
                        {
                            CASPart part = parts[i];
                            if (part.BodyType == BodyTypes.LowerBody)
                            {
                                simBuilder.RemovePart(part);
                            }
                            // if theyre wearing fullbody we wanna remove that too and and the top :p
                            else if (part.BodyType == BodyTypes.FullBody)
                            {
                                simBuilder.RemovePart(part);
                                simBuilder.AddPart(NakeyTop);
                            }
                        }
                    }
                    else if (PassionBase.GetPlayer(PlayerSim).UndressLevel == "UpperBody")
                    {
                        CASPart[] parts = PlayerSim.CurrentOutfit.Parts;
                        for (int i = 0; i < parts.Length; i++)
                        {
                            CASPart part = parts[i];
                            if (part.BodyType == BodyTypes.UpperBody)
                            {
                                simBuilder.RemovePart(part);
                            }
                        }
                    }
                    else
                    {
                        CASPart[] parts = PlayerSim.CurrentOutfit.Parts;
                        for (int i = 0; i < parts.Length; i++)
                        {
                            CASPart part = parts[i];
                            if (part.BodyType == BodyTypes.FullBody)
                            {
                                simBuilder.RemovePart(part);
                            }
                            else if (part.BodyType == BodyTypes.LowerBody)
                            {
                                simBuilder.RemovePart(part);
                            }
                        }
                    }


                    CASPart part2 = junk;
                    // add the dong
                    simBuilder.AddPart(part2);

                    // special checks if undress type is fullbody or top, so we can add the sim's top too


                    if (PassionBase.GetPlayer(PlayerSim).UndressLevel != "LowerBody")
                    {
                        simBuilder.AddPart(NakeyTop);
                    }



                    ResourceKey key = simBuilder.CacheOutfit("BOE_Nakey" + simDescription.SimDescriptionId);
                    SimOutfit uniform = new SimOutfit(key);


                    //if (OutfitUtils.TryApplyUniformToOutfit(simDescription.GetOutfit(OutfitCategories.Naked, 0), uniform, simDescription, "imdying", out resultOutfit))
                    //{
                    simDescription.AddOutfit(uniform, OutfitCategories.Naked, 0);
                    SwitchOutfitHelper = new Sim.SwitchOutfitHelper(PlayerSim, OutfitCategories.Naked, 0);
                    SwitchOutfitHelper.Start();
                    SwitchOutfitHelper.Wait(false);
                    try
                    {
                        PlayerSim.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, uniform, 0);
                    }
                    catch
                    {
                    }

                    PassionBase.GetPlayer(PlayerSim).PeenIsErect = true;
                    return true;
                    //}
                }
                // end peen addition

                // if we're removing it
                else if (!AddIt)
                {
                    SimDescription simDescription2 = PlayerSim.SimDescription;
                    if (simDescription2.GetOutfitCount(OutfitCategories.Naked) != 1)
                    {
                        while (simDescription2.GetOutfitCount(OutfitCategories.Naked) > 1)
                        {
                            simDescription2.RemoveOutfit(OutfitCategories.Naked, 0, true);
                        }
                        try
                        {
                            PlayerSim.SwitchToOutfitWithoutSpin(PreviousOutfitCategory, PreviousOutfitIndex);
                        }
                        catch
                        {
                        }
                        return false;
                    }
                }
                // end peen removal
            }

            return false;


        }



        public void OnAnimationCompleted(StateMachineClient sender, IEvent evt)
        {
            if (State != PassionState.Leaving && State != PassionState.Stopping)
            {
                State = PassionState.Ready;
            }
        }

        public void Watch(Sim sim)
        {
            Watch(PassionBase.GetPlayer(sim));
        }

        // watch passion interaction
        // double check this to make sure that this action only runs for sims who'd be into it
        public void Watch(Player target)
        {
            if (!IsValid || !target.IsValid || !target.IsActive || !Actor.RouteToObjectRadialRange(target.Actor, 1.5f, 3f) || !target.IsValid || !target.IsActive)
            {
                return;
            }
            // shitty math time
            Actor.RouteTurnToFace(target.Actor.Position);
            State = PassionState.Watching;
            InteractionInstance currentInteraction = Actor.CurrentInteraction;
            currentInteraction.StandardEntry();
            currentInteraction.BeginCommodityUpdates();
            currentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Fun, 100f, false, 100f, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
            Libido.IncreaseUrgency(Actor);
            long num = SimClock.CurrentTicks + RandomUtil.GetInt(600, 1200);
            long num2 = SimClock.CurrentTicks + RandomUtil.GetInt(30, 50);
            string DudeName = Actor.Name;
            string DudeNameTarget = target.Actor.Name;
            while (IsWatching && Actor.HasNoExitReason() && target.IsActive && (!currentInteraction.Autonomous || SimClock.CurrentTicks < num))
            {
                if (SimClock.CurrentTicks > num2)
                {
                    // if a sim is a party animal or is the RNG check passes a sim who is watching may join
                    // ...rewrite this
                    if (PersistableSettings.Settings.AutonomyChance > 0 && (Actor.HasTrait(TraitNames.PartyAnimal) || RandomUtil.GetInt(0, 399) < PersistableSettings.Settings.AutonomyChance))
                    {
                        if (IsAutonomous && target.HasPart && target.Part.IsAutonomous && target.Part.HasRoom && WillPassion(target.Part) && Join(target.Part))
                        {
                            if (PickBoolean.Show(DudeName + " would like to join the fun with " + DudeNameTarget + "! Is this OK?", false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
                            {
                                currentInteraction.EndCommodityUpdates(true);
                                DirectStartLoop();
                                return;
                            }
                            else
                            {
                                currentInteraction.EndCommodityUpdates(true);
                                currentInteraction.StandardExit();
                            }


                        }
                        if (Join(PassionBase.GetTarget(Actor)))
                        {
                            if (PickBoolean.Show(DudeName + " would like to join the fun with " + DudeNameTarget + "! Is this OK?", false, PassionCommon.Localize("S3_Passion.Terms.Yes"), PassionCommon.Localize("S3_Passion.Terms.No")))
                            {
                                currentInteraction.EndCommodityUpdates(true);
                                DirectStartLoop();
                                return;
                            }
                            else
                            {
                                currentInteraction.EndCommodityUpdates(true);
                                currentInteraction.StandardExit();
                            }
                        }
                    }
                    Actor.PlayReaction(PassionCommon.RandomReactionPos, target.Actor, ReactionSpeed.ImmediateWithoutOverlay);
                    num2 = SimClock.CurrentTicks + RandomUtil.GetInt(75, 125);
                }
                PassionCommon.Wait(10);
            }
            currentInteraction.EndCommodityUpdates(true);
            currentInteraction.StandardExit();
            if (IsWatching)
            {
                State = PassionState.None;
            }
        }

        // switch position?
        // things without prefix infer the actor, partner is player2 aka the other guy
        public void Switch(Player partner)
        {
            if (partner == null || !IsValid || !partner.IsValid || !HasPart || !partner.HasPart)
            {
                return;
            }
            if (Part.BroWeAreSwitching == false)
            {
                return;
            }
            if (SwitchBuffer == true)
            {
                return;
            }
            Part part = Part;
            Part part2 = partner.Part;
            int positionIndex = PositionIndex;
            int positionIndex2 = partner.PositionIndex;
            if (part == part2)
            {
                PositionIndex = positionIndex2;
                partner.PositionIndex = positionIndex;
                BufferedAnimation = Part.Position.GetAnimation(this);
                partner.BufferedAnimation = Part.Position.GetAnimation(partner);
                SwitchBuffer = true;
                partner.SwitchBuffer = true;
                return;
            }
            else if (part.Target == part2.Target)
            {
                Vector3 exitPoint = ExitPoint;
                Vector3 exitPoint2 = partner.ExitPoint;
                PositionIndex = positionIndex2;
                partner.PositionIndex = positionIndex;
                ExitPoint = exitPoint2;
                partner.ExitPoint = exitPoint;
                IsInPlace = false;
                partner.IsInPlace = false;
                Join(part2);
                partner.Join(part);
                part.Players.Remove(ID);
                if (part.Initiator == this)
                {
                    part.SwapInitiator(partner);
                }
                part2.Players.Remove(partner.ID);
                if (part2.Initiator == partner)
                {
                    part2.SwapInitiator(this);
                }
                BufferedAnimation = part2.Position.GetAnimation(this);
                partner.BufferedAnimation = part.Position.GetAnimation(partner);
                SwitchBuffer = true;
                partner.SwitchBuffer = true;
                return;
            }
            else
            {
                SwitchPart = part2;
                partner.SwitchPart = part;
                ActiveLeaveJoin = true;
                partner.ActiveLeaveJoin = true;
                Stop();
                partner.Stop();
                SwitchBuffer = true;
                partner.SwitchBuffer = true;
                return;
            }
        }

        public void EndSwitch()
        {
            CanSwitch = false;
            SwitchPart = null;
            SwitchBuffer = false;

        }

        // stop interaction (but dont actually leave it?)
        public void Stop(ExitReason reason)
        {
            if (IsValid)
            {
                Actor.AddExitReason(reason);
            }
            Stop();
        }

        public void Stop()
        {
            State = PassionState.Stopping;
        }

        // leave the passion interaction
        public bool Leave()
        {
            State = PassionState.Leaving;
            CanAnimate = false;
            try
            {
                Actor.LookAtManager.EnableLookAts();
                ClearPosture();
                if (PersistableSettings.Settings.LibidoBuff)
                {
                    Libido.SatisfactionCalc(Actor, Partner.Actor);
                }
                Part.BroWeAreSwitching = false;
                RegisterWoohoo();
                try
                {
                    // if sim has peen that needs to be de erected
                    if (!ActiveLeave && !CanSwitch && PersistableSettings.Settings.GetSoft && (!PersistableSettings.Settings.StrapOnMode || PassionBase.GetPlayer(Actor).SimGenitalType == "penis" || PassionBase.GetPlayer(Actor).SimGenitalType == "both"))
                    {
                        SwitchToPeener(Actor, false);
                    }
                    // if sim is just nakey (no peen)
                    else if (!ActiveLeave && !CanSwitch && PersistableSettings.Settings.GetSoft && (!PersistableSettings.Settings.StrapOnMode || PassionBase.GetPlayer(Actor).SimGenitalType == "vagina" || PassionBase.GetPlayer(Actor).SimGenitalType == "neither"))
                    {
                        GetNaked(Actor, false);
                    }
                }
                catch
                {
                }
                if (IsInPlace && ExitPoint != Vector3.Empty && ExitPoint != Vector3.Invalid)
                {
                    Location = ExitPoint;
                }
                if (!ActiveLeave && !CanSwitch && (PersistableSettings.Settings.Outfit != 0 || HasPreferredOutfit))
                {
                    RevertOutfit();
                    IsNaked = false;
                    PeenIsErect = false;
                }
                PassionCommon.Modules.PostProcessing(Actor);
                ActiveLeave = false;
            }
            catch
            {
            }
            try
            {
                if (HasPart)
                {
                    Part.Remove(this);
                }
            }
            catch
            {
            }
            Reset();
            return false;
        }

        public void RegisterWoohoo()
        {
            if (!IsValid || !HadPartner)
            {
                return;
            }
            if (HasPart && Part.HasTarget && Part.Target.HasObject && Part.Target.ObjectType.IsGameObject)
            {
                EventTracker.SendEvent(new WooHooEvent(EventTypeId.kWooHooed, Actor, Partner.Actor, Part.Target.Object));
            }
            else
            {
                EventTracker.SendEvent(new WooHooEvent(EventTypeId.kWooHooed, Actor, Partner.Actor, Partner.Actor));
            }
            if (Actor.SimDescription.HadFirstWooHoo)
            {
                return;
            }
            // first time buff
            Actor.SimDescription.SetFirstWooHoo();
            if (PersistableSettings.Settings.WoohooBuff)
            {
                if (Actor.BuffManager.HasElement((BuffNames)9944098001884692765uL))
                {
                    Actor.BuffManager.RemoveElement((BuffNames)9944098001884692765uL);
                }
                Actor.BuffManager.AddElement((BuffNames)9944098001884692765uL, Origin.None);
            }
            EventTracker.SendEvent(EventTypeId.kHadFirstWoohoo, Actor, Partner.Actor);
        }

        // reset all da shit!!!!!!!!!!!
        public void Reset()
        {
            StopJealousyBroadcast();
            Part = null;
            IsActive = false;
            IsInPlace = false;
            IsAutonomous = false;
            CanAnimate = false;
            CanSwitch = false;
            SpinDisabled = false;
            DirectTargeted = false;
            ForceNoStrap = false;
            StartTime = 0L;
            CancelledOnTwitterDotCom = false;
            NumberAccepted = 0;
            PositionIndex = 0;
            PartnersToCheckCount = 0;
            PreviousOutfitIndex = 0;
            PreviousOutfitCategory = OutfitCategories.None;
            PotentialImpregnators = new List<Player>();
            PartnersToCheck = new List<Player>();
            PartnersUpdated = new List<Player>();
            ExitPoint = Vector3.Empty;
            State = PassionState.None;
            BufferedAnimation = string.Empty;
            BufferedTargetAnimation = string.Empty;
            BufferedObjectAnimation = string.Empty;
            HeldItem = null;
            Partner = null;
            PersistableSettings.Settings.PassionFuckSession = false;
            PersistableSettings.Settings.CondomIsBroken = false;
            PersistableSettings.Settings.RemoveCondom = true;
            PassionBase.SwitchPlayerActor = null;
            PassionBase.SwitchPlayerPartner = null;
        }
    }
}
