using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000023 RID: 35
	public class CVRSpatialAnchors
	{
		// Token: 0x06000153 RID: 339 RVA: 0x000040A6 File Offset: 0x000022A6
		internal CVRSpatialAnchors(IntPtr pInterface)
		{
			this.FnTable = (IVRSpatialAnchors)Marshal.PtrToStructure(pInterface, typeof(IVRSpatialAnchors));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000040C9 File Offset: 0x000022C9
		public EVRSpatialAnchorError CreateSpatialAnchorFromDescriptor(string pchDescriptor, ref uint pHandleOut)
		{
			pHandleOut = 0U;
			return this.FnTable.CreateSpatialAnchorFromDescriptor(pchDescriptor, ref pHandleOut);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000040E0 File Offset: 0x000022E0
		public EVRSpatialAnchorError CreateSpatialAnchorFromPose(uint unDeviceIndex, ETrackingUniverseOrigin eOrigin, ref SpatialAnchorPose_t pPose, ref uint pHandleOut)
		{
			pHandleOut = 0U;
			return this.FnTable.CreateSpatialAnchorFromPose(unDeviceIndex, eOrigin, ref pPose, ref pHandleOut);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000040FB File Offset: 0x000022FB
		public EVRSpatialAnchorError GetSpatialAnchorPose(uint unHandle, ETrackingUniverseOrigin eOrigin, ref SpatialAnchorPose_t pPoseOut)
		{
			return this.FnTable.GetSpatialAnchorPose(unHandle, eOrigin, ref pPoseOut);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00004110 File Offset: 0x00002310
		public EVRSpatialAnchorError GetSpatialAnchorDescriptor(uint unHandle, StringBuilder pchDescriptorOut, ref uint punDescriptorBufferLenInOut)
		{
			punDescriptorBufferLenInOut = 0U;
			return this.FnTable.GetSpatialAnchorDescriptor(unHandle, pchDescriptorOut, ref punDescriptorBufferLenInOut);
		}

		// Token: 0x04000157 RID: 343
		private IVRSpatialAnchors FnTable;
	}
}
