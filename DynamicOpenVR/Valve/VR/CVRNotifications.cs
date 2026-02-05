using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200001C RID: 28
	public class CVRNotifications
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00003A9F File Offset: 0x00001C9F
		internal CVRNotifications(IntPtr pInterface)
		{
			this.FnTable = (IVRNotifications)Marshal.PtrToStructure(pInterface, typeof(IVRNotifications));
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00003AC2 File Offset: 0x00001CC2
		public EVRNotificationError CreateNotification(ulong ulOverlayHandle, ulong ulUserValue, EVRNotificationType type, string pchText, EVRNotificationStyle style, ref NotificationBitmap_t pImage, ref uint pNotificationId)
		{
			pNotificationId = 0U;
			return this.FnTable.CreateNotification(ulOverlayHandle, ulUserValue, type, pchText, style, ref pImage, ref pNotificationId);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003AE3 File Offset: 0x00001CE3
		public EVRNotificationError RemoveNotification(uint notificationId)
		{
			return this.FnTable.RemoveNotification(notificationId);
		}

		// Token: 0x04000150 RID: 336
		private IVRNotifications FnTable;
	}
}
