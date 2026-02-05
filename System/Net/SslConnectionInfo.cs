using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000213 RID: 531
	[StructLayout(LayoutKind.Sequential)]
	internal class SslConnectionInfo
	{
		// Token: 0x060013BD RID: 5053 RVA: 0x00068510 File Offset: 0x00066710
		internal unsafe SslConnectionInfo(byte[] nativeBuffer)
		{
			fixed (byte[] array = nativeBuffer)
			{
				void* ptr;
				if (nativeBuffer == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = (void*)(&array[0]);
				}
				IntPtr intPtr = new IntPtr(ptr);
				this.Protocol = Marshal.ReadInt32(intPtr);
				this.DataCipherAlg = Marshal.ReadInt32(intPtr, 4);
				this.DataKeySize = Marshal.ReadInt32(intPtr, 8);
				this.DataHashAlg = Marshal.ReadInt32(intPtr, 12);
				this.DataHashKeySize = Marshal.ReadInt32(intPtr, 16);
				this.KeyExchangeAlg = Marshal.ReadInt32(intPtr, 20);
				this.KeyExchKeySize = Marshal.ReadInt32(intPtr, 24);
			}
		}

		// Token: 0x040015A7 RID: 5543
		public readonly int Protocol;

		// Token: 0x040015A8 RID: 5544
		public readonly int DataCipherAlg;

		// Token: 0x040015A9 RID: 5545
		public readonly int DataKeySize;

		// Token: 0x040015AA RID: 5546
		public readonly int DataHashAlg;

		// Token: 0x040015AB RID: 5547
		public readonly int DataHashKeySize;

		// Token: 0x040015AC RID: 5548
		public readonly int KeyExchangeAlg;

		// Token: 0x040015AD RID: 5549
		public readonly int KeyExchKeySize;
	}
}
