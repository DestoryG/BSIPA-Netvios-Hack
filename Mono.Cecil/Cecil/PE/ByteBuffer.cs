using System;

namespace Mono.Cecil.PE
{
	// Token: 0x020000CF RID: 207
	internal class ByteBuffer
	{
		// Token: 0x060008BD RID: 2237 RVA: 0x0001B181 File Offset: 0x00019381
		public ByteBuffer()
		{
			this.buffer = Empty<byte>.Array;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0001B194 File Offset: 0x00019394
		public ByteBuffer(int length)
		{
			this.buffer = new byte[length];
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001B1A8 File Offset: 0x000193A8
		public ByteBuffer(byte[] buffer)
		{
			this.buffer = buffer ?? Empty<byte>.Array;
			this.length = this.buffer.Length;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001B1CE File Offset: 0x000193CE
		public void Advance(int length)
		{
			this.position += length;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0001B1E0 File Offset: 0x000193E0
		public byte ReadByte()
		{
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			return array[num];
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001B205 File Offset: 0x00019405
		public sbyte ReadSByte()
		{
			return (sbyte)this.ReadByte();
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001B210 File Offset: 0x00019410
		public byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			Buffer.BlockCopy(this.buffer, this.position, array, 0, length);
			this.position += length;
			return array;
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001B247 File Offset: 0x00019447
		public ushort ReadUInt16()
		{
			ushort num = (ushort)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8));
			this.position += 2;
			return num;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001B277 File Offset: 0x00019477
		public short ReadInt16()
		{
			return (short)this.ReadUInt16();
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001B280 File Offset: 0x00019480
		public uint ReadUInt32()
		{
			uint num = (uint)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8) | ((int)this.buffer[this.position + 2] << 16) | ((int)this.buffer[this.position + 3] << 24));
			this.position += 4;
			return num;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001B2E0 File Offset: 0x000194E0
		public int ReadInt32()
		{
			return (int)this.ReadUInt32();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001B2E8 File Offset: 0x000194E8
		public ulong ReadUInt64()
		{
			uint num = this.ReadUInt32();
			return ((ulong)this.ReadUInt32() << 32) | (ulong)num;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001B309 File Offset: 0x00019509
		public long ReadInt64()
		{
			return (long)this.ReadUInt64();
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001B314 File Offset: 0x00019514
		public uint ReadCompressedUInt32()
		{
			byte b = this.ReadByte();
			if ((b & 128) == 0)
			{
				return (uint)b;
			}
			if ((b & 64) == 0)
			{
				return (((uint)b & 4294967167U) << 8) | (uint)this.ReadByte();
			}
			return (uint)((((int)b & -193) << 24) | ((int)this.ReadByte() << 16) | ((int)this.ReadByte() << 8) | (int)this.ReadByte());
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001B370 File Offset: 0x00019570
		public int ReadCompressedInt32()
		{
			byte b = this.buffer[this.position];
			uint num = this.ReadCompressedUInt32();
			int num2 = (int)num >> 1;
			if ((num & 1U) == 0U)
			{
				return num2;
			}
			int num3 = (int)(b & 192);
			if (num3 == 0 || num3 == 64)
			{
				return num2 - 64;
			}
			if (num3 != 128)
			{
				return num2 - 268435456;
			}
			return num2 - 8192;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001B3C9 File Offset: 0x000195C9
		public float ReadSingle()
		{
			if (!BitConverter.IsLittleEndian)
			{
				byte[] array = this.ReadBytes(4);
				Array.Reverse(array);
				return BitConverter.ToSingle(array, 0);
			}
			float num = BitConverter.ToSingle(this.buffer, this.position);
			this.position += 4;
			return num;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0001B405 File Offset: 0x00019605
		public double ReadDouble()
		{
			if (!BitConverter.IsLittleEndian)
			{
				byte[] array = this.ReadBytes(8);
				Array.Reverse(array);
				return BitConverter.ToDouble(array, 0);
			}
			double num = BitConverter.ToDouble(this.buffer, this.position);
			this.position += 8;
			return num;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0001B444 File Offset: 0x00019644
		public void WriteByte(byte value)
		{
			if (this.position == this.buffer.Length)
			{
				this.Grow(1);
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = value;
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0000F9B1 File Offset: 0x0000DBB1
		public void WriteSByte(sbyte value)
		{
			this.WriteByte((byte)value);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0001B49C File Offset: 0x0001969C
		public void WriteUInt16(ushort value)
		{
			if (this.position + 2 > this.buffer.Length)
			{
				this.Grow(2);
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array2[num] = (byte)(value >> 8);
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0001B512 File Offset: 0x00019712
		public void WriteInt16(short value)
		{
			this.WriteUInt16((ushort)value);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0001B51C File Offset: 0x0001971C
		public void WriteUInt32(uint value)
		{
			if (this.position + 4 > this.buffer.Length)
			{
				this.Grow(4);
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array2[num] = (byte)(value >> 8);
			byte[] array3 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array3[num] = (byte)(value >> 16);
			byte[] array4 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array4[num] = (byte)(value >> 24);
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0001B5CC File Offset: 0x000197CC
		public void WriteInt32(int value)
		{
			this.WriteUInt32((uint)value);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0001B5D8 File Offset: 0x000197D8
		public void WriteUInt64(ulong value)
		{
			if (this.position + 8 > this.buffer.Length)
			{
				this.Grow(8);
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array2[num] = (byte)(value >> 8);
			byte[] array3 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array3[num] = (byte)(value >> 16);
			byte[] array4 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array4[num] = (byte)(value >> 24);
			byte[] array5 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array5[num] = (byte)(value >> 32);
			byte[] array6 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array6[num] = (byte)(value >> 40);
			byte[] array7 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array7[num] = (byte)(value >> 48);
			byte[] array8 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array8[num] = (byte)(value >> 56);
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0001B6FC File Offset: 0x000198FC
		public void WriteInt64(long value)
		{
			this.WriteUInt64((ulong)value);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0001B708 File Offset: 0x00019908
		public void WriteCompressedUInt32(uint value)
		{
			if (value < 128U)
			{
				this.WriteByte((byte)value);
				return;
			}
			if (value < 16384U)
			{
				this.WriteByte((byte)(128U | (value >> 8)));
				this.WriteByte((byte)(value & 255U));
				return;
			}
			this.WriteByte((byte)((value >> 24) | 192U));
			this.WriteByte((byte)((value >> 16) & 255U));
			this.WriteByte((byte)((value >> 8) & 255U));
			this.WriteByte((byte)(value & 255U));
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0001B790 File Offset: 0x00019990
		public void WriteCompressedInt32(int value)
		{
			if (value >= 0)
			{
				this.WriteCompressedUInt32((uint)((uint)value << 1));
				return;
			}
			if (value > -64)
			{
				value = 64 + value;
			}
			else if (value >= -8192)
			{
				value = 8192 + value;
			}
			else if (value >= -536870912)
			{
				value = 536870912 + value;
			}
			this.WriteCompressedUInt32((uint)((value << 1) | 1));
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0001B7E8 File Offset: 0x000199E8
		public void WriteBytes(byte[] bytes)
		{
			int num = bytes.Length;
			if (this.position + num > this.buffer.Length)
			{
				this.Grow(num);
			}
			Buffer.BlockCopy(bytes, 0, this.buffer, this.position, num);
			this.position += num;
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0001B850 File Offset: 0x00019A50
		public void WriteBytes(int length)
		{
			if (this.position + length > this.buffer.Length)
			{
				this.Grow(length);
			}
			this.position += length;
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001B8A0 File Offset: 0x00019AA0
		public void WriteBytes(ByteBuffer buffer)
		{
			if (this.position + buffer.length > this.buffer.Length)
			{
				this.Grow(buffer.length);
			}
			Buffer.BlockCopy(buffer.buffer, 0, this.buffer, this.position, buffer.length);
			this.position += buffer.length;
			if (this.position > this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0001B91C File Offset: 0x00019B1C
		public void WriteSingle(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			this.WriteBytes(bytes);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001B944 File Offset: 0x00019B44
		public void WriteDouble(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			this.WriteBytes(bytes);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001B96C File Offset: 0x00019B6C
		private void Grow(int desired)
		{
			byte[] array = this.buffer;
			int num = array.Length;
			byte[] array2 = new byte[Math.Max(num + desired, num * 2)];
			Buffer.BlockCopy(array, 0, array2, 0, num);
			this.buffer = array2;
		}

		// Token: 0x04000338 RID: 824
		internal byte[] buffer;

		// Token: 0x04000339 RID: 825
		internal int length;

		// Token: 0x0400033A RID: 826
		internal int position;
	}
}
