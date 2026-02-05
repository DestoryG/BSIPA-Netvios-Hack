using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000057 RID: 87
	public class ScrollViewTag : BSMLTag
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000A589 File Offset: 0x00008789
		public override string[] Aliases
		{
			get
			{
				return new string[] { "scroll-view" };
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000A59C File Offset: 0x0000879C
		public override GameObject CreateObject(Transform parent)
		{
			TextPageScrollView textPageScrollView = Object.Instantiate<TextPageScrollView>(Resources.FindObjectsOfTypeAll<ReleaseInfoViewController>().First<ReleaseInfoViewController>().GetField("_textPageScrollView"), parent);
			textPageScrollView.name = "BSMLScrollView";
			Button field = textPageScrollView.GetField("_pageUpButton");
			Button field2 = textPageScrollView.GetField("_pageDownButton");
			VerticalScrollIndicator field3 = textPageScrollView.GetField("_verticalScrollIndicator");
			RectTransform field4 = textPageScrollView.GetField("_viewport");
			Object.Destroy(textPageScrollView.GetField("_text").gameObject);
			GameObject gameObject = textPageScrollView.gameObject;
			Object.Destroy(textPageScrollView);
			gameObject.SetActive(false);
			BSMLScrollView bsmlscrollView = gameObject.AddComponent<BSMLScrollView>();
			bsmlscrollView.SetField("_pageUpButton", field);
			bsmlscrollView.SetField("_pageDownButton", field2);
			bsmlscrollView.SetField("_verticalScrollIndicator", field3);
			bsmlscrollView.SetField("_viewport", field4);
			field4.anchorMin = new Vector2(0f, 0f);
			field4.anchorMax = new Vector2(1f, 1f);
			bsmlscrollView.ReserveButtonSpace = false;
			GameObject gameObject2 = new GameObject();
			gameObject2.name = "BSMLScrollViewContent";
			gameObject2.transform.SetParent(field4, false);
			VerticalLayoutGroup verticalLayoutGroup = gameObject2.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			RectTransform rectTransform = gameObject2.transform as RectTransform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.sizeDelta = new Vector2(0f, 0f);
			gameObject2.AddComponent<ScrollViewContent>().scrollView = bsmlscrollView;
			GameObject gameObject3 = new GameObject();
			gameObject3.name = "BSMLScrollViewContentContainer";
			gameObject3.transform.SetParent(rectTransform, false);
			VerticalLayoutGroup verticalLayoutGroup2 = gameObject3.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.childControlHeight = false;
			verticalLayoutGroup2.childForceExpandHeight = false;
			verticalLayoutGroup2.childAlignment = 4;
			verticalLayoutGroup2.spacing = 0.5f;
			gameObject3.AddComponent<ContentSizeFitter>();
			gameObject3.AddComponent<LayoutElement>();
			ExternalComponents externalComponents = gameObject3.AddComponent<ExternalComponents>();
			externalComponents.components.Add(bsmlscrollView);
			externalComponents.components.Add(bsmlscrollView.transform);
			externalComponents.components.Add(gameObject.AddComponent<LayoutElement>());
			(gameObject3.transform as RectTransform).sizeDelta = new Vector2(0f, -1f);
			bsmlscrollView.SetField("_contentRectTransform", gameObject2.transform as RectTransform);
			gameObject.SetActive(true);
			return gameObject3;
		}
	}
}
