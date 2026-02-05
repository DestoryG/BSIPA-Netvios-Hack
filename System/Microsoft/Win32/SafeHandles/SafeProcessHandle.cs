using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000031 RID: 49
	[SuppressUnmanagedCodeSecurity]
	public sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060002AA RID: 682 RVA: 0x000108CB File Offset: 0x0000EACB
		internal SafeProcessHandle()
			: base(true)
		{
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000108D4 File Offset: 0x0000EAD4
		internal SafeProcessHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000108E4 File Offset: 0x0000EAE4
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public SafeProcessHandle(IntPtr existingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x060002AD RID: 685
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeProcessHandle OpenProcess(int access, bool inherit, int processId);

		// Token: 0x060002AE RID: 686 RVA: 0x000108F4 File Offset: 0x0000EAF4
		internal void InitialSetHandle(IntPtr h)
		{
			this.handle = h;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000108FD File Offset: 0x0000EAFD
		protected override bool ReleaseHandle()
		{
			return SafeNativeMethods.CloseHandle(this.handle);
		}

		// Token: 0x04000395 RID: 917
		internal static SafeProcessHandle InvalidHandle = new SafeProcessHandle(IntPtr.Zero);
	}
}
