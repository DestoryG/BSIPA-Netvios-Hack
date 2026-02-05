using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000018 RID: 24
	public class CVRChaperoneSetup
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00002B15 File Offset: 0x00000D15
		internal CVRChaperoneSetup(IntPtr pInterface)
		{
			this.FnTable = (IVRChaperoneSetup)Marshal.PtrToStructure(pInterface, typeof(IVRChaperoneSetup));
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002B38 File Offset: 0x00000D38
		public bool CommitWorkingCopy(EChaperoneConfigFile configFile)
		{
			return this.FnTable.CommitWorkingCopy(configFile);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002B4B File Offset: 0x00000D4B
		public void RevertWorkingCopy()
		{
			this.FnTable.RevertWorkingCopy();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002B5D File Offset: 0x00000D5D
		public bool GetWorkingPlayAreaSize(ref float pSizeX, ref float pSizeZ)
		{
			pSizeX = 0f;
			pSizeZ = 0f;
			return this.FnTable.GetWorkingPlayAreaSize(ref pSizeX, ref pSizeZ);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002B7F File Offset: 0x00000D7F
		public bool GetWorkingPlayAreaRect(ref HmdQuad_t rect)
		{
			return this.FnTable.GetWorkingPlayAreaRect(ref rect);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002B94 File Offset: 0x00000D94
		public bool GetWorkingCollisionBoundsInfo(out HmdQuad_t[] pQuadsBuffer)
		{
			uint num = 0U;
			this.FnTable.GetWorkingCollisionBoundsInfo(null, ref num);
			pQuadsBuffer = new HmdQuad_t[num];
			return this.FnTable.GetWorkingCollisionBoundsInfo(pQuadsBuffer, ref num);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public bool GetLiveCollisionBoundsInfo(out HmdQuad_t[] pQuadsBuffer)
		{
			uint num = 0U;
			this.FnTable.GetLiveCollisionBoundsInfo(null, ref num);
			pQuadsBuffer = new HmdQuad_t[num];
			return this.FnTable.GetLiveCollisionBoundsInfo(pQuadsBuffer, ref num);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002C13 File Offset: 0x00000E13
		public bool GetWorkingSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatSeatedZeroPoseToRawTrackingPose)
		{
			return this.FnTable.GetWorkingSeatedZeroPoseToRawTrackingPose(ref pmatSeatedZeroPoseToRawTrackingPose);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002C26 File Offset: 0x00000E26
		public bool GetWorkingStandingZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatStandingZeroPoseToRawTrackingPose)
		{
			return this.FnTable.GetWorkingStandingZeroPoseToRawTrackingPose(ref pmatStandingZeroPoseToRawTrackingPose);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002C39 File Offset: 0x00000E39
		public void SetWorkingPlayAreaSize(float sizeX, float sizeZ)
		{
			this.FnTable.SetWorkingPlayAreaSize(sizeX, sizeZ);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002C4D File Offset: 0x00000E4D
		public void SetWorkingCollisionBoundsInfo(HmdQuad_t[] pQuadsBuffer)
		{
			this.FnTable.SetWorkingCollisionBoundsInfo(pQuadsBuffer, (uint)pQuadsBuffer.Length);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002C63 File Offset: 0x00000E63
		public void SetWorkingPerimeter(HmdVector2_t[] pPointBuffer)
		{
			this.FnTable.SetWorkingPerimeter(pPointBuffer, (uint)pPointBuffer.Length);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002C79 File Offset: 0x00000E79
		public void SetWorkingSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pMatSeatedZeroPoseToRawTrackingPose)
		{
			this.FnTable.SetWorkingSeatedZeroPoseToRawTrackingPose(ref pMatSeatedZeroPoseToRawTrackingPose);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002C8C File Offset: 0x00000E8C
		public void SetWorkingStandingZeroPoseToRawTrackingPose(ref HmdMatrix34_t pMatStandingZeroPoseToRawTrackingPose)
		{
			this.FnTable.SetWorkingStandingZeroPoseToRawTrackingPose(ref pMatStandingZeroPoseToRawTrackingPose);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002C9F File Offset: 0x00000E9F
		public void ReloadFromDisk(EChaperoneConfigFile configFile)
		{
			this.FnTable.ReloadFromDisk(configFile);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002CB2 File Offset: 0x00000EB2
		public bool GetLiveSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatSeatedZeroPoseToRawTrackingPose)
		{
			return this.FnTable.GetLiveSeatedZeroPoseToRawTrackingPose(ref pmatSeatedZeroPoseToRawTrackingPose);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002CC5 File Offset: 0x00000EC5
		public bool ExportLiveToBuffer(StringBuilder pBuffer, ref uint pnBufferLength)
		{
			pnBufferLength = 0U;
			return this.FnTable.ExportLiveToBuffer(pBuffer, ref pnBufferLength);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002CDC File Offset: 0x00000EDC
		public bool ImportFromBufferToWorking(string pBuffer, uint nImportFlags)
		{
			return this.FnTable.ImportFromBufferToWorking(pBuffer, nImportFlags);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002CF0 File Offset: 0x00000EF0
		public void ShowWorkingSetPreview()
		{
			this.FnTable.ShowWorkingSetPreview();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002D02 File Offset: 0x00000F02
		public void HideWorkingSetPreview()
		{
			this.FnTable.HideWorkingSetPreview();
		}

		// Token: 0x0400014C RID: 332
		private IVRChaperoneSetup FnTable;
	}
}
