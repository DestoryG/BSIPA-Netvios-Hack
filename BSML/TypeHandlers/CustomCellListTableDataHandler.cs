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
	// Token: 0x0200001C RID: 28
	[ComponentHandler(typeof(CustomCellListTableData))]
	public class CustomCellListTableDataHandler : TypeHandler
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00005564 File Offset: 0x00003764
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
						"listWidth",
						new string[] { "list-width" }
					},
					{
						"listHeight",
						new string[] { "list-height" }
					},
					{
						"listDirection",
						new string[] { "list-direction" }
					},
					{
						"data",
						new string[] { "contents", "data" }
					},
					{
						"cellClickable",
						new string[] { "clickable-cells" }
					},
					{
						"cellTemplate",
						new string[] { "_children" }
					}
				};
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000567C File Offset: 0x0000387C
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			CustomCellListTableData customCellListTableData = componentType.component as CustomCellListTableData;
			string selectCell;
			if (componentType.data.TryGetValue("selectCell", out selectCell))
			{
				customCellListTableData.tableView.didSelectCellWithIdxEvent += delegate(TableView table, int index)
				{
					BSMLAction bsmlaction;
					if (!parserParams.actions.TryGetValue(selectCell, out bsmlaction))
					{
						throw new Exception("select-cell action '" + componentType.data["selectCell"] + "' not found");
					}
					bsmlaction.Invoke(new object[]
					{
						table,
						(table.dataSource as CustomCellListTableData).data[index]
					});
				};
			}
			string text;
			if (componentType.data.TryGetValue("listDirection", out text))
			{
				customCellListTableData.tableView.SetField("_tableType", (TableView.TableType)Enum.Parse(typeof(TableView.TableType), text));
			}
			string text2;
			if (componentType.data.TryGetValue("cellSize", out text2))
			{
				customCellListTableData.cellSize = Parse.Float(text2);
			}
			string text3;
			if (componentType.data.TryGetValue("cellTemplate", out text3))
			{
				customCellListTableData.cellTemplate = "<bg>" + text3 + "</bg>";
			}
			string text4;
			if (componentType.data.TryGetValue("cellClickable", out text4))
			{
				customCellListTableData.clickableCells = Parse.Bool(text4);
			}
			string text5;
			if (componentType.data.TryGetValue("data", out text5))
			{
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text5, out bsmlvalue))
				{
					throw new Exception("value '" + text5 + "' not found");
				}
				customCellListTableData.data = bsmlvalue.GetValue() as List<object>;
				customCellListTableData.tableView.ReloadData();
			}
			TableView.TableType tableType = customCellListTableData.tableView.tableType;
			if (tableType != null)
			{
				if (tableType == 1)
				{
					string text6;
					string text7;
					(componentType.component.gameObject.transform as RectTransform).sizeDelta = new Vector2(customCellListTableData.cellSize * (componentType.data.TryGetValue("visibleCells", out text6) ? Parse.Float(text6) : 4f), componentType.data.TryGetValue("listHeight", out text7) ? Parse.Float(text7) : 40f);
				}
			}
			else
			{
				string text8;
				string text9;
				(componentType.component.gameObject.transform as RectTransform).sizeDelta = new Vector2(componentType.data.TryGetValue("listWidth", out text8) ? Parse.Float(text8) : 60f, customCellListTableData.cellSize * (componentType.data.TryGetValue("visibleCells", out text9) ? Parse.Float(text9) : 7f));
				customCellListTableData.tableView.contentTransform.anchorMin = new Vector2(0f, 1f);
			}
			componentType.component.gameObject.GetComponent<LayoutElement>().preferredHeight = (componentType.component.gameObject.transform as RectTransform).sizeDelta.y;
			componentType.component.gameObject.GetComponent<LayoutElement>().preferredWidth = (componentType.component.gameObject.transform as RectTransform).sizeDelta.x;
			customCellListTableData.tableView.gameObject.SetActive(true);
			string text10;
			if (componentType.data.TryGetValue("id", out text10))
			{
				TableViewScroller field = customCellListTableData.tableView.GetField("_scroller");
				parserParams.AddEvent(text10 + "#PageUp", new Action(field.PageScrollUp));
				parserParams.AddEvent(text10 + "#PageDown", new Action(field.PageScrollDown));
			}
		}
	}
}
