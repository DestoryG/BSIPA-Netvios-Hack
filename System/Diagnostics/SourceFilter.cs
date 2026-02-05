using System;

namespace System.Diagnostics
{
	// Token: 0x020004A3 RID: 1187
	public class SourceFilter : TraceFilter
	{
		// Token: 0x06002BFC RID: 11260 RVA: 0x000C6B95 File Offset: 0x000C4D95
		public SourceFilter(string source)
		{
			this.Source = source;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000C6BA4 File Offset: 0x000C4DA4
		public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return string.Equals(this.src, source);
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06002BFE RID: 11262 RVA: 0x000C6BC0 File Offset: 0x000C4DC0
		// (set) Token: 0x06002BFF RID: 11263 RVA: 0x000C6BC8 File Offset: 0x000C4DC8
		public string Source
		{
			get
			{
				return this.src;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("source");
				}
				this.src = value;
			}
		}

		// Token: 0x0400269B RID: 9883
		private string src;
	}
}
