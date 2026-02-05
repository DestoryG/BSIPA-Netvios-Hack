using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using IPA.Logging;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000028 RID: 40
	[ComponentHandler(typeof(ModalKeyboard))]
	public class ModalKeyboardHandler : TypeHandler
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00006B44 File Offset: 0x00004D44
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
						"onEnter",
						new string[] { "on-enter" }
					},
					{
						"clearOnOpen",
						new string[] { "clear-on-open" }
					}
				};
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006BA4 File Offset: 0x00004DA4
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			try
			{
				ModalKeyboard modalKeyboard = componentType.component as ModalKeyboard;
				string text;
				if (componentType.data.TryGetValue("clearOnOpen", out text))
				{
					modalKeyboard.clearOnOpen = bool.Parse(text);
				}
				string text2;
				if (componentType.data.TryGetValue("value", out text2))
				{
					BSMLValue bsmlvalue;
					if (!parserParams.values.TryGetValue(text2, out bsmlvalue))
					{
						throw new Exception("value '" + text2 + "' not found");
					}
					modalKeyboard.associatedValue = bsmlvalue;
				}
				string text3;
				if (componentType.data.TryGetValue("onEnter", out text3))
				{
					BSMLAction bsmlaction;
					if (!parserParams.actions.TryGetValue(text3, out bsmlaction))
					{
						throw new Exception("on-enter action '" + text3 + "' not found");
					}
					modalKeyboard.onEnter = bsmlaction;
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
