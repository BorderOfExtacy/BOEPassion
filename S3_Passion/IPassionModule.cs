using System;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;

namespace S3_Passion
{
	public interface IPassionModule
	{
		void StartListeners();

		void LoadSim(Sim sim);

		void LoadObject(GameObject obj);

		void PreProcessing(Sim sim);

		void LoopProcessing(Sim sim);

		void PostProcessing(Sim sim);

		int GetPassionScoringModifier(Sim sim, Sim[] partners);

		GameObject[] GetCustomObjects();

		Type[] GetCustomObjectTypes();
	}
}
