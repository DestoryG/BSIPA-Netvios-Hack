using System;

namespace System.IO.Compression
{
	// Token: 0x02000420 RID: 1056
	internal interface IInflater : IDisposable
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06002775 RID: 10101
		int AvailableOutput { get; }

		// Token: 0x06002776 RID: 10102
		int Inflate(byte[] bytes, int offset, int length);

		// Token: 0x06002777 RID: 10103
		bool Finished();

		// Token: 0x06002778 RID: 10104
		bool NeedsInput();

		// Token: 0x06002779 RID: 10105
		void SetInput(byte[] inputBytes, int offset, int length);
	}
}
