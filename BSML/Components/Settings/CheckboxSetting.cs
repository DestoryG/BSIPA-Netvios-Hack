using System;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000AE RID: 174
	public class CheckboxSetting : GenericSetting
	{
		// Token: 0x170000EA RID: 234
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0001154F File Offset: 0x0000F74F
		public bool EnableCheckbox
		{
			set
			{
				this.checkbox.interactable = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0001155D File Offset: 0x0000F75D
		public bool CheckboxValue
		{
			set
			{
				this.checkbox.isOn = value;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001156B File Offset: 0x0000F76B
		protected void OnEnable()
		{
			this.checkbox.onValueChanged.AddListener(new UnityAction<bool>(this.CheckboxToggled));
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00011589 File Offset: 0x0000F789
		protected void OnDisable()
		{
			this.checkbox.onValueChanged.RemoveListener(new UnityAction<bool>(this.CheckboxToggled));
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00011429 File Offset: 0x0000F629
		public override void Setup()
		{
			this.ReceiveValue();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000115A7 File Offset: 0x0000F7A7
		public void CheckboxToggled(bool value)
		{
			BSMLAction onChange = this.onChange;
			if (onChange != null)
			{
				onChange.Invoke(new object[] { this.checkbox.isOn });
			}
			if (this.updateOnChange)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000115E4 File Offset: 0x0000F7E4
		public override void ApplyValue()
		{
			if (this.associatedValue != null && this.checkbox.isOn != (bool)this.associatedValue.GetValue())
			{
				this.associatedValue.SetValue(this.checkbox.isOn);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00011631 File Offset: 0x0000F831
		public override void ReceiveValue()
		{
			if (this.associatedValue != null)
			{
				this.CheckboxValue = (bool)this.associatedValue.GetValue();
			}
		}

		// Token: 0x04000112 RID: 274
		public Toggle checkbox;
	}
}
