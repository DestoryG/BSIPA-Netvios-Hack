using System;
using System.Collections.Generic;
using System.Linq;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x0200009C RID: 156
	public class CustomCellListTableData : MonoBehaviour, TableView.IDataSource
	{
		// Token: 0x06000326 RID: 806 RVA: 0x0000F354 File Offset: 0x0000D554
		public virtual TableCell CellForIdx(TableView tableView, int idx)
		{
			CustomCellTableCell customCellTableCell = new GameObject().AddComponent<CustomCellTableCell>();
			if (this.clickableCells)
			{
				customCellTableCell.gameObject.AddComponent<Touchable>();
				customCellTableCell.interactable = true;
			}
			customCellTableCell.reuseIdentifier = "BSMLCustomCellListCell";
			customCellTableCell.name = "BSMLCustomTableCell";
			customCellTableCell.parserParams = PersistentSingleton<BSMLParser>.instance.Parse(this.cellTemplate, customCellTableCell.gameObject, this.data[idx]);
			customCellTableCell.SetupPostParse();
			return customCellTableCell;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000F3CC File Offset: 0x0000D5CC
		public float CellSize()
		{
			return this.cellSize;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000F3D4 File Offset: 0x0000D5D4
		public int NumberOfCells()
		{
			return this.data.Count<object>();
		}

		// Token: 0x040000C5 RID: 197
		public List<object> data = new List<object>();

		// Token: 0x040000C6 RID: 198
		public string cellTemplate;

		// Token: 0x040000C7 RID: 199
		public float cellSize = 8.5f;

		// Token: 0x040000C8 RID: 200
		public TableView tableView;

		// Token: 0x040000C9 RID: 201
		public bool clickableCells = true;
	}
}
