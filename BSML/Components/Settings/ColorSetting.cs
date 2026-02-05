using System;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000AF RID: 175
	public class ColorSetting : GenericSetting
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00011659 File Offset: 0x0000F859
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00011661 File Offset: 0x0000F861
		public Color CurrentColor
		{
			get
			{
				return this._currentColor;
			}
			set
			{
				this._currentColor = value;
				if (this.colorImage != null)
				{
					this.colorImage.color = this._currentColor;
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001168C File Offset: 0x0000F88C
		public override void Setup()
		{
			this.modalColorPicker.onChange = this.onChange;
			ModalColorPicker modalColorPicker = this.modalColorPicker;
			modalColorPicker.doneEvent = (Action<Color>)Delegate.Combine(modalColorPicker.doneEvent, new Action<Color>(this.DonePressed));
			ModalColorPicker modalColorPicker2 = this.modalColorPicker;
			modalColorPicker2.cancelEvent = (Action)Delegate.Combine(modalColorPicker2.cancelEvent, new Action(this.CancelPressed));
			this.ReceiveValue();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000116FE File Offset: 0x0000F8FE
		protected virtual void OnEnable()
		{
			this.editButton.onClick.AddListener(new UnityAction(this.EditButtonPressed));
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001171C File Offset: 0x0000F91C
		protected void OnDisable()
		{
			this.editButton.onClick.RemoveListener(new UnityAction(this.EditButtonPressed));
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001173A File Offset: 0x0000F93A
		public void EditButtonPressed()
		{
			this.modalColorPicker.CurrentColor = this.CurrentColor;
			this.modalColorPicker.modalView.Show(true, true, null);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00011760 File Offset: 0x0000F960
		public void DonePressed(Color color)
		{
			this.CurrentColor = color;
			if (this.updateOnChange)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00011777 File Offset: 0x0000F977
		public void CancelPressed()
		{
			BSMLAction onChange = this.onChange;
			if (onChange == null)
			{
				return;
			}
			onChange.Invoke(new object[] { this.CurrentColor });
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001179E File Offset: 0x0000F99E
		public override void ApplyValue()
		{
			if (this.associatedValue != null)
			{
				this.associatedValue.SetValue(this.CurrentColor);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000117BE File Offset: 0x0000F9BE
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.CurrentColor = (Color)this.associatedValue.GetValue();
			}
		}

		// Token: 0x04000113 RID: 275
		public Button editButton;

		// Token: 0x04000114 RID: 276
		public ModalColorPicker modalColorPicker;

		// Token: 0x04000115 RID: 277
		public Image colorImage;

		// Token: 0x04000116 RID: 278
		private Color _currentColor = Color.white;
	}
}
