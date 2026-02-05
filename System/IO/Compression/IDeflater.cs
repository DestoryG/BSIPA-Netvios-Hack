using System;

namespace System.IO.Compression
{
	// Token: 0x0200041F RID: 1055
	internal interface IDeflater : IDisposable
	{
		// Token: 0x06002771 RID: 10097
		bool NeedsInput();

		// Token: 0x06002772 RID: 10098
		void SetInput(byte[] inputBuffer, int startIndex, int count);

		// Token: 0x06002773 RID: 10099
		int GetDeflateOutput(byte[] outputBuffer);

		// Token: 0x06002774 RID: 10100
		bool Finish(byte[] outputBuffer, out int bytesRead);
	}
}
