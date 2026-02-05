using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000148 RID: 328
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct hostent
	{
		// Token: 0x040010E4 RID: 4324
		public IntPtr h_name;

		// Token: 0x040010E5 RID: 4325
		public IntPtr h_aliases;

		// Token: 0x040010E6 RID: 4326
		public short h_addrtype;

		// Token: 0x040010E7 RID: 4327
		public short h_length;

		// Token: 0x040010E8 RID: 4328
		public IntPtr h_addr_list;
	}
}
