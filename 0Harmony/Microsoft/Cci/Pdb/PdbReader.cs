using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000311 RID: 785
	internal class PdbReader
	{
		// Token: 0x06001223 RID: 4643 RVA: 0x0003C95B File Offset: 0x0003AB5B
		internal PdbReader(Stream reader, int pageSize)
		{
			this.pageSize = pageSize;
			this.reader = reader;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0003C971 File Offset: 0x0003AB71
		internal void Seek(int page, int offset)
		{
			this.reader.Seek((long)(page * this.pageSize + offset), SeekOrigin.Begin);
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0003C98B File Offset: 0x0003AB8B
		internal void Read(byte[] bytes, int offset, int count)
		{
			this.reader.Read(bytes, offset, count);
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0003C99C File Offset: 0x0003AB9C
		internal int PagesFromSize(int size)
		{
			return (size + this.pageSize - 1) / this.pageSize;
		}

		// Token: 0x04000F13 RID: 3859
		internal readonly int pageSize;

		// Token: 0x04000F14 RID: 3860
		internal readonly Stream reader;
	}
}
