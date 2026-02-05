using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x02000015 RID: 21
	public class SortFilterCellInfo : CustomListTableData.CustomCellInfo
	{
		// Token: 0x06000106 RID: 262 RVA: 0x00004A09 File Offset: 0x00002C09
		public SortFilterCellInfo(SortFilter filter, string text, string subtext = null, Texture2D icon = null)
			: base(text, subtext, icon)
		{
			this.sortFilter = filter;
		}

		// Token: 0x0400004F RID: 79
		public SortFilter sortFilter;
	}
}
