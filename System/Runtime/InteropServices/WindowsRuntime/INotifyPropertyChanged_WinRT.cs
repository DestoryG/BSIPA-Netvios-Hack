using System;
using System.ComponentModel;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003EB RID: 1003
	[Guid("cf75d69c-f2f4-486b-b302-bb4c09baebfa")]
	[ComImport]
	internal interface INotifyPropertyChanged_WinRT
	{
		// Token: 0x0600261D RID: 9757
		EventRegistrationToken add_PropertyChanged(PropertyChangedEventHandler value);

		// Token: 0x0600261E RID: 9758
		void remove_PropertyChanged(EventRegistrationToken token);
	}
}
