using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000D1 RID: 209
	internal class DataStream
	{
		// Token: 0x06000173 RID: 371 RVA: 0x000037D5 File Offset: 0x000019D5
		internal DataStream()
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00004339 File Offset: 0x00002539
		internal DataStream(int contentSize, BitAccess bits, int count)
		{
			this.contentSize = contentSize;
			if (count > 0)
			{
				this.pages = new int[count];
				bits.ReadInt32(this.pages);
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00004364 File Offset: 0x00002564
		internal void Read(PdbReader reader, BitAccess bits)
		{
			bits.MinCapacity(this.contentSize);
			this.Read(reader, 0, bits.Buffer, 0, this.contentSize);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00004388 File Offset: 0x00002588
		internal void Read(PdbReader reader, int position, byte[] bytes, int offset, int data)
		{
			if (position + data > this.contentSize)
			{
				throw new PdbException("DataStream can't read off end of stream. (pos={0},siz={1})", new object[] { position, data });
			}
			if (position == this.contentSize)
			{
				return;
			}
			int i = data;
			int num = position / reader.pageSize;
			int num2 = position % reader.pageSize;
			if (num2 != 0)
			{
				int num3 = reader.pageSize - num2;
				if (num3 > i)
				{
					num3 = i;
				}
				reader.Seek(this.pages[num], num2);
				reader.Read(bytes, offset, num3);
				offset += num3;
				i -= num3;
				num++;
			}
			while (i > 0)
			{
				int num4 = reader.pageSize;
				if (num4 > i)
				{
					num4 = i;
				}
				reader.Seek(this.pages[num], 0);
				reader.Read(bytes, offset, num4);
				offset += num4;
				i -= num4;
				num++;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000445D File Offset: 0x0000265D
		internal int Length
		{
			get
			{
				return this.contentSize;
			}
		}

		// Token: 0x04000480 RID: 1152
		internal int contentSize;

		// Token: 0x04000481 RID: 1153
		internal int[] pages;
	}
}
