using System;

namespace Mono.Cecil.PE
{
	// Token: 0x02000191 RID: 401
	internal class ByteBuffer
	{
		// Token: 0x06000C9B RID: 3227 RVA: 0x0002A281 File Offset: 0x00028481
		public ByteBuffer()
		{
			this.buffer = Empty<byte>.Array;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0002A294 File Offset: 0x00028494
		public ByteBuffer(int length)
		{
			this.buffer = new byte[length];
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0002A2A8 File Offset: 0x000284A8
		public ByteBuffer(byte[] buffer)
		{
			this.buffer = buffer ?? Empty<byte>.Array;
			this.length = this.buffer.Length;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0002A2CE File Offset: 0x000284CE
		public void Advance(int length)
		{
			this.position += length;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0002A2E0 File Offset: 0x000284E0
		public byte ReadByte()
		{
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			return array[num];
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0002A305 File Offset: 0x00028505
		public sbyte ReadSByte()
		{
			return (sbyte)this.ReadByte();
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002A310 File Offset: 0x00028510
		public byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			Buffer.BlockCopy(this.buffer, this.position, array, 0, length);
			this.position += length;
			return array;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0002A347 File Offset: 0x00028547
		public ushort ReadUInt16()
		{
			ushort num = (ushort)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8));
			this.position += 2;
			return num;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0002A377 File Offset: 0x00028577
		public short ReadInt16()
		{
			return (short)this.ReadUInt16();
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0002A380 File Offset: 0x00028580
		public uint ReadUInt32()
		{
			uint num = (uint)((int)this.buffer[this.position] | ((int)this.buffer[this.position + 1] << 8) | ((int)this.buffer[this.position + 2] << 16) | ((int)this.buffer[this.position + 3] << 24));
			this.position += 4;
			return num;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0002A3E0 File Offset: 0x000285E0
		public int ReadInt32()
		{
			return (int)this.ReadUInt32();
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0002A3E8 File Offset: 0x000285E8
		public ulong ReadUInt64()
		{
			uint num = this.ReadUInt32();
			return ((ulong)this.ReadUInt32() << 32) | (ulong)num;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0002A409 File Offset: 0x00028609
		public long ReadInt64()
		{
			return (long)this.ReadUInt64();
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0002A414 File Offset: 0x00028614
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

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002A470 File Offset: 0x00028670
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

		// Token: 0x06000CAA RID: 3242 RVA: 0x0002A4C9 File Offset: 0x000286C9
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

		// Token: 0x06000CAB RID: 3243 RVA: 0x0002A505 File Offset: 0x00028705
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

		// Token: 0x06000CAC RID: 3244 RVA: 0x0002A544 File Offset: 0x00028744
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

		// Token: 0x06000CAD RID: 3245 RVA: 0x0001DF4D File Offset: 0x0001C14D
		public void WriteSByte(sbyte value)
		{
			this.WriteByte((byte)value);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0002A59C File Offset: 0x0002879C
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

		// Token: 0x06000CAF RID: 3247 RVA: 0x0002A612 File Offset: 0x00028812
		public void WriteInt16(short value)
		{
			this.WriteUInt16((ushort)value);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0002A61C File Offset: 0x0002881C
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

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0002A6CC File Offset: 0x000288CC
		public void WriteInt32(int value)
		{
			this.WriteUInt32((uint)value);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0002A6D8 File Offset: 0x000288D8
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

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0002A7FC File Offset: 0x000289FC
		public void WriteInt64(long value)
		{
			this.WriteUInt64((ulong)value);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0002A808 File Offset: 0x00028A08
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

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0002A890 File Offset: 0x00028A90
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

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0002A8E8 File Offset: 0x00028AE8
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

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0002A950 File Offset: 0x00028B50
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

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0002A9A0 File Offset: 0x00028BA0
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

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0002AA1C File Offset: 0x00028C1C
		public void WriteSingle(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			this.WriteBytes(bytes);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0002AA44 File Offset: 0x00028C44
		public void WriteDouble(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			this.WriteBytes(bytes);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0002AA6C File Offset: 0x00028C6C
		private void Grow(int desired)
		{
			byte[] array = this.buffer;
			int num = array.Length;
			byte[] array2 = new byte[Math.Max(num + desired, num * 2)];
			Buffer.BlockCopy(array, 0, array2, 0, num);
			this.buffer = array2;
		}

		// Token: 0x04000595 RID: 1429
		internal byte[] buffer;

		// Token: 0x04000596 RID: 1430
		internal int length;

		// Token: 0x04000597 RID: 1431
		internal int position;
	}
}
