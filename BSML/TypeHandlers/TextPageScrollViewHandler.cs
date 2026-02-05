using System;
using System.Collections.Generic;
using HMUI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000032 RID: 50
	[ComponentHandler(typeof(TextPageScrollView))]
	public class TextPageScrollViewHandler : TypeHandler<TextPageScrollView>
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007E74 File Offset: 0x00006074
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"text",
					new string[] { "text" }
				} };
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00007EA1 File Offset: 0x000060A1
		public override Dictionary<string, Action<TextPageScrollView, string>> Setters
		{
			get
			{
				Dictionary<string, Action<TextPageScrollView, string>> dictionary = new Dictionary<string, Action<TextPageScrollView, string>>();
				dictionary.Add("text", delegate(TextPageScrollView component, string value)
				{
					component.SetText(value ?? string.Empty);
				});
				return dictionary;
			}
		}
	}
}
