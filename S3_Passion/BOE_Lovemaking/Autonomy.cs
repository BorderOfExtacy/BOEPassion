using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects.Decorations.Mimics;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.SimIFace.VideoRecording;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Settings;
using S3_Passion.BOE_Objects;
using S3_Passion.BOE_Interaction;
using Sims3.Gameplay.Objects.Beds;

namespace S3_Passion.BOE_Lovemaking
{
    public class Autonomy
    {
        public static ListenerAction HappendAtMassage(Event e)
        {
            return ListenerAction.Keep;
        }

        // nude/stripper dance autonomy
        public static ListenerAction DanceNude2Music(Event e)
        {
            ResourceKey key = ResourceKey.FromString("0x02DC343F-0x08000000-0x475CA79579FF223E");
            if (World.ResourceExists(key) && PersistableSettings.Settings.StripperAutonomy)
            {
                try
                {
                    // if the autonomy chance is higher than the RNG number
                    if (PersistableSettings.Settings.AutonomyChance > 0 && RandomUtil.GetInt(0, 99) < PersistableSettings.Settings.AutonomyChance)
                    {
                        Sim sim = e.Actor as Sim;
                        GameObject gameObject = e.TargetObject as GameObject;
                        int num = sim.LotCurrent.GetAllActorsCount();
                        uint num2 = sim.LotCurrent.CountObjects<SculptureFloorGunShow>();
                        foreach (Sim allActor in sim.LotCurrent.GetAllActors())
                        {
                            Player player = PassionBase.GetPlayer(allActor);
                            if (allActor != null && !allActor.LotCurrent.IsWorldLot && (player.Actor.SimDescription.ChildOrBelow || !player.Actor.SimDescription.IsHuman))
                            {
                                num--;
                            }
                        }
                        if (PersistableSettings.Settings.AutonomyActive)
                        {
                            Player player2 = PassionBase.GetPlayer(sim);
                            SimDescription simDescription = sim.SimDescription;
                            if (player2.IsValid && !simDescription.ChildOrBelow && num >= 3 && num2 != 0 && RandomUtil.CoinFlip())
                            {
                                if (num2 <= 2)
                                {
                                    if (RandomUtil.CoinFlip())
                                    {
                                        PassionCommon.Wait(1800u);
                                    }
                                    else
                                    {
                                        PassionCommon.Wait(1200u);
                                    }
                                }
                                else if (num2 > 2)
                                {
                                    if (RandomUtil.CoinFlip())
                                    {
                                        PassionCommon.Wait(900u);
                                    }
                                    else
                                    {
                                        PassionCommon.Wait(600u);
                                    }
                                }
                                List<string> list = new List<string>();
                                foreach (Sim sim2 in sim.LotCurrent.GetSims())
                                {
                                    if (sim2.SimDescription.IsServicePerson && !(sim2.Service is Butler) && !(sim2.Service is Maid))
                                    {
                                        list.Add(sim2.Name.ToString());
                                    }
                                }
                                string randomObjectFromList;
                                if (list.Count > 0)
                                {
                                    randomObjectFromList = RandomUtil.GetRandomObjectFromList(list);
                                }
                                else
                                {
                                    foreach (Sim sim3 in sim.LotCurrent.GetSims())
                                    {
                                        if (sim3.SimDescription.TeenOrAbove && sim3.SimDescription.IsHuman && !sim3.LotCurrent.IsWorldLot)
                                        {
                                            list.Add(sim3.Name.ToString());
                                        }
                                    }
                                    randomObjectFromList = RandomUtil.GetRandomObjectFromList(list);
                                }
                                foreach (Sim sim4 in sim.LotCurrent.GetSims())
                                {
                                    if (sim4.Name.ToString() == randomObjectFromList)
                                    {
                                        sim = sim4;
                                        player2 = PassionBase.GetPlayer(sim);
                                    }
                                }
                                player2.IsAutonomous = true;
                                List<SculptureFloorGunShow> list2 = new List<SculptureFloorGunShow>(Sims3.Gameplay.Queries.GetObjects<SculptureFloorGunShow>(sim.LotCurrent, sim.RoomId));
                                if (list2.Count > 0)
                                {
                                    foreach (SculptureFloorGunShow item in list2)
                                    {
                                        if (item.UseCount < 1)
                                        {
                                            gameObject = item;
                                            break;
                                        }
                                        gameObject = null;
                                    }
                                }
                                if (sim != null && gameObject != null && RandomUtil.CoinFlip() && !sim.SimDescription.Elder && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != StripperPole.WatchStrip.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.UseObjectForPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.UseSimForPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.ActiveJoinPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.JoinPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.AskToJoinPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.AskToSoloPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.AskToPassionOther.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.AskToWatchPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.WatchPassion.Singleton && sim.InteractionQueue.GetCurrentInteraction().InteractionDefinition != Interactions.WatchMasturbate.Singleton)
                                {
                                    if (sim.SimDescription.IsServicePerson && !(sim.Service is Butler) && !(sim.Service is Maid) && sim.CurrentOutfitCategory != OutfitCategories.Career)
                                    {
                                        PassionBase.MyOutfit = sim.CurrentOutfitCategory;
                                        sim.SwitchToOutfitWithoutSpin(OutfitCategories.Career, 0);
                                    }
                                    sim.InteractionQueue.AddAfterCheckingForDuplicates(StripperPole.AutoDance.Singleton.CreateInstance(gameObject, sim, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                return ListenerAction.Keep;
            }
            return ListenerAction.Keep;
        }

        // masturbate while watching passion tv
        public static ListenerAction WhenWatchTV(Event e)
        {
            ResourceKey key = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x8DC278D813275705");
            ResourceKey key2 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0xEFD91C1084B3E2DA");
            ResourceKey key3 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x8647C1AAD74E30F7");
            ResourceKey key4 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0xDC118FA318BD0EE4");
            ResourceKey key5 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x046C240AEA28A251");
            ResourceKey key6 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x875667F1FFDDF916");
            ResourceKey key7 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0xCC02BFCB7272D603");
            ResourceKey key8 = ResourceKey.FromString("0xB1CC1AF6-0x00000000-0x7649533123BEF070");
            if (World.ResourceExists(key) && World.ResourceExists(key2) && World.ResourceExists(key3) && World.ResourceExists(key4) && World.ResourceExists(key5) && World.ResourceExists(key6) && World.ResourceExists(key7) && World.ResourceExists(key8))
            {
                try
                {
                    if (PersistableSettings.Settings.AutonomyChance > 0 && RandomUtil.GetInt(0, 99) < PersistableSettings.Settings.AutonomyChance)
                    {
                        Sim sim = e.Actor as Sim;
                        Sim sim2 = e.TargetObject as Sim;
                        if (TVChannelData.GetChannelNames(TVChannelLevel.Level1).Contains("Gameplay/Excel/TV/TVChannel:Channel_1") && TVChannelData.GetChannelNames(TVChannelLevel.Level1).Contains("Gameplay/Excel/TV/TVChannel:Channel_2") && TVChannelData.GetChannelNames(TVChannelLevel.Level1).Contains("Gameplay/Excel/TV/TVChannel:Channel_3") && TVChannelData.GetChannelNames(TVChannelLevel.Level1).Contains("Gameplay/Excel/TV/TVChannel:Channel_4") && (PersistableSettings.Settings.AutonomyActive || sim.Household != Household.ActiveHousehold && sim2.Household != Household.ActiveHousehold) && (PersistableSettings.Settings.AutonomyPublic || sim.LotCurrent.LotType == LotType.Residential))
                        {
                            Player player = PassionBase.GetPlayer(sim);
                            SimDescription simDescription = sim.SimDescription;
                            if (player.IsValid && !player.IsActive && !simDescription.ChildOrBelow && (player.Actor.InteractionQueue.GetHeadInteraction().ToString() == PassionCommon.Localize("Gameplay/Objects/Electronics/TV/WatchTV:WatchTVInteractionName") || player.Actor.InteractionQueue.GetHeadInteraction().ToString() == PassionCommon.Localize("Gameplay/Objects/Electronics/TV/WatchTVAutonomously:WatchTVInteractionName")))
                            {
                                player.IsAutonomous = true;
                                ObjectGuid objectId = player.Actor.CurrentInteraction.Target.ObjectId;
                                IGameObject target = player.Actor.CurrentInteraction.Target;
                                // from what i can tell...
                                // this is effectively, if the sim is aroused, wait and then auto masturbate. the more aroused they are, the shorter the wait

                                //100%
                                if (player.Actor.BuffManager.HasElement((BuffNames)2922253427052633003uL))
                                {
                                    PassionCommon.Wait(400);
                                    player.Actor.InteractionQueue.CancelAllInteractions();
                                    player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
                                }
                                // 90%
                                else if (player.Actor.BuffManager.HasElement((BuffNames)13147589483235469726uL))
                                {
                                    PassionCommon.Wait(600);
                                    player.Actor.InteractionQueue.CancelAllInteractions();
                                    player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
                                }
                                // 80%
                                else if (player.Actor.BuffManager.HasElement((BuffNames)14041574305464178967uL))
                                {
                                    PassionCommon.Wait(800);
                                    player.Actor.InteractionQueue.CancelAllInteractions();
                                    player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
                                }
                                // 70%
                                else if (player.Actor.BuffManager.HasElement((BuffNames)16251613925768384549uL))
                                {
                                    PassionCommon.Wait(1000);
                                    player.Actor.InteractionQueue.CancelAllInteractions();
                                    player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
                                }
                                // 60%
                                else if (player.Actor.BuffManager.HasElement((BuffNames)2917472750494117670uL))
                                {
                                    PassionCommon.Wait(1500);
                                    player.Actor.InteractionQueue.CancelAllInteractions();
                                    player.Actor.InteractionQueue.AddNext(Interactions.AutoSoloPassion.Singleton.CreateInstance(player.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                return ListenerAction.Keep;
            }
            return ListenerAction.Remove;
        }

        // passioncheck -- check to **initiate** autonomous passion
        // this runs through 'willpassion' for both sims, so see that for like. conditions n shit
        public static ListenerAction PassionCheck(Event e)
        {
            try
            {
                // im scared
                // PassionCommon.SystemMessage("passioncheck fired alright");

                Sim guy = e.Actor as Sim;
                Sim guy2 = e.TargetObject as Sim;

                ShortTermContext sTC = Relationship.GetSTC(guy, guy2);

                // if the current convo stc is romantic
                // otherwise fuck you
                if (sTC.IsRomantic)
                {

                    int ChargeThreshold = 0;


                    Player playerguy = PassionBase.GetPlayer(guy);
                    Player playerguy2 = PassionBase.GetPlayer(guy2);

                    // add 10 to their charge
                    if (guy.TraitManager.HasElement((TraitNames)5711695705602619160uL))
                    {
                        playerguy.PassionCharge += 25;
                    }
                    else
                    {
                        playerguy.PassionCharge += 10;
                    }
                    // PassionCommon.SystemMessage("sim1 preroll charge is" + playerguy.PassionCharge);
                    if (guy2.TraitManager.HasElement((TraitNames)5711695705602619160uL))
                    {
                        playerguy2.PassionCharge += 25;
                    }
                    else
                    {
                        playerguy2.PassionCharge += 10;
                    }
                    // PassionCommon.SystemMessage("sim2 preroll charge is" + playerguy2.PassionCharge);

                    ChargeThreshold = RandomUtil.GetInt(0, 100);
                    // PassionCommon.SystemMessage("charge threshold is" + ChargeThreshold);

                    if (playerguy.PassionCharge >= ChargeThreshold)
                    {
                        Libido.IncreaseUrgency(guy);
                        playerguy.PassionCharge = 0;
                        // PassionCommon.SystemMessage("sim1 increased libido");
                    }
                    if (playerguy2.PassionCharge >= ChargeThreshold)
                    {
                        Libido.IncreaseUrgency(guy2);
                        playerguy2.PassionCharge = 0;
                        // PassionCommon.SystemMessage("sim2 increased libido");
                    }


                    // if autonomychance is higher than random, check continues.
                    // refactor this so it takes libido into account as well?
                    if (PersistableSettings.Settings.AutonomyChance > 0 && RandomUtil.GetInt(0, 99) < PersistableSettings.Settings.AutonomyChance)
                    {
                        Sim sim = e.Actor as Sim;
                        Sim sim2 = e.TargetObject as Sim;
                        if (!(sim.Posture is RelaxingPosture) && !(sim.Posture is SittingPosture) && Player.CanPassion(sim, sim2) && Player.WillPassion(sim, sim2) && Player.WillPassion(sim2, sim) && (PersistableSettings.Settings.AutonomyActive || sim.Household != Household.ActiveHousehold && sim2.Household != Household.ActiveHousehold))
                        {
                            Player player = PassionBase.GetPlayer(sim);
                            Player player2 = PassionBase.GetPlayer(sim2);
                            if (player.IsValid && player2.IsValid && !player.IsActive && !player2.IsActive)
                            {

                                PassionTarget target = player.GetNearbySupportedTarget();
                                if (target == null)
                                {
                                    target = PassionBase.GetTarget(sim2);
                                }
                                if (target != null && target.IsValid)
                                {
                                    Part part = null;
                                    if (target.Parts.Count > 0)
                                    {
                                        part = RandomUtil.GetRandomObjectFromList(new List<Part>(target.Parts.Values));
                                    }
                                    if (part != null && player.Join(part))
                                    {
                                        player.IsAutonomous = true;
                                        player2.IsAutonomous = true;
                                        player.Actor.InteractionQueue.CancelAllInteractions();
                                        player.Actor.InteractionQueue.AddNext(Interactions.AskToPassion.Singleton.CreateInstance(player2.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                                        if (PersistableSettings.Settings.AutonomyNotify)
                                        {
                                            try
                                            {
                                                StringBuilder stringBuilder = new StringBuilder(PassionCommon.Localize("S3_Passion.Terms.AutonomyNotifySimMessage"));
                                                stringBuilder.Replace("[player]", player.Name);
                                                stringBuilder.Replace("[label]", PersistableSettings.Settings.ActiveLabel.ToLower());
                                                stringBuilder.Replace("[partner]", player2.Name);
                                                stringBuilder.Replace("[address]", player.Actor.LotCurrent.Name);
                                                PassionCommon.SimMessage(stringBuilder.ToString(), player.Actor, player2.Actor);
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                }
                                else if (PassionCommon.Testing)
                                {
                                    PassionCommon.SystemMessage("No valid target found for Autonomy for " + player.Name + " & " + player2.Name);
                                }

                            }
                        }
                    }

                }
                else
                {
                    // SystemMessage("context isnt romantic. fuck you.");
                }
            }
            catch
            {
                // SystemMessage("uh oh");
            }
            return ListenerAction.Keep;
        }

        // jealousy/cheating check
        public static void JealousyCheck(Sim witness, ReactionBroadcaster rb)
        {
            try
            {
                // if jealousy is off or there are no witnesses
                if (!PersistableSettings.Settings.Jealousy || witness == null || !(rb.BroadcastingObject is Sim))
                {
                    return;
                }
                Player player = PassionBase.GetPlayer(rb.BroadcastingObject as Sim);
                if (player == null || !player.IsInitiator || !player.HadPartner || witness == player.Actor)
                {
                    return;
                }
                bool flag = PersistableSettings.Settings.PolyamorousJealousy;
                List<Sim> list = new List<Sim>();
                List<Sim> list2 = new List<Sim>();
                foreach (Player value in player.Part.Players.Values)
                {
                    if (value == null || !value.IsValid)
                    {
                        continue;
                    }
                    bool flag2 = value.Actor.HasTrait(TraitNames.NoJealousy);
                    if (!flag2)
                    {
                        Relationship relationship = Relationship.Get(witness, value.Actor, false);
                        if (relationship != null)
                        {
                            LTRData lTRData = LTRData.Get(relationship.LTR.CurrentLTR);
                            if (witness.Partner != value.Actor.SimDescription && !lTRData.IsRomantic)
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    if (!list.Contains(value.Actor))
                    {
                        list.Add(value.Actor);
                    }
                    if (witness == value.Actor)
                    {
                        return;
                    }
                    if (!flag2 && !list2.Contains(value.Actor))
                    {
                        list2.Add(value.Actor);
                    }
                }
                if (flag || list2.Count < 1)
                {
                    return;
                }
                List<Sim> list3 = new List<Sim>();
                foreach (Sim item in list2)
                {
                    Relationship relationship2 = Relationship.Get(witness, item, false);
                    if (relationship2 != null)
                    {
                        LTRData lTRData2 = LTRData.Get(relationship2.LTR.CurrentLTR);
                        if (witness.Partner == item.SimDescription || lTRData2.IsRomantic)
                        {
                            SocialComponent.OnIWasCheatedOn(witness, item.SimDescription, player.Actor.SimDescription, JealousyLevel.High);
                            RomanceVisibilityState.PushAccuseSimOfBetrayal(witness, item);
                            continue;
                        }
                    }
                    if (!PersistableSettings.Settings.PolyamorousJealousy && item.Partner != null && item.Partner.CreatedSim != null && !list.Contains(item.Partner.CreatedSim) && witness.Genealogy.IsBloodRelated(item.Partner.Genealogy))
                    {
                        SocialComponent.OnSomeoneICareAboutWasCheatedOn(witness, item.Partner, item.SimDescription, player.Actor.SimDescription, JealousyLevel.High);
                    }
                }
            }
            catch
            {
                if (PassionCommon.Testing)
                {
                    PassionCommon.SystemMessage("Error during JealousyCheck");
                }
            }
        }
    }

}
