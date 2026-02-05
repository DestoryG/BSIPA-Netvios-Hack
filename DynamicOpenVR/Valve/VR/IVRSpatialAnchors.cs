using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000012 RID: 18
	public struct IVRSpatialAnchors
	{
		// Token: 0x04000143 RID: 323
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSpatialAnchors._CreateSpatialAnchorFromDescriptor CreateSpatialAnchorFromDescriptor;

		// Token: 0x04000144 RID: 324
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSpatialAnchors._CreateSpatialAnchorFromPose CreateSpatialAnchorFromPose;

		// Token: 0x04000145 RID: 325
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSpatialAnchors._GetSpatialAnchorPose GetSpatialAnchorPose;

		// Token: 0x04000146 RID: 326
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSpatialAnchors._GetSpatialAnchorDescriptor GetSpatialAnchorDescriptor;

		// Token: 0x0200022B RID: 555
		// (Invoke) Token: 0x06000760 RID: 1888
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRSpatialAnchorError _CreateSpatialAnchorFromDescriptor(string pchDescriptor, ref uint pHandleOut);

		// Token: 0x0200022C RID: 556
		// (Invoke) Token: 0x06000764 RID: 1892
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRSpatialAnchorError _CreateSpatialAnchorFromPose(uint unDeviceIndex, ETrackingUniverseOrigin eOrigin, ref SpatialAnchorPose_t pPose, ref uint pHandleOut);

		// Token: 0x0200022D RID: 557
		// (Invoke) Token: 0x06000768 RID: 1896
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRSpatialAnchorError _GetSpatialAnchorPose(uint unHandle, ETrackingUniverseOrigin eOrigin, ref SpatialAnchorPose_t pPoseOut);

		// Token: 0x0200022E RID: 558
		// (Invoke) Token: 0x0600076C RID: 1900
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRSpatialAnchorError _GetSpatialAnchorDescriptor(uint unHandle, StringBuilder pchDescriptorOut, ref uint punDescriptorBufferLenInOut);
	}
}
