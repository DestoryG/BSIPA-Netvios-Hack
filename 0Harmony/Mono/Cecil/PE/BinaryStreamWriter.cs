using System;
using System.IO;

namespace Mono.Cecil.PE
{
	// Token: 0x02000190 RID: 400
	internal class BinaryStreamWriter : BinaryWriter
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0002A1A6 File Offset: 0x000283A6
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x0002A1B4 File Offset: 0x000283B4
		public int Position
		{
			get
			{
				return (int)this.BaseStream.Position;
			}
			set
			{
				this.BaseStream.Position = (long)value;
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0002A1C3 File Offset: 0x000283C3
		public BinaryStreamWriter(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0002A1CC File Offset: 0x000283CC
		public void WriteByte(byte value)
		{
			this.Write(value);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0002A1D5 File Offset: 0x000283D5
		public void WriteUInt16(ushort value)
		{
			this.Write(value);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0002A1DE File Offset: 0x000283DE
		public void WriteInt16(short value)
		{
			this.Write(value);
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0002A1E7 File Offset: 0x000283E7
		public void WriteUInt32(uint value)
		{
			this.Write(value);
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0002A1F0 File Offset: 0x000283F0
		public void WriteInt32(int value)
		{
			this.Write(value);
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0002A1F9 File Offset: 0x000283F9
		public void WriteUInt64(ulong value)
		{
			this.Write(value);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0002A202 File Offset: 0x00028402
		public void WriteBytes(byte[] bytes)
		{
			this.Write(bytes);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0002A20B File Offset: 0x0002840B
		public void WriteDataDirectory(DataDirectory directory)
		{
			this.Write(directory.VirtualAddress);
			this.Write(directory.Size);
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0002A225 File Offset: 0x00028425
		public void WriteBuffer(ByteBuffer buffer)
		{
			this.Write(buffer.buffer, 0, buffer.length);
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0002A23A File Offset: 0x0002843A
		protected void Advance(int bytes)
		{
			this.BaseStream.Seek((long)bytes, SeekOrigin.Current);
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0002A24C File Offset: 0x0002844C
		public void Align(int align)
		{
			align--;
			int position = this.Position;
			int num = ((position + align) & ~align) - position;
			for (int i = 0; i < num; i++)
			{
				this.WriteByte(0);
			}
		}
	}
}
