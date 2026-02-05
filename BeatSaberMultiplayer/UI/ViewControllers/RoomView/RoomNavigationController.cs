using System;
using BeatSaberMarkupLanguage;
using HMUI;
using TMPro;
using UnityEngine;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x0200005F RID: 95
	internal class RoomNavigationController : NavigationController
	{
		// Token: 0x0600076A RID: 1898 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType activationType)
		{
			if (firstActivation && activationType == null)
			{
				this._errorText = BeatSaberUI.CreateText(base.rectTransform, "", new Vector2(0f, 0f));
				this._errorText.fontSize = 8f;
				this._errorText.alignment = 514;
				this._errorText.rectTransform.sizeDelta = new Vector2(120f, 6f);
			}
			this._errorText.text = "";
			this._errorText.gameObject.SetActive(false);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001F158 File Offset: 0x0001D358
		public void DisplayError(string error)
		{
			if (this._errorText != null)
			{
				this._errorText.gameObject.SetActive(true);
				this._errorText.text = error;
			}
		}

		// Token: 0x040003B0 RID: 944
		public TextMeshProUGUI _errorText;
	}
}
