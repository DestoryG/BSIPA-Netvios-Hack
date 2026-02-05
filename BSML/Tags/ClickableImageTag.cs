using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000042 RID: 66
	public class ClickableImageTag : BSMLTag
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00008CF3 File Offset: 0x00006EF3
		public override string[] Aliases
		{
			get
			{
				return new string[] { "clickable-image" };
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00008D04 File Offset: 0x00006F04
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject("BSMLClickableImage");
			ClickableImage clickableImage = gameObject.AddComponent<ClickableImage>();
			clickableImage.material = Utilities.ImageResources.NoGlowMat;
			clickableImage.rectTransform.SetParent(parent, false);
			clickableImage.rectTransform.sizeDelta = new Vector2(20f, 20f);
			clickableImage.sprite = Utilities.ImageResources.BlankSprite;
			gameObject.AddComponent<LayoutElement>();
			return gameObject;
		}
	}
}
