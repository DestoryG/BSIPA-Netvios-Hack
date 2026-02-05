using System;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x02000246 RID: 582
	internal interface IEncodableStream
	{
		// Token: 0x06001611 RID: 5649
		int DecodeBytes(byte[] buffer, int offset, int count);

		// Token: 0x06001612 RID: 5650
		int EncodeBytes(byte[] buffer, int offset, int count);

		// Token: 0x06001613 RID: 5651
		string GetEncodedString();

		// Token: 0x06001614 RID: 5652
		Stream GetStream();
	}
}
