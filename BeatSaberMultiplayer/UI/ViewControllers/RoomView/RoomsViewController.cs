using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.Helper;
using Com.Netvios.Proto.Outbound;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000061 RID: 97
	internal class RoomsViewController : BSMLResourceViewController
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0001C5DE File Offset: 0x0001A7DE
		public override string ResourceName
		{
			get
			{
				return string.Join(".", new string[]
				{
					base.GetType().Namespace,
					base.GetType().Name,
					"bsml"
				});
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000771 RID: 1905 RVA: 0x0001F19C File Offset: 0x0001D39C
		// (remove) Token: 0x06000772 RID: 1906 RVA: 0x0001F1D4 File Offset: 0x0001D3D4
		public event Action createRoomEvent;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000773 RID: 1907 RVA: 0x0001F20C File Offset: 0x0001D40C
		// (remove) Token: 0x06000774 RID: 1908 RVA: 0x0001F244 File Offset: 0x0001D444
		public event Action refreshRoomsEvent;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000775 RID: 1909 RVA: 0x0001F27C File Offset: 0x0001D47C
		// (remove) Token: 0x06000776 RID: 1910 RVA: 0x0001F2B4 File Offset: 0x0001D4B4
		public event Action<BeatSaberMultiplayer.Data.Room, string> selectedRoomEvent;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06000777 RID: 1911 RVA: 0x0001F2EC File Offset: 0x0001D4EC
		// (remove) Token: 0x06000778 RID: 1912 RVA: 0x0001F324 File Offset: 0x0001D524
		public event Action fastMatchEvent;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000779 RID: 1913 RVA: 0x0001F35C File Offset: 0x0001D55C
		// (remove) Token: 0x0600077A RID: 1914 RVA: 0x0001F394 File Offset: 0x0001D594
		public event Action cancelMatchEvent;

		// Token: 0x0600077B RID: 1915 RVA: 0x0001F3CC File Offset: 0x0001D5CC
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			this.btnContainer.gameObject.SetActive(true);
			this.loadingContainer.gameObject.SetActive(true);
			this.roomsContainer.gameObject.SetActive(false);
			this.matchContainer.gameObject.SetActive(false);
			this.SetTipText("数据加载中......");
			this._roomsList.tableView.ClearSelection();
			this.parserParams.EmitEvent("cancel");
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001F452 File Offset: 0x0001D652
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			this.parserParams.EmitEvent("closeAllMPModals");
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001F46B File Offset: 0x0001D66B
		public void SetTipText(string msg)
		{
			this._noRoomsText.text = msg;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001F479 File Offset: 0x0001D679
		public void CancelAutoMatch()
		{
			this.CancleMatchRoom();
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001F484 File Offset: 0x0001D684
		public void SetRooms(RoomList roomList)
		{
			List<BeatSaberMultiplayer.Data.Room> list = new List<BeatSaberMultiplayer.Data.Room>();
			foreach (Com.Netvios.Proto.Outbound.Room room in roomList.Rooms)
			{
				BeatSaberMultiplayer.Data.RoomCfg roomCfg = new BeatSaberMultiplayer.Data.RoomCfg(room.RoomId, room.RoomCfg.RoomName, room.RoomCfg.IsPrivate, room.RoomCfg.MaxPlayers, (float)room.RoomCfg.ResultDisplaySeconds);
				List<Player> list2 = new List<Player>();
				foreach (RoomPlayer roomPlayer in room.Players)
				{
					PlayerCfg playerCfg = new PlayerCfg(roomPlayer.PlayerId, roomPlayer.PersonalCfg.HeadphoneOn, roomPlayer.PersonalCfg.MicrophoneOn);
					Player player = new Player(roomPlayer.AppChannel, roomPlayer.PlayerId, roomPlayer.Nickname, roomPlayer.Avatar, roomPlayer.Score, roomPlayer.Status, playerCfg);
					list2.Add(player);
				}
				BeatSaberMultiplayer.Data.SongCfg songCfg = new BeatSaberMultiplayer.Data.SongCfg(room.SongCfg.SongId, room.SongCfg.SongName, room.SongCfg.Mode, room.SongCfg.Difficulty, room.SongCfg.SongCoverImg, room.SongCfg.SongDuration, room.SongCfg.Rules);
				BeatSaberMultiplayer.Data.Room room2 = new BeatSaberMultiplayer.Data.Room(room.RoomId, room.RoomOwner, room.RoomOwnerName, room.Status, roomCfg, list2.ToArray(), songCfg);
				list.Add(room2);
			}
			this._roomInfosList.Clear();
			if (list != null && list.Count != 0)
			{
				IEnumerable<BeatSaberMultiplayer.Data.Room> enumerable = list.OrderByDescending((BeatSaberMultiplayer.Data.Room x) => x.roomCfg.roomId);
				int num = 0;
				foreach (BeatSaberMultiplayer.Data.Room room3 in enumerable)
				{
					num++;
					this._roomInfosList.Add(new RoomsViewController.RoomListObject(room3, num));
				}
				this.roomsContainer.gameObject.SetActive(true);
				this.loadingContainer.gameObject.SetActive(false);
			}
			else
			{
				this.SetTipText("还没有任何房间，赶紧去创建吧");
				this.loadingContainer.gameObject.SetActive(true);
				this.roomsContainer.gameObject.SetActive(false);
			}
			this._roomsList.tableView.ReloadData();
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001F750 File Offset: 0x0001D950
		[UIAction("room-selected")]
		private void RoomSelected(TableView sender, RoomsViewController.RoomListObject obj)
		{
			if (obj.room.roomCfg.isPrivate)
			{
				this._selectedRoom = obj.room;
				this._passwordKeyboard.modalView.Show(true, false, null);
				return;
			}
			Action<BeatSaberMultiplayer.Data.Room, string> action = this.selectedRoomEvent;
			if (action == null)
			{
				return;
			}
			action(obj.room, null);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001F7A6 File Offset: 0x0001D9A6
		[UIAction("join-pressed")]
		private void PasswordEntered(string pass)
		{
			Action<BeatSaberMultiplayer.Data.Room, string> action = this.selectedRoomEvent;
			if (action == null)
			{
				return;
			}
			action(this._selectedRoom, pass);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001F7C0 File Offset: 0x0001D9C0
		[UIAction("match-room-btn-pressed")]
		private void FastMatchRoom()
		{
			Action action = this.fastMatchEvent;
			if (action != null)
			{
				action();
			}
			this.matchContainer.gameObject.SetActive(true);
			this.btnContainer.gameObject.SetActive(false);
			this.loadingContainer.gameObject.SetActive(false);
			this.roomsContainer.gameObject.SetActive(false);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001F824 File Offset: 0x0001DA24
		[UIAction("cancel-match-btn-pressed")]
		private void CancleMatchRoom()
		{
			Action action = this.cancelMatchEvent;
			if (action != null)
			{
				action();
			}
			this.btnContainer.gameObject.SetActive(true);
			this.roomsContainer.gameObject.SetActive(true);
			this.loadingContainer.gameObject.SetActive(false);
			this.matchContainer.gameObject.SetActive(false);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001F888 File Offset: 0x0001DA88
		public void MatchSuccess()
		{
			this.btnContainer.gameObject.SetActive(true);
			this.roomsContainer.gameObject.SetActive(true);
			this.loadingContainer.gameObject.SetActive(false);
			this.matchContainer.gameObject.SetActive(false);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001F8D9 File Offset: 0x0001DAD9
		[UIAction("create-room-btn-pressed")]
		private void CreateRoom()
		{
			Action action = this.createRoomEvent;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001F8EB File Offset: 0x0001DAEB
		[UIAction("search-room-btn-pressed")]
		private void SearchRoom()
		{
			Logger.log.Info("======== search room ========");
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001F8FC File Offset: 0x0001DAFC
		[UIAction("refresh-btn-pressed")]
		private void RefreshRoom()
		{
			this.SetTipText("数据加载中......");
			this.loadingContainer.gameObject.SetActive(true);
			this.roomsContainer.gameObject.SetActive(false);
			Action action = this.refreshRoomsEvent;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040003B6 RID: 950
		private float _refreshRoomInterval = 60f;

		// Token: 0x040003B7 RID: 951
		private float _refreshRoomTimer;

		// Token: 0x040003B8 RID: 952
		private BeatSaberMultiplayer.Data.Room _selectedRoom;

		// Token: 0x040003B9 RID: 953
		[UIParams]
		private BSMLParserParams parserParams;

		// Token: 0x040003BA RID: 954
		[UIComponent("rooms-container")]
		private RectTransform roomsContainer;

		// Token: 0x040003BB RID: 955
		[UIComponent("match-container")]
		private RectTransform matchContainer;

		// Token: 0x040003BC RID: 956
		[UIComponent("loading-container")]
		private RectTransform loadingContainer;

		// Token: 0x040003BD RID: 957
		[UIComponent("btn-container")]
		private RectTransform btnContainer;

		// Token: 0x040003BE RID: 958
		[UIComponent("no-rooms-text")]
		private TextMeshProUGUI _noRoomsText;

		// Token: 0x040003BF RID: 959
		[UIComponent("rooms-list")]
		private CustomCellListTableData _roomsList;

		// Token: 0x040003C0 RID: 960
		[UIValue("rooms")]
		private List<object> _roomInfosList = new List<object>();

		// Token: 0x040003C1 RID: 961
		[UIComponent("password-keyboard")]
		private ModalKeyboard _passwordKeyboard;

		// Token: 0x020000F1 RID: 241
		private class RoomListObject
		{
			// Token: 0x06000ACB RID: 2763 RVA: 0x00027F38 File Offset: 0x00026138
			[UIAction("refresh-visuals")]
			public void Refresh(bool selected, bool highlighted)
			{
				this._background.texture = Sprites.whitePixel.texture;
				this._background.color = new Color(1f, 1f, 1f, 0.125f);
				this._background.rectTransform.sizeDelta = this._bgSize;
			}

			// Token: 0x06000ACC RID: 2764 RVA: 0x00027F94 File Offset: 0x00026194
			[UIAction("#post-parse")]
			private void SetUp()
			{
				this._lockedIcon.texture = Sprites.lockedRoomIcon.texture;
				this._lockedIcon.enabled = this._locked;
				this._lockedIcon.transform.localScale = new Vector3(0.3f, 0.5f, 1f);
				this._roomStateText.color = new Color(0.65f, 0.65f, 0.65f, 1f);
			}

			// Token: 0x06000ACD RID: 2765 RVA: 0x00028010 File Offset: 0x00026210
			public RoomListObject(BeatSaberMultiplayer.Data.Room room, int roomNumber)
			{
				this.room = room;
				this._roomNumber = roomNumber.ToString();
				this._roomName = Utils.Truncate(room.roomCfg.roomName, 22);
				this._roomStateString = room.roomStatus.ToString();
				this._locked = room.roomCfg.isPrivate;
				this._songName = "(" + Utils.Truncate(room.songCfg.songName, 20) + ")";
				this._roomCounter = string.Concat(new string[]
				{
					"(",
					room.players.Length.ToString(),
					"/",
					room.roomCfg.maxPlayers.ToString(),
					")"
				});
			}

			// Token: 0x040005B7 RID: 1463
			public BeatSaberMultiplayer.Data.Room room;

			// Token: 0x040005B8 RID: 1464
			private Vector2 _bgSize = new Vector2(150f, 10f);

			// Token: 0x040005B9 RID: 1465
			[UIValue("room-number")]
			private string _roomNumber;

			// Token: 0x040005BA RID: 1466
			[UIValue("room-name")]
			private string _roomName;

			// Token: 0x040005BB RID: 1467
			[UIValue("song-name")]
			private string _songName;

			// Token: 0x040005BC RID: 1468
			[UIValue("room-counter")]
			private string _roomCounter;

			// Token: 0x040005BD RID: 1469
			[UIValue("room-state")]
			private string _roomStateString;

			// Token: 0x040005BE RID: 1470
			[UIComponent("locked-icon")]
			private RawImage _lockedIcon;

			// Token: 0x040005BF RID: 1471
			private bool _locked;

			// Token: 0x040005C0 RID: 1472
			[UIComponent("bg")]
			private RawImage _background;

			// Token: 0x040005C1 RID: 1473
			[UIComponent("room-state-text")]
			private TextMeshProUGUI _roomStateText;
		}
	}
}
