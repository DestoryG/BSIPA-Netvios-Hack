using System;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CustomAvatar
{
	// Token: 0x02000016 RID: 22
	public class ComboReachedEvent : EventFilterBehaviour
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00003262 File Offset: 0x00001462
		private void OnEnable()
		{
			base.EventManager.OnComboChanged.AddListener(new UnityAction<int>(this.OnComboReached));
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003282 File Offset: 0x00001482
		private void OnDisable()
		{
			base.EventManager.OnComboChanged.RemoveListener(new UnityAction<int>(this.OnComboReached));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000032A4 File Offset: 0x000014A4
		private void OnComboReached(int combo)
		{
			bool flag = combo == this.ComboTarget;
			if (flag)
			{
				this.ComboReached.Invoke();
			}
		}

		// Token: 0x04000062 RID: 98
		public int ComboTarget = 50;

		// Token: 0x04000063 RID: 99
		[FormerlySerializedAs("NthComboReached")]
		public UnityEvent ComboReached;
	}
}
