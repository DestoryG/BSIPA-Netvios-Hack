using System;
using System.Collections.Generic;
using IPA.Logging;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000022 RID: 34
	[ComponentHandler(typeof(Image))]
	internal class ImageHandler : TypeHandler<Image>
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000637C File Offset: 0x0000457C
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"image",
						new string[] { "source", "src" }
					},
					{
						"preserveAspect",
						new string[] { "preserve-aspect" }
					},
					{
						"imageColor",
						new string[] { "image-color", "img-color" }
					}
				};
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000063EC File Offset: 0x000045EC
		public override Dictionary<string, Action<Image, string>> Setters
		{
			get
			{
				Dictionary<string, Action<Image, string>> dictionary = new Dictionary<string, Action<Image, string>>();
				dictionary.Add("image", new Action<Image, string>(BeatSaberUI.SetImage));
				dictionary.Add("preserveAspect", delegate(Image image, string preserveAspect)
				{
					image.preserveAspect = bool.Parse(preserveAspect);
				});
				dictionary.Add("image-color", delegate(Image image, string color)
				{
					image.color = ImageHandler.GetColor(color);
				});
				return dictionary;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000646C File Offset: 0x0000466C
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
