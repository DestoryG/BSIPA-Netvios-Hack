using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001FA RID: 506
	internal abstract class SafeFreeCredentials : SafeHandle
	{
		// Token: 0x06001325 RID: 4901 RVA: 0x00064807 File Offset: 0x00062A07
		protected SafeFreeCredentials()
			: base(IntPtr.Zero, true)
		{
			this._handle = default(SSPIHandle);
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x00064821 File Offset: 0x00062A21
		public override bool IsInvalid
		{
			get
			{
				return base.IsClosed || this._handle.IsZero;
			}
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00064838 File Offset: 0x00062A38
		public static int AcquireCredentialsHandle(SecurDll dll, string package, CredentialUse intent, ref AuthIdentity authdata, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			if (dll == SecurDll.SECURITY)
			{
				outCredential = new SafeFreeCredential_SECURITY();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_0052;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
				}
				goto IL_002F;
				IL_0052:
				if (num != 0)
				{
					outCredential.SetHandleAsInvalid();
				}
				return num;
			}
			IL_002F:
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x000648B4 File Offset: 0x00062AB4
		public static int AcquireDefaultCredential(SecurDll dll, string package, CredentialUse intent, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			if (dll == SecurDll.SECURITY)
			{
				outCredential = new SafeFreeCredential_SECURITY();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_0054;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, IntPtr.Zero, null, null, ref outCredential._handle, out num2);
				}
				goto IL_0031;
				IL_0054:
				if (num != 0)
				{
					outCredential.SetHandleAsInvalid();
				}
				return num;
			}
			IL_0031:
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00064930 File Offset: 0x00062B30
		public static int AcquireCredentialsHandle(string package, CredentialUse intent, ref SafeSspiAuthDataHandle authdata, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			outCredential = new SafeFreeCredential_SECURITY();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				long num2;
				num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, authdata, null, null, ref outCredential._handle, out num2);
			}
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00064984 File Offset: 0x00062B84
		public unsafe static int AcquireCredentialsHandle(SecurDll dll, string package, CredentialUse intent, ref SecureCredential authdata, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			IntPtr certContextArray = authdata.certContextArray;
			try
			{
				IntPtr intPtr = new IntPtr((void*)(&certContextArray));
				if (certContextArray != IntPtr.Zero)
				{
					authdata.certContextArray = intPtr;
				}
				if (dll == SecurDll.SECURITY)
				{
					outCredential = new SafeFreeCredential_SECURITY();
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						goto IL_007F;
					}
					finally
					{
						long num2;
						num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
					}
				}
				throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
			}
			finally
			{
				authdata.certContextArray = certContextArray;
			}
			IL_007F:
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00064A38 File Offset: 0x00062C38
		public static int AcquireCredentialsHandle(SecurDll dll, string package, CredentialUse intent, ref SecureCredential2 authdata, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			if (dll == SecurDll.SECURITY)
			{
				outCredential = new SafeFreeCredential_SECURITY();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_0052;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
				}
				goto IL_002F;
				IL_0052:
				if (num != 0)
				{
					outCredential.SetHandleAsInvalid();
				}
				return num;
			}
			IL_002F:
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
		}

		// Token: 0x04001548 RID: 5448
		internal SSPIHandle _handle;
	}
}
