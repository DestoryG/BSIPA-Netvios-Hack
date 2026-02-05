using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020004CA RID: 1226
	public class EventInstance
	{
		// Token: 0x06002DA9 RID: 11689 RVA: 0x000CD5B1 File Offset: 0x000CB7B1
		public EventInstance(long instanceId, int categoryId)
		{
			this.CategoryId = categoryId;
			this.InstanceId = instanceId;
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000CD5CE File Offset: 0x000CB7CE
		public EventInstance(long instanceId, int categoryId, EventLogEntryType entryType)
			: this(instanceId, categoryId)
		{
			this.EntryType = entryType;
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06002DAB RID: 11691 RVA: 0x000CD5DF File Offset: 0x000CB7DF
		// (set) Token: 0x06002DAC RID: 11692 RVA: 0x000CD5E7 File Offset: 0x000CB7E7
		public int CategoryId
		{
			get
			{
				return this._categoryNumber;
			}
			set
			{
				if (value > 65535 || value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._categoryNumber = value;
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002DAD RID: 11693 RVA: 0x000CD607 File Offset: 0x000CB807
		// (set) Token: 0x06002DAE RID: 11694 RVA: 0x000CD60F File Offset: 0x000CB80F
		public EventLogEntryType EntryType
		{
			get
			{
				return this._entryType;
			}
			set
			{
				if (!Enum.IsDefined(typeof(EventLogEntryType), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(EventLogEntryType));
				}
				this._entryType = value;
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06002DAF RID: 11695 RVA: 0x000CD645 File Offset: 0x000CB845
		// (set) Token: 0x06002DB0 RID: 11696 RVA: 0x000CD64D File Offset: 0x000CB84D
		public long InstanceId
		{
			get
			{
				return this._instanceId;
			}
			set
			{
				if (value > (long)((ulong)(-1)) || value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._instanceId = value;
			}
		}

		// Token: 0x04002729 RID: 10025
		private int _categoryNumber;

		// Token: 0x0400272A RID: 10026
		private EventLogEntryType _entryType = EventLogEntryType.Information;

		// Token: 0x0400272B RID: 10027
		private long _instanceId;
	}
}
