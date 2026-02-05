using System;

namespace System.Diagnostics
{
	// Token: 0x02000505 RID: 1285
	internal class ProcessData
	{
		// Token: 0x060030E1 RID: 12513 RVA: 0x000DDFC3 File Offset: 0x000DC1C3
		public ProcessData(int pid, long startTime)
		{
			this.ProcessId = pid;
			this.StartupTime = startTime;
		}

		// Token: 0x040028C9 RID: 10441
		public int ProcessId;

		// Token: 0x040028CA RID: 10442
		public long StartupTime;
	}
}
