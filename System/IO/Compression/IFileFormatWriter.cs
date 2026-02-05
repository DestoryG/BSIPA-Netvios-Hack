using System;

namespace System.IO.Compression
{
	// Token: 0x0200042B RID: 1067
	internal interface IFileFormatWriter
	{
		// Token: 0x06002808 RID: 10248
		byte[] GetHeader();

		// Token: 0x06002809 RID: 10249
		void UpdateWithBytesRead(byte[] buffer, int offset, int bytesToCopy);

		// Token: 0x0600280A RID: 10250
		byte[] GetFooter();
	}
}
