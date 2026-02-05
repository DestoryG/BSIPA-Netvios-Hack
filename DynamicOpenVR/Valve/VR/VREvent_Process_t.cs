using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000089 RID: 137
	public struct VREvent_Process_t
	{
		// Token: 0x040005E8 RID: 1512
		public uint pid;

		// Token: 0x040005E9 RID: 1513
		public uint oldPid;

		// Token: 0x040005EA RID: 1514
		[MarshalAs(UnmanagedType.I1)]
		public bool bForced;

		// Token: 0x040005EB RID: 1515
		[MarshalAs(UnmanagedType.I1)]
		public bool bConnectionLost;
	}
}
