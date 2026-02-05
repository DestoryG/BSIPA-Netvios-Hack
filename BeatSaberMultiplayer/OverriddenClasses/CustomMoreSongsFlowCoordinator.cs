using System;
using System.Reflection;
using BeatSaberMarkupLanguage;
using BeatSaberMultiplayer.Interop;
using BeatSaverDownloader.UI;
using BeatSaverDownloader.UI.ViewControllers;
using HMUI;
using IPA.Utilities;

namespace BeatSaberMultiplayer.OverriddenClasses
{
	// Token: 0x0200006E RID: 110
	public class CustomMoreSongsFlowCoordinator : MoreSongsFlowCoordinator, IDismissable
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x0002360D File Offset: 0x0002180D
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x00023614 File Offset: 0x00021814
		public static bool CanCreate { get; private set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0002361C File Offset: 0x0002181C
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x00023624 File Offset: 0x00021824
		public new FlowCoordinator ParentFlowCoordinator { get; set; }

		// Token: 0x06000855 RID: 2133 RVA: 0x00023630 File Offset: 0x00021830
		static CustomMoreSongsFlowCoordinator()
		{
			try
			{
				CustomMoreSongsFlowCoordinator.SongDetailViewController = FieldAccessor<MoreSongsFlowCoordinator, BeatSaverDownloader.UI.ViewControllers.SongDetailViewController>.GetAccessor("_songDetailView");
				CustomMoreSongsFlowCoordinator.MoreSongsNavigationController = FieldAccessor<MoreSongsFlowCoordinator, NavigationController>.GetAccessor("_moreSongsNavigationcontroller");
				CustomMoreSongsFlowCoordinator.MoreSongsView = FieldAccessor<MoreSongsFlowCoordinator, MoreSongsListViewController>.GetAccessor("_moreSongsView");
				CustomMoreSongsFlowCoordinator.DownloadQueueView = FieldAccessor<MoreSongsFlowCoordinator, DownloadQueueViewController>.GetAccessor("_downloadQueueView");
				string text = "AbortAllDownloads";
				CustomMoreSongsFlowCoordinator.AbortAllDownloadsMethod = typeof(DownloadQueueViewController).GetMethod(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (CustomMoreSongsFlowCoordinator.AbortAllDownloadsMethod == null)
				{
					throw new MissingMethodException("Method " + text + " does not exist.", text);
				}
				CustomMoreSongsFlowCoordinator.CanCreate = true;
			}
			catch (Exception ex)
			{
				CustomMoreSongsFlowCoordinator.CanCreate = false;
				Logger.log.Error("Error creating accessors for MoreSongsFlowCoordinator, Downloader will be unavailable in Multiplayer: " + ex.Message);
				Logger.log.Debug(ex);
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00023700 File Offset: 0x00021900
		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			this.Dismiss(false);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0002370C File Offset: 0x0002190C
		public unsafe void Dismiss(bool immediately)
		{
			MoreSongsFlowCoordinator moreSongsFlowCoordinator = this;
			if (CustomMoreSongsFlowCoordinator.SongDetailViewController(ref moreSongsFlowCoordinator)->isInViewControllerHierarchy)
			{
				base.PopViewControllersFromNavigationController(*CustomMoreSongsFlowCoordinator.MoreSongsNavigationController(ref moreSongsFlowCoordinator), 1, null, true);
			}
			CustomMoreSongsFlowCoordinator.MoreSongsView(ref moreSongsFlowCoordinator)->Cleanup();
			CustomMoreSongsFlowCoordinator.AbortAllDownloadsMethod.Invoke(*CustomMoreSongsFlowCoordinator.DownloadQueueView(ref moreSongsFlowCoordinator), null);
			this.ParentFlowCoordinator.DismissFlowCoordinator(this, null, immediately);
		}

		// Token: 0x0400041E RID: 1054
		private static FieldAccessor<MoreSongsFlowCoordinator, SongDetailViewController>.Accessor SongDetailViewController;

		// Token: 0x0400041F RID: 1055
		private static FieldAccessor<MoreSongsFlowCoordinator, NavigationController>.Accessor MoreSongsNavigationController;

		// Token: 0x04000420 RID: 1056
		private static FieldAccessor<MoreSongsFlowCoordinator, MoreSongsListViewController>.Accessor MoreSongsView;

		// Token: 0x04000421 RID: 1057
		private static FieldAccessor<MoreSongsFlowCoordinator, DownloadQueueViewController>.Accessor DownloadQueueView;

		// Token: 0x04000422 RID: 1058
		private static MethodInfo AbortAllDownloadsMethod;
	}
}
