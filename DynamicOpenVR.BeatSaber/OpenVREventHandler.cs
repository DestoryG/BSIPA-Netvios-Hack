using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x02000009 RID: 9
	internal class OpenVREventHandler : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000C RID: 12 RVA: 0x00002128 File Offset: 0x00000328
		// (remove) Token: 0x0600000D RID: 13 RVA: 0x00002160 File Offset: 0x00000360
		public event Action gamePaused;

		// Token: 0x0600000E RID: 14 RVA: 0x00002195 File Offset: 0x00000395
		private void Start()
		{
			this._evt = default(VREvent_t);
			this._size = (uint)Marshal.SizeOf<VREvent_t>();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021B0 File Offset: 0x000003B0
		private void Update()
		{
			if (OpenVR.System.PollNextEvent(ref this._evt, this._size) && this._pauseEvents.Contains((EVREventType)this._evt.eventType))
			{
				Action action = this.gamePaused;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x04000033 RID: 51
		private readonly HashSet<EVREventType> _pauseEvents = new HashSet<EVREventType>(new EVREventType[]
		{
			EVREventType.VREvent_InputFocusCaptured,
			EVREventType.VREvent_DashboardActivated,
			EVREventType.VREvent_OverlayShown
		});

		// Token: 0x04000035 RID: 53
		private VREvent_t _evt;

		// Token: 0x04000036 RID: 54
		private uint _size;
	}
}
