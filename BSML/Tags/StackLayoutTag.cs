using System;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000059 RID: 89
	public class StackLayoutTag : BSMLTag
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000A910 File Offset: 0x00008B10
		public override string[] Aliases
		{
			get
			{
				return new string[] { "stack" };
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000A920 File Offset: 0x00008B20
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLStackLayoutGroup";
			gameObject.transform.SetParent(parent, false);
			gameObject.AddComponent<StackLayoutGroup>();
			gameObject.AddComponent<ContentSizeFitter>();
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
