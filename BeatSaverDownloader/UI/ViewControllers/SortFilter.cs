using System;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x02000014 RID: 20
	public class SortFilter
	{
		// Token: 0x06000105 RID: 261 RVA: 0x000049DE File Offset: 0x00002BDE
		public SortFilter(MoreSongsListViewController.FilterMode mode, MoreSongsListViewController.BeatSaverFilterOptions beatSaverOption = MoreSongsListViewController.BeatSaverFilterOptions.Latest, MoreSongsListViewController.ScoreSaberFilterOptions scoreSaberOption = MoreSongsListViewController.ScoreSaberFilterOptions.Trending)
		{
			this.Mode = mode;
			this.BeatSaverOption = beatSaverOption;
			this.ScoreSaberOption = scoreSaberOption;
		}

		// Token: 0x0400004C RID: 76
		public MoreSongsListViewController.FilterMode Mode = MoreSongsListViewController.FilterMode.BeatSaver;

		// Token: 0x0400004D RID: 77
		public MoreSongsListViewController.BeatSaverFilterOptions BeatSaverOption = MoreSongsListViewController.BeatSaverFilterOptions.Hot;

		// Token: 0x0400004E RID: 78
		public MoreSongsListViewController.ScoreSaberFilterOptions ScoreSaberOption;
	}
}
