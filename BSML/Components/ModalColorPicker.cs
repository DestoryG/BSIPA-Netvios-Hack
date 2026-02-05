using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A0 RID: 160
	public class ModalColorPicker : MonoBehaviour
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000F693 File Offset: 0x0000D893
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000F69C File Offset: 0x0000D89C
		public Color CurrentColor
		{
			get
			{
				return this._currentColor;
			}
			set
			{
				this._currentColor = value;
				if (this.rgbPanel != null)
				{
					this.rgbPanel.color = this._currentColor;
				}
				if (this.hsvPanel != null && this.hsvPanel.color != this._currentColor)
				{
					this.hsvPanel.color = this._currentColor;
				}
				if (this.colorImage != null)
				{
					this.colorImage.color = this._currentColor;
				}
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000F725 File Offset: 0x0000D925
		private void OnEnable()
		{
			if (this.associatedValue != null)
			{
				this.CurrentColor = (Color)this.associatedValue.GetValue();
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000F745 File Offset: 0x0000D945
		[UIAction("cancel")]
		public void CancelPressed()
		{
			BSMLAction bsmlaction = this.onCancel;
			if (bsmlaction != null)
			{
				bsmlaction.Invoke(Array.Empty<object>());
			}
			Action action = this.cancelEvent;
			if (action != null)
			{
				action();
			}
			this.modalView.Hide(true, null);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000F77C File Offset: 0x0000D97C
		[UIAction("done")]
		public void DonePressed()
		{
			BSMLValue bsmlvalue = this.associatedValue;
			if (bsmlvalue != null)
			{
				bsmlvalue.SetValue(this.CurrentColor);
			}
			BSMLAction bsmlaction = this.onDone;
			if (bsmlaction != null)
			{
				bsmlaction.Invoke(new object[] { this.CurrentColor });
			}
			Action<Color> action = this.doneEvent;
			if (action != null)
			{
				action(this.CurrentColor);
			}
			this.modalView.Hide(true, null);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000F7EF File Offset: 0x0000D9EF
		public void OnChange(Color color, ColorChangeUIEventType type)
		{
			BSMLAction bsmlaction = this.onChange;
			if (bsmlaction != null)
			{
				bsmlaction.Invoke(new object[] { color });
			}
			this.CurrentColor = color;
		}

		// Token: 0x040000D0 RID: 208
		public ModalView modalView;

		// Token: 0x040000D1 RID: 209
		public RGBPanelController rgbPanel;

		// Token: 0x040000D2 RID: 210
		public HSVPanelController hsvPanel;

		// Token: 0x040000D3 RID: 211
		public Image colorImage;

		// Token: 0x040000D4 RID: 212
		public BSMLValue associatedValue;

		// Token: 0x040000D5 RID: 213
		public BSMLAction onCancel;

		// Token: 0x040000D6 RID: 214
		public BSMLAction onDone;

		// Token: 0x040000D7 RID: 215
		public BSMLAction onChange;

		// Token: 0x040000D8 RID: 216
		public Action<Color> doneEvent;

		// Token: 0x040000D9 RID: 217
		public Action cancelEvent;

		// Token: 0x040000DA RID: 218
		private Color _currentColor;
	}
}
