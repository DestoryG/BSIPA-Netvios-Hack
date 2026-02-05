using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000202 RID: 514
	internal class MyBinaryReader : BinaryReader
	{
		// Token: 0x06000F60 RID: 3936 RVA: 0x0002A13E File Offset: 0x0002833E
		public MyBinaryReader(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00033D74 File Offset: 0x00031F74
		public int ReadLeb128()
		{
			return base.Read7BitEncodedInt();
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00033D7C File Offset: 0x00031F7C
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
