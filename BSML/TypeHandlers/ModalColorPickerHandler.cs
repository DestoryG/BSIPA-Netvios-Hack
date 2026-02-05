using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using IPA.Logging;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000023 RID: 35
	[ComponentHandler(typeof(ModalColorPicker))]
	public class ModalColorPickerHandler : TypeHandler
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000064B4 File Offset: 0x000046B4
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"value",
						new string[] { "value" }
					},
					{
						"onCancel",
						new string[] { "on-cancel" }
					},
					{
						"onDone",
						new string[] { "on-done" }
					},
					{
						"onChange",
						new string[] { "color-change" }
					}
				};
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000652C File Offset: 0x0000472C
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			try
			{
				ModalColorPicker modalColorPicker = componentType.component as ModalColorPicker;
				string text;
				if (componentType.data.TryGetValue("value", out text))
				{
					BSMLValue bsmlvalue;
					if (!parserParams.values.TryGetValue(text, out bsmlvalue))
					{
						throw new Exception("value '" + text + "' not found");
					}
					modalColorPicker.associatedValue = bsmlvalue;
				}
				string text2;
				if (componentType.data.TryGetValue("onCancel", out text2))
				{
					BSMLAction bsmlaction;
					if (!parserParams.actions.TryGetValue(text2, out bsmlaction))
					{
						throw new Exception("on-cancel action '" + text2 + "' not found");
					}
					modalColorPicker.onCancel = bsmlaction;
				}
				string text3;
				if (componentType.data.TryGetValue("onDone", out text3))
				{
					BSMLAction bsmlaction2;
					if (!parserParams.actions.TryGetValue(text3, out bsmlaction2))
					{
						throw new Exception("on-done action '" + text3 + "' not found");
					}
					modalColorPicker.onDone = bsmlaction2;
				}
				string text4;
				if (componentType.data.TryGetValue("onChange", out text4))
				{
					BSMLAction bsmlaction3;
					if (!parserParams.actions.TryGetValue(text4, out bsmlaction3))
					{
						throw new Exception("color-change action '" + text4 + "' not found");
					}
					modalColorPicker.onChange = bsmlaction3;
				}
			}
			catch (Exception ex)
			{
				Logger log = Logger.log;
				if (log != null)
				{
					log.Error(ex);
				}
			}
		}
	}
}
