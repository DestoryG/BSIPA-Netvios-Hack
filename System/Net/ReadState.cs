using System;

namespace System.Net
{
	// Token: 0x0200019B RID: 411
	internal enum ReadState
	{
		// Token: 0x0400130C RID: 4876
		Start,
		// Token: 0x0400130D RID: 4877
		StatusLine,
		// Token: 0x0400130E RID: 4878
		Headers,
		// Token: 0x0400130F RID: 4879
		Data
	}
}
