using System;

namespace System.Xml
{
	// Token: 0x02000052 RID: 82
	internal class MtomBinaryData
	{
		// Token: 0x060005AE RID: 1454 RVA: 0x0001B6F3 File Offset: 0x000198F3
		internal MtomBinaryData(IStreamProvider provider)
		{
			this.type = MtomBinaryDataType.Provider;
			this.provider = provider;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001B709 File Offset: 0x00019909
		internal MtomBinaryData(byte[] buffer, int offset, int count)
		{
			this.type = MtomBinaryDataType.Segment;
			this.chunk = new byte[count];
			Buffer.BlockCopy(buffer, offset, this.chunk, 0, count);
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0001B733 File Offset: 0x00019933
		internal long Length
		{
			get
			{
				if (this.type == MtomBinaryDataType.Segment)
				{
					return (long)this.chunk.Length;
				}
				return -1L;
			}
		}

		// Token: 0x04000285 RID: 645
		internal MtomBinaryDataType type;

		// Token: 0x04000286 RID: 646
		internal IStreamProvider provider;

		// Token: 0x04000287 RID: 647
		internal byte[] chunk;
	}
}
