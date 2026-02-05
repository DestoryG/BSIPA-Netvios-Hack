using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomSaber
{
	// Token: 0x02000009 RID: 9
	[AddComponentMenu("Custom Sabers/Every Nth Combo")]
	public class EveryNthComboFilter : EventFilterBehaviour
	{
		// Token: 0x06000012 RID: 18 RVA: 0x0000254A File Offset: 0x0000074A
		private void OnEnable()
		{
			base.EventManager.OnComboChanged.AddListener(new UnityAction<int>(this.OnComboStep));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002569 File Offset: 0x00000769
		private void OnDisable()
		{
			base.EventManager.OnComboChanged.RemoveListener(new UnityAction<int>(this.OnComboStep));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002588 File Offset: 0x00000788
		private void OnComboStep(int combo)
		{
			bool flag = combo % this.ComboStep == 0 && combo != 0;
			if (flag)
			{
				this.NthComboReached.Invoke();
			}
		}

		// Token: 0x04000026 RID: 38
		public int ComboStep = 50;

		// Token: 0x04000027 RID: 39
		public UnityEvent NthComboReached;
	}
}
