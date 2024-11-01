using System;
using System.Collections.Generic;
using Sims3.SimIFace;
using Sims3.UI;

namespace S3_Passion
{
	public class MenuList : ModalDialog
	{
		private const int CANCEL_BUTTON = 99576786;

		private const int ITEM_TABLE = 99576784;

		private const string kLayoutName = "UiObjectPicker";

		private const int kWinExportID = 1;

		private const int OKAY_BUTTON = 99576785;

		private const int TABLE_BACKGROUND = 99576788;

		private const int TABLE_BEZEL = 99576789;

		private const int TITLE_TEXT = 99576787;

		private Button mCloseButton;

		private Button mOkayButton;

		private List<ObjectPicker.RowInfo> mPreSelectedRows;

		private List<ObjectPicker.RowInfo> mResult;

		private ObjectPicker mTable;

		private Vector2 mTableOffset;

		public List<ObjectPicker.RowInfo> Result
		{
			get
			{
				return mResult;
			}
		}

		public MenuList(bool modal, PauseMode pauseMode, string title, string buttonTrue, string buttonFalse, List<ObjectPicker.TabInfo> listObjs, List<ObjectPicker.HeaderInfo> headers, int numSelectableRows, Vector2 position, bool viewTypeToggle, List<ObjectPicker.RowInfo> preSelectedRows, bool showHeadersAndToggle, bool disableCloseButton)
			: base("UiObjectPicker", 1, modal, pauseMode, null)
		{
			if (mModalDialogWindow != null)
			{
				Text text = mModalDialogWindow.GetChildByID(99576787u, false) as Text;
				text.Caption = title;
				mTable = mModalDialogWindow.GetChildByID(99576784u, false) as ObjectPicker;
				mTable.ObjectTable.TableChanged += OnTableChanged;
				mTable.SelectionChanged += OnSelectionChanged;
				mTable.RowSelected += OnSelectionChanged;
				mOkayButton = mModalDialogWindow.GetChildByID(99576785u, false) as Button;
				mOkayButton.TooltipText = buttonTrue;
				mOkayButton.Enabled = false;
				mOkayButton.Click += OnOkayButtonClick;
				base.OkayID = mOkayButton.ID;
				base.SelectedID = mOkayButton.ID;
				mCloseButton = mModalDialogWindow.GetChildByID(99576786u, false) as Button;
				mCloseButton.TooltipText = buttonFalse;
				mCloseButton.Click += OnCloseButtonClick;
				if (disableCloseButton)
				{
					mCloseButton.Enabled = false;
				}
				base.CancelID = mCloseButton.ID;
				mTableOffset = mModalDialogWindow.Area.BottomRight - mModalDialogWindow.Area.TopLeft - (mTable.Area.BottomRight - mTable.Area.TopLeft);
				mTable.ShowHeaders = showHeadersAndToggle;
				mTable.ShowToggle = showHeadersAndToggle;
				mTable.ObjectTable.NoAutoSizeGridResize = true;
				mTable.Populate(listObjs, headers, numSelectableRows);
				mTable.ViewTypeToggle = viewTypeToggle;
				mPreSelectedRows = preSelectedRows;
				mTable.TablePopulationComplete += OnPopulationCompleted;
				if (!mTable.ShowToggle)
				{
					Window window = mModalDialogWindow.GetChildByID(99576788u, false) as Window;
					Window window2 = mModalDialogWindow.GetChildByID(99576789u, false) as Window;
					mTable.Area = new Rect(mTable.Area.TopLeft.x, mTable.Area.TopLeft.y - 64f, mTable.Area.BottomRight.x, mTable.Area.BottomRight.y);
					window2.Area = new Rect(window2.Area.TopLeft.x, window2.Area.TopLeft.y - 64f, window2.Area.BottomRight.x, window2.Area.BottomRight.y);
					window.Area = new Rect(window.Area.TopLeft.x, window.Area.TopLeft.y - 64f, window.Area.BottomRight.x, window.Area.BottomRight.y);
				}
				mModalDialogWindow.Area = new Rect(mModalDialogWindow.Area.TopLeft, mModalDialogWindow.Area.TopLeft + mTable.TableArea.BottomRight + mTableOffset);
				Rect area = mModalDialogWindow.Area;
				float num = area.BottomRight.x - area.TopLeft.x;
				float num2 = area.BottomRight.y - area.TopLeft.y;
				if (!mTable.ShowToggle)
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
				mModalDialogWindow.Visible = true;
			}
		}

		private void OnCloseButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			eventArgs.Handled = true;
			EndDialog(base.CancelID);
		}

		protected override bool OnEnd(uint endID)
		{
			if (endID == base.OkayID)
			{
				if (!mOkayButton.Enabled)
				{
					return false;
				}
				mResult = mTable.Selected;
			}
			else
			{
				mResult = null;
			}
			mTable.Populate(null, null, 0);
			return true;
		}

		private void OnOkayButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
		{
			eventArgs.Handled = true;
			EndDialog(base.OkayID);
		}

		private void OnPopulationCompleted()
		{
			mTable.Selected = mPreSelectedRows;
		}

		private void OnSelectionChanged(List<ObjectPicker.RowInfo> selectedRows)
		{
			Audio.StartSound("ui_tertiary_button");
			OnTableChanged();
			EndDialog(base.OkayID);
		}

		private void OnTableChanged()
		{
			List<ObjectPicker.RowInfo> selected = mTable.Selected;
			if (selected != null && selected.Count > 0)
			{
				mOkayButton.Enabled = true;
			}
			else
			{
				mOkayButton.Enabled = false;
			}
		}

		public static List<ObjectPicker.RowInfo> Show(string title, string buttonTrue, string buttonFalse, List<ObjectPicker.TabInfo> tabs, List<ObjectPicker.HeaderInfo> headers)
		{
			return Show(title, buttonTrue, buttonFalse, tabs, headers, 1, new Vector2(-1f, -1f), false, true, false);
		}

		public static List<ObjectPicker.RowInfo> Show(string title, string buttonTrue, string buttonFalse, List<ObjectPicker.TabInfo> tabs, List<ObjectPicker.HeaderInfo> headers, int numSelectableRows, Vector2 position, bool viewTypeToggle, bool showHeadersAndToggle, bool disableCloseButton)
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
