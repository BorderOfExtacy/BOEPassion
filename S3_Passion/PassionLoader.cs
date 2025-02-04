using Sims3.SimIFace;

namespace Passion.S3_Passion
{
	public class PassionLoader
	{
		[Tunable]
		protected static bool LoadPassion;

		static PassionLoader()
		{
			LoadPassion = false;
			LoadSaveManager.ObjectGroupsPreLoad += Passion.Preload;
			World.sOnWorldLoadFinishedEventHandler += Passion.Load;
			World.sOnWorldQuitEventHandler += Passion.UnLoad;
		}
	}
}
