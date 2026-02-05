using System;
using System.Text.RegularExpressions;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Data;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000053 RID: 83
	internal class CreateRoomViewController : BSMLResourceViewController
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0001C5DE File Offset: 0x0001A7DE
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

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060006E0 RID: 1760 RVA: 0x0001C614 File Offset: 0x0001A814
		// (remove) Token: 0x060006E1 RID: 1761 RVA: 0x0001C64C File Offset: 0x0001A84C
		public event Action didFinishEvent;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060006E2 RID: 1762 RVA: 0x0001C684 File Offset: 0x0001A884
		// (remove) Token: 0x060006E3 RID: 1763 RVA: 0x0001C6BC File Offset: 0x0001A8BC
		public event Action<RoomSettings> CreatedRoomEvent;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060006E4 RID: 1764 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
		// (remove) Token: 0x060006E5 RID: 1765 RVA: 0x0001C72C File Offset: 0x0001A92C
		public event Action<RoomSettings, string> SavePresetEvent;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060006E6 RID: 1766 RVA: 0x0001C764 File Offset: 0x0001A964
		// (remove) Token: 0x060006E7 RID: 1767 RVA: 0x0001C79C File Offset: 0x0001A99C
		public event Action LoadPresetEvent;

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001C7D4 File Offset: 0x0001A9D4
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			if (firstActivation)
			{
				int num = new Random().Next(100, 999);
				this._roomName = string.Format("ROOM-{0}", num);
				this._roomPassword = "";
			}
			this._createRoomButton.interactable = true;
			this.parserParams.EmitEvent("cancel");
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001C83C File Offset: 0x0001AA3C
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			this.parserParams.EmitEvent("closeAllMPModals");
			if (this._roomNameKeyboard != null)
			{
				this._roomNameKeyboard.modalKeyboard.modalView.Hide(false, null);
			}
			if (this._passwordKeyboard != null)
			{
				this._passwordKeyboard.modalKeyboard.modalView.Hide(false, null);
			}
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001C8AC File Offset: 0x0001AAAC
		public void Update()
		{
			if (this._createButtonClicked)
			{
				if (this._createButtonClickedTimer > this._createButtonClickedInterval)
				{
					this._createButtonClickedTimer = 0f;
					this._createRoomButton.interactable = true;
					this._createButtonClicked = false;
					return;
				}
				this._createButtonClicked = false;
				this._createButtonClickedTimer += Time.deltaTime;
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001C907 File Offset: 0x0001AB07
		[UIAction("room-password-changed")]
		private void PasswordEntered(string obj)
		{
			this._roomPassword = ((obj != null) ? obj.ToUpper() : null) ?? "";
			this._createRoomButton.interactable = true;
			this.parserParams.EmitEvent("cancel");
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001C940 File Offset: 0x0001AB40
		[UIAction("room-name-changed")]
		private void NameEntered(string obj)
		{
			this._roomName = ((obj != null) ? obj.ToUpper() : null) ?? "";
			this._roomName = CreateRoomViewController.Truncate(this._roomName, 16);
			this._createRoomButton.interactable = true;
			this.parserParams.EmitEvent("cancel");
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001C997 File Offset: 0x0001AB97
		[UIAction("max-players-format")]
		public string MaxPlayersFormatter(float value)
		{
			if (value < 1E-45f)
			{
				return "无限制";
			}
			return value.ToString("0");
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001C9B4 File Offset: 0x0001ABB4
		[UIAction("create-room-btn-pressed")]
		private void CreateRoomBtnPressed()
		{
			this._createRoomButton.interactable = false;
			this._createButtonClicked = true;
			Action<RoomSettings> createdRoomEvent = this.CreatedRoomEvent;
			if (createdRoomEvent == null)
			{
				return;
			}
			createdRoomEvent(new RoomSettings
			{
				name = this._roomName,
				password = this._roomPassword,
				maxPlayers = this._maxPlayers,
				resultsShowTime = this._resultsShowTime
			});
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001CA24 File Offset: 0x0001AC24
		public static string Truncate(string str, int maxLength)
		{
			string text = str;
			if (Regex.Replace(text, "[一-龥]", "zz", RegexOptions.IgnoreCase).Length <= maxLength)
			{
				return text;
			}
			for (int i = text.Length; i >= 0; i--)
			{
				text = text.Substring(0, i);
				if (Regex.Replace(text, "[一-龥]", "zz", RegexOptions.IgnoreCase).Length <= maxLength - 3)
				{
					return text + "...";
				}
			}
			return "...";
		}

		// Token: 0x04000345 RID: 837
		private string _presetName;

		// Token: 0x04000346 RID: 838
		[UIParams]
		private BSMLParserParams parserParams;

		// Token: 0x04000347 RID: 839
		[UIValue("room-name")]
		private string _roomName;

		// Token: 0x04000348 RID: 840
		[UIValue("room-password")]
		private string _roomPassword;

		// Token: 0x04000349 RID: 841
		[UIValue("max-players")]
		private int _maxPlayers = 2;

		// Token: 0x0400034A RID: 842
		[UIValue("results-show-time")]
		private int _resultsShowTime = 10;

		// Token: 0x0400034B RID: 843
		[UIValue("per-player-difficulty")]
		private bool _allowPerPlayerDifficulty = true;

		// Token: 0x0400034C RID: 844
		[UIComponent("create-room-btn")]
		private Button _createRoomButton;

		// Token: 0x0400034D RID: 845
		[UIComponent("preset-name-keyboard")]
		private ModalKeyboard _presetNameKeyboard;

		// Token: 0x0400034E RID: 846
		[UIComponent("room-name-keyboard")]
		private StringSetting _roomNameKeyboard;

		// Token: 0x0400034F RID: 847
		[UIComponent("password-keyboard")]
		private StringSetting _passwordKeyboard;

		// Token: 0x04000350 RID: 848
		private bool _createButtonClicked;

		// Token: 0x04000351 RID: 849
		private float _createButtonClickedInterval = 1f;

		// Token: 0x04000352 RID: 850
		private float _createButtonClickedTimer;
	}
}
