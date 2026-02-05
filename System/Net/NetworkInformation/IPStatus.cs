using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002A9 RID: 681
	public enum IPStatus
	{
		// Token: 0x040018EA RID: 6378
		Success,
		// Token: 0x040018EB RID: 6379
		DestinationNetworkUnreachable = 11002,
		// Token: 0x040018EC RID: 6380
		DestinationHostUnreachable,
		// Token: 0x040018ED RID: 6381
		DestinationProtocolUnreachable,
		// Token: 0x040018EE RID: 6382
		DestinationPortUnreachable,
		// Token: 0x040018EF RID: 6383
		DestinationProhibited = 11004,
		// Token: 0x040018F0 RID: 6384
		NoResources = 11006,
		// Token: 0x040018F1 RID: 6385
		BadOption,
		// Token: 0x040018F2 RID: 6386
		HardwareError,
		// Token: 0x040018F3 RID: 6387
		PacketTooBig,
		// Token: 0x040018F4 RID: 6388
		TimedOut,
		// Token: 0x040018F5 RID: 6389
		BadRoute = 11012,
		// Token: 0x040018F6 RID: 6390
		TtlExpired,
		// Token: 0x040018F7 RID: 6391
		TtlReassemblyTimeExceeded,
		// Token: 0x040018F8 RID: 6392
		ParameterProblem,
		// Token: 0x040018F9 RID: 6393
		SourceQuench,
		// Token: 0x040018FA RID: 6394
		BadDestination = 11018,
		// Token: 0x040018FB RID: 6395
		DestinationUnreachable = 11040,
		// Token: 0x040018FC RID: 6396
		TimeExceeded,
		// Token: 0x040018FD RID: 6397
		BadHeader,
		// Token: 0x040018FE RID: 6398
		UnrecognizedNextHeader,
		// Token: 0x040018FF RID: 6399
		IcmpError,
		// Token: 0x04001900 RID: 6400
		DestinationScopeMismatch,
		// Token: 0x04001901 RID: 6401
		Unknown = -1
	}
}
