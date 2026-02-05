using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Settings.UI.ViewControllers
{
	// Token: 0x0200006F RID: 111
	[ViewDefinition("BeatSaberMarkupLanguage.Views.settings-list.bsml")]
	internal class SettingsMenuListViewController : BSMLAutomaticViewController
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000BF44 File Offset: 0x0000A144
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			if (firstActivation)
			{
				base.rectTransform.sizeDelta = new Vector2(35f, 0f);
				base.rectTransform.anchorMin = new Vector2(0.5f, 0f);
				base.rectTransform.anchorMax = new Vector2(0.5f, 1f);
			}
			this.list.data = BSMLSettings.instance.settingsMenus;
			TableView tableView = this.list.tableView;
			if (tableView == null)
			{
				return;
			}
			tableView.ReloadData();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
		[UIAction("settings-click")]
		private void SettingsClick(TableView tableView, int index)
		{
			Action<ViewController> action = this.clickedMenu;
			if (action == null)
			{
				return;
			}
			action((this.list.data[index] as SettingsMenu).viewController);
		}

		// Token: 0x0400004B RID: 75
		[UIComponent("list")]
		public CustomListTableData list;

		// Token: 0x0400004C RID: 76
		public Action<ViewController> clickedMenu;
	}
}
