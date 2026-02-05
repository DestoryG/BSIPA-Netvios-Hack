using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200067B RID: 1659
	internal static class FileIntegrity
	{
		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06003D28 RID: 15656 RVA: 0x000FBAB1 File Offset: 0x000F9CB1
		public static bool IsEnabled
		{
			get
			{
				return FileIntegrity.s_lazyIsEnabled.Value;
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x000FBAC0 File Offset: 0x000F9CC0
		public static void MarkAsTrusted(SafeFileHandle safeFileHandle)
		{
			int num = Microsoft.Win32.UnsafeNativeMethods.WldpSetDynamicCodeTrust(safeFileHandle);
			Marshal.ThrowExceptionForHR(num, new IntPtr(-1));
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x000FBAE0 File Offset: 0x000F9CE0
		public static bool IsTrusted(SafeFileHandle safeFileHandle)
		{
			int num = Microsoft.Win32.UnsafeNativeMethods.WldpQueryDynamicCodeTrust(safeFileHandle, IntPtr.Zero, 0U);
			if (num == -805305819)
			{
				return false;
			}
			Marshal.ThrowExceptionForHR(num, new IntPtr(-1));
			return true;
		}

		// Token: 0x04002C90 RID: 11408
		private static readonly Lazy<bool> s_lazyIsEnabled = new Lazy<bool>(delegate
		{
			Version version = Environment.OSVersion.Version;
			if (version.Major < 6 || (version.Major == 6 && version.Minor < 2))
			{
				return false;
			}
			bool flag;
			using (Microsoft.Win32.SafeHandles.SafeLibraryHandle safeLibraryHandle = Microsoft.Win32.SafeHandles.SafeLibraryHandle.LoadLibraryEx("wldp.dll", IntPtr.Zero, 2048))
			{
				if (safeLibraryHandle.IsInvalid)
				{
					flag = false;
				}
				else
				{
					IntPtr moduleHandle = Microsoft.Win32.UnsafeNativeMethods.GetModuleHandle("wldp.dll");
					if (!(moduleHandle != IntPtr.Zero) || !(IntPtr.Zero != Microsoft.Win32.UnsafeNativeMethods.GetProcAddress(moduleHandle, "WldpIsDynamicCodePolicyEnabled")) || !(IntPtr.Zero != Microsoft.Win32.UnsafeNativeMethods.GetProcAddress(moduleHandle, "WldpSetDynamicCodeTrust")) || !(IntPtr.Zero != Microsoft.Win32.UnsafeNativeMethods.GetProcAddress(moduleHandle, "WldpQueryDynamicCodeTrust")))
					{
						flag = false;
					}
					else
					{
						int num = 0;
						int num2 = Microsoft.Win32.UnsafeNativeMethods.WldpIsDynamicCodePolicyEnabled(out num);
						Marshal.ThrowExceptionForHR(num2, new IntPtr(-1));
						flag = num != 0;
					}
				}
			}
			return flag;
		});
	}
}
