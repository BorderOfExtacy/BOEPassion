using System;
using System.Collections.Generic;
using System.Text;
using S3_Passion;
using Sims3.Gameplay.Actors;

namespace Passion.S3_Passion
{
    class JunkLogic
    {

        public object SimPenis (Sim sim)
        {
            if (GetPlayer(sim).SimGenitalia.Equals(PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_AM))
            {
                return PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_AM;
            }
            else if (GetPlayer(sim).SimGenitalia.Equals(PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_AF))
            {
                return PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_AF;
            }
            else if (GetPlayer(sim).SimGenitalia.Equals(PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_TM))
            {
                return PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_TM;
            }
            else if (GetPlayer(sim).SimGenitalia.Equals(PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_TF))
            {
                return PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_TF;
            }
            else if (GetPlayer(sim).SimGenitalia.Equals(PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_EM))
            {
                return PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_EM;
            }
            else if (GetPlayer(sim).SimGenitalia.Equals(PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_EF))
            {
                return PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_EF;
            }
            else
            {
                PassionCommon.SystemMessage("this sim's cock is MISSING what the FUCK");
                return PassionGenitals.SimGenitaliaList.VANILLA_defaultpenis_EF;
            }
        }

    }
}
