using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000018 RID: 24
	[ComponentHandler(typeof(ButtonIconImage))]
	public class ButtonIconHandler : TypeHandler<ButtonIconImage>
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00005094 File Offset: 0x00003294
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"icon",
					new string[] { "icon" }
				} };
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000050C1 File Offset: 0x000032C1
		public override Dictionary<string, Action<ButtonIconImage, string>> Setters
		{
			get
			{
				Dictionary<string, Action<ButtonIconImage, string>> dictionary = new Dictionary<string, Action<ButtonIconImage, string>>();
				dictionary.Add("icon", delegate(ButtonIconImage images, string iconPath)
				{
					images.SetIcon(iconPath);
				});
				return dictionary;
			}
		}
	}
}
