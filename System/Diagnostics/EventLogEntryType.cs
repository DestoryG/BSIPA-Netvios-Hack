using System;

namespace System.Diagnostics
{
	// Token: 0x020004CF RID: 1231
	public enum EventLogEntryType
	{
		// Token: 0x04002763 RID: 10083
		Error = 1,
		// Token: 0x04002764 RID: 10084
		Warning,
		// Token: 0x04002765 RID: 10085
		Information = 4,
		// Token: 0x04002766 RID: 10086
		SuccessAudit = 8,
		// Token: 0x04002767 RID: 10087
		FailureAudit = 16
	}
}
