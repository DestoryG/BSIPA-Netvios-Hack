using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000011 RID: 17
	public struct IVRIOBuffer
	{
		// Token: 0x0400013D RID: 317
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRIOBuffer._Open Open;

		// Token: 0x0400013E RID: 318
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRIOBuffer._Close Close;

		// Token: 0x0400013F RID: 319
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRIOBuffer._Read Read;

		// Token: 0x04000140 RID: 320
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRIOBuffer._Write Write;

		// Token: 0x04000141 RID: 321
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRIOBuffer._PropertyContainer PropertyContainer;

		// Token: 0x04000142 RID: 322
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRIOBuffer._HasReaders HasReaders;

		// Token: 0x02000225 RID: 549
		// (Invoke) Token: 0x06000748 RID: 1864
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EIOBufferError _Open(string pchPath, EIOBufferMode mode, uint unElementSize, uint unElements, ref ulong pulBuffer);

		// Token: 0x02000226 RID: 550
		// (Invoke) Token: 0x0600074C RID: 1868
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EIOBufferError _Close(ulong ulBuffer);

		// Token: 0x02000227 RID: 551
		// (Invoke) Token: 0x06000750 RID: 1872
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EIOBufferError _Read(ulong ulBuffer, IntPtr pDst, uint unBytes, ref uint punRead);

		// Token: 0x02000228 RID: 552
		// (Invoke) Token: 0x06000754 RID: 1876
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EIOBufferError _Write(ulong ulBuffer, IntPtr pSrc, uint unBytes);

		// Token: 0x02000229 RID: 553
		// (Invoke) Token: 0x06000758 RID: 1880
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _PropertyContainer(ulong ulBuffer);

		// Token: 0x0200022A RID: 554
		// (Invoke) Token: 0x0600075C RID: 1884
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _HasReaders(ulong ulBuffer);
	}
}
