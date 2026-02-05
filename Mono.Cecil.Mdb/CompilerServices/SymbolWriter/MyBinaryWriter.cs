using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000003 RID: 3
	internal sealed class MyBinaryWriter : BinaryWriter
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002071 File Offset: 0x00000271
		public MyBinaryWriter(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000207A File Offset: 0x0000027A
		public void WriteLeb128(int value)
		{
			base.Write7BitEncodedInt(value);
		}
	}
}
