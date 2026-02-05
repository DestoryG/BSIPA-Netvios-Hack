using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.TypeHandlers.Settings
{
	// Token: 0x0200003B RID: 59
	[ComponentHandler(typeof(ListSetting))]
	public class ListSettingHandler : TypeHandler
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00008648 File Offset: 0x00006848
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"options",
					new string[] { "options", "choices" }
				} };
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00008680 File Offset: 0x00006880
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			ListSetting listSetting = componentType.component as ListSetting;
			string text;
			if (!componentType.data.TryGetValue("options", out text))
			{
				throw new Exception("list must have associated options");
			}
			BSMLValue bsmlvalue;
			if (!parserParams.values.TryGetValue(text, out bsmlvalue))
			{
				throw new Exception("options '" + text + "' not found");
			}
			listSetting.values = bsmlvalue.GetValue() as List<object>;
		}
	}
}
