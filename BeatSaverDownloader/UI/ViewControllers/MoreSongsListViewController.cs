using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaverDownloader.Misc;
using BeatSaverSharp;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x02000013 RID: 19
	public class MoreSongsListViewController : BSMLResourceViewController
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003FC2 File Offset: 0x000021C2
		public override string ResourceName
		{
			get
			{
				return "BeatSaverDownloader.UI.BSML.moreSongsList.bsml";
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003FC9 File Offset: 0x000021C9
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00003FD1 File Offset: 0x000021D1
		[UIValue("searchValue")]
		public string SearchValue
		{
			get
			{
				return this._searchValue;
			}
			set
			{
				this._searchValue = value;
				base.NotifyPropertyChanged("SearchValue");
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003FE5 File Offset: 0x000021E5
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00003FED File Offset: 0x000021ED
		public bool Working
		{
			get
			{
				return this._working;
			}
			set
			{
				this._working = value;
				this._songsDownButton.interactable = !value;
				if (!this.loadingSpinner)
				{
					return;
				}
				this.SetLoading(value, 0.0, "");
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00004028 File Offset: 0x00002228
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004030 File Offset: 0x00002230
		internal bool MultiSelectEnabled
		{
			get
			{
				return this._multiSelectEnabled;
			}
			set
			{
				this._multiSelectEnabled = value;
				this.ToggleMultiSelect(value);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004040 File Offset: 0x00002240
		internal void ToggleMultiSelect(bool value)
		{
			this.MultiSelectClear();
			if (value)
			{
				this.customListTableData.tableView.selectionType = 2;
				this._sortButton.interactable = false;
				return;
			}
			this._sortButton.interactable = true;
			this.customListTableData.tableView.selectionType = 1;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004091 File Offset: 0x00002291
		internal void MultiSelectClear()
		{
			this.customListTableData.tableView.ClearSelection();
			this._multiSelectSongs.Clear();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000040B0 File Offset: 0x000022B0
		[UIAction("listSelect")]
		internal void Select(TableView tableView, int row)
		{
			if (this.MultiSelectEnabled && !this._multiSelectSongs.Any((Tuple<Beatmap, Texture2D> x) => x.Item1 == this._songs[row].Value))
			{
				TupleListExtensions.Add<Beatmap, Texture2D>(this._multiSelectSongs, this._songs[row].Value, this.customListTableData.data[row].icon);
			}
			Action<StrongBox<Beatmap>, Texture2D> action = this.didSelectSong;
			if (action == null)
			{
				return;
			}
			action(this._songs[row], this.customListTableData.data[row].icon);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000416A File Offset: 0x0000236A
		internal void SortClosed()
		{
			Plugin.log.Info("Sort modal closed");
			this._currentFilter = this._previousFilter;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004187 File Offset: 0x00002387
		[UIAction("sortPressed")]
		internal void SortPressed()
		{
			this.sourceListTableData.tableView.ClearSelection();
			this.sortListTableData.tableView.ClearSelection();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000041AC File Offset: 0x000023AC
		[UIAction("sortSelect")]
		internal async void SelectedSortOption(TableView tableView, int row)
		{
			SortFilter sortFilter = (this.sortListTableData.data[row] as SortFilterCellInfo).sortFilter;
			this._currentFilter = sortFilter.Mode;
			this._previousFilter = sortFilter.Mode;
			this._currentBeatSaverFilter = sortFilter.BeatSaverOption;
			this._currentScoreSaberFilter = sortFilter.ScoreSaberOption;
			this.parserParams.EmitEvent("close-sortModal");
			this.ClearData();
			Action action = this.filterDidChange;
			if (action != null)
			{
				action();
			}
			await this.GetNewPage(3U);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000041EC File Offset: 0x000023EC
		[UIAction("sourceSelect")]
		internal void SelectedSource(TableView tableView, int row)
		{
			this.parserParams.EmitEvent("close-sourceModal");
			MoreSongsListViewController.FilterMode filter = (this.sourceListTableData.data[row] as SourceCellInfo).filter;
			this._previousFilter = this._currentFilter;
			this._currentFilter = filter;
			this.SetupSortOptions();
			this.parserParams.EmitEvent("open-sortModal");
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004250 File Offset: 0x00002450
		[UIAction("searchPressed")]
		internal async void SearchPressed(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				this._currentSearch = text;
				this._currentFilter = MoreSongsListViewController.FilterMode.Search;
				this.ClearData();
				Action action = this.filterDidChange;
				if (action != null)
				{
					action();
				}
				await this.GetNewPage(3U);
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000428F File Offset: 0x0000248F
		[UIAction("abortClicked")]
		internal void AbortPageFetch()
		{
			this.cancellationTokenSource.Cancel();
			this.cancellationTokenSource.Dispose();
			this.cancellationTokenSource = new CancellationTokenSource();
			this.Working = false;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000042BC File Offset: 0x000024BC
		internal async void SortByUser(User user)
		{
			this._currentUploader = user;
			this._currentFilter = MoreSongsListViewController.FilterMode.BeatSaver;
			this._currentBeatSaverFilter = MoreSongsListViewController.BeatSaverFilterOptions.Uploader;
			this.ClearData();
			Action action = this.filterDidChange;
			if (action != null)
			{
				action();
			}
			await this.GetNewPage(3U);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00002053 File Offset: 0x00000253
		[UIAction("pageUpPressed")]
		internal void PageUpPressed()
		{
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000042FC File Offset: 0x000024FC
		[UIAction("pageDownPressed")]
		internal async void PageDownPressed()
		{
			if (this.customListTableData.data.Count >= 1 && !this._endOfResults && !this._multiSelectEnabled)
			{
				if (this.customListTableData.data.Count<CustomListTableData.CustomCellInfo>() - this.customListTableData.tableView.visibleCells.Last<TableCell>().idx <= 7)
				{
					await this.GetNewPage(4U);
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004333 File Offset: 0x00002533
		internal void SetSongListViewTip(string description)
		{
			if (this.songListViewTip)
			{
				this.songListViewTip.SetText(description);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004350 File Offset: 0x00002550
		internal void ClearData()
		{
			this.lastPage = 0U;
			this.customListTableData.data.Clear();
			this.customListTableData.tableView.ReloadData();
			this.customListTableData.tableView.ScrollToCellWithIdx(0, 0, false);
			this._songs.Clear();
			this._multiSelectSongs.Clear();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000043AD File Offset: 0x000025AD
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000043B6 File Offset: 0x000025B6
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			if (!firstActivation)
			{
				this.InitSongList();
				this.SetSongListViewTip("更多歌曲请到 https://beatsaberbbs.com");
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000043D4 File Offset: 0x000025D4
		[UIAction("#post-parse")]
		internal void SetupList()
		{
			(base.transform as RectTransform).sizeDelta = new Vector2(70f, 0f);
			(base.transform as RectTransform).anchorMin = new Vector2(0.5f, 0f);
			(base.transform as RectTransform).anchorMax = new Vector2(0.5f, 1f);
			this.loadingSpinner = Object.Instantiate<LoadingControl>(Resources.FindObjectsOfTypeAll<LoadingControl>().First<LoadingControl>(), this.loadingModal.transform);
			Object.Destroy(this.loadingSpinner.GetComponent<Touchable>());
			this.fetchProgress = new Progress<double>(new Action<double>(this.ProgressUpdate));
			this.SetupSourceOptions();
			this.InitSongList();
			this.SetSongListViewTip("更多歌曲请到 https://beatsaberbbs.com");
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000449C File Offset: 0x0000269C
		internal async void InitSongList()
		{
			await this.GetNewPage(3U);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000044D3 File Offset: 0x000026D3
		public void ProgressUpdate(double progress)
		{
			this.SetLoading(true, progress, this._fetchingDetails);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000044E3 File Offset: 0x000026E3
		public void SetLoading(bool value, double progress = 0.0, string details = "")
		{
			if (value)
			{
				this.parserParams.EmitEvent("open-loadingModal");
				this.loadingSpinner.ShowDownloadingProgress("歌曲获取中... " + details, (float)progress);
				return;
			}
			this.parserParams.EmitEvent("close-loadingModal");
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004524 File Offset: 0x00002724
		public void SetupSourceOptions()
		{
			this.sourceListTableData.data.Clear();
			this.sourceListTableData.data.Add(new SourceCellInfo(MoreSongsListViewController.FilterMode.BeatSaver, "BeatSaver", null, Sprites.BeatSaverIcon.texture));
			this.sourceListTableData.tableView.ReloadData();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004578 File Offset: 0x00002778
		public void SetupSortOptions()
		{
			this.sortListTableData.data.Clear();
			MoreSongsListViewController.FilterMode currentFilter = this._currentFilter;
			if (currentFilter != MoreSongsListViewController.FilterMode.BeatSaver)
			{
				if (currentFilter == MoreSongsListViewController.FilterMode.ScoreSaber)
				{
					this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.ScoreSaber, MoreSongsListViewController.BeatSaverFilterOptions.Latest, MoreSongsListViewController.ScoreSaberFilterOptions.Trending), "Trending", "ScoreSaber", Sprites.ScoreSaberIcon.texture));
					this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.ScoreSaber, MoreSongsListViewController.BeatSaverFilterOptions.Latest, MoreSongsListViewController.ScoreSaberFilterOptions.Ranked), "Ranked", "ScoreSaber", Sprites.ScoreSaberIcon.texture));
					this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.ScoreSaber, MoreSongsListViewController.BeatSaverFilterOptions.Latest, MoreSongsListViewController.ScoreSaberFilterOptions.Qualified), "Qualified", "ScoreSaber", Sprites.ScoreSaberIcon.texture));
					this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.ScoreSaber, MoreSongsListViewController.BeatSaverFilterOptions.Latest, MoreSongsListViewController.ScoreSaberFilterOptions.Loved), "Loved", "ScoreSaber", Sprites.ScoreSaberIcon.texture));
					this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.ScoreSaber, MoreSongsListViewController.BeatSaverFilterOptions.Latest, MoreSongsListViewController.ScoreSaberFilterOptions.Difficulty), "Difficulty", "ScoreSaber", Sprites.ScoreSaberIcon.texture));
					this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.ScoreSaber, MoreSongsListViewController.BeatSaverFilterOptions.Latest, MoreSongsListViewController.ScoreSaberFilterOptions.Plays), "Plays", "ScoreSaber", Sprites.ScoreSaberIcon.texture));
				}
			}
			else
			{
				this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.BeatSaver, MoreSongsListViewController.BeatSaverFilterOptions.Hot, MoreSongsListViewController.ScoreSaberFilterOptions.Trending), "Hot", "BeatSaver", Sprites.BeatSaverIcon.texture));
				this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.BeatSaver, MoreSongsListViewController.BeatSaverFilterOptions.Latest, MoreSongsListViewController.ScoreSaberFilterOptions.Trending), "Latest", "BeatSaver", Sprites.BeatSaverIcon.texture));
				this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.BeatSaver, MoreSongsListViewController.BeatSaverFilterOptions.Rating, MoreSongsListViewController.ScoreSaberFilterOptions.Trending), "Rating", "BeatSaver", Sprites.BeatSaverIcon.texture));
				this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.BeatSaver, MoreSongsListViewController.BeatSaverFilterOptions.Downloads, MoreSongsListViewController.ScoreSaberFilterOptions.Trending), "Downloads", "BeatSaver", Sprites.BeatSaverIcon.texture));
				this.sortListTableData.data.Add(new SortFilterCellInfo(new SortFilter(MoreSongsListViewController.FilterMode.BeatSaver, MoreSongsListViewController.BeatSaverFilterOptions.Plays, MoreSongsListViewController.ScoreSaberFilterOptions.Trending), "Plays", "BeatSaver", Sprites.BeatSaverIcon.texture));
			}
			this.sortListTableData.tableView.ReloadData();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000047DC File Offset: 0x000029DC
		public void Cleanup()
		{
			this.AbortPageFetch();
			this.parserParams.EmitEvent("closeAllModals");
			this.ClearData();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000047FC File Offset: 0x000029FC
		internal async Task GetNewPage(uint count = 1U)
		{
			if (!this.Working)
			{
				this._endOfResults = false;
				Plugin.log.Info(string.Format("Fetching {0} new page(s)", count));
				this.Working = true;
				try
				{
					MoreSongsListViewController.FilterMode currentFilter = this._currentFilter;
					if (currentFilter != MoreSongsListViewController.FilterMode.Search)
					{
						if (currentFilter == MoreSongsListViewController.FilterMode.BeatSaver)
						{
							await this.GetPagesBeatSaver(count);
						}
					}
					else
					{
						await this.GetPagesSearch(count);
					}
				}
				catch (Exception ex)
				{
					if (ex is TaskCanceledException)
					{
						Plugin.log.Warn("Page Fetching Aborted.");
					}
					else
					{
						Plugin.log.Critical("Failed to fetch new pages! \n" + ((ex != null) ? ex.ToString() : null));
					}
				}
				this.Working = false;
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004848 File Offset: 0x00002A48
		internal async Task GetPagesBeatSaver(uint count)
		{
			List<Beatmap> newMaps = new List<Beatmap>();
			for (uint i = 0U; i < count; i += 1U)
			{
				this._fetchingDetails = string.Format("({0}/{1})", i + 1U, count);
				Page page = null;
				switch (this._currentBeatSaverFilter)
				{
				case MoreSongsListViewController.BeatSaverFilterOptions.Latest:
					page = await Plugin.BeatSaver.Latest(this.lastPage, this.cancellationTokenSource.Token, this.fetchProgress);
					break;
				case MoreSongsListViewController.BeatSaverFilterOptions.Hot:
					page = await Plugin.BeatSaver.Hot(this.lastPage, this.cancellationTokenSource.Token, this.fetchProgress);
					break;
				case MoreSongsListViewController.BeatSaverFilterOptions.Rating:
					page = await Plugin.BeatSaver.Rating(this.lastPage, this.cancellationTokenSource.Token, this.fetchProgress);
					break;
				case MoreSongsListViewController.BeatSaverFilterOptions.Downloads:
					page = await Plugin.BeatSaver.Downloads(this.lastPage, this.cancellationTokenSource.Token, this.fetchProgress);
					break;
				case MoreSongsListViewController.BeatSaverFilterOptions.Plays:
					page = await Plugin.BeatSaver.Plays(this.lastPage, this.cancellationTokenSource.Token, this.fetchProgress);
					break;
				case MoreSongsListViewController.BeatSaverFilterOptions.Uploader:
					page = await this._currentUploader.Beatmaps(this.lastPage, this.cancellationTokenSource.Token, this.fetchProgress);
					break;
				}
				this.lastPage += 1U;
				if (page.TotalDocs == 0 || page.NextPage == null)
				{
					this._endOfResults = true;
				}
				if (page.Docs != null)
				{
					newMaps.AddRange(page.Docs);
				}
				if (this._endOfResults)
				{
					break;
				}
			}
			newMaps.ForEach(delegate(Beatmap x)
			{
				this._songs.Add(new StrongBox<Beatmap>(x));
			});
			foreach (Beatmap beatmap in newMaps)
			{
				if (SongDownloader.Instance.IsSongDownloaded(beatmap.Hash))
				{
					this.customListTableData.data.Add(new BeatSaverCustomSongCellInfo(beatmap, new Action<CustomListTableData.CustomCellInfo>(this.CellDidSetImage), "<#7F7F7F>" + beatmap.Name, beatmap.Uploader.Username));
				}
				else
				{
					this.customListTableData.data.Add(new BeatSaverCustomSongCellInfo(beatmap, new Action<CustomListTableData.CustomCellInfo>(this.CellDidSetImage), beatmap.Name, beatmap.Uploader.Username));
				}
				this.customListTableData.tableView.ReloadData();
			}
			this._fetchingDetails = "";
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004894 File Offset: 0x00002A94
		internal async Task GetPagesSearch(uint count)
		{
			if (this._currentSearch.StartsWith("Key:"))
			{
				string text = this._currentSearch.Split(new char[] { ':' })[1];
				this._fetchingDetails = " (By Key:" + text;
				Beatmap beatmap = await Plugin.BeatSaver.Key(text, this.fetchProgress);
				Beatmap keyMap = beatmap;
				if (keyMap != null && !this._songs.Any((StrongBox<Beatmap> x) => x.Value == keyMap))
				{
					this._songs.Add(new StrongBox<Beatmap>(keyMap));
					if (SongDownloader.Instance.IsSongDownloaded(keyMap.Hash))
					{
						this.customListTableData.data.Add(new BeatSaverCustomSongCellInfo(keyMap, new Action<CustomListTableData.CustomCellInfo>(this.CellDidSetImage), "<#7F7F7F>" + keyMap.Name, keyMap.Uploader.Username));
					}
					else
					{
						this.customListTableData.data.Add(new BeatSaverCustomSongCellInfo(keyMap, new Action<CustomListTableData.CustomCellInfo>(this.CellDidSetImage), keyMap.Name, keyMap.Uploader.Username));
					}
					this.customListTableData.tableView.ReloadData();
				}
				this._fetchingDetails = "";
			}
			else
			{
				List<Beatmap> newMaps = new List<Beatmap>();
				for (uint i = 0U; i < count; i += 1U)
				{
					this._fetchingDetails = string.Format("({0}/{1})", i + 1U, count);
					Page page = await Plugin.BeatSaver.Search(this._currentSearch, this.lastPage, this.cancellationTokenSource.Token, this.fetchProgress);
					this.lastPage += 1U;
					if (page.TotalDocs == 0 || page.NextPage == null)
					{
						this._endOfResults = true;
					}
					if (page.Docs != null)
					{
						newMaps.AddRange(page.Docs);
					}
					if (this._endOfResults)
					{
						break;
					}
				}
				newMaps.ForEach(delegate(Beatmap x)
				{
					this._songs.Add(new StrongBox<Beatmap>(x));
				});
				foreach (Beatmap beatmap2 in newMaps)
				{
					if (SongDownloader.Instance.IsSongDownloaded(beatmap2.Hash))
					{
						this.customListTableData.data.Add(new BeatSaverCustomSongCellInfo(beatmap2, new Action<CustomListTableData.CustomCellInfo>(this.CellDidSetImage), "<#7F7F7F>" + beatmap2.Name, beatmap2.Uploader.Username));
					}
					else
					{
						this.customListTableData.data.Add(new BeatSaverCustomSongCellInfo(beatmap2, new Action<CustomListTableData.CustomCellInfo>(this.CellDidSetImage), beatmap2.Name, beatmap2.Uploader.Username));
					}
					this.customListTableData.tableView.ReloadData();
				}
				this._fetchingDetails = "";
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000048E0 File Offset: 0x00002AE0
		internal void CellDidSetImage(CustomListTableData.CustomCellInfo cell)
		{
			foreach (TableCell tableCell in this.customListTableData.tableView.visibleCells)
			{
				TextMeshProUGUI field = (tableCell as LevelListTableCell).GetField("_songNameText");
				if (((field != null) ? field.text : null) == cell.text)
				{
					this.customListTableData.tableView.RefreshCellsContent();
					break;
				}
			}
		}

		// Token: 0x0400002E RID: 46
		private const string pageTip = "更多歌曲请到 https://beatsaberbbs.com";

		// Token: 0x0400002F RID: 47
		internal MoreSongsListViewController.FilterMode _currentFilter = MoreSongsListViewController.FilterMode.BeatSaver;

		// Token: 0x04000030 RID: 48
		private MoreSongsListViewController.FilterMode _previousFilter = MoreSongsListViewController.FilterMode.BeatSaver;

		// Token: 0x04000031 RID: 49
		internal MoreSongsListViewController.BeatSaverFilterOptions _currentBeatSaverFilter = MoreSongsListViewController.BeatSaverFilterOptions.Hot;

		// Token: 0x04000032 RID: 50
		internal MoreSongsListViewController.ScoreSaberFilterOptions _currentScoreSaberFilter;

		// Token: 0x04000033 RID: 51
		private User _currentUploader;

		// Token: 0x04000034 RID: 52
		private string _currentSearch;

		// Token: 0x04000035 RID: 53
		private string _fetchingDetails = "";

		// Token: 0x04000036 RID: 54
		internal NavigationController navController;

		// Token: 0x04000037 RID: 55
		internal CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04000038 RID: 56
		[UIParams]
		internal BSMLParserParams parserParams;

		// Token: 0x04000039 RID: 57
		[UIComponent("list")]
		public CustomListTableData customListTableData;

		// Token: 0x0400003A RID: 58
		[UIComponent("sortList")]
		public CustomListTableData sortListTableData;

		// Token: 0x0400003B RID: 59
		[UIComponent("sourceList")]
		public CustomListTableData sourceListTableData;

		// Token: 0x0400003C RID: 60
		[UIComponent("loadingModal")]
		public ModalView loadingModal;

		// Token: 0x0400003D RID: 61
		[UIComponent("sortModal")]
		private Button _sortButton;

		// Token: 0x0400003E RID: 62
		private string _searchValue = "";

		// Token: 0x0400003F RID: 63
		private List<StrongBox<Beatmap>> _songs = new List<StrongBox<Beatmap>>();

		// Token: 0x04000040 RID: 64
		public List<Tuple<Beatmap, Texture2D>> _multiSelectSongs = new List<Tuple<Beatmap, Texture2D>>();

		// Token: 0x04000041 RID: 65
		public LoadingControl loadingSpinner;

		// Token: 0x04000042 RID: 66
		internal Progress<double> fetchProgress;

		// Token: 0x04000043 RID: 67
		public Action<StrongBox<Beatmap>, Texture2D> didSelectSong;

		// Token: 0x04000044 RID: 68
		public Action filterDidChange;

		// Token: 0x04000045 RID: 69
		public Action multiSelectDidChange;

		// Token: 0x04000046 RID: 70
		private bool _working;

		// Token: 0x04000047 RID: 71
		private uint lastPage;

		// Token: 0x04000048 RID: 72
		private bool _endOfResults;

		// Token: 0x04000049 RID: 73
		private bool _multiSelectEnabled;

		// Token: 0x0400004A RID: 74
		[UIComponent("songsPageDown")]
		private Button _songsDownButton;

		// Token: 0x0400004B RID: 75
		[UIComponent("songListViewTip")]
		private TextMeshProUGUI songListViewTip;

		// Token: 0x02000031 RID: 49
		public enum FilterMode
		{
			// Token: 0x040000C3 RID: 195
			Search,
			// Token: 0x040000C4 RID: 196
			BeatSaver,
			// Token: 0x040000C5 RID: 197
			ScoreSaber
		}

		// Token: 0x02000032 RID: 50
		public enum BeatSaverFilterOptions
		{
			// Token: 0x040000C7 RID: 199
			Latest,
			// Token: 0x040000C8 RID: 200
			Hot,
			// Token: 0x040000C9 RID: 201
			Rating,
			// Token: 0x040000CA RID: 202
			Downloads,
			// Token: 0x040000CB RID: 203
			Plays,
			// Token: 0x040000CC RID: 204
			Uploader
		}

		// Token: 0x02000033 RID: 51
		public enum ScoreSaberFilterOptions
		{
			// Token: 0x040000CE RID: 206
			Trending,
			// Token: 0x040000CF RID: 207
			Ranked,
			// Token: 0x040000D0 RID: 208
			Difficulty,
			// Token: 0x040000D1 RID: 209
			Qualified,
			// Token: 0x040000D2 RID: 210
			Loved,
			// Token: 0x040000D3 RID: 211
			Plays
		}
	}
}
