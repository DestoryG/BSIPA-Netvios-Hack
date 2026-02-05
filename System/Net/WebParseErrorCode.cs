using System;

namespace System.Net
{
	// Token: 0x0200019F RID: 415
	internal enum WebParseErrorCode
	{
		// Token: 0x04001321 RID: 4897
		Generic,
		// Token: 0x04001322 RID: 4898
		InvalidHeaderName,
		// Token: 0x04001323 RID: 4899
		InvalidContentLength,
		// Token: 0x04001324 RID: 4900
		IncompleteHeaderLine,
		// Token: 0x04001325 RID: 4901
		CrLfError,
		// Token: 0x04001326 RID: 4902
		InvalidChunkFormat,
		// Token: 0x04001327 RID: 4903
		UnexpectedServerResponse
	}
}
