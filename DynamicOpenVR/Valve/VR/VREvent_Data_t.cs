using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200006C RID: 108
	[StructLayout(LayoutKind.Explicit)]
	public struct VREvent_Data_t
	{
		// Token: 0x0400054F RID: 1359
		[FieldOffset(0)]
		public VREvent_Reserved_t reserved;

		// Token: 0x04000550 RID: 1360
		[FieldOffset(0)]
		public VREvent_Controller_t controller;

		// Token: 0x04000551 RID: 1361
		[FieldOffset(0)]
		public VREvent_Mouse_t mouse;

		// Token: 0x04000552 RID: 1362
		[FieldOffset(0)]
		public VREvent_Scroll_t scroll;

		// Token: 0x04000553 RID: 1363
		[FieldOffset(0)]
		public VREvent_Process_t process;

		// Token: 0x04000554 RID: 1364
		[FieldOffset(0)]
		public VREvent_Notification_t notification;

		// Token: 0x04000555 RID: 1365
		[FieldOffset(0)]
		public VREvent_Overlay_t overlay;

		// Token: 0x04000556 RID: 1366
		[FieldOffset(0)]
		public VREvent_Status_t status;

		// Token: 0x04000557 RID: 1367
		[FieldOffset(0)]
		public VREvent_Ipd_t ipd;

		// Token: 0x04000558 RID: 1368
		[FieldOffset(0)]
		public VREvent_Chaperone_t chaperone;

		// Token: 0x04000559 RID: 1369
		[FieldOffset(0)]
		public VREvent_PerformanceTest_t performanceTest;

		// Token: 0x0400055A RID: 1370
		[FieldOffset(0)]
		public VREvent_TouchPadMove_t touchPadMove;

		// Token: 0x0400055B RID: 1371
		[FieldOffset(0)]
		public VREvent_SeatedZeroPoseReset_t seatedZeroPoseReset;

		// Token: 0x0400055C RID: 1372
		[FieldOffset(0)]
		public VREvent_Screenshot_t screenshot;

		// Token: 0x0400055D RID: 1373
		[FieldOffset(0)]
		public VREvent_ScreenshotProgress_t screenshotProgress;

		// Token: 0x0400055E RID: 1374
		[FieldOffset(0)]
		public VREvent_ApplicationLaunch_t applicationLaunch;

		// Token: 0x0400055F RID: 1375
		[FieldOffset(0)]
		public VREvent_EditingCameraSurface_t cameraSurface;

		// Token: 0x04000560 RID: 1376
		[FieldOffset(0)]
		public VREvent_MessageOverlay_t messageOverlay;

		// Token: 0x04000561 RID: 1377
		[FieldOffset(0)]
		public VREvent_Property_t property;

		// Token: 0x04000562 RID: 1378
		[FieldOffset(0)]
		public VREvent_DualAnalog_t dualAnalog;

		// Token: 0x04000563 RID: 1379
		[FieldOffset(0)]
		public VREvent_HapticVibration_t hapticVibration;

		// Token: 0x04000564 RID: 1380
		[FieldOffset(0)]
		public VREvent_WebConsole_t webConsole;

		// Token: 0x04000565 RID: 1381
		[FieldOffset(0)]
		public VREvent_InputBindingLoad_t inputBinding;

		// Token: 0x04000566 RID: 1382
		[FieldOffset(0)]
		public VREvent_SpatialAnchor_t spatialAnchor;

		// Token: 0x04000567 RID: 1383
		[FieldOffset(0)]
		public VREvent_InputActionManifestLoad_t actionManifest;

		// Token: 0x04000568 RID: 1384
		[FieldOffset(0)]
		public VREvent_ProgressUpdate_t progressUpdate;

		// Token: 0x04000569 RID: 1385
		[FieldOffset(0)]
		public VREvent_ShowUI_t showUi;

		// Token: 0x0400056A RID: 1386
		[FieldOffset(0)]
		public VREvent_ShowDevTools_t showDevTools;

		// Token: 0x0400056B RID: 1387
		[FieldOffset(0)]
		public VREvent_HDCPError_t hdcpError;

		// Token: 0x0400056C RID: 1388
		[FieldOffset(0)]
		public VREvent_Keyboard_t keyboard;
	}
}
