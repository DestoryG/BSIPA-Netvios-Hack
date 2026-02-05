using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020003E6 RID: 998
	[global::__DynamicallyInvokable]
	public struct STGMEDIUM
	{
		// Token: 0x04002093 RID: 8339
		[global::__DynamicallyInvokable]
		public TYMED tymed;

		// Token: 0x04002094 RID: 8340
		public IntPtr unionmember;

		// Token: 0x04002095 RID: 8341
		[global::__DynamicallyInvokable]
		[MarshalAs(UnmanagedType.IUnknown)]
		public object pUnkForRelease;
	}
}
