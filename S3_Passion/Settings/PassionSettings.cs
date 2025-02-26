using System.Collections.Generic;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion.Settings
{
	[Persistable(false)]
	public class PassionSettings : ModalDialog
	{
		private class GeneralTabComponents
		{
			public ComboBox Outfit;

			public ComboBox Motives;

			public ComboBox InitialCategory;

			public Button NakedShower;
		}

		private class SettingsBufferObject
		{
			public OutfitCategories Outfit = PassionCore.Settings.Outfit;

			public PassionMotives Motives = PassionCore.Settings.Motives;

			public int InitialCategory = PassionCore.Settings.InitialCategory;

			public bool NakedShower = PassionCore.Settings.NakedShower;

			public void Save()
			{
				PassionCore.Settings.Outfit = Outfit;
				PassionCore.Settings.Motives = Motives;
				PassionCore.Settings.InitialCategory = InitialCategory;
				PassionCore.Settings.NakedShower = NakedShower;
			}
		}

		public const uint OkButton = 2822726152u;

		public const uint CancelButton = 2822726153u;

		public const uint GeneralTab = 2822726224u;

		public const uint OutfitCombo = 2822726243u;

		public const uint MotivesCombo = 2822726244u;

		public const uint InitialCategoryCombo = 2822726245u;

		public const uint NakedShowerButton = 2822726246u;

		public const uint RestoreDefaultsButton = 2822726247u;

		private static PassionSettings _dialog;

		public List<Window> AdditionalTabs = new List<Window>();

		private readonly GeneralTabComponents _generalComponents = new GeneralTabComponents();

		private readonly SettingsBufferObject _settingsBuffer = new SettingsBufferObject();

		public PassionSettings()
			: base("PassionSettingsDialog", 1, false, PauseMode.PauseSimulator, null)
		{
			Button button = mModalDialogWindow.GetChildByID(2822726152u, true) as Button;
			if (button != null) button.Click += OnOkay;
			Button button2 = mModalDialogWindow.GetChildByID(2822726153u, true) as Button;
			if (button2 != null) button2.Click += OnCancel;
			OkayID = 2822726152u;
			CancelID = 2822726153u;
			Window generalTabWindow = mModalDialogWindow.GetChildByID(2822726224u, true) as Window;
			if (generalTabWindow != null)
			{
				Button button3 = generalTabWindow.GetChildByID(2822726247u, true) as Button;
				if (button3 != null) button3.Click += OnRestoreDefaultsClick;
			}

			if (generalTabWindow != null)
			{
				_generalComponents.Outfit = generalTabWindow.GetChildByID(2822726243u, true) as ComboBox;
				if (_generalComponents.Outfit != null)
				{
					_generalComponents.Outfit.ValueList.Add("Preferred Only", OutfitCategories.None);
					_generalComponents.Outfit.ValueList.Add("Naked", OutfitCategories.Naked);
					_generalComponents.Outfit.ValueList.Add("Sleepwear", OutfitCategories.Sleepwear);
					try
					{
						int num = _generalComponents.Outfit.ValueList.IndexOf(_settingsBuffer.Outfit);
						if (num < 0)
						{
							num = 0;
						}

						_generalComponents.Outfit.CurrentSelection = (uint)num;
					}
					catch
					{
						// ignored
					}

					_generalComponents.Outfit.SelectionChange += OnStartingOutfitChange;
					_generalComponents.Motives = generalTabWindow.GetChildByID(2822726244u, true) as ComboBox;
					if (_generalComponents.Motives != null)
					{
						_generalComponents.Motives.ValueList.Add("Standard", PassionMotives.PassionStandard);
						_generalComponents.Motives.ValueList.Add("No Change", PassionMotives.EaDefault);
						_generalComponents.Motives.ValueList.Add("No Decay", PassionMotives.NoDecay);
						_generalComponents.Motives.ValueList.Add("Freeze", PassionMotives.Freeze);
						_generalComponents.Motives.ValueList.Add("Max All", PassionMotives.MaxAll);
						try
						{
							int num2 = _generalComponents.Motives.ValueList.IndexOf(_settingsBuffer.Motives);
							if (num2 < 0)
							{
								num2 = 0;
							}

							_generalComponents.Motives.CurrentSelection = (uint)num2;
						}
						catch
						{
							// ignored
						}

						_generalComponents.Motives.SelectionChange += OnMotivesChange;
					}

					_generalComponents.InitialCategory = generalTabWindow.GetChildByID(2822726245u, true) as ComboBox;
					if (_generalComponents.InitialCategory != null)
					{
						_generalComponents.InitialCategory.ValueList.Add("Any", 1);
						_generalComponents.InitialCategory.ValueList.Add("Masturbate", 48);
						_generalComponents.InitialCategory.ValueList.Add("Oral", 2);
						_generalComponents.InitialCategory.ValueList.Add("Fuck", 12);
						try
						{
							int num3 = _generalComponents.InitialCategory.ValueList.IndexOf(_settingsBuffer
								.InitialCategory);
							if (num3 < 0)
							{
								num3 = 0;
							}

							_generalComponents.InitialCategory.CurrentSelection = (uint)num3;
						}
						catch
						{
							// ignored
						}
					}

					_generalComponents.Outfit.SelectionChange += OnInitialCategoryChange;
				}

				_generalComponents.NakedShower = generalTabWindow.GetChildByID(2822726246u, true) as Button;
			}

			if (_generalComponents.NakedShower == null) return;
			_generalComponents.NakedShower.Selected = _settingsBuffer.NakedShower;
			_generalComponents.NakedShower.Click += OnNakedShowerClick;
		}

		private void OnStartingOutfitChange(WindowBase sender, UISelectionChangeEventArgs eventArgs)
		{
			try
			{
				_settingsBuffer.Outfit = (OutfitCategories)((ComboBox)sender).EntryTags[(int)eventArgs.NewIndex];
			}
			catch
			{
				// ignored
			}
		}

		private void OnMotivesChange(WindowBase sender, UISelectionChangeEventArgs eventArgs)
		{
			try
			{
				_settingsBuffer.Motives = (PassionMotives)((ComboBox)sender).EntryTags[(int)eventArgs.NewIndex];
			}
			catch
			{
				// ignored
			}
		}

		private void OnInitialCategoryChange(WindowBase sender, UISelectionChangeEventArgs eventArgs)
		{
			try
			{
				_settingsBuffer.InitialCategory = (int)((ComboBox)sender).EntryTags[(int)eventArgs.NewIndex];
			}
			catch
			{
				// ignored
			}
		}

		private void OnNakedShowerClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			Button button = sender as Button;
			if (button != null)
			{
				_settingsBuffer.NakedShower = button.Selected;
			}
		}

		private void OnRestoreDefaultsClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			_settingsBuffer.Outfit = OutfitCategories.None;
			_settingsBuffer.Motives = PassionMotives.PassionStandard;
			_settingsBuffer.InitialCategory = 1;
			_settingsBuffer.NakedShower = true;
			try
			{
				_generalComponents.Outfit.CurrentSelection = 0u;
				_generalComponents.Motives.CurrentSelection = 0u;
				_generalComponents.InitialCategory.CurrentSelection = 0u;
				_generalComponents.NakedShower.Selected = true;
			}
			catch
			{
				// ignored
			}
		}

		private void OnOkay(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			_settingsBuffer.Save();
			eventArgs.Handled = true;
			EndDialog(sender.ID);
		}

		private void OnCancel(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			eventArgs.Handled = true;
			EndDialog(sender.ID);
		}

		private static void PassionSettingsTask()
		{
			_dialog = new PassionSettings();
			_dialog.StartModal();
			_dialog = null;
		}

		public static void Show()
		{
			Simulator.AddObject(new OneShotFunctionTask(PassionSettingsTask));
		}
	}
}
