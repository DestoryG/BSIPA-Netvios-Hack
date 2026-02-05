using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000058 RID: 88
	public class SettingsContainerTag : BSMLTag
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000A7F0 File Offset: 0x000089F0
		public override string[] Aliases
		{
			get
			{
				return new string[] { "settings-container" };
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000A800 File Offset: 0x00008A00
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLSettingsContent";
			gameObject.transform.SetParent(parent, false);
			VerticalLayoutGroup verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.padding = new RectOffset(3, 3, 2, 2);
			ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = 2;
			contentSizeFitter.verticalFit = 2;
			gameObject.AddComponent<Backgroundable>().ApplyBackground("round-rect-panel");
			RectTransform rectTransform = gameObject.transform as RectTransform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.anchoredPosition = new Vector2(2f, 6f);
			gameObject.AddComponent<LayoutElement>();
			GameObject gameObject2 = new GameObject();
			gameObject2.name = "BSMLSettingsContainer";
			gameObject2.transform.SetParent(rectTransform, false);
			VerticalLayoutGroup verticalLayoutGroup2 = gameObject2.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.childControlHeight = false;
			verticalLayoutGroup2.childForceExpandHeight = false;
			verticalLayoutGroup2.childAlignment = 4;
			verticalLayoutGroup2.spacing = 0.5f;
			gameObject2.AddComponent<ContentSizeFitter>();
			gameObject2.AddComponent<LayoutElement>();
			return gameObject2;
		}
	}
}
