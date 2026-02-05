using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000B6 RID: 182
	public class ListSliderSetting : GenericSliderSetting
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00011FC5 File Offset: 0x000101C5
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x00011FE4 File Offset: 0x000101E4
		public object Value
		{
			get
			{
				return this.values[(int)Math.Round((double)this.slider.value)];
			}
			set
			{
				this.slider.value = (float)this.values.IndexOf(value) * 1f;
				this.text.text = this.TextForValue(this.Value);
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001201C File Offset: 0x0001021C
		public override void Setup()
		{
			this.slider.minValue = 0f;
			this.slider.maxValue = (float)(this.values.Count<object>() - 1);
			this.text = this.slider.GetComponentInChildren<TextMeshProUGUI>();
			this.slider.numberOfSteps = this.values.Count;
			this.ReceiveValue();
			this.slider.valueDidChangeEvent += new Action<RangeValuesTextSlider, float>(this.OnChange);
			base.StartCoroutine(this.SetInitialText());
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000120A3 File Offset: 0x000102A3
		private void OnEnable()
		{
			base.StartCoroutine(this.SetInitialText());
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000120B2 File Offset: 0x000102B2
		private IEnumerator SetInitialText()
		{
			yield return new WaitForFixedUpdate();
			this.text.text = this.TextForValue(this.Value);
			yield return new WaitForSeconds(0.1f);
			this.text.text = this.TextForValue(this.Value);
			yield break;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000120C4 File Offset: 0x000102C4
		private void OnChange(TextSlider _, float val)
		{
			this.text.text = this.TextForValue(this.Value);
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

		// Token: 0x060003DA RID: 986 RVA: 0x00012117 File Offset: 0x00010317
		public override void ApplyValue()
		{
			if (this.associatedValue != null)
			{
				this.associatedValue.SetValue(this.Value);
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00012132 File Offset: 0x00010332
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.Value = this.associatedValue.GetValue();
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001214D File Offset: 0x0001034D
		protected string TextForValue(object value)
		{
			if (this.formatter != null)
			{
				return this.formatter.Invoke(new object[] { value }) as string;
			}
			return value.ToString();
		}

		// Token: 0x0400012D RID: 301
		public List<object> values;
	}
}
