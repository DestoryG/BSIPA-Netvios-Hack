using System;
using BeatSaberMarkupLanguage;
using BeatSaberMultiplayer.UI.ViewControllers.MasterView;
using BS_Utils.Utilities;
using HMUI;

namespace BeatSaberMultiplayer.UI.FlowCoordinators
{
	// Token: 0x02000068 RID: 104
	internal class MasterFlowCoordinator : FlowCoordinator
	{
		// Token: 0x060007DA RID: 2010 RVA: 0x00020E74 File Offset: 0x0001F074
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			if (firstActivation)
			{
				base.title = "大师对战";
				this._masterViewController = BeatSaberUI.CreateViewController<MasterViewController>();
			}
			base.showBackButton = true;
			base.ProvideInitialViewControllers(this._masterViewController, null, null, null, null);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00020EA6 File Offset: 0x0001F0A6
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			if (topViewController == this._masterViewController)
			{
				PluginUI.instance.multiPlayerFlowCoordinator.InvokeMethod("DismissFlowCoordinator", new object[] { this, null, false });
			}
		}

		// Token: 0x040003E8 RID: 1000
		private MasterViewController _masterViewController;
	}
}
