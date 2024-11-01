using Sims3.SimIFace;
using Sims3.UI;

namespace S3_Passion
{
	internal class PickString
	{
		public static string Show(string title, string prompt, string initial)
		{
			return Show(title, prompt, initial, ThumbnailKey.kInvalidThumbnailKey);
		}

		public static string Show(string title, string prompt, string initial, ThumbnailKey key)
		{
			return StringInputDialog.Show(title, prompt, initial, -1, key, new Vector2(-1f, -1f), StringInputDialog.Validation.None, false, ModalDialog.PauseMode.PauseSimulator, false, true);
		}
	}
}
