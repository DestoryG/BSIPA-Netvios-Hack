using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000054 RID: 84
	public class ScrollableContainerTag : BSMLTag
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000A25D File Offset: 0x0000845D
		public override string[] Aliases { get; } = new string[] { "scrollable-container" };

		// Token: 0x06000184 RID: 388 RVA: 0x0000A268 File Offset: 0x00008468
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject("BSMLScrollScrollableContainer");
			gameObject.SetActive(false);
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(parent, false);
			rectTransform.localPosition = Vector2.zero;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = Vector2.zero;
			GameObject gameObject2 = new GameObject("Viewport");
			RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
			rectTransform2.SetParent(rectTransform, false);
			rectTransform2.localPosition = Vector2.zero;
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = Vector2.zero;
			Mask mask = gameObject2.AddComponent<Mask>();
			Image image = gameObject2.AddComponent<Image>();
			mask.showMaskGraphic = false;
			image.color = Color.white;
			image.sprite = Utilities.ImageResources.WhitePixel;
			image.material = Utilities.ImageResources.NoGlowMat;
			GameObject gameObject3 = new GameObject("Content Wrapper");
			RectTransform rectTransform3 = gameObject3.AddComponent<RectTransform>();
			rectTransform3.SetParent(rectTransform2, false);
			rectTransform3.localPosition = Vector2.zero;
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.anchoredPosition = Vector2.zero;
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			ContentSizeFitter contentSizeFitter = gameObject3.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = 0;
			contentSizeFitter.verticalFit = 2;
			VerticalLayoutGroup verticalLayoutGroup = gameObject3.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			BSMLScrollableContainer bsmlscrollableContainer = gameObject.AddComponent<BSMLScrollableContainer>();
			bsmlscrollableContainer.ContentRect = rectTransform3;
			bsmlscrollableContainer.Viewport = rectTransform2;
			gameObject3.AddComponent<ExternalComponents>().components.Add(bsmlscrollableContainer);
			gameObject.SetActive(true);
			return gameObject3;
		}
	}
}
