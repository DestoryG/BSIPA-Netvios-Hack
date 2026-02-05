using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000004 RID: 4
	internal class MyBinaryReader : BinaryReader
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002083 File Offset: 0x00000283
		public MyBinaryReader(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000208C File Offset: 0x0000028C
		public int ReadLeb128()
		{
			return base.Read7BitEncodedInt();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002094 File Offset: 0x00000294
		public string ReadString(int offset)
		{
			long position = this.BaseStream.Position;
			this.BaseStream.Position = (long)offset;
			string text = this.ReadString();
			this.BaseStream.Position = position;
			return text;
		}
	}
}
