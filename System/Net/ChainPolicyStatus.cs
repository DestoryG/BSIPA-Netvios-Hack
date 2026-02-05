using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000128 RID: 296
	internal struct ChainPolicyStatus
	{
		// Token: 0x04000FFF RID: 4095
		public uint cbSize;

		// Token: 0x04001000 RID: 4096
		public uint dwError;

		// Token: 0x04001001 RID: 4097
		public uint lChainIndex;

		// Token: 0x04001002 RID: 4098
		public uint lElementIndex;

		// Token: 0x04001003 RID: 4099
		public unsafe void* pvExtraPolicyStatus;

		// Token: 0x04001004 RID: 4100
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainPolicyStatus));
	}
}
