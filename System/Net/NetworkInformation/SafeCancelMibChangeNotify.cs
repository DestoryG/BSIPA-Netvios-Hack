using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002EF RID: 751
	[SuppressUnmanagedCodeSecurity]
	internal class SafeCancelMibChangeNotify : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001A5F RID: 6751 RVA: 0x0007FDEC File Offset: 0x0007DFEC
		public SafeCancelMibChangeNotify()
			: base(true)
		{
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0007FDF8 File Offset: 0x0007DFF8
		protected override bool ReleaseHandle()
		{
			uint num = UnsafeNetInfoNativeMethods.CancelMibChangeNotify2(this.handle);
			this.handle = IntPtr.Zero;
			return num == 0U;
		}
	}
}
