using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200001E RID: 30
	public class CVRScreenshots
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00003C22 File Offset: 0x00001E22
		internal CVRScreenshots(IntPtr pInterface)
		{
			this.FnTable = (IVRScreenshots)Marshal.PtrToStructure(pInterface, typeof(IVRScreenshots));
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00003C45 File Offset: 0x00001E45
		public EVRScreenshotError RequestScreenshot(ref uint pOutScreenshotHandle, EVRScreenshotType type, string pchPreviewFilename, string pchVRFilename)
		{
			pOutScreenshotHandle = 0U;
			return this.FnTable.RequestScreenshot(ref pOutScreenshotHandle, type, pchPreviewFilename, pchVRFilename);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00003C5F File Offset: 0x00001E5F
		public EVRScreenshotError HookScreenshot(EVRScreenshotType[] pSupportedTypes)
		{
			return this.FnTable.HookScreenshot(pSupportedTypes, pSupportedTypes.Length);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00003C75 File Offset: 0x00001E75
		public EVRScreenshotType GetScreenshotPropertyType(uint screenshotHandle, ref EVRScreenshotError pError)
		{
			return this.FnTable.GetScreenshotPropertyType(screenshotHandle, ref pError);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00003C89 File Offset: 0x00001E89
		public uint GetScreenshotPropertyFilename(uint screenshotHandle, EVRScreenshotPropertyFilenames filenameType, StringBuilder pchFilename, uint cchFilename, ref EVRScreenshotError pError)
		{
			return this.FnTable.GetScreenshotPropertyFilename(screenshotHandle, filenameType, pchFilename, cchFilename, ref pError);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00003CA2 File Offset: 0x00001EA2
		public EVRScreenshotError UpdateScreenshotProgress(uint screenshotHandle, float flProgress)
		{
			return this.FnTable.UpdateScreenshotProgress(screenshotHandle, flProgress);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00003CB6 File Offset: 0x00001EB6
		public EVRScreenshotError TakeStereoScreenshot(ref uint pOutScreenshotHandle, string pchPreviewFilename, string pchVRFilename)
		{
			pOutScreenshotHandle = 0U;
			return this.FnTable.TakeStereoScreenshot(ref pOutScreenshotHandle, pchPreviewFilename, pchVRFilename);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00003CCE File Offset: 0x00001ECE
		public EVRScreenshotError SubmitScreenshot(uint screenshotHandle, EVRScreenshotType type, string pchSourcePreviewFilename, string pchSourceVRFilename)
		{
			return this.FnTable.SubmitScreenshot(screenshotHandle, type, pchSourcePreviewFilename, pchSourceVRFilename);
		}

		// Token: 0x04000152 RID: 338
		private IVRScreenshots FnTable;
	}
}
