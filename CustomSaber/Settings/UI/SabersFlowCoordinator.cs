using System;
using BeatSaberMarkupLanguage;
using CustomSaber.Data;
using CustomSaber.Utilities;
using HMUI;
using UnityEngine;

namespace CustomSaber.Settings.UI
{
	// Token: 0x02000016 RID: 22
	internal class SabersFlowCoordinator : FlowCoordinator
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00004660 File Offset: 0x00002860
		public void Awake()
		{
			bool flag = !this.saberPreviewView;
			if (flag)
			{
				this.saberPreviewView = BeatSaberUI.CreateViewController<SaberPreviewViewController>();
			}
			bool flag2 = !this.saberSettingsView;
			if (flag2)
			{
				this.saberSettingsView = BeatSaberUI.CreateViewController<SaberSettingsViewController>();
			}
			bool flag3 = !this.saberListView;
			if (flag3)
			{
				this.saberListView = BeatSaberUI.CreateViewController<SaberListViewController>();
				SaberListViewController saberListViewController = this.saberListView;
				saberListViewController.customSaberChanged = (Action<CustomSaberData>)Delegate.Combine(saberListViewController.customSaberChanged, new Action<CustomSaberData>(this.saberPreviewView.OnSaberWasChanged));
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000046F8 File Offset: 0x000028F8
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			try
			{
				if (firstActivation)
				{
					base.title = "光剑定制";
					base.showBackButton = true;
					base.ProvideInitialViewControllers(this.saberListView, this.saberSettingsView, this.saberPreviewView, null, null);
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error(ex);
			}
			Logger.log.Info("=====================================");
			DefaultSaberGrabber defaultSaberGrabber = new GameObject("Default Saber Grabber").AddComponent<DefaultSaberGrabber>();
			Object.DontDestroyOnLoad(defaultSaberGrabber);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004788 File Offset: 0x00002988
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, false);
		}

		// Token: 0x04000062 RID: 98
		private SaberListViewController saberListView;

		// Token: 0x04000063 RID: 99
		private SaberPreviewViewController saberPreviewView;

		// Token: 0x04000064 RID: 100
		private SaberSettingsViewController saberSettingsView;
	}
}
