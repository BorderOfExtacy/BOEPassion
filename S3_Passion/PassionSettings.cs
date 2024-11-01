using System.Collections.Generic;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;

namespace S3_Passion
{
	[Persistable(false)]
	public class PassionSettings : ModalDialog
	{
		public class GeneralTabComponents
		{
			public ComboBox Outfit;

			public ComboBox Motives;

			public ComboBox InitialCategory;

			public Button NakedShower;
		}

		public class SettingsBufferObject
		{
			public OutfitCategories Outfit;

			public PassionMotives Motives;

			public int InitialCategory;

			public bool NakedShower;

			public SettingsBufferObject()
			{
				Outfit = Passion.Settings.Outfit;
				Motives = Passion.Settings.Motives;
				InitialCategory = Passion.Settings.InitialCategory;
				NakedShower = Passion.Settings.NakedShower;
			}

			public void Save()
			{
				Passion.Settings.Outfit = Outfit;
				Passion.Settings.Motives = Motives;
				Passion.Settings.InitialCategory = InitialCategory;
				Passion.Settings.NakedShower = NakedShower;
			}
		}

		public const uint OK_BUTTON = 2822726152u;

		public const uint CANCEL_BUTTON = 2822726153u;

		public const uint GENERAL_TAB = 2822726224u;

		public const uint OUTFIT_COMBO = 2822726243u;

		public const uint MOTIVES_COMBO = 2822726244u;

		public const uint INITIAL_CATEGORY_COMBO = 2822726245u;

		public const uint NAKED_SHOWER_BUTTON = 2822726246u;

		public const uint RESTORE_DEFAULTS_BUTTON = 2822726247u;

		public static PassionSettings Dialog;

		public Window GeneralTab;

		public List<Window> AdditionalTabs = new List<Window>();

		public GeneralTabComponents GeneralComponents = new GeneralTabComponents();

		public SettingsBufferObject SettingsBuffer = new SettingsBufferObject();

		public PassionSettings()
			: base("PassionSettingsDialog", 1, false, PauseMode.PauseSimulator, null)
		{
			Button button = mModalDialogWindow.GetChildByID(2822726152u, true) as Button;
			button.Click += OnOkay;
			Button button2 = mModalDialogWindow.GetChildByID(2822726153u, true) as Button;
			button2.Click += OnCancel;
			base.OkayID = 2822726152u;
			base.CancelID = 2822726153u;
			GeneralTab = mModalDialogWindow.GetChildByID(2822726224u, true) as Window;
			Button button3 = GeneralTab.GetChildByID(2822726247u, true) as Button;
			button3.Click += OnRestoreDefaultsClick;
			GeneralComponents.Outfit = GeneralTab.GetChildByID(2822726243u, true) as ComboBox;
			GeneralComponents.Outfit.ValueList.Add("Preferred Only", OutfitCategories.None);
			GeneralComponents.Outfit.ValueList.Add("Naked", OutfitCategories.Naked);
			GeneralComponents.Outfit.ValueList.Add("Sleepwear", OutfitCategories.Sleepwear);
			try
			{
				int num = GeneralComponents.Outfit.ValueList.IndexOf(SettingsBuffer.Outfit);
				if (num < 0)
				{
					num = 0;
				}
				GeneralComponents.Outfit.CurrentSelection = (uint)num;
			}
			catch
			{
			}
			GeneralComponents.Outfit.SelectionChange += OnStartingOutfitChange;
			GeneralComponents.Motives = GeneralTab.GetChildByID(2822726244u, true) as ComboBox;
			GeneralComponents.Motives.ValueList.Add("Standard", PassionMotives.PassionStandard);
			GeneralComponents.Motives.ValueList.Add("No Change", PassionMotives.EADefault);
			GeneralComponents.Motives.ValueList.Add("No Decay", PassionMotives.NoDecay);
			GeneralComponents.Motives.ValueList.Add("Freeze", PassionMotives.Freeze);
			GeneralComponents.Motives.ValueList.Add("Max All", PassionMotives.MaxAll);
			try
			{
				int num2 = GeneralComponents.Motives.ValueList.IndexOf(SettingsBuffer.Motives);
				if (num2 < 0)
				{
					num2 = 0;
				}
				GeneralComponents.Motives.CurrentSelection = (uint)num2;
			}
			catch
			{
			}
			GeneralComponents.Motives.SelectionChange += OnMotivesChange;
			GeneralComponents.InitialCategory = GeneralTab.GetChildByID(2822726245u, true) as ComboBox;
			GeneralComponents.InitialCategory.ValueList.Add("Any", 1);
			GeneralComponents.InitialCategory.ValueList.Add("Masturbate", 48);
			GeneralComponents.InitialCategory.ValueList.Add("Oral", 2);
			GeneralComponents.InitialCategory.ValueList.Add("Fuck", 12);
			try
			{
				int num3 = GeneralComponents.InitialCategory.ValueList.IndexOf(SettingsBuffer.InitialCategory);
				if (num3 < 0)
				{
					num3 = 0;
				}
				GeneralComponents.InitialCategory.CurrentSelection = (uint)num3;
			}
			catch
			{
			}
			GeneralComponents.Outfit.SelectionChange += OnInitialCategoryChange;
			GeneralComponents.NakedShower = GeneralTab.GetChildByID(2822726246u, true) as Button;
			GeneralComponents.NakedShower.Selected = SettingsBuffer.NakedShower;
			GeneralComponents.NakedShower.Click += OnNakedShowerClick;
		}

		public void OnStartingOutfitChange(WindowBase sender, UISelectionChangeEventArgs eventArgs)
		{
			try
			{
				SettingsBuffer.Outfit = (OutfitCategories)(sender as ComboBox).EntryTags[(int)eventArgs.NewIndex];
			}
			catch
			{
			}
		}

		public void OnMotivesChange(WindowBase sender, UISelectionChangeEventArgs eventArgs)
		{
			try
			{
				SettingsBuffer.Motives = (PassionMotives)(sender as ComboBox).EntryTags[(int)eventArgs.NewIndex];
			}
			catch
			{
			}
		}

		public void OnInitialCategoryChange(WindowBase sender, UISelectionChangeEventArgs eventArgs)
		{
			try
			{
				SettingsBuffer.InitialCategory = (int)(sender as ComboBox).EntryTags[(int)eventArgs.NewIndex];
			}
			catch
			{
			}
		}

		public void OnNakedShowerClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			Button button = sender as Button;
			if (button != null)
			{
				SettingsBuffer.NakedShower = button.Selected;
			}
		}

		public void OnRestoreDefaultsClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			SettingsBuffer.Outfit = OutfitCategories.None;
			SettingsBuffer.Motives = PassionMotives.PassionStandard;
			SettingsBuffer.InitialCategory = 1;
			SettingsBuffer.NakedShower = true;
			try
			{
				GeneralComponents.Outfit.CurrentSelection = 0u;
				GeneralComponents.Motives.CurrentSelection = 0u;
				GeneralComponents.InitialCategory.CurrentSelection = 0u;
				GeneralComponents.NakedShower.Selected = true;
			}
			catch
			{
			}
		}

		public void OnOkay(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			SettingsBuffer.Save();
			eventArgs.Handled = true;
			EndDialog(sender.ID);
		}

		public void OnCancel(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			eventArgs.Handled = true;
			EndDialog(sender.ID);
		}

		private static void PassionSettingsTask()
		{
			Dialog = new PassionSettings();
			Dialog.StartModal();
			Dialog = null;
		}

		public static void Show()
		{
			Simulator.AddObject(new OneShotFunctionTask(PassionSettingsTask));
		}
	}
}
