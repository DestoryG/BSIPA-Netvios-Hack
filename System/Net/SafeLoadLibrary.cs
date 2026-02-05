using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001F4 RID: 500
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeLoadLibrary : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600130C RID: 4876 RVA: 0x000643C0 File Offset: 0x000625C0
		static SafeLoadLibrary()
		{
			try
			{
				IntPtr moduleHandleW = UnsafeNclNativeMethods.SafeNetHandles.GetModuleHandleW("kernel32.dll");
				if (moduleHandleW != IntPtr.Zero && UnsafeNclNativeMethods.GetProcAddress(moduleHandleW, "AddDllDirectory") != IntPtr.Zero)
				{
					SafeLoadLibrary._flags = 2048U;
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0006442C File Offset: 0x0006262C
		private SafeLoadLibrary()
			: base(true)
		{
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00064435 File Offset: 0x00062635
		private SafeLoadLibrary(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00064440 File Offset: 0x00062640
		public static SafeLoadLibrary LoadLibraryEx(string library)
		{
			SafeLoadLibrary safeLoadLibrary = UnsafeNclNativeMethods.SafeNetHandles.LoadLibraryExW(library, null, SafeLoadLibrary._flags);
			if (safeLoadLibrary.IsInvalid)
			{
				safeLoadLibrary.SetHandleAsInvalid();
			}
			return safeLoadLibrary;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0006446C File Offset: 0x0006266C
		public bool HasFunction(string functionName)
		{
			IntPtr procAddress = UnsafeNclNativeMethods.GetProcAddress(this, functionName);
			return procAddress != IntPtr.Zero;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0006448C File Offset: 0x0006268C
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.FreeLibrary(this.handle);
		}

		// Token: 0x04001538 RID: 5432
		private const string KERNEL32 = "kernel32.dll";

		// Token: 0x04001539 RID: 5433
		private const string AddDllDirectory = "AddDllDirectory";

		// Token: 0x0400153A RID: 5434
		private const uint LOAD_LIBRARY_SEARCH_SYSTEM32 = 2048U;

		// Token: 0x0400153B RID: 5435
		public static readonly SafeLoadLibrary Zero = new SafeLoadLibrary(false);

		// Token: 0x0400153C RID: 5436
		private static uint _flags = 0U;
	}
}
