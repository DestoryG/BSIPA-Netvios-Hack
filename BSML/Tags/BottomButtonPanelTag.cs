using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200003F RID: 63
	public class BottomButtonPanelTag : BSMLTag
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00008A5F File Offset: 0x00006C5F
		public override string[] Aliases
		{
			get
			{
				return new string[] { "bottom-button-panel" };
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00008A70 File Offset: 0x00006C70
		public override GameObject CreateObject(Transform parent)
		{
			RectTransform rectTransform = new GameObject().AddComponent<RectTransform>();
			rectTransform.name = "BSMLBottomPanelContainer";
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 0f);
			rectTransform.sizeDelta = new Vector2(0f, 0f);
			StartMiddleEndButtonsGroup startMiddleEndButtonsGroup = Object.Instantiate<StartMiddleEndButtonsGroup>(Resources.FindObjectsOfTypeAll<StartMiddleEndButtonsGroup>().First((StartMiddleEndButtonsGroup x) => x.name == "Buttons"), rectTransform, false);
			startMiddleEndButtonsGroup.name = "BSMLBottomPanelButtons";
			foreach (object obj in startMiddleEndButtonsGroup.transform)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			startMiddleEndButtonsGroup.gameObject.AddComponent<ExternalComponents>().components.Add(rectTransform);
			return startMiddleEndButtonsGroup.gameObject;
		}
	}
}
