using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020003E4 RID: 996
	[Guid("00000103-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumSTATDATA
	{
		// Token: 0x06002611 RID: 9745
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] STATDATA[] rgelt, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] [Out] int[] pceltFetched);

		// Token: 0x06002612 RID: 9746
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06002613 RID: 9747
		[PreserveSig]
		int Reset();

		// Token: 0x06002614 RID: 9748
		void Clone(out IEnumSTATDATA newEnum);
	}
}
