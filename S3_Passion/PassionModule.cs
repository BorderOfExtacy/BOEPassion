using System;
using Passion.S3_Passion.Utilities;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;

namespace Passion.S3_Passion
{
	public class PassionModule : IPassionModule
	{
		public virtual void StartListeners()
		{
		}

		public virtual void LoadSim(Sim sim)
		{
		}

		public virtual void LoadObject(GameObject obj)
		{
		}

		public virtual void PreProcessing(Sim sim)
		{
		}

		public virtual void LoopProcessing(Sim sim)
		{
		}

		public virtual void PostProcessing(Sim sim)
		{
		}

		public virtual int GetPassionScoringModifier(Sim sim, Sim[] partners)
		{
			return 0;
		}

		public virtual GameObject[] GetCustomObjects()
		{
			return new GameObject[0];
		}

		public virtual Type[] GetCustomObjectTypes()
		{
			return new Type[0];
		}
	}
}
