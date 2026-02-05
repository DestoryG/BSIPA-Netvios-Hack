using System;

namespace System.Xml
{
	// Token: 0x0200004D RID: 77
	internal static class MimeGlobals
	{
		// Token: 0x04000268 RID: 616
		internal static string MimeVersionHeader = "MIME-Version";

		// Token: 0x04000269 RID: 617
		internal static string DefaultVersion = "1.0";

		// Token: 0x0400026A RID: 618
		internal static string ContentIDScheme = "cid:";

		// Token: 0x0400026B RID: 619
		internal static string ContentIDHeader = "Content-ID";

		// Token: 0x0400026C RID: 620
		internal static string ContentTypeHeader = "Content-Type";

		// Token: 0x0400026D RID: 621
		internal static string ContentTransferEncodingHeader = "Content-Transfer-Encoding";

		// Token: 0x0400026E RID: 622
		internal static string EncodingBinary = "binary";

		// Token: 0x0400026F RID: 623
		internal static string Encoding8bit = "8bit";

		// Token: 0x04000270 RID: 624
		internal static byte[] COLONSPACE = new byte[] { 58, 32 };

		// Token: 0x04000271 RID: 625
		internal static byte[] DASHDASH = new byte[] { 45, 45 };

		// Token: 0x04000272 RID: 626
		internal static byte[] CRLF = new byte[] { 13, 10 };

		// Token: 0x04000273 RID: 627
		internal static byte[] BoundaryPrefix = new byte[] { 13, 10, 45, 45 };
	}
}
