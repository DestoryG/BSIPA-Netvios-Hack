using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Input;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003F6 RID: 1014
	[SecurityCritical]
	internal sealed class ICommandToWinRTAdapter
	{
		// Token: 0x06002643 RID: 9795 RVA: 0x000B090B File Offset: 0x000AEB0B
		private ICommandToWinRTAdapter()
		{
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000B0914 File Offset: 0x000AEB14
		private EventRegistrationToken add_CanExecuteChanged(EventHandler<object> value)
		{
			ICommand command = JitHelpers.UnsafeCast<ICommand>(this);
			EventRegistrationTokenTable<EventHandler> orCreateValue = ICommandToWinRTAdapter.m_weakTable.GetOrCreateValue(command);
			EventHandler eventHandler = ICommandAdapterHelpers.CreateWrapperHandler(value);
			EventRegistrationToken eventRegistrationToken = orCreateValue.AddEventHandler(eventHandler);
			command.CanExecuteChanged += eventHandler;
			return eventRegistrationToken;
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000B094C File Offset: 0x000AEB4C
		private void remove_CanExecuteChanged(EventRegistrationToken token)
		{
			ICommand command = JitHelpers.UnsafeCast<ICommand>(this);
			EventRegistrationTokenTable<EventHandler> orCreateValue = ICommandToWinRTAdapter.m_weakTable.GetOrCreateValue(command);
			EventHandler eventHandler = orCreateValue.ExtractHandler(token);
			if (eventHandler != null)
			{
				command.CanExecuteChanged -= eventHandler;
			}
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000B0980 File Offset: 0x000AEB80
		private bool CanExecute(object parameter)
		{
			ICommand command = JitHelpers.UnsafeCast<ICommand>(this);
			return command.CanExecute(parameter);
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000B099C File Offset: 0x000AEB9C
		private void Execute(object parameter)
		{
			ICommand command = JitHelpers.UnsafeCast<ICommand>(this);
			command.Execute(parameter);
		}

		// Token: 0x040020A2 RID: 8354
		private static ConditionalWeakTable<ICommand, EventRegistrationTokenTable<EventHandler>> m_weakTable = new ConditionalWeakTable<ICommand, EventRegistrationTokenTable<EventHandler>>();
	}
}
