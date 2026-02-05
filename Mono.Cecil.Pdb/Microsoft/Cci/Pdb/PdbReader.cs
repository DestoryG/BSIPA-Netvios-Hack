using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000E2 RID: 226
	internal class PdbReader
	{
		// Token: 0x060001AD RID: 429 RVA: 0x0000673F File Offset: 0x0000493F
		internal PdbReader(Stream reader, int pageSize)
		{
			this.pageSize = pageSize;
			this.reader = reader;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006755 File Offset: 0x00004955
		internal void Seek(int page, int offset)
		{
			this.reader.Seek((long)(page * this.pageSize + offset), SeekOrigin.Begin);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000676F File Offset: 0x0000496F
		internal void Read(byte[] bytes, int offset, int count)
		{
			this.reader.Read(bytes, offset, count);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006780 File Offset: 0x00004980
		internal int PagesFromSize(int size)
		{
			return (size + this.pageSize - 1) / this.pageSize;
		}

		// Token: 0x040004EA RID: 1258
		internal readonly int pageSize;

		// Token: 0x040004EB RID: 1259
		internal readonly Stream reader;
	}
}
