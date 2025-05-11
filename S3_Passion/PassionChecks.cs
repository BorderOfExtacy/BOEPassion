using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.SimIFace.VideoRecording;
using Sims3.SimIFace;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Objects.Beds;


namespace S3_Passion
{
    class PassionChecks
    {

        public static void InitiationCheck(Event e)
        {
            // if autonomychance is higher than random, check continues.
            // refactor this so it takes libido into account as well?
            if (Passion.Settings.AutonomyChance > 0 && RandomUtil.GetInt(0, 99) < Passion.Settings.AutonomyChance)
            {
                Sim sim = e.Actor as Sim;
                Sim sim2 = e.TargetObject as Sim;
                if (!(sim.Posture is RelaxingPosture) && !(sim.Posture is SittingPosture) && Passion.Player.CanPassion(sim, sim2) && Passion.Player.WillPassion(sim, sim2) && Passion.Player.WillPassion(sim2, sim) && (Passion.Settings.AutonomyActive || (sim.Household != Household.ActiveHousehold && sim2.Household != Household.ActiveHousehold)))
                {
                    Passion.Player player = Passion.GetPlayer(sim);
                    Passion.Player player2 = Passion.GetPlayer(sim2);
                    if (player.IsValid && player2.IsValid && !player.IsActive && !player2.IsActive)
                    {

                            Passion.Target target = player.GetNearbySupportedTarget();
                            if (target == null)
                            {
                                target = Passion.GetTarget(sim2);
                            }
                            if (target != null && target.IsValid)
                            {
                                Passion.Part part = null;
                                if (target.Parts.Count > 0)
                                {
                                    part = RandomUtil.GetRandomObjectFromList(new List<Passion.Part>(target.Parts.Values));
                                }
                                if (part != null && player.Join(part))
                                {
                                    player.IsAutonomous = true;
                                    player2.IsAutonomous = true;
                                    player.Actor.InteractionQueue.CancelAllInteractions();
                                    player.Actor.InteractionQueue.AddNext(Passion.Interactions.AskToPassion.Singleton.CreateInstance(player2.Actor, player.Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
                                    if (Passion.Settings.AutonomyNotify)
                                    {
                                        try
                                        {
                                            StringBuilder stringBuilder = new StringBuilder(PassionCommon.Localize("S3_Passion.Terms.AutonomyNotifySimMessage"));
                                            stringBuilder.Replace("[player]", player.Name);
                                            stringBuilder.Replace("[label]", Passion.Settings.ActiveLabel.ToLower());
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




    }
}
