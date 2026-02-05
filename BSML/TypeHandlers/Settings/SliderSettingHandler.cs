using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.TypeHandlers.Settings
{
	// Token: 0x0200003C RID: 60
	[ComponentHandler(typeof(SliderSetting))]
	public class SliderSettingHandler : TypeHandler
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000086F0 File Offset: 0x000068F0
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

		// Token: 0x06000139 RID: 313 RVA: 0x00008768 File Offset: 0x00006968
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			SliderSetting sliderSetting = componentType.component as SliderSetting;
			string text;
			if (componentType.data.TryGetValue("isInt", out text))
			{
				sliderSetting.isInt = Parse.Bool(text);
			}
			string text2;
			if (componentType.data.TryGetValue("increment", out text2))
			{
				sliderSetting.increments = Parse.Float(text2);
			}
			string text3;
			if (componentType.data.TryGetValue("minValue", out text3))
			{
				sliderSetting.slider.minValue = Parse.Float(text3);
			}
			string text4;
			if (componentType.data.TryGetValue("maxValue", out text4))
			{
				sliderSetting.slider.maxValue = Parse.Float(text4);
			}
		}
	}
}
