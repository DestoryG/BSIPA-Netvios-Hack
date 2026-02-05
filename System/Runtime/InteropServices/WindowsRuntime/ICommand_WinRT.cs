using System;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003EC RID: 1004
	[Guid("e5af3542-ca67-4081-995b-709dd13792df")]
	[ComImport]
	internal interface ICommand_WinRT
	{
		// Token: 0x0600261F RID: 9759
		EventRegistrationToken add_CanExecuteChanged(EventHandler<object> value);

		// Token: 0x06002620 RID: 9760
		void remove_CanExecuteChanged(EventRegistrationToken token);

		// Token: 0x06002621 RID: 9761
		bool CanExecute(object parameter);

		// Token: 0x06002622 RID: 9762
		void Execute(object parameter);
	}
}
