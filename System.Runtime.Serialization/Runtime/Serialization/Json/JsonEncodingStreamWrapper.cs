using System;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000104 RID: 260
	internal class JsonEncodingStreamWrapper : Stream
	{
		// Token: 0x06000FF2 RID: 4082 RVA: 0x00041590 File Offset: 0x0003F790
		public JsonEncodingStreamWrapper(Stream stream, Encoding encoding, bool isReader)
		{
			this.isReading = isReader;
			if (isReader)
			{
				this.InitForReading(stream, encoding);
				return;
			}
			if (encoding == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
			}
			this.InitForWriting(stream, encoding);
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x000415CD File Offset: 0x0003F7CD
		public override bool CanRead
		{
			get
			{
				return this.isReading && this.stream.CanRead;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x000415E4 File Offset: 0x0003F7E4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x000415E7 File Offset: 0x0003F7E7
		public override bool CanTimeout
		{
			get
			{
				return this.stream.CanTimeout;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x000415F4 File Offset: 0x0003F7F4
		public override bool CanWrite
		{
			get
			{
				return !this.isReading && this.stream.CanWrite;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x0004160B File Offset: 0x0003F80B
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00041618 File Offset: 0x0003F818
		// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x00041624 File Offset: 0x0003F824
		public override long Position
		{
			get
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
			}
			set
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00041630 File Offset: 0x0003F830
		// (set) Token: 0x06000FFB RID: 4091 RVA: 0x0004163D File Offset: 0x0003F83D
		public override int ReadTimeout
		{
			get
			{
				return this.stream.ReadTimeout;
			}
			set
			{
				this.stream.ReadTimeout = value;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x0004164B File Offset: 0x0003F84B
		// (set) Token: 0x06000FFD RID: 4093 RVA: 0x00041658 File Offset: 0x0003F858
		public override int WriteTimeout
		{
			get
			{
				return this.stream.WriteTimeout;
			}
			set
			{
				this.stream.WriteTimeout = value;
			}
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00041668 File Offset: 0x0003F868
		public static ArraySegment<byte> ProcessBuffer(byte[] buffer, int offset, int count, Encoding encoding)
		{
			ArraySegment<byte> arraySegment;
			try
			{
				JsonEncodingStreamWrapper.SupportedEncoding supportedEncoding = JsonEncodingStreamWrapper.GetSupportedEncoding(encoding);
				JsonEncodingStreamWrapper.SupportedEncoding supportedEncoding2;
				if (count < 2)
				{
					supportedEncoding2 = JsonEncodingStreamWrapper.SupportedEncoding.UTF8;
				}
				else
				{
					supportedEncoding2 = JsonEncodingStreamWrapper.ReadEncoding(buffer[offset], buffer[offset + 1]);
				}
				if (supportedEncoding != JsonEncodingStreamWrapper.SupportedEncoding.None && supportedEncoding != supportedEncoding2)
				{
					JsonEncodingStreamWrapper.ThrowExpectedEncodingMismatch(supportedEncoding, supportedEncoding2);
				}
				if (supportedEncoding2 == JsonEncodingStreamWrapper.SupportedEncoding.UTF8)
				{
					arraySegment = new ArraySegment<byte>(buffer, offset, count);
				}
				else
				{
					arraySegment = new ArraySegment<byte>(JsonEncodingStreamWrapper.ValidatingUTF8.GetBytes(JsonEncodingStreamWrapper.GetEncoding(supportedEncoding2).GetChars(buffer, offset, count)));
				}
			}
			catch (DecoderFallbackException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid bytes in JSON."), ex));
			}
			return arraySegment;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x000416F8 File Offset: 0x0003F8F8
		public override void Close()
		{
			this.Flush();
			base.Close();
			this.stream.Close();
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00041711 File Offset: 0x0003F911
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00041720 File Offset: 0x0003F920
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num2;
			try
			{
				if (this.byteCount == 0)
				{
					if (this.encodingCode == JsonEncodingStreamWrapper.SupportedEncoding.UTF8)
					{
						return this.stream.Read(buffer, offset, count);
					}
					this.byteOffset = 0;
					this.byteCount = this.stream.Read(this.bytes, this.byteCount, (this.chars.Length - 1) * 2);
					if (this.byteCount == 0)
					{
						return 0;
					}
					this.CleanupCharBreak();
					int num = this.encoding.GetChars(this.bytes, 0, this.byteCount, this.chars, 0);
					this.byteCount = Encoding.UTF8.GetBytes(this.chars, 0, num, this.bytes, 0);
				}
				if (this.byteCount < count)
				{
					count = this.byteCount;
				}
				Buffer.BlockCopy(this.bytes, this.byteOffset, buffer, offset, count);
				this.byteOffset += count;
				this.byteCount -= count;
				num2 = count;
			}
			catch (DecoderFallbackException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid bytes in JSON."), ex));
			}
			return num2;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00041848 File Offset: 0x0003FA48
		public override int ReadByte()
		{
			if (this.byteCount == 0 && this.encodingCode == JsonEncodingStreamWrapper.SupportedEncoding.UTF8)
			{
				return this.stream.ReadByte();
			}
			if (this.Read(this.byteBuffer, 0, 1) == 0)
			{
				return -1;
			}
			return (int)this.byteBuffer[0];
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00041880 File Offset: 0x0003FA80
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0004188C File Offset: 0x0003FA8C
		public override void SetLength(long value)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00041898 File Offset: 0x0003FA98
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.encodingCode == JsonEncodingStreamWrapper.SupportedEncoding.UTF8)
			{
				this.stream.Write(buffer, offset, count);
				return;
			}
			while (count > 0)
			{
				int num = ((this.chars.Length < count) ? this.chars.Length : count);
				int num2 = this.dec.GetChars(buffer, offset, num, this.chars, 0, false);
				this.byteCount = this.enc.GetBytes(this.chars, 0, num2, this.bytes, 0, false);
				this.stream.Write(this.bytes, 0, this.byteCount);
				offset += num;
				count -= num;
			}
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00041934 File Offset: 0x0003FB34
		public override void WriteByte(byte b)
		{
			if (this.encodingCode == JsonEncodingStreamWrapper.SupportedEncoding.UTF8)
			{
				this.stream.WriteByte(b);
				return;
			}
			this.byteBuffer[0] = b;
			this.Write(this.byteBuffer, 0, 1);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00041962 File Offset: 0x0003FB62
		private static Encoding GetEncoding(JsonEncodingStreamWrapper.SupportedEncoding e)
		{
			switch (e)
			{
			case JsonEncodingStreamWrapper.SupportedEncoding.UTF8:
				return JsonEncodingStreamWrapper.ValidatingUTF8;
			case JsonEncodingStreamWrapper.SupportedEncoding.UTF16LE:
				return JsonEncodingStreamWrapper.ValidatingUTF16;
			case JsonEncodingStreamWrapper.SupportedEncoding.UTF16BE:
				return JsonEncodingStreamWrapper.ValidatingBEUTF16;
			default:
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON Encoding is not supported.")));
			}
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0004199E File Offset: 0x0003FB9E
		private static string GetEncodingName(JsonEncodingStreamWrapper.SupportedEncoding enc)
		{
			switch (enc)
			{
			case JsonEncodingStreamWrapper.SupportedEncoding.UTF8:
				return "utf-8";
			case JsonEncodingStreamWrapper.SupportedEncoding.UTF16LE:
				return "utf-16LE";
			case JsonEncodingStreamWrapper.SupportedEncoding.UTF16BE:
				return "utf-16BE";
			default:
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON Encoding is not supported.")));
			}
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x000419DC File Offset: 0x0003FBDC
		private static JsonEncodingStreamWrapper.SupportedEncoding GetSupportedEncoding(Encoding encoding)
		{
			if (encoding == null)
			{
				return JsonEncodingStreamWrapper.SupportedEncoding.None;
			}
			if (encoding.WebName == JsonEncodingStreamWrapper.ValidatingUTF8.WebName)
			{
				return JsonEncodingStreamWrapper.SupportedEncoding.UTF8;
			}
			if (encoding.WebName == JsonEncodingStreamWrapper.ValidatingUTF16.WebName)
			{
				return JsonEncodingStreamWrapper.SupportedEncoding.UTF16LE;
			}
			if (encoding.WebName == JsonEncodingStreamWrapper.ValidatingBEUTF16.WebName)
			{
				return JsonEncodingStreamWrapper.SupportedEncoding.UTF16BE;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON Encoding is not supported.")));
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00041A4D File Offset: 0x0003FC4D
		private static JsonEncodingStreamWrapper.SupportedEncoding ReadEncoding(byte b1, byte b2)
		{
			if (b1 == 0 && b2 != 0)
			{
				return JsonEncodingStreamWrapper.SupportedEncoding.UTF16BE;
			}
			if (b1 != 0 && b2 == 0)
			{
				return JsonEncodingStreamWrapper.SupportedEncoding.UTF16LE;
			}
			if (b1 == 0 && b2 == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid bytes in JSON.")));
			}
			return JsonEncodingStreamWrapper.SupportedEncoding.UTF8;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00041A7B File Offset: 0x0003FC7B
		private static void ThrowExpectedEncodingMismatch(JsonEncodingStreamWrapper.SupportedEncoding expEnc, JsonEncodingStreamWrapper.SupportedEncoding actualEnc)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Expected encoding '{0}', got '{1}' instead.", new object[]
			{
				JsonEncodingStreamWrapper.GetEncodingName(expEnc),
				JsonEncodingStreamWrapper.GetEncodingName(actualEnc)
			})));
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00041AAC File Offset: 0x0003FCAC
		private void CleanupCharBreak()
		{
			int num = this.byteOffset + this.byteCount;
			if (this.byteCount % 2 != 0)
			{
				int num2 = this.stream.ReadByte();
				if (num2 < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected end of file in JSON.")));
				}
				this.bytes[num++] = (byte)num2;
				this.byteCount++;
			}
			int num3;
			if (this.encodingCode == JsonEncodingStreamWrapper.SupportedEncoding.UTF16LE)
			{
				num3 = (int)this.bytes[num - 2] + ((int)this.bytes[num - 1] << 8);
			}
			else
			{
				num3 = (int)this.bytes[num - 1] + ((int)this.bytes[num - 2] << 8);
			}
			if ((num3 & 56320) != 56320 && num3 >= 55296 && num3 <= 56319)
			{
				int num4 = this.stream.ReadByte();
				int num5 = this.stream.ReadByte();
				if (num5 < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected end of file in JSON.")));
				}
				this.bytes[num++] = (byte)num4;
				this.bytes[num++] = (byte)num5;
				this.byteCount += 2;
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00041BC9 File Offset: 0x0003FDC9
		private void EnsureBuffers()
		{
			this.EnsureByteBuffer();
			if (this.chars == null)
			{
				this.chars = new char[128];
			}
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00041BE9 File Offset: 0x0003FDE9
		private void EnsureByteBuffer()
		{
			if (this.bytes != null)
			{
				return;
			}
			this.bytes = new byte[512];
			this.byteOffset = 0;
			this.byteCount = 0;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00041C14 File Offset: 0x0003FE14
		private void FillBuffer(int count)
		{
			int num;
			for (count -= this.byteCount; count > 0; count -= num)
			{
				num = this.stream.Read(this.bytes, this.byteOffset + this.byteCount, count);
				if (num == 0)
				{
					break;
				}
				this.byteCount += num;
			}
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00041C68 File Offset: 0x0003FE68
		private void InitForReading(Stream inputStream, Encoding expectedEncoding)
		{
			try
			{
				this.stream = new BufferedStream(inputStream);
				JsonEncodingStreamWrapper.SupportedEncoding supportedEncoding = JsonEncodingStreamWrapper.GetSupportedEncoding(expectedEncoding);
				JsonEncodingStreamWrapper.SupportedEncoding supportedEncoding2 = this.ReadEncoding();
				if (supportedEncoding != JsonEncodingStreamWrapper.SupportedEncoding.None && supportedEncoding != supportedEncoding2)
				{
					JsonEncodingStreamWrapper.ThrowExpectedEncodingMismatch(supportedEncoding, supportedEncoding2);
				}
				if (supportedEncoding2 != JsonEncodingStreamWrapper.SupportedEncoding.UTF8)
				{
					this.EnsureBuffers();
					this.FillBuffer(254);
					this.encodingCode = supportedEncoding2;
					this.encoding = JsonEncodingStreamWrapper.GetEncoding(supportedEncoding2);
					this.CleanupCharBreak();
					int num = this.encoding.GetChars(this.bytes, this.byteOffset, this.byteCount, this.chars, 0);
					this.byteOffset = 0;
					this.byteCount = JsonEncodingStreamWrapper.ValidatingUTF8.GetBytes(this.chars, 0, num, this.bytes, 0);
				}
			}
			catch (DecoderFallbackException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid bytes in JSON."), ex));
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00041D40 File Offset: 0x0003FF40
		private void InitForWriting(Stream outputStream, Encoding writeEncoding)
		{
			this.encoding = writeEncoding;
			this.stream = new BufferedStream(outputStream);
			this.encodingCode = JsonEncodingStreamWrapper.GetSupportedEncoding(writeEncoding);
			if (this.encodingCode != JsonEncodingStreamWrapper.SupportedEncoding.UTF8)
			{
				this.EnsureBuffers();
				this.dec = JsonEncodingStreamWrapper.ValidatingUTF8.GetDecoder();
				this.enc = this.encoding.GetEncoder();
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00041D9C File Offset: 0x0003FF9C
		private JsonEncodingStreamWrapper.SupportedEncoding ReadEncoding()
		{
			int num = this.stream.ReadByte();
			int num2 = this.stream.ReadByte();
			this.EnsureByteBuffer();
			JsonEncodingStreamWrapper.SupportedEncoding supportedEncoding;
			if (num == -1)
			{
				supportedEncoding = JsonEncodingStreamWrapper.SupportedEncoding.UTF8;
				this.byteCount = 0;
			}
			else if (num2 == -1)
			{
				supportedEncoding = JsonEncodingStreamWrapper.SupportedEncoding.UTF8;
				this.bytes[0] = (byte)num;
				this.byteCount = 1;
			}
			else
			{
				supportedEncoding = JsonEncodingStreamWrapper.ReadEncoding((byte)num, (byte)num2);
				this.bytes[0] = (byte)num;
				this.bytes[1] = (byte)num2;
				this.byteCount = 2;
			}
			return supportedEncoding;
		}

		// Token: 0x040007DE RID: 2014
		private static readonly UnicodeEncoding SafeBEUTF16 = new UnicodeEncoding(true, false, false);

		// Token: 0x040007DF RID: 2015
		private static readonly UnicodeEncoding SafeUTF16 = new UnicodeEncoding(false, false, false);

		// Token: 0x040007E0 RID: 2016
		private static readonly UTF8Encoding SafeUTF8 = new UTF8Encoding(false, false);

		// Token: 0x040007E1 RID: 2017
		private static readonly UnicodeEncoding ValidatingBEUTF16 = new UnicodeEncoding(true, false, true);

		// Token: 0x040007E2 RID: 2018
		private static readonly UnicodeEncoding ValidatingUTF16 = new UnicodeEncoding(false, false, true);

		// Token: 0x040007E3 RID: 2019
		private static readonly UTF8Encoding ValidatingUTF8 = new UTF8Encoding(false, true);

		// Token: 0x040007E4 RID: 2020
		private const int BufferLength = 128;

		// Token: 0x040007E5 RID: 2021
		private byte[] byteBuffer = new byte[1];

		// Token: 0x040007E6 RID: 2022
		private int byteCount;

		// Token: 0x040007E7 RID: 2023
		private int byteOffset;

		// Token: 0x040007E8 RID: 2024
		private byte[] bytes;

		// Token: 0x040007E9 RID: 2025
		private char[] chars;

		// Token: 0x040007EA RID: 2026
		private Decoder dec;

		// Token: 0x040007EB RID: 2027
		private Encoder enc;

		// Token: 0x040007EC RID: 2028
		private Encoding encoding;

		// Token: 0x040007ED RID: 2029
		private JsonEncodingStreamWrapper.SupportedEncoding encodingCode;

		// Token: 0x040007EE RID: 2030
		private bool isReading;

		// Token: 0x040007EF RID: 2031
		private Stream stream;

		// Token: 0x0200018A RID: 394
		private enum SupportedEncoding
		{
			// Token: 0x04000A54 RID: 2644
			UTF8,
			// Token: 0x04000A55 RID: 2645
			UTF16LE,
			// Token: 0x04000A56 RID: 2646
			UTF16BE,
			// Token: 0x04000A57 RID: 2647
			None
		}
	}
}
