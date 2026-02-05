using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200001D RID: 29
	[ComponentHandler(typeof(CustomListTableData))]
	public class CustomListTableDataHandler : TypeHandler
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005A3C File Offset: 0x00003C3C
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"selectCell",
						new string[] { "select-cell" }
					},
					{
						"visibleCells",
						new string[] { "visible-cells" }
					},
					{
						"cellSize",
						new string[] { "cell-size" }
					},
					{
						"id",
						new string[] { "id" }
					},
					{
						"data",
						new string[] { "data", "content" }
					},
					{
						"listWidth",
						new string[] { "list-width" }
					},
					{
						"listHeight",
						new string[] { "list-height" }
					},
					{
						"expandCell",
						new string[] { "expand-cell" }
					},
					{
						"listStyle",
						new string[] { "list-style" }
					},
					{
						"listDirection",
						new string[] { "list-direction" }
					},
					{
						"alignCenter",
						new string[] { "align-to-center" }
					}
				};
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005B6C File Offset: 0x00003D6C
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			CustomListTableData customListTableData = componentType.component as CustomListTableData;
			string selectCell;
			if (componentType.data.TryGetValue("selectCell", out selectCell))
			{
				customListTableData.tableView.didSelectCellWithIdxEvent += delegate(TableView table, int index)
				{
					BSMLAction bsmlaction;
					if (!parserParams.actions.TryGetValue(selectCell, out bsmlaction))
					{
						throw new Exception("select-cell action '" + componentType.data["onClick"] + "' not found");
					}
					bsmlaction.Invoke(new object[] { table, index });
				};
			}
			string text;
			if (componentType.data.TryGetValue("listDirection", out text))
			{
				customListTableData.tableView.SetField("_tableType", (TableView.TableType)Enum.Parse(typeof(TableView.TableType), text));
			}
			string text2;
			if (componentType.data.TryGetValue("listStyle", out text2))
			{
				customListTableData.Style = (CustomListTableData.ListStyle)Enum.Parse(typeof(CustomListTableData.ListStyle), text2);
			}
			string text3;
			if (componentType.data.TryGetValue("cellSize", out text3))
			{
				customListTableData.cellSize = Parse.Float(text3);
			}
			string text4;
			if (componentType.data.TryGetValue("expandCell", out text4))
			{
				customListTableData.expandCell = Parse.Bool(text4);
			}
			string text5;
			if (componentType.data.TryGetValue("alignCenter", out text5))
			{
				customListTableData.tableView.SetField("_alignToCenter", Parse.Bool(text5));
			}
			string text6;
			if (componentType.data.TryGetValue("data", out text6))
			{
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text6, out bsmlvalue))
				{
					throw new Exception("value '" + text6 + "' not found");
				}
				customListTableData.data = bsmlvalue.GetValue() as List<CustomListTableData.CustomCellInfo>;
				customListTableData.tableView.ReloadData();
			}
			TableView.TableType tableType = customListTableData.tableView.tableType;
			if (tableType != null)
			{
				if (tableType == 1)
				{
					string text7;
					string text8;
					(componentType.component.gameObject.transform as RectTransform).sizeDelta = new Vector2(customListTableData.cellSize * (componentType.data.TryGetValue("visibleCells", out text7) ? Parse.Float(text7) : 4f), componentType.data.TryGetValue("listHeight", out text8) ? Parse.Float(text8) : 40f);
				}
			}
			else
			{
				string text9;
				string text10;
				(componentType.component.gameObject.transform as RectTransform).sizeDelta = new Vector2(componentType.data.TryGetValue("listWidth", out text9) ? Parse.Float(text9) : 60f, customListTableData.cellSize * (componentType.data.TryGetValue("visibleCells", out text10) ? Parse.Float(text10) : 7f));
				customListTableData.tableView.contentTransform.anchorMin = new Vector2(0f, 1f);
			}
			componentType.component.gameObject.GetComponent<LayoutElement>().preferredHeight = (componentType.component.gameObject.transform as RectTransform).sizeDelta.y;
			componentType.component.gameObject.GetComponent<LayoutElement>().preferredWidth = (componentType.component.gameObject.transform as RectTransform).sizeDelta.x;
			customListTableData.tableView.gameObject.SetActive(true);
			string text11;
			if (componentType.data.TryGetValue("id", out text11))
			{
				TableViewScroller field = customListTableData.tableView.GetField("_scroller");
				parserParams.AddEvent(text11 + "#PageUp", new Action(field.PageScrollUp));
				parserParams.AddEvent(text11 + "#PageDown", new Action(field.PageScrollDown));
			}
		}
	}
}
