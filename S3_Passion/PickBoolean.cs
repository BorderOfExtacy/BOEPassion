namespace Passion.S3_Passion
{
	internal class PickBoolean : GenericDialog
	{
		public static bool Show(string prompt)
		{
			return Show(prompt, false);
		}

		public static bool Show(string prompt, bool current)
		{
			return Show(prompt, current, "True", "False");
		}

		public static bool Show(string prompt, bool current, string t, string f)
		{
			OptionList<bool> optionList = new OptionList<bool>();
			optionList.Add(t, true, current);
			optionList.Add(f, false);
			return GenericDialog.Ask(optionList, prompt);
		}
	}
}
