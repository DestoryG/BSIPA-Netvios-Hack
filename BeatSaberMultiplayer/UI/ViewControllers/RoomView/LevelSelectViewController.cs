using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Helper;
using BeatSaberMultiplayer.Interop;
using BeatSaberMultiplayer.UI.FlowCoordinators;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000058 RID: 88
	[ViewDefinition("BeatSaberMultiplayer.UI.ViewControllers.RoomView.LevelSelectViewController.bsml")]
	internal class LevelSelectViewController : BSMLAutomaticViewController, TableView.IDataSource
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600071C RID: 1820 RVA: 0x0001DF20 File Offset: 0x0001C120
		// (remove) Token: 0x0600071D RID: 1821 RVA: 0x0001DF58 File Offset: 0x0001C158
		public event Action<IPreviewBeatmapLevel, Texture2D> SongSelectedEvent;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x0600071E RID: 1822 RVA: 0x0001DF90 File Offset: 0x0001C190
		// (remove) Token: 0x0600071F RID: 1823 RVA: 0x0001DFC8 File Offset: 0x0001C1C8
		public event Action<string> SearchPressedEvent;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000720 RID: 1824 RVA: 0x0001E000 File Offset: 0x0001C200
		// (remove) Token: 0x06000721 RID: 1825 RVA: 0x0001E038 File Offset: 0x0001C238
		public event Action<SortMode> SortPressedEvent;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000722 RID: 1826 RVA: 0x0001E070 File Offset: 0x0001C270
		// (remove) Token: 0x06000723 RID: 1827 RVA: 0x0001E0A8 File Offset: 0x0001C2A8
		public event Action RequestModePressedEvent;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000724 RID: 1828 RVA: 0x0001E0E0 File Offset: 0x0001C2E0
		// (remove) Token: 0x06000725 RID: 1829 RVA: 0x0001E118 File Offset: 0x0001C318
		public event Action RandomLevelPressedEvent;

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001E14D File Offset: 0x0001C34D
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001E155 File Offset: 0x0001C355
		public RoomFlowCoordinator ParentFlowCoordinator { get; internal set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001E15E File Offset: 0x0001C35E
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001E166 File Offset: 0x0001C366
		[UIValue("more-btn-active")]
		private bool MoreSongsAvailable
		{
			get
			{
				return this._moreSongsAvailable;
			}
			set
			{
				if (this._moreSongsAvailable == value)
				{
					return;
				}
				this._moreSongsAvailable = value;
				base.NotifyPropertyChanged("MoreSongsAvailable");
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001E184 File Offset: 0x0001C384
		public unsafe TableViewScroller SongListScroller
		{
			get
			{
				if (this._songListScroller == null)
				{
					this._songListScroller = *this.GetTableViewScroller(ref this._songsTableView.tableView);
					this._songListScroller.positionDidChangeEvent += this.SongListScrollerPositionDidChangeEvent;
				}
				return this._songListScroller;
			}
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001E1DC File Offset: 0x0001C3DC
		private void SelectTopButtons(TopButtonsState _newState)
		{
			switch (_newState)
			{
			case TopButtonsState.Select:
				this._selectBtnsRect.gameObject.SetActive(true);
				return;
			case TopButtonsState.SortBy:
				this._selectBtnsRect.gameObject.SetActive(false);
				this._diffButton.interactable = ScrappedData.Downloaded;
				return;
			case TopButtonsState.Search:
				this._selectBtnsRect.gameObject.SetActive(false);
				return;
			case TopButtonsState.Mode:
				this._selectBtnsRect.gameObject.SetActive(false);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001E258 File Offset: 0x0001C458
		public TableCell CellForIdx(TableView tableView, int idx)
		{
			LevelListTableCell levelListTableCell = (LevelListTableCell)tableView.DequeueReusableCellForIdentifier(this._songsTableView.reuseIdentifier);
			if (!levelListTableCell)
			{
				if (this._songListTableCellInstance == null)
				{
					this._songListTableCellInstance = Resources.FindObjectsOfTypeAll<LevelListTableCell>().First((LevelListTableCell x) => x.name == "LevelListTableCell");
				}
				levelListTableCell = Object.Instantiate<LevelListTableCell>(this._songListTableCellInstance);
			}
			levelListTableCell.SetDataFromLevelAsync(this._availableSongs[idx], this._playerDataModel.playerData.favoritesLevelIds.Contains(this._availableSongs[idx].levelID));
			levelListTableCell.RefreshAvailabilityAsync(this._additionalContentModel, this._availableSongs[idx].levelID);
			levelListTableCell.reuseIdentifier = this._songsTableView.reuseIdentifier;
			return levelListTableCell;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001E334 File Offset: 0x0001C534
		private async void SongsTableView_DidSelectRow(TableView sender, int row)
		{
			if (Client.Instance.isHost)
			{
				CancellationToken cancellationToken = default(CancellationToken);
				Texture2D texture2D = await this._availableSongs[row].GetCoverImageTexture2DAsync(cancellationToken);
				Action<IPreviewBeatmapLevel, Texture2D> songSelectedEvent = this.SongSelectedEvent;
				if (songSelectedEvent != null)
				{
					songSelectedEvent(this._availableSongs[row], texture2D);
				}
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001E373 File Offset: 0x0001C573
		public float CellSize()
		{
			return 10f;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001E37A File Offset: 0x0001C57A
		public int NumberOfCells()
		{
			return this._availableSongs.Count;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001E388 File Offset: 0x0001C588
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			if (firstActivation)
			{
				if (Plugin.DownloaderExists)
				{
					this._downloader = new BeatSaverDownloaderInterop();
					if (this._downloader == null)
					{
						Logger.log.Warn("BeatSaverDownloaderInterop could not be created.");
					}
					else
					{
						this.MoreSongsAvailable = this._downloader.CanCreate;
						Logger.log.Debug(string.Format("{0} is {1}", "MoreSongsAvailable", this.MoreSongsAvailable));
					}
				}
				else
				{
					Logger.log.Warn("BeatSaverDownloader not found, More Songs button won't be created.");
					this.MoreSongsAvailable = false;
				}
				this._songsTableView.tableView.dataSource = this;
				this._songsTableView.tableView.didSelectCellWithIdxEvent += this.SongsTableView_DidSelectRow;
				this._playerDataModel = Resources.FindObjectsOfTypeAll<PlayerDataModel>().First<PlayerDataModel>();
				this._additionalContentModel = Resources.FindObjectsOfTypeAll<AdditionalContentModel>().First<AdditionalContentModel>();
			}
			this.SelectTopButtons(TopButtonsState.Select);
			this._selectBtnsRect.gameObject.SetActive(Client.Instance.isHost);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001E489 File Offset: 0x0001C689
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001E492 File Offset: 0x0001C692
		public void SetSongs(List<IPreviewBeatmapLevel> levels)
		{
			this._availableSongs = levels;
			this.UpdateViewController();
			this._selectBtnsRect.gameObject.SetActive(Client.Instance.isHost);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001E4BC File Offset: 0x0001C6BC
		public void ScrollToLevel(string levelId)
		{
			if (string.IsNullOrEmpty(levelId))
			{
				return;
			}
			if (this._availableSongs.Any((IPreviewBeatmapLevel x) => x.levelID == levelId))
			{
				int num = this._availableSongs.FindIndex((IPreviewBeatmapLevel x) => x.levelID == levelId);
				this._songsTableView.tableView.ScrollToCellWithIdx(num, 0, false);
				this._songsTableView.tableView.SelectCellWithIdx(num, false);
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001E53C File Offset: 0x0001C73C
		public unsafe bool ScrollToPosition(float position)
		{
			TableViewScroller songListScroller = this.SongListScroller;
			if (position >= 0f && position <= songListScroller.scrollableSize)
			{
				float num = 0.01f;
				if (position + num > songListScroller.scrollableSize)
				{
					num = -0.01f;
				}
				if (position + num < 0f)
				{
					num = 0f;
				}
				*this.GetTableViewScrollerTargetPosition(ref songListScroller) = position + num;
				this.SetScrollPosition(ref songListScroller, position);
				songListScroller.enabled = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001E5B1 File Offset: 0x0001C7B1
		private void SongListScrollerPositionDidChangeEvent(TableViewScroller arg1, float arg2)
		{
			this.ParentFlowCoordinator.LastScrollPosition = arg2;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001E5C0 File Offset: 0x0001C7C0
		private void UpdateViewController()
		{
			this._songsTableView.tableView.ReloadData();
			if (Client.Instance.isHost)
			{
				return;
			}
			int num = 0;
			this.ScrollToLevel(this._availableSongs[num].levelID);
			this._songsTableView.tableView.ScrollToCellWithIdx(num, 0, false);
			this._songsTableView.tableView.SelectCellWithIdx(num, false);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001E628 File Offset: 0x0001C828
		[UIAction("fast-scroll-down-pressed")]
		private void FastScrollDown()
		{
			for (int i = 0; i < 5; i++)
			{
				this._parserParams.EmitEvent("song-list#PageDown");
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001E654 File Offset: 0x0001C854
		[UIAction("fast-scroll-up-pressed")]
		private void FastScrollUp()
		{
			for (int i = 0; i < 5; i++)
			{
				this._parserParams.EmitEvent("song-list#PageUp");
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001E67D File Offset: 0x0001C87D
		[UIAction("more-btn-pressed")]
		private void MoreButtonPressed()
		{
			BeatSaverDownloaderInterop downloader = this._downloader;
			if (downloader == null)
			{
				return;
			}
			downloader.PresentDownloaderFlowCoordinator(this.ParentFlowCoordinator, null);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001E698 File Offset: 0x0001C898
		[UIAction("search-btn-pressed")]
		private void SearchButtonPressed()
		{
			this.SelectTopButtons(TopButtonsState.Select);
			this._searchKeyboard.modalView.Show(true, false, null);
			this.keyboardOriginPostion = this._searchKeyboard.modalView.transform.position;
			this._searchKeyboard.modalView.transform.position = new Vector3(this.keyboardOriginPostion.x + 1.4f, this.keyboardOriginPostion.y, this.keyboardOriginPostion.z);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001E71C File Offset: 0x0001C91C
		[UIAction("search-pressed")]
		private void SearchStartPressed(string value)
		{
			this.SelectTopButtons(TopButtonsState.Select);
			this._searchKeyboard.modalView.Hide(true, null);
			this._searchKeyboard.modalView.transform.position = this.keyboardOriginPostion;
			Action<string> searchPressedEvent = this.SearchPressedEvent;
			if (searchPressedEvent == null)
			{
				return;
			}
			searchPressedEvent(value);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001E76E File Offset: 0x0001C96E
		[UIAction("random-btn-pressed")]
		private void RandomLevelPressed()
		{
			Action randomLevelPressedEvent = this.RandomLevelPressedEvent;
			if (randomLevelPressedEvent == null)
			{
				return;
			}
			randomLevelPressedEvent();
		}

		// Token: 0x0400037F RID: 895
		private PropertyAccessor<TableViewScroller, float>.Setter SetScrollPosition = PropertyAccessor<TableViewScroller, float>.GetSetter("position");

		// Token: 0x04000380 RID: 896
		private FieldAccessor<TableViewScroller, float>.Accessor GetTableViewScrollerTargetPosition = FieldAccessor<TableViewScroller, float>.GetAccessor("_targetPosition");

		// Token: 0x04000381 RID: 897
		private FieldAccessor<TableView, TableViewScroller>.Accessor GetTableViewScroller = FieldAccessor<TableView, TableViewScroller>.GetAccessor("_scroller");

		// Token: 0x04000388 RID: 904
		private BeatSaverDownloaderInterop _downloader;

		// Token: 0x04000389 RID: 905
		private LevelListTableCell _songListTableCellInstance;

		// Token: 0x0400038A RID: 906
		private PlayerDataModel _playerDataModel;

		// Token: 0x0400038B RID: 907
		private AdditionalContentModel _additionalContentModel;

		// Token: 0x0400038C RID: 908
		private Vector3 keyboardOriginPostion;

		// Token: 0x0400038D RID: 909
		private List<IPreviewBeatmapLevel> _availableSongs = new List<IPreviewBeatmapLevel>();

		// Token: 0x0400038E RID: 910
		[UIParams]
		private BSMLParserParams _parserParams;

		// Token: 0x0400038F RID: 911
		[UIComponent("song-list")]
		private CustomListTableData _songsTableView;

		// Token: 0x04000390 RID: 912
		[UIComponent("song-list-rect")]
		private RectTransform _songListRect;

		// Token: 0x04000391 RID: 913
		[UIComponent("select-btns-rect")]
		private RectTransform _selectBtnsRect;

		// Token: 0x04000392 RID: 914
		[UIComponent("search-keyboard")]
		private ModalKeyboard _searchKeyboard;

		// Token: 0x04000393 RID: 915
		[UIComponent("diff-sort-btn")]
		private Button _diffButton;

		// Token: 0x04000394 RID: 916
		[UIValue("search-value")]
		private string searchValue;

		// Token: 0x04000395 RID: 917
		private bool _moreSongsAvailable = true;

		// Token: 0x04000396 RID: 918
		private TableViewScroller _songListScroller;
	}
}
