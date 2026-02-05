using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000201 RID: 513
	internal sealed class MyBinaryWriter : BinaryWriter
	{
		// Token: 0x06000F5E RID: 3934 RVA: 0x0002A1C3 File Offset: 0x000283C3
		public MyBinaryWriter(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00033D6B File Offset: 0x00031F6B
		public void WriteLeb128(int value)
		{
			base.Write7BitEncodedInt(value);
		}
	}
}
