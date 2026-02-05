using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x02000016 RID: 22
	public class SourceCellInfo : CustomListTableData.CustomCellInfo
	{
		// Token: 0x06000107 RID: 263 RVA: 0x00004A1C File Offset: 0x00002C1C
		public SourceCellInfo(MoreSongsListViewController.FilterMode filter, string text, string subtext = null, Texture2D icon = null)
			: base(text, subtext, icon)
		{
			this.filter = filter;
		}

		// Token: 0x04000050 RID: 80
		public MoreSongsListViewController.FilterMode filter;
	}
}
