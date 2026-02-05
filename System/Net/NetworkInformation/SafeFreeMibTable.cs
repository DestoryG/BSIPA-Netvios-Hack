using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F0 RID: 752
	[SuppressUnmanagedCodeSecurity]
	internal class SafeFreeMibTable : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001A61 RID: 6753 RVA: 0x0007FE20 File Offset: 0x0007E020
		public SafeFreeMibTable()
			: base(true)
		{
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0007FE29 File Offset: 0x0007E029
		protected override bool ReleaseHandle()
		{
			UnsafeNetInfoNativeMethods.FreeMibTable(this.handle);
			this.handle = IntPtr.Zero;
			return true;
		}
	}
}
