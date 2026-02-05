using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000045 RID: 69
	public class GridLayoutTag : BSMLTag
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000901F File Offset: 0x0000721F
		public override string[] Aliases
		{
			get
			{
				return new string[] { "grid" };
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00009030 File Offset: 0x00007230
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLGridLayoutGroup";
			gameObject.transform.SetParent(parent, false);
			gameObject.AddComponent<GridLayoutGroup>();
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
