using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000053 RID: 83
	public class PageButtonTag : BSMLTag
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00009FD0 File Offset: 0x000081D0
		public override string[] Aliases
		{
			get
			{
				return new string[] { "page-button", "pg-button" };
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00009FE8 File Offset: 0x000081E8
		public override GameObject CreateObject(Transform parent)
		{
			Button button = Object.Instantiate<Button>(Resources.FindObjectsOfTypeAll<Button>().Last((Button x) => x.name == "PageDownButton"), parent, false);
			button.gameObject.SetActive(false);
			button.name = "BSMLPageButton";
			button.interactable = true;
			button.gameObject.AddComponent<PageButton>();
			LayoutElement layoutElement = button.gameObject.AddComponent<LayoutElement>();
			layoutElement.preferredWidth = -1f;
			layoutElement.preferredHeight = -1f;
			layoutElement.flexibleHeight = 0f;
			layoutElement.flexibleWidth = 0f;
			ContentSizeFitter contentSizeFitter = button.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = 2;
			contentSizeFitter.verticalFit = 2;
			RectTransform rectTransform = button.transform.GetChild(0) as RectTransform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.sizeDelta = new Vector2(0f, 0f);
			(button.transform as RectTransform).pivot = new Vector2(0.5f, 0.5f);
			RectTransform rectTransform2 = Object.Instantiate<GameObject>(Resources.FindObjectsOfTypeAll<GameObject>().Last((GameObject x) => x.name == "GlowContainer"), button.transform).transform as RectTransform;
			rectTransform2.gameObject.name = "BSMLPageButtonGlowContainer";
			rectTransform2.SetParent(rectTransform);
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.sizeDelta = new Vector2(0f, 0f);
			rectTransform2.anchoredPosition = new Vector2(0f, 0f);
			Glowable glowable = button.gameObject.AddComponent<Glowable>();
			glowable.image = (from x in button.gameObject.GetComponentsInChildren<Image>(true)
				where x.gameObject.name == "Glow"
				select x).FirstOrDefault<Image>();
			glowable.SetGlow("none");
			button.gameObject.AddComponent<ButtonIconImage>().image = (from x in button.gameObject.GetComponentsInChildren<Image>(true)
				where x.gameObject.name == "Arrow"
				select x).FirstOrDefault<Image>();
			button.gameObject.SetActive(true);
			return button.gameObject;
		}
	}
}
