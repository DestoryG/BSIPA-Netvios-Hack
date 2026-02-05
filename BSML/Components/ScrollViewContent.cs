using System;
using System.Collections;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A5 RID: 165
	public class ScrollViewContent : MonoBehaviour
	{
		// Token: 0x06000360 RID: 864 RVA: 0x0001076D File Offset: 0x0000E96D
		private void Start()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.transform as RectTransform);
			base.StopAllCoroutines();
			base.StartCoroutine(this.SetupScrollView());
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00010792 File Offset: 0x0000E992
		private void OnEnable()
		{
			this.UpdateScrollView();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00010792 File Offset: 0x0000E992
		private void OnRectTransformDimensionsChange()
		{
			this.UpdateScrollView();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0001079A File Offset: 0x0000E99A
		private IEnumerator SetupScrollView()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			yield return new WaitWhile(() => rectTransform.sizeDelta.y == -1f);
			this.UpdateScrollView();
			yield break;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000107A9 File Offset: 0x0000E9A9
		private void UpdateScrollView()
		{
			ScrollView scrollView = this.scrollView;
			if (scrollView != null)
			{
				scrollView.Setup();
			}
			ScrollView scrollView2 = this.scrollView;
			if (scrollView2 == null)
			{
				return;
			}
			scrollView2.RefreshButtonsInteractibility();
		}

		// Token: 0x040000F6 RID: 246
		public ScrollView scrollView;
	}
}
