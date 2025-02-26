namespace Passion.S3_Passion
{
	internal class PickBoolean : GenericDialog
	{
		public static bool Show(string prompt)
		{
			return Show(prompt, false);
		}

		private static bool Show(string prompt, bool current)
		{
			return Show(prompt, current, "True", "False");
		}

		/*private static bool Show(string prompt, bool current, string t, string f)
		{
			OptionList<bool> optionList = new OptionList<bool>();
			optionList.Add(t, true, current);
			optionList.Add(f, false);
			return GenericDialog.Ask(optionList, prompt);
		}*/


		public static bool Show(string localize, bool b, string s, string localize1)
		{
			OptionList<bool> optionList = new OptionList<bool>();
			optionList.Add(localize, true, b);
			optionList.Add(s, false);
			return GenericDialog.Ask(optionList, localize1);
		}
	}
}
