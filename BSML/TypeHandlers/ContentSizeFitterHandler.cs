using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200001B RID: 27
	[ComponentHandler(typeof(ContentSizeFitter))]
	public class ContentSizeFitterHandler : TypeHandler<ContentSizeFitter>
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000054AC File Offset: 0x000036AC
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"horizontalFit",
						new string[] { "horizontal-fit" }
					},
					{
						"verticalFit",
						new string[] { "vertical-fit" }
					}
				};
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000054F4 File Offset: 0x000036F4
		public override Dictionary<string, Action<ContentSizeFitter, string>> Setters
		{
			get
			{
				Dictionary<string, Action<ContentSizeFitter, string>> dictionary = new Dictionary<string, Action<ContentSizeFitter, string>>();
				dictionary.Add("horizontalFit", delegate(ContentSizeFitter component, string value)
				{
					component.horizontalFit = (ContentSizeFitter.FitMode)Enum.Parse(typeof(ContentSizeFitter.FitMode), value);
				});
				dictionary.Add("verticalFit", delegate(ContentSizeFitter component, string value)
				{
					component.verticalFit = (ContentSizeFitter.FitMode)Enum.Parse(typeof(ContentSizeFitter.FitMode), value);
				});
				return dictionary;
			}
		}
	}
}
