using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;
using HMUI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000021 RID: 33
	[ComponentHandler(typeof(IconSegmentedControl))]
	public class IconSegmentedControlHandler : TypeHandler
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00006270 File Offset: 0x00004470
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
						"data",
						new string[] { "contents", "data" }
					}
				};
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000062C0 File Offset: 0x000044C0
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			IconSegmentedControl iconSegmentedControl = componentType.component as IconSegmentedControl;
			string text;
			if (componentType.data.TryGetValue("data", out text))
			{
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text, out bsmlvalue))
				{
					throw new Exception("value '" + text + "' not found");
				}
				iconSegmentedControl.SetData((bsmlvalue.GetValue() as List<IconSegmentedControl.DataItem>).ToArray());
			}
			string selectCell;
			if (componentType.data.TryGetValue("selectCell", out selectCell))
			{
				iconSegmentedControl.didSelectCellEvent += delegate(SegmentedControl control, int index)
				{
					BSMLAction bsmlaction;
					if (!parserParams.actions.TryGetValue(selectCell, out bsmlaction))
					{
						throw new Exception("select-cell action '" + componentType.data["selectCell"] + "' not found");
					}
					bsmlaction.Invoke(new object[] { control, index });
				};
			}
		}
	}
}
