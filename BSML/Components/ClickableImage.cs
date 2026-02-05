using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x0200009A RID: 154
	public class ClickableImage : Image, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000F195 File Offset: 0x0000D395
		// (set) Token: 0x06000311 RID: 785 RVA: 0x0000F19D File Offset: 0x0000D39D
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

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000F1AC File Offset: 0x0000D3AC
		// (set) Token: 0x06000313 RID: 787 RVA: 0x0000F1B4 File Offset: 0x0000D3B4
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000F1C3 File Offset: 0x0000D3C3
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000F1CB File Offset: 0x0000D3CB
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

		// Token: 0x06000316 RID: 790 RVA: 0x0000F1DA File Offset: 0x0000D3DA
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

		// Token: 0x06000317 RID: 791 RVA: 0x0000F1F4 File Offset: 0x0000D3F4
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

		// Token: 0x06000318 RID: 792 RVA: 0x0000F20E File Offset: 0x0000D40E
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

		// Token: 0x06000319 RID: 793 RVA: 0x0000F228 File Offset: 0x0000D428
		private void UpdateHighlight()
		{
			this.color = (this.IsHighlighted ? this.HighlightColor : this.DefaultColor);
		}

		// Token: 0x040000B9 RID: 185
		private Color _highlightColor = new Color(0.6f, 0.8f, 1f);

		// Token: 0x040000BA RID: 186
		private Color _defaultColor = Color.white;

		// Token: 0x040000BB RID: 187
		public Action<PointerEventData> OnClickEvent;

		// Token: 0x040000BC RID: 188
		public Action<PointerEventData> PointerEnterEvent;

		// Token: 0x040000BD RID: 189
		public Action<PointerEventData> PointerExitEvent;

		// Token: 0x040000BE RID: 190
		private bool _isHighlighted;
	}
}
