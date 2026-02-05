using System;

namespace System.Net
{
	// Token: 0x0200019C RID: 412
	internal enum DataParseStatus
	{
		// Token: 0x04001311 RID: 4881
		NeedMoreData,
		// Token: 0x04001312 RID: 4882
		ContinueParsing,
		// Token: 0x04001313 RID: 4883
		Done,
		// Token: 0x04001314 RID: 4884
		Invalid,
		// Token: 0x04001315 RID: 4885
		DataTooBig
	}
}
