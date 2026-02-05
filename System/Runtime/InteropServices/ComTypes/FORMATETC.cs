using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020003E0 RID: 992
	[global::__DynamicallyInvokable]
	public struct FORMATETC
	{
		// Token: 0x0400208A RID: 8330
		[global::__DynamicallyInvokable]
		[MarshalAs(UnmanagedType.U2)]
		public short cfFormat;

		// Token: 0x0400208B RID: 8331
		public IntPtr ptd;

		// Token: 0x0400208C RID: 8332
		[global::__DynamicallyInvokable]
		[MarshalAs(UnmanagedType.U4)]
		public DVASPECT dwAspect;

		// Token: 0x0400208D RID: 8333
		[global::__DynamicallyInvokable]
		public int lindex;

		// Token: 0x0400208E RID: 8334
		[global::__DynamicallyInvokable]
		[MarshalAs(UnmanagedType.U4)]
		public TYMED tymed;
	}
}
