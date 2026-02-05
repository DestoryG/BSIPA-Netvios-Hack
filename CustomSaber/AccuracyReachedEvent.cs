using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomSaber
{
	// Token: 0x02000003 RID: 3
	[AddComponentMenu("Custom Sabers/Accuracy Reached Event")]
	public class AccuracyReachedEvent : EventFilterBehaviour
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002081 File Offset: 0x00000281
		private void OnEnable()
		{
			base.EventManager.OnAccuracyChanged.AddListener(new UnityAction<float>(this.OnAccuracyReached));
			this.prevAccuracy = 1f;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020AC File Offset: 0x000002AC
		private void OnDisable()
		{
			base.EventManager.OnAccuracyChanged.RemoveListener(new UnityAction<float>(this.OnAccuracyReached));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020CC File Offset: 0x000002CC
		private void OnAccuracyReached(float accuracy)
		{
			bool flag = (this.prevAccuracy > this.Target && accuracy < this.Target) || (this.prevAccuracy < this.Target && accuracy > this.Target);
			if (flag)
			{
				this.OnAccuracyReachTarget.Invoke();
			}
			bool flag2 = this.prevAccuracy < this.Target && accuracy > this.Target;
			if (flag2)
			{
				this.OnAccuracyHigherThanTarget.Invoke();
			}
			bool flag3 = this.prevAccuracy > this.Target && accuracy < this.Target;
			if (flag3)
			{
				this.OnAccuracyLowerThanTarget.Invoke();
			}
			this.prevAccuracy = accuracy;
		}

		// Token: 0x04000005 RID: 5
		[Tooltip("Event will be triggered when accuracy crosses this value, expressed as a value between 0 and 1")]
		[Range(0f, 1f)]
		public float Target = 1f;

		// Token: 0x04000006 RID: 6
		public UnityEvent OnAccuracyReachTarget;

		// Token: 0x04000007 RID: 7
		public UnityEvent OnAccuracyHigherThanTarget;

		// Token: 0x04000008 RID: 8
		public UnityEvent OnAccuracyLowerThanTarget;

		// Token: 0x04000009 RID: 9
		private float prevAccuracy;
	}
}
