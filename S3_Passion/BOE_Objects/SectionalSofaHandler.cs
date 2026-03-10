using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Utilities;
using S3_Passion.BOE_Core;
using S3_Passion.BOE_Interaction;

namespace S3_Passion.BOE_Objects
{
    public class SectionalManager : IAlarmOwner
    {
        public const float LoadDelay = 2f;

        public static SectionalManager Singleton = new SectionalManager();

        public static AlarmHandle UpdateAlarm = AlarmHandle.kInvalidHandle;

        public static void TriggerUpdate()
        {
            if (UpdateAlarm == AlarmHandle.kInvalidHandle)
            {
                UpdateAlarm = AlarmManager.Global.AddAlarm(2f, TimeUnit.Minutes, CheckSectionals, "Sectional Furniture Check", AlarmType.NeverPersisted, Singleton);
            }
        }

        public static void CheckSectionals()
        {
            try
            {
                ChairSectional[] objects = Sims3.Gameplay.Queries.GetObjects<ChairSectional>();
                if (objects != null)
                {
                    ChairSectional[] array = objects;
                    foreach (ChairSectional chairSectional in array)
                    {
                        if (chairSectional.Section != Sims3.Gameplay.Objects.Seating.Section.CornerConcave)
                        {
                            chairSectional.AddInteraction(Interactions.UseObjectForPassion.Singleton, true);
                            chairSectional.AddInteraction(Interactions.UseObjectForPassionWithSim.Singleton, true);
                            chairSectional.AddInteraction(Interactions.ResetMe.Singleton, true);
                            chairSectional.AddInteraction(Interactions.ResetMeActive.Singleton, true);
                            chairSectional.AddInteraction(Interactions.MoveTo.Singleton, true);
                            chairSectional.AddInteraction(Interactions.MoveGroupTo.Singleton, true);
                        }
                        else
                        {
                            chairSectional.RemoveInteractionByType(Interactions.UseObjectForPassion.Singleton);
                            chairSectional.RemoveInteractionByType(Interactions.UseObjectForPassionWithSim.Singleton);
                            chairSectional.RemoveInteractionByType(Interactions.ResetMe.Singleton);
                            chairSectional.RemoveInteractionByType(Interactions.ResetMeActive.Singleton);
                            chairSectional.RemoveInteractionByType(Interactions.MoveTo.Singleton);
                            chairSectional.RemoveInteractionByType(Interactions.MoveGroupTo.Singleton);
                        }
                    }
                }
            }
            catch
            {
            }
            UpdateAlarm = AlarmHandle.kInvalidHandle;
        }
    }
}
