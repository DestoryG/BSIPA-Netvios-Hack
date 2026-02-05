using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200003D RID: 61
	public class BackgroundTag : BSMLTag
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000880C File Offset: 0x00006A0C
		public override string[] Aliases
		{
			get
			{
				return new string[] { "bg", "background", "div" };
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000882C File Offset: 0x00006A2C
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLBackground";
			gameObject.transform.SetParent(parent, false);
			gameObject.AddComponent<ContentSizeFitter>();
			gameObject.AddComponent<Backgroundable>();
			RectTransform rectTransform = gameObject.transform as RectTransform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.sizeDelta = new Vector2(0f, 0f);
			return gameObject;
		}
	}
}
