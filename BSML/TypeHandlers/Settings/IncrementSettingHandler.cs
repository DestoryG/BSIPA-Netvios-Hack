using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.TypeHandlers.Settings
{
	// Token: 0x02000039 RID: 57
	[ComponentHandler(typeof(IncrementSetting))]
	public class IncrementSettingHandler : TypeHandler
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000848C File Offset: 0x0000668C
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"increment",
						new string[] { "increment" }
					},
					{
						"minValue",
						new string[] { "min" }
					},
					{
						"maxValue",
						new string[] { "max" }
					},
					{
						"isInt",
						new string[] { "integer-only" }
					}
				};
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00008504 File Offset: 0x00006704
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			IncrementSetting incrementSetting = componentType.component as IncrementSetting;
			string text;
			if (componentType.data.TryGetValue("isInt", out text))
			{
				incrementSetting.isInt = Parse.Bool(text);
			}
			string text2;
			if (componentType.data.TryGetValue("increment", out text2))
			{
				incrementSetting.increments = Parse.Float(text2);
			}
			string text3;
			if (componentType.data.TryGetValue("minValue", out text3))
			{
				incrementSetting.minValue = Parse.Float(text3);
			}
			string text4;
			if (componentType.data.TryGetValue("maxValue", out text4))
			{
				incrementSetting.maxValue = Parse.Float(text4);
			}
		}
	}
}
