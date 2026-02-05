using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200000C RID: 12
	public struct IVRSettings
	{
		// Token: 0x0400010C RID: 268
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetSettingsErrorNameFromEnum GetSettingsErrorNameFromEnum;

		// Token: 0x0400010D RID: 269
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._Sync Sync;

		// Token: 0x0400010E RID: 270
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._SetBool SetBool;

		// Token: 0x0400010F RID: 271
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._SetInt32 SetInt32;

		// Token: 0x04000110 RID: 272
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._SetFloat SetFloat;

		// Token: 0x04000111 RID: 273
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._SetString SetString;

		// Token: 0x04000112 RID: 274
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetBool GetBool;

		// Token: 0x04000113 RID: 275
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetInt32 GetInt32;

		// Token: 0x04000114 RID: 276
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetFloat GetFloat;

		// Token: 0x04000115 RID: 277
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetString GetString;

		// Token: 0x04000116 RID: 278
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._RemoveSection RemoveSection;

		// Token: 0x04000117 RID: 279
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._RemoveKeyInSection RemoveKeyInSection;

		// Token: 0x020001F4 RID: 500
		// (Invoke) Token: 0x06000684 RID: 1668
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetSettingsErrorNameFromEnum(EVRSettingsError eError);

		// Token: 0x020001F5 RID: 501
		// (Invoke) Token: 0x06000688 RID: 1672
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _Sync(bool bForce, ref EVRSettingsError peError);

		// Token: 0x020001F6 RID: 502
		// (Invoke) Token: 0x0600068C RID: 1676
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetBool(string pchSection, string pchSettingsKey, bool bValue, ref EVRSettingsError peError);

		// Token: 0x020001F7 RID: 503
		// (Invoke) Token: 0x06000690 RID: 1680
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetInt32(string pchSection, string pchSettingsKey, int nValue, ref EVRSettingsError peError);

		// Token: 0x020001F8 RID: 504
		// (Invoke) Token: 0x06000694 RID: 1684
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetFloat(string pchSection, string pchSettingsKey, float flValue, ref EVRSettingsError peError);

		// Token: 0x020001F9 RID: 505
		// (Invoke) Token: 0x06000698 RID: 1688
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetString(string pchSection, string pchSettingsKey, string pchValue, ref EVRSettingsError peError);

		// Token: 0x020001FA RID: 506
		// (Invoke) Token: 0x0600069C RID: 1692
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetBool(string pchSection, string pchSettingsKey, ref EVRSettingsError peError);

		// Token: 0x020001FB RID: 507
		// (Invoke) Token: 0x060006A0 RID: 1696
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate int _GetInt32(string pchSection, string pchSettingsKey, ref EVRSettingsError peError);

		// Token: 0x020001FC RID: 508
		// (Invoke) Token: 0x060006A4 RID: 1700
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate float _GetFloat(string pchSection, string pchSettingsKey, ref EVRSettingsError peError);

		// Token: 0x020001FD RID: 509
		// (Invoke) Token: 0x060006A8 RID: 1704
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetString(string pchSection, string pchSettingsKey, StringBuilder pchValue, uint unValueLen, ref EVRSettingsError peError);

		// Token: 0x020001FE RID: 510
		// (Invoke) Token: 0x060006AC RID: 1708
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _RemoveSection(string pchSection, ref EVRSettingsError peError);

		// Token: 0x020001FF RID: 511
		// (Invoke) Token: 0x060006B0 RID: 1712
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _RemoveKeyInSection(string pchSection, string pchSettingsKey, ref EVRSettingsError peError);
	}
}
