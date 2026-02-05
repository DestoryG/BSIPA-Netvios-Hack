using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using TMPro;
using UnityEngine;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x0200001A RID: 26
	internal class MultiSelectDetailViewController : BSMLResourceViewController
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000509F File Offset: 0x0000329F
		public override string ResourceName
		{
			get
			{
				return "BeatSaverDownloader.UI.BSML.multiSelectDetailView.bsml";
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000050A6 File Offset: 0x000032A6
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000050AE File Offset: 0x000032AE
		[UIValue("multiDownloadText")]
		public string MultiDownloadText
		{
			get
			{
				return this._multiDownloadText;
			}
			set
			{
				this._multiDownloadText = value;
				base.NotifyPropertyChanged("MultiDownloadText");
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000050C2 File Offset: 0x000032C2
		[UIAction("clearPressed")]
		internal void ClearButtonPressed()
		{
			Action action = this.multiSelectClearPressed;
			if (action != null)
			{
				action();
			}
			this.MultiDownloadText = "Add Songs To Queue";
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000050E0 File Offset: 0x000032E0
		[UIAction("downloadPressed")]
		internal void DownloadButtonPressed()
		{
			Action action = this.multiSelectDownloadPressed;
			if (action != null)
			{
				action();
			}
			this.MultiDownloadText = "Add Songs To Queue";
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005100 File Offset: 0x00003300
		[UIAction("#post-parse")]
		internal void Setup()
		{
			(base.transform as RectTransform).sizeDelta = new Vector2(70f, 0f);
			(base.transform as RectTransform).anchorMin = new Vector2(0.5f, 0f);
			(base.transform as RectTransform).anchorMax = new Vector2(0.5f, 1f);
			this._multiTextPage.GetComponentInChildren<TextMeshProUGUI>().alignment = 514;
		}

		// Token: 0x04000065 RID: 101
		[UIValue("multiDescription")]
		private string _multiDescription = "\n <size=135%><b>Multi-Select Activated!</b></size>\n New Pages will not be fetched while this mode is on.\nSongs will not be added to queue if already downloaded.\n\n Press the \"Add Songs to Queue\" Button to download all of your selected songs, and press the clear button to clear your selection. </align>";

		// Token: 0x04000066 RID: 102
		[UIComponent("textPage")]
		private TextPageScrollView _multiTextPage;

		// Token: 0x04000067 RID: 103
		public Action multiSelectClearPressed;

		// Token: 0x04000068 RID: 104
		public Action multiSelectDownloadPressed;

		// Token: 0x04000069 RID: 105
		private string _multiDownloadText = "Add Songs To Queue";
	}
}
