using System;
using System.IO;
using System.Text;

namespace Google.Protobuf
{
	// Token: 0x02000005 RID: 5
	public sealed class CodedOutputStream : IDisposable
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003BF6 File Offset: 0x00001DF6
		public static int ComputeDoubleSize(double value)
		{
			return 8;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003BF9 File Offset: 0x00001DF9
		public static int ComputeFloatSize(float value)
		{
			return 4;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003BFC File Offset: 0x00001DFC
		public static int ComputeUInt64Size(ulong value)
		{
			return CodedOutputStream.ComputeRawVarint64Size(value);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003C04 File Offset: 0x00001E04
		public static int ComputeInt64Size(long value)
		{
			return CodedOutputStream.ComputeRawVarint64Size((ulong)value);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003C0C File Offset: 0x00001E0C
		public static int ComputeInt32Size(int value)
		{
			if (value >= 0)
			{
				return CodedOutputStream.ComputeRawVarint32Size((uint)value);
			}
			return 10;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003C1B File Offset: 0x00001E1B
		public static int ComputeFixed64Size(ulong value)
		{
			return 8;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003C1E File Offset: 0x00001E1E
		public static int ComputeFixed32Size(uint value)
		{
			return 4;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003C21 File Offset: 0x00001E21
		public static int ComputeBoolSize(bool value)
		{
			return 1;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003C24 File Offset: 0x00001E24
		public static int ComputeStringSize(string value)
		{
			int byteCount = CodedOutputStream.Utf8Encoding.GetByteCount(value);
			return CodedOutputStream.ComputeLengthSize(byteCount) + byteCount;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003C45 File Offset: 0x00001E45
		public static int ComputeGroupSize(IMessage value)
		{
			return value.CalculateSize();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003C50 File Offset: 0x00001E50
		public static int ComputeMessageSize(IMessage value)
		{
			int num = value.CalculateSize();
			return CodedOutputStream.ComputeLengthSize(num) + num;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003C6C File Offset: 0x00001E6C
		public static int ComputeBytesSize(ByteString value)
		{
			return CodedOutputStream.ComputeLengthSize(value.Length) + value.Length;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003C80 File Offset: 0x00001E80
		public static int ComputeUInt32Size(uint value)
		{
			return CodedOutputStream.ComputeRawVarint32Size(value);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003C88 File Offset: 0x00001E88
		public static int ComputeEnumSize(int value)
		{
			return CodedOutputStream.ComputeInt32Size(value);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003C90 File Offset: 0x00001E90
		public static int ComputeSFixed32Size(int value)
		{
			return 4;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003C93 File Offset: 0x00001E93
		public static int ComputeSFixed64Size(long value)
		{
			return 8;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003C96 File Offset: 0x00001E96
		public static int ComputeSInt32Size(int value)
		{
			return CodedOutputStream.ComputeRawVarint32Size(CodedOutputStream.EncodeZigZag32(value));
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003CA3 File Offset: 0x00001EA3
		public static int ComputeSInt64Size(long value)
		{
			return CodedOutputStream.ComputeRawVarint64Size(CodedOutputStream.EncodeZigZag64(value));
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003CB0 File Offset: 0x00001EB0
		public static int ComputeLengthSize(int length)
		{
			return CodedOutputStream.ComputeRawVarint32Size((uint)length);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003CB8 File Offset: 0x00001EB8
		public static int ComputeRawVarint32Size(uint value)
		{
			if ((value & 4294967168U) == 0U)
			{
				return 1;
			}
			if ((value & 4294950912U) == 0U)
			{
				return 2;
			}
			if ((value & 4292870144U) == 0U)
			{
				return 3;
			}
			if ((value & 4026531840U) == 0U)
			{
				return 4;
			}
			return 5;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003CE4 File Offset: 0x00001EE4
		public static int ComputeRawVarint64Size(ulong value)
		{
			if ((value & 18446744073709551488UL) == 0UL)
			{
				return 1;
			}
			if ((value & 18446744073709535232UL) == 0UL)
			{
				return 2;
			}
			if ((value & 18446744073707454464UL) == 0UL)
			{
				return 3;
			}
			if ((value & 18446744073441116160UL) == 0UL)
			{
				return 4;
			}
			if ((value & 18446744039349813248UL) == 0UL)
			{
				return 5;
			}
			if ((value & 18446739675663040512UL) == 0UL)
			{
				return 6;
			}
			if ((value & 18446181123756130304UL) == 0UL)
			{
				return 7;
			}
			if ((value & 18374686479671623680UL) == 0UL)
			{
				return 8;
			}
			if ((value & 9223372036854775808UL) == 0UL)
			{
				return 9;
			}
			return 10;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003D6C File Offset: 0x00001F6C
		public static int ComputeTagSize(int fieldNumber)
		{
			return CodedOutputStream.ComputeRawVarint32Size(WireFormat.MakeTag(fieldNumber, WireFormat.WireType.Varint));
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003D7A File Offset: 0x00001F7A
		public CodedOutputStream(byte[] flatArray)
			: this(flatArray, 0, flatArray.Length)
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003D87 File Offset: 0x00001F87
		private CodedOutputStream(byte[] buffer, int offset, int length)
		{
			this.output = null;
			this.buffer = ProtoPreconditions.CheckNotNull<byte[]>(buffer, "buffer");
			this.position = offset;
			this.limit = offset + length;
			this.leaveOpen = true;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003DBE File Offset: 0x00001FBE
		private CodedOutputStream(Stream output, byte[] buffer, bool leaveOpen)
		{
			this.output = ProtoPreconditions.CheckNotNull<Stream>(output, "output");
			this.buffer = buffer;
			this.position = 0;
			this.limit = buffer.Length;
			this.leaveOpen = leaveOpen;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003DF5 File Offset: 0x00001FF5
		public CodedOutputStream(Stream output)
			: this(output, CodedOutputStream.DefaultBufferSize, false)
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003E04 File Offset: 0x00002004
		public CodedOutputStream(Stream output, int bufferSize)
			: this(output, new byte[bufferSize], false)
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003E14 File Offset: 0x00002014
		public CodedOutputStream(Stream output, bool leaveOpen)
			: this(output, CodedOutputStream.DefaultBufferSize, leaveOpen)
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003E23 File Offset: 0x00002023
		public CodedOutputStream(Stream output, int bufferSize, bool leaveOpen)
			: this(output, new byte[bufferSize], leaveOpen)
		{
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003E33 File Offset: 0x00002033
		public long Position
		{
			get
			{
				if (this.output != null)
				{
					return this.output.Position + (long)this.position;
				}
				return (long)this.position;
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003E58 File Offset: 0x00002058
		public void WriteDouble(double value)
		{
			this.WriteRawLittleEndian64((ulong)BitConverter.DoubleToInt64Bits(value));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003E68 File Offset: 0x00002068
		public void WriteFloat(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				ByteArray.Reverse(bytes);
			}
			if (this.limit - this.position >= 4)
			{
				byte[] array = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array[num] = bytes[0];
				byte[] array2 = this.buffer;
				num = this.position;
				this.position = num + 1;
				array2[num] = bytes[1];
				byte[] array3 = this.buffer;
				num = this.position;
				this.position = num + 1;
				array3[num] = bytes[2];
				byte[] array4 = this.buffer;
				num = this.position;
				this.position = num + 1;
				array4[num] = bytes[3];
				return;
			}
			this.WriteRawBytes(bytes, 0, 4);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003F0F File Offset: 0x0000210F
		public void WriteUInt64(ulong value)
		{
			this.WriteRawVarint64(value);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003F18 File Offset: 0x00002118
		public void WriteInt64(long value)
		{
			this.WriteRawVarint64((ulong)value);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003F21 File Offset: 0x00002121
		public void WriteInt32(int value)
		{
			if (value >= 0)
			{
				this.WriteRawVarint32((uint)value);
				return;
			}
			this.WriteRawVarint64((ulong)((long)value));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003F37 File Offset: 0x00002137
		public void WriteFixed64(ulong value)
		{
			this.WriteRawLittleEndian64(value);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003F40 File Offset: 0x00002140
		public void WriteFixed32(uint value)
		{
			this.WriteRawLittleEndian32(value);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003F49 File Offset: 0x00002149
		public void WriteBool(bool value)
		{
			this.WriteRawByte(value ? 1 : 0);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003F58 File Offset: 0x00002158
		public void WriteString(string value)
		{
			int byteCount = CodedOutputStream.Utf8Encoding.GetByteCount(value);
			this.WriteLength(byteCount);
			if (this.limit - this.position >= byteCount)
			{
				if (byteCount == value.Length)
				{
					for (int i = 0; i < byteCount; i++)
					{
						this.buffer[this.position + i] = (byte)value[i];
					}
				}
				else
				{
					CodedOutputStream.Utf8Encoding.GetBytes(value, 0, value.Length, this.buffer, this.position);
				}
				this.position += byteCount;
				return;
			}
			byte[] bytes = CodedOutputStream.Utf8Encoding.GetBytes(value);
			this.WriteRawBytes(bytes);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003FF7 File Offset: 0x000021F7
		public void WriteMessage(IMessage value)
		{
			this.WriteLength(value.CalculateSize());
			value.WriteTo(this);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000400C File Offset: 0x0000220C
		public void WriteGroup(IMessage value)
		{
			value.WriteTo(this);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004015 File Offset: 0x00002215
		public void WriteBytes(ByteString value)
		{
			this.WriteLength(value.Length);
			value.WriteRawBytesTo(this);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000402A File Offset: 0x0000222A
		public void WriteUInt32(uint value)
		{
			this.WriteRawVarint32(value);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004033 File Offset: 0x00002233
		public void WriteEnum(int value)
		{
			this.WriteInt32(value);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000403C File Offset: 0x0000223C
		public void WriteSFixed32(int value)
		{
			this.WriteRawLittleEndian32((uint)value);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004045 File Offset: 0x00002245
		public void WriteSFixed64(long value)
		{
			this.WriteRawLittleEndian64((ulong)value);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000404E File Offset: 0x0000224E
		public void WriteSInt32(int value)
		{
			this.WriteRawVarint32(CodedOutputStream.EncodeZigZag32(value));
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000405C File Offset: 0x0000225C
		public void WriteSInt64(long value)
		{
			this.WriteRawVarint64(CodedOutputStream.EncodeZigZag64(value));
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000406A File Offset: 0x0000226A
		public void WriteLength(int length)
		{
			this.WriteRawVarint32((uint)length);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004073 File Offset: 0x00002273
		public void WriteTag(int fieldNumber, WireFormat.WireType type)
		{
			this.WriteRawVarint32(WireFormat.MakeTag(fieldNumber, type));
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004082 File Offset: 0x00002282
		public void WriteTag(uint tag)
		{
			this.WriteRawVarint32(tag);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000408B File Offset: 0x0000228B
		public void WriteRawTag(byte b1)
		{
			this.WriteRawByte(b1);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004094 File Offset: 0x00002294
		public void WriteRawTag(byte b1, byte b2)
		{
			this.WriteRawByte(b1);
			this.WriteRawByte(b2);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000040A4 File Offset: 0x000022A4
		public void WriteRawTag(byte b1, byte b2, byte b3)
		{
			this.WriteRawByte(b1);
			this.WriteRawByte(b2);
			this.WriteRawByte(b3);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000040BB File Offset: 0x000022BB
		public void WriteRawTag(byte b1, byte b2, byte b3, byte b4)
		{
			this.WriteRawByte(b1);
			this.WriteRawByte(b2);
			this.WriteRawByte(b3);
			this.WriteRawByte(b4);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000040DA File Offset: 0x000022DA
		public void WriteRawTag(byte b1, byte b2, byte b3, byte b4, byte b5)
		{
			this.WriteRawByte(b1);
			this.WriteRawByte(b2);
			this.WriteRawByte(b3);
			this.WriteRawByte(b4);
			this.WriteRawByte(b5);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004104 File Offset: 0x00002304
		internal void WriteRawVarint32(uint value)
		{
			if (value < 128U && this.position < this.limit)
			{
				byte[] array = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array[num] = (byte)value;
				return;
			}
			while (value > 127U)
			{
				if (this.position >= this.limit)
				{
					break;
				}
				byte[] array2 = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array2[num] = (byte)((value & 127U) | 128U);
				value >>= 7;
			}
			while (value > 127U)
			{
				this.WriteRawByte((byte)((value & 127U) | 128U));
				value >>= 7;
			}
			if (this.position < this.limit)
			{
				byte[] array3 = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array3[num] = (byte)value;
				return;
			}
			this.WriteRawByte((byte)value);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000041CC File Offset: 0x000023CC
		internal void WriteRawVarint64(ulong value)
		{
			while (value > 127UL)
			{
				if (this.position >= this.limit)
				{
					break;
				}
				byte[] array = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array[num] = (byte)((value & 127UL) | 128UL);
				value >>= 7;
			}
			while (value > 127UL)
			{
				this.WriteRawByte((byte)((value & 127UL) | 128UL));
				value >>= 7;
			}
			if (this.position < this.limit)
			{
				byte[] array2 = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array2[num] = (byte)value;
				return;
			}
			this.WriteRawByte((byte)value);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000426C File Offset: 0x0000246C
		internal void WriteRawLittleEndian32(uint value)
		{
			if (this.position + 4 > this.limit)
			{
				this.WriteRawByte((byte)value);
				this.WriteRawByte((byte)(value >> 8));
				this.WriteRawByte((byte)(value >> 16));
				this.WriteRawByte((byte)(value >> 24));
				return;
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
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004324 File Offset: 0x00002524
		internal void WriteRawLittleEndian64(ulong value)
		{
			if (this.position + 8 > this.limit)
			{
				this.WriteRawByte((byte)value);
				this.WriteRawByte((byte)(value >> 8));
				this.WriteRawByte((byte)(value >> 16));
				this.WriteRawByte((byte)(value >> 24));
				this.WriteRawByte((byte)(value >> 32));
				this.WriteRawByte((byte)(value >> 40));
				this.WriteRawByte((byte)(value >> 48));
				this.WriteRawByte((byte)(value >> 56));
				return;
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
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000447C File Offset: 0x0000267C
		internal void WriteRawByte(byte value)
		{
			if (this.position == this.limit)
			{
				this.RefreshBuffer();
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = value;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000044B6 File Offset: 0x000026B6
		internal void WriteRawByte(uint value)
		{
			this.WriteRawByte((byte)value);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000044C0 File Offset: 0x000026C0
		internal void WriteRawBytes(byte[] value)
		{
			this.WriteRawBytes(value, 0, value.Length);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000044D0 File Offset: 0x000026D0
		internal void WriteRawBytes(byte[] value, int offset, int length)
		{
			if (this.limit - this.position >= length)
			{
				ByteArray.Copy(value, offset, this.buffer, this.position, length);
				this.position += length;
				return;
			}
			int num = this.limit - this.position;
			ByteArray.Copy(value, offset, this.buffer, this.position, num);
			offset += num;
			length -= num;
			this.position = this.limit;
			this.RefreshBuffer();
			if (length <= this.limit)
			{
				ByteArray.Copy(value, offset, this.buffer, 0, length);
				this.position = length;
				return;
			}
			this.output.Write(value, offset, length);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000457C File Offset: 0x0000277C
		internal static uint EncodeZigZag32(int n)
		{
			return (uint)((n << 1) ^ (n >> 31));
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004586 File Offset: 0x00002786
		internal static ulong EncodeZigZag64(long n)
		{
			return (ulong)((n << 1) ^ (n >> 63));
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004590 File Offset: 0x00002790
		private void RefreshBuffer()
		{
			if (this.output == null)
			{
				throw new CodedOutputStream.OutOfSpaceException();
			}
			this.output.Write(this.buffer, 0, this.position);
			this.position = 0;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000045BF File Offset: 0x000027BF
		public void Dispose()
		{
			this.Flush();
			if (!this.leaveOpen)
			{
				this.output.Dispose();
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000045DA File Offset: 0x000027DA
		public void Flush()
		{
			if (this.output != null)
			{
				this.RefreshBuffer();
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000045EA File Offset: 0x000027EA
		public void CheckNoSpaceLeft()
		{
			if (this.SpaceLeft != 0)
			{
				throw new InvalidOperationException("Did not write as much data as expected.");
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000045FF File Offset: 0x000027FF
		public int SpaceLeft
		{
			get
			{
				if (this.output == null)
				{
					return this.limit - this.position;
				}
				throw new InvalidOperationException("SpaceLeft can only be called on CodedOutputStreams that are writing to a flat array.");
			}
		}

		// Token: 0x04000017 RID: 23
		private const int LittleEndian64Size = 8;

		// Token: 0x04000018 RID: 24
		private const int LittleEndian32Size = 4;

		// Token: 0x04000019 RID: 25
		internal const int DoubleSize = 8;

		// Token: 0x0400001A RID: 26
		internal const int FloatSize = 4;

		// Token: 0x0400001B RID: 27
		internal const int BoolSize = 1;

		// Token: 0x0400001C RID: 28
		internal static readonly Encoding Utf8Encoding = Encoding.UTF8;

		// Token: 0x0400001D RID: 29
		public static readonly int DefaultBufferSize = 4096;

		// Token: 0x0400001E RID: 30
		private readonly bool leaveOpen;

		// Token: 0x0400001F RID: 31
		private readonly byte[] buffer;

		// Token: 0x04000020 RID: 32
		private readonly int limit;

		// Token: 0x04000021 RID: 33
		private int position;

		// Token: 0x04000022 RID: 34
		private readonly Stream output;

		// Token: 0x0200008C RID: 140
		public sealed class OutOfSpaceException : IOException
		{
			// Token: 0x060008B8 RID: 2232 RVA: 0x0001E942 File Offset: 0x0001CB42
			internal OutOfSpaceException()
				: base("CodedOutputStream was writing to a flat byte array and ran out of space.")
			{
			}
		}
	}
}
