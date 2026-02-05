using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200005C RID: 92
	public class TextPageScrollViewTag : BSMLTag
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000AAE3 File Offset: 0x00008CE3
		public override string[] Aliases
		{
			get
			{
				return new string[] { "text-page" };
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		public override GameObject CreateObject(Transform parent)
		{
			TextPageScrollView textPageScrollView = Object.Instantiate<TextPageScrollView>(Resources.FindObjectsOfTypeAll<ReleaseInfoViewController>().First<ReleaseInfoViewController>().GetField("_textPageScrollView"), parent);
			textPageScrollView.name = "BSMLTextPageScrollView";
			textPageScrollView.enabled = true;
			textPageScrollView.gameObject.AddComponent<TextPageScrollViewRefresher>().scrollView = textPageScrollView;
			return textPageScrollView.gameObject;
		}
	}
}
