using System;
using HMUI;
using IPA.Utilities;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A9 RID: 169
	internal class TextPageScrollViewRefresher : MonoBehaviour
	{
		// Token: 0x06000374 RID: 884 RVA: 0x00010CB1 File Offset: 0x0000EEB1
		private void OnEnable()
		{
			TextPageScrollView textPageScrollView = this.scrollView;
			if (textPageScrollView == null)
			{
				return;
			}
			textPageScrollView.SetText(this.scrollView.GetField("_text").text);
		}

		// Token: 0x04000106 RID: 262
		public TextPageScrollView scrollView;
	}
}
