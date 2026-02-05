using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaverDownloader.Misc;
using BeatSaverSharp;
using BeatSaverSharp.Exceptions;
using HMUI;
using SongCore;
using UnityEngine;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x02000018 RID: 24
	public class DownloadQueueViewController : BSMLResourceViewController
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00004A87 File Offset: 0x00002C87
		public override string ResourceName
		{
			get
			{
				return "BeatSaverDownloader.UI.BSML.downloadQueue.bsml";
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000043AD File Offset: 0x000025AD
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004A90 File Offset: 0x00002C90
		[UIAction("#post-parse")]
		internal void Setup()
		{
			CustomCellListTableData downloadList = this._downloadList;
			if (downloadList != null)
			{
				TableView tableView = downloadList.tableView;
				if (tableView != null)
				{
					tableView.ReloadData();
				}
			}
			DownloadQueueViewController.didAbortDownload = (Action<DownloadQueueItem>)Delegate.Combine(DownloadQueueViewController.didAbortDownload, new Action<DownloadQueueItem>(this.DownloadAborted));
			DownloadQueueViewController.didFinishDownloadingItem = (Action<DownloadQueueItem>)Delegate.Combine(DownloadQueueViewController.didFinishDownloadingItem, new Action<DownloadQueueItem>(this.UpdateDownloadingState));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004AFC File Offset: 0x00002CFC
		internal void EnqueueSong(Beatmap song, Texture2D cover)
		{
			DownloadQueueItem downloadQueueItem = new DownloadQueueItem(song, cover);
			this.queueItems.Add(downloadQueueItem);
			SongDownloader.Instance.QueuedDownload(song.Hash);
			CustomCellListTableData downloadList = this._downloadList;
			if (downloadList != null)
			{
				TableView tableView = downloadList.tableView;
				if (tableView != null)
				{
					tableView.ReloadData();
				}
			}
			this.UpdateDownloadingState(downloadQueueItem);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004B50 File Offset: 0x00002D50
		internal void AbortAllDownloads()
		{
			this.cancellationTokenSource.Cancel();
			this.cancellationTokenSource.Dispose();
			this.cancellationTokenSource = new CancellationTokenSource();
			object[] array = this.queueItems.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				((DownloadQueueItem)array[i]).AbortDownload();
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004BA8 File Offset: 0x00002DA8
		internal async void EnqueueSongs(Tuple<Beatmap, Texture2D>[] songs, CancellationToken cancellationToken)
		{
			int i = 0;
			while (i < songs.Length)
			{
				DownloadQueueViewController.<>c__DisplayClass11_0 CS$<>8__locals1 = new DownloadQueueViewController.<>c__DisplayClass11_0();
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}
				Tuple<Beatmap, Texture2D> pair = songs[i];
				CS$<>8__locals1.map = pair.Item1;
				if (!CS$<>8__locals1.map.Partial)
				{
					goto IL_011B;
				}
				if (!SongDownloader.Instance.IsSongDownloaded(CS$<>8__locals1.map.Hash))
				{
					try
					{
						await CS$<>8__locals1.map.Populate();
					}
					catch (InvalidPartialException)
					{
						Plugin.log.Warn("Map not found on BeatSaver");
						goto IL_0186;
					}
					goto IL_011B;
				}
				IL_0186:
				i++;
				continue;
				IL_011B:
				int num = (this.queueItems.Any((object x) => (x as DownloadQueueItem).beatmap == CS$<>8__locals1.map) ? 1 : 0);
				bool flag = SongDownloader.Instance.IsSongDownloaded(CS$<>8__locals1.map.Hash);
				if ((num == 0) & !flag)
				{
					this.EnqueueSong(CS$<>8__locals1.map, pair.Item2);
				}
				CS$<>8__locals1 = null;
				pair = null;
				goto IL_0186;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004BF0 File Offset: 0x00002DF0
		internal void UpdateDownloadingState(DownloadQueueItem item)
		{
			foreach (DownloadQueueItem downloadQueueItem in this.queueItems.Where((object x) => (x as DownloadQueueItem).queueState == SongQueueState.Queued).ToArray<object>())
			{
				if (PluginConfig.maxSimultaneousDownloads > this.queueItems.Where((object x) => (x as DownloadQueueItem).queueState == SongQueueState.Downloading).ToArray<object>().Length)
				{
					downloadQueueItem.Download();
				}
			}
			foreach (DownloadQueueItem downloadQueueItem2 in this.queueItems.Where((object x) => (x as DownloadQueueItem).queueState == SongQueueState.Downloaded).ToArray<object>())
			{
				this.queueItems.Remove(downloadQueueItem2);
				CustomCellListTableData downloadList = this._downloadList;
				if (downloadList != null)
				{
					TableView tableView = downloadList.tableView;
					if (tableView != null)
					{
						tableView.ReloadData();
					}
				}
			}
			if (this.queueItems.Count == 0)
			{
				Loader.Instance.RefreshSongs(false);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004D0C File Offset: 0x00002F0C
		internal void DownloadAborted(DownloadQueueItem download)
		{
			if (this.queueItems.Contains(download))
			{
				this.queueItems.Remove(download);
			}
			if (this.queueItems.Count == 0)
			{
				Loader.Instance.RefreshSongs(false);
			}
			CustomCellListTableData downloadList = this._downloadList;
			if (downloadList == null)
			{
				return;
			}
			TableView tableView = downloadList.tableView;
			if (tableView == null)
			{
				return;
			}
			tableView.ReloadData();
		}

		// Token: 0x04000053 RID: 83
		internal static Action<DownloadQueueItem> didAbortDownload;

		// Token: 0x04000054 RID: 84
		internal static Action<DownloadQueueItem> didFinishDownloadingItem;

		// Token: 0x04000055 RID: 85
		[UIValue("download-queue")]
		internal List<object> queueItems = new List<object>();

		// Token: 0x04000056 RID: 86
		internal CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04000057 RID: 87
		[UIComponent("download-list")]
		private CustomCellListTableData _downloadList;
	}
}
