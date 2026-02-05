using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMultiplayer.Configuration;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.Helper;
using BeatSaberMultiplayer.Interop;
using BeatSaberMultiplayer.UI.ViewControllers.InGame;
using BeatSaberMultiplayer.UI.ViewControllers.RoomView;
using BeatSaberMultiplayer.VOIP;
using BS_Utils.Utilities;
using Com.Netvios.Proto;
using Com.Netvios.Proto.Outbound;
using HMUI;
using Newtonsoft.Json;
using PlayerDataPlugin;
using PlayerDataPlugin.BSHandler;
using SongCore;
using UnityEngine;

namespace BeatSaberMultiplayer.UI.FlowCoordinators
{
	// Token: 0x0200006B RID: 107
	internal class RoomFlowCoordinator : FlowCoordinator
	{
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x060007E7 RID: 2023 RVA: 0x000210EC File Offset: 0x0001F2EC
		// (remove) Token: 0x060007E8 RID: 2024 RVA: 0x00021124 File Offset: 0x0001F324
		public event Action didFinishEvent;

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00021159 File Offset: 0x0001F359
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x00021161 File Offset: 0x0001F361
		private SortMode LastSortMode
		{
			get
			{
				return this._lastSortMode;
			}
			set
			{
				if (this._lastSortMode == value)
				{
					return;
				}
				this._lastSortMode = value;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00021174 File Offset: 0x0001F374
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x0002117C File Offset: 0x0001F37C
		private string LastSearchRequest
		{
			get
			{
				return this._lastSearchRequest;
			}
			set
			{
				if (this._lastSearchRequest == value)
				{
					return;
				}
				this._lastSearchRequest = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00021194 File Offset: 0x0001F394
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0002119C File Offset: 0x0001F39C
		public float LastScrollPosition
		{
			get
			{
				return this._lastScrollPosition;
			}
			set
			{
				if (this._lastScrollPosition == value)
				{
					return;
				}
				this._lastScrollPosition = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x000211AF File Offset: 0x0001F3AF
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x000211B7 File Offset: 0x0001F3B7
		private IAnnotatedBeatmapLevelCollection LastSelectedCollection
		{
			get
			{
				return this._lastSelectedCollection;
			}
			set
			{
				if (this._lastSelectedCollection == value)
				{
					return;
				}
				this._lastSelectedCollection = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x000211CA File Offset: 0x0001F3CA
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x000211D2 File Offset: 0x0001F3D2
		private string LastSelectedSongId
		{
			get
			{
				return this._lastSelectedSongId;
			}
			set
			{
				if (this._lastSelectedSongId == value)
				{
					return;
				}
				this._lastSelectedSongId = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x000211EA File Offset: 0x0001F3EA
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x00021210 File Offset: 0x0001F410
		private SongPreviewPlayer PreviewPlayer
		{
			get
			{
				if (this._songPreviewPlayer == null)
				{
					this._songPreviewPlayer = Resources.FindObjectsOfTypeAll<SongPreviewPlayer>().FirstOrDefault<SongPreviewPlayer>();
				}
				return this._songPreviewPlayer;
			}
			set
			{
				this._songPreviewPlayer = value;
			}
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0002121C File Offset: 0x0001F41C
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			this._beatmapLevelsModel = Resources.FindObjectsOfTypeAll<BeatmapLevelsModel>().FirstOrDefault<BeatmapLevelsModel>();
			if (firstActivation && activationType == null)
			{
				base.title = "房间";
				this._playerManagerViewController = BeatSaberUI.CreateViewController<PlayerManagerViewController>();
				this._playerManagerViewController.gameplayModifiersChanged += this.GameplayModifierChanged;
				this._playerManagerViewController.changeRoomOwnerPressed += this.ChangeRoomOwner;
				this._playerManagerViewController.kickOutRoomPressedEvent += this.KickOutRoom;
				this._voipSettingViewController = BeatSaberUI.CreateViewController<VoipSettingViewController>();
				this._roomNavigationController = BeatSaberUI.CreateViewController<RoomNavigationController>();
				this._detailNavigationController = BeatSaberUI.CreateViewController<NavigationController>();
				PluginUI instance = PluginUI.instance;
				instance.didDownloadSongEvent = (Action)Delegate.Combine(instance.didDownloadSongEvent, new Action(this.RefreshSongs));
			}
			base.showBackButton = true;
			base.ProvideInitialViewControllers(this._roomNavigationController, this._playerManagerViewController, this._voipSettingViewController, null, null);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00021308 File Offset: 0x0001F508
		private void SwitchViewController(bool flg)
		{
			PlayerManagerViewController playerManagerViewController = this._playerManagerViewController;
			if (playerManagerViewController != null)
			{
				playerManagerViewController.gameObject.SetActive(flg);
			}
			VoipSettingViewController voipSettingViewController = this._voipSettingViewController;
			if (voipSettingViewController != null)
			{
				voipSettingViewController.gameObject.SetActive(flg);
			}
			if (Client.Instance.isHost)
			{
				LevelPacksViewController levelPacksViewController = this._levelPacksViewController;
				if (levelPacksViewController == null)
				{
					return;
				}
				levelPacksViewController.gameObject.SetActive(flg);
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00021368 File Offset: 0x0001F568
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			if (topViewController == this._roomNavigationController)
			{
				if (this._multiResultsViewController != null)
				{
					return;
				}
				this.SwitchViewController(false);
				SimpleDialogPromptViewController simpleDialog = Object.Instantiate<SimpleDialogPromptViewController>(PluginUI.simpleDialog, base.gameObject.transform);
				simpleDialog.Init("提示", "确定退出房间么？", "确定", "取消", delegate(int selectedButton)
				{
					this.InvokeMethod("DismissViewController", new object[]
					{
						simpleDialog,
						null,
						selectedButton == 0
					});
					if (selectedButton == 0)
					{
						this.LeaveRoom();
						Action action = this.didFinishEvent;
						if (action != null)
						{
							action();
						}
					}
					this.SwitchViewController(true);
					this.simpleDialogIsShow = false;
					this.simpleDialogTemp = null;
				});
				this.InvokeMethod("PresentViewController", new object[] { simpleDialog, null, false });
				this.simpleDialogIsShow = true;
				this.simpleDialogTemp = simpleDialog;
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00021428 File Offset: 0x0001F628
		private void ShowError(int code, string errMsg)
		{
			if (code == 0)
			{
				return;
			}
			if (string.IsNullOrEmpty(errMsg))
			{
				errMsg = "未知错误";
			}
			SimpleDialogPromptViewController simpleDialog = Object.Instantiate<SimpleDialogPromptViewController>(PluginUI.simpleDialog, base.gameObject.transform);
			simpleDialog.Init("提示", errMsg, "知道了", delegate(int selectedButton)
			{
				this.InvokeMethod("DismissViewController", new object[]
				{
					simpleDialog,
					null,
					selectedButton == 0
				});
				if (code == Client.Instance.TcpReceiveMessageErrorCode)
				{
					this.LeaveRoom();
					Action action = this.didFinishEvent;
					if (action != null)
					{
						action();
					}
				}
				this.simpleDialogIsShow = false;
				this.simpleDialogTemp = null;
			});
			this.InvokeMethod("PresentViewController", new object[] { simpleDialog, null, false });
			this.simpleDialogIsShow = true;
			this.simpleDialogTemp = simpleDialog;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000214D8 File Offset: 0x0001F6D8
		private void SetSongs(IAnnotatedBeatmapLevelCollection selectedCollection, SortMode sortMode, string searchRequest)
		{
			this.LastSortMode = sortMode;
			this.LastSearchRequest = searchRequest;
			this.LastSelectedCollection = selectedCollection;
			List<IPreviewBeatmapLevel> list = new List<IPreviewBeatmapLevel>();
			if (this.LastSelectedCollection != null)
			{
				list = this.LastSelectedCollection.beatmapLevelCollection.beatmapLevels.ToList<IPreviewBeatmapLevel>();
			}
			this._levelSelectViewController.SetSongs(list);
			this._levelPacksViewController.SetSelectedPack(this.LastSelectedCollection);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002153C File Offset: 0x0001F73C
		private void RefreshSongs()
		{
			List<IPreviewBeatmapLevel> list = new List<IPreviewBeatmapLevel>();
			if (this.LastSelectedCollection != null)
			{
				list = this.LastSelectedCollection.beatmapLevelCollection.beatmapLevels.ToList<IPreviewBeatmapLevel>();
			}
			this._levelSelectViewController.SetSongs(list);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002157C File Offset: 0x0001F77C
		private void AddRoomListener()
		{
			Client.Instance.OccurredErrorEvent -= this.ShowError;
			Client.Instance.OccurredErrorEvent += this.ShowError;
			Client.Instance.ChangeRoomOwnerEvent -= this.ChangeRoomOwnerCallback;
			Client.Instance.ChangeRoomOwnerEvent += this.ChangeRoomOwnerCallback;
			Client.Instance.RoomUpdatedNoticeEvent -= this.RoomUpdatedNoticeCallback;
			Client.Instance.RoomUpdatedNoticeEvent += this.RoomUpdatedNoticeCallback;
			Client.Instance.KickedOutRoomNoticeEvent -= this.KickedOutRoomNoticeCallback;
			Client.Instance.KickedOutRoomNoticeEvent += this.KickedOutRoomNoticeCallback;
			Client.Instance.StartGameNoticeEvent -= this.StartGameCallback;
			Client.Instance.StartGameNoticeEvent += this.StartGameCallback;
			Client.Instance.RoomBroadcastEvent -= this.RoomBroadcastCallback;
			Client.Instance.RoomBroadcastEvent += this.RoomBroadcastCallback;
			Client.Instance.RoomBroadcastNoticeEvent -= this.RoomBroadcastNoticeCallback;
			Client.Instance.RoomBroadcastNoticeEvent += this.RoomBroadcastNoticeCallback;
			Client.Instance.RoomSubmitScoreNoticeEvent -= this.UpdateResultScoreUI;
			Client.Instance.RoomSubmitScoreNoticeEvent += this.UpdateResultScoreUI;
			Client.Instance.GetRoomEvent -= this.GetRoomCallback;
			Client.Instance.GetRoomEvent += this.GetRoomCallback;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00021718 File Offset: 0x0001F918
		private void RemoveRoomListener()
		{
			Client.Instance.OccurredErrorEvent -= this.ShowError;
			Client.Instance.RoomUpdatedNoticeEvent -= this.RoomUpdatedNoticeCallback;
			Client.Instance.ChangeRoomOwnerEvent -= this.ChangeRoomOwnerCallback;
			Client.Instance.KickedOutRoomNoticeEvent -= this.KickedOutRoomNoticeCallback;
			Client.Instance.StartGameNoticeEvent -= this.StartGameCallback;
			Client.Instance.RoomBroadcastNoticeEvent -= this.RoomBroadcastNoticeCallback;
			Client.Instance.RoomSubmitScoreNoticeEvent -= this.UpdateResultScoreUI;
			Client.Instance.GetRoomEvent -= this.GetRoomCallback;
			Client.Instance.RoomBroadcastEvent -= this.RoomBroadcastCallback;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x000217EC File Offset: 0x0001F9EC
		public void JoinRoom(BeatSaberMultiplayer.Data.Room room)
		{
			if (Client.Instance.isHost)
			{
				this.JoinOwnRoom(room);
				return;
			}
			this.AddRoomListener();
			this._room = room;
			this.LevelIdToLastSelectedOptions(this._room.songCfg.songId, delegate(bool completedFlg)
			{
				if (completedFlg)
				{
					this.UpdateRoomTitleAndPlayersAndModifiers(true);
					this.ShowSongsList();
					return;
				}
				Logger.log.Error("[LevelIdToLastSelectedOptions] join room error");
				this.ShowError(1, "加入房间失败");
			});
			InGameController instance = InGameController.instance;
			if (instance == null)
			{
				return;
			}
			instance.VoiceChatStartRecording();
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002184C File Offset: 0x0001FA4C
		public void JoinOwnRoom(BeatSaberMultiplayer.Data.Room room)
		{
			if (!Client.Instance.isHost)
			{
				this.JoinRoom(room);
				return;
			}
			this.AddRoomListener();
			this._room = room;
			if (!string.IsNullOrEmpty(this._room.songCfg.songId))
			{
				this.LevelIdToLastSelectedOptions(this._room.songCfg.songId, delegate(bool completedFlg)
				{
					if (completedFlg)
					{
						this.UpdateRoomTitleAndPlayersAndModifiers(true);
						this.ShowSongsList();
						return;
					}
					Logger.log.Error("[LevelIdToLastSelectedOptions] join own room error");
					this.ShowError(1, "加入房间失败");
				});
			}
			else
			{
				this.UpdateRoomTitleAndPlayersAndModifiers(true);
				this.ShowSongsList();
			}
			if (this._levelDetailViewController != null)
			{
				this._levelDetailViewController.StartActionInteractable = true;
			}
			InGameController instance = InGameController.instance;
			if (instance == null)
			{
				return;
			}
			instance.VoiceChatStartRecording();
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000218EC File Offset: 0x0001FAEC
		private async void ChangeRoomOwnerCallback(long roomOwner, string roomOwnerName, string roomStatus)
		{
			await this.SeekAndSetupLevel();
			this.ShowSongsList();
			this._room.roomOwner = roomOwner;
			this._room.roomOwnerName = roomOwnerName;
			this._room.SetRoomStatus(roomStatus);
			this.UpdatePlayers();
			this._playerManagerViewController.SetGameplayModifiers(this._room.songCfg.ToGameplayModifiers());
			this.SwitchPlayerManagerButtonAndSelectedButton();
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002193C File Offset: 0x0001FB3C
		private void GetRoomCallback(int code, string msg, GetRoom getRoomData)
		{
			BeatSaberMultiplayer.Data.Room.RoomStatus roomStatus = this._room.roomStatus;
			BeatSaberMultiplayer.Data.Room.RoomStatus roomStatus2 = (BeatSaberMultiplayer.Data.Room.RoomStatus)Enum.Parse(typeof(BeatSaberMultiplayer.Data.Room.RoomStatus), getRoomData.Status, true);
			if (roomStatus != roomStatus2)
			{
				this._room.SetRoomStatus(getRoomData.Status);
				this.SwitchPlayerManagerButtonAndSelectedButton();
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002198C File Offset: 0x0001FB8C
		private void RoomUpdatedNoticeCallback(int code, string msg, RoomUpdatedNotice roomUpdatedNoticeData)
		{
			if (code != 0)
			{
				return;
			}
			this._room.SetRoomStatus(roomUpdatedNoticeData.Status);
			switch (roomUpdatedNoticeData.EventType)
			{
			case DataType.JoinRoom:
			{
				Player[] array;
				ProtoHelper.FormatNetviosOutboundData(roomUpdatedNoticeData.Players, out array);
				this._room.players = array;
				this.UpdatePlayers();
				if (Client.Instance.isHost)
				{
					this._levelDetailViewController.StartActionInteractable = false;
					return;
				}
				return;
			}
			case DataType.ExitRoom:
			case DataType.KickOutRoomPlayer:
			{
				Player[] array2;
				ProtoHelper.FormatNetviosOutboundData(roomUpdatedNoticeData.Players, out array2);
				foreach (Player player in this._room.players)
				{
					bool flag = false;
					foreach (Player player2 in array2)
					{
						if (player.playerId == player2.playerId)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						if (this._downloadLevelCompletedCounter.ContainsKey(player.playerId))
						{
							this._downloadLevelCompletedCounter.Remove(player.playerId);
						}
						if (InGameController.instance.isPlaying)
						{
							this._exitPlayersWhenPlaying.Add(player);
						}
					}
				}
				if (roomUpdatedNoticeData.EventType == DataType.ExitRoom)
				{
					this._room.roomOwner = roomUpdatedNoticeData.RoomOwner;
					this._room.roomOwnerName = roomUpdatedNoticeData.RoomOwnerName;
				}
				this._room.players = array2;
				this.UpdatePlayers();
				this.SwitchPlayerManagerButtonAndSelectedButton();
				return;
			}
			case DataType.ChangeRoomOwner:
			{
				Debug.Log("====== Com.Netvios.Proto.DataType.ChangeRoomOwner =======");
				Player[] array4;
				ProtoHelper.FormatNetviosOutboundData(roomUpdatedNoticeData.Players, out array4);
				this._room.players = array4;
				this._room.roomOwner = roomUpdatedNoticeData.RoomOwner;
				this.UpdatePlayers();
				this.SwitchPlayerManagerButtonAndSelectedButton();
				if (!Client.Instance.isHost)
				{
					this.UpdateRoomTitleAndPlayersAndModifiers(false);
					this.LevelIdToLastSelectedOptions(this._room.songCfg.songId, delegate(bool complatedFlag)
					{
						if (complatedFlag)
						{
							this._levelSelectViewController.SetSongs(this._levels);
							this.UpdateLevelDetailOptions();
							return;
						}
						Logger.log.Error("[RoomUpdatedNoticeCallback] [ModifySongCfg] LevelIdToLastSelectedOptions error");
						this.ShowError(1, "LevelIdToLastSelectedOptions error");
					});
					return;
				}
				return;
			}
			case DataType.ModifyPersonalCfg:
				return;
			case DataType.ModifyRoomCfg:
				if (!Client.Instance.isHost)
				{
					BeatSaberMultiplayer.Data.RoomCfg roomCfg;
					ProtoHelper.FormatNetviosOutboundData(roomUpdatedNoticeData.RoomId, roomUpdatedNoticeData.RoomCfg, out roomCfg);
					this._room.roomCfg = roomCfg;
					this.UpdateRoomTitleAndPlayersAndModifiers(false);
					return;
				}
				return;
			case DataType.ModifySongCfg:
			{
				Debug.Log("======  Com.Netvios.Proto.DataType.ModifySongCfg =====");
				BeatSaberMultiplayer.Data.SongCfg songCfg;
				ProtoHelper.FormatNetviosOutboundData(roomUpdatedNoticeData.SongCfg, out songCfg);
				string songId = this._room.songCfg.songId;
				this._room.songCfg = songCfg;
				if (!Client.Instance.isHost)
				{
					this.UpdateRoomTitleAndPlayersAndModifiers(false);
					this.LevelIdToLastSelectedOptions(this._room.songCfg.songId, delegate(bool complatedFlag)
					{
						if (complatedFlag)
						{
							this._levelSelectViewController.SetSongs(this._levels);
							this.UpdateLevelDetailOptions();
							return;
						}
						Logger.log.Error("[RoomUpdatedNoticeCallback] [ModifySongCfg] LevelIdToLastSelectedOptions error");
						this.ShowError(1, "LevelIdToLastSelectedOptions error");
					});
					return;
				}
				if (songId != this._room.songCfg.songId)
				{
					this._downloadLevelCompletedCounter.Clear();
				}
				RoomBroadcastDataUpdateProgressStruct roomBroadcastDataUpdateProgressStruct = new RoomBroadcastDataUpdateProgressStruct(Client.Instance.player.playerId, 1f);
				Client.Instance.RoomBroadcast(RoomBroadcastDataType.DownloadLevel, JsonConvert.SerializeObject(roomBroadcastDataUpdateProgressStruct));
				return;
			}
			}
			BeatSaberMultiplayer.Data.RoomCfg roomCfg2;
			BeatSaberMultiplayer.Data.SongCfg songCfg2;
			Player[] array5;
			ProtoHelper.FormatNetviosOutboundData(roomUpdatedNoticeData.RoomId, roomUpdatedNoticeData.RoomCfg, roomUpdatedNoticeData.SongCfg, roomUpdatedNoticeData.Players, out roomCfg2, out songCfg2, out array5);
			this._room.roomOwner = roomUpdatedNoticeData.RoomOwner;
			this._room.roomOwnerName = roomUpdatedNoticeData.RoomOwnerName;
			this._room.SetRoomStatus(roomUpdatedNoticeData.Status);
			this._room.players = array5;
			if (!Client.Instance.isHost)
			{
				this._room.roomCfg = roomCfg2;
				this._room.songCfg = songCfg2;
				this.UpdateRoomTitleAndPlayersAndModifiers(false);
				this.LevelIdToLastSelectedOptions(this._room.songCfg.songId, delegate(bool complatedFlag)
				{
					if (complatedFlag)
					{
						this._levelSelectViewController.SetSongs(this._levels);
						return;
					}
					Logger.log.Error("[RoomUpdatedNoticeCallback] [default] LevelIdToLastSelectedOptions error");
					this.ShowError(1, "LevelIdToLastSelectedOptions error");
				});
			}
			this.UpdatePlayers();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00021D40 File Offset: 0x0001FF40
		public void LeaveRoom()
		{
			this.RemoveRoomListener();
			Client.Instance.LeaveRoom();
			this._room = null;
			this._downloadLevelCompletedCounter.Clear();
			this.PreviewPlayer.CrossfadeToDefault();
			this.PopAllViewControllers();
			InGameController instance = InGameController.instance;
			if (instance == null)
			{
				return;
			}
			instance.VoiceChatStopRecording();
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00021D90 File Offset: 0x0001FF90
		private async void UpdateLevelDetailOptions()
		{
			if (this._levelDetailViewController == null)
			{
				this._levelDetailViewController = BeatSaberUI.CreateViewController<LevelDetailViewController>();
				this._levelDetailViewController.SongDetailSelectedEvent += this.SongDetailSelected;
				this._levelDetailViewController.StartGameEvent += this.StartGame;
			}
			this._detailNavigationController.ClearChildViewControllers();
			base.SetViewControllerToNavigationController(this._detailNavigationController, this._levelDetailViewController);
			CancellationToken cancellationToken = default(CancellationToken);
			BeatmapLevelsModel.GetBeatmapLevelResult getBeatmapLevelResult = await this._beatmapLevelsModel.GetBeatmapLevelAsync(this._lastSelectedSong.levelID, cancellationToken);
			this._levelDetailViewController.SetContent(this._lastSelectedSong, this._lastSelectedSongCoverImg, getBeatmapLevelResult.beatmapLevel, this._room.songCfg.mode, this._room.songCfg.difficulty);
			IBeatmapLevel beatmapLevel = getBeatmapLevelResult.beatmapLevel;
			this.PreviewPlayer.CrossfadeTo(beatmapLevel.beatmapLevelData.audioClip, beatmapLevel.previewStartTime, beatmapLevel.beatmapLevelData.audioClip.length - beatmapLevel.previewStartTime, 1f);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00021DC7 File Offset: 0x0001FFC7
		private void HideSongsList()
		{
			this._roomNavigationController.ClearChildViewControllers();
			base.SetBottomScreenViewController(null, false);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00021DDC File Offset: 0x0001FFDC
		private void SongSelected(IPreviewBeatmapLevel song, Texture2D coverImg)
		{
			this.LastSelectedSongId = song.levelID;
			this._lastSelectedSong = song;
			this._lastSelectedSongCoverImg = coverImg;
			this.UpdateLevelDetailOptions();
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00021E00 File Offset: 0x00020000
		private async void RandomLevelSelected()
		{
			IBeatmapLevelPack[] beatmapLevelPacks = this._beatmapLevelsModel.ostAndExtrasPackCollection.beatmapLevelPacks;
			List<IBeatmapLevelPack> list = new List<IBeatmapLevelPack>();
			foreach (IBeatmapLevelPack beatmapLevelPack in beatmapLevelPacks)
			{
				if (beatmapLevelPack.beatmapLevelCollection.beatmapLevels.Length != 0)
				{
					list.Add(beatmapLevelPack);
				}
			}
			IBeatmapLevelPack[] array2 = list.ToArray();
			Random random = new Random();
			int num = random.Next(0, array2.Length);
			IBeatmapLevelPack pack = array2[num];
			this.LastSelectedCollection = pack;
			this.LastSortMode = SortMode.Default;
			this.LastSearchRequest = "";
			this.SetSongs(this.LastSelectedCollection, this.LastSortMode, this.LastSearchRequest);
			int num2 = random.Next(0, pack.beatmapLevelCollection.beatmapLevels.Length);
			IPreviewBeatmapLevel level = pack.beatmapLevelCollection.beatmapLevels[num2];
			this.LastSelectedSongId = level.levelID;
			Texture2D texture2D = await level.GetCoverImageTexture2DAsync(default(CancellationToken));
			this._lastSelectedSongCoverImg = texture2D;
			this._lastSelectedCollection = pack;
			this.SongSelected(level, this._lastSelectedSongCoverImg);
			this._levelSelectViewController.ScrollToLevel(this.LastSelectedSongId);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00021E38 File Offset: 0x00020038
		private void SongDetailSelected(string selectedCharacteristicStr, string selectedDifficultyStr)
		{
			if (!Client.Instance.isHost)
			{
				return;
			}
			this._selectedCharacteristicStr = selectedCharacteristicStr;
			this._selectedDifficultyStr = selectedDifficultyStr;
			string text = JsonConvert.SerializeObject(new GameplayModifiersStruct(this._playerManagerViewController.modifiersPanel.gameplayModifiers));
			string text2 = this.LastSelectedSongId.Replace("custom_level_", "");
			Logger.log.Info("==== _room songId: " + this._room.songCfg.songId);
			Logger.log.Info("==== newLevelId: " + text2);
			Logger.log.Info("==== _room mode: " + this._room.songCfg.mode);
			Logger.log.Info("==== _selectedCharacteristicStr: " + this._selectedCharacteristicStr);
			Logger.log.Info("==== _room difficulty: " + this._room.songCfg.difficulty);
			Logger.log.Info("==== _selectedDifficultyStr: " + this._selectedDifficultyStr);
			Logger.log.Info("==== _room.songCfg.rules: " + this._room.songCfg.rules);
			Logger.log.Info("==== rules: " + text);
			Logger.log.Info(string.Format("==== _downloadLevelCompletedCounter.Count: {0}", this._downloadLevelCompletedCounter.Count));
			Logger.log.Info(string.Format("==== _room.players.Length: {0}", this._room.players.Length));
			if ((this._room.players.Length == 1 || this._downloadLevelCompletedCounter.Count == this._room.players.Length) && text2 == this._room.songCfg.songId && this._selectedCharacteristicStr == this._room.songCfg.mode && this._selectedDifficultyStr == this._room.songCfg.difficulty && this._room.songCfg.rules == text)
			{
				Debug.Log("=== [ModifySongCfg] no change ===");
				this._levelDetailViewController.SelectActionInteractable = false;
				this._levelDetailViewController.StartActionInteractable = true;
				return;
			}
			if (text2 != this._room.songCfg.songId)
			{
				foreach (Player player in this._room.players)
				{
					if (player.playerId != this._room.roomOwner)
					{
						this._playerManagerViewController.UpdateDownloadProgress(player.playerId, 0f);
					}
				}
			}
			this._downloadLevelCompletedCounter.Clear();
			Client.Instance.ModifySongCfg(this._room.roomId, text2, this._selectedCharacteristicStr, this._selectedDifficultyStr, text);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00022118 File Offset: 0x00020318
		public void ShowResult(LevelCompletionResults levelCompletionResults)
		{
			LevelPacksViewController levelPacksViewController = this._levelPacksViewController;
			if (levelPacksViewController != null)
			{
				levelPacksViewController.gameObject.SetActive(false);
			}
			Singleton<GameEventController>.Instance.SetLevelCompletionResultsAndDifficultyBeatmap(levelCompletionResults, this._levelDetailViewController.difficultyBeatmap);
			this._roomNavigationController.ClearChildViewControllers();
			if (this._multiResultsViewController == null)
			{
				this._multiResultsViewController = BeatSaberUI.CreateViewController<MultiResultsViewController>();
			}
			base.SetViewControllerToNavigationController(this._roomNavigationController, this._multiResultsViewController);
			Player[] array = new Player[this._room.players.Length + this._exitPlayersWhenPlaying.Count];
			this._room.players.CopyTo(array, 0);
			this._exitPlayersWhenPlaying.CopyTo(array, this._room.players.Length);
			foreach (Player player in array)
			{
				this._multiResultsViewController.SetResults(player.playerId, player.nickname, player.score);
			}
			base.StartCoroutine(this.HideResult());
			this._exitPlayersWhenPlaying.Clear();
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002221B File Offset: 0x0002041B
		private IEnumerator HideResult()
		{
			this._resultTotalTime = this._room.roomCfg.resultsShowTime;
			while (this._resultTotalTime >= 0f)
			{
				this._multiResultsViewController.TimeText = this._resultTotalTime.ToString();
				yield return new WaitForSeconds(1f);
				this._resultTotalTime -= 1f;
			}
			LevelPacksViewController levelPacksViewController = this._levelPacksViewController;
			if (levelPacksViewController != null)
			{
				levelPacksViewController.gameObject.SetActive(true);
			}
			Object.Destroy(this._multiResultsViewController);
			this._multiResultsViewController = null;
			this.ShowSongsList();
			this.SwitchPlayerManagerButtonAndSelectedButton();
			yield break;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002222C File Offset: 0x0002042C
		private void UpdateResultScoreUI(int code, string msg, RoomSubmitScoreNotice roomSubmitScoreNoticeData)
		{
			Client.Instance.GetRoom();
			if (code == 0)
			{
				if (this._multiResultsViewController != null)
				{
					string text = null;
					foreach (Player player in this._room.players)
					{
						if (player.playerId == roomSubmitScoreNoticeData.PlayerId)
						{
							text = player.nickname;
						}
					}
					if (text != null)
					{
						this._multiResultsViewController.SetResults(roomSubmitScoreNoticeData.PlayerId, text, roomSubmitScoreNoticeData.Score);
						return;
					}
				}
			}
			else
			{
				Logger.log.Error(string.Format("UpdateResultScoreUI error,code: {0}, msg: {1}", code, msg));
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000222C0 File Offset: 0x000204C0
		private void ShowLevelSelectViewController()
		{
			if (this._levelSelectViewController == null)
			{
				this._levelSelectViewController = BeatSaberUI.CreateViewController<LevelSelectViewController>();
				this._levelSelectViewController.ParentFlowCoordinator = this;
				this._levelSelectViewController.SongSelectedEvent += this.SongSelected;
				this._levelSelectViewController.RandomLevelPressedEvent += this.RandomLevelSelected;
				this._levelSelectViewController.SortPressedEvent += delegate(SortMode sortMode)
				{
					this.SetSongs(this.LastSelectedCollection, sortMode, this.LastSearchRequest);
				};
				this._levelSelectViewController.SearchPressedEvent += delegate(string value)
				{
					this.SetSongs(this.LastSelectedCollection, this.LastSortMode, value);
				};
			}
			if (this._roomNavigationController.viewControllers.IndexOf(this._levelSelectViewController) < 0)
			{
				base.SetViewControllerToNavigationController(this._roomNavigationController, this._levelSelectViewController);
			}
			if (this._roomNavigationController.viewControllers.IndexOf(this._detailNavigationController) < 0)
			{
				base.PushViewControllerToNavigationController(this._roomNavigationController, this._detailNavigationController, null, false);
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000223A8 File Offset: 0x000205A8
		private void ShowLevelPackImageViewController()
		{
			if (this._levelPackImageViewController == null)
			{
				this._levelPackImageViewController = BeatSaberUI.CreateViewController<LevelPackImageViewController>();
			}
			if (this._detailNavigationController.viewControllers.IndexOf(this._levelPackImageViewController) < 0)
			{
				this._detailNavigationController.ClearChildViewControllers();
				base.SetViewControllerToNavigationController(this._detailNavigationController, this._levelPackImageViewController);
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00022404 File Offset: 0x00020604
		private void ShowLevelPacksViewController()
		{
			if (this._levelPacksViewController == null)
			{
				this._levelPacksViewController = BeatSaberUI.CreateViewController<LevelPacksViewController>();
				this._levelPacksViewController.packSelected += delegate(IAnnotatedBeatmapLevelCollection pack)
				{
					float lastScrollPosition = this.LastScrollPosition;
					if (this.LastSelectedCollection != pack)
					{
						this.LastSelectedCollection = pack;
						this.LastSortMode = SortMode.Default;
						this.LastSearchRequest = "";
					}
					this.SetSongs(this.LastSelectedCollection, this.LastSortMode, this.LastSearchRequest);
					if (!this._levelSelectViewController.ScrollToPosition(lastScrollPosition))
					{
						Logger.log.Debug(string.Format("Couldn't scroll to {0}, max is {1}", lastScrollPosition, this._levelSelectViewController.SongListScroller.scrollableSize));
						this._levelSelectViewController.ScrollToLevel(this.LastSelectedSongId);
					}
					this._detailNavigationController.ClearChildViewControllers();
					base.SetViewControllersToNavigationController(this._detailNavigationController, new ViewController[] { this._levelPackImageViewController });
					this._levelPackImageViewController.SetPackImage(pack);
				};
			}
			this._levelPacksViewController.gameObject.SetActive(true);
			base.SetBottomScreenViewController(this._levelPacksViewController, false);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00022460 File Offset: 0x00020660
		private void ShowSongsList()
		{
			if (this._multiResultsViewController != null)
			{
				return;
			}
			if (InGameController.instance.isPlaying)
			{
				return;
			}
			this.ShowLevelSelectViewController();
			if (Client.Instance.isHost)
			{
				this.ShowLevelPackImageViewController();
				this.ShowLevelPacksViewController();
				this.SetSongs(this.LastSelectedCollection, SortMode.Default, "");
			}
			else
			{
				this._levelSelectViewController.SetSongs(this._levels);
				LevelPacksViewController levelPacksViewController = this._levelPacksViewController;
				if (levelPacksViewController != null)
				{
					levelPacksViewController.gameObject.SetActive(false);
				}
			}
			this._levelSelectViewController.ScrollToLevel(this.LastSelectedSongId);
			this.UpdateLevelDetailOptions();
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000224FC File Offset: 0x000206FC
		private void UpdateRoomTitleAndPlayersAndModifiers(bool needUpdatePlayers = true)
		{
			base.title = this._room.roomCfg.roomName;
			GameplayModifiers gameplayModifiers = this._room.songCfg.ToGameplayModifiers();
			this._playerManagerViewController.SetGameplayModifiers(gameplayModifiers);
			if (needUpdatePlayers)
			{
				this.UpdatePlayers();
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00022545 File Offset: 0x00020745
		private void UpdatePlayers()
		{
			this._playerManagerViewController.SetPlayers(this._room.players, this._room.roomOwner, false);
			InGameController instance = InGameController.instance;
			if (instance == null)
			{
				return;
			}
			instance.SetPlayerControllers(this._room.players, false);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00022584 File Offset: 0x00020784
		private void PopAllViewControllers()
		{
			if (base.childFlowCoordinator != null)
			{
				if (base.childFlowCoordinator is IDismissable)
				{
					(base.childFlowCoordinator as IDismissable).Dismiss(true);
				}
				else
				{
					base.DismissFlowCoordinator(base.childFlowCoordinator, null, true);
				}
			}
			if (this._roomNavigationController.viewControllers.Contains(this._levelSelectViewController))
			{
				this.HideSongsList();
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000225EC File Offset: 0x000207EC
		private void GameplayModifierChanged()
		{
			if (!Client.Instance.isHost)
			{
				return;
			}
			string text = JsonConvert.SerializeObject(new GameplayModifiersStruct(this._playerManagerViewController.modifiersPanel.gameplayModifiers));
			this._levelDetailViewController.SelectActionInteractable = true;
			this._downloadLevelCompletedCounter.Clear();
			Client.Instance.ModifySongCfg(this._room.roomId, this._room.songCfg.songId, this._room.songCfg.mode, this._room.songCfg.difficulty, text);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00022684 File Offset: 0x00020884
		private async void LevelIdToLastSelectedOptions(string songId, Action<bool> CompletedCallback)
		{
			this.LastSelectedSongId = this.SongIdConvertToLocalFormat(songId);
			TaskAwaiter<bool> taskAwaiter = this.SeekAndSetupLevel().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				Action<bool> completedCallback = CompletedCallback;
				if (completedCallback != null)
				{
					completedCallback(true);
				}
				RoomBroadcastDataUpdateProgressStruct roomBroadcastDataUpdateProgressStruct = new RoomBroadcastDataUpdateProgressStruct(Client.Instance.player.playerId, 1f);
				Client.Instance.RoomBroadcast(RoomBroadcastDataType.DownloadLevel, JsonConvert.SerializeObject(roomBroadcastDataUpdateProgressStruct));
			}
			else
			{
				string text = this.LastSelectedSongId.Replace("custom_level_", "");
				Action<bool> <>9__1;
				Action<float> <>9__2;
				SongDownloader.Instance.RequestSongByLevelID(text, delegate(BeatSaberMultiplayer.Helper.Song song)
				{
					if (song == null)
					{
						Logger.log.Error("Unable to download song! search result: song is null");
						this.ShowError(1, "歌曲下载失败");
						return;
					}
					SongDownloader instance = SongDownloader.Instance;
					Action<bool> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(bool success)
						{
							if (success)
							{
								Loader.SongsLoadedEvent -= base.<LevelIdToLastSelectedOptions>g__onLoaded|3;
								Loader.SongsLoadedEvent += base.<LevelIdToLastSelectedOptions>g__onLoaded|3;
								Loader.Instance.RefreshSongs(false);
								return;
							}
							Action<bool> completedCallback2 = CompletedCallback;
							if (completedCallback2 != null)
							{
								completedCallback2(false);
							}
							Logger.log.Error("Unable to download song! An error occurred");
							this.ShowError(1, "歌曲下载失败");
						});
					}
					Action<float> action2;
					if ((action2 = <>9__2) == null)
					{
						action2 = (<>9__2 = delegate(float progress)
						{
							PlayerManagerViewController playerManagerViewController = this._playerManagerViewController;
							if (playerManagerViewController != null)
							{
								playerManagerViewController.UpdateDownloadProgress(Client.Instance.player.playerId, progress);
							}
							RoomBroadcastDataUpdateProgressStruct roomBroadcastDataUpdateProgressStruct2 = new RoomBroadcastDataUpdateProgressStruct(Client.Instance.player.playerId, progress);
							Client.Instance.RoomBroadcast(RoomBroadcastDataType.DownloadLevel, JsonConvert.SerializeObject(roomBroadcastDataUpdateProgressStruct2));
						});
					}
					instance.DownloadSong(song, action, action2);
				});
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000226CC File Offset: 0x000208CC
		private async Task<bool> SeekAndSetupLevel()
		{
			IBeatmapLevelPack[] beatmapLevelPacks = this._beatmapLevelsModel.ostAndExtrasPackCollection.beatmapLevelPacks;
			this._levels.Clear();
			foreach (IBeatmapLevelPack pack in beatmapLevelPacks)
			{
				foreach (IPreviewBeatmapLevel previewBeatmapLevel in pack.beatmapLevelCollection.beatmapLevels)
				{
					if (previewBeatmapLevel.levelID.Contains(this.LastSelectedSongId))
					{
						CancellationToken cancellationToken = default(CancellationToken);
						BeatmapLevelsModel.GetBeatmapLevelResult getBeatmapLevelResult = await this._beatmapLevelsModel.GetBeatmapLevelAsync(previewBeatmapLevel.levelID, cancellationToken);
						this._levels.Add(getBeatmapLevelResult.beatmapLevel);
						this._lastSelectedSong = getBeatmapLevelResult.beatmapLevel;
						CancellationToken cancellationToken2 = default(CancellationToken);
						Texture2D texture2D = await getBeatmapLevelResult.beatmapLevel.GetCoverImageTexture2DAsync(cancellationToken2);
						this._lastSelectedSongCoverImg = texture2D;
						this._lastSelectedCollection = pack;
						return true;
					}
				}
				pack = null;
			}
			return false;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002270F File Offset: 0x0002090F
		private void ChangeRoomOwner(long playerId)
		{
			Client.Instance.ChangeRoomOwner(this._room.roomId, playerId);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00022727 File Offset: 0x00020927
		private void KickOutRoom(long playerId)
		{
			Client.Instance.KickOutRoomPlayer(this._room.roomId, playerId);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002273F File Offset: 0x0002093F
		private void KickedOutRoomNoticeCallback(int code, string msg, KickedOutRoomNotice roomUpdatedNoticeData)
		{
			this.LeaveRoom();
			Action action = this.didFinishEvent;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00022757 File Offset: 0x00020957
		private void StartGame()
		{
			Client.Instance.StartGame(this._room.roomId);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00022770 File Offset: 0x00020970
		private void StartGameCallback(int code, string msg, StartGameNotice startGameNoticeData)
		{
			if (this.simpleDialogIsShow && this.simpleDialogIsShow && this.simpleDialogTemp != null)
			{
				this.InvokeMethod("DismissViewController", new object[] { this.simpleDialogTemp, null, false });
			}
			this._room.SetRoomStatus(BeatSaberMultiplayer.Data.Room.RoomStatus.Playing.ToString());
			this.SwitchPlayerManagerButtonAndSelectedButton();
			MenuTransitionsHelper menuTransitionsHelper = Resources.FindObjectsOfTypeAll<MenuTransitionsHelper>().FirstOrDefault<MenuTransitionsHelper>();
			if (menuTransitionsHelper != null)
			{
				PlayerData playerData = PluginUI.instance.playerData;
				PlayerSpecificSettings playerSpecificSettings = playerData.playerSpecificSettings;
				PracticeSettings practiceSettings = playerData.practiceSettings;
				OverrideEnvironmentSettings overrideEnvironmentSettings = playerData.overrideEnvironmentSettings;
				ColorScheme colorScheme = (playerData.colorSchemesSettings.overrideDefaultColors ? playerData.colorSchemesSettings.GetColorSchemeForId(playerData.colorSchemesSettings.selectedColorSchemeId) : null);
				GameplayModifiersStruct gameplayModifiersStruct;
				if (startGameNoticeData.SongCfg == null)
				{
					gameplayModifiersStruct = new GameplayModifiersStruct("");
				}
				else
				{
					gameplayModifiersStruct = new GameplayModifiersStruct(startGameNoticeData.SongCfg.Rules);
				}
				GameplayModifiers gameplayModifiers = gameplayModifiersStruct.ToGameplayModifiers();
				practiceSettings.songSpeedMul = gameplayModifiers.songSpeedMul;
				Player[] players = this._room.players;
				for (int i = 0; i < players.Length; i++)
				{
					players[i].score = 0;
				}
				InGameController instance = InGameController.instance;
				if (instance != null)
				{
					instance.SetPlayers(this._room.players);
				}
				menuTransitionsHelper.StartStandardLevel(this._levelDetailViewController.difficultyBeatmap, overrideEnvironmentSettings, colorScheme, gameplayModifiers, playerSpecificSettings, practiceSettings, "返回房间", false, delegate
				{
				}, new Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(InGameController.instance.SongFinished));
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00022914 File Offset: 0x00020B14
		private void SwitchPlayerManagerButtonAndSelectedButton()
		{
			LevelDetailViewController levelDetailViewController = this._levelDetailViewController;
			if (levelDetailViewController != null)
			{
				levelDetailViewController.SwitchTipText();
			}
			if (!Client.Instance.isHost)
			{
				return;
			}
			PlayerManagerViewController playerManagerViewController = this._playerManagerViewController;
			if (playerManagerViewController != null)
			{
				playerManagerViewController.RefreshModifierTogglesControll();
			}
			if (this._room.roomStatus == BeatSaberMultiplayer.Data.Room.RoomStatus.Playing)
			{
				PlayerManagerViewController playerManagerViewController2 = this._playerManagerViewController;
				if (playerManagerViewController2 != null)
				{
					playerManagerViewController2.SetPlayers(this._room.players, this._room.roomOwner, true);
				}
				if (this._levelDetailViewController != null)
				{
					this._levelDetailViewController.SelectActionInteractable = false;
					this._levelDetailViewController.StartActionInteractable = false;
					return;
				}
			}
			else
			{
				PlayerManagerViewController playerManagerViewController3 = this._playerManagerViewController;
				if (playerManagerViewController3 != null)
				{
					playerManagerViewController3.SetPlayers(this._room.players, this._room.roomOwner, false);
				}
				if (this._levelDetailViewController != null)
				{
					this._levelDetailViewController.SelectActionInteractable = true;
				}
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x000229F0 File Offset: 0x00020BF0
		private void RoomBroadcastNoticeCallback(int code, string msg, RoomBroadcastNotice roomBroadcastNoticeData)
		{
			switch ((RoomBroadcastDataType)Enum.Parse(typeof(RoomBroadcastDataType), roomBroadcastNoticeData.Type, true))
			{
			case RoomBroadcastDataType.UpdateScore:
			{
				RoomBroadcastDataUpdateScoreStruct roomBroadcastDataUpdateScoreStruct = JsonConvert.DeserializeObject<RoomBroadcastDataUpdateScoreStruct>(roomBroadcastNoticeData.Content);
				for (int i = 0; i < this._room.players.Length; i++)
				{
					if (this._room.players[i].playerId == roomBroadcastDataUpdateScoreStruct.playerId)
					{
						this._room.players[i].score = roomBroadcastDataUpdateScoreStruct.score;
					}
				}
				if (InGameController.instance.isPlaying)
				{
					InGameController instance = InGameController.instance;
					if (instance == null)
					{
						return;
					}
					instance.SetPlayers(this._room.players);
					return;
				}
				else if (this._multiResultsViewController != null)
				{
					string text = null;
					foreach (Player player in this._room.players)
					{
						if (player.playerId == roomBroadcastDataUpdateScoreStruct.playerId)
						{
							text = player.nickname;
						}
					}
					if (text != null)
					{
						this._multiResultsViewController.SetResults(roomBroadcastDataUpdateScoreStruct.playerId, text, roomBroadcastDataUpdateScoreStruct.score);
						return;
					}
				}
				break;
			}
			case RoomBroadcastDataType.VOIP:
			{
				if (!PluginConfig.Instance.CustomVoiceEnabled)
				{
					return;
				}
				VoipFragment voipFragment = JsonConvert.DeserializeObject<VoipFragment>(roomBroadcastNoticeData.Content);
				if (voipFragment.playerId == Client.Instance.player.playerId)
				{
					return;
				}
				InGameController instance2 = InGameController.instance;
				if (instance2 != null)
				{
					instance2.PlayVOIPFragment(voipFragment);
				}
				this._playerManagerViewController.VoiceTipBling(voipFragment.playerId);
				return;
			}
			case RoomBroadcastDataType.DownloadLevel:
				this.DownloadLevelCompletedCounter(roomBroadcastNoticeData.Content);
				return;
			default:
				Logger.log.Error("invalid RoomBroadcastNoticeData Type");
				break;
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00022B94 File Offset: 0x00020D94
		private void RoomBroadcastCallback(int code, string msg, RoomBroadcast roomBroadcastData)
		{
			RoomBroadcastDataType roomBroadcastDataType = (RoomBroadcastDataType)Enum.Parse(typeof(RoomBroadcastDataType), roomBroadcastData.Type, true);
			if (roomBroadcastDataType == RoomBroadcastDataType.UpdateScore)
			{
				RoomBroadcastDataUpdateScoreStruct roomBroadcastDataUpdateScoreStruct = JsonConvert.DeserializeObject<RoomBroadcastDataUpdateScoreStruct>(roomBroadcastData.Content);
				for (int i = 0; i < this._room.players.Length; i++)
				{
					if (this._room.players[i].playerId == roomBroadcastDataUpdateScoreStruct.playerId && this._room.players[i].playerId == Client.Instance.player.playerId)
					{
						this._room.players[i].score = roomBroadcastDataUpdateScoreStruct.score;
					}
				}
				return;
			}
			if (roomBroadcastDataType == RoomBroadcastDataType.DownloadLevel)
			{
				this.DownloadLevelCompletedCounter(roomBroadcastData.Content);
				return;
			}
			Logger.log.Info("RoomBroadcastData Type: " + roomBroadcastData.Type.ToString());
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00022C70 File Offset: 0x00020E70
		private void DownloadLevelCompletedCounter(string content)
		{
			if (!Client.Instance.isHost)
			{
				return;
			}
			RoomBroadcastDataUpdateProgressStruct roomBroadcastDataUpdateProgressStruct = JsonConvert.DeserializeObject<RoomBroadcastDataUpdateProgressStruct>(content);
			PlayerManagerViewController playerManagerViewController = this._playerManagerViewController;
			if (playerManagerViewController != null)
			{
				playerManagerViewController.UpdateDownloadProgress(roomBroadcastDataUpdateProgressStruct.playerId, roomBroadcastDataUpdateProgressStruct.progress);
			}
			if (this._downloadLevelCompletedCounter.ContainsKey(roomBroadcastDataUpdateProgressStruct.playerId))
			{
				return;
			}
			if ((double)Math.Abs(1f - roomBroadcastDataUpdateProgressStruct.progress) <= 0.01)
			{
				this._downloadLevelCompletedCounter.Add(roomBroadcastDataUpdateProgressStruct.playerId, true);
			}
			Logger.log.Info(string.Format("========= _downloadLevelCompletedCounter.Count: {0}", this._downloadLevelCompletedCounter.Count));
			Logger.log.Info(string.Format("========= _room.players.Length: {0}", this._room.players.Length));
			if (this._downloadLevelCompletedCounter.Count == this._room.players.Length)
			{
				bool flag = true;
				if (Client.Instance.songCfg != null)
				{
					flag = this.LastSelectedSongId.Contains(Client.Instance.songCfg.SongId);
				}
				LevelDetailViewController levelDetailViewController = this._levelDetailViewController;
				if (levelDetailViewController == null)
				{
					return;
				}
				levelDetailViewController.DownloadLevelCompletedCallback(flag);
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00022D8F File Offset: 0x00020F8F
		private string SongIdConvertToLocalFormat(string songId)
		{
			if (songId.Length >= 40)
			{
				return "custom_level_" + songId;
			}
			return songId;
		}

		// Token: 0x040003F4 RID: 1012
		private PlayerManagerViewController _playerManagerViewController;

		// Token: 0x040003F5 RID: 1013
		private LevelSelectViewController _levelSelectViewController;

		// Token: 0x040003F6 RID: 1014
		private VoipSettingViewController _voipSettingViewController;

		// Token: 0x040003F7 RID: 1015
		private LevelPacksViewController _levelPacksViewController;

		// Token: 0x040003F8 RID: 1016
		private LevelPackImageViewController _levelPackImageViewController;

		// Token: 0x040003F9 RID: 1017
		private LevelDetailViewController _levelDetailViewController;

		// Token: 0x040003FA RID: 1018
		private RoomNavigationController _roomNavigationController;

		// Token: 0x040003FB RID: 1019
		private NavigationController _detailNavigationController;

		// Token: 0x040003FC RID: 1020
		private MultiResultsViewController _multiResultsViewController;

		// Token: 0x040003FD RID: 1021
		private BeatmapLevelsModel _beatmapLevelsModel;

		// Token: 0x040003FE RID: 1022
		private List<IPreviewBeatmapLevel> _levels = new List<IPreviewBeatmapLevel>();

		// Token: 0x040003FF RID: 1023
		private bool simpleDialogIsShow;

		// Token: 0x04000400 RID: 1024
		private SimpleDialogPromptViewController simpleDialogTemp;

		// Token: 0x04000401 RID: 1025
		private List<BeatSaberMultiplayer.Data.SongCfg> _requestedSongs = new List<BeatSaberMultiplayer.Data.SongCfg>();

		// Token: 0x04000402 RID: 1026
		private SortMode _lastSortMode;

		// Token: 0x04000403 RID: 1027
		private string _lastSearchRequest;

		// Token: 0x04000404 RID: 1028
		private float _lastScrollPosition;

		// Token: 0x04000405 RID: 1029
		private IAnnotatedBeatmapLevelCollection _lastSelectedCollection;

		// Token: 0x04000406 RID: 1030
		private IPreviewBeatmapLevel _lastSelectedSong;

		// Token: 0x04000407 RID: 1031
		private Texture2D _lastSelectedSongCoverImg;

		// Token: 0x04000408 RID: 1032
		private string _lastSelectedSongId;

		// Token: 0x04000409 RID: 1033
		private string _selectedCharacteristicStr;

		// Token: 0x0400040A RID: 1034
		private string _selectedDifficultyStr;

		// Token: 0x0400040B RID: 1035
		private SongPreviewPlayer _songPreviewPlayer;

		// Token: 0x0400040C RID: 1036
		private BeatSaberMultiplayer.Data.Room _room;

		// Token: 0x0400040D RID: 1037
		private float _resultTotalTime = 10f;

		// Token: 0x0400040E RID: 1038
		private List<Player> _exitPlayersWhenPlaying = new List<Player>();

		// Token: 0x0400040F RID: 1039
		private Dictionary<long, bool> _downloadLevelCompletedCounter = new Dictionary<long, bool>();
	}
}
