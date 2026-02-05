using System;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x02000097 RID: 151
	public class BSMLTableView : TableView
	{
		// Token: 0x0600030A RID: 778 RVA: 0x0000F094 File Offset: 0x0000D294
		public override void ReloadData()
		{
			base.ReloadData();
			if (base.tableType == 1)
			{
				base.contentTransform.anchorMin = new Vector2(0f, 0f);
				base.contentTransform.anchorMax = new Vector2(1f, 1f);
				base.contentTransform.sizeDelta = new Vector2(0f, 0f);
			}
		}
	}
}
