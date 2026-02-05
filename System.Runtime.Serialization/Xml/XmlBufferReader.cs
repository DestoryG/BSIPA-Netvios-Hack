using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000031 RID: 49
	internal class XmlBufferReader
	{
		// Token: 0x060002DB RID: 731 RVA: 0x0000F6AB File Offset: 0x0000D8AB
		public XmlBufferReader(XmlDictionaryReader reader)
		{
			this.reader = reader;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F6BA File Offset: 0x0000D8BA
		public XmlBufferReader(byte[] buffer)
		{
			this.reader = null;
			this.buffer = buffer;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000F6D0 File Offset: 0x0000D8D0
		public static XmlBufferReader Empty
		{
			get
			{
				return XmlBufferReader.empty;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000F6D7 File Offset: 0x0000D8D7
		public byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000F6DF File Offset: 0x0000D8DF
		public bool IsStreamed
		{
			get
			{
				return this.stream != null;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F6EA File Offset: 0x0000D8EA
		public void SetBuffer(Stream stream, IXmlDictionary dictionary, XmlBinaryReaderSession session)
		{
			if (this.streamBuffer == null)
			{
				this.streamBuffer = new byte[128];
			}
			this.SetBuffer(stream, this.streamBuffer, 0, 0, dictionary, session);
			this.windowOffset = 0;
			this.windowOffsetMax = this.streamBuffer.Length;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F72A File Offset: 0x0000D92A
		public void SetBuffer(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlBinaryReaderSession session)
		{
			this.SetBuffer(null, buffer, offset, count, dictionary, session);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F73A File Offset: 0x0000D93A
		private void SetBuffer(Stream stream, byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlBinaryReaderSession session)
		{
			this.stream = stream;
			this.buffer = buffer;
			this.offsetMin = offset;
			this.offset = offset;
			this.offsetMax = offset + count;
			this.dictionary = dictionary;
			this.session = session;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F774 File Offset: 0x0000D974
		public void Close()
		{
			if (this.streamBuffer != null && this.streamBuffer.Length > 4096)
			{
				this.streamBuffer = null;
			}
			if (this.stream != null)
			{
				this.stream.Close();
				this.stream = null;
			}
			this.buffer = XmlBufferReader.emptyByteArray;
			this.offset = 0;
			this.offsetMax = 0;
			this.windowOffset = 0;
			this.windowOffsetMax = 0;
			this.dictionary = null;
			this.session = null;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000F7EE File Offset: 0x0000D9EE
		public bool EndOfFile
		{
			get
			{
				return this.offset == this.offsetMax && !this.TryEnsureByte();
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F80C File Offset: 0x0000DA0C
		public byte GetByte()
		{
			int num = this.offset;
			if (num < this.offsetMax)
			{
				return this.buffer[num];
			}
			return this.GetByteHard();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F838 File Offset: 0x0000DA38
		public void SkipByte()
		{
			this.Advance(1);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F841 File Offset: 0x0000DA41
		private byte GetByteHard()
		{
			this.EnsureByte();
			return this.buffer[this.offset];
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F856 File Offset: 0x0000DA56
		public byte[] GetBuffer(int count, out int offset)
		{
			offset = this.offset;
			if (offset <= this.offsetMax - count)
			{
				return this.buffer;
			}
			return this.GetBufferHard(count, out offset);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F87C File Offset: 0x0000DA7C
		public byte[] GetBuffer(int count, out int offset, out int offsetMax)
		{
			offset = this.offset;
			if (offset <= this.offsetMax - count)
			{
				offsetMax = this.offset + count;
			}
			else
			{
				this.TryEnsureBytes(Math.Min(count, this.windowOffsetMax - offset));
				offsetMax = this.offsetMax;
			}
			return this.buffer;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000F8CD File Offset: 0x0000DACD
		public byte[] GetBuffer(out int offset, out int offsetMax)
		{
			offset = this.offset;
			offsetMax = this.offsetMax;
			return this.buffer;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F8E5 File Offset: 0x0000DAE5
		private byte[] GetBufferHard(int count, out int offset)
		{
			offset = this.offset;
			this.EnsureBytes(count);
			return this.buffer;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F8FC File Offset: 0x0000DAFC
		private void EnsureByte()
		{
			if (!this.TryEnsureByte())
			{
				XmlExceptionHelper.ThrowUnexpectedEndOfFile(this.reader);
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000F914 File Offset: 0x0000DB14
		private bool TryEnsureByte()
		{
			if (this.stream == null)
			{
				return false;
			}
			if (this.offsetMax >= this.windowOffsetMax)
			{
				XmlExceptionHelper.ThrowMaxBytesPerReadExceeded(this.reader, this.windowOffsetMax - this.windowOffset);
			}
			if (this.offsetMax >= this.buffer.Length)
			{
				return this.TryEnsureBytes(1);
			}
			int num = this.stream.ReadByte();
			if (num == -1)
			{
				return false;
			}
			byte[] array = this.buffer;
			int num2 = this.offsetMax;
			this.offsetMax = num2 + 1;
			array[num2] = (byte)num;
			return true;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000F996 File Offset: 0x0000DB96
		private void EnsureBytes(int count)
		{
			if (!this.TryEnsureBytes(count))
			{
				XmlExceptionHelper.ThrowUnexpectedEndOfFile(this.reader);
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000F9AC File Offset: 0x0000DBAC
		private bool TryEnsureBytes(int count)
		{
			if (this.stream == null)
			{
				return false;
			}
			if (this.offset > 2147483647 - count)
			{
				XmlExceptionHelper.ThrowMaxBytesPerReadExceeded(this.reader, this.windowOffsetMax - this.windowOffset);
			}
			int num = this.offset + count;
			if (num < this.offsetMax)
			{
				return true;
			}
			if (num > this.windowOffsetMax)
			{
				XmlExceptionHelper.ThrowMaxBytesPerReadExceeded(this.reader, this.windowOffsetMax - this.windowOffset);
			}
			if (num > this.buffer.Length)
			{
				byte[] array = new byte[Math.Max(num, this.buffer.Length * 2)];
				global::System.Buffer.BlockCopy(this.buffer, 0, array, 0, this.offsetMax);
				this.buffer = array;
				this.streamBuffer = array;
			}
			int num2;
			for (int i = num - this.offsetMax; i > 0; i -= num2)
			{
				num2 = this.stream.Read(this.buffer, this.offsetMax, i);
				if (num2 == 0)
				{
					return false;
				}
				this.offsetMax += num2;
			}
			return true;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FAA2 File Offset: 0x0000DCA2
		public void Advance(int count)
		{
			this.offset += count;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000FAB4 File Offset: 0x0000DCB4
		public void InsertBytes(byte[] buffer, int offset, int count)
		{
			if (this.offsetMax > buffer.Length - count)
			{
				byte[] array = new byte[this.offsetMax + count];
				global::System.Buffer.BlockCopy(this.buffer, 0, array, 0, this.offsetMax);
				this.buffer = array;
				this.streamBuffer = array;
			}
			global::System.Buffer.BlockCopy(this.buffer, this.offset, this.buffer, this.offset + count, this.offsetMax - this.offset);
			this.offsetMax += count;
			global::System.Buffer.BlockCopy(buffer, offset, this.buffer, this.offset, count);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000FB4C File Offset: 0x0000DD4C
		public void SetWindow(int windowOffset, int windowLength)
		{
			if (windowOffset > 2147483647 - windowLength)
			{
				windowLength = int.MaxValue - windowOffset;
			}
			if (this.offset != windowOffset)
			{
				global::System.Buffer.BlockCopy(this.buffer, this.offset, this.buffer, windowOffset, this.offsetMax - this.offset);
				this.offsetMax = windowOffset + (this.offsetMax - this.offset);
				this.offset = windowOffset;
			}
			this.windowOffset = windowOffset;
			this.windowOffsetMax = Math.Max(windowOffset + windowLength, this.offsetMax);
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000FBD1 File Offset: 0x0000DDD1
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x0000FBD9 File Offset: 0x0000DDD9
		public int Offset
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

		// Token: 0x060002F5 RID: 757 RVA: 0x0000FBE2 File Offset: 0x0000DDE2
		public int ReadBytes(int count)
		{
			int num = this.offset;
			if (num > this.offsetMax - count)
			{
				this.EnsureBytes(count);
			}
			this.offset += count;
			return num;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000FC0C File Offset: 0x0000DE0C
		public int ReadMultiByteUInt31()
		{
			int num = (int)this.GetByte();
			this.Advance(1);
			if ((num & 128) == 0)
			{
				return num;
			}
			num &= 127;
			int @byte = (int)this.GetByte();
			this.Advance(1);
			num |= (@byte & 127) << 7;
			if ((@byte & 128) == 0)
			{
				return num;
			}
			int byte2 = (int)this.GetByte();
			this.Advance(1);
			num |= (byte2 & 127) << 14;
			if ((byte2 & 128) == 0)
			{
				return num;
			}
			int byte3 = (int)this.GetByte();
			this.Advance(1);
			num |= (byte3 & 127) << 21;
			if ((byte3 & 128) == 0)
			{
				return num;
			}
			int byte4 = (int)this.GetByte();
			this.Advance(1);
			num |= byte4 << 28;
			if ((byte4 & 248) != 0)
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
			}
			return num;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FCCC File Offset: 0x0000DECC
		public int ReadUInt8()
		{
			int @byte = (int)this.GetByte();
			this.Advance(1);
			return @byte;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000FCDB File Offset: 0x0000DEDB
		public int ReadInt8()
		{
			return (int)((sbyte)this.ReadUInt8());
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000FCE4 File Offset: 0x0000DEE4
		public int ReadUInt16()
		{
			int num;
			byte[] array = this.GetBuffer(2, out num);
			int num2 = (int)array[num] + ((int)array[num + 1] << 8);
			this.Advance(2);
			return num2;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000FD0D File Offset: 0x0000DF0D
		public int ReadInt16()
		{
			return (int)((short)this.ReadUInt16());
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000FD18 File Offset: 0x0000DF18
		public int ReadInt32()
		{
			int num;
			byte[] array = this.GetBuffer(4, out num);
			byte b = array[num];
			byte b2 = array[num + 1];
			byte b3 = array[num + 2];
			int num2 = (int)array[num + 3];
			this.Advance(4);
			return (((num2 << 8) + (int)b3 << 8) + (int)b2 << 8) + (int)b;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000FD55 File Offset: 0x0000DF55
		public int ReadUInt31()
		{
			int num = this.ReadInt32();
			if (num < 0)
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
			}
			return num;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000FD6C File Offset: 0x0000DF6C
		public long ReadInt64()
		{
			long num = (long)((ulong)this.ReadInt32());
			return (long)(((ulong)this.ReadInt32() << 32) + (ulong)num);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000FD90 File Offset: 0x0000DF90
		[SecuritySafeCritical]
		public unsafe float ReadSingle()
		{
			int num;
			byte[] array = this.GetBuffer(4, out num);
			float num2;
			byte* ptr = (byte*)(&num2);
			*ptr = array[num];
			ptr[1] = array[num + 1];
			ptr[2] = array[num + 2];
			ptr[3] = array[num + 3];
			this.Advance(4);
			return num2;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000FDD4 File Offset: 0x0000DFD4
		[SecuritySafeCritical]
		public unsafe double ReadDouble()
		{
			int num;
			byte[] array = this.GetBuffer(8, out num);
			double num2;
			byte* ptr = (byte*)(&num2);
			*ptr = array[num];
			ptr[1] = array[num + 1];
			ptr[2] = array[num + 2];
			ptr[3] = array[num + 3];
			ptr[4] = array[num + 4];
			ptr[5] = array[num + 5];
			ptr[6] = array[num + 6];
			ptr[7] = array[num + 7];
			this.Advance(8);
			return num2;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000FE3C File Offset: 0x0000E03C
		[SecuritySafeCritical]
		public unsafe decimal ReadDecimal()
		{
			int num;
			byte[] array = this.GetBuffer(16, out num);
			byte b = array[num];
			byte b2 = array[num + 1];
			byte b3 = array[num + 2];
			int num2 = ((((int)array[num + 3] << 8) + (int)b3 << 8) + (int)b2 << 8) + (int)b;
			if ((num2 & 2130771967) == 0 && (num2 & 16711680) <= 1835008)
			{
				decimal num3;
				byte* ptr = (byte*)(&num3);
				for (int i = 0; i < 16; i++)
				{
					ptr[i] = array[num + i];
				}
				this.Advance(16);
				return num3;
			}
			XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
			return 0m;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		public UniqueId ReadUniqueId()
		{
			int num;
			UniqueId uniqueId = new UniqueId(this.GetBuffer(16, out num), num);
			this.Advance(16);
			return uniqueId;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
		public DateTime ReadDateTime()
		{
			long num = 0L;
			DateTime dateTime;
			try
			{
				num = this.ReadInt64();
				dateTime = DateTime.FromBinary(num);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(num.ToString(CultureInfo.InvariantCulture), "DateTime", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(num.ToString(CultureInfo.InvariantCulture), "DateTime", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(num.ToString(CultureInfo.InvariantCulture), "DateTime", ex3));
			}
			return dateTime;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000FFA0 File Offset: 0x0000E1A0
		public TimeSpan ReadTimeSpan()
		{
			long num = 0L;
			TimeSpan timeSpan;
			try
			{
				num = this.ReadInt64();
				timeSpan = TimeSpan.FromTicks(num);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(num.ToString(CultureInfo.InvariantCulture), "TimeSpan", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(num.ToString(CultureInfo.InvariantCulture), "TimeSpan", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(num.ToString(CultureInfo.InvariantCulture), "TimeSpan", ex3));
			}
			return timeSpan;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010048 File Offset: 0x0000E248
		public Guid ReadGuid()
		{
			int num;
			this.GetBuffer(16, out num);
			Guid guid = this.GetGuid(num);
			this.Advance(16);
			return guid;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00010070 File Offset: 0x0000E270
		public string ReadUTF8String(int length)
		{
			int num;
			this.GetBuffer(length, out num);
			char[] charBuffer = this.GetCharBuffer(length);
			int num2 = this.GetChars(num, length, charBuffer);
			string text = new string(charBuffer, 0, num2);
			this.Advance(length);
			return text;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000100A8 File Offset: 0x0000E2A8
		[SecurityCritical]
		public unsafe void UnsafeReadArray(byte* dst, byte* dstMax)
		{
			this.UnsafeReadArray(dst, (int)((long)(dstMax - dst)));
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000100B8 File Offset: 0x0000E2B8
		[SecurityCritical]
		private unsafe void UnsafeReadArray(byte* dst, int length)
		{
			if (this.stream != null)
			{
				while (length >= 256)
				{
					byte[] array = this.GetBuffer(256, out this.offset);
					for (int i = 0; i < 256; i++)
					{
						*(dst++) = array[this.offset + i];
					}
					this.Advance(256);
					length -= 256;
				}
			}
			if (length > 0)
			{
				fixed (byte* ptr = &this.GetBuffer(length, out this.offset)[this.offset])
				{
					byte* ptr2 = ptr;
					byte* ptr3 = dst + length;
					while (dst < ptr3)
					{
						*dst = *ptr2;
						dst++;
						ptr2++;
					}
				}
				this.Advance(length);
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00010161 File Offset: 0x0000E361
		private char[] GetCharBuffer(int count)
		{
			if (count > 1024)
			{
				return new char[count];
			}
			if (this.chars == null || this.chars.Length < count)
			{
				this.chars = new char[count];
			}
			return this.chars;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00010198 File Offset: 0x0000E398
		private int GetChars(int offset, int length, char[] chars)
		{
			byte[] array = this.buffer;
			for (int i = 0; i < length; i++)
			{
				byte b = array[offset + i];
				if (b >= 128)
				{
					return i + XmlConverter.ToChars(array, offset + i, length - i, chars, i);
				}
				chars[i] = (char)b;
			}
			return length;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000101DC File Offset: 0x0000E3DC
		private int GetChars(int offset, int length, char[] chars, int charOffset)
		{
			byte[] array = this.buffer;
			for (int i = 0; i < length; i++)
			{
				byte b = array[offset + i];
				if (b >= 128)
				{
					return i + XmlConverter.ToChars(array, offset + i, length - i, chars, charOffset + i);
				}
				chars[charOffset + i] = (char)b;
			}
			return length;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00010228 File Offset: 0x0000E428
		public string GetString(int offset, int length)
		{
			char[] charBuffer = this.GetCharBuffer(length);
			int num = this.GetChars(offset, length, charBuffer);
			return new string(charBuffer, 0, num);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0001024F File Offset: 0x0000E44F
		public string GetUnicodeString(int offset, int length)
		{
			return XmlConverter.ToStringUnicode(this.buffer, offset, length);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00010260 File Offset: 0x0000E460
		public string GetString(int offset, int length, XmlNameTable nameTable)
		{
			char[] charBuffer = this.GetCharBuffer(length);
			int num = this.GetChars(offset, length, charBuffer);
			return nameTable.Add(charBuffer, 0, num);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00010288 File Offset: 0x0000E488
		public int GetEscapedChars(int offset, int length, char[] chars)
		{
			byte[] array = this.buffer;
			int num = 0;
			int num2 = offset;
			int num3 = offset + length;
			for (;;)
			{
				if (offset >= num3 || !this.IsAttrChar((int)array[offset]))
				{
					num += this.GetChars(num2, offset - num2, chars, num);
					if (offset == num3)
					{
						break;
					}
					num2 = offset;
					if (array[offset] == 38)
					{
						while (offset < num3 && array[offset] != 59)
						{
							offset++;
						}
						offset++;
						int charEntity = this.GetCharEntity(num2, offset - num2);
						num2 = offset;
						if (charEntity > 65535)
						{
							SurrogateChar surrogateChar = new SurrogateChar(charEntity);
							chars[num++] = surrogateChar.HighChar;
							chars[num++] = surrogateChar.LowChar;
						}
						else
						{
							chars[num++] = (char)charEntity;
						}
					}
					else if (array[offset] == 10 || array[offset] == 9)
					{
						chars[num++] = ' ';
						offset++;
						num2 = offset;
					}
					else
					{
						chars[num++] = ' ';
						offset++;
						if (offset < num3 && array[offset] == 10)
						{
							offset++;
						}
						num2 = offset;
					}
				}
				else
				{
					offset++;
				}
			}
			return num;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00010389 File Offset: 0x0000E589
		private bool IsAttrChar(int ch)
		{
			return ch - 9 > 1 && ch != 13 && ch != 38;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x000103A0 File Offset: 0x0000E5A0
		public string GetEscapedString(int offset, int length)
		{
			char[] charBuffer = this.GetCharBuffer(length);
			int escapedChars = this.GetEscapedChars(offset, length, charBuffer);
			return new string(charBuffer, 0, escapedChars);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000103C8 File Offset: 0x0000E5C8
		public string GetEscapedString(int offset, int length, XmlNameTable nameTable)
		{
			char[] charBuffer = this.GetCharBuffer(length);
			int escapedChars = this.GetEscapedChars(offset, length, charBuffer);
			return nameTable.Add(charBuffer, 0, escapedChars);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000103F0 File Offset: 0x0000E5F0
		private int GetLessThanCharEntity(int offset, int length)
		{
			byte[] array = this.buffer;
			if (length != 4 || array[offset + 1] != 108 || array[offset + 2] != 116)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
			}
			return 60;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00010428 File Offset: 0x0000E628
		private int GetGreaterThanCharEntity(int offset, int length)
		{
			byte[] array = this.buffer;
			if (length != 4 || array[offset + 1] != 103 || array[offset + 2] != 116)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
			}
			return 62;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00010460 File Offset: 0x0000E660
		private int GetQuoteCharEntity(int offset, int length)
		{
			byte[] array = this.buffer;
			if (length != 6 || array[offset + 1] != 113 || array[offset + 2] != 117 || array[offset + 3] != 111 || array[offset + 4] != 116)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
			}
			return 34;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000104AC File Offset: 0x0000E6AC
		private int GetAmpersandCharEntity(int offset, int length)
		{
			byte[] array = this.buffer;
			if (length != 5 || array[offset + 1] != 97 || array[offset + 2] != 109 || array[offset + 3] != 112)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
			}
			return 38;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000104EC File Offset: 0x0000E6EC
		private int GetApostropheCharEntity(int offset, int length)
		{
			byte[] array = this.buffer;
			if (length != 6 || array[offset + 1] != 97 || array[offset + 2] != 112 || array[offset + 3] != 111 || array[offset + 4] != 115)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
			}
			return 39;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00010538 File Offset: 0x0000E738
		private int GetDecimalCharEntity(int offset, int length)
		{
			byte[] array = this.buffer;
			int num = 0;
			for (int i = 2; i < length - 1; i++)
			{
				byte b = array[offset + i];
				if (b < 48 || b > 57)
				{
					XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
				}
				num = num * 10 + (int)(b - 48);
				if (num > 1114111)
				{
					XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
				}
			}
			return num;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00010598 File Offset: 0x0000E798
		private int GetHexCharEntity(int offset, int length)
		{
			byte[] array = this.buffer;
			int num = 0;
			for (int i = 3; i < length - 1; i++)
			{
				byte b = array[offset + i];
				int num2 = 0;
				if (b >= 48 && b <= 57)
				{
					num2 = (int)(b - 48);
				}
				else if (b >= 97 && b <= 102)
				{
					num2 = (int)(10 + (b - 97));
				}
				else if (b >= 65 && b <= 70)
				{
					num2 = (int)(10 + (b - 65));
				}
				else
				{
					XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
				}
				num = num * 16 + num2;
				if (num > 1114111)
				{
					XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
				}
			}
			return num;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00010628 File Offset: 0x0000E828
		public int GetCharEntity(int offset, int length)
		{
			if (length < 3)
			{
				XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
			}
			byte[] array = this.buffer;
			byte b = array[offset + 1];
			if (b <= 97)
			{
				if (b != 35)
				{
					if (b == 97)
					{
						if (array[offset + 2] == 109)
						{
							return this.GetAmpersandCharEntity(offset, length);
						}
						return this.GetApostropheCharEntity(offset, length);
					}
				}
				else
				{
					if (array[offset + 2] == 120)
					{
						return this.GetHexCharEntity(offset, length);
					}
					return this.GetDecimalCharEntity(offset, length);
				}
			}
			else
			{
				if (b == 103)
				{
					return this.GetGreaterThanCharEntity(offset, length);
				}
				if (b == 108)
				{
					return this.GetLessThanCharEntity(offset, length);
				}
				if (b == 113)
				{
					return this.GetQuoteCharEntity(offset, length);
				}
			}
			XmlExceptionHelper.ThrowInvalidCharRef(this.reader);
			return 0;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000106D0 File Offset: 0x0000E8D0
		public bool IsWhitespaceKey(int key)
		{
			string value = this.GetDictionaryString(key).Value;
			for (int i = 0; i < value.Length; i++)
			{
				if (!XmlConverter.IsWhitespace(value[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001070C File Offset: 0x0000E90C
		public bool IsWhitespaceUTF8(int offset, int length)
		{
			byte[] array = this.buffer;
			for (int i = 0; i < length; i++)
			{
				if (!XmlConverter.IsWhitespace((char)array[offset + i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0001073C File Offset: 0x0000E93C
		public bool IsWhitespaceUnicode(int offset, int length)
		{
			byte[] array = this.buffer;
			for (int i = 0; i < length; i += 2)
			{
				if (!XmlConverter.IsWhitespace((char)this.GetInt16(offset + i)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00010770 File Offset: 0x0000E970
		public bool Equals2(int key1, int key2, XmlBufferReader bufferReader2)
		{
			return key1 == key2 || this.GetDictionaryString(key1).Value == bufferReader2.GetDictionaryString(key2).Value;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00010795 File Offset: 0x0000E995
		public bool Equals2(int key1, XmlDictionaryString xmlString2)
		{
			if ((key1 & 1) == 0 && xmlString2.Dictionary == this.dictionary)
			{
				return xmlString2.Key == key1 >> 1;
			}
			return this.GetDictionaryString(key1).Value == xmlString2.Value;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000107D0 File Offset: 0x0000E9D0
		public bool Equals2(int offset1, int length1, byte[] buffer2)
		{
			int num = buffer2.Length;
			if (length1 != num)
			{
				return false;
			}
			byte[] array = this.buffer;
			for (int i = 0; i < length1; i++)
			{
				if (array[offset1 + i] != buffer2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00010808 File Offset: 0x0000EA08
		public bool Equals2(int offset1, int length1, XmlBufferReader bufferReader2, int offset2, int length2)
		{
			if (length1 != length2)
			{
				return false;
			}
			byte[] array = this.buffer;
			byte[] array2 = bufferReader2.buffer;
			for (int i = 0; i < length1; i++)
			{
				if (array[offset1 + i] != array2[offset2 + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00010848 File Offset: 0x0000EA48
		public bool Equals2(int offset1, int length1, int offset2, int length2)
		{
			if (length1 != length2)
			{
				return false;
			}
			if (offset1 == offset2)
			{
				return true;
			}
			byte[] array = this.buffer;
			for (int i = 0; i < length1; i++)
			{
				if (array[offset1 + i] != array[offset2 + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00010884 File Offset: 0x0000EA84
		[SecuritySafeCritical]
		public unsafe bool Equals2(int offset1, int length1, string s2)
		{
			int length2 = s2.Length;
			if (length1 < length2 || length1 > length2 * 3)
			{
				return false;
			}
			byte[] array = this.buffer;
			if (length1 < 8)
			{
				int num = Math.Min(length1, length2);
				for (int i = 0; i < num; i++)
				{
					byte b = array[offset1 + i];
					if (b >= 128)
					{
						return XmlConverter.ToString(array, offset1, length1) == s2;
					}
					if (s2[i] != (char)b)
					{
						return false;
					}
				}
				return length1 == length2;
			}
			int num2 = Math.Min(length1, length2);
			fixed (byte* ptr = &array[offset1])
			{
				byte* ptr2 = ptr;
				byte* ptr3 = ptr2 + num2;
				fixed (string text = s2)
				{
					char* ptr4 = text;
					if (ptr4 != null)
					{
						ptr4 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char* ptr5 = ptr4;
					int num3 = 0;
					while (ptr2 < ptr3 && *ptr2 < 128)
					{
						num3 = (int)(*ptr2 - (byte)(*ptr5));
						if (num3 != 0)
						{
							break;
						}
						ptr2++;
						ptr5++;
					}
					if (num3 != 0)
					{
						return false;
					}
					if (ptr2 == ptr3)
					{
						return length1 == length2;
					}
				}
			}
			return XmlConverter.ToString(array, offset1, length1) == s2;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00010990 File Offset: 0x0000EB90
		public int Compare(int offset1, int length1, int offset2, int length2)
		{
			byte[] array = this.buffer;
			int num = Math.Min(length1, length2);
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)(array[offset1 + i] - array[offset2 + i]);
				if (num2 != 0)
				{
					return num2;
				}
			}
			return length1 - length2;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000109CE File Offset: 0x0000EBCE
		public byte GetByte(int offset)
		{
			return this.buffer[offset];
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000109D8 File Offset: 0x0000EBD8
		public int GetInt8(int offset)
		{
			return (int)((sbyte)this.GetByte(offset));
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000109E4 File Offset: 0x0000EBE4
		public int GetInt16(int offset)
		{
			byte[] array = this.buffer;
			return (int)((short)((int)array[offset] + ((int)array[offset + 1] << 8)));
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00010A04 File Offset: 0x0000EC04
		public int GetInt32(int offset)
		{
			byte[] array = this.buffer;
			byte b = array[offset];
			byte b2 = array[offset + 1];
			byte b3 = array[offset + 2];
			return ((((int)array[offset + 3] << 8) + (int)b3 << 8) + (int)b2 << 8) + (int)b;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00010A38 File Offset: 0x0000EC38
		public long GetInt64(int offset)
		{
			byte[] array = this.buffer;
			byte b = array[offset];
			byte b2 = array[offset + 1];
			byte b3 = array[offset + 2];
			long num = (long)((ulong)(((((int)array[offset + 3] << 8) + (int)b3 << 8) + (int)b2 << 8) + (int)b));
			b = array[offset + 4];
			b2 = array[offset + 5];
			b3 = array[offset + 6];
			return (long)(((ulong)(((((int)array[offset + 7] << 8) + (int)b3 << 8) + (int)b2 << 8) + (int)b) << 32) + (ulong)num);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00010A96 File Offset: 0x0000EC96
		public ulong GetUInt64(int offset)
		{
			return (ulong)this.GetInt64(offset);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00010AA0 File Offset: 0x0000ECA0
		[SecuritySafeCritical]
		public unsafe float GetSingle(int offset)
		{
			byte[] array = this.buffer;
			float num;
			byte* ptr = (byte*)(&num);
			*ptr = array[offset];
			ptr[1] = array[offset + 1];
			ptr[2] = array[offset + 2];
			ptr[3] = array[offset + 3];
			return num;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00010ADC File Offset: 0x0000ECDC
		[SecuritySafeCritical]
		public unsafe double GetDouble(int offset)
		{
			byte[] array = this.buffer;
			double num;
			byte* ptr = (byte*)(&num);
			*ptr = array[offset];
			ptr[1] = array[offset + 1];
			ptr[2] = array[offset + 2];
			ptr[3] = array[offset + 3];
			ptr[4] = array[offset + 4];
			ptr[5] = array[offset + 5];
			ptr[6] = array[offset + 6];
			ptr[7] = array[offset + 7];
			return num;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00010B3C File Offset: 0x0000ED3C
		[SecuritySafeCritical]
		public unsafe decimal GetDecimal(int offset)
		{
			byte[] array = this.buffer;
			byte b = array[offset];
			byte b2 = array[offset + 1];
			byte b3 = array[offset + 2];
			int num = ((((int)array[offset + 3] << 8) + (int)b3 << 8) + (int)b2 << 8) + (int)b;
			if ((num & 2130771967) == 0 && (num & 16711680) <= 1835008)
			{
				decimal num2;
				byte* ptr = (byte*)(&num2);
				for (int i = 0; i < 16; i++)
				{
					ptr[i] = array[offset + i];
				}
				return num2;
			}
			XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
			return 0m;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00010BC1 File Offset: 0x0000EDC1
		public UniqueId GetUniqueId(int offset)
		{
			return new UniqueId(this.buffer, offset);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00010BCF File Offset: 0x0000EDCF
		public Guid GetGuid(int offset)
		{
			if (this.guid == null)
			{
				this.guid = new byte[16];
			}
			global::System.Buffer.BlockCopy(this.buffer, offset, this.guid, 0, this.guid.Length);
			return new Guid(this.guid);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00010C0C File Offset: 0x0000EE0C
		public void GetBase64(int srcOffset, byte[] buffer, int dstOffset, int count)
		{
			global::System.Buffer.BlockCopy(this.buffer, srcOffset, buffer, dstOffset, count);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00010C1E File Offset: 0x0000EE1E
		public XmlBinaryNodeType GetNodeType()
		{
			return (XmlBinaryNodeType)this.GetByte();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00010C26 File Offset: 0x0000EE26
		public void SkipNodeType()
		{
			this.SkipByte();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00010C30 File Offset: 0x0000EE30
		public object[] GetList(int offset, int count)
		{
			int num = this.Offset;
			this.Offset = offset;
			object[] array2;
			try
			{
				object[] array = new object[count];
				for (int i = 0; i < count; i++)
				{
					XmlBinaryNodeType nodeType = this.GetNodeType();
					this.SkipNodeType();
					this.ReadValue(nodeType, this.listValue);
					array[i] = this.listValue.ToObject();
				}
				array2 = array;
			}
			finally
			{
				this.Offset = num;
			}
			return array2;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00010CA8 File Offset: 0x0000EEA8
		public XmlDictionaryString GetDictionaryString(int key)
		{
			IXmlDictionary xmlDictionary;
			if ((key & 1) != 0)
			{
				xmlDictionary = this.session;
			}
			else
			{
				xmlDictionary = this.dictionary;
			}
			XmlDictionaryString xmlDictionaryString;
			if (!xmlDictionary.TryLookup(key >> 1, out xmlDictionaryString))
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
			}
			return xmlDictionaryString;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		public int ReadDictionaryKey()
		{
			int num = this.ReadMultiByteUInt31();
			if ((num & 1) != 0)
			{
				if (this.session == null)
				{
					XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
				}
				int num2 = num >> 1;
				XmlDictionaryString xmlDictionaryString;
				if (!this.session.TryLookup(num2, out xmlDictionaryString))
				{
					if (num2 < 0 || num2 > 536870911)
					{
						XmlExceptionHelper.ThrowXmlDictionaryStringIDOutOfRange(this.reader);
					}
					XmlExceptionHelper.ThrowXmlDictionaryStringIDUndefinedSession(this.reader, num2);
				}
			}
			else
			{
				if (this.dictionary == null)
				{
					XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
				}
				int num3 = num >> 1;
				XmlDictionaryString xmlDictionaryString2;
				if (!this.dictionary.TryLookup(num3, out xmlDictionaryString2))
				{
					if (num3 < 0 || num3 > 536870911)
					{
						XmlExceptionHelper.ThrowXmlDictionaryStringIDOutOfRange(this.reader);
					}
					XmlExceptionHelper.ThrowXmlDictionaryStringIDUndefinedStatic(this.reader, num3);
				}
			}
			return num;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00010D94 File Offset: 0x0000EF94
		public void ReadValue(XmlBinaryNodeType nodeType, ValueHandle value)
		{
			switch (nodeType)
			{
			case XmlBinaryNodeType.MinText:
				value.SetValue(ValueHandleType.Zero);
				return;
			case XmlBinaryNodeType.ZeroTextWithEndElement:
			case XmlBinaryNodeType.OneTextWithEndElement:
			case XmlBinaryNodeType.FalseTextWithEndElement:
			case XmlBinaryNodeType.TrueTextWithEndElement:
			case XmlBinaryNodeType.Int8TextWithEndElement:
			case XmlBinaryNodeType.Int16TextWithEndElement:
			case XmlBinaryNodeType.Int32TextWithEndElement:
			case XmlBinaryNodeType.Int64TextWithEndElement:
			case XmlBinaryNodeType.FloatTextWithEndElement:
			case XmlBinaryNodeType.DoubleTextWithEndElement:
			case XmlBinaryNodeType.DecimalTextWithEndElement:
			case XmlBinaryNodeType.DateTimeTextWithEndElement:
			case XmlBinaryNodeType.Chars8TextWithEndElement:
			case XmlBinaryNodeType.Chars16TextWithEndElement:
			case XmlBinaryNodeType.Chars32TextWithEndElement:
			case XmlBinaryNodeType.Bytes8TextWithEndElement:
			case XmlBinaryNodeType.Bytes16TextWithEndElement:
			case XmlBinaryNodeType.Bytes32TextWithEndElement:
				break;
			case XmlBinaryNodeType.OneText:
				value.SetValue(ValueHandleType.One);
				return;
			case XmlBinaryNodeType.FalseText:
				value.SetValue(ValueHandleType.False);
				return;
			case XmlBinaryNodeType.TrueText:
				value.SetValue(ValueHandleType.True);
				return;
			case XmlBinaryNodeType.Int8Text:
				this.ReadValue(value, ValueHandleType.Int8, 1);
				return;
			case XmlBinaryNodeType.Int16Text:
				this.ReadValue(value, ValueHandleType.Int16, 2);
				return;
			case XmlBinaryNodeType.Int32Text:
				this.ReadValue(value, ValueHandleType.Int32, 4);
				return;
			case XmlBinaryNodeType.Int64Text:
				this.ReadValue(value, ValueHandleType.Int64, 8);
				return;
			case XmlBinaryNodeType.FloatText:
				this.ReadValue(value, ValueHandleType.Single, 4);
				return;
			case XmlBinaryNodeType.DoubleText:
				this.ReadValue(value, ValueHandleType.Double, 8);
				return;
			case XmlBinaryNodeType.DecimalText:
				this.ReadValue(value, ValueHandleType.Decimal, 16);
				return;
			case XmlBinaryNodeType.DateTimeText:
				this.ReadValue(value, ValueHandleType.DateTime, 8);
				return;
			case XmlBinaryNodeType.Chars8Text:
				this.ReadValue(value, ValueHandleType.UTF8, this.ReadUInt8());
				return;
			case XmlBinaryNodeType.Chars16Text:
				this.ReadValue(value, ValueHandleType.UTF8, this.ReadUInt16());
				return;
			case XmlBinaryNodeType.Chars32Text:
				this.ReadValue(value, ValueHandleType.UTF8, this.ReadUInt31());
				return;
			case XmlBinaryNodeType.Bytes8Text:
				this.ReadValue(value, ValueHandleType.Base64, this.ReadUInt8());
				return;
			case XmlBinaryNodeType.Bytes16Text:
				this.ReadValue(value, ValueHandleType.Base64, this.ReadUInt16());
				return;
			case XmlBinaryNodeType.Bytes32Text:
				this.ReadValue(value, ValueHandleType.Base64, this.ReadUInt31());
				return;
			case XmlBinaryNodeType.StartListText:
				this.ReadList(value);
				return;
			default:
				switch (nodeType)
				{
				case XmlBinaryNodeType.EmptyText:
					value.SetValue(ValueHandleType.Empty);
					return;
				case XmlBinaryNodeType.DictionaryText:
					value.SetDictionaryValue(this.ReadDictionaryKey());
					return;
				case XmlBinaryNodeType.UniqueIdText:
					this.ReadValue(value, ValueHandleType.UniqueId, 16);
					return;
				case XmlBinaryNodeType.TimeSpanText:
					this.ReadValue(value, ValueHandleType.TimeSpan, 8);
					return;
				case XmlBinaryNodeType.GuidText:
					this.ReadValue(value, ValueHandleType.Guid, 16);
					return;
				case XmlBinaryNodeType.UInt64Text:
					this.ReadValue(value, ValueHandleType.UInt64, 8);
					return;
				case XmlBinaryNodeType.BoolText:
					value.SetValue((this.ReadUInt8() != 0) ? ValueHandleType.True : ValueHandleType.False);
					return;
				case XmlBinaryNodeType.UnicodeChars8Text:
					this.ReadUnicodeValue(value, this.ReadUInt8());
					return;
				case XmlBinaryNodeType.UnicodeChars16Text:
					this.ReadUnicodeValue(value, this.ReadUInt16());
					return;
				case XmlBinaryNodeType.UnicodeChars32Text:
					this.ReadUnicodeValue(value, this.ReadUInt31());
					return;
				case XmlBinaryNodeType.QNameDictionaryText:
					this.ReadQName(value);
					return;
				}
				break;
			}
			XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00011018 File Offset: 0x0000F218
		private void ReadValue(ValueHandle value, ValueHandleType type, int length)
		{
			int num = this.ReadBytes(length);
			value.SetValue(type, num, length);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00011036 File Offset: 0x0000F236
		private void ReadUnicodeValue(ValueHandle value, int length)
		{
			if ((length & 1) != 0)
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
			}
			this.ReadValue(value, ValueHandleType.Unicode, length);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00011054 File Offset: 0x0000F254
		private void ReadList(ValueHandle value)
		{
			if (this.listValue == null)
			{
				this.listValue = new ValueHandle(this);
			}
			int num = 0;
			int num2 = this.Offset;
			for (;;)
			{
				XmlBinaryNodeType nodeType = this.GetNodeType();
				this.SkipNodeType();
				if (nodeType == XmlBinaryNodeType.StartListText)
				{
					XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
				}
				if (nodeType == XmlBinaryNodeType.EndListText)
				{
					break;
				}
				this.ReadValue(nodeType, this.listValue);
				num++;
			}
			value.SetValue(ValueHandleType.List, num2, num);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000110C4 File Offset: 0x0000F2C4
		public void ReadQName(ValueHandle value)
		{
			int num = this.ReadUInt8();
			if (num >= 26)
			{
				XmlExceptionHelper.ThrowInvalidBinaryFormat(this.reader);
			}
			int num2 = this.ReadDictionaryKey();
			value.SetQNameValue(num, num2);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000110F8 File Offset: 0x0000F2F8
		public int[] GetRows()
		{
			if (this.buffer == null)
			{
				return new int[1];
			}
			ArrayList arrayList = new ArrayList();
			arrayList.Add(this.offsetMin);
			for (int i = this.offsetMin; i < this.offsetMax; i++)
			{
				if (this.buffer[i] == 13 || this.buffer[i] == 10)
				{
					if (i + 1 < this.offsetMax && this.buffer[i + 1] == 10)
					{
						i++;
					}
					arrayList.Add(i + 1);
				}
			}
			return (int[])arrayList.ToArray(typeof(int));
		}

		// Token: 0x040001AC RID: 428
		private XmlDictionaryReader reader;

		// Token: 0x040001AD RID: 429
		private Stream stream;

		// Token: 0x040001AE RID: 430
		private byte[] streamBuffer;

		// Token: 0x040001AF RID: 431
		private byte[] buffer;

		// Token: 0x040001B0 RID: 432
		private int offsetMin;

		// Token: 0x040001B1 RID: 433
		private int offsetMax;

		// Token: 0x040001B2 RID: 434
		private IXmlDictionary dictionary;

		// Token: 0x040001B3 RID: 435
		private XmlBinaryReaderSession session;

		// Token: 0x040001B4 RID: 436
		private byte[] guid;

		// Token: 0x040001B5 RID: 437
		private int offset;

		// Token: 0x040001B6 RID: 438
		private const int maxBytesPerChar = 3;

		// Token: 0x040001B7 RID: 439
		private char[] chars;

		// Token: 0x040001B8 RID: 440
		private int windowOffset;

		// Token: 0x040001B9 RID: 441
		private int windowOffsetMax;

		// Token: 0x040001BA RID: 442
		private ValueHandle listValue;

		// Token: 0x040001BB RID: 443
		private static byte[] emptyByteArray = new byte[0];

		// Token: 0x040001BC RID: 444
		private static XmlBufferReader empty = new XmlBufferReader(XmlBufferReader.emptyByteArray);
	}
}
