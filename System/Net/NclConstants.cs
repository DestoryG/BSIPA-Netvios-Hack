using System;

namespace System.Net
{
	// Token: 0x02000116 RID: 278
	internal static class NclConstants
	{
		// Token: 0x04000F5B RID: 3931
		internal static readonly object Sentinel = new object();

		// Token: 0x04000F5C RID: 3932
		internal static readonly object[] EmptyObjectArray = new object[0];

		// Token: 0x04000F5D RID: 3933
		internal static readonly Uri[] EmptyUriArray = new Uri[0];

		// Token: 0x04000F5E RID: 3934
		internal static readonly byte[] CRLF = new byte[] { 13, 10 };

		// Token: 0x04000F5F RID: 3935
		internal static readonly byte[] ChunkTerminator = new byte[] { 48, 13, 10, 13, 10 };
	}
}
