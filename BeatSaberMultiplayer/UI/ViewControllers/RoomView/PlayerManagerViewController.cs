using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.Helper;
using BS_Utils.Utilities;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000059 RID: 89
	internal class PlayerManagerViewController : BSMLResourceViewController
	{
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001C5DE File Offset: 0x0001A7DE
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

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600073F RID: 1855 RVA: 0x0001E7D8 File Offset: 0x0001C9D8
		// (remove) Token: 0x06000740 RID: 1856 RVA: 0x0001E810 File Offset: 0x0001CA10
		public event Action<long> kickOutRoomPressedEvent;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000741 RID: 1857 RVA: 0x0001E848 File Offset: 0x0001CA48
		// (remove) Token: 0x06000742 RID: 1858 RVA: 0x0001E880 File Offset: 0x0001CA80
		public event Action<long> changeRoomOwnerPressed;

		// Token: 0x06000743 RID: 1859 RVA: 0x0001E8B5 File Offset: 0x0001CAB5
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			this.playersList.tableView.ClearSelection();
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001E8D1 File Offset: 0x0001CAD1
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
			this.playerInfosList.Clear();
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001E8E8 File Offset: 0x0001CAE8
		public void SetPlayers(Player[] dataPlayers, long roomOwner, bool switchFlg = false)
		{
			this.playerInfosList.Clear();
			List<Player> list = new List<Player>();
			foreach (Player player in dataPlayers)
			{
				Player player2 = new Player(player.appChannel, player.playerId, player.nickname, player.avatar, 0, player.status.ToString(), null);
				list.Add(player2);
			}
			if (list != null && list.Count != 0)
			{
				foreach (Player player3 in list)
				{
					this.playerInfosList.Add(new PlayerManagerViewController.PlayerListObject(player3, roomOwner, this.changeRoomOwnerPressed, this.kickOutRoomPressedEvent, switchFlg));
				}
				this._noRoomsText.enabled = false;
			}
			else
			{
				this._noRoomsText.enabled = true;
			}
			this.playersList.tableView.ReloadData();
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001E9E8 File Offset: 0x0001CBE8
		[UIAction("transfer-room-owner-pressed")]
		private void TransferRoomOwnerPressed()
		{
			Logger.log.Info("========== transfer-room-owner-pressed ==========");
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000196A0 File Offset: 0x000178A0
		[UIAction("player-selected")]
		private void PlayerSelected()
		{
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000748 RID: 1864 RVA: 0x0001E9FC File Offset: 0x0001CBFC
		// (remove) Token: 0x06000749 RID: 1865 RVA: 0x0001EA34 File Offset: 0x0001CC34
		public event Action gameplayModifiersChanged;

		// Token: 0x0600074A RID: 1866 RVA: 0x0001EA6C File Offset: 0x0001CC6C
		[UIAction("#post-parse")]
		protected void SetupViewController()
		{
			this.modifiersPanel = Object.Instantiate<GameplayModifiersPanelController>(Resources.FindObjectsOfTypeAll<GameplayModifiersPanelController>().First<GameplayModifiersPanelController>(), base.rectTransform, false);
			this.modifiersPanel.gameObject.SetActive(true);
			this.modifiersPanel.transform.SetParent(this.modifiersTab, false);
			(this.modifiersPanel.transform as RectTransform).anchorMin = new Vector2(0.5f, 0f);
			(this.modifiersPanel.transform as RectTransform).anchorMax = new Vector2(0.5f, 1f);
			(this.modifiersPanel.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
			(this.modifiersPanel.transform as RectTransform).sizeDelta = new Vector2(120f, 57f);
			HoverHintController hoverHintController = Resources.FindObjectsOfTypeAll<HoverHintController>().First<HoverHintController>();
			HoverHint[] componentsInChildren = this.modifiersPanel.GetComponentsInChildren<HoverHint>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetPrivateField("_hoverHintController", hoverHintController);
			}
			this.modifiersPanel.Awake();
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001EB8B File Offset: 0x0001CD8B
		private void ModifiersChangeEvent(bool enabled)
		{
			Action action = this.gameplayModifiersChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001EBA0 File Offset: 0x0001CDA0
		public void RefreshModifierTogglesControll()
		{
			GameplayModifierToggle[] privateField = this.modifiersPanel.GetPrivateField("_gameplayModifierToggles");
			if (Client.Instance.isHost && Client.Instance.roomStatus == Room.RoomStatus.Waiting)
			{
				foreach (GameplayModifierToggle gameplayModifierToggle in privateField)
				{
					gameplayModifierToggle.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.ModifiersChangeEvent));
					gameplayModifierToggle.toggle.interactable = true;
					gameplayModifierToggle.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.ModifiersChangeEvent));
				}
				return;
			}
			foreach (GameplayModifierToggle gameplayModifierToggle2 in privateField)
			{
				gameplayModifierToggle2.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.ModifiersChangeEvent));
				gameplayModifierToggle2.toggle.interactable = false;
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001EC68 File Offset: 0x0001CE68
		public void SetGameplayModifiers(GameplayModifiers modifiers)
		{
			if (this.modifiersPanel != null)
			{
				this.modifiersPanel.SetData(modifiers);
				this.modifiersPanel.Refresh();
				this.RefreshModifierTogglesControll();
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001EC98 File Offset: 0x0001CE98
		private void Update()
		{
			if (Time.frameCount % 45 == 0 && this.pingText != null && Client.Instance.isConnected)
			{
				if (Client.Instance.pingCounter < 0)
				{
					return;
				}
				this.pingText.text = "PING: " + Client.Instance.pingCounter.ToString() + "ms";
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001ED00 File Offset: 0x0001CF00
		public void UpdateDownloadProgress(long playerId, float progress)
		{
			foreach (object obj in this.playerInfosList)
			{
				PlayerManagerViewController.PlayerListObject playerListObject = (PlayerManagerViewController.PlayerListObject)obj;
				if (playerListObject.player.playerId == playerId)
				{
					playerListObject.UpdateProgress(progress);
					break;
				}
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001ED68 File Offset: 0x0001CF68
		public void VoiceTipBling(long playerId)
		{
			if (!base.gameObject.active)
			{
				return;
			}
			foreach (object obj in this.playerInfosList)
			{
				PlayerManagerViewController.PlayerListObject playerListObject = (PlayerManagerViewController.PlayerListObject)obj;
				if (playerListObject.player.playerId == playerId)
				{
					base.StartCoroutine(playerListObject.VoiceIconBling());
					break;
				}
			}
		}

		// Token: 0x04000399 RID: 921
		[UIComponent("ping-text")]
		public TextMeshProUGUI pingText;

		// Token: 0x0400039A RID: 922
		[UIComponent("no-players-text")]
		private TextMeshProUGUI _noRoomsText;

		// Token: 0x0400039B RID: 923
		[UIComponent("players-list")]
		public CustomCellListTableData playersList;

		// Token: 0x0400039C RID: 924
		[UIValue("players")]
		public List<object> playerInfosList = new List<object>();

		// Token: 0x0400039D RID: 925
		[UIComponent("modifiers-rect")]
		public RectTransform modifiersTab;

		// Token: 0x0400039E RID: 926
		[UIComponent("modifiers-panel-blocker")]
		public Image modifiersPanelBlocker;

		// Token: 0x0400039F RID: 927
		public GameplayModifiersPanelController modifiersPanel;

		// Token: 0x020000EE RID: 238
		public class PlayerListObject
		{
			// Token: 0x14000034 RID: 52
			// (add) Token: 0x06000AB6 RID: 2742 RVA: 0x00027B88 File Offset: 0x00025D88
			// (remove) Token: 0x06000AB7 RID: 2743 RVA: 0x00027BC0 File Offset: 0x00025DC0
			public event Action<long> subChangeRoomOwnerPressedEvent;

			// Token: 0x14000035 RID: 53
			// (add) Token: 0x06000AB8 RID: 2744 RVA: 0x00027BF8 File Offset: 0x00025DF8
			// (remove) Token: 0x06000AB9 RID: 2745 RVA: 0x00027C30 File Offset: 0x00025E30
			public event Action<long> subKickOutRoomPressedEvent;

			// Token: 0x06000ABA RID: 2746 RVA: 0x00027C65 File Offset: 0x00025E65
			[UIAction("change-room-owner-pressed")]
			private void ChangeRoomOwner()
			{
				Action<long> action = this.subChangeRoomOwnerPressedEvent;
				if (action == null)
				{
					return;
				}
				action(this.player.playerId);
			}

			// Token: 0x06000ABB RID: 2747 RVA: 0x00027C82 File Offset: 0x00025E82
			[UIAction("kick-out-room-pressed")]
			private void KickOutRoom()
			{
				Action<long> action = this.subKickOutRoomPressedEvent;
				if (action == null)
				{
					return;
				}
				action(this.player.playerId);
			}

			// Token: 0x06000ABC RID: 2748 RVA: 0x00027CA0 File Offset: 0x00025EA0
			[UIAction("#post-parse")]
			private void SetUp()
			{
				this._voiceIcon.texture = Sprites.speakerIcon.texture;
				this._voiceIcon.enabled = true;
				if (this._switchFlg)
				{
					this._changeOwnerButton.interactable = false;
					this._kickOutButton.interactable = false;
					return;
				}
				this._changeOwnerButton.interactable = this._buttonShow;
				this._kickOutButton.interactable = this._buttonShow;
			}

			// Token: 0x06000ABD RID: 2749 RVA: 0x00027D11 File Offset: 0x00025F11
			public IEnumerator VoiceIconBling()
			{
				yield return null;
				this._voiceIcon.enabled = false;
				yield return new WaitForSeconds(0.1f);
				this._voiceIcon.enabled = true;
				yield return new WaitForSeconds(0.1f);
				this._voiceIcon.enabled = false;
				yield return new WaitForSeconds(0.1f);
				this._voiceIcon.enabled = true;
				yield break;
			}

			// Token: 0x06000ABE RID: 2750 RVA: 0x00027D20 File Offset: 0x00025F20
			public PlayerListObject(Player player, long roomOwner, Action<long> changeRoomOwnerEvent, Action<long> kickOutRoomEvent, bool switchFlg = false)
			{
				this.player = player;
				this._roomOwner = roomOwner;
				this.subChangeRoomOwnerPressedEvent = changeRoomOwnerEvent;
				this.subKickOutRoomPressedEvent = kickOutRoomEvent;
				this._switchFlg = switchFlg;
				if (Client.Instance.isHost)
				{
					this._buttonShow = true;
				}
				if (player.playerId == Client.Instance.player.playerId)
				{
					this._buttonShow = false;
				}
				if (player.playerId == this._roomOwner)
				{
					this._nicknameColor = "red";
				}
				this._nickname = player.nickname;
			}

			// Token: 0x06000ABF RID: 2751 RVA: 0x00027DB9 File Offset: 0x00025FB9
			public void SetChangeRoomOwnerEvent(Action<long> changeRoomOwnerEvent)
			{
				this.subChangeRoomOwnerPressedEvent = changeRoomOwnerEvent;
			}

			// Token: 0x06000AC0 RID: 2752 RVA: 0x00027DC2 File Offset: 0x00025FC2
			public void SetKickOutRoomEvent(Action<long> kickOutRoomEvent)
			{
				this.subKickOutRoomPressedEvent = kickOutRoomEvent;
			}

			// Token: 0x06000AC1 RID: 2753 RVA: 0x00027DCC File Offset: 0x00025FCC
			public void UpdateProgress(float progress)
			{
				if (this._progressText == null)
				{
					return;
				}
				if (Math.Abs(1f - progress) <= 0.01f)
				{
					this._progressText.text = "歌曲下载完毕";
					return;
				}
				this._progressText.text = "下载中( " + Math.Round((double)(progress * 100f), 2).ToString() + "/100 )";
			}

			// Token: 0x040005A7 RID: 1447
			public Player player;

			// Token: 0x040005A8 RID: 1448
			[UIComponent("voice-icon")]
			private RawImage _voiceIcon;

			// Token: 0x040005A9 RID: 1449
			[UIValue("nickname")]
			private string _nickname;

			// Token: 0x040005AA RID: 1450
			[UIValue("nickname-color")]
			private string _nicknameColor = "white";

			// Token: 0x040005AB RID: 1451
			[UIComponent("change-owner")]
			private Button _changeOwnerButton;

			// Token: 0x040005AC RID: 1452
			[UIComponent("kick-out")]
			private Button _kickOutButton;

			// Token: 0x040005AD RID: 1453
			[UIComponent("progress-text")]
			private TextMeshProUGUI _progressText;

			// Token: 0x040005AE RID: 1454
			private long _roomOwner;

			// Token: 0x040005AF RID: 1455
			private bool _buttonShow;

			// Token: 0x040005B0 RID: 1456
			private bool _switchFlg;
		}
	}
}
