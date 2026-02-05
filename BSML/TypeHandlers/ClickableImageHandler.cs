using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using IPA.Logging;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000019 RID: 25
	[ComponentHandler(typeof(ClickableImage))]
	public class ClickableImageHandler : TypeHandler<ClickableImage>
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000050FC File Offset: 0x000032FC
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BA RID: 186 RVA: 0x0000517C File Offset: 0x0000337C
		public override Dictionary<string, Action<ClickableImage, string>> Setters
		{
			get
			{
				Dictionary<string, Action<ClickableImage, string>> dictionary = new Dictionary<string, Action<ClickableImage, string>>();
				dictionary.Add("highlightColor", delegate(ClickableImage image, string color)
				{
					image.HighlightColor = ClickableImageHandler.GetColor(color);
				});
				dictionary.Add("defaultColor", delegate(ClickableImage image, string color)
				{
					image.DefaultColor = ClickableImageHandler.GetColor(color);
				});
				return dictionary;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000051E4 File Offset: 0x000033E4
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			base.HandleType(componentType, parserParams);
			ClickableImage clickableImage = componentType.component as ClickableImage;
			string onClick;
			if (componentType.data.TryGetValue("onClick", out onClick))
			{
				ClickableImage clickableImage2 = clickableImage;
				clickableImage2.OnClickEvent = (Action<PointerEventData>)Delegate.Combine(clickableImage2.OnClickEvent, new Action<PointerEventData>(delegate
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
				ClickableImage clickableImage3 = clickableImage;
				clickableImage3.OnClickEvent = (Action<PointerEventData>)Delegate.Combine(clickableImage3.OnClickEvent, new Action<PointerEventData>(delegate
				{
					parserParams.EmitEvent(clickEvent);
				}));
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000528C File Offset: 0x0000348C
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
