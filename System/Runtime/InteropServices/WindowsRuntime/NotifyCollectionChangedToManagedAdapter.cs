using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003F1 RID: 1009
	internal sealed class NotifyCollectionChangedToManagedAdapter
	{
		// Token: 0x0600262F RID: 9775 RVA: 0x000B0661 File Offset: 0x000AE861
		private NotifyCollectionChangedToManagedAdapter()
		{
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06002630 RID: 9776 RVA: 0x000B066C File Offset: 0x000AE86C
		// (remove) Token: 0x06002631 RID: 9777 RVA: 0x000B06A4 File Offset: 0x000AE8A4
		internal event NotifyCollectionChangedEventHandler CollectionChanged
		{
			[SecurityCritical]
			add
			{
				INotifyCollectionChanged_WinRT notifyCollectionChanged_WinRT = JitHelpers.UnsafeCast<INotifyCollectionChanged_WinRT>(this);
				Func<NotifyCollectionChangedEventHandler, EventRegistrationToken> func = new Func<NotifyCollectionChangedEventHandler, EventRegistrationToken>(notifyCollectionChanged_WinRT.add_CollectionChanged);
				Action<EventRegistrationToken> action = new Action<EventRegistrationToken>(notifyCollectionChanged_WinRT.remove_CollectionChanged);
				WindowsRuntimeMarshal.AddEventHandler<NotifyCollectionChangedEventHandler>(func, action, value);
			}
			[SecurityCritical]
			remove
			{
				INotifyCollectionChanged_WinRT notifyCollectionChanged_WinRT = JitHelpers.UnsafeCast<INotifyCollectionChanged_WinRT>(this);
				Action<EventRegistrationToken> action = new Action<EventRegistrationToken>(notifyCollectionChanged_WinRT.remove_CollectionChanged);
				WindowsRuntimeMarshal.RemoveEventHandler<NotifyCollectionChangedEventHandler>(action, value);
			}
		}
	}
}
