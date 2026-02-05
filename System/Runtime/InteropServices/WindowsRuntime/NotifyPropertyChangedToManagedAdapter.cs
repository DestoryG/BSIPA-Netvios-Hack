using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003F3 RID: 1011
	internal sealed class NotifyPropertyChangedToManagedAdapter
	{
		// Token: 0x06002636 RID: 9782 RVA: 0x000B0746 File Offset: 0x000AE946
		private NotifyPropertyChangedToManagedAdapter()
		{
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06002637 RID: 9783 RVA: 0x000B0750 File Offset: 0x000AE950
		// (remove) Token: 0x06002638 RID: 9784 RVA: 0x000B0788 File Offset: 0x000AE988
		internal event PropertyChangedEventHandler PropertyChanged
		{
			[SecurityCritical]
			add
			{
				INotifyPropertyChanged_WinRT notifyPropertyChanged_WinRT = JitHelpers.UnsafeCast<INotifyPropertyChanged_WinRT>(this);
				Func<PropertyChangedEventHandler, EventRegistrationToken> func = new Func<PropertyChangedEventHandler, EventRegistrationToken>(notifyPropertyChanged_WinRT.add_PropertyChanged);
				Action<EventRegistrationToken> action = new Action<EventRegistrationToken>(notifyPropertyChanged_WinRT.remove_PropertyChanged);
				WindowsRuntimeMarshal.AddEventHandler<PropertyChangedEventHandler>(func, action, value);
			}
			[SecurityCritical]
			remove
			{
				INotifyPropertyChanged_WinRT notifyPropertyChanged_WinRT = JitHelpers.UnsafeCast<INotifyPropertyChanged_WinRT>(this);
				Action<EventRegistrationToken> action = new Action<EventRegistrationToken>(notifyPropertyChanged_WinRT.remove_PropertyChanged);
				WindowsRuntimeMarshal.RemoveEventHandler<PropertyChangedEventHandler>(action, value);
			}
		}
	}
}
