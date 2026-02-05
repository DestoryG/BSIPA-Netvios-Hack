using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000214 RID: 532
	internal struct NegotiationInfo
	{
		// Token: 0x040015AE RID: 5550
		internal IntPtr PackageInfo;

		// Token: 0x040015AF RID: 5551
		internal uint NegotiationState;

		// Token: 0x040015B0 RID: 5552
		internal static readonly int Size = Marshal.SizeOf(typeof(NegotiationInfo));

		// Token: 0x040015B1 RID: 5553
		internal static readonly int NegotiationStateOffest = (int)Marshal.OffsetOf(typeof(NegotiationInfo), "NegotiationState");
	}
}
