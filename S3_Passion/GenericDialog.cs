using System.Collections.Generic;
using Passion.S3_Passion.UI;
using Sims3.UI;

namespace Passion.S3_Passion
{
	internal class GenericDialog
	{
		public class OptionList<T>
		{
			private readonly List<Option<T>> _mOptions = new List<Option<T>>();

			public List<Option<T>> Options
			{
				get
				{
					return _mOptions;
				}
			}

			public int Count
			{
				get
				{
					return Options.Count;
				}
			}

			public void Add(string text, T value)
			{
				Add(text, value, false);
			}

			public void Add(string text, T value, bool selected)
			{
				Options.Add(Option<T>.Create(text, value, selected));
			}
		}

		public class Option<T>
		{
			public string Text = string.Empty;

			public bool Selected;

			public T Value;

			public static Option<T> Create(string text, T value)
			{
				return Create(text, value, false);
			}

			public static Option<T> Create(string text, T value, bool selected)
			{
				Option<T> option = new Option<T>();
				option.Text = text;
				option.Value = value;
				option.Selected = selected;
				return option;
			}
		}

		public static List<T> Ask<T>(OptionList<T> list, int selectable)
		{
			return Ask(list, selectable, string.Empty);
		}

		private static List<T> Ask<T>(OptionList<T> list, int selectable, string prompt)
		{
			List<T> list2 = new List<T>();
			List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();
			foreach (Option<T> option in list.Options)
			{
				ObjectPicker.RowInfo rowInfo = new ObjectPicker.RowInfo(option, new List<ObjectPicker.ColumnInfo>());
				rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(option.Text));
				list3.Add(rowInfo);
			}
			List<ObjectPicker.HeaderInfo> list4 = new List<ObjectPicker.HeaderInfo>();
			list4.Add(new ObjectPicker.HeaderInfo(string.Empty, null, 300));
			List<ObjectPicker.TabInfo> list5 = new List<ObjectPicker.TabInfo>();
			list5.Add(new ObjectPicker.TabInfo("shop_all_r2", "All", list3));
			List<ObjectPicker.RowInfo> list6 = ObjectPickerDialog.Show(false, ModalDialog.PauseMode.PauseSimulator, prompt, "Ok", "Cancel", list5, list4, selectable);
			if (list6 == null) return list2;
			foreach (ObjectPicker.RowInfo item in list6)
			{
				Option<T> option = item.Item as Option<T>;
				if (option != null)
				{
					list2.Add(option.Value);
				}
			}
			return list2;
		}

		protected static T Ask<T>(OptionList<T> list, string prompt)
		{
			return Ask(list, prompt, false);
		}

		private static T Ask<T>(OptionList<T> list, string prompt, bool alwaysList)
		{
			T result = default(T);
			if (list == null || list.Count <= 0) return result;
			try
			{
				if (!alwaysList && list.Count < 4)
				{
					switch (list.Count)
					{
						case 1:
							result = list.Options[0].Value;
							return result;
						case 2:
							if (TwoButtonDialog.Show(prompt, list.Options[0].Text, list.Options[1].Text))
							{
								result = list.Options[0].Value;
								return result;
							}
							result = list.Options[1].Value;
							return result;
						case 3:
							switch (ThreeButtonDialog.Show(prompt, list.Options[0].Text, list.Options[1].Text, list.Options[2].Text))
							{
								case ThreeButtonDialog.ButtonPressed.SecondButton:
									result = list.Options[1].Value;
									return result;
								case ThreeButtonDialog.ButtonPressed.ThirdButton:
									result = list.Options[2].Value;
									return result;
								case ThreeButtonDialog.ButtonPressed.FirstButton:
								default:
									result = list.Options[0].Value;
									return result;
							}
					}
				}
				else
				{
					List<ObjectPicker.RowInfo> list2 = new List<ObjectPicker.RowInfo>();
					foreach (Option<T> option in list.Options)
					{
						ObjectPicker.RowInfo rowInfo = new ObjectPicker.RowInfo(option, new List<ObjectPicker.ColumnInfo>());
						rowInfo.ColumnInfo.Add(new ObjectPicker.TextColumn(option.Text));
						list2.Add(rowInfo);
					}
					List<ObjectPicker.HeaderInfo> list3 = new List<ObjectPicker.HeaderInfo>();
					list3.Add(new ObjectPicker.HeaderInfo("   ", null, 300));
					List<ObjectPicker.TabInfo> list4 = new List<ObjectPicker.TabInfo>();
					list4.Add(new ObjectPicker.TabInfo("shop_all_r2", "All", list2));
					List<ObjectPicker.RowInfo> list5 = MenuList.Show(prompt, "Ok", "Cancel", list4, list3);
					if (list5 != null && list5.Count > 0 && list5[0].Item is Option<T>)
					{
						result = ((Option<T>)list5[0].Item).Value;
						return result;
					}
				}
			}
			catch
			{
				// ignored
			}

			return result;
		}
	}
}
