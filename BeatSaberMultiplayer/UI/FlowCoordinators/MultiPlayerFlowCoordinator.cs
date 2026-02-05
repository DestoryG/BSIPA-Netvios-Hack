using System;
using BeatSaberMarkupLanguage;
using BeatSaberMultiplayer.UI.ViewControllers.MultiPlayerView;
using HMUI;

namespace BeatSaberMultiplayer.UI.FlowCoordinators
{
	// Token: 0x02000069 RID: 105
	internal class MultiPlayerFlowCoordinator : FlowCoordinator
	{
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060007DD RID: 2013 RVA: 0x00020EE0 File Offset: 0x0001F0E0
		// (remove) Token: 0x060007DE RID: 2014 RVA: 0x00020F18 File Offset: 0x0001F118
		public event Action didFinishEvent;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060007DF RID: 2015 RVA: 0x00020F50 File Offset: 0x0001F150
		// (remove) Token: 0x060007E0 RID: 2016 RVA: 0x00020F88 File Offset: 0x0001F188
		public event Action<int, string> showTcpErrorEvent;

		// Token: 0x060007E1 RID: 2017 RVA: 0x00020FC0 File Offset: 0x0001F1C0
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			if (firstActivation)
			{
				base.title = PluginSetting.Title;
				this._multiPlayerViewController = BeatSaberUI.CreateViewController<MultiPlayerViewController>();
				this._multiPlayerViewController.didSelectRoomsEvent += delegate
				{
					base.PresentFlowCoordinator(this.roomsFlowCoordinator, null, false, false);
				};
				this._multiPlayerViewController.didSelectMasterEvent += delegate
				{
					base.PresentFlowCoordinator(this.masterFlowCoordinator, null, false, false);
				};
				this._userEditorViewController = BeatSaberUI.CreateViewController<UserEditorViewController>();
				if (this.roomsFlowCoordinator == null)
				{
					this.roomsFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<RoomsFlowCoordinator>();
				}
				if (this.masterFlowCoordinator == null)
				{
					this.masterFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<MasterFlowCoordinator>();
				}
			}
			Client.Instance.OccurredTcpConnectErrorEvent -= this.OccurredTcpConnectNetError;
			Client.Instance.OccurredTcpConnectErrorEvent += this.OccurredTcpConnectNetError;
			base.showBackButton = true;
			base.ProvideInitialViewControllers(this._multiPlayerViewController, this._userEditorViewController, null, null, null);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002109C File Offset: 0x0001F29C
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			if (topViewController == this._multiPlayerViewController)
			{
				Action action = this.didFinishEvent;
				if (action != null)
				{
					action();
				}
			}
			Client.Instance.LogoutTcpServer();
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000196A0 File Offset: 0x000178A0
		private void OccurredTcpConnectNetError(int code, string message)
		{
		}

		// Token: 0x040003EB RID: 1003
		public RoomsFlowCoordinator roomsFlowCoordinator;

		// Token: 0x040003EC RID: 1004
		public MasterFlowCoordinator masterFlowCoordinator;

		// Token: 0x040003ED RID: 1005
		private MultiPlayerViewController _multiPlayerViewController;

		// Token: 0x040003EE RID: 1006
		private UserEditorViewController _userEditorViewController;
	}
}
