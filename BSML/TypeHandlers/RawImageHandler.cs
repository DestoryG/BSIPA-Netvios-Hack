using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000024 RID: 36
	[ComponentHandler(typeof(RawImage))]
	internal class RawImageHandler : TypeHandler<RawImage>
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00006684 File Offset: 0x00004884
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"image",
					new string[] { "source", "src" }
				} };
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000066B9 File Offset: 0x000048B9
		public override Dictionary<string, Action<RawImage, string>> Setters
		{
			get
			{
				return new Dictionary<string, Action<RawImage, string>> { 
				{
					"image",
					new Action<RawImage, string>(this.SetImage)
				} };
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000066D8 File Offset: 0x000048D8
		public void SetImage(RawImage image, string imagePath)
		{
			if (imagePath.StartsWith("#"))
			{
				string imgName = imagePath.Substring(1);
				try
				{
					image.texture = Resources.FindObjectsOfTypeAll<Texture>().First((Texture x) => x.name == imgName);
					return;
				}
				catch
				{
					Logger.log.Error("Could not find Texture with image name " + imgName);
					return;
				}
			}
			Utilities.GetData(imagePath, delegate(byte[] data)
			{
				image.texture = Utilities.LoadTextureRaw(data);
			});
		}
	}
}
