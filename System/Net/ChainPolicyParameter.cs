using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000126 RID: 294
	internal struct ChainPolicyParameter
	{
		// Token: 0x04000FF6 RID: 4086
		public uint cbSize;

		// Token: 0x04000FF7 RID: 4087
		public uint dwFlags;

		// Token: 0x04000FF8 RID: 4088
		public unsafe SSL_EXTRA_CERT_CHAIN_POLICY_PARA* pvExtraPolicyPara;

		// Token: 0x04000FF9 RID: 4089
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainPolicyParameter));
	}
}
