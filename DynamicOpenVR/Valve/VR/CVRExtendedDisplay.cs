using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000014 RID: 20
	public class CVRExtendedDisplay
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000025CD File Offset: 0x000007CD
		internal CVRExtendedDisplay(IntPtr pInterface)
		{
			this.FnTable = (IVRExtendedDisplay)Marshal.PtrToStructure(pInterface, typeof(IVRExtendedDisplay));
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000025F0 File Offset: 0x000007F0
		public void GetWindowBounds(ref int pnX, ref int pnY, ref uint pnWidth, ref uint pnHeight)
		{
			pnX = 0;
			pnY = 0;
			pnWidth = 0U;
			pnHeight = 0U;
			this.FnTable.GetWindowBounds(ref pnX, ref pnY, ref pnWidth, ref pnHeight);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002614 File Offset: 0x00000814
		public void GetEyeOutputViewport(EVREye eEye, ref uint pnX, ref uint pnY, ref uint pnWidth, ref uint pnHeight)
		{
			pnX = 0U;
			pnY = 0U;
			pnWidth = 0U;
			pnHeight = 0U;
			this.FnTable.GetEyeOutputViewport(eEye, ref pnX, ref pnY, ref pnWidth, ref pnHeight);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000263B File Offset: 0x0000083B
		public void GetDXGIOutputInfo(ref int pnAdapterIndex, ref int pnAdapterOutputIndex)
		{
			pnAdapterIndex = 0;
			pnAdapterOutputIndex = 0;
			this.FnTable.GetDXGIOutputInfo(ref pnAdapterIndex, ref pnAdapterOutputIndex);
		}

		// Token: 0x04000148 RID: 328
		private IVRExtendedDisplay FnTable;
	}
}
