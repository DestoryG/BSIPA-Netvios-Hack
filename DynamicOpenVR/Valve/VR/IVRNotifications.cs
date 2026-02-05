using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200000B RID: 11
	public struct IVRNotifications
	{
		// Token: 0x0400010A RID: 266
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRNotifications._CreateNotification CreateNotification;

		// Token: 0x0400010B RID: 267
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRNotifications._RemoveNotification RemoveNotification;

		// Token: 0x020001F2 RID: 498
		// (Invoke) Token: 0x0600067C RID: 1660
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRNotificationError _CreateNotification(ulong ulOverlayHandle, ulong ulUserValue, EVRNotificationType type, string pchText, EVRNotificationStyle style, ref NotificationBitmap_t pImage, ref uint pNotificationId);

		// Token: 0x020001F3 RID: 499
		// (Invoke) Token: 0x06000680 RID: 1664
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRNotificationError _RemoveNotification(uint notificationId);
	}
}
