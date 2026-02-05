using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Data;
using HMUI;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x0200005B RID: 91
	[ViewDefinition("BeatSaberMultiplayer.UI.ViewControllers.RoomView.RequestsViewController.bsml")]
	internal class RequestsViewController : BSMLAutomaticViewController, TableView.IDataSource
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000754 RID: 1876 RVA: 0x0001EDF8 File Offset: 0x0001CFF8
		// (remove) Token: 0x06000755 RID: 1877 RVA: 0x0001EE30 File Offset: 0x0001D030
		public event Action BackPressed;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000756 RID: 1878 RVA: 0x0001EE68 File Offset: 0x0001D068
		// (remove) Token: 0x06000757 RID: 1879 RVA: 0x0001EEA0 File Offset: 0x0001D0A0
		public event Action<SongCfg> SongSelected;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000758 RID: 1880 RVA: 0x0001EED8 File Offset: 0x0001D0D8
		// (remove) Token: 0x06000759 RID: 1881 RVA: 0x0001EF10 File Offset: 0x0001D110
		public event Action<SongCfg> RemovePressed;

		// Token: 0x0600075A RID: 1882 RVA: 0x0001EF45 File Offset: 0x0001D145
		public TableCell CellForIdx(TableView tableView, int idx)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001E373 File Offset: 0x0001C573
		public float CellSize()
		{
			return 10f;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001EF4C File Offset: 0x0001D14C
		public int NumberOfCells()
		{
			return this.requestedSongs.Count;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000196A0 File Offset: 0x000178A0
		public void SetSongs(List<SongCfg> songs)
		{
		}

		// Token: 0x040003A4 RID: 932
		private List<SongCfg> requestedSongs = new List<SongCfg>();
	}
}
