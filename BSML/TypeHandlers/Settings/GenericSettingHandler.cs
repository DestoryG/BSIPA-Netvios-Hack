using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.TypeHandlers.Settings
{
	// Token: 0x02000038 RID: 56
	[ComponentHandler(typeof(GenericSetting))]
	public class GenericSettingHandler : TypeHandler
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00008260 File Offset: 0x00006460
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"onChange",
						new string[] { "on-change" }
					},
					{
						"value",
						new string[] { "value" }
					},
					{
						"setEvent",
						new string[] { "set-event" }
					},
					{
						"getEvent",
						new string[] { "get-event" }
					},
					{
						"applyOnChange",
						new string[] { "apply-on-change" }
					},
					{
						"formatter",
						new string[] { "formatter" }
					}
				};
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000830C File Offset: 0x0000650C
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			GenericSetting genericSetting = componentType.component as GenericSetting;
			string text;
			if (componentType.data.TryGetValue("formatter", out text))
			{
				BSMLAction bsmlaction;
				if (!parserParams.actions.TryGetValue(text, out bsmlaction))
				{
					string text2 = "formatter action '";
					BSMLAction bsmlaction2 = bsmlaction;
					throw new Exception(text2 + ((bsmlaction2 != null) ? bsmlaction2.ToString() : null) + "' not found");
				}
				genericSetting.formatter = bsmlaction;
			}
			string text3;
			if (componentType.data.TryGetValue("applyOnChange", out text3))
			{
				genericSetting.updateOnChange = Parse.Bool(text3);
			}
			string text4;
			if (componentType.data.TryGetValue("onChange", out text4))
			{
				BSMLAction bsmlaction3;
				if (!parserParams.actions.TryGetValue(text4, out bsmlaction3))
				{
					throw new Exception("on-change action '" + text4 + "' not found");
				}
				genericSetting.onChange = bsmlaction3;
			}
			string text5;
			if (componentType.data.TryGetValue("value", out text5))
			{
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text5, out bsmlvalue))
				{
					throw new Exception("value '" + text5 + "' not found");
				}
				genericSetting.associatedValue = bsmlvalue;
			}
			string text6;
			parserParams.AddEvent(componentType.data.TryGetValue("setEvent", out text6) ? text6 : "apply", new Action(genericSetting.ApplyValue));
			string text7;
			parserParams.AddEvent(componentType.data.TryGetValue("getEvent", out text7) ? text7 : "cancel", new Action(genericSetting.ReceiveValue));
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000847A File Offset: 0x0000667A
		public override void HandleTypeAfterChildren(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			(componentType.component as GenericSetting).Setup();
		}
	}
}
