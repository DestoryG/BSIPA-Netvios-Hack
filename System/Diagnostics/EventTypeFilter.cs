using System;

namespace System.Diagnostics
{
	// Token: 0x020004A0 RID: 1184
	public class EventTypeFilter : TraceFilter
	{
		// Token: 0x06002BE3 RID: 11235 RVA: 0x000C6808 File Offset: 0x000C4A08
		public EventTypeFilter(SourceLevels level)
		{
			this.level = level;
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000C6817 File Offset: 0x000C4A17
		public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
		{
			return (eventType & (TraceEventType)this.level) > (TraceEventType)0;
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06002BE5 RID: 11237 RVA: 0x000C6824 File Offset: 0x000C4A24
		// (set) Token: 0x06002BE6 RID: 11238 RVA: 0x000C682C File Offset: 0x000C4A2C
		public SourceLevels EventType
		{
			get
			{
				return this.level;
			}
			set
			{
				this.level = value;
			}
		}

		// Token: 0x04002693 RID: 9875
		private SourceLevels level;
	}
}
