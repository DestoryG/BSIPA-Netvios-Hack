using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Settings.UI.ViewControllers;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Settings
{
	// Token: 0x0200006D RID: 109
	internal class ModSettingsFlowCoordinator : FlowCoordinator
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x0000BB94 File Offset: 0x00009D94
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			if (firstActivation)
			{
				base.title = "Mod Settings";
				this.navigationController = BeatSaberUI.CreateViewController<NavigationController>();
				PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatSaberMarkupLanguage.Views.settings-buttons.bsml"), this.navigationController.gameObject, this);
				this.settingsMenuListViewController = BeatSaberUI.CreateViewController<SettingsMenuListViewController>();
				SettingsMenuListViewController settingsMenuListViewController = this.settingsMenuListViewController;
				settingsMenuListViewController.clickedMenu = (Action<ViewController>)Delegate.Combine(settingsMenuListViewController.clickedMenu, new Action<ViewController>(this.OpenMenu));
				base.SetViewControllerToNavigationController(this.navigationController, this.settingsMenuListViewController);
				base.ProvideInitialViewControllers(this.navigationController, null, null, null, null);
				foreach (CustomListTableData.CustomCellInfo customCellInfo in BSMLSettings.instance.settingsMenus)
				{
					(customCellInfo as SettingsMenu).parserParams.AddEvent("back", new Action(this.Back));
				}
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000BC9C File Offset: 0x00009E9C
		public void OpenMenu(ViewController viewController)
		{
			this.OpenMenu(viewController, false, false);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000BCA8 File Offset: 0x00009EA8
		public void OpenMenu(ViewController viewController, bool isSubmenu, bool isBack)
		{
			if (this.isPresenting)
			{
				return;
			}
			if (!isBack)
			{
				if (isSubmenu)
				{
					this.submenuStack.Push(this.activeController);
				}
				else
				{
					this.submenuStack.Clear();
				}
			}
			bool flag = this.activeController != null;
			if (flag)
			{
				base.PopViewControllerFromNavigationController(this.navigationController, null, true);
			}
			base.PushViewControllerToNavigationController(this.navigationController, viewController, delegate
			{
				this.isPresenting = false;
			}, flag);
			this.activeController = viewController;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000BD24 File Offset: 0x00009F24
		public void ShowInitial()
		{
			if (this.activeController != null)
			{
				return;
			}
			this.settingsMenuListViewController.list.tableView.SelectCellWithIdx(0, false);
			this.OpenMenu((BSMLSettings.instance.settingsMenus.First<CustomListTableData.CustomCellInfo>() as SettingsMenu).viewController);
			this.isPresenting = true;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000BD7D File Offset: 0x00009F7D
		[UIAction("ok-click")]
		private void Ok()
		{
			this.Apply();
			Resources.FindObjectsOfTypeAll<MenuTransitionsHelper>().First<MenuTransitionsHelper>().RestartGame();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000BD94 File Offset: 0x00009F94
		[UIAction("apply-click")]
		private void Apply()
		{
			this.EmitEventToAll("apply");
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000BDA1 File Offset: 0x00009FA1
		[UIAction("cancel-click")]
		private void Cancel()
		{
			if (this.isPresenting || this.isAnimating)
			{
				return;
			}
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, false);
			this.EmitEventToAll("cancel");
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000BDCC File Offset: 0x00009FCC
		private void Back()
		{
			if (this.submenuStack.Count > 0)
			{
				this.OpenMenu(this.submenuStack.Pop(), false, true);
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		private void EmitEventToAll(string ev)
		{
			foreach (CustomListTableData.CustomCellInfo customCellInfo in BSMLSettings.instance.settingsMenus)
			{
				(customCellInfo as SettingsMenu).parserParams.EmitEvent(ev);
			}
		}

		// Token: 0x04000040 RID: 64
		protected SettingsMenuListViewController settingsMenuListViewController;

		// Token: 0x04000041 RID: 65
		protected NavigationController navigationController;

		// Token: 0x04000042 RID: 66
		protected ViewController activeController;

		// Token: 0x04000043 RID: 67
		private Stack<ViewController> submenuStack = new Stack<ViewController>();

		// Token: 0x04000044 RID: 68
		private bool isPresenting;

		// Token: 0x04000045 RID: 69
		public bool isAnimating;
	}
}
