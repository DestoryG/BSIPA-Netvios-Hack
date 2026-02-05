using System;

namespace System.Net
{
	// Token: 0x020000E5 RID: 229
	internal enum BoundaryType
	{
		// Token: 0x04000D35 RID: 3381
		ContentLength,
		// Token: 0x04000D36 RID: 3382
		Chunked,
		// Token: 0x04000D37 RID: 3383
		Multipart = 3,
		// Token: 0x04000D38 RID: 3384
		None,
		// Token: 0x04000D39 RID: 3385
		Invalid
	}
}
