using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using S3_Passion.PassionDance;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.Gameplay.Objects.Counters;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.Gameplay.Objects.Decorations.Mimics;
using Sims3.Gameplay.Objects.Door;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Objects.Entertainment;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.HobbiesSkills;
using Sims3.Gameplay.Objects.HobbiesSkills.BrainEnhancingMachine;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Objects.Tables;
using Sims3.Gameplay.Objects.Vehicles;
using Sims3.Gameplay.Pools;
using Sims3.Gameplay.Seasons;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Situations;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.ThoughtBalloons;
using Sims3.Gameplay.TimeTravel;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.SimIFace.RouteDestinations;
using Sims3.Store.Objects;
using Sims3.UI;
using Sims3.UI.Controller;
using Sims3.UI.GameEntry;

namespace S3_Passion
{
    public class PassionGenitals
    {

        public enum SimGenitaliaList
        {
            UNSET,
            VANILLA_defaultpenis_masc, // UUID; MASCDEFAULTSOFT || MASCDEFAULTHARD
            VANILLA_defaultpenis_fem, // UUID; FEMDEFAULTSOFT || FEMDEFAULTHARD
            VANILLA_defaultvagina_masc, // UUID; MASCVAGINA
            VANILLA_defaultvagina_fem, // UUID; FEMVAGINA
            VANILLA_barbiedoll_masc, // UUID; BARBIEDOLLMASC
            VANILLA_barbiedoll_fem, // UUID; BARBIEDOLLFEM
        }
        // uuid format is: SOFT || HARD

        public enum SimStraponList
        {
            UNSET,
            VANILLA_default,
        }

        public enum GenitalTypeList
        {
            UNSET,
            penis,
            vagina,
            both,
            neither,
        }


    }
}
