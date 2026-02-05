using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000056 RID: 86
	public class ScrollIndicatorTag : BSMLTag
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000A4D4 File Offset: 0x000086D4
		public override string[] Aliases { get; } = new string[] { "vertical-scroll-indicator", "scroll-indicator" };

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000A4DC File Offset: 0x000086DC
		public static GameObject VerticalScrollTemplate
		{
			get
			{
				if (ScrollIndicatorTag._verticalScrollTemplate == null)
				{
					ScrollIndicatorTag._verticalScrollTemplate = Resources.FindObjectsOfTypeAll<VerticalScrollIndicator>().First<VerticalScrollIndicator>().gameObject;
				}
				return ScrollIndicatorTag._verticalScrollTemplate;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000A504 File Offset: 0x00008704
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(ScrollIndicatorTag.VerticalScrollTemplate);
			gameObject.SetActive(false);
			gameObject.name = "BSMLScrollIndicator";
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(parent, false);
			Object.DestroyImmediate(gameObject.GetComponent<VerticalScrollIndicator>());
			gameObject.AddComponent<BSMLScrollIndicator>().Handle = component.GetChild(0).GetComponent<RectTransform>();
			gameObject.SetActive(true);
			return gameObject;
		}

		// Token: 0x04000039 RID: 57
		private static GameObject _verticalScrollTemplate;
	}
}
