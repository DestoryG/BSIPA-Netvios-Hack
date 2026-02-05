using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020003E3 RID: 995
	[Guid("00000103-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[global::__DynamicallyInvokable]
	[ComImport]
	public interface IEnumFORMATETC
	{
		// Token: 0x0600260D RID: 9741
		[global::__DynamicallyInvokable]
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] FORMATETC[] rgelt, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

		// Token: 0x0600260E RID: 9742
		[global::__DynamicallyInvokable]
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600260F RID: 9743
		[global::__DynamicallyInvokable]
		[PreserveSig]
		int Reset();

		// Token: 0x06002610 RID: 9744
		[global::__DynamicallyInvokable]
		void Clone(out IEnumFORMATETC newEnum);
	}
}
