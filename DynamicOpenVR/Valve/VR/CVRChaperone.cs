using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000017 RID: 23
	public class CVRChaperone
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00002A4A File Offset: 0x00000C4A
		internal CVRChaperone(IntPtr pInterface)
		{
			this.FnTable = (IVRChaperone)Marshal.PtrToStructure(pInterface, typeof(IVRChaperone));
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002A6D File Offset: 0x00000C6D
		public ChaperoneCalibrationState GetCalibrationState()
		{
			return this.FnTable.GetCalibrationState();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002A7F File Offset: 0x00000C7F
		public bool GetPlayAreaSize(ref float pSizeX, ref float pSizeZ)
		{
			pSizeX = 0f;
			pSizeZ = 0f;
			return this.FnTable.GetPlayAreaSize(ref pSizeX, ref pSizeZ);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002AA1 File Offset: 0x00000CA1
		public bool GetPlayAreaRect(ref HmdQuad_t rect)
		{
			return this.FnTable.GetPlayAreaRect(ref rect);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public void ReloadInfo()
		{
			this.FnTable.ReloadInfo();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002AC6 File Offset: 0x00000CC6
		public void SetSceneColor(HmdColor_t color)
		{
			this.FnTable.SetSceneColor(color);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002AD9 File Offset: 0x00000CD9
		public void GetBoundsColor(ref HmdColor_t pOutputColorArray, int nNumOutputColors, float flCollisionBoundsFadeDistance, ref HmdColor_t pOutputCameraColor)
		{
			this.FnTable.GetBoundsColor(ref pOutputColorArray, nNumOutputColors, flCollisionBoundsFadeDistance, ref pOutputCameraColor);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002AF0 File Offset: 0x00000CF0
		public bool AreBoundsVisible()
		{
			return this.FnTable.AreBoundsVisible();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002B02 File Offset: 0x00000D02
		public void ForceBoundsVisible(bool bForce)
		{
			this.FnTable.ForceBoundsVisible(bForce);
		}

		// Token: 0x0400014B RID: 331
		private IVRChaperone FnTable;
	}
}
