using System;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000055 RID: 85
	public class ScrollableSettingsContainerTag : ScrollViewTag
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000A44F File Offset: 0x0000864F
		public override string[] Aliases
		{
			get
			{
				return new string[] { "settings-scroll-view", "scrollable-settings-container" };
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000A468 File Offset: 0x00008668
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = base.CreateObject(parent);
			ExternalComponents component = gameObject.GetComponent<ExternalComponents>();
			RectTransform rectTransform = component.Get<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(2f, 6f);
			rectTransform.sizeDelta = new Vector2(0f, -20f);
			rectTransform.gameObject.name = "BSMLScrollableSettingsContainer";
			component.Get<BSMLScrollView>().ReserveButtonSpace = true;
			return gameObject;
		}
	}
}
