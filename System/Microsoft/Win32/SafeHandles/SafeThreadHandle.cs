using System;
using System.Security;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000033 RID: 51
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x00010931 File Offset: 0x0000EB31
		internal SafeThreadHandle()
			: base(true)
		{
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0001093A File Offset: 0x0000EB3A
		internal void InitialSetHandle(IntPtr h)
		{
			base.SetHandle(h);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00010943 File Offset: 0x0000EB43
		protected override bool ReleaseHandle()
		{
			return SafeNativeMethods.CloseHandle(this.handle);
		}
	}
}
