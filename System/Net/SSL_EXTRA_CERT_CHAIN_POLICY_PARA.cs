using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000127 RID: 295
	internal struct SSL_EXTRA_CERT_CHAIN_POLICY_PARA
	{
		// Token: 0x06000B36 RID: 2870 RVA: 0x0003DA2E File Offset: 0x0003BC2E
		internal SSL_EXTRA_CERT_CHAIN_POLICY_PARA(bool amIServer)
		{
			this.u.cbStruct = SSL_EXTRA_CERT_CHAIN_POLICY_PARA.StructSize;
			this.u.cbSize = SSL_EXTRA_CERT_CHAIN_POLICY_PARA.StructSize;
			this.dwAuthType = (amIServer ? 1 : 2);
			this.fdwChecks = 0U;
			this.pwszServerName = null;
		}

		// Token: 0x04000FFA RID: 4090
		internal SSL_EXTRA_CERT_CHAIN_POLICY_PARA.U u;

		// Token: 0x04000FFB RID: 4091
		internal int dwAuthType;

		// Token: 0x04000FFC RID: 4092
		internal uint fdwChecks;

		// Token: 0x04000FFD RID: 4093
		internal unsafe char* pwszServerName;

		// Token: 0x04000FFE RID: 4094
		private static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(SSL_EXTRA_CERT_CHAIN_POLICY_PARA));

		// Token: 0x0200070A RID: 1802
		[StructLayout(LayoutKind.Explicit)]
		internal struct U
		{
			// Token: 0x040030EE RID: 12526
			[FieldOffset(0)]
			internal uint cbStruct;

			// Token: 0x040030EF RID: 12527
			[FieldOffset(0)]
			internal uint cbSize;
		}
	}
}
