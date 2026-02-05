using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200004D RID: 77
	public class RawImageTag : BSMLTag
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600016E RID: 366 RVA: 0x000098EF File Offset: 0x00007AEF
		public override string[] Aliases
		{
			get
			{
				return new string[] { "raw-image" };
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00009900 File Offset: 0x00007B00
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject("BSMLRawImage");
			RawImage rawImage = gameObject.AddComponent<RawImage>();
			rawImage.material = Utilities.ImageResources.NoGlowMat;
			rawImage.rectTransform.SetParent(parent, false);
			rawImage.rectTransform.sizeDelta = new Vector2(20f, 20f);
			rawImage.texture = Utilities.ImageResources.BlankSprite.texture;
			gameObject.AddComponent<LayoutElement>();
			return gameObject;
		}
	}
}
