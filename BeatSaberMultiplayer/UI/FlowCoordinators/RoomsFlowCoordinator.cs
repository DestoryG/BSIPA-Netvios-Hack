using System;
using System.Collections;
using BeatSaberMarkupLanguage;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.Helper;
using BeatSaberMultiplayer.UI.ViewControllers.RoomView;
using BS_Utils.Utilities;
using Com.Netvios.Proto.Outbound;
using HMUI;
using UnityEngine;

namespace BeatSaberMultiplayer.UI.FlowCoordinators
{
	// Token: 0x0200006C RID: 108
	internal class RoomsFlowCoordinator : FlowCoordinator
	{
		// Token: 0x06000828 RID: 2088 RVA: 0x00022FA8 File Offset: 0x000211A8
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			if (firstActivation)
			{
				base.title = "休闲对战";
				this._roomsViewController = BeatSaberUI.CreateViewController<RoomsViewController>();
				this._roomsViewController.selectedRoomEvent += this.SelectedRoom;
				this._roomsViewController.createRoomEvent += this.CreateRoom;
				this._roomsViewController.fastMatchEvent += this.FastMatchRoom;
				this._roomsViewController.cancelMatchEvent += this.CancelMatchRoom;
				this._roomsViewController.refreshRoomsEvent += this.RefreshRoomsPresed;
			}
			base.showBackButton = true;
			base.ProvideInitialViewControllers(this._roomsViewController, null, null, null, null);
			Client.Instance.OccurredTcpConnectErrorEvent -= this.OccurredTcpConnectNetError;
			Client.Instance.OccurredTcpConnectErrorEvent += this.OccurredTcpConnectNetError;
			base.StartCoroutine(this.UpdateRoomsListCoroutine());
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000196A0 File Offset: 0x000178A0
		private void OccurredTcpConnectNetError(int code, string message)
		{
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00023094 File Offset: 0x00021294
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			if (topViewController == this._roomsViewController)
			{
				PluginUI.instance.multiPlayerFlowCoordinator.InvokeMethod("DismissFlowCoordinator", new object[] { this, null, false });
			}
			RoomsViewController roomsViewController = this._roomsViewController;
			if (roomsViewController == null)
			{
				return;
			}
			roomsViewController.CancelAutoMatch();
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x000230E7 File Offset: 0x000212E7
		private void RefreshRoomsPresed()
		{
			base.StartCoroutine(this.UpdateRoomsListCoroutine());
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000230F6 File Offset: 0x000212F6
		private IEnumerator UpdateRoomsListCoroutine()
		{
			yield return null;
			Client.Instance.GetRoomsEvent -= this.GetRoomsCallback;
			Client.Instance.GetRoomsEvent += this.GetRoomsCallback;
			Client.Instance.GetRooms(0, 100);
			yield break;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00023105 File Offset: 0x00021305
		private void GetRoomsCallback(int code, string msg, RoomList rooms)
		{
			Client.Instance.GetRoomsEvent -= this.GetRoomsCallback;
			if (code != 0)
			{
				this.DisplayError(msg);
				return;
			}
			this._roomsViewController.SetRooms(rooms);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00023134 File Offset: 0x00021334
		private void CreateRoom()
		{
			if (this.createRoomFlowCoordinator == null)
			{
				this.createRoomFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<CreateRoomFlowCoordinator>();
				this.createRoomFlowCoordinator.didFinishEvent += delegate
				{
					base.DismissFlowCoordinator(this.createRoomFlowCoordinator, null, false);
				};
				this.createRoomFlowCoordinator.CreateRoomEvent += this.CreatedRoom;
			}
			base.PresentFlowCoordinator(this.createRoomFlowCoordinator, null, false, false);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00023198 File Offset: 0x00021398
		private void CreatedRoom(RoomSettings settings)
		{
			Client.Instance.CreatedRoomEvent -= this.CreatedRoomCallback;
			Client.Instance.CreatedRoomEvent += this.CreatedRoomCallback;
			GameplayModifiers gameplayModifiers = new GameplayModifiers();
			if (PluginUI.instance.playerData != null)
			{
				gameplayModifiers = PluginUI.instance.playerData.gameplayModifiers;
			}
			Client.Instance.CreateRoom(settings, gameplayModifiers);
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00023200 File Offset: 0x00021400
		private void CreatedRoomCallback(int code, string msg, CreateRoom createRoomData)
		{
			base.DismissFlowCoordinator(this.createRoomFlowCoordinator, null, true);
			Client.Instance.CreatedRoomEvent -= this.CreatedRoomCallback;
			if (code != 0)
			{
				this.DisplayError(msg);
				return;
			}
			BeatSaberMultiplayer.Data.RoomCfg roomCfg;
			BeatSaberMultiplayer.Data.SongCfg songCfg;
			Player[] array;
			ProtoHelper.FormatNetviosOutboundData(createRoomData.RoomId, createRoomData.RoomCfg, createRoomData.SongCfg, createRoomData.Players, out roomCfg, out songCfg, out array);
			BeatSaberMultiplayer.Data.Room room = new BeatSaberMultiplayer.Data.Room(createRoomData.RoomId, createRoomData.RoomOwner, createRoomData.RoomOwnerName, createRoomData.Status, roomCfg, array, songCfg);
			this.DisplayRoom();
			this.roomFlowCoordinator.JoinOwnRoom(room);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00023292 File Offset: 0x00021492
		private void SelectedRoom(BeatSaberMultiplayer.Data.Room room, string password)
		{
			Client.Instance.JoinRoomEvent -= this.JoinRoomCallback;
			Client.Instance.JoinRoomEvent += this.JoinRoomCallback;
			Client.Instance.JoinRoom(room.roomId, password);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x000232D4 File Offset: 0x000214D4
		private void JoinRoomCallback(int code, string msg, JoinRoom joinRoomData)
		{
			Client.Instance.JoinRoomEvent -= this.JoinRoomCallback;
			if (code != 0)
			{
				this.DisplayError(msg);
				return;
			}
			BeatSaberMultiplayer.Data.RoomCfg roomCfg;
			BeatSaberMultiplayer.Data.SongCfg songCfg;
			Player[] array;
			ProtoHelper.FormatNetviosOutboundData(joinRoomData.RoomId, joinRoomData.RoomCfg, joinRoomData.SongCfg, joinRoomData.Players, out roomCfg, out songCfg, out array);
			BeatSaberMultiplayer.Data.Room room = new BeatSaberMultiplayer.Data.Room(joinRoomData.RoomId, joinRoomData.RoomOwner, joinRoomData.RoomOwnerName, joinRoomData.Status, roomCfg, array, songCfg);
			this.DisplayRoom();
			this.roomFlowCoordinator.JoinRoom(room);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00023358 File Offset: 0x00021558
		private void FastMatchRoom()
		{
			Client.Instance.FastMatchEvent -= this.FastMatchRoomCallback;
			Client.Instance.FastMatchEvent += this.FastMatchRoomCallback;
			Client.Instance.FastMatch(null);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00023394 File Offset: 0x00021594
		private void FastMatchRoomCallback(int code, string msg, FastMatch fastMatchData)
		{
			Client.Instance.FastMatchEvent -= this.FastMatchRoomCallback;
			Client.Instance.CreatedRoomEvent -= this.CreatedRoomCallback;
			if (code != 0)
			{
				this.DisplayError(msg);
				return;
			}
			BeatSaberMultiplayer.Data.RoomCfg roomCfg;
			BeatSaberMultiplayer.Data.SongCfg songCfg;
			Player[] array;
			ProtoHelper.FormatNetviosOutboundData(fastMatchData.RoomId, fastMatchData.RoomCfg, fastMatchData.SongCfg, fastMatchData.Players, out roomCfg, out songCfg, out array);
			BeatSaberMultiplayer.Data.Room room = new BeatSaberMultiplayer.Data.Room(fastMatchData.RoomId, fastMatchData.RoomOwner, fastMatchData.RoomOwnerName, fastMatchData.Status, roomCfg, array, songCfg);
			this._roomsViewController.MatchSuccess();
			this.DisplayRoom();
			this.roomFlowCoordinator.JoinRoom(room);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00023439 File Offset: 0x00021639
		private void CancelMatchRoom()
		{
			Client.Instance.FastMatchEvent -= this.FastMatchRoomCallback;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00023454 File Offset: 0x00021654
		private void DisplayRoom()
		{
			if (this.roomFlowCoordinator == null)
			{
				this.roomFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<RoomFlowCoordinator>();
				this.roomFlowCoordinator.didFinishEvent += delegate
				{
					base.DismissFlowCoordinator(this.roomFlowCoordinator, null, false);
				};
			}
			base.PresentFlowCoordinator(this.roomFlowCoordinator, null, false, false);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000234A0 File Offset: 0x000216A0
		private void DisplayError(string msg)
		{
			SimpleDialogPromptViewController simpleDialog = Object.Instantiate<SimpleDialogPromptViewController>(PluginUI.simpleDialog, base.gameObject.transform);
			simpleDialog.Init("错误提示", msg, "知道了", delegate(int selectedButton)
			{
				this.InvokeMethod("DismissViewController", new object[]
				{
					simpleDialog,
					null,
					selectedButton == 0
				});
				this.RefreshRoomsPresed();
			});
			this.InvokeMethod("PresentViewController", new object[] { simpleDialog, null, false });
		}

		// Token: 0x04000410 RID: 1040
		public RoomFlowCoordinator roomFlowCoordinator;

		// Token: 0x04000411 RID: 1041
		public CreateRoomFlowCoordinator createRoomFlowCoordinator;

		// Token: 0x04000412 RID: 1042
		private RoomsViewController _roomsViewController;
	}
}
