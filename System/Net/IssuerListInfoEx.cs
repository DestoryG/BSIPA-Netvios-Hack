using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200012D RID: 301
	internal struct IssuerListInfoEx
	{
		// Token: 0x06000B3A RID: 2874 RVA: 0x0003DAB0 File Offset: 0x0003BCB0
		public unsafe IssuerListInfoEx(SafeHandle handle, byte[] nativeBuffer)
		{
			this.aIssuers = handle;
			fixed (byte[] array = nativeBuffer)
			{
				byte* ptr;
				if (nativeBuffer == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				this.cIssuers = *(uint*)(ptr + IntPtr.Size);
			}
		}

		// Token: 0x04001012 RID: 4114
		public SafeHandle aIssuers;

		// Token: 0x04001013 RID: 4115
		public uint cIssuers;
	}
}
