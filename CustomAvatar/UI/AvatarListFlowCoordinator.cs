using System;
using System.Linq;
using BeatSaberMarkupLanguage;
using CustomAvatar.Utilities;
using HMUI;
using UnityEngine;

namespace CustomAvatar.UI
{
	// Token: 0x02000031 RID: 49
	internal class AvatarListFlowCoordinator : FlowCoordinator
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x00006E58 File Offset: 0x00005058
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			this._mainScreen = GameObject.Find("MainScreen");
			this._mainScreenScale = this._mainScreen.transform.localScale;
			base.showBackButton = true;
			if (firstActivation)
			{
				base.title = "角色模型";
				ViewController viewController = BeatSaberUI.CreateViewController<MirrorViewController>();
				ViewController viewController2 = BeatSaberUI.CreateViewController<SettingsViewController>();
				ViewController viewController3 = BeatSaberUI.CreateViewController<AvatarListViewController>();
				base.ProvideInitialViewControllers(viewController, viewController2, viewController3, null, null);
				this._mainScreen.transform.localScale = Vector3.zero;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006EDC File Offset: 0x000050DC
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			this._mainScreen.transform.localScale = this._mainScreenScale;
			MainFlowCoordinator mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First<MainFlowCoordinator>();
			mainFlowCoordinator.InvokePrivateMethod("DismissFlowCoordinator", new object[] { this, null, false });
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00003123 File Offset: 0x00001323
		protected override void DidDeactivate(FlowCoordinator.DeactivationType deactivationType)
		{
		}

		// Token: 0x04000175 RID: 373
		private GameObject _mainScreen;

		// Token: 0x04000176 RID: 374
		private Vector3 _mainScreenScale;
	}
}
