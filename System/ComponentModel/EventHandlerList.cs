using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000551 RID: 1361
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class EventHandlerList : IDisposable
	{
		// Token: 0x0600332E RID: 13102 RVA: 0x000E36CD File Offset: 0x000E18CD
		public EventHandlerList()
		{
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000E36D5 File Offset: 0x000E18D5
		internal EventHandlerList(Component parent)
		{
			this.parent = parent;
		}

		// Token: 0x17000C83 RID: 3203
		public Delegate this[object key]
		{
			get
			{
				EventHandlerList.ListEntry listEntry = null;
				if (this.parent == null || this.parent.CanRaiseEventsInternal)
				{
					listEntry = this.Find(key);
				}
				if (listEntry != null)
				{
					return listEntry.handler;
				}
				return null;
			}
			set
			{
				EventHandlerList.ListEntry listEntry = this.Find(key);
				if (listEntry != null)
				{
					listEntry.handler = value;
					return;
				}
				this.head = new EventHandlerList.ListEntry(key, value, this.head);
			}
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000E3750 File Offset: 0x000E1950
		public void AddHandler(object key, Delegate value)
		{
			EventHandlerList.ListEntry listEntry = this.Find(key);
			if (listEntry != null)
			{
				listEntry.handler = Delegate.Combine(listEntry.handler, value);
				return;
			}
			this.head = new EventHandlerList.ListEntry(key, value, this.head);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000E3790 File Offset: 0x000E1990
		public void AddHandlers(EventHandlerList listToAddFrom)
		{
			for (EventHandlerList.ListEntry next = listToAddFrom.head; next != null; next = next.next)
			{
				this.AddHandler(next.key, next.handler);
			}
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000E37C2 File Offset: 0x000E19C2
		public void Dispose()
		{
			this.head = null;
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000E37CC File Offset: 0x000E19CC
		private EventHandlerList.ListEntry Find(object key)
		{
			EventHandlerList.ListEntry next = this.head;
			while (next != null && next.key != key)
			{
				next = next.next;
			}
			return next;
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000E37F8 File Offset: 0x000E19F8
		public void RemoveHandler(object key, Delegate value)
		{
			EventHandlerList.ListEntry listEntry = this.Find(key);
			if (listEntry != null)
			{
				listEntry.handler = Delegate.Remove(listEntry.handler, value);
			}
		}

		// Token: 0x040029AA RID: 10666
		private EventHandlerList.ListEntry head;

		// Token: 0x040029AB RID: 10667
		private Component parent;

		// Token: 0x02000894 RID: 2196
		private sealed class ListEntry
		{
			// Token: 0x06004591 RID: 17809 RVA: 0x00122F29 File Offset: 0x00121129
			public ListEntry(object key, Delegate handler, EventHandlerList.ListEntry next)
			{
				this.next = next;
				this.key = key;
				this.handler = handler;
			}

			// Token: 0x040037BE RID: 14270
			internal EventHandlerList.ListEntry next;

			// Token: 0x040037BF RID: 14271
			internal object key;

			// Token: 0x040037C0 RID: 14272
			internal Delegate handler;
		}
	}
}
