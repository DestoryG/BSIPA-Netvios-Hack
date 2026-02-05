using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using IPA.Logging;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200001A RID: 26
	[ComponentHandler(typeof(ClickableText))]
	public class ClickableTextHandler : TypeHandler<ClickableText>
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000052D4 File Offset: 0x000034D4
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"onClick",
						new string[] { "on-click" }
					},
					{
						"clickEvent",
						new string[] { "click-event", "event-click" }
					},
					{
						"highlightColor",
						new string[] { "highlight-color" }
					},
					{
						"defaultColor",
						new string[] { "default-color" }
					}
				};
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005354 File Offset: 0x00003554
		public override Dictionary<string, Action<ClickableText, string>> Setters
		{
			get
			{
				Dictionary<string, Action<ClickableText, string>> dictionary = new Dictionary<string, Action<ClickableText, string>>();
				dictionary.Add("highlightColor", delegate(ClickableText text, string color)
				{
					text.HighlightColor = ClickableTextHandler.GetColor(color);
				});
				dictionary.Add("defaultColor", delegate(ClickableText text, string color)
				{
					text.DefaultColor = ClickableTextHandler.GetColor(color);
				});
				return dictionary;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000053BC File Offset: 0x000035BC
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			base.HandleType(componentType, parserParams);
			ClickableText clickableText = componentType.component as ClickableText;
			string onClick;
			if (componentType.data.TryGetValue("onClick", out onClick))
			{
				ClickableText clickableText2 = clickableText;
				clickableText2.OnClickEvent = (Action<PointerEventData>)Delegate.Combine(clickableText2.OnClickEvent, new Action<PointerEventData>(delegate
				{
					BSMLAction bsmlaction;
					if (!parserParams.actions.TryGetValue(onClick, out bsmlaction))
					{
						throw new Exception("on-click action '" + onClick + "' not found");
					}
					bsmlaction.Invoke(Array.Empty<object>());
				}));
			}
			string clickEvent;
			if (componentType.data.TryGetValue("clickEvent", out clickEvent))
			{
				ClickableText clickableText3 = clickableText;
				clickableText3.OnClickEvent = (Action<PointerEventData>)Delegate.Combine(clickableText3.OnClickEvent, new Action<PointerEventData>(delegate
				{
					parserParams.EmitEvent(clickEvent);
				}));
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005464 File Offset: 0x00003664
		private static Color GetColor(string colorStr)
		{
			Color color;
			if (ColorUtility.TryParseHtmlString(colorStr, ref color))
			{
				return color;
			}
			Logger log = Logger.log;
			if (log != null)
			{
				log.Warn("Color " + colorStr + ", is not a valid color.");
			}
			return Color.white;
		}
	}
}
