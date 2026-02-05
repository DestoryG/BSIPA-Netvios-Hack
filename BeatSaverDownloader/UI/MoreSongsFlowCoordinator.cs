using System;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage;
using BeatSaverDownloader.UI.ViewControllers;
using BeatSaverSharp;
using HMUI;
using UnityEngine;

namespace BeatSaverDownloader.UI
{
	// Token: 0x02000010 RID: 16
	public class MoreSongsFlowCoordinator : FlowCoordinator
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003849 File Offset: 0x00001A49
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00003851 File Offset: 0x00001A51
		public FlowCoordinator ParentFlowCoordinator { get; protected set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000385A File Offset: 0x00001A5A
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00003862 File Offset: 0x00001A62
		public bool AllowFlowCoordinatorChange { get; protected set; } = true;

		// Token: 0x060000CB RID: 203 RVA: 0x0000386C File Offset: 0x00001A6C
		public void Awake()
		{
			if (this._moreSongsView == null)
			{
				this._moreSongsView = BeatSaberUI.CreateViewController<MoreSongsListViewController>();
				this._songDetailView = BeatSaberUI.CreateViewController<SongDetailViewController>();
				this._multiSelectDetailView = BeatSaberUI.CreateViewController<MultiSelectDetailViewController>();
				this._moreSongsNavigationcontroller = BeatSaberUI.CreateViewController<NavigationController>();
				this._moreSongsView.navController = this._moreSongsNavigationcontroller;
				this._songDescriptionView = BeatSaberUI.CreateViewController<SongDescriptionViewController>();
				this._downloadQueueView = BeatSaberUI.CreateViewController<DownloadQueueViewController>();
				MoreSongsListViewController moreSongsView = this._moreSongsView;
				moreSongsView.didSelectSong = (Action<StrongBox<Beatmap>, Texture2D>)Delegate.Combine(moreSongsView.didSelectSong, new Action<StrongBox<Beatmap>, Texture2D>(this.HandleDidSelectSong));
				MoreSongsListViewController moreSongsView2 = this._moreSongsView;
				moreSongsView2.filterDidChange = (Action)Delegate.Combine(moreSongsView2.filterDidChange, new Action(this.HandleFilterDidChange));
				MoreSongsListViewController moreSongsView3 = this._moreSongsView;
				moreSongsView3.multiSelectDidChange = (Action)Delegate.Combine(moreSongsView3.multiSelectDidChange, new Action(this.HandleMultiSelectDidChange));
				SongDetailViewController songDetailView = this._songDetailView;
				songDetailView.didPressDownload = (Action<Beatmap, Texture2D>)Delegate.Combine(songDetailView.didPressDownload, new Action<Beatmap, Texture2D>(this.HandleDidPressDownload));
				SongDetailViewController songDetailView2 = this._songDetailView;
				songDetailView2.didPressUploader = (Action<User>)Delegate.Combine(songDetailView2.didPressUploader, new Action<User>(this.HandleDidPressUploader));
				SongDetailViewController songDetailView3 = this._songDetailView;
				songDetailView3.setDescription = (Action<string>)Delegate.Combine(songDetailView3.setDescription, new Action<string>(this._songDescriptionView.Initialize));
				MultiSelectDetailViewController multiSelectDetailView = this._multiSelectDetailView;
				multiSelectDetailView.multiSelectClearPressed = (Action)Delegate.Combine(multiSelectDetailView.multiSelectClearPressed, new Action(this._moreSongsView.MultiSelectClear));
				MultiSelectDetailViewController multiSelectDetailView2 = this._multiSelectDetailView;
				multiSelectDetailView2.multiSelectDownloadPressed = (Action)Delegate.Combine(multiSelectDetailView2.multiSelectDownloadPressed, new Action(this.HandleMultiSelectDownload));
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003A20 File Offset: 0x00001C20
		protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
		{
			try
			{
				if (firstActivation)
				{
					this.UpdateTitle();
					base.showBackButton = true;
					base.SetViewControllersToNavigationController(this._moreSongsNavigationcontroller, new ViewController[] { this._moreSongsView });
					base.ProvideInitialViewControllers(this._moreSongsNavigationcontroller, this._downloadQueueView, null, null, null);
				}
			}
			catch (Exception ex)
			{
				Plugin.log.Error(ex);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003A90 File Offset: 0x00001C90
		public void SetParentFlowCoordinator(FlowCoordinator parent)
		{
			if (!this.AllowFlowCoordinatorChange)
			{
				throw new InvalidOperationException("Changing the parent FlowCoordinator is not allowed on this instance.");
			}
			this.ParentFlowCoordinator = parent;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003AAC File Offset: 0x00001CAC
		internal void UpdateTitle()
		{
			base.title = "歌曲列表";
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003ABC File Offset: 0x00001CBC
		internal void HandleDidSelectSong(StrongBox<Beatmap> song, Texture2D cover = null)
		{
			this._songDetailView.ClearData();
			this._songDescriptionView.ClearData();
			if (!this._moreSongsView.MultiSelectEnabled)
			{
				if (!this._songDetailView.isInViewControllerHierarchy)
				{
					base.PushViewControllerToNavigationController(this._moreSongsNavigationcontroller, this._songDetailView, null, false);
				}
				base.SetRightScreenViewController(this._songDescriptionView, false);
				this._songDetailView.Initialize(song, cover);
				return;
			}
			int count = this._moreSongsView._multiSelectSongs.Count;
			string text = ((count > 1) ? "Songs" : "Song");
			this._multiSelectDetailView.MultiDownloadText = string.Format("Add {0} {1} To Queue", count, text);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003B68 File Offset: 0x00001D68
		internal void HandleDidPressDownload(Beatmap song, Texture2D cover)
		{
			Plugin.log.Info("Download pressed for song: " + song.Metadata.SongName);
			this._songDetailView.UpdateDownloadButtonStatus();
			this._downloadQueueView.EnqueueSong(song, cover);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003BAF File Offset: 0x00001DAF
		internal void HandleMultiSelectDownload()
		{
			this._downloadQueueView.EnqueueSongs(this._moreSongsView._multiSelectSongs.ToArray(), this._downloadQueueView.cancellationTokenSource.Token);
			this._moreSongsView.MultiSelectClear();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003BE8 File Offset: 0x00001DE8
		internal void HandleMultiSelectDidChange()
		{
			if (this._moreSongsView.MultiSelectEnabled)
			{
				this._songDetailView.ClearData();
				this._songDescriptionView.ClearData();
				this._moreSongsNavigationcontroller.PopViewControllers(this._moreSongsNavigationcontroller.viewControllers.Count, null, true);
				this._moreSongsNavigationcontroller.PushViewController(this._moreSongsView, null, true);
				this._moreSongsNavigationcontroller.PushViewController(this._multiSelectDetailView, null, false);
				return;
			}
			this._moreSongsNavigationcontroller.PopViewControllers(this._moreSongsNavigationcontroller.viewControllers.Count, null, true);
			this._moreSongsNavigationcontroller.PushViewController(this._moreSongsView, null, true);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003C8C File Offset: 0x00001E8C
		internal void HandleDidPressUploader(User uploader)
		{
			Plugin.log.Info("Uploader pressed for user: " + uploader.Username);
			this._moreSongsView.SortByUser(uploader);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003CB4 File Offset: 0x00001EB4
		internal void HandleFilterDidChange()
		{
			this.UpdateTitle();
			if (this._songDetailView.isInViewControllerHierarchy)
			{
				base.PopViewControllersFromNavigationController(this._moreSongsNavigationcontroller, 1, null, false);
			}
			this._songDetailView.ClearData();
			this._songDescriptionView.ClearData();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003CF0 File Offset: 0x00001EF0
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			if (this._songDetailView.isInViewControllerHierarchy)
			{
				base.PopViewControllersFromNavigationController(this._moreSongsNavigationcontroller, 1, null, true);
			}
			this._moreSongsView.Cleanup();
			this._downloadQueueView.AbortAllDownloads();
			this.ParentFlowCoordinator.DismissFlowCoordinator(this, null, false);
		}

		// Token: 0x04000025 RID: 37
		private NavigationController _moreSongsNavigationcontroller;

		// Token: 0x04000026 RID: 38
		private MoreSongsListViewController _moreSongsView;

		// Token: 0x04000027 RID: 39
		private SongDetailViewController _songDetailView;

		// Token: 0x04000028 RID: 40
		private MultiSelectDetailViewController _multiSelectDetailView;

		// Token: 0x04000029 RID: 41
		private SongDescriptionViewController _songDescriptionView;

		// Token: 0x0400002A RID: 42
		private DownloadQueueViewController _downloadQueueView;
	}
}
