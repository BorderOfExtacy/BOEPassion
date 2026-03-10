using System;
using System.Collections.Generic;
using System.Reflection;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;

namespace S3_Passion.BOE_Core
{
	public class PassionLoader
	{
		[Tunable]
		protected static bool LoadPassion;

		static PassionLoader()
		{
			LoadPassion = false;
			LoadSaveManager.ObjectGroupsPreLoad += PassionBase.Preload;
			World.sOnWorldLoadFinishedEventHandler += PassionBase.Load;
			World.sOnWorldQuitEventHandler += PassionBase.UnLoad;
		}
	}
}
