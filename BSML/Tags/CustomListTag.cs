using System;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000044 RID: 68
	public class CustomListTag : BSMLTag
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00008E20 File Offset: 0x00007020
		public override string[] Aliases
		{
			get
			{
				return new string[] { "custom-list" };
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00008E30 File Offset: 0x00007030
		public override bool AddChildren
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00008E34 File Offset: 0x00007034
		public override GameObject CreateObject(Transform parent)
		{
			RectTransform rectTransform = new GameObject("BSMLCustomListContainer", new Type[] { typeof(RectTransform) }).transform as RectTransform;
			rectTransform.gameObject.AddComponent<LayoutElement>();
			rectTransform.SetParent(parent, false);
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLCustomList";
			gameObject.SetActive(false);
			TableView tableView = gameObject.AddComponent<BSMLTableView>();
			CustomCellListTableData customCellListTableData = rectTransform.gameObject.AddComponent<CustomCellListTableData>();
			customCellListTableData.tableView = tableView;
			gameObject.AddComponent<RectMask2D>();
			tableView.transform.SetParent(rectTransform, false);
			tableView.SetField("_preallocatedCells", new TableView.CellsGroup[0]);
			tableView.SetField("_isInitialized", false);
			RectTransform rectTransform2 = new GameObject("Viewport").AddComponent<RectTransform>();
			rectTransform2.SetParent(gameObject.GetComponent<RectTransform>(), false);
			gameObject.GetComponent<ScrollRect>().viewport = rectTransform2;
			(rectTransform2.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
			(rectTransform2.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
			(rectTransform2.transform as RectTransform).sizeDelta = new Vector2(0f, 0f);
			(rectTransform2.transform as RectTransform).anchoredPosition = new Vector3(0f, 0f);
			(tableView.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
			(tableView.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
			(tableView.transform as RectTransform).sizeDelta = new Vector2(0f, 0f);
			(tableView.transform as RectTransform).anchoredPosition = new Vector3(0f, 0f);
			tableView.dataSource = customCellListTableData;
			return rectTransform.gameObject;
		}
	}
}
