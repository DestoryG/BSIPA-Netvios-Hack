using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000014 RID: 20
	[ComponentHandler(typeof(Backgroundable))]
	public class BackgroundableHandler : TypeHandler<Backgroundable>
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004D70 File Offset: 0x00002F70
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"background",
						new string[] { "bg", "background" }
					},
					{
						"backgroundColor",
						new string[] { "bg-color", "background-color" }
					}
				};
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004DC8 File Offset: 0x00002FC8
		public override Dictionary<string, Action<Backgroundable, string>> Setters
		{
			get
			{
				Dictionary<string, Action<Backgroundable, string>> dictionary = new Dictionary<string, Action<Backgroundable, string>>();
				dictionary.Add("background", delegate(Backgroundable component, string value)
				{
					component.ApplyBackground(value);
				});
				dictionary.Add("backgroundColor", new Action<Backgroundable, string>(BackgroundableHandler.TrySetBackgroundColor));
				return dictionary;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004E1C File Offset: 0x0000301C
		public static void TrySetBackgroundColor(Backgroundable background, string colorStr)
		{
			if (colorStr == "none")
			{
				return;
			}
			Color color;
			ColorUtility.TryParseHtmlString(colorStr, ref color);
			background.background.color = color;
		}
	}
}
