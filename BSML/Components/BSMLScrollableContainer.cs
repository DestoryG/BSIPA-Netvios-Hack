using System;
using System.Linq;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x02000096 RID: 150
	public class BSMLScrollableContainer : ScrollView
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000E9F1 File Offset: 0x0000CBF1
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000E9F9 File Offset: 0x0000CBF9
		public Button PageUpButton
		{
			get
			{
				return this._pageUpButton;
			}
			set
			{
				this._pageUpButton = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000EA02 File Offset: 0x0000CC02
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000EA0A File Offset: 0x0000CC0A
		public Button PageDownButton
		{
			get
			{
				return this._pageDownButton;
			}
			set
			{
				this._pageDownButton = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000EA13 File Offset: 0x0000CC13
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0000EA1B File Offset: 0x0000CC1B
		public RectTransform Viewport
		{
			get
			{
				return this._viewport;
			}
			set
			{
				this._viewport = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000EA24 File Offset: 0x0000CC24
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x0000EA2C File Offset: 0x0000CC2C
		public RectTransform ContentRect
		{
			get
			{
				return this._contentRectTransform;
			}
			set
			{
				this._contentRectTransform = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000EA35 File Offset: 0x0000CC35
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x0000EA3D File Offset: 0x0000CC3D
		public VerticalScrollIndicator ScrollIndicator
		{
			get
			{
				return this._verticalScrollIndicator;
			}
			set
			{
				this._verticalScrollIndicator = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000EA46 File Offset: 0x0000CC46
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x0000EA4E File Offset: 0x0000CC4E
		public bool AlignBottom
		{
			get
			{
				return this.alignBottom;
			}
			set
			{
				this.alignBottom = value;
				this.ScrollAt(this._dstPosY, true);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000EA64 File Offset: 0x0000CC64
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		public bool MaskOverflow
		{
			get
			{
				return this.maskOverflow;
			}
			set
			{
				this.maskOverflow = value;
				this.UpdateViewportMask();
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000EA7C File Offset: 0x0000CC7C
		private void UpdateViewportMask()
		{
			Image component = this.Viewport.GetComponent<Image>();
			if (component != null)
			{
				component.enabled = this.MaskOverflow;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000EAAA File Offset: 0x0000CCAA
		public void Awake()
		{
			this._buttonBinder = new ButtonBinder();
			this.Setup();
			this.RefreshButtonsInteractibility();
			this.runScrollAnim = false;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000EACA File Offset: 0x0000CCCA
		public void Setup()
		{
			this.RefreshContent();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
		public void RefreshBindings()
		{
			this._buttonBinder.ClearBindings();
			if (this.PageUpButton != null)
			{
				this._buttonBinder.AddBinding(this.PageUpButton, new Action(this.PageUpButtonPressed));
			}
			if (this.PageDownButton != null)
			{
				this._buttonBinder.AddBinding(this.PageDownButton, new Action(this.PageDownButtonPressed));
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000EB44 File Offset: 0x0000CD44
		public void RefreshContent()
		{
			this._contentHeight = this._contentRectTransform.rect.height;
			this._scrollPageHeight = this._viewport.rect.height;
			bool flag = this._contentHeight > this._viewport.rect.height;
			Button pageUpButton = this.PageUpButton;
			if (pageUpButton != null)
			{
				GameObject gameObject = pageUpButton.gameObject;
				if (gameObject != null)
				{
					gameObject.SetActive(flag);
				}
			}
			Button pageDownButton = this.PageDownButton;
			if (pageDownButton != null)
			{
				GameObject gameObject2 = pageDownButton.gameObject;
				if (gameObject2 != null)
				{
					gameObject2.SetActive(flag);
				}
			}
			this.RefreshBindings();
			if (this._verticalScrollIndicator != null)
			{
				this._verticalScrollIndicator.gameObject.SetActive(flag);
				this._verticalScrollIndicator.normalizedPageHeight = this._viewport.rect.height / this._contentHeight;
			}
			this.ComputeScrollFocusPosY();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000EC29 File Offset: 0x0000CE29
		public void ContentSizeUpdated()
		{
			this.RefreshContent();
			this.RefreshButtonsInteractibility();
			this.ScrollAt(0f, false);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000EC44 File Offset: 0x0000CE44
		public void Update()
		{
			if (this._contentHeight != this._contentRectTransform.rect.height && this._contentRectTransform.rect.height > 0f)
			{
				this.ContentSizeUpdated();
			}
			if (this.runScrollAnim)
			{
				float num = Mathf.Lerp(this._contentRectTransform.anchoredPosition.y, this._dstPosY, Time.deltaTime * this._smooth);
				if (Mathf.Abs(num - this._dstPosY) < 0.01f)
				{
					num = this._dstPosY;
					this.runScrollAnim = false;
				}
				this._contentRectTransform.anchoredPosition = new Vector2(0f, num);
				this.UpdateVerticalScrollIndicator(this._contentRectTransform.anchoredPosition.y);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000ED0C File Offset: 0x0000CF0C
		public void RefreshButtonsInteractibility()
		{
			if (this.PageUpButton != null)
			{
				this.PageUpButton.interactable = this._dstPosY > 0f;
			}
			if (this.PageDownButton != null)
			{
				Selectable pageDownButton = this.PageDownButton;
				float dstPosY = this._dstPosY;
				float contentHeight = this._contentHeight;
				RectTransform viewport = this._viewport;
				pageDownButton.interactable = dstPosY < contentHeight - ((viewport != null) ? viewport.rect.height : 0f);
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000ED88 File Offset: 0x0000CF88
		public void ComputeScrollFocusPosY()
		{
			ItemForFocussedScrolling[] componentsInChildren = base.GetComponentsInChildren<ItemForFocussedScrolling>(true);
			this._scrollFocusPosYs = (from item in componentsInChildren
				select this.WorldPositionToScrollViewPosition(item.transform.position).y into i
				orderby i
				select i).ToArray<float>();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000EDE0 File Offset: 0x0000CFE0
		public void UpdateVerticalScrollIndicator(float posY)
		{
			if (this._verticalScrollIndicator != null)
			{
				this._verticalScrollIndicator.progress = posY / (this._contentHeight - this._viewport.rect.height);
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000EE24 File Offset: 0x0000D024
		public void ScrollDown(bool animated)
		{
			float num = this._contentHeight - this._viewport.rect.height;
			this.ScrollAt(num, animated);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000EE54 File Offset: 0x0000D054
		public void ScrollToWorldPosition(Vector3 worldPosition, float pageRelativePosition, bool animated)
		{
			float num = this.WorldPositionToScrollViewPosition(worldPosition).y;
			num -= pageRelativePosition * this._scrollPageHeight;
			this.ScrollAt(num, animated);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000EE84 File Offset: 0x0000D084
		public void ScrollToWorldPositionIfOutsideArea(Vector3 worldPosition, float pageRelativePosition, float relativeBoundaryStart, float relativeBoundaryEnd, bool animated)
		{
			float num = this.WorldPositionToScrollViewPosition(worldPosition).y;
			float num2 = this._dstPosY + relativeBoundaryStart * this._scrollPageHeight;
			float num3 = this._dstPosY + relativeBoundaryEnd * this._scrollPageHeight;
			if (num > num2 && num < num3)
			{
				return;
			}
			num -= pageRelativePosition * this._scrollPageHeight;
			this.ScrollAt(num, animated);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000EEDC File Offset: 0x0000D0DC
		public void ScrollAt(float dstPosY, bool animated)
		{
			this.SetDstPosY(dstPosY);
			if (!animated)
			{
				this._contentRectTransform.anchoredPosition = new Vector2(0f, this._dstPosY);
			}
			this.RefreshButtonsInteractibility();
			this.runScrollAnim = true;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000EF10 File Offset: 0x0000D110
		public void PageUpButtonPressed()
		{
			float threshold = this._dstPosY + this._scrollItemRelativeThresholdPosition * this._scrollPageHeight;
			float num = this._scrollFocusPosYs.Where((float posy) => posy < threshold).DefaultIfEmpty(this._dstPosY).Max();
			num -= this._pageStepRelativePosition * this._scrollPageHeight;
			this.SetDstPosY(num);
			this.RefreshButtonsInteractibility();
			this.runScrollAnim = true;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000EF8C File Offset: 0x0000D18C
		public void PageDownButtonPressed()
		{
			float threshold = this._dstPosY + (1f - this._scrollItemRelativeThresholdPosition) * this._scrollPageHeight;
			float num = this._scrollFocusPosYs.Where((float posy) => posy > threshold).DefaultIfEmpty(this._dstPosY + this._scrollPageHeight).Min();
			num -= (1f - this._pageStepRelativePosition) * this._scrollPageHeight;
			this.SetDstPosY(num);
			this.RefreshButtonsInteractibility();
			this.runScrollAnim = true;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000F018 File Offset: 0x0000D218
		public void SetDstPosY(float value)
		{
			float num = this._contentHeight - this._viewport.rect.height;
			if (num < 0f && !this.AlignBottom)
			{
				num = 0f;
			}
			this._dstPosY = Mathf.Min(num, Mathf.Max(0f, value));
		}

		// Token: 0x040000B4 RID: 180
		private bool alignBottom;

		// Token: 0x040000B5 RID: 181
		private bool maskOverflow = true;

		// Token: 0x040000B6 RID: 182
		private bool runScrollAnim;
	}
}
