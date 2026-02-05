using System;

namespace System.IO.Compression
{
	// Token: 0x0200042C RID: 1068
	internal interface IFileFormatReader
	{
		// Token: 0x0600280B RID: 10251
		bool ReadHeader(InputBuffer input);

		// Token: 0x0600280C RID: 10252
		bool ReadFooter(InputBuffer input);

		// Token: 0x0600280D RID: 10253
		void UpdateWithBytesRead(byte[] buffer, int offset, int bytesToCopy);

		// Token: 0x0600280E RID: 10254
		void Validate();
	}
}
