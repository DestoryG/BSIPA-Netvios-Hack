using System;

namespace System.Diagnostics
{
	// Token: 0x020004C8 RID: 1224
	public class EntryWrittenEventArgs : EventArgs
	{
		// Token: 0x06002DA2 RID: 11682 RVA: 0x000CD592 File Offset: 0x000CB792
		public EntryWrittenEventArgs()
		{
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000CD59A File Offset: 0x000CB79A
		public EntryWrittenEventArgs(EventLogEntry entry)
		{
			this.entry = entry;
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x000CD5A9 File Offset: 0x000CB7A9
		public EventLogEntry Entry
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x04002728 RID: 10024
		private EventLogEntry entry;
	}
}
