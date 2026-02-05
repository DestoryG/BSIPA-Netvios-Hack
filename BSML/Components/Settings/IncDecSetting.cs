using System;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components.Settings
{
	// Token: 0x020000B3 RID: 179
	public abstract class IncDecSetting : GenericSetting
	{
		// Token: 0x170000EE RID: 238
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00011A63 File Offset: 0x0000FC63
		public bool EnableDec
		{
			set
			{
				this.decButton.interactable = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x00011A71 File Offset: 0x0000FC71
		public bool EnableInc
		{
			set
			{
				this.incButton.interactable = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00011A7F File Offset: 0x0000FC7F
		public string Text
		{
			set
			{
				this.text.text = value;
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00011A8D File Offset: 0x0000FC8D
		protected virtual void OnEnable()
		{
			this.incButton.onClick.AddListener(new UnityAction(this.IncButtonPressed));
			this.decButton.onClick.AddListener(new UnityAction(this.DecButtonPressed));
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00011AC9 File Offset: 0x0000FCC9
		protected void OnDisable()
		{
			this.incButton.onClick.RemoveListener(new UnityAction(this.IncButtonPressed));
			this.decButton.onClick.RemoveListener(new UnityAction(this.DecButtonPressed));
		}

		// Token: 0x060003B9 RID: 953
		public abstract void IncButtonPressed();

		// Token: 0x060003BA RID: 954
		public abstract void DecButtonPressed();

		// Token: 0x04000123 RID: 291
		public TextMeshProUGUI text;

		// Token: 0x04000124 RID: 292
		public Button decButton;

		// Token: 0x04000125 RID: 293
		public Button incButton;
	}
}
