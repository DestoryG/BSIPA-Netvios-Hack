using System;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003F5 RID: 1013
	[SecurityCritical]
	internal sealed class ICommandToManagedAdapter
	{
		// Token: 0x0600263D RID: 9789 RVA: 0x000B082A File Offset: 0x000AEA2A
		private ICommandToManagedAdapter()
		{
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x0600263E RID: 9790 RVA: 0x000B0834 File Offset: 0x000AEA34
		// (remove) Token: 0x0600263F RID: 9791 RVA: 0x000B0884 File Offset: 0x000AEA84
		private event EventHandler CanExecuteChanged
		{
			add
			{
				ICommand_WinRT command_WinRT = JitHelpers.UnsafeCast<ICommand_WinRT>(this);
				Func<EventHandler<object>, EventRegistrationToken> func = new Func<EventHandler<object>, EventRegistrationToken>(command_WinRT.add_CanExecuteChanged);
				Action<EventRegistrationToken> action = new Action<EventRegistrationToken>(command_WinRT.remove_CanExecuteChanged);
				EventHandler<object> value2 = ICommandToManagedAdapter.m_weakTable.GetValue(value, new ConditionalWeakTable<EventHandler, EventHandler<object>>.CreateValueCallback(ICommandAdapterHelpers.CreateWrapperHandler));
				WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(func, action, value2);
			}
			remove
			{
				ICommand_WinRT command_WinRT = JitHelpers.UnsafeCast<ICommand_WinRT>(this);
				Action<EventRegistrationToken> action = new Action<EventRegistrationToken>(command_WinRT.remove_CanExecuteChanged);
				EventHandler<object> valueFromEquivalentKey = ICommandAdapterHelpers.GetValueFromEquivalentKey(ICommandToManagedAdapter.m_weakTable, value, new ConditionalWeakTable<EventHandler, EventHandler<object>>.CreateValueCallback(ICommandAdapterHelpers.CreateWrapperHandler));
				WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>(action, valueFromEquivalentKey);
			}
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000B08C8 File Offset: 0x000AEAC8
		private bool CanExecute(object parameter)
		{
			ICommand_WinRT command_WinRT = JitHelpers.UnsafeCast<ICommand_WinRT>(this);
			return command_WinRT.CanExecute(parameter);
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000B08E4 File Offset: 0x000AEAE4
		private void Execute(object parameter)
		{
			ICommand_WinRT command_WinRT = JitHelpers.UnsafeCast<ICommand_WinRT>(this);
			command_WinRT.Execute(parameter);
		}

		// Token: 0x040020A1 RID: 8353
		private static ConditionalWeakTable<EventHandler, EventHandler<object>> m_weakTable = new ConditionalWeakTable<EventHandler, EventHandler<object>>();
	}
}
