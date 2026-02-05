using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000016 RID: 22
	[ComponentHandler(typeof(ButtonArtworkImage))]
	public class ButtonArtworkHandler : TypeHandler<ButtonArtworkImage>
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004EBC File Offset: 0x000030BC
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"artwork",
					new string[] { "artwork", "art", "bg-artwork", "bg-art" }
				} };
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004F01 File Offset: 0x00003101
		public override Dictionary<string, Action<ButtonArtworkImage, string>> Setters
		{
			get
			{
				Dictionary<string, Action<ButtonArtworkImage, string>> dictionary = new Dictionary<string, Action<ButtonArtworkImage, string>>();
				dictionary.Add("artwork", delegate(ButtonArtworkImage images, string artPath)
				{
					images.SetArtwork(artPath);
				});
				return dictionary;
			}
		}
	}
}
