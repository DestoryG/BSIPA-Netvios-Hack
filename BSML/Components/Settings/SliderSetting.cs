using System;
using System.Collections;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000B7 RID: 183
	public class SliderSetting : GenericSliderSetting
	{
		// Token: 0x060003DE RID: 990 RVA: 0x00012180 File Offset: 0x00010380
		public override void Setup()
		{
			this.text = this.slider.GetComponentInChildren<TextMeshProUGUI>();
			this.slider.numberOfSteps = (int)Math.Round((double)((this.slider.maxValue - this.slider.minValue) / this.increments)) + 1;
			this.ReceiveValue();
			this.slider.valueDidChangeEvent += new Action<RangeValuesTextSlider, float>(this.OnChange);
			base.StartCoroutine(this.SetInitialText());
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000121FA File Offset: 0x000103FA
		private void OnEnable()
		{
			base.StartCoroutine(this.SetInitialText());
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00012209 File Offset: 0x00010409
		private IEnumerator SetInitialText()
		{
			yield return new WaitForFixedUpdate();
			this.text.text = this.TextForValue(this.slider.value);
			yield return new WaitForSeconds(0.1f);
			this.text.text = this.TextForValue(this.slider.value);
			yield break;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00012218 File Offset: 0x00010418
		private void OnChange(TextSlider _, float val)
		{
			this.text.text = this.TextForValue(this.slider.value);
			if (this.isInt)
			{
				BSMLAction onChange = this.onChange;
				if (onChange != null)
				{
					onChange.Invoke(new object[] { (int)Math.Round((double)this.slider.value) });
				}
			}
			else
			{
				BSMLAction onChange2 = this.onChange;
				if (onChange2 != null)
				{
					onChange2.Invoke(new object[] { this.slider.value });
				}
			}
			if (this.updateOnChange)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000122B8 File Offset: 0x000104B8
		public override void ApplyValue()
		{
			if (this.associatedValue != null)
			{
				if (this.isInt)
				{
					this.associatedValue.SetValue((int)Math.Round((double)this.slider.value));
					return;
				}
				this.associatedValue.SetValue(this.slider.value);
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00012314 File Offset: 0x00010514
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.slider.value = (this.isInt ? ((float)((int)this.associatedValue.GetValue())) : ((float)this.associatedValue.GetValue()));
			}
			this.text.text = this.TextForValue(this.slider.value);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001237C File Offset: 0x0001057C
		protected string TextForValue(float value)
		{
			if (this.isInt)
			{
				if (this.formatter != null)
				{
					return this.formatter.Invoke(new object[] { (int)Math.Round((double)value) }) as string;
				}
				return ((int)Math.Round((double)value)).ToString();
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

		// Token: 0x0400012E RID: 302
		public bool isInt;

		// Token: 0x0400012F RID: 303
		public float increments;
	}
}
