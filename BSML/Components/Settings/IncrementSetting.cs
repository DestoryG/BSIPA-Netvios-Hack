using System;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000B4 RID: 180
	public class IncrementSetting : IncDecSetting
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00011B05 File Offset: 0x0000FD05
		// (set) Token: 0x060003BD RID: 957 RVA: 0x00011B13 File Offset: 0x0000FD13
		public float Value
		{
			get
			{
				this.ValidateRange();
				return this.currentValue;
			}
			set
			{
				if (this.isInt)
				{
					this.currentValue = (float)Convert.ToInt32(value);
				}
				else
				{
					this.currentValue = value;
				}
				this.UpdateState();
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00011B3C File Offset: 0x0000FD3C
		public override void Setup()
		{
			this.ReceiveValue();
			if (this.isInt)
			{
				this.minValue = (float)this.ConvertToInt(this.minValue);
				this.maxValue = (float)this.ConvertToInt(this.maxValue);
				this.increments = (float)this.ConvertToInt(this.increments);
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00011B90 File Offset: 0x0000FD90
		public override void DecButtonPressed()
		{
			this.currentValue -= this.increments;
			this.EitherPressed();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00011BAB File Offset: 0x0000FDAB
		public override void IncButtonPressed()
		{
			this.currentValue += this.increments;
			this.EitherPressed();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00011BC8 File Offset: 0x0000FDC8
		private void EitherPressed()
		{
			this.UpdateState();
			if (this.isInt)
			{
				BSMLAction onChange = this.onChange;
				if (onChange != null)
				{
					onChange.Invoke(new object[] { Convert.ToInt32(this.Value) });
				}
			}
			else
			{
				BSMLAction onChange2 = this.onChange;
				if (onChange2 != null)
				{
					onChange2.Invoke(new object[] { this.Value });
				}
			}
			if (this.updateOnChange)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00011C44 File Offset: 0x0000FE44
		public override void ApplyValue()
		{
			if (this.associatedValue != null)
			{
				if (this.isInt)
				{
					this.associatedValue.SetValue(Convert.ToInt32(this.Value));
					return;
				}
				this.associatedValue.SetValue(this.Value);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00011C93 File Offset: 0x0000FE93
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.Value = (this.isInt ? ((float)Convert.ToInt32(this.associatedValue.GetValue())) : Convert.ToSingle(this.associatedValue.GetValue()));
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00011CCE File Offset: 0x0000FECE
		private void ValidateRange()
		{
			if (this.currentValue < this.minValue)
			{
				this.currentValue = this.minValue;
				return;
			}
			if (this.currentValue > this.maxValue)
			{
				this.currentValue = this.maxValue;
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00011D08 File Offset: 0x0000FF08
		private void UpdateState()
		{
			this.ValidateRange();
			base.EnableDec = this.currentValue > this.minValue;
			base.EnableInc = this.currentValue < this.maxValue;
			base.Text = this.TextForValue(this.currentValue);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00011D58 File Offset: 0x0000FF58
		protected string TextForValue(float value)
		{
			if (this.isInt)
			{
				if (this.formatter != null)
				{
					return this.formatter.Invoke(new object[] { this.ConvertToInt(value) }) as string;
				}
				return this.ConvertToInt(value).ToString();
			}
			else
			{
				if (this.formatter != null)
				{
					return this.formatter.Invoke(new object[] { value }) as string;
				}
				return value.ToString("N2");
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00011DE0 File Offset: 0x0000FFE0
		private int ConvertToInt(float value)
		{
			int num;
			if (value < -2.1474836E+09f)
			{
				num = int.MinValue;
			}
			else if (value > 2.1474836E+09f)
			{
				num = int.MaxValue;
			}
			else
			{
				num = Convert.ToInt32(value);
			}
			return num;
		}

		// Token: 0x04000126 RID: 294
		private float currentValue;

		// Token: 0x04000127 RID: 295
		public bool isInt;

		// Token: 0x04000128 RID: 296
		public float minValue = float.MinValue;

		// Token: 0x04000129 RID: 297
		public float maxValue = float.MaxValue;

		// Token: 0x0400012A RID: 298
		public float increments = 1f;
	}
}
