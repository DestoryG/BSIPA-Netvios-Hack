using System;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x02000369 RID: 873
	internal struct NetworkEvents
	{
		// Token: 0x04001DAB RID: 7595
		public AsyncEventBits Events;

		// Token: 0x04001DAC RID: 7596
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] ErrorCodes;
	}
}
