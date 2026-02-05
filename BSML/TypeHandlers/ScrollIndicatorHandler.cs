using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200002C RID: 44
	[ComponentHandler(typeof(BSMLScrollIndicator))]
	public class ScrollIndicatorHandler : TypeHandler<BSMLScrollIndicator>
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000072F7 File Offset: 0x000054F7
		public override Dictionary<string, string[]> Props { get; } = new Dictionary<string, string[]>
		{
			{
				"handleColor",
				new string[] { "handle-color" }
			},
			{
				"handleImage",
				new string[] { "handle-image" }
			}
		};

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000072FF File Offset: 0x000054FF
		public override Dictionary<string, Action<BSMLScrollIndicator, string>> Setters { get; }

		// Token: 0x060000FD RID: 253 RVA: 0x00007307 File Offset: 0x00005507
		private static Image GetHandleImage(BSMLScrollIndicator indicator)
		{
			return indicator.Handle.GetComponent<Image>();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007314 File Offset: 0x00005514
		private static void TrySetHandleColor(BSMLScrollIndicator indicator, string colorString)
		{
			Color color;
			if (!ColorUtility.TryParseHtmlString(colorString, ref color))
			{
				Logger.log.Warn("String " + colorString + " not a valid color");
				return;
			}
			ScrollIndicatorHandler.GetHandleImage(indicator).color = color;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00007354 File Offset: 0x00005554
		public ScrollIndicatorHandler()
		{
			Dictionary<string, Action<BSMLScrollIndicator, string>> dictionary = new Dictionary<string, Action<BSMLScrollIndicator, string>>();
			dictionary.Add("handleColor", new Action<BSMLScrollIndicator, string>(ScrollIndicatorHandler.TrySetHandleColor));
			dictionary.Add("handleImage", delegate(BSMLScrollIndicator indic, string src)
			{
				ScrollIndicatorHandler.GetHandleImage(indic).SetImage(src);
			});
			this.Setters = dictionary;
			base..ctor();
		}
	}
}
