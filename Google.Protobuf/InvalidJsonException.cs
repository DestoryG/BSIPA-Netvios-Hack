using System;
using System.IO;

namespace Google.Protobuf
{
	// Token: 0x02000018 RID: 24
	public sealed class InvalidJsonException : IOException
	{
		// Token: 0x0600014B RID: 331 RVA: 0x000062FA File Offset: 0x000044FA
		internal InvalidJsonException(string message)
			: base(message)
		{
		}
	}
}
