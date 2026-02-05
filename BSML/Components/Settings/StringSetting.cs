using System;
using BeatSaberMarkupLanguage.Parser;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000B8 RID: 184
	public class StringSetting : GenericSetting
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00012405 File Offset: 0x00010605
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x0001240D File Offset: 0x0001060D
		public string Text
		{
			get
			{
				return this.currentValue;
			}
			set
			{
				this.currentValue = value;
				this.text.text = ((this.formatter == null) ? value : (this.formatter.Invoke(new object[] { value }) as string));
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00012448 File Offset: 0x00010648
		private void Update()
		{
			this.boundingBox.sizeDelta = new Vector2(this.text.textBounds.size.x + 7f, 0f);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00012488 File Offset: 0x00010688
		public override void Setup()
		{
			this.modalKeyboard.clearOnOpen = false;
			this.ReceiveValue();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001249C File Offset: 0x0001069C
		protected virtual void OnEnable()
		{
			this.editButton.onClick.AddListener(new UnityAction(this.EditButtonPressed));
			this.modalKeyboard.keyboard.EnterPressed += this.EnterPressed;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000124D6 File Offset: 0x000106D6
		protected void OnDisable()
		{
			this.editButton.onClick.RemoveListener(new UnityAction(this.EditButtonPressed));
			this.modalKeyboard.keyboard.EnterPressed -= this.EnterPressed;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00012510 File Offset: 0x00010710
		public void EditButtonPressed()
		{
			this.modalKeyboard.modalView.Show(true, true, null);
			this.modalKeyboard.SetText(this.Text);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00012536 File Offset: 0x00010736
		public void EnterPressed(string text)
		{
			this.Text = text;
			BSMLAction onChange = this.onChange;
			if (onChange != null)
			{
				onChange.Invoke(new object[] { this.Text });
			}
			if (this.updateOnChange)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001256E File Offset: 0x0001076E
		public override void ApplyValue()
		{
			if (this.associatedValue != null)
			{
				this.associatedValue.SetValue(this.Text);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00012589 File Offset: 0x00010789
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.Text = (string)this.associatedValue.GetValue();
			}
		}

		// Token: 0x04000130 RID: 304
		public TextMeshProUGUI text;

		// Token: 0x04000131 RID: 305
		public Button editButton;

		// Token: 0x04000132 RID: 306
		public RectTransform boundingBox;

		// Token: 0x04000133 RID: 307
		public ModalKeyboard modalKeyboard;

		// Token: 0x04000134 RID: 308
		private string currentValue;
	}
}
