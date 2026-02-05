using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000304 RID: 772
	[SuppressUnmanagedCodeSecurity]
	internal class TeredoHelper
	{
		// Token: 0x06001B5B RID: 7003 RVA: 0x00081D1F File Offset: 0x0007FF1F
		static TeredoHelper()
		{
			AppDomain.CurrentDomain.DomainUnload += TeredoHelper.OnAppDomainUnload;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x00081D41 File Offset: 0x0007FF41
		private TeredoHelper(Action<object> callback, object state)
		{
			this.callback = callback;
			this.state = state;
			this.onStabilizedDelegate = new StableUnicastIpAddressTableDelegate(this.OnStabilized);
			this.runCallbackCalled = false;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x00081D70 File Offset: 0x0007FF70
		public static bool UnsafeNotifyStableUnicastIpAddressTable(Action<object> callback, object state)
		{
			TeredoHelper teredoHelper = new TeredoHelper(callback, state);
			uint num = 0U;
			SafeFreeMibTable safeFreeMibTable = null;
			List<TeredoHelper> list = TeredoHelper.pendingNotifications;
			lock (list)
			{
				if (TeredoHelper.impendingAppDomainUnload)
				{
					return false;
				}
				num = UnsafeNetInfoNativeMethods.NotifyStableUnicastIpAddressTable(AddressFamily.Unspecified, out safeFreeMibTable, teredoHelper.onStabilizedDelegate, IntPtr.Zero, out teredoHelper.cancelHandle);
				if (safeFreeMibTable != null)
				{
					safeFreeMibTable.Dispose();
				}
				if (num == 997U)
				{
					TeredoHelper.pendingNotifications.Add(teredoHelper);
					return false;
				}
			}
			if (num != 0U)
			{
				throw new Win32Exception((int)num);
			}
			return true;
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x00081E0C File Offset: 0x0008000C
		private static void OnAppDomainUnload(object sender, EventArgs args)
		{
			List<TeredoHelper> list = TeredoHelper.pendingNotifications;
			lock (list)
			{
				TeredoHelper.impendingAppDomainUnload = true;
				foreach (TeredoHelper teredoHelper in TeredoHelper.pendingNotifications)
				{
					teredoHelper.cancelHandle.Dispose();
				}
			}
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x00081E90 File Offset: 0x00080090
		private void RunCallback(object o)
		{
			List<TeredoHelper> list = TeredoHelper.pendingNotifications;
			lock (list)
			{
				if (TeredoHelper.impendingAppDomainUnload)
				{
					return;
				}
				TeredoHelper.pendingNotifications.Remove(this);
				this.cancelHandle.Dispose();
			}
			this.callback(this.state);
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x00081EFC File Offset: 0x000800FC
		private void OnStabilized(IntPtr context, IntPtr table)
		{
			UnsafeNetInfoNativeMethods.FreeMibTable(table);
			if (!this.runCallbackCalled)
			{
				lock (this)
				{
					if (!this.runCallbackCalled)
					{
						this.runCallbackCalled = true;
						ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.RunCallback), null);
					}
				}
			}
		}

		// Token: 0x04001AE1 RID: 6881
		private static List<TeredoHelper> pendingNotifications = new List<TeredoHelper>();

		// Token: 0x04001AE2 RID: 6882
		private static bool impendingAppDomainUnload;

		// Token: 0x04001AE3 RID: 6883
		private readonly Action<object> callback;

		// Token: 0x04001AE4 RID: 6884
		private readonly object state;

		// Token: 0x04001AE5 RID: 6885
		private bool runCallbackCalled;

		// Token: 0x04001AE6 RID: 6886
		private readonly StableUnicastIpAddressTableDelegate onStabilizedDelegate;

		// Token: 0x04001AE7 RID: 6887
		private SafeCancelMibChangeNotify cancelHandle;
	}
}
