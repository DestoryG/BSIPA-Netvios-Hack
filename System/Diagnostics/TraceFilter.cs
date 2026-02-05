using System;

namespace System.Diagnostics
{
	// Token: 0x020004B0 RID: 1200
	public abstract class TraceFilter
	{
		// Token: 0x06002C87 RID: 11399
		public abstract bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data);

		// Token: 0x06002C88 RID: 11400 RVA: 0x000C7D30 File Offset: 0x000C5F30
		internal bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage)
		{
			return this.ShouldTrace(cache, source, eventType, id, formatOrMessage, null, null, null);
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x000C7D50 File Offset: 0x000C5F50
		internal bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args)
		{
			return this.ShouldTrace(cache, source, eventType, id, formatOrMessage, args, null, null);
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x000C7D70 File Offset: 0x000C5F70
		internal bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1)
		{
			return this.ShouldTrace(cache, source, eventType, id, formatOrMessage, args, data1, null);
		}

		// Token: 0x040026D2 RID: 9938
		internal string initializeData;
	}
}
