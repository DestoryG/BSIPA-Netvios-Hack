using System;

namespace Microsoft.Win32
{
	// Token: 0x0200001D RID: 29
	public enum SessionSwitchReason
	{
		// Token: 0x04000308 RID: 776
		ConsoleConnect = 1,
		// Token: 0x04000309 RID: 777
		ConsoleDisconnect,
		// Token: 0x0400030A RID: 778
		RemoteConnect,
		// Token: 0x0400030B RID: 779
		RemoteDisconnect,
		// Token: 0x0400030C RID: 780
		SessionLogon,
		// Token: 0x0400030D RID: 781
		SessionLogoff,
		// Token: 0x0400030E RID: 782
		SessionLock,
		// Token: 0x0400030F RID: 783
		SessionUnlock,
		// Token: 0x04000310 RID: 784
		SessionRemoteControl
	}
}
