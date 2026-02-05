using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Notify;
using HMUI;
using TMPro;

namespace BeatSaberMarkupLanguage.ViewControllers
{
	// Token: 0x02000011 RID: 17
	[ViewDefinition("BeatSaberMarkupLanguage.Views.test.bsml")]
	public class TestViewController : BSMLAutomaticViewController, INotifiableHost
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004988 File Offset: 0x00002B88
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004990 File Offset: 0x00002B90
		[UIValue("header")]
		public string HeaderText
		{
			get
			{
				return this.headerText;
			}
			set
			{
				this.headerText = value;
				base.NotifyPropertyChanged("HeaderText");
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000049A4 File Offset: 0x00002BA4
		[UIValue("contents")]
		public List<object> contents
		{
			get
			{
				return new List<object>
				{
					new TestListObject("first", false),
					new TestListObject("second", true),
					new TestListObject("third", true)
				};
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000049DE File Offset: 0x00002BDE
		[UIAction("click")]
		private void ButtonPress()
		{
			this.HeaderText = "It works!";
			this.buttonText.text = "Clicked";
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000049FB File Offset: 0x00002BFB
		[UIAction("cell click")]
		private void CellClick(TableView tableView, TestListObject testObj)
		{
			Logger.log.Info("Clicked - " + testObj.title);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004A17 File Offset: 0x00002C17
		[UIAction("keyboard-enter")]
		private void KeyboardEnter(string value)
		{
			Logger.log.Info("Keyboard typed: " + value);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004A30 File Offset: 0x00002C30
		[UIAction("#post-parse")]
		private void PostParse()
		{
			List<CustomListTableData.CustomCellInfo> list = new List<CustomListTableData.CustomCellInfo>();
			for (int i = 0; i < 10; i++)
			{
				list.Add(new CustomListTableData.CustomCellInfo("test" + i.ToString(), "yee haw", null, null));
			}
			this.tableData.data = list;
			this.tableData.tableView.ReloadData();
		}

		// Token: 0x04000025 RID: 37
		public string headerText = "Header comes from code!";

		// Token: 0x04000026 RID: 38
		[UIComponent("test-external")]
		public TextMeshProUGUI buttonText;

		// Token: 0x04000027 RID: 39
		[UIComponent("list")]
		public CustomListTableData tableData;
	}
}
