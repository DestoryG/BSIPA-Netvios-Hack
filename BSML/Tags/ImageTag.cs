using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000049 RID: 73
	public class ImageTag : BSMLTag
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00009414 File Offset: 0x00007614
		public override string[] Aliases
		{
			get
			{
				return new string[] { "image", "img" };
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000942C File Offset: 0x0000762C
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject("BSMLImage");
			Image image = gameObject.AddComponent<Image>();
			image.material = Utilities.ImageResources.NoGlowMat;
			image.rectTransform.SetParent(parent, false);
			image.rectTransform.sizeDelta = new Vector2(20f, 20f);
			image.sprite = Utilities.ImageResources.BlankSprite;
			gameObject.AddComponent<LayoutElement>();
			return gameObject;
		}
	}
}
