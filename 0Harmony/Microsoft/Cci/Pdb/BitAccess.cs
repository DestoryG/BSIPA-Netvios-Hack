using System;
using System.IO;
using System.Text;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200023C RID: 572
	internal class BitAccess
	{
		// Token: 0x060011B9 RID: 4537 RVA: 0x00039B47 File Offset: 0x00037D47
		internal BitAccess(int capacity)
		{
			this.buffer = new byte[capacity];
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00039B5B File Offset: 0x00037D5B
		internal BitAccess(byte[] buffer)
		{
			this.buffer = buffer;
			this.offset = 0;
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00039B71 File Offset: 0x00037D71
		internal byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00039B79 File Offset: 0x00037D79
		internal void FillBuffer(Stream stream, int capacity)
		{
			this.MinCapacity(capacity);
			stream.Read(this.buffer, 0, capacity);
			this.offset = 0;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00039B98 File Offset: 0x00037D98
		internal void Append(Stream stream, int count)
		{
			int num = this.offset + count;
			if (this.buffer.Length < num)
			{
				byte[] array = new byte[num];
				Array.Copy(this.buffer, array, this.buffer.Length);
				this.buffer = array;
			}
			stream.Read(this.buffer, this.offset, count);
			this.offset += count;
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x00039BFD File Offset: 0x00037DFD
		// (set) Token: 0x060011BF RID: 4543 RVA: 0x00039C05 File Offset: 0x00037E05
		internal int Position
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00039C0E File Offset: 0x00037E0E
		internal void MinCapacity(int capacity)
		{
			if (this.buffer.Length < capacity)
			{
				this.buffer = new byte[capacity];
			}
			this.offset = 0;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00039C2E File Offset: 0x00037E2E
		internal void Align(int alignment)
		{
			while (this.offset % alignment != 0)
			{
				this.offset++;
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00039C4A File Offset: 0x00037E4A
		internal void ReadInt16(out short value)
		{
			value = (short)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8));
			this.offset += 2;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x00039C82 File Offset: 0x00037E82
		internal void ReadInt8(out sbyte value)
		{
			value = (sbyte)this.buffer[this.offset];
			this.offset++;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00039CA4 File Offset: 0x00037EA4
		internal void ReadInt32(out int value)
		{
			value = (int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8) | ((int)this.buffer[this.offset + 2] << 16) | ((int)this.buffer[this.offset + 3] << 24);
			this.offset += 4;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00039D0C File Offset: 0x00037F0C
		internal void ReadInt64(out long value)
		{
			value = (long)(((ulong)this.buffer[this.offset] & 255UL) | ((ulong)this.buffer[this.offset + 1] << 8) | ((ulong)this.buffer[this.offset + 2] << 16) | ((ulong)this.buffer[this.offset + 3] << 24) | ((ulong)this.buffer[this.offset + 4] << 32) | ((ulong)this.buffer[this.offset + 5] << 40) | ((ulong)this.buffer[this.offset + 6] << 48) | ((ulong)this.buffer[this.offset + 7] << 56));
			this.offset += 8;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00039DC9 File Offset: 0x00037FC9
		internal void ReadUInt16(out ushort value)
		{
			value = (ushort)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8));
			this.offset += 2;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00039E01 File Offset: 0x00038001
		internal void ReadUInt8(out byte value)
		{
			value = this.buffer[this.offset] & byte.MaxValue;
			this.offset++;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00039E28 File Offset: 0x00038028
		internal void ReadUInt32(out uint value)
		{
			value = (uint)((int)(this.buffer[this.offset] & byte.MaxValue) | ((int)this.buffer[this.offset + 1] << 8) | ((int)this.buffer[this.offset + 2] << 16) | ((int)this.buffer[this.offset + 3] << 24));
			this.offset += 4;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00039E90 File Offset: 0x00038090
		internal void ReadUInt64(out ulong value)
		{
			value = ((ulong)this.buffer[this.offset] & 255UL) | ((ulong)this.buffer[this.offset + 1] << 8) | ((ulong)this.buffer[this.offset + 2] << 16) | ((ulong)this.buffer[this.offset + 3] << 24) | ((ulong)this.buffer[this.offset + 4] << 32) | ((ulong)this.buffer[this.offset + 5] << 40) | ((ulong)this.buffer[this.offset + 6] << 48) | ((ulong)this.buffer[this.offset + 7] << 56);
			this.offset += 8;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00039F50 File Offset: 0x00038150
		internal void ReadInt32(int[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.ReadInt32(out values[i]);
			}
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00039F78 File Offset: 0x00038178
		internal void ReadUInt32(uint[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.ReadUInt32(out values[i]);
			}
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00039FA0 File Offset: 0x000381A0
		internal void ReadBytes(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				int num = i;
				byte[] array = this.buffer;
				int num2 = this.offset;
				this.offset = num2 + 1;
				bytes[num] = array[num2];
			}
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00039FD6 File Offset: 0x000381D6
		internal float ReadFloat()
		{
			float num = BitConverter.ToSingle(this.buffer, this.offset);
			this.offset += 4;
			return num;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00039FF7 File Offset: 0x000381F7
		internal double ReadDouble()
		{
			double num = BitConverter.ToDouble(this.buffer, this.offset);
			this.offset += 8;
			return num;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0003A018 File Offset: 0x00038218
		internal decimal ReadDecimal()
		{
			int[] array = new int[4];
			this.ReadInt32(array);
			return new decimal(array[2], array[3], array[1], array[0] < 0, (byte)((array[0] & 16711680) >> 16));
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0003A054 File Offset: 0x00038254
		internal void ReadBString(out string value)
		{
			ushort num;
			this.ReadUInt16(out num);
			value = Encoding.UTF8.GetString(this.buffer, this.offset, (int)num);
			this.offset += (int)num;
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0003A090 File Offset: 0x00038290
		internal string ReadBString(int len)
		{
			string @string = Encoding.UTF8.GetString(this.buffer, this.offset, len);
			this.offset += len;
			return @string;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0003A0B8 File Offset: 0x000382B8
		internal void ReadCString(out string value)
		{
			int num = 0;
			while (this.offset + num < this.buffer.Length && this.buffer[this.offset + num] != 0)
			{
				num++;
			}
			value = Encoding.UTF8.GetString(this.buffer, this.offset, num);
			this.offset += num + 1;
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0003A11C File Offset: 0x0003831C
		internal void SkipCString(out string value)
		{
			int num = 0;
			while (this.offset + num < this.buffer.Length && this.buffer[this.offset + num] != 0)
			{
				num++;
			}
			this.offset += num + 1;
			value = null;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0003A168 File Offset: 0x00038368
		internal void ReadGuid(out Guid guid)
		{
			uint num;
			this.ReadUInt32(out num);
			ushort num2;
			this.ReadUInt16(out num2);
			ushort num3;
			this.ReadUInt16(out num3);
			byte b;
			this.ReadUInt8(out b);
			byte b2;
			this.ReadUInt8(out b2);
			byte b3;
			this.ReadUInt8(out b3);
			byte b4;
			this.ReadUInt8(out b4);
			byte b5;
			this.ReadUInt8(out b5);
			byte b6;
			this.ReadUInt8(out b6);
			byte b7;
			this.ReadUInt8(out b7);
			byte b8;
			this.ReadUInt8(out b8);
			guid = new Guid(num, num2, num3, b, b2, b3, b4, b5, b6, b7, b8);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0003A1EC File Offset: 0x000383EC
		internal string ReadString()
		{
			int num = 0;
			while (this.offset + num < this.buffer.Length && this.buffer[this.offset + num] != 0)
			{
				num += 2;
			}
			string @string = Encoding.Unicode.GetString(this.buffer, this.offset, num);
			this.offset += num + 2;
			return @string;
		}

		// Token: 0x04000A3A RID: 2618
		private byte[] buffer;

		// Token: 0x04000A3B RID: 2619
		private int offset;
	}
}
