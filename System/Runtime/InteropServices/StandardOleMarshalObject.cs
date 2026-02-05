using System;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Runtime.InteropServices
{
	// Token: 0x020003DC RID: 988
	[ComVisible(true)]
	public class StandardOleMarshalObject : MarshalByRefObject, Microsoft.Win32.UnsafeNativeMethods.IMarshal
	{
		// Token: 0x060025F6 RID: 9718 RVA: 0x000B03DF File Offset: 0x000AE5DF
		protected StandardOleMarshalObject()
		{
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000B03E8 File Offset: 0x000AE5E8
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private IntPtr GetStdMarshaler(ref Guid riid, int dwDestContext, int mshlflags)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr iunknownForObject = Marshal.GetIUnknownForObject(this);
			if (iunknownForObject != IntPtr.Zero)
			{
				try
				{
					if (Microsoft.Win32.UnsafeNativeMethods.CoGetStandardMarshal(ref riid, iunknownForObject, dwDestContext, IntPtr.Zero, mshlflags, out zero) == 0)
					{
						return zero;
					}
				}
				finally
				{
					Marshal.Release(iunknownForObject);
				}
			}
			throw new InvalidOperationException(SR.GetString("StandardOleMarshalObjectGetMarshalerFailed", new object[] { riid.ToString() }));
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000B0468 File Offset: 0x000AE668
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int Microsoft.Win32.UnsafeNativeMethods.IMarshal.GetUnmarshalClass(ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags, out Guid pCid)
		{
			pCid = StandardOleMarshalObject.CLSID_StdMarshal;
			return 0;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000B0478 File Offset: 0x000AE678
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		unsafe int Microsoft.Win32.UnsafeNativeMethods.IMarshal.GetMarshalSizeMax(ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags, out int pSize)
		{
			IntPtr stdMarshaler = this.GetStdMarshaler(ref riid, dwDestContext, mshlflags);
			int num;
			try
			{
				IntPtr intPtr = *(IntPtr*)stdMarshaler.ToPointer();
				IntPtr intPtr2 = *(IntPtr*)((byte*)intPtr.ToPointer() + (IntPtr)4 * (IntPtr)sizeof(IntPtr));
				StandardOleMarshalObject.GetMarshalSizeMax_Delegate getMarshalSizeMax_Delegate = (StandardOleMarshalObject.GetMarshalSizeMax_Delegate)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(StandardOleMarshalObject.GetMarshalSizeMax_Delegate));
				num = getMarshalSizeMax_Delegate(stdMarshaler, ref riid, pv, dwDestContext, pvDestContext, mshlflags, out pSize);
			}
			finally
			{
				Marshal.Release(stdMarshaler);
			}
			return num;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000B04F0 File Offset: 0x000AE6F0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		unsafe int Microsoft.Win32.UnsafeNativeMethods.IMarshal.MarshalInterface(IntPtr pStm, ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags)
		{
			IntPtr stdMarshaler = this.GetStdMarshaler(ref riid, dwDestContext, mshlflags);
			int num;
			try
			{
				IntPtr intPtr = *(IntPtr*)stdMarshaler.ToPointer();
				IntPtr intPtr2 = *(IntPtr*)((byte*)intPtr.ToPointer() + (IntPtr)5 * (IntPtr)sizeof(IntPtr));
				StandardOleMarshalObject.MarshalInterface_Delegate marshalInterface_Delegate = (StandardOleMarshalObject.MarshalInterface_Delegate)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(StandardOleMarshalObject.MarshalInterface_Delegate));
				num = marshalInterface_Delegate(stdMarshaler, pStm, ref riid, pv, dwDestContext, pvDestContext, mshlflags);
			}
			finally
			{
				Marshal.Release(stdMarshaler);
			}
			return num;
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000B056C File Offset: 0x000AE76C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int Microsoft.Win32.UnsafeNativeMethods.IMarshal.UnmarshalInterface(IntPtr pStm, ref Guid riid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			return -2147467263;
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000B057A File Offset: 0x000AE77A
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int Microsoft.Win32.UnsafeNativeMethods.IMarshal.ReleaseMarshalData(IntPtr pStm)
		{
			return -2147467263;
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000B0581 File Offset: 0x000AE781
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int Microsoft.Win32.UnsafeNativeMethods.IMarshal.DisconnectObject(int dwReserved)
		{
			return -2147467263;
		}

		// Token: 0x04002079 RID: 8313
		private static readonly Guid CLSID_StdMarshal = new Guid("00000017-0000-0000-c000-000000000046");

		// Token: 0x0200080F RID: 2063
		// (Invoke) Token: 0x060044E5 RID: 17637
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int GetMarshalSizeMax_Delegate(IntPtr _this, ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags, out int pSize);

		// Token: 0x02000810 RID: 2064
		// (Invoke) Token: 0x060044E9 RID: 17641
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int MarshalInterface_Delegate(IntPtr _this, IntPtr pStm, ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags);
	}
}
