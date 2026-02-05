using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomSaber
{
	// Token: 0x02000004 RID: 4
	[AddComponentMenu("Custom Sabers/Combo Reached Event")]
	public class ComboReachedEvent : EventFilterBehaviour
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002193 File Offset: 0x00000393
		private void OnEnable()
		{
			base.EventManager.OnComboChanged.AddListener(new UnityAction<int>(this.OnComboReached));
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021B2 File Offset: 0x000003B2
		private void OnDisable()
		{
			base.EventManager.OnComboChanged.RemoveListener(new UnityAction<int>(this.OnComboReached));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021D4 File Offset: 0x000003D4
		private void OnComboReached(int combo)
		{
			bool flag = combo == this.ComboTarget;
			if (flag)
			{
				this.NthComboReached.Invoke();
			}
		}

		// Token: 0x0400000A RID: 10
		public int ComboTarget = 50;

		// Token: 0x0400000B RID: 11
		public UnityEvent NthComboReached;
	}
}
