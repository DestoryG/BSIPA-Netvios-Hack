using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Parser;
using HMUI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000033 RID: 51
	[ComponentHandler(typeof(TextSegmentedControl))]
	public class TextSegmentedControlHandler : TypeHandler
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00007EDC File Offset: 0x000060DC
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

		// Token: 0x0600011B RID: 283 RVA: 0x00007F2C File Offset: 0x0000612C
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			TextSegmentedControl textSegmentedControl = componentType.component as TextSegmentedControl;
			string text;
			if (componentType.data.TryGetValue("data", out text))
			{
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text, out bsmlvalue))
				{
					throw new Exception("value '" + text + "' not found");
				}
				textSegmentedControl.SetTexts((bsmlvalue.GetValue() as List<object>).Select((object x) => x.ToString()).ToArray<string>());
			}
			string selectCell;
			if (componentType.data.TryGetValue("selectCell", out selectCell))
			{
				textSegmentedControl.didSelectCellEvent += delegate(SegmentedControl control, int index)
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
