using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAvatar
{
	// Token: 0x02000018 RID: 24
	public class EventManager : MonoBehaviour
	{
		// Token: 0x040000A0 RID: 160
		public UnityEvent OnSlice;

		// Token: 0x040000A1 RID: 161
		public UnityEvent OnComboBreak;

		// Token: 0x040000A2 RID: 162
		public UnityEvent MultiplierUp;

		// Token: 0x040000A3 RID: 163
		public UnityEvent SaberStartColliding;

		// Token: 0x040000A4 RID: 164
		public UnityEvent SaberStopColliding;

		// Token: 0x040000A5 RID: 165
		public UnityEvent OnMenuEnter;

		// Token: 0x040000A6 RID: 166
		public UnityEvent OnLevelStart;

		// Token: 0x040000A7 RID: 167
		public UnityEvent OnLevelFail;

		// Token: 0x040000A8 RID: 168
		public UnityEvent OnLevelFinish;

		// Token: 0x040000A9 RID: 169
		public UnityEvent OnBlueLightOn;

		// Token: 0x040000AA RID: 170
		public UnityEvent OnRedLightOn;

		// Token: 0x040000AB RID: 171
		public EventManager.ComboChangedEvent OnComboChanged = new EventManager.ComboChangedEvent();

		// Token: 0x02000040 RID: 64
		[Serializable]
		public class ComboChangedEvent : UnityEvent<int>
		{
		}
	}
}
