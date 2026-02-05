using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002C9 RID: 713
	internal struct MibIcmpStatsEx
	{
		// Token: 0x040019F1 RID: 6641
		internal uint dwMsgs;

		// Token: 0x040019F2 RID: 6642
		internal uint dwErrors;

		// Token: 0x040019F3 RID: 6643
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		internal uint[] rgdwTypeCount;
	}
}
