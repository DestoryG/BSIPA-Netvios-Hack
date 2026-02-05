using System;
using System.Collections.Generic;
using System.IO;

namespace Google.Protobuf
{
	// Token: 0x02000004 RID: 4
	public sealed class CodedInputStream : IDisposable
	{
		// Token: 0x06000022 RID: 34 RVA: 0x0000238A File Offset: 0x0000058A
		public CodedInputStream(byte[] buffer)
			: this(null, ProtoPreconditions.CheckNotNull<byte[]>(buffer, "buffer"), 0, buffer.Length, true)
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023A4 File Offset: 0x000005A4
		public CodedInputStream(byte[] buffer, int offset, int length)
			: this(null, ProtoPreconditions.CheckNotNull<byte[]>(buffer, "buffer"), offset, offset + length, true)
		{
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset", "Offset must be within the buffer");
			}
			if (length < 0 || offset + length > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("length", "Length must be non-negative and within the buffer");
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023FE File Offset: 0x000005FE
		public CodedInputStream(Stream input)
			: this(input, false)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002408 File Offset: 0x00000608
		public CodedInputStream(Stream input, bool leaveOpen)
			: this(ProtoPreconditions.CheckNotNull<Stream>(input, "input"), new byte[4096], 0, 0, leaveOpen)
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002428 File Offset: 0x00000628
		internal CodedInputStream(Stream input, byte[] buffer, int bufferPos, int bufferSize, bool leaveOpen)
		{
			this.input = input;
			this.buffer = buffer;
			this.bufferPos = bufferPos;
			this.bufferSize = bufferSize;
			this.sizeLimit = int.MaxValue;
			this.recursionLimit = 100;
			this.leaveOpen = leaveOpen;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002480 File Offset: 0x00000680
		internal CodedInputStream(Stream input, byte[] buffer, int bufferPos, int bufferSize, int sizeLimit, int recursionLimit, bool leaveOpen)
			: this(input, buffer, bufferPos, bufferSize, leaveOpen)
		{
			if (sizeLimit <= 0)
			{
				throw new ArgumentOutOfRangeException("sizeLimit", "Size limit must be positive");
			}
			if (recursionLimit <= 0)
			{
				throw new ArgumentOutOfRangeException("recursionLimit!", "Recursion limit must be positive");
			}
			this.sizeLimit = sizeLimit;
			this.recursionLimit = recursionLimit;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000024D4 File Offset: 0x000006D4
		public static CodedInputStream CreateWithLimits(Stream input, int sizeLimit, int recursionLimit)
		{
			return new CodedInputStream(input, new byte[4096], 0, 0, sizeLimit, recursionLimit, false);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000024EB File Offset: 0x000006EB
		public long Position
		{
			get
			{
				if (this.input != null)
				{
					return this.input.Position - (long)(this.bufferSize + this.bufferSizeAfterLimit - this.bufferPos);
				}
				return (long)this.bufferPos;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002A RID: 42 RVA: 0x0000251E File Offset: 0x0000071E
		internal uint LastTag
		{
			get
			{
				return this.lastTag;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002526 File Offset: 0x00000726
		public int SizeLimit
		{
			get
			{
				return this.sizeLimit;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000252E File Offset: 0x0000072E
		public int RecursionLimit
		{
			get
			{
				return this.recursionLimit;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002536 File Offset: 0x00000736
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000253E File Offset: 0x0000073E
		internal bool DiscardUnknownFields { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002547 File Offset: 0x00000747
		// (set) Token: 0x06000030 RID: 48 RVA: 0x0000254F File Offset: 0x0000074F
		internal ExtensionRegistry ExtensionRegistry { get; set; }

		// Token: 0x06000031 RID: 49 RVA: 0x00002558 File Offset: 0x00000758
		public void Dispose()
		{
			if (!this.leaveOpen)
			{
				this.input.Dispose();
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000256D File Offset: 0x0000076D
		internal void CheckReadEndOfStreamTag()
		{
			if (this.lastTag != 0U)
			{
				throw InvalidProtocolBufferException.MoreDataAvailable();
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000257D File Offset: 0x0000077D
		internal void CheckLastTagWas(uint expectedTag)
		{
			if (this.lastTag != expectedTag)
			{
				throw InvalidProtocolBufferException.InvalidEndTag();
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002590 File Offset: 0x00000790
		public uint PeekTag()
		{
			if (this.hasNextTag)
			{
				return this.nextTag;
			}
			uint num = this.lastTag;
			this.nextTag = this.ReadTag();
			this.hasNextTag = true;
			this.lastTag = num;
			return this.nextTag;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000025D4 File Offset: 0x000007D4
		public uint ReadTag()
		{
			if (this.hasNextTag)
			{
				this.lastTag = this.nextTag;
				this.hasNextTag = false;
				return this.lastTag;
			}
			if (this.bufferPos + 2 <= this.bufferSize)
			{
				byte[] array = this.buffer;
				int num = this.bufferPos;
				this.bufferPos = num + 1;
				int num2 = array[num];
				if (num2 < 128)
				{
					this.lastTag = (uint)num2;
				}
				else
				{
					int num3 = num2 & 127;
					byte[] array2 = this.buffer;
					num = this.bufferPos;
					this.bufferPos = num + 1;
					if ((num2 = array2[num]) < 128)
					{
						num3 |= num2 << 7;
						this.lastTag = (uint)num3;
					}
					else
					{
						this.bufferPos -= 2;
						this.lastTag = this.ReadRawVarint32();
					}
				}
			}
			else
			{
				if (this.IsAtEnd)
				{
					this.lastTag = 0U;
					return 0U;
				}
				this.lastTag = this.ReadRawVarint32();
			}
			if (WireFormat.GetTagFieldNumber(this.lastTag) == 0)
			{
				throw InvalidProtocolBufferException.InvalidTag();
			}
			return this.lastTag;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000026C4 File Offset: 0x000008C4
		public void SkipLastField()
		{
			if (this.lastTag == 0U)
			{
				throw new InvalidOperationException("SkipLastField cannot be called at the end of a stream");
			}
			switch (WireFormat.GetTagWireType(this.lastTag))
			{
			case WireFormat.WireType.Varint:
				this.ReadRawVarint32();
				return;
			case WireFormat.WireType.Fixed64:
				this.ReadFixed64();
				return;
			case WireFormat.WireType.LengthDelimited:
			{
				int num = this.ReadLength();
				this.SkipRawBytes(num);
				return;
			}
			case WireFormat.WireType.StartGroup:
				this.SkipGroup(this.lastTag);
				return;
			case WireFormat.WireType.EndGroup:
				throw new InvalidProtocolBufferException("SkipLastField called on an end-group tag, indicating that the corresponding start-group was missing");
			case WireFormat.WireType.Fixed32:
				this.ReadFixed32();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002750 File Offset: 0x00000950
		internal void SkipGroup(uint startGroupTag)
		{
			this.recursionDepth++;
			if (this.recursionDepth >= this.recursionLimit)
			{
				throw InvalidProtocolBufferException.RecursionLimitExceeded();
			}
			uint num;
			for (;;)
			{
				num = this.ReadTag();
				if (num == 0U)
				{
					break;
				}
				if (WireFormat.GetTagWireType(num) == WireFormat.WireType.EndGroup)
				{
					goto IL_0043;
				}
				this.SkipLastField();
			}
			throw InvalidProtocolBufferException.TruncatedMessage();
			IL_0043:
			int tagFieldNumber = WireFormat.GetTagFieldNumber(startGroupTag);
			int tagFieldNumber2 = WireFormat.GetTagFieldNumber(num);
			if (tagFieldNumber != tagFieldNumber2)
			{
				throw new InvalidProtocolBufferException(string.Format("Mismatched end-group tag. Started with field {0}; ended with field {1}", tagFieldNumber, tagFieldNumber2));
			}
			this.recursionDepth--;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000027DC File Offset: 0x000009DC
		public double ReadDouble()
		{
			if (this.bufferPos + 8 > this.bufferSize)
			{
				return BitConverter.Int64BitsToDouble((long)this.ReadRawLittleEndian64());
			}
			if (BitConverter.IsLittleEndian)
			{
				double num = BitConverter.ToDouble(this.buffer, this.bufferPos);
				this.bufferPos += 8;
				return num;
			}
			byte[] array = new byte[]
			{
				this.buffer[this.bufferPos + 7],
				this.buffer[this.bufferPos + 6],
				this.buffer[this.bufferPos + 5],
				this.buffer[this.bufferPos + 4],
				this.buffer[this.bufferPos + 3],
				this.buffer[this.bufferPos + 2],
				this.buffer[this.bufferPos + 1],
				this.buffer[this.bufferPos]
			};
			this.bufferPos += 8;
			return BitConverter.ToDouble(array, 0);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000028D8 File Offset: 0x00000AD8
		public float ReadFloat()
		{
			if (BitConverter.IsLittleEndian && 4 <= this.bufferSize - this.bufferPos)
			{
				float num = BitConverter.ToSingle(this.buffer, this.bufferPos);
				this.bufferPos += 4;
				return num;
			}
			byte[] array = this.ReadRawBytes(4);
			if (!BitConverter.IsLittleEndian)
			{
				ByteArray.Reverse(array);
			}
			return BitConverter.ToSingle(array, 0);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002938 File Offset: 0x00000B38
		public ulong ReadUInt64()
		{
			return this.ReadRawVarint64();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002940 File Offset: 0x00000B40
		public long ReadInt64()
		{
			return (long)this.ReadRawVarint64();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002948 File Offset: 0x00000B48
		public int ReadInt32()
		{
			return (int)this.ReadRawVarint32();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002950 File Offset: 0x00000B50
		public ulong ReadFixed64()
		{
			return this.ReadRawLittleEndian64();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002958 File Offset: 0x00000B58
		public uint ReadFixed32()
		{
			return this.ReadRawLittleEndian32();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002960 File Offset: 0x00000B60
		public bool ReadBool()
		{
			return this.ReadRawVarint64() > 0UL;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000296C File Offset: 0x00000B6C
		public string ReadString()
		{
			int num = this.ReadLength();
			if (num == 0)
			{
				return "";
			}
			if (num <= this.bufferSize - this.bufferPos && num > 0)
			{
				string @string = CodedOutputStream.Utf8Encoding.GetString(this.buffer, this.bufferPos, num);
				this.bufferPos += num;
				return @string;
			}
			return CodedOutputStream.Utf8Encoding.GetString(this.ReadRawBytes(num), 0, num);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000029D8 File Offset: 0x00000BD8
		public void ReadMessage(IMessage builder)
		{
			int num = this.ReadLength();
			if (this.recursionDepth >= this.recursionLimit)
			{
				throw InvalidProtocolBufferException.RecursionLimitExceeded();
			}
			int num2 = this.PushLimit(num);
			this.recursionDepth++;
			builder.MergeFrom(this);
			this.CheckReadEndOfStreamTag();
			if (!this.ReachedLimit)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			this.recursionDepth--;
			this.PopLimit(num2);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002A48 File Offset: 0x00000C48
		public void ReadGroup(IMessage builder)
		{
			if (this.recursionDepth >= this.recursionLimit)
			{
				throw InvalidProtocolBufferException.RecursionLimitExceeded();
			}
			this.recursionDepth++;
			int tagFieldNumber = WireFormat.GetTagFieldNumber(this.lastTag);
			builder.MergeFrom(this);
			this.CheckLastTagWas(WireFormat.MakeTag(tagFieldNumber, WireFormat.WireType.EndGroup));
			this.recursionDepth--;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002AA8 File Offset: 0x00000CA8
		internal void ReadGroup(int fieldNumber, UnknownFieldSet set)
		{
			if (this.recursionDepth >= this.recursionLimit)
			{
				throw InvalidProtocolBufferException.RecursionLimitExceeded();
			}
			this.recursionDepth++;
			set.MergeGroupFrom(this);
			this.CheckLastTagWas(WireFormat.MakeTag(fieldNumber, WireFormat.WireType.EndGroup));
			this.recursionDepth--;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002AFC File Offset: 0x00000CFC
		public ByteString ReadBytes()
		{
			int num = this.ReadLength();
			if (num <= this.bufferSize - this.bufferPos && num > 0)
			{
				ByteString byteString = ByteString.CopyFrom(this.buffer, this.bufferPos, num);
				this.bufferPos += num;
				return byteString;
			}
			return ByteString.AttachBytes(this.ReadRawBytes(num));
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002B51 File Offset: 0x00000D51
		public uint ReadUInt32()
		{
			return this.ReadRawVarint32();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B59 File Offset: 0x00000D59
		public int ReadEnum()
		{
			return (int)this.ReadRawVarint32();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002B61 File Offset: 0x00000D61
		public int ReadSFixed32()
		{
			return (int)this.ReadRawLittleEndian32();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002B69 File Offset: 0x00000D69
		public long ReadSFixed64()
		{
			return (long)this.ReadRawLittleEndian64();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002B71 File Offset: 0x00000D71
		public int ReadSInt32()
		{
			return CodedInputStream.DecodeZigZag32(this.ReadRawVarint32());
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002B7E File Offset: 0x00000D7E
		public long ReadSInt64()
		{
			return CodedInputStream.DecodeZigZag64(this.ReadRawVarint64());
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002B8B File Offset: 0x00000D8B
		public int ReadLength()
		{
			return (int)this.ReadRawVarint32();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002B93 File Offset: 0x00000D93
		public bool MaybeConsumeTag(uint tag)
		{
			if (this.PeekTag() == tag)
			{
				this.hasNextTag = false;
				return true;
			}
			return false;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002BA8 File Offset: 0x00000DA8
		internal static float? ReadFloatWrapperLittleEndian(CodedInputStream input)
		{
			if (input.bufferPos + 6 > input.bufferSize)
			{
				return CodedInputStream.ReadFloatWrapperSlow(input);
			}
			int num = (int)input.buffer[input.bufferPos];
			if (num == 0)
			{
				input.bufferPos++;
				return new float?(0f);
			}
			if (num != 5 || input.buffer[input.bufferPos + 1] != 13)
			{
				return CodedInputStream.ReadFloatWrapperSlow(input);
			}
			float num2 = BitConverter.ToSingle(input.buffer, input.bufferPos + 2);
			input.bufferPos += 6;
			return new float?(num2);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002C3C File Offset: 0x00000E3C
		internal static float? ReadFloatWrapperSlow(CodedInputStream input)
		{
			int num = input.ReadLength();
			if (num == 0)
			{
				return new float?(0f);
			}
			int num2 = input.totalBytesRetired + input.bufferPos + num;
			float num3 = 0f;
			do
			{
				if (input.ReadTag() == 13U)
				{
					num3 = input.ReadFloat();
				}
				else
				{
					input.SkipLastField();
				}
			}
			while (input.totalBytesRetired + input.bufferPos < num2);
			return new float?(num3);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CA4 File Offset: 0x00000EA4
		internal static double? ReadDoubleWrapperLittleEndian(CodedInputStream input)
		{
			if (input.bufferPos + 10 > input.bufferSize)
			{
				return CodedInputStream.ReadDoubleWrapperSlow(input);
			}
			int num = (int)input.buffer[input.bufferPos];
			if (num == 0)
			{
				input.bufferPos++;
				return new double?(0.0);
			}
			if (num != 9 || input.buffer[input.bufferPos + 1] != 9)
			{
				return CodedInputStream.ReadDoubleWrapperSlow(input);
			}
			double num2 = BitConverter.ToDouble(input.buffer, input.bufferPos + 2);
			input.bufferPos += 10;
			return new double?(num2);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002D40 File Offset: 0x00000F40
		internal static double? ReadDoubleWrapperSlow(CodedInputStream input)
		{
			int num = input.ReadLength();
			if (num == 0)
			{
				return new double?(0.0);
			}
			int num2 = input.totalBytesRetired + input.bufferPos + num;
			double num3 = 0.0;
			do
			{
				if (input.ReadTag() == 9U)
				{
					num3 = input.ReadDouble();
				}
				else
				{
					input.SkipLastField();
				}
			}
			while (input.totalBytesRetired + input.bufferPos < num2);
			return new double?(num3);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002DB0 File Offset: 0x00000FB0
		internal static bool? ReadBoolWrapper(CodedInputStream input)
		{
			ulong? num = CodedInputStream.ReadUInt64Wrapper(input);
			ulong num2 = 0UL;
			return new bool?(!((num.GetValueOrDefault() == num2) & (num != null)));
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002DE4 File Offset: 0x00000FE4
		internal static uint? ReadUInt32Wrapper(CodedInputStream input)
		{
			if (input.bufferPos + 7 > input.bufferSize)
			{
				return CodedInputStream.ReadUInt32WrapperSlow(input);
			}
			int num = input.bufferPos;
			byte[] array = input.buffer;
			int num2 = input.bufferPos;
			input.bufferPos = num2 + 1;
			int num3 = array[num2];
			if (num3 == 0)
			{
				return new uint?(0U);
			}
			if (num3 >= 128)
			{
				input.bufferPos = num;
				return CodedInputStream.ReadUInt32WrapperSlow(input);
			}
			int num4 = input.bufferPos + num3;
			byte[] array2 = input.buffer;
			num2 = input.bufferPos;
			input.bufferPos = num2 + 1;
			if (array2[num2] != 8)
			{
				input.bufferPos = num;
				return CodedInputStream.ReadUInt32WrapperSlow(input);
			}
			uint num5 = input.ReadUInt32();
			if (input.bufferPos != num4)
			{
				input.bufferPos = num;
				return CodedInputStream.ReadUInt32WrapperSlow(input);
			}
			return new uint?(num5);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002EA8 File Offset: 0x000010A8
		private static uint? ReadUInt32WrapperSlow(CodedInputStream input)
		{
			int num = input.ReadLength();
			if (num == 0)
			{
				return new uint?(0U);
			}
			int num2 = input.totalBytesRetired + input.bufferPos + num;
			uint num3 = 0U;
			do
			{
				if (input.ReadTag() == 8U)
				{
					num3 = input.ReadUInt32();
				}
				else
				{
					input.SkipLastField();
				}
			}
			while (input.totalBytesRetired + input.bufferPos < num2);
			return new uint?(num3);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002F08 File Offset: 0x00001108
		internal static int? ReadInt32Wrapper(CodedInputStream input)
		{
			uint? num = CodedInputStream.ReadUInt32Wrapper(input);
			if (num == null)
			{
				return null;
			}
			return new int?((int)num.GetValueOrDefault());
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002F3C File Offset: 0x0000113C
		internal static ulong? ReadUInt64Wrapper(CodedInputStream input)
		{
			if (input.bufferPos + 12 > input.bufferSize)
			{
				return CodedInputStream.ReadUInt64WrapperSlow(input);
			}
			int num = input.bufferPos;
			byte[] array = input.buffer;
			int num2 = input.bufferPos;
			input.bufferPos = num2 + 1;
			int num3 = array[num2];
			if (num3 == 0)
			{
				return new ulong?(0UL);
			}
			if (num3 >= 128)
			{
				input.bufferPos = num;
				return CodedInputStream.ReadUInt64WrapperSlow(input);
			}
			int num4 = input.bufferPos + num3;
			byte[] array2 = input.buffer;
			num2 = input.bufferPos;
			input.bufferPos = num2 + 1;
			if (array2[num2] != 8)
			{
				input.bufferPos = num;
				return CodedInputStream.ReadUInt64WrapperSlow(input);
			}
			ulong num5 = input.ReadUInt64();
			if (input.bufferPos != num4)
			{
				input.bufferPos = num;
				return CodedInputStream.ReadUInt64WrapperSlow(input);
			}
			return new ulong?(num5);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003004 File Offset: 0x00001204
		internal static ulong? ReadUInt64WrapperSlow(CodedInputStream input)
		{
			int num = input.ReadLength();
			if (num == 0)
			{
				return new ulong?(0UL);
			}
			int num2 = input.totalBytesRetired + input.bufferPos + num;
			ulong num3 = 0UL;
			do
			{
				if (input.ReadTag() == 8U)
				{
					num3 = input.ReadUInt64();
				}
				else
				{
					input.SkipLastField();
				}
			}
			while (input.totalBytesRetired + input.bufferPos < num2);
			return new ulong?(num3);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003064 File Offset: 0x00001264
		internal static long? ReadInt64Wrapper(CodedInputStream input)
		{
			ulong? num = CodedInputStream.ReadUInt64Wrapper(input);
			if (num == null)
			{
				return null;
			}
			return new long?((long)num.GetValueOrDefault());
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003098 File Offset: 0x00001298
		private uint SlowReadRawVarint32()
		{
			int num = (int)this.ReadRawByte();
			if (num < 128)
			{
				return (uint)num;
			}
			int num2 = num & 127;
			if ((num = (int)this.ReadRawByte()) < 128)
			{
				num2 |= num << 7;
			}
			else
			{
				num2 |= (num & 127) << 7;
				if ((num = (int)this.ReadRawByte()) < 128)
				{
					num2 |= num << 14;
				}
				else
				{
					num2 |= (num & 127) << 14;
					if ((num = (int)this.ReadRawByte()) < 128)
					{
						num2 |= num << 21;
					}
					else
					{
						num2 |= (num & 127) << 21;
						num2 |= (num = (int)this.ReadRawByte()) << 28;
						if (num >= 128)
						{
							for (int i = 0; i < 5; i++)
							{
								if (this.ReadRawByte() < 128)
								{
									return (uint)num2;
								}
							}
							throw InvalidProtocolBufferException.MalformedVarint();
						}
					}
				}
			}
			return (uint)num2;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000315C File Offset: 0x0000135C
		internal uint ReadRawVarint32()
		{
			if (this.bufferPos + 5 > this.bufferSize)
			{
				return this.SlowReadRawVarint32();
			}
			byte[] array = this.buffer;
			int num = this.bufferPos;
			this.bufferPos = num + 1;
			int num2 = array[num];
			if (num2 < 128)
			{
				return (uint)num2;
			}
			int num3 = num2 & 127;
			byte[] array2 = this.buffer;
			num = this.bufferPos;
			this.bufferPos = num + 1;
			if ((num2 = array2[num]) < 128)
			{
				num3 |= num2 << 7;
			}
			else
			{
				num3 |= (num2 & 127) << 7;
				byte[] array3 = this.buffer;
				num = this.bufferPos;
				this.bufferPos = num + 1;
				if ((num2 = array3[num]) < 128)
				{
					num3 |= num2 << 14;
				}
				else
				{
					num3 |= (num2 & 127) << 14;
					byte[] array4 = this.buffer;
					num = this.bufferPos;
					this.bufferPos = num + 1;
					if ((num2 = array4[num]) < 128)
					{
						num3 |= num2 << 21;
					}
					else
					{
						num3 |= (num2 & 127) << 21;
						int num4 = num3;
						byte[] array5 = this.buffer;
						num = this.bufferPos;
						this.bufferPos = num + 1;
						num3 = num4 | ((num2 = array5[num]) << 28);
						if (num2 >= 128)
						{
							for (int i = 0; i < 5; i++)
							{
								if (this.ReadRawByte() < 128)
								{
									return (uint)num3;
								}
							}
							throw InvalidProtocolBufferException.MalformedVarint();
						}
					}
				}
			}
			return (uint)num3;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003294 File Offset: 0x00001494
		internal static uint ReadRawVarint32(Stream input)
		{
			int num = 0;
			int i;
			for (i = 0; i < 32; i += 7)
			{
				int num2 = input.ReadByte();
				if (num2 == -1)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				num |= (num2 & 127) << i;
				if ((num2 & 128) == 0)
				{
					return (uint)num;
				}
			}
			while (i < 64)
			{
				int num3 = input.ReadByte();
				if (num3 == -1)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				if ((num3 & 128) == 0)
				{
					return (uint)num;
				}
				i += 7;
			}
			throw InvalidProtocolBufferException.MalformedVarint();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003304 File Offset: 0x00001504
		internal ulong ReadRawVarint64()
		{
			if (this.bufferPos + 10 > this.bufferSize)
			{
				int num = 0;
				ulong num2 = 0UL;
				for (;;)
				{
					byte b = this.ReadRawByte();
					num2 |= (ulong)((ulong)((long)(b & 127)) << num);
					if (b < 128)
					{
						break;
					}
					num += 7;
					if (num >= 64)
					{
						goto IL_00B1;
					}
				}
				return num2;
			}
			byte[] array = this.buffer;
			int num3 = this.bufferPos;
			this.bufferPos = num3 + 1;
			ulong num4 = array[num3];
			if (num4 < 128UL)
			{
				return num4;
			}
			num4 &= 127UL;
			int num5 = 7;
			for (;;)
			{
				byte[] array2 = this.buffer;
				num3 = this.bufferPos;
				this.bufferPos = num3 + 1;
				byte b2 = array2[num3];
				num4 |= (ulong)((ulong)((long)(b2 & 127)) << num5);
				if (b2 < 128)
				{
					break;
				}
				num5 += 7;
				if (num5 >= 64)
				{
					goto Block_4;
				}
			}
			return num4;
			Block_4:
			IL_00B1:
			throw InvalidProtocolBufferException.MalformedVarint();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000033C8 File Offset: 0x000015C8
		internal uint ReadRawLittleEndian32()
		{
			if (this.bufferPos + 4 > this.bufferSize)
			{
				uint num = (uint)this.ReadRawByte();
				uint num2 = (uint)this.ReadRawByte();
				uint num3 = (uint)this.ReadRawByte();
				uint num4 = (uint)this.ReadRawByte();
				return num | (num2 << 8) | (num3 << 16) | (num4 << 24);
			}
			if (BitConverter.IsLittleEndian)
			{
				uint num5 = BitConverter.ToUInt32(this.buffer, this.bufferPos);
				this.bufferPos += 4;
				return num5;
			}
			uint num6 = (uint)this.buffer[this.bufferPos];
			uint num7 = (uint)this.buffer[this.bufferPos + 1];
			uint num8 = (uint)this.buffer[this.bufferPos + 2];
			uint num9 = (uint)this.buffer[this.bufferPos + 3];
			this.bufferPos += 4;
			return num6 | (num7 << 8) | (num8 << 16) | (num9 << 24);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003498 File Offset: 0x00001698
		internal ulong ReadRawLittleEndian64()
		{
			if (this.bufferPos + 8 > this.bufferSize)
			{
				ulong num = (ulong)this.ReadRawByte();
				ulong num2 = (ulong)this.ReadRawByte();
				ulong num3 = (ulong)this.ReadRawByte();
				ulong num4 = (ulong)this.ReadRawByte();
				ulong num5 = (ulong)this.ReadRawByte();
				ulong num6 = (ulong)this.ReadRawByte();
				ulong num7 = (ulong)this.ReadRawByte();
				ulong num8 = (ulong)this.ReadRawByte();
				return num | (num2 << 8) | (num3 << 16) | (num4 << 24) | (num5 << 32) | (num6 << 40) | (num7 << 48) | (num8 << 56);
			}
			if (BitConverter.IsLittleEndian)
			{
				ulong num9 = BitConverter.ToUInt64(this.buffer, this.bufferPos);
				this.bufferPos += 8;
				return num9;
			}
			ulong num10 = (ulong)this.buffer[this.bufferPos];
			ulong num11 = (ulong)this.buffer[this.bufferPos + 1];
			ulong num12 = (ulong)this.buffer[this.bufferPos + 2];
			ulong num13 = (ulong)this.buffer[this.bufferPos + 3];
			ulong num14 = (ulong)this.buffer[this.bufferPos + 4];
			ulong num15 = (ulong)this.buffer[this.bufferPos + 5];
			ulong num16 = (ulong)this.buffer[this.bufferPos + 6];
			ulong num17 = (ulong)this.buffer[this.bufferPos + 7];
			this.bufferPos += 8;
			return num10 | (num11 << 8) | (num12 << 16) | (num13 << 24) | (num14 << 32) | (num15 << 40) | (num16 << 48) | (num17 << 56);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000360A File Offset: 0x0000180A
		internal static int DecodeZigZag32(uint n)
		{
			return (int)((n >> 1) ^ -(int)(n & 1U));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003614 File Offset: 0x00001814
		internal static long DecodeZigZag64(ulong n)
		{
			return (long)((n >> 1) ^ -(long)(n & 1UL));
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003620 File Offset: 0x00001820
		internal int PushLimit(int byteLimit)
		{
			if (byteLimit < 0)
			{
				throw InvalidProtocolBufferException.NegativeSize();
			}
			byteLimit += this.totalBytesRetired + this.bufferPos;
			int num = this.currentLimit;
			if (byteLimit > num)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			this.currentLimit = byteLimit;
			this.RecomputeBufferSizeAfterLimit();
			return num;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003668 File Offset: 0x00001868
		private void RecomputeBufferSizeAfterLimit()
		{
			this.bufferSize += this.bufferSizeAfterLimit;
			int num = this.totalBytesRetired + this.bufferSize;
			if (num > this.currentLimit)
			{
				this.bufferSizeAfterLimit = num - this.currentLimit;
				this.bufferSize -= this.bufferSizeAfterLimit;
				return;
			}
			this.bufferSizeAfterLimit = 0;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000036C8 File Offset: 0x000018C8
		internal void PopLimit(int oldLimit)
		{
			this.currentLimit = oldLimit;
			this.RecomputeBufferSizeAfterLimit();
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000036D7 File Offset: 0x000018D7
		internal bool ReachedLimit
		{
			get
			{
				return this.currentLimit != int.MaxValue && this.totalBytesRetired + this.bufferPos >= this.currentLimit;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003700 File Offset: 0x00001900
		public bool IsAtEnd
		{
			get
			{
				return this.bufferPos == this.bufferSize && !this.RefillBuffer(false);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000371C File Offset: 0x0000191C
		private bool RefillBuffer(bool mustSucceed)
		{
			if (this.bufferPos < this.bufferSize)
			{
				throw new InvalidOperationException("RefillBuffer() called when buffer wasn't empty.");
			}
			if (this.totalBytesRetired + this.bufferSize == this.currentLimit)
			{
				if (mustSucceed)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				return false;
			}
			else
			{
				this.totalBytesRetired += this.bufferSize;
				this.bufferPos = 0;
				this.bufferSize = ((this.input == null) ? 0 : this.input.Read(this.buffer, 0, this.buffer.Length));
				if (this.bufferSize < 0)
				{
					throw new InvalidOperationException("Stream.Read returned a negative count");
				}
				if (this.bufferSize == 0)
				{
					if (mustSucceed)
					{
						throw InvalidProtocolBufferException.TruncatedMessage();
					}
					return false;
				}
				else
				{
					this.RecomputeBufferSizeAfterLimit();
					int num = this.totalBytesRetired + this.bufferSize + this.bufferSizeAfterLimit;
					if (num < 0 || num > this.sizeLimit)
					{
						throw InvalidProtocolBufferException.SizeLimitExceeded();
					}
					return true;
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003800 File Offset: 0x00001A00
		internal byte ReadRawByte()
		{
			if (this.bufferPos == this.bufferSize)
			{
				this.RefillBuffer(true);
			}
			byte[] array = this.buffer;
			int num = this.bufferPos;
			this.bufferPos = num + 1;
			return array[num];
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000383C File Offset: 0x00001A3C
		internal byte[] ReadRawBytes(int size)
		{
			if (size < 0)
			{
				throw InvalidProtocolBufferException.NegativeSize();
			}
			if (this.totalBytesRetired + this.bufferPos + size > this.currentLimit)
			{
				this.SkipRawBytes(this.currentLimit - this.totalBytesRetired - this.bufferPos);
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			if (size <= this.bufferSize - this.bufferPos)
			{
				byte[] array = new byte[size];
				ByteArray.Copy(this.buffer, this.bufferPos, array, 0, size);
				this.bufferPos += size;
				return array;
			}
			if (size < this.buffer.Length)
			{
				byte[] array2 = new byte[size];
				int num = this.bufferSize - this.bufferPos;
				ByteArray.Copy(this.buffer, this.bufferPos, array2, 0, num);
				this.bufferPos = this.bufferSize;
				this.RefillBuffer(true);
				while (size - num > this.bufferSize)
				{
					Buffer.BlockCopy(this.buffer, 0, array2, num, this.bufferSize);
					num += this.bufferSize;
					this.bufferPos = this.bufferSize;
					this.RefillBuffer(true);
				}
				ByteArray.Copy(this.buffer, 0, array2, num, size - num);
				this.bufferPos = size - num;
				return array2;
			}
			int num2 = this.bufferPos;
			int num3 = this.bufferSize;
			this.totalBytesRetired += this.bufferSize;
			this.bufferPos = 0;
			this.bufferSize = 0;
			int i = size - (num3 - num2);
			List<byte[]> list = new List<byte[]>();
			while (i > 0)
			{
				byte[] array3 = new byte[Math.Min(i, this.buffer.Length)];
				int num4;
				for (int j = 0; j < array3.Length; j += num4)
				{
					num4 = ((this.input == null) ? (-1) : this.input.Read(array3, j, array3.Length - j));
					if (num4 <= 0)
					{
						throw InvalidProtocolBufferException.TruncatedMessage();
					}
					this.totalBytesRetired += num4;
				}
				i -= array3.Length;
				list.Add(array3);
			}
			byte[] array4 = new byte[size];
			int num5 = num3 - num2;
			ByteArray.Copy(this.buffer, num2, array4, 0, num5);
			foreach (byte[] array5 in list)
			{
				Buffer.BlockCopy(array5, 0, array4, num5, array5.Length);
				num5 += array5.Length;
			}
			return array4;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003AA0 File Offset: 0x00001CA0
		private void SkipRawBytes(int size)
		{
			if (size < 0)
			{
				throw InvalidProtocolBufferException.NegativeSize();
			}
			if (this.totalBytesRetired + this.bufferPos + size > this.currentLimit)
			{
				this.SkipRawBytes(this.currentLimit - this.totalBytesRetired - this.bufferPos);
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			if (size <= this.bufferSize - this.bufferPos)
			{
				this.bufferPos += size;
				return;
			}
			int num = this.bufferSize - this.bufferPos;
			this.totalBytesRetired += this.bufferSize;
			this.bufferPos = 0;
			this.bufferSize = 0;
			if (num < size)
			{
				if (this.input == null)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				this.SkipImpl(size - num);
				this.totalBytesRetired += size - num;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003B68 File Offset: 0x00001D68
		private void SkipImpl(int amountToSkip)
		{
			if (this.input.CanSeek)
			{
				long position = this.input.Position;
				this.input.Position += (long)amountToSkip;
				if (this.input.Position != position + (long)amountToSkip)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
			}
			else
			{
				byte[] array = new byte[Math.Min(1024, amountToSkip)];
				while (amountToSkip > 0)
				{
					int num = this.input.Read(array, 0, Math.Min(array.Length, amountToSkip));
					if (num <= 0)
					{
						throw InvalidProtocolBufferException.TruncatedMessage();
					}
					amountToSkip -= num;
				}
			}
		}

		// Token: 0x04000004 RID: 4
		private readonly bool leaveOpen;

		// Token: 0x04000005 RID: 5
		private readonly byte[] buffer;

		// Token: 0x04000006 RID: 6
		private int bufferSize;

		// Token: 0x04000007 RID: 7
		private int bufferSizeAfterLimit;

		// Token: 0x04000008 RID: 8
		private int bufferPos;

		// Token: 0x04000009 RID: 9
		private readonly Stream input;

		// Token: 0x0400000A RID: 10
		private uint lastTag;

		// Token: 0x0400000B RID: 11
		private uint nextTag;

		// Token: 0x0400000C RID: 12
		private bool hasNextTag;

		// Token: 0x0400000D RID: 13
		internal const int DefaultRecursionLimit = 100;

		// Token: 0x0400000E RID: 14
		internal const int DefaultSizeLimit = 2147483647;

		// Token: 0x0400000F RID: 15
		internal const int BufferSize = 4096;

		// Token: 0x04000010 RID: 16
		private int totalBytesRetired;

		// Token: 0x04000011 RID: 17
		private int currentLimit = int.MaxValue;

		// Token: 0x04000012 RID: 18
		private int recursionDepth;

		// Token: 0x04000013 RID: 19
		private readonly int recursionLimit;

		// Token: 0x04000014 RID: 20
		private readonly int sizeLimit;
	}
}
