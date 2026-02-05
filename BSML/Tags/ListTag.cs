using System;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200004F RID: 79
	public class ListTag : BSMLTag
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00009A3C File Offset: 0x00007C3C
		public override string[] Aliases
		{
			get
			{
				return new string[] { "list" };
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00009A4C File Offset: 0x00007C4C
		public override GameObject CreateObject(Transform parent)
		{
			RectTransform rectTransform = new GameObject("BSMLListContainer", new Type[] { typeof(RectTransform) }).transform as RectTransform;
			rectTransform.gameObject.AddComponent<LayoutElement>();
			rectTransform.SetParent(parent, false);
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLList";
			gameObject.SetActive(false);
			TableView tableView = gameObject.AddComponent<BSMLTableView>();
			CustomListTableData customListTableData = rectTransform.gameObject.AddComponent<CustomListTableData>();
			customListTableData.tableView = tableView;
			tableView.transform.SetParent(rectTransform, false);
			tableView.SetField("_preallocatedCells", new TableView.CellsGroup[0]);
			tableView.SetField("_isInitialized", false);
			RectTransform rectTransform2 = new GameObject("Viewport").AddComponent<RectTransform>();
			rectTransform2.SetParent(gameObject.GetComponent<RectTransform>(), false);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			gameObject.GetComponent<ScrollRect>().viewport = rectTransform2;
			(rectTransform2.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
			(rectTransform2.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
			(rectTransform2.transform as RectTransform).sizeDelta = new Vector2(0f, 0f);
			(rectTransform2.transform as RectTransform).anchoredPosition = new Vector3(0f, 0f);
			(tableView.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
			(tableView.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
			(tableView.transform as RectTransform).sizeDelta = new Vector2(0f, 0f);
			(tableView.transform as RectTransform).anchoredPosition = new Vector3(0f, 0f);
			tableView.dataSource = customListTableData;
			return rectTransform.gameObject;
		}
	}
}
