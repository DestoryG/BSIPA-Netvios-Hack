using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x020004F0 RID: 1264
	internal class ProcessInfo
	{
		// Token: 0x04002859 RID: 10329
		public ArrayList threadInfoList = new ArrayList();

		// Token: 0x0400285A RID: 10330
		public int basePriority;

		// Token: 0x0400285B RID: 10331
		public string processName;

		// Token: 0x0400285C RID: 10332
		public int processId;

		// Token: 0x0400285D RID: 10333
		public int handleCount;

		// Token: 0x0400285E RID: 10334
		public long poolPagedBytes;

		// Token: 0x0400285F RID: 10335
		public long poolNonpagedBytes;

		// Token: 0x04002860 RID: 10336
		public long virtualBytes;

		// Token: 0x04002861 RID: 10337
		public long virtualBytesPeak;

		// Token: 0x04002862 RID: 10338
		public long workingSetPeak;

		// Token: 0x04002863 RID: 10339
		public long workingSet;

		// Token: 0x04002864 RID: 10340
		public long pageFileBytesPeak;

		// Token: 0x04002865 RID: 10341
		public long pageFileBytes;

		// Token: 0x04002866 RID: 10342
		public long privateBytes;

		// Token: 0x04002867 RID: 10343
		public int mainModuleId;

		// Token: 0x04002868 RID: 10344
		public int sessionId;
	}
}
