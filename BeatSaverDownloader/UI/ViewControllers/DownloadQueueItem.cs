using System;
using System.Threading;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Notify;
using BeatSaverDownloader.Misc;
using BeatSaverSharp;
using SongCore.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x02000019 RID: 25
	internal class DownloadQueueItem : INotifiableHost
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000113 RID: 275 RVA: 0x00004D84 File Offset: 0x00002F84
		// (remove) Token: 0x06000114 RID: 276 RVA: 0x00004DBC File Offset: 0x00002FBC
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000115 RID: 277 RVA: 0x00004DF1 File Offset: 0x00002FF1
		[UIAction("abortClicked")]
		internal void AbortDownload()
		{
			this.cancellationTokenSource.Cancel();
			Action<DownloadQueueItem> didAbortDownload = DownloadQueueViewController.didAbortDownload;
			if (didAbortDownload == null)
			{
				return;
			}
			didAbortDownload(this);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004E0E File Offset: 0x0000300E
		public DownloadQueueItem()
		{
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004E24 File Offset: 0x00003024
		public DownloadQueueItem(Beatmap song, Texture2D cover)
		{
			this.beatmap = song;
			this._songName = song.Metadata.SongName;
			this._coverTexture = cover;
			this._authorName = song.Metadata.SongAuthorName + " <size=80%>[" + song.Metadata.LevelAuthorName + "]";
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004E98 File Offset: 0x00003098
		[UIAction("#post-parse")]
		internal void Setup()
		{
			if (!this._coverImage || !this._songNameText || !this._authorNameText)
			{
				return;
			}
			AspectRatioFitter aspectRatioFitter = this._coverImage.gameObject.AddComponent<AspectRatioFitter>();
			aspectRatioFitter.aspectRatio = 1f;
			aspectRatioFitter.aspectMode = 2;
			this._coverImage.texture = this._coverTexture;
			this._coverImage.texture.wrapMode = 1;
			this._coverImage.rectTransform.sizeDelta = new Vector2(8f, 0f);
			this._songNameText.text = this._songName;
			this._authorNameText.text = this._authorName;
			this.downloadProgress = new Progress<double>(new Action<double>(this.ProgressUpdate));
			this._bgImage = this._coverImage.transform.parent.gameObject.AddComponent<Image>();
			this._bgImage.enabled = true;
			this._bgImage.sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0f, 0f, 1f, 1f), Vector2.one / 2f);
			this._bgImage.type = 3;
			this._bgImage.fillMethod = 0;
			this._bgImage.fillAmount = 0f;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004FFC File Offset: 0x000031FC
		internal void ProgressUpdate(double progress)
		{
			this._downloadingProgess = (float)progress;
			Color color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(this._downloadingProgess * 0.35f, 1f), 1f, 1f));
			color.a = 0.35f;
			this._bgImage.color = color;
			this._bgImage.fillAmount = this._downloadingProgess;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005068 File Offset: 0x00003268
		public async void Download()
		{
			this.queueState = SongQueueState.Downloading;
			await SongDownloader.Instance.DownloadSong(this.beatmap, this.cancellationTokenSource.Token, this.downloadProgress, false);
			this.queueState = SongQueueState.Downloaded;
			Action<DownloadQueueItem> didFinishDownloadingItem = DownloadQueueViewController.didFinishDownloadingItem;
			if (didFinishDownloadingItem != null)
			{
				didFinishDownloadingItem(this);
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00002053 File Offset: 0x00000253
		public void Update()
		{
		}

		// Token: 0x04000058 RID: 88
		public SongQueueState queueState;

		// Token: 0x04000059 RID: 89
		internal Progress<double> downloadProgress;

		// Token: 0x0400005A RID: 90
		internal Beatmap beatmap;

		// Token: 0x0400005B RID: 91
		private Image _bgImage;

		// Token: 0x0400005C RID: 92
		private float _downloadingProgess;

		// Token: 0x0400005D RID: 93
		internal CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x0400005F RID: 95
		[UIComponent("coverImage")]
		private RawImage _coverImage;

		// Token: 0x04000060 RID: 96
		[UIComponent("songNameText")]
		private TextMeshProUGUI _songNameText;

		// Token: 0x04000061 RID: 97
		[UIComponent("authorNameText")]
		private TextMeshProUGUI _authorNameText;

		// Token: 0x04000062 RID: 98
		private string _songName;

		// Token: 0x04000063 RID: 99
		private string _authorName;

		// Token: 0x04000064 RID: 100
		private Texture2D _coverTexture;
	}
}
