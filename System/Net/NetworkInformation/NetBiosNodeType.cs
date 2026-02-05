using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002E5 RID: 741
	[global::__DynamicallyInvokable]
	public enum NetBiosNodeType
	{
		// Token: 0x04001A53 RID: 6739
		[global::__DynamicallyInvokable]
		Unknown,
		// Token: 0x04001A54 RID: 6740
		[global::__DynamicallyInvokable]
		Broadcast,
		// Token: 0x04001A55 RID: 6741
		[global::__DynamicallyInvokable]
		Peer2Peer,
		// Token: 0x04001A56 RID: 6742
		[global::__DynamicallyInvokable]
		Mixed = 4,
		// Token: 0x04001A57 RID: 6743
		[global::__DynamicallyInvokable]
		Hybrid = 8
	}
}
