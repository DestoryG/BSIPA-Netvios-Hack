using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200005F RID: 95
	public class VerticalLayoutTag : BSMLTag
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000ACF7 File Offset: 0x00008EF7
		public override string[] Aliases
		{
			get
			{
				return new string[] { "vertical" };
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000AD08 File Offset: 0x00008F08
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLVerticalLayoutGroup";
			gameObject.transform.SetParent(parent, false);
			gameObject.AddComponent<VerticalLayoutGroup>();
			gameObject.AddComponent<ContentSizeFitter>().horizontalFit = 2;
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
