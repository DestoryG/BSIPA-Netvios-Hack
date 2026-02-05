using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200000D RID: 13
	public struct IVRScreenshots
	{
		// Token: 0x04000118 RID: 280
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._RequestScreenshot RequestScreenshot;

		// Token: 0x04000119 RID: 281
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._HookScreenshot HookScreenshot;

		// Token: 0x0400011A RID: 282
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._GetScreenshotPropertyType GetScreenshotPropertyType;

		// Token: 0x0400011B RID: 283
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._GetScreenshotPropertyFilename GetScreenshotPropertyFilename;

		// Token: 0x0400011C RID: 284
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._UpdateScreenshotProgress UpdateScreenshotProgress;

		// Token: 0x0400011D RID: 285
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._TakeStereoScreenshot TakeStereoScreenshot;

		// Token: 0x0400011E RID: 286
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._SubmitScreenshot SubmitScreenshot;

		// Token: 0x02000200 RID: 512
		// (Invoke) Token: 0x060006B4 RID: 1716
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _RequestScreenshot(ref uint pOutScreenshotHandle, EVRScreenshotType type, string pchPreviewFilename, string pchVRFilename);

		// Token: 0x02000201 RID: 513
		// (Invoke) Token: 0x060006B8 RID: 1720
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _HookScreenshot([In] [Out] EVRScreenshotType[] pSupportedTypes, int numTypes);

		// Token: 0x02000202 RID: 514
		// (Invoke) Token: 0x060006BC RID: 1724
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotType _GetScreenshotPropertyType(uint screenshotHandle, ref EVRScreenshotError pError);

		// Token: 0x02000203 RID: 515
		// (Invoke) Token: 0x060006C0 RID: 1728
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetScreenshotPropertyFilename(uint screenshotHandle, EVRScreenshotPropertyFilenames filenameType, StringBuilder pchFilename, uint cchFilename, ref EVRScreenshotError pError);

		// Token: 0x02000204 RID: 516
		// (Invoke) Token: 0x060006C4 RID: 1732
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _UpdateScreenshotProgress(uint screenshotHandle, float flProgress);

		// Token: 0x02000205 RID: 517
		// (Invoke) Token: 0x060006C8 RID: 1736
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _TakeStereoScreenshot(ref uint pOutScreenshotHandle, string pchPreviewFilename, string pchVRFilename);

		// Token: 0x02000206 RID: 518
		// (Invoke) Token: 0x060006CC RID: 1740
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _SubmitScreenshot(uint screenshotHandle, EVRScreenshotType type, string pchSourcePreviewFilename, string pchSourceVRFilename);
	}
}
