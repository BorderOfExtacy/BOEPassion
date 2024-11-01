using Sims3.SimIFace;

namespace S3_Passion
{
	public class PassionLoader
	{
		[Tunable]
		protected static bool LoadPassion;

		static PassionLoader()
		{
			LoadPassion = false;
			LoadSaveManager.ObjectGroupsPreLoad += Passion.Preload;
			World.OnWorldLoadFinishedEventHandler += Passion.Load;
			World.OnWorldQuitEventHandler += Passion.UnLoad;
		}
	}
}
