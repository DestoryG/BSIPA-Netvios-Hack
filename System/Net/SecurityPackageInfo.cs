using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000216 RID: 534
	internal struct SecurityPackageInfo
	{
		// Token: 0x040015B7 RID: 5559
		internal int Capabilities;

		// Token: 0x040015B8 RID: 5560
		internal short Version;

		// Token: 0x040015B9 RID: 5561
		internal short RPCID;

		// Token: 0x040015BA RID: 5562
		internal int MaxToken;

		// Token: 0x040015BB RID: 5563
		internal IntPtr Name;

		// Token: 0x040015BC RID: 5564
		internal IntPtr Comment;

		// Token: 0x040015BD RID: 5565
		internal static readonly int Size = Marshal.SizeOf(typeof(SecurityPackageInfo));

		// Token: 0x040015BE RID: 5566
		internal static readonly int NameOffest = (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Name");
	}
}
