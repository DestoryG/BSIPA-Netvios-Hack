using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomSaber
{
	// Token: 0x02000008 RID: 8
	[AddComponentMenu("Custom Sabers/Event Manager")]
	public class EventManager : MonoBehaviour
	{
		// Token: 0x0400001A RID: 26
		public UnityEvent OnSlice;

		// Token: 0x0400001B RID: 27
		public UnityEvent OnComboBreak;

		// Token: 0x0400001C RID: 28
		public UnityEvent MultiplierUp;

		// Token: 0x0400001D RID: 29
		public UnityEvent SaberStartColliding;

		// Token: 0x0400001E RID: 30
		public UnityEvent SaberStopColliding;

		// Token: 0x0400001F RID: 31
		public UnityEvent OnLevelStart;

		// Token: 0x04000020 RID: 32
		public UnityEvent OnLevelFail;

		// Token: 0x04000021 RID: 33
		public UnityEvent OnLevelEnded;

		// Token: 0x04000022 RID: 34
		public UnityEvent OnBlueLightOn;

		// Token: 0x04000023 RID: 35
		public UnityEvent OnRedLightOn;

		// Token: 0x04000024 RID: 36
		[HideInInspector]
		public EventManager.ComboChangedEvent OnComboChanged = new EventManager.ComboChangedEvent();

		// Token: 0x04000025 RID: 37
		[HideInInspector]
		public EventManager.AccuracyChangedEvent OnAccuracyChanged = new EventManager.AccuracyChangedEvent();

		// Token: 0x0200001B RID: 27
		[Serializable]
		public class ComboChangedEvent : UnityEvent<int>
		{
		}

		// Token: 0x0200001C RID: 28
		[Serializable]
		public class AccuracyChangedEvent : UnityEvent<float>
		{
		}
	}
}
