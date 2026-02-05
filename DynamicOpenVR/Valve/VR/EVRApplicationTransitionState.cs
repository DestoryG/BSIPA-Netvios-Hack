using System;

namespace Valve.VR
{
	// Token: 0x02000050 RID: 80
	public enum EVRApplicationTransitionState
	{
		// Token: 0x040004A1 RID: 1185
		VRApplicationTransition_None,
		// Token: 0x040004A2 RID: 1186
		VRApplicationTransition_OldAppQuitSent = 10,
		// Token: 0x040004A3 RID: 1187
		VRApplicationTransition_WaitingForExternalLaunch,
		// Token: 0x040004A4 RID: 1188
		VRApplicationTransition_NewAppLaunched = 20
	}
}
