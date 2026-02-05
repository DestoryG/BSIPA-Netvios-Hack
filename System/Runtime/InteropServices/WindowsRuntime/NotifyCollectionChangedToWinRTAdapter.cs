using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003F2 RID: 1010
	internal sealed class NotifyCollectionChangedToWinRTAdapter
	{
		// Token: 0x06002632 RID: 9778 RVA: 0x000B06CD File Offset: 0x000AE8CD
		private NotifyCollectionChangedToWinRTAdapter()
		{
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000B06D8 File Offset: 0x000AE8D8
		[SecurityCritical]
		internal EventRegistrationToken add_CollectionChanged(NotifyCollectionChangedEventHandler value)
		{
			INotifyCollectionChanged notifyCollectionChanged = JitHelpers.UnsafeCast<INotifyCollectionChanged>(this);
			EventRegistrationTokenTable<NotifyCollectionChangedEventHandler> orCreateValue = NotifyCollectionChangedToWinRTAdapter.m_weakTable.GetOrCreateValue(notifyCollectionChanged);
			EventRegistrationToken eventRegistrationToken = orCreateValue.AddEventHandler(value);
			notifyCollectionChanged.CollectionChanged += value;
			return eventRegistrationToken;
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000B0708 File Offset: 0x000AE908
		[SecurityCritical]
		internal void remove_CollectionChanged(EventRegistrationToken token)
		{
			INotifyCollectionChanged notifyCollectionChanged = JitHelpers.UnsafeCast<INotifyCollectionChanged>(this);
			EventRegistrationTokenTable<NotifyCollectionChangedEventHandler> orCreateValue = NotifyCollectionChangedToWinRTAdapter.m_weakTable.GetOrCreateValue(notifyCollectionChanged);
			NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler = orCreateValue.ExtractHandler(token);
			if (notifyCollectionChangedEventHandler != null)
			{
				notifyCollectionChanged.CollectionChanged -= notifyCollectionChangedEventHandler;
			}
		}

		// Token: 0x0400209F RID: 8351
		private static ConditionalWeakTable<INotifyCollectionChanged, EventRegistrationTokenTable<NotifyCollectionChangedEventHandler>> m_weakTable = new ConditionalWeakTable<INotifyCollectionChanged, EventRegistrationTokenTable<NotifyCollectionChangedEventHandler>>();
	}
}
