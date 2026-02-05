using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002F8 RID: 760
	internal class DataStream
	{
		// Token: 0x060011DA RID: 4570 RVA: 0x00002AB9 File Offset: 0x00000CB9
		internal DataStream()
		{
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0003A2BD File Offset: 0x000384BD
		internal DataStream(int contentSize, BitAccess bits, int count)
		{
			this.contentSize = contentSize;
			if (count > 0)
			{
				this.pages = new int[count];
				bits.ReadInt32(this.pages);
			}
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0003A2E8 File Offset: 0x000384E8
		internal void Read(PdbReader reader, BitAccess bits)
		{
			bits.MinCapacity(this.contentSize);
			this.Read(reader, 0, bits.Buffer, 0, this.contentSize);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0003A30C File Offset: 0x0003850C
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

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x0003A3E1 File Offset: 0x000385E1
		internal int Length
		{
			get
			{
				return this.contentSize;
			}
		}

		// Token: 0x04000E9C RID: 3740
		internal int contentSize;

		// Token: 0x04000E9D RID: 3741
		internal int[] pages;
	}
}
