using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.TypeHandlers.Settings
{
	// Token: 0x0200003A RID: 58
	[ComponentHandler(typeof(ListSliderSetting))]
	public class ListSliderSettingHandler : TypeHandler
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000085A0 File Offset: 0x000067A0
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

		// Token: 0x06000133 RID: 307 RVA: 0x000085D8 File Offset: 0x000067D8
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			ListSliderSetting listSliderSetting = componentType.component as ListSliderSetting;
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
			listSliderSetting.values = bsmlvalue.GetValue() as List<object>;
		}
	}
}
