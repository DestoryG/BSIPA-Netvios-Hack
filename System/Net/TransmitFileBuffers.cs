using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001D5 RID: 469
	[StructLayout(LayoutKind.Sequential)]
	internal class TransmitFileBuffers
	{
		// Token: 0x040014D4 RID: 5332
		internal IntPtr preBuffer;

		// Token: 0x040014D5 RID: 5333
		internal int preBufferLength;

		// Token: 0x040014D6 RID: 5334
		internal IntPtr postBuffer;

		// Token: 0x040014D7 RID: 5335
		internal int postBufferLength;
	}
}
