using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200000E RID: 14
	public struct IVRResources
	{
		// Token: 0x0400011F RID: 287
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRResources._LoadSharedResource LoadSharedResource;

		// Token: 0x04000120 RID: 288
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRResources._GetResourceFullPath GetResourceFullPath;

		// Token: 0x02000207 RID: 519
		// (Invoke) Token: 0x060006D0 RID: 1744
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _LoadSharedResource(string pchResourceName, string pchBuffer, uint unBufferLen);

		// Token: 0x02000208 RID: 520
		// (Invoke) Token: 0x060006D4 RID: 1748
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetResourceFullPath(string pchResourceName, string pchResourceTypeDirectory, StringBuilder pchPathBuffer, uint unBufferLen);
	}
}
