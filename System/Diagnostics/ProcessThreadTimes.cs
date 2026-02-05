using System;

namespace System.Diagnostics
{
	// Token: 0x020004F5 RID: 1269
	internal class ProcessThreadTimes
	{
		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x000D97BE File Offset: 0x000D79BE
		public DateTime StartTime
		{
			get
			{
				return DateTime.FromFileTime(this.create);
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x000D97CB File Offset: 0x000D79CB
		public DateTime ExitTime
		{
			get
			{
				return DateTime.FromFileTime(this.exit);
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x000D97D8 File Offset: 0x000D79D8
		public TimeSpan PrivilegedProcessorTime
		{
			get
			{
				return new TimeSpan(this.kernel);
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x000D97E5 File Offset: 0x000D79E5
		public TimeSpan UserProcessorTime
		{
			get
			{
				return new TimeSpan(this.user);
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x000D97F2 File Offset: 0x000D79F2
		public TimeSpan TotalProcessorTime
		{
			get
			{
				return new TimeSpan(this.user + this.kernel);
			}
		}

		// Token: 0x04002877 RID: 10359
		internal long create;

		// Token: 0x04002878 RID: 10360
		internal long exit;

		// Token: 0x04002879 RID: 10361
		internal long kernel;

		// Token: 0x0400287A RID: 10362
		internal long user;
	}
}
