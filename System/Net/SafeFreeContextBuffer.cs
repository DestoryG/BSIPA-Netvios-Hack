using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001EF RID: 495
	[SuppressUnmanagedCodeSecurity]
	internal abstract class SafeFreeContextBuffer : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012F3 RID: 4851 RVA: 0x0006405E File Offset: 0x0006225E
		protected SafeFreeContextBuffer()
			: base(true)
		{
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00064067 File Offset: 0x00062267
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Set(IntPtr value)
		{
			this.handle = value;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00064070 File Offset: 0x00062270
		internal static int EnumeratePackages(SecurDll Dll, out int pkgnum, out SafeFreeContextBuffer pkgArray)
		{
			if (Dll == SecurDll.SECURITY)
			{
				SafeFreeContextBuffer_SECURITY safeFreeContextBuffer_SECURITY = null;
				int num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.EnumerateSecurityPackagesW(out pkgnum, out safeFreeContextBuffer_SECURITY);
				pkgArray = safeFreeContextBuffer_SECURITY;
				if (num != 0 && pkgArray != null)
				{
					pkgArray.SetHandleAsInvalid();
				}
				return num;
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x000640C4 File Offset: 0x000622C4
		internal static SafeFreeContextBuffer CreateEmptyHandle(SecurDll dll)
		{
			if (dll == SecurDll.SECURITY)
			{
				return new SafeFreeContextBuffer_SECURITY();
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "dll");
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x000640F1 File Offset: 0x000622F1
		public unsafe static int QueryContextAttributes(SecurDll dll, SafeDeleteContext phContext, ContextAttribute contextAttribute, byte* buffer, SafeHandle refHandle)
		{
			if (dll == SecurDll.SECURITY)
			{
				return SafeFreeContextBuffer.QueryContextAttributes_SECURITY(phContext, contextAttribute, buffer, refHandle);
			}
			return -1;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00064104 File Offset: 0x00062304
		private unsafe static int QueryContextAttributes_SECURITY(SafeDeleteContext phContext, ContextAttribute contextAttribute, byte* buffer, SafeHandle refHandle)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				phContext.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					phContext.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.QueryContextAttributesW(ref phContext._handle, contextAttribute, (void*)buffer);
					phContext.DangerousRelease();
				}
				if (num == 0 && refHandle != null)
				{
					if (refHandle is SafeFreeContextBuffer)
					{
						((SafeFreeContextBuffer)refHandle).Set(*(IntPtr*)buffer);
					}
					else
					{
						((SafeFreeCertContext)refHandle).Set(*(IntPtr*)buffer);
					}
				}
				if (num != 0 && refHandle != null)
				{
					refHandle.SetHandleAsInvalid();
				}
			}
			return num;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x000641AC File Offset: 0x000623AC
		public static int SetContextAttributes(SecurDll dll, SafeDeleteContext phContext, ContextAttribute contextAttribute, byte[] buffer)
		{
			if (dll == SecurDll.SECURITY)
			{
				return SafeFreeContextBuffer.SetContextAttributes_SECURITY(phContext, contextAttribute, buffer);
			}
			return -1;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x000641BC File Offset: 0x000623BC
		private static int SetContextAttributes_SECURITY(SafeDeleteContext phContext, ContextAttribute contextAttribute, byte[] buffer)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				phContext.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					phContext.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.SetContextAttributesW(ref phContext._handle, contextAttribute, buffer, buffer.Length);
					phContext.DangerousRelease();
				}
			}
			return num;
		}
	}
}
