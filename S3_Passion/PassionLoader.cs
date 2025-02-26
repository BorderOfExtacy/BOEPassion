using Sims3.SimIFace;
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion
{
	public class PassionLoader
	{
		[Tunable]
		protected static bool LoadPassion;

		static PassionLoader()
		{
			LoadPassion = false;
			LoadSaveManager.ObjectGroupsPreLoad += PassionCore.Preload;
			World.sOnWorldLoadFinishedEventHandler += PassionCore.Load;
			World.sOnWorldQuitEventHandler += PassionCore.UnLoad;
		}
	}
}
