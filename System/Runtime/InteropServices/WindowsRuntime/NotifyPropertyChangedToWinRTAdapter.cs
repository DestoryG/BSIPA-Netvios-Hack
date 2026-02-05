using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003F4 RID: 1012
	internal sealed class NotifyPropertyChangedToWinRTAdapter
	{
		// Token: 0x06002639 RID: 9785 RVA: 0x000B07B1 File Offset: 0x000AE9B1
		private NotifyPropertyChangedToWinRTAdapter()
		{
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000B07BC File Offset: 0x000AE9BC
		[SecurityCritical]
		internal EventRegistrationToken add_PropertyChanged(PropertyChangedEventHandler value)
		{
			INotifyPropertyChanged notifyPropertyChanged = JitHelpers.UnsafeCast<INotifyPropertyChanged>(this);
			EventRegistrationTokenTable<PropertyChangedEventHandler> orCreateValue = NotifyPropertyChangedToWinRTAdapter.m_weakTable.GetOrCreateValue(notifyPropertyChanged);
			EventRegistrationToken eventRegistrationToken = orCreateValue.AddEventHandler(value);
			notifyPropertyChanged.PropertyChanged += value;
			return eventRegistrationToken;
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000B07EC File Offset: 0x000AE9EC
		[SecurityCritical]
		internal void remove_PropertyChanged(EventRegistrationToken token)
		{
			INotifyPropertyChanged notifyPropertyChanged = JitHelpers.UnsafeCast<INotifyPropertyChanged>(this);
			EventRegistrationTokenTable<PropertyChangedEventHandler> orCreateValue = NotifyPropertyChangedToWinRTAdapter.m_weakTable.GetOrCreateValue(notifyPropertyChanged);
			PropertyChangedEventHandler propertyChangedEventHandler = orCreateValue.ExtractHandler(token);
			if (propertyChangedEventHandler != null)
			{
				notifyPropertyChanged.PropertyChanged -= propertyChangedEventHandler;
			}
		}

		// Token: 0x040020A0 RID: 8352
		private static ConditionalWeakTable<INotifyPropertyChanged, EventRegistrationTokenTable<PropertyChangedEventHandler>> m_weakTable = new ConditionalWeakTable<INotifyPropertyChanged, EventRegistrationTokenTable<PropertyChangedEventHandler>>();
	}
}
