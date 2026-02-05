using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020004D8 RID: 1240
	[Guid("73386977-D6FD-11D2-BED5-00C04F79E3AE")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface ICollectData
	{
		// Token: 0x06002ED8 RID: 11992
		[return: MarshalAs(UnmanagedType.I4)]
		void CollectData([MarshalAs(UnmanagedType.I4)] [In] int id, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr valueName, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr data, [MarshalAs(UnmanagedType.I4)] [In] int totalBytes, [MarshalAs(UnmanagedType.SysInt)] out IntPtr res);

		// Token: 0x06002ED9 RID: 11993
		void CloseData();
	}
}
