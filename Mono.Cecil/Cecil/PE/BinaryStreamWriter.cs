using System;
using System.IO;

namespace Mono.Cecil.PE
{
	// Token: 0x020000CE RID: 206
	internal class BinaryStreamWriter : BinaryWriter
	{
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001B0A6 File Offset: 0x000192A6
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x0001B0B4 File Offset: 0x000192B4
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

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001B0C3 File Offset: 0x000192C3
		public BinaryStreamWriter(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001B0CC File Offset: 0x000192CC
		public void WriteByte(byte value)
		{
			this.Write(value);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001B0D5 File Offset: 0x000192D5
		public void WriteUInt16(ushort value)
		{
			this.Write(value);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001B0DE File Offset: 0x000192DE
		public void WriteInt16(short value)
		{
			this.Write(value);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001B0E7 File Offset: 0x000192E7
		public void WriteUInt32(uint value)
		{
			this.Write(value);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0001B0F0 File Offset: 0x000192F0
		public void WriteInt32(int value)
		{
			this.Write(value);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001B0F9 File Offset: 0x000192F9
		public void WriteUInt64(ulong value)
		{
			this.Write(value);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001B102 File Offset: 0x00019302
		public void WriteBytes(byte[] bytes)
		{
			this.Write(bytes);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001B10B File Offset: 0x0001930B
		public void WriteDataDirectory(DataDirectory directory)
		{
			this.Write(directory.VirtualAddress);
			this.Write(directory.Size);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0001B125 File Offset: 0x00019325
		public void WriteBuffer(ByteBuffer buffer)
		{
			this.Write(buffer.buffer, 0, buffer.length);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001B13A File Offset: 0x0001933A
		protected void Advance(int bytes)
		{
			this.BaseStream.Seek((long)bytes, SeekOrigin.Current);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0001B14C File Offset: 0x0001934C
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
