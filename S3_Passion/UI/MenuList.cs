using System;
using System.Collections.Generic;
using Sims3.SimIFace;
using Sims3.UI;

namespace Passion.S3_Passion.UI
{
	public class MenuList : ModalDialog
	{
		private const int CancelButton = 99576786;

		private const int ItemTable = 99576784;

		private const string KLayoutName = "UiObjectPicker";

		private const int KWinExportId = 1;

		private const int OkayButton = 99576785;

		private const int TableBackground = 99576788;

		private const int TableBezel = 99576789;

		private const int TitleText = 99576787;

		private readonly Button _mCloseButton;

		private readonly Button _mOkayButton;

		private readonly List<ObjectPicker.RowInfo> _mPreSelectedRows;

		private List<ObjectPicker.RowInfo> _mResult;

		private readonly ObjectPicker _mTable;

		private List<ObjectPicker.RowInfo> Result
		{
			get
			{
				return _mResult;
			}
		}

		private MenuList(bool modal, PauseMode pauseMode, string title, string buttonTrue, string buttonFalse, List<ObjectPicker.TabInfo> listObjs, List<ObjectPicker.HeaderInfo> headers, int numSelectableRows, Vector2 position, bool viewTypeToggle, List<ObjectPicker.RowInfo> preSelectedRows, bool showHeadersAndToggle, bool disableCloseButton)
			: base("UiObjectPicker", 1, modal, pauseMode, null)
		{
			if (mModalDialogWindow == null) return;
			Text text = mModalDialogWindow.GetChildByID(99576787u, false) as Text;
			if (text != null) text.Caption = title;
			_mTable = mModalDialogWindow.GetChildByID(99576784u, false) as ObjectPicker;
			if (_mTable != null)
			{
				_mTable.ObjectTable.TableChanged += OnTableChanged;
				_mTable.SelectionChanged += OnSelectionChanged;
				_mTable.RowSelected += OnSelectionChanged;
				_mOkayButton = mModalDialogWindow.GetChildByID(99576785u, false) as Button;
				if (_mOkayButton != null)
				{
					_mOkayButton.TooltipText = buttonTrue;
					_mOkayButton.Enabled = false;
					_mOkayButton.Click += OnOkayButtonClick;
					OkayID = _mOkayButton.ID;
					SelectedID = _mOkayButton.ID;
				}

				_mCloseButton = mModalDialogWindow.GetChildByID(99576786u, false) as Button;
				if (_mCloseButton != null)
				{
					_mCloseButton.TooltipText = buttonFalse;
					_mCloseButton.Click += OnCloseButtonClick;
					if (disableCloseButton)
					{
						_mCloseButton.Enabled = false;
					}

					CancelID = _mCloseButton.ID;
				}

				Vector2 mTableOffset = mModalDialogWindow.Area.BottomRight - mModalDialogWindow.Area.TopLeft -
				                       (_mTable.Area.BottomRight - _mTable.Area.TopLeft);
				_mTable.ShowHeaders = showHeadersAndToggle;
				_mTable.ShowToggle = showHeadersAndToggle;
				_mTable.ObjectTable.NoAutoSizeGridResize = true;
				_mTable.Populate(listObjs, headers, numSelectableRows);
				_mTable.ViewTypeToggle = viewTypeToggle;
				_mPreSelectedRows = preSelectedRows;
				_mTable.TablePopulationComplete += OnPopulationCompleted;
				if (!_mTable.ShowToggle)
				{
					Window window = mModalDialogWindow.GetChildByID(99576788u, false) as Window;
					Window window2 = mModalDialogWindow.GetChildByID(99576789u, false) as Window;
					_mTable.Area = new Rect(_mTable.Area.TopLeft.x, _mTable.Area.TopLeft.y - 64f,
						_mTable.Area.BottomRight.x, _mTable.Area.BottomRight.y);
					if (window2 != null)
						window2.Area = new Rect(window2.Area.TopLeft.x, window2.Area.TopLeft.y - 64f,
							window2.Area.BottomRight.x, window2.Area.BottomRight.y);
					if (window != null)
						window.Area = new Rect(window.Area.TopLeft.x, window.Area.TopLeft.y - 64f,
							window.Area.BottomRight.x, window.Area.BottomRight.y);
				}

				mModalDialogWindow.Area = new Rect(mModalDialogWindow.Area.TopLeft,
					mModalDialogWindow.Area.TopLeft + _mTable.TableArea.BottomRight + mTableOffset);
				Rect area = mModalDialogWindow.Area;
				float num = area.BottomRight.x - area.TopLeft.x;
				float num2 = area.BottomRight.y - area.TopLeft.y;
				if (!_mTable.ShowToggle)
				{
					num2 -= 50f;
				}

				float num3 = position.x;
				float num4 = position.y;
				if (num3 < 0f && num4 < 0f)
				{
					Rect area2 = mModalDialogWindow.Parent.Area;
					float num5 = area2.BottomRight.x - area2.TopLeft.x;
					float num6 = area2.BottomRight.y - area2.TopLeft.y;
					num3 = (float)Math.Round((num5 - num) / 2f);
					num4 = (float)Math.Round((num6 - num2) / 2f);
				}

				area.Set(num3, num4, num3 + num, num4 + num2);
				mModalDialogWindow.Area = area;
			}

			mModalDialogWindow.Visible = true;
		}

		private void OnCloseButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			eventArgs.Handled = true;
			EndDialog(CancelID);
		}

		public override bool OnEnd(uint endId)
		{
			if (endId == OkayID)
			{
				if (!_mOkayButton.Enabled)
				{
					return false;
				}
				_mResult = _mTable.Selected;
			}
			else
			{
				_mResult = null;
			}
			_mTable.Populate(null, null, 0);
			return true;
		}

		private void OnOkayButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			eventArgs.Handled = true;
			EndDialog(OkayID);
		}

		private void OnPopulationCompleted()
		{
			_mTable.Selected = _mPreSelectedRows;
		}

		private void OnSelectionChanged(List<ObjectPicker.RowInfo> selectedRows)
		{
			Audio.StartSound("ui_tertiary_button");
			OnTableChanged();
			EndDialog(OkayID);
		}

		private void OnTableChanged()
		{
			List<ObjectPicker.RowInfo> selected = _mTable.Selected;
			if (selected != null && selected.Count > 0)
			{
				_mOkayButton.Enabled = true;
			}
			else
			{
				_mOkayButton.Enabled = false;
			}
		}

		public static List<ObjectPicker.RowInfo> Show(string title, string buttonTrue, string buttonFalse, List<ObjectPicker.TabInfo> tabs, List<ObjectPicker.HeaderInfo> headers)
		{
			return Show(title, buttonTrue, buttonFalse, tabs, headers, 1, new Vector2(-1f, -1f), false, true, false);
		}

		private static List<ObjectPicker.RowInfo> Show(string title, string buttonTrue, string buttonFalse, List<ObjectPicker.TabInfo> tabs, List<ObjectPicker.HeaderInfo> headers, int numSelectableRows, Vector2 position, bool viewTypeToggle, bool showHeadersAndToggle, bool disableCloseButton)
		{
			using (MenuList menuList = new MenuList(false, PauseMode.PauseSimulator, title, buttonTrue, buttonFalse, tabs, headers, numSelectableRows, position, viewTypeToggle, null, showHeadersAndToggle, disableCloseButton))
			{
				menuList.StartModal();
				if (menuList.Result == null || menuList.Result.Count == 0)
				{
					return null;
				}
				return menuList.Result;
			}
		}
	}
}
