using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000046 RID: 70
	public class HorizontalLayoutTag : BSMLTag
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000090B8 File Offset: 0x000072B8
		public override string[] Aliases
		{
			get
			{
				return new string[] { "horizontal" };
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000090C8 File Offset: 0x000072C8
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLHorizontalLayoutGroup";
			gameObject.transform.SetParent(parent, false);
			gameObject.AddComponent<HorizontalLayoutGroup>();
			gameObject.AddComponent<ContentSizeFitter>().verticalFit = 2;
			gameObject.AddComponent<Backgroundable>();
			RectTransform rectTransform = gameObject.transform as RectTransform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.sizeDelta = new Vector2(0f, 0f);
			gameObject.AddComponent<LayoutElement>();
			return gameObject;
		}
	}
}
