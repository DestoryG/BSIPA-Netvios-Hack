using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Configuration;
using Com.Netvios.Proto.Outbound;
using HMUI;
using TMPro;

namespace BeatSaberMultiplayer.UI.ViewControllers.MultiPlayerView
{
	// Token: 0x02000064 RID: 100
	internal class UserEditorViewController : BSMLResourceViewController
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001C5DE File Offset: 0x0001A7DE
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

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001FEE2 File Offset: 0x0001E0E2
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x0001FF05 File Offset: 0x0001E105
		[UIValue("nickname")]
		private string nickname
		{
			get
			{
				this._nickname = this.formatNickname(Client.Instance.player.nickname);
				return this._nickname;
			}
			set
			{
				this._nickname = this.formatNickname(value);
				Client.Instance.player.nickname = this._nickname;
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0001FF29 File Offset: 0x0001E129
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			if (firstActivation)
			{
				Client.Instance.LoginEvent += this.RefreshUserInfo;
			}
			this.parserParams.EmitEvent("cancel");
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0001FF5C File Offset: 0x0001E15C
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			this.parserParams.EmitEvent("closeAllMPModals");
			if (this._nicknameKeyboard != null)
			{
				this._nicknameKeyboard.modalKeyboard.modalView.Hide(false, null);
			}
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0001FF9C File Offset: 0x0001E19C
		private void RefreshUserInfo()
		{
			this._loginStatusBool = !string.IsNullOrEmpty(Client.Instance.player.unionid);
			this._loginStatus = (this._loginStatusBool ? "已登录" : "未登录");
			this._loginStatusTxt.text = this._loginStatus;
			this._nicknameKeyboard.text.text = this.formatNickname(Client.Instance.player.nickname);
			this._nicknameKeyboard.editButton.interactable = !this._loginStatusBool;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00020030 File Offset: 0x0001E230
		[UIAction("nickname-changed")]
		private void NicknameEntered(string keyBoardEnterNickname)
		{
			if (string.IsNullOrWhiteSpace(keyBoardEnterNickname))
			{
				return;
			}
			this.nickname = this.formatNickname(keyBoardEnterNickname);
			Client.Instance.ModifyNicknameEvent -= this.ModifyNicknameCallback;
			Client.Instance.ModifyNicknameEvent += this.ModifyNicknameCallback;
			Client.Instance.ModifyNickname(this.nickname);
			this._nicknameKeyboard.text.text = this.nickname;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x000200A5 File Offset: 0x0001E2A5
		private void ModifyNicknameCallback(int cod, string msg, ModifyNickname modifyNicknameData)
		{
			Client.Instance.ModifyNicknameEvent -= this.ModifyNicknameCallback;
			PluginConfig.Instance.Nickname = modifyNicknameData.Nickname;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x000200D0 File Offset: 0x0001E2D0
		public string formatNickname(string inStr)
		{
			int num = 8;
			if (inStr.Length > num)
			{
				inStr = inStr.Substring(0, num) + "...";
			}
			return inStr;
		}

		// Token: 0x040003C7 RID: 967
		[UIParams]
		private BSMLParserParams parserParams;

		// Token: 0x040003C8 RID: 968
		[UIComponent("login-status-text")]
		private TextMeshProUGUI _loginStatusTxt;

		// Token: 0x040003C9 RID: 969
		[UIValue("login-status-bool")]
		private bool _loginStatusBool;

		// Token: 0x040003CA RID: 970
		[UIValue("login-status")]
		private string _loginStatus = "未登录";

		// Token: 0x040003CB RID: 971
		private string _nickname;

		// Token: 0x040003CC RID: 972
		[UIComponent("nickname-keyboard")]
		private StringSetting _nicknameKeyboard;
	}
}
