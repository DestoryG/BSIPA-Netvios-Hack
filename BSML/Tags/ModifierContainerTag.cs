using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200004B RID: 75
	public class ModifierContainerTag : BSMLTag
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00009758 File Offset: 0x00007958
		public override string[] Aliases
		{
			get
			{
				return new string[] { "modifier-container" };
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00009768 File Offset: 0x00007968
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLModifierContainer";
			gameObject.transform.SetParent(parent, false);
			VerticalLayoutGroup verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(3, 3, 2, 2);
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			gameObject.AddComponent<ContentSizeFitter>().verticalFit = 2;
			gameObject.AddComponent<Backgroundable>().ApplyBackground("round-rect-panel");
			RectTransform rectTransform = gameObject.transform as RectTransform;
			rectTransform.anchoredPosition = new Vector2(0f, 3f);
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(54f, 0f);
			gameObject.AddComponent<LayoutElement>();
			return gameObject;
		}
	}
}
