using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x0200009B RID: 155
	public class ClickableText : TextMeshProUGUI, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000F273 File Offset: 0x0000D473
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000F27B File Offset: 0x0000D47B
		public Color HighlightColor
		{
			get
			{
				return this._highlightColor;
			}
			set
			{
				this._highlightColor = value;
				this.UpdateHighlight();
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000F28A File Offset: 0x0000D48A
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000F292 File Offset: 0x0000D492
		public Color DefaultColor
		{
			get
			{
				return this._defaultColor;
			}
			set
			{
				this._defaultColor = value;
				this.UpdateHighlight();
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000F2A1 File Offset: 0x0000D4A1
		// (set) Token: 0x06000320 RID: 800 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		private bool IsHighlighted
		{
			get
			{
				return this._isHighlighted;
			}
			set
			{
				this._isHighlighted = value;
				this.UpdateHighlight();
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000F2B8 File Offset: 0x0000D4B8
		public void OnPointerClick(PointerEventData eventData)
		{
			this.IsHighlighted = false;
			Action<PointerEventData> onClickEvent = this.OnClickEvent;
			if (onClickEvent == null)
			{
				return;
			}
			onClickEvent(eventData);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000F2D2 File Offset: 0x0000D4D2
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.IsHighlighted = true;
			Action<PointerEventData> pointerEnterEvent = this.PointerEnterEvent;
			if (pointerEnterEvent == null)
			{
				return;
			}
			pointerEnterEvent(eventData);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000F2EC File Offset: 0x0000D4EC
		public void OnPointerExit(PointerEventData eventData)
		{
			this.IsHighlighted = false;
			Action<PointerEventData> pointerExitEvent = this.PointerExitEvent;
			if (pointerExitEvent == null)
			{
				return;
			}
			pointerExitEvent(eventData);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000F306 File Offset: 0x0000D506
		private void UpdateHighlight()
		{
			this.color = (this.IsHighlighted ? this.HighlightColor : this.DefaultColor);
		}

		// Token: 0x040000BF RID: 191
		private Color _highlightColor = new Color(0.6f, 0.8f, 1f);

		// Token: 0x040000C0 RID: 192
		private Color _defaultColor = Color.white;

		// Token: 0x040000C1 RID: 193
		public Action<PointerEventData> OnClickEvent;

		// Token: 0x040000C2 RID: 194
		public Action<PointerEventData> PointerEnterEvent;

		// Token: 0x040000C3 RID: 195
		public Action<PointerEventData> PointerExitEvent;

		// Token: 0x040000C4 RID: 196
		private bool _isHighlighted;
	}
}
