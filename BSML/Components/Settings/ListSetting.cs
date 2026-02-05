using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000B5 RID: 181
	public class ListSetting : IncDecSetting
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00011E3E File Offset: 0x0001003E
		// (set) Token: 0x060003CA RID: 970 RVA: 0x00011E57 File Offset: 0x00010057
		public object Value
		{
			get
			{
				this.ValidateRange();
				return this.values[this.index];
			}
			set
			{
				this.index = this.values.IndexOf(value);
				if (this.index < 0)
				{
					this.index = 0;
				}
				this.UpdateState();
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00011429 File Offset: 0x0000F629
		public override void Setup()
		{
			this.ReceiveValue();
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00011E81 File Offset: 0x00010081
		public override void DecButtonPressed()
		{
			this.index--;
			this.EitherPressed();
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00011E97 File Offset: 0x00010097
		public override void IncButtonPressed()
		{
			this.index++;
			this.EitherPressed();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00011EAD File Offset: 0x000100AD
		private void EitherPressed()
		{
			this.UpdateState();
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

		// Token: 0x060003CF RID: 975 RVA: 0x00011EE4 File Offset: 0x000100E4
		public override void ApplyValue()
		{
			if (this.associatedValue != null)
			{
				this.associatedValue.SetValue(this.Value);
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00011EFF File Offset: 0x000100FF
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.Value = this.associatedValue.GetValue();
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00011F1A File Offset: 0x0001011A
		private void ValidateRange()
		{
			if (this.index >= this.values.Count)
			{
				this.index = this.values.Count - 1;
			}
			if (this.index < 0)
			{
				this.index = 0;
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00011F54 File Offset: 0x00010154
		private void UpdateState()
		{
			base.EnableDec = this.index > 0;
			base.EnableInc = this.index < this.values.Count - 1;
			base.Text = ((this.formatter == null) ? this.Value.ToString() : (this.formatter.Invoke(new object[] { this.Value }) as string));
		}

		// Token: 0x0400012B RID: 299
		private int index;

		// Token: 0x0400012C RID: 300
		public List<object> values;
	}
}
