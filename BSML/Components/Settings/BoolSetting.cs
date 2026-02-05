using System;
using BeatSaberMarkupLanguage.Parser;
using Polyglot;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000AD RID: 173
	public class BoolSetting : IncDecSetting
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00011412 File Offset: 0x0000F612
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0001141A File Offset: 0x0000F61A
		public bool Value
		{
			get
			{
				return this.currentValue;
			}
			set
			{
				this.currentValue = value;
				this.UpdateState();
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00011429 File Offset: 0x0000F629
		public override void Setup()
		{
			this.ReceiveValue();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00011431 File Offset: 0x0000F631
		public override void DecButtonPressed()
		{
			this.Value = false;
			this.EitherPressed();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00011440 File Offset: 0x0000F640
		public override void IncButtonPressed()
		{
			this.Value = true;
			this.EitherPressed();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001144F File Offset: 0x0000F64F
		private void EitherPressed()
		{
			BSMLAction onChange = this.onChange;
			if (onChange != null)
			{
				onChange.Invoke(new object[] { this.Value });
			}
			if (this.updateOnChange)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00011485 File Offset: 0x0000F685
		public override void ApplyValue()
		{
			if (this.associatedValue != null)
			{
				this.associatedValue.SetValue(this.Value);
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000114A5 File Offset: 0x0000F6A5
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.Value = (bool)this.associatedValue.GetValue();
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000114C8 File Offset: 0x0000F6C8
		private void UpdateState()
		{
			base.EnableDec = this.currentValue;
			base.EnableInc = !this.currentValue;
			if (this.formatter != null)
			{
				base.Text = this.formatter.Invoke(new object[] { this.currentValue }) as string;
				return;
			}
			base.Text = (this.currentValue ? Localization.Get("SETTINGS_ON") : Localization.Get("SETTINGS_OFF"));
		}

		// Token: 0x04000111 RID: 273
		private bool currentValue;
	}
}
