using System;
using UnityEngine.Events;

namespace CustomAvatar
{
	// Token: 0x02000015 RID: 21
	public class EveryNthComboFilter : EventFilterBehaviour
	{
		// Token: 0x06000034 RID: 52 RVA: 0x000031DE File Offset: 0x000013DE
		private void OnEnable()
		{
			base.EventManager.OnComboChanged.AddListener(new UnityAction<int>(this.OnComboStep));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000031FE File Offset: 0x000013FE
		private void OnDisable()
		{
			base.EventManager.OnComboChanged.RemoveListener(new UnityAction<int>(this.OnComboStep));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003220 File Offset: 0x00001420
		private void OnComboStep(int combo)
		{
			bool flag = combo % this.ComboStep == 0 && combo != 0;
			if (flag)
			{
				this.NthComboReached.Invoke();
			}
		}

		// Token: 0x04000060 RID: 96
		public int ComboStep = 50;

		// Token: 0x04000061 RID: 97
		public UnityEvent NthComboReached;
	}
}
