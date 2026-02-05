using System;
using BeatSaberMarkupLanguage;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.Helper;
using BeatSaberMultiplayer.UI.ViewControllers.RoomView;
using HMUI;

namespace BeatSaberMultiplayer.UI.FlowCoordinators
{
	// Token: 0x02000067 RID: 103
	internal class CreateRoomFlowCoordinator : FlowCoordinator
	{
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060007D1 RID: 2001 RVA: 0x00020CCC File Offset: 0x0001EECC
		// (remove) Token: 0x060007D2 RID: 2002 RVA: 0x00020D04 File Offset: 0x0001EF04
		public event Action didFinishEvent;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060007D3 RID: 2003 RVA: 0x00020D3C File Offset: 0x0001EF3C
		// (remove) Token: 0x060007D4 RID: 2004 RVA: 0x00020D74 File Offset: 0x0001EF74
		public event Action<RoomSettings> CreateRoomEvent;

		// Token: 0x060007D5 RID: 2005 RVA: 0x00020DAC File Offset: 0x0001EFAC
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			if (firstActivation)
			{
				base.title = "创建房间";
				this._createRoomViewController = BeatSaberUI.CreateViewController<CreateRoomViewController>();
				this._createRoomViewController.CreatedRoomEvent += this.CreateRoom;
				this._createRoomViewController.SavePresetEvent += this.SavePreset;
			}
			base.showBackButton = true;
			base.ProvideInitialViewControllers(this._createRoomViewController, null, null, null, null);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00020E17 File Offset: 0x0001F017
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			if (topViewController == this._createRoomViewController)
			{
				Action action = this.didFinishEvent;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00020E37 File Offset: 0x0001F037
		private void SavePreset(RoomSettings settings, string name)
		{
			new RoomPreset(settings).SavePreset("UserData/RoomPresets/" + name + ".json");
			PresetsCollection.ReloadPresets();
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00020E59 File Offset: 0x0001F059
		private void CreateRoom(RoomSettings settings)
		{
			Action<RoomSettings> createRoomEvent = this.CreateRoomEvent;
			if (createRoomEvent == null)
			{
				return;
			}
			createRoomEvent(settings);
		}

		// Token: 0x040003E7 RID: 999
		private CreateRoomViewController _createRoomViewController;
	}
}
