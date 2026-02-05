using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.TypeHandlers.Settings
{
	// Token: 0x02000037 RID: 55
	[ComponentHandler(typeof(DropDownListSetting))]
	public class DropDownListSettingHandler : TypeHandler
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000081B8 File Offset: 0x000063B8
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

		// Token: 0x06000129 RID: 297 RVA: 0x000081F0 File Offset: 0x000063F0
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			DropDownListSetting dropDownListSetting = componentType.component as DropDownListSetting;
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
			dropDownListSetting.values = bsmlvalue.GetValue() as List<object>;
		}
	}
}
