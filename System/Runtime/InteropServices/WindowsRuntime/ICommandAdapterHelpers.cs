using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003F7 RID: 1015
	internal static class ICommandAdapterHelpers
	{
		// Token: 0x06002649 RID: 9801 RVA: 0x000B09C4 File Offset: 0x000AEBC4
		internal static EventHandler<object> CreateWrapperHandler(EventHandler handler)
		{
			return delegate(object sender, object e)
			{
				EventArgs eventArgs = e as EventArgs;
				handler(sender, (eventArgs == null) ? EventArgs.Empty : eventArgs);
			};
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000B09EC File Offset: 0x000AEBEC
		internal static EventHandler CreateWrapperHandler(EventHandler<object> handler)
		{
			return delegate(object sender, EventArgs e)
			{
				handler(sender, e);
			};
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000B0A14 File Offset: 0x000AEC14
		internal static EventHandler<object> GetValueFromEquivalentKey(ConditionalWeakTable<EventHandler, EventHandler<object>> table, EventHandler key, ConditionalWeakTable<EventHandler, EventHandler<object>>.CreateValueCallback callback)
		{
			EventHandler<object> eventHandler;
			if (table.FindEquivalentKeyUnsafe(key, out eventHandler) == null)
			{
				eventHandler = callback(key);
				table.Add(key, eventHandler);
			}
			return eventHandler;
		}
	}
}
