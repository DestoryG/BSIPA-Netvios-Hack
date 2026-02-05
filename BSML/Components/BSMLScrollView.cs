using System;
using HMUI;
using IPA.Utilities;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x02000095 RID: 149
	public class BSMLScrollView : ScrollView
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000E894 File Offset: 0x0000CA94
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000E89C File Offset: 0x0000CA9C
		public bool ReserveButtonSpace
		{
			get
			{
				return this.reserveButtonSpace;
			}
			set
			{
				this.reserveButtonSpace = value;
				this._viewport.sizeDelta = new Vector2(-13f, (float)(this.reserveButtonSpace ? (-20) : (-8)));
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000E8CC File Offset: 0x0000CACC
		public override void Setup()
		{
			if (this._contentRectTransform == null)
			{
				return;
			}
			this._contentHeight = (this._contentRectTransform.GetChild(0).transform as RectTransform).rect.height;
			this._scrollPageHeight = this._viewport.rect.height;
			bool flag = this._contentHeight > this._viewport.rect.height;
			this._pageUpButton.gameObject.SetActive(flag);
			this._pageDownButton.gameObject.SetActive(flag);
			if (this._verticalScrollIndicator != null)
			{
				this._verticalScrollIndicator.gameObject.SetActive(flag);
				this._verticalScrollIndicator.normalizedPageHeight = this._viewport.rect.height / this._contentHeight;
			}
			this.ComputeScrollFocusPosY();
			RectTransform field = this._verticalScrollIndicator.GetField("_handle");
			field.sizeDelta = new Vector2(field.sizeDelta.x, Math.Abs(field.sizeDelta.y));
		}

		// Token: 0x040000B3 RID: 179
		private bool reserveButtonSpace;
	}
}
