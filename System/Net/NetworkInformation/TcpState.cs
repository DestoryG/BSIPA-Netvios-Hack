using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000306 RID: 774
	[global::__DynamicallyInvokable]
	public enum TcpState
	{
		// Token: 0x04001AE9 RID: 6889
		[global::__DynamicallyInvokable]
		Unknown,
		// Token: 0x04001AEA RID: 6890
		[global::__DynamicallyInvokable]
		Closed,
		// Token: 0x04001AEB RID: 6891
		[global::__DynamicallyInvokable]
		Listen,
		// Token: 0x04001AEC RID: 6892
		[global::__DynamicallyInvokable]
		SynSent,
		// Token: 0x04001AED RID: 6893
		[global::__DynamicallyInvokable]
		SynReceived,
		// Token: 0x04001AEE RID: 6894
		[global::__DynamicallyInvokable]
		Established,
		// Token: 0x04001AEF RID: 6895
		[global::__DynamicallyInvokable]
		FinWait1,
		// Token: 0x04001AF0 RID: 6896
		[global::__DynamicallyInvokable]
		FinWait2,
		// Token: 0x04001AF1 RID: 6897
		[global::__DynamicallyInvokable]
		CloseWait,
		// Token: 0x04001AF2 RID: 6898
		[global::__DynamicallyInvokable]
		Closing,
		// Token: 0x04001AF3 RID: 6899
		[global::__DynamicallyInvokable]
		LastAck,
		// Token: 0x04001AF4 RID: 6900
		[global::__DynamicallyInvokable]
		TimeWait,
		// Token: 0x04001AF5 RID: 6901
		[global::__DynamicallyInvokable]
		DeleteTcb
	}
}
