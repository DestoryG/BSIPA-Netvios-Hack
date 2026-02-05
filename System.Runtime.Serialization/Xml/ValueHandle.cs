using System;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000026 RID: 38
	internal class ValueHandle
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00004BEC File Offset: 0x00002DEC
		public ValueHandle(XmlBufferReader bufferReader)
		{
			this.bufferReader = bufferReader;
			this.type = ValueHandleType.Empty;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004C02 File Offset: 0x00002E02
		private static Base64Encoding Base64Encoding
		{
			get
			{
				if (ValueHandle.base64Encoding == null)
				{
					ValueHandle.base64Encoding = new Base64Encoding();
				}
				return ValueHandle.base64Encoding;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004C1A File Offset: 0x00002E1A
		public void SetConstantValue(ValueHandleConstStringType constStringType)
		{
			this.type = ValueHandleType.ConstString;
			this.offset = (int)constStringType;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004C2B File Offset: 0x00002E2B
		public void SetValue(ValueHandleType type)
		{
			this.type = type;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004C34 File Offset: 0x00002E34
		public void SetDictionaryValue(int key)
		{
			this.SetValue(ValueHandleType.Dictionary, key, 0);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004C40 File Offset: 0x00002E40
		public void SetCharValue(int ch)
		{
			this.SetValue(ValueHandleType.Char, ch, 0);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004C4C File Offset: 0x00002E4C
		public void SetQNameValue(int prefix, int key)
		{
			this.SetValue(ValueHandleType.QName, key, prefix);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004C58 File Offset: 0x00002E58
		public void SetValue(ValueHandleType type, int offset, int length)
		{
			this.type = type;
			this.offset = offset;
			this.length = length;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004C70 File Offset: 0x00002E70
		public bool IsWhitespace()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType - ValueHandleType.True > 3)
			{
				switch (valueHandleType)
				{
				case ValueHandleType.UTF8:
					return this.bufferReader.IsWhitespaceUTF8(this.offset, this.length);
				case ValueHandleType.EscapedUTF8:
					return this.bufferReader.IsWhitespaceUTF8(this.offset, this.length);
				case ValueHandleType.Dictionary:
					return this.bufferReader.IsWhitespaceKey(this.offset);
				case ValueHandleType.Char:
				{
					int @char = this.GetChar();
					return @char <= 65535 && XmlConverter.IsWhitespace((char)@char);
				}
				case ValueHandleType.Unicode:
					return this.bufferReader.IsWhitespaceUnicode(this.offset, this.length);
				case ValueHandleType.ConstString:
					return ValueHandle.constStrings[this.offset].Length == 0;
				}
				return this.length == 0;
			}
			return false;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004D54 File Offset: 0x00002F54
		public Type ToType()
		{
			switch (this.type)
			{
			case ValueHandleType.Empty:
			case ValueHandleType.UTF8:
			case ValueHandleType.EscapedUTF8:
			case ValueHandleType.Dictionary:
			case ValueHandleType.Char:
			case ValueHandleType.Unicode:
			case ValueHandleType.QName:
			case ValueHandleType.ConstString:
				return typeof(string);
			case ValueHandleType.True:
			case ValueHandleType.False:
				return typeof(bool);
			case ValueHandleType.Zero:
			case ValueHandleType.One:
			case ValueHandleType.Int8:
			case ValueHandleType.Int16:
			case ValueHandleType.Int32:
				return typeof(int);
			case ValueHandleType.Int64:
				return typeof(long);
			case ValueHandleType.UInt64:
				return typeof(ulong);
			case ValueHandleType.Single:
				return typeof(float);
			case ValueHandleType.Double:
				return typeof(double);
			case ValueHandleType.Decimal:
				return typeof(decimal);
			case ValueHandleType.DateTime:
				return typeof(DateTime);
			case ValueHandleType.TimeSpan:
				return typeof(TimeSpan);
			case ValueHandleType.Guid:
				return typeof(Guid);
			case ValueHandleType.UniqueId:
				return typeof(UniqueId);
			case ValueHandleType.Base64:
				return typeof(byte[]);
			case ValueHandleType.List:
				return typeof(object[]);
			default:
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004E80 File Offset: 0x00003080
		public bool ToBoolean()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType == ValueHandleType.False)
			{
				return false;
			}
			if (valueHandleType == ValueHandleType.True)
			{
				return true;
			}
			if (valueHandleType == ValueHandleType.UTF8)
			{
				return XmlConverter.ToBoolean(this.bufferReader.Buffer, this.offset, this.length);
			}
			if (valueHandleType == ValueHandleType.Int8)
			{
				int @int = this.GetInt8();
				if (@int == 0)
				{
					return false;
				}
				if (@int == 1)
				{
					return true;
				}
			}
			return XmlConverter.ToBoolean(this.GetString());
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004EE4 File Offset: 0x000030E4
		public int ToInt()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType == ValueHandleType.Zero)
			{
				return 0;
			}
			if (valueHandleType == ValueHandleType.One)
			{
				return 1;
			}
			if (valueHandleType == ValueHandleType.Int8)
			{
				return this.GetInt8();
			}
			if (valueHandleType == ValueHandleType.Int16)
			{
				return this.GetInt16();
			}
			if (valueHandleType == ValueHandleType.Int32)
			{
				return this.GetInt32();
			}
			if (valueHandleType == ValueHandleType.Int64)
			{
				long @int = this.GetInt64();
				if (@int >= -2147483648L && @int <= 2147483647L)
				{
					return (int)@int;
				}
			}
			if (valueHandleType == ValueHandleType.UInt64)
			{
				ulong @uint = this.GetUInt64();
				if (@uint <= 2147483647UL)
				{
					return (int)@uint;
				}
			}
			if (valueHandleType == ValueHandleType.UTF8)
			{
				return XmlConverter.ToInt32(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToInt32(this.GetString());
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004F8C File Offset: 0x0000318C
		public long ToLong()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType == ValueHandleType.Zero)
			{
				return 0L;
			}
			if (valueHandleType == ValueHandleType.One)
			{
				return 1L;
			}
			if (valueHandleType == ValueHandleType.Int8)
			{
				return (long)this.GetInt8();
			}
			if (valueHandleType == ValueHandleType.Int16)
			{
				return (long)this.GetInt16();
			}
			if (valueHandleType == ValueHandleType.Int32)
			{
				return (long)this.GetInt32();
			}
			if (valueHandleType == ValueHandleType.Int64)
			{
				return this.GetInt64();
			}
			if (valueHandleType == ValueHandleType.UInt64)
			{
				ulong @uint = this.GetUInt64();
				if (@uint <= 9223372036854775807UL)
				{
					return (long)@uint;
				}
			}
			if (valueHandleType == ValueHandleType.UTF8)
			{
				return XmlConverter.ToInt64(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToInt64(this.GetString());
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005024 File Offset: 0x00003224
		public ulong ToULong()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType == ValueHandleType.Zero)
			{
				return 0UL;
			}
			if (valueHandleType == ValueHandleType.One)
			{
				return 1UL;
			}
			if (valueHandleType >= ValueHandleType.Int8 && valueHandleType <= ValueHandleType.Int64)
			{
				long num = this.ToLong();
				if (num >= 0L)
				{
					return (ulong)num;
				}
			}
			if (valueHandleType == ValueHandleType.UInt64)
			{
				return this.GetUInt64();
			}
			if (valueHandleType == ValueHandleType.UTF8)
			{
				return XmlConverter.ToUInt64(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToUInt64(this.GetString());
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005098 File Offset: 0x00003298
		public float ToSingle()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType == ValueHandleType.Single)
			{
				return this.GetSingle();
			}
			if (valueHandleType == ValueHandleType.Double)
			{
				double @double = this.GetDouble();
				if ((@double >= -3.4028234663852886E+38 && @double <= 3.4028234663852886E+38) || double.IsInfinity(@double) || double.IsNaN(@double))
				{
					return (float)@double;
				}
			}
			if (valueHandleType == ValueHandleType.Zero)
			{
				return 0f;
			}
			if (valueHandleType == ValueHandleType.One)
			{
				return 1f;
			}
			if (valueHandleType == ValueHandleType.Int8)
			{
				return (float)this.GetInt8();
			}
			if (valueHandleType == ValueHandleType.Int16)
			{
				return (float)this.GetInt16();
			}
			if (valueHandleType == ValueHandleType.UTF8)
			{
				return XmlConverter.ToSingle(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToSingle(this.GetString());
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005148 File Offset: 0x00003348
		public double ToDouble()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType == ValueHandleType.Double)
			{
				return this.GetDouble();
			}
			if (valueHandleType == ValueHandleType.Single)
			{
				return (double)this.GetSingle();
			}
			if (valueHandleType == ValueHandleType.Zero)
			{
				return 0.0;
			}
			if (valueHandleType == ValueHandleType.One)
			{
				return 1.0;
			}
			if (valueHandleType == ValueHandleType.Int8)
			{
				return (double)this.GetInt8();
			}
			if (valueHandleType == ValueHandleType.Int16)
			{
				return (double)this.GetInt16();
			}
			if (valueHandleType == ValueHandleType.Int32)
			{
				return (double)this.GetInt32();
			}
			if (valueHandleType == ValueHandleType.UTF8)
			{
				return XmlConverter.ToDouble(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToDouble(this.GetString());
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000051E4 File Offset: 0x000033E4
		public decimal ToDecimal()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType == ValueHandleType.Decimal)
			{
				return this.GetDecimal();
			}
			if (valueHandleType == ValueHandleType.Zero)
			{
				return 0m;
			}
			if (valueHandleType == ValueHandleType.One)
			{
				return 1m;
			}
			if (valueHandleType >= ValueHandleType.Int8 && valueHandleType <= ValueHandleType.Int64)
			{
				return this.ToLong();
			}
			if (valueHandleType == ValueHandleType.UInt64)
			{
				return this.GetUInt64();
			}
			if (valueHandleType == ValueHandleType.UTF8)
			{
				return XmlConverter.ToDecimal(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToDecimal(this.GetString());
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000526C File Offset: 0x0000346C
		public DateTime ToDateTime()
		{
			if (this.type == ValueHandleType.DateTime)
			{
				return XmlConverter.ToDateTime(this.GetInt64());
			}
			if (this.type == ValueHandleType.UTF8)
			{
				return XmlConverter.ToDateTime(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToDateTime(this.GetString());
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000052C4 File Offset: 0x000034C4
		public UniqueId ToUniqueId()
		{
			if (this.type == ValueHandleType.UniqueId)
			{
				return this.GetUniqueId();
			}
			if (this.type == ValueHandleType.UTF8)
			{
				return XmlConverter.ToUniqueId(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToUniqueId(this.GetString());
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005314 File Offset: 0x00003514
		public TimeSpan ToTimeSpan()
		{
			if (this.type == ValueHandleType.TimeSpan)
			{
				return new TimeSpan(this.GetInt64());
			}
			if (this.type == ValueHandleType.UTF8)
			{
				return XmlConverter.ToTimeSpan(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToTimeSpan(this.GetString());
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000536C File Offset: 0x0000356C
		public Guid ToGuid()
		{
			if (this.type == ValueHandleType.Guid)
			{
				return this.GetGuid();
			}
			if (this.type == ValueHandleType.UTF8)
			{
				return XmlConverter.ToGuid(this.bufferReader.Buffer, this.offset, this.length);
			}
			return XmlConverter.ToGuid(this.GetString());
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000053BC File Offset: 0x000035BC
		public override string ToString()
		{
			return this.GetString();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000053C4 File Offset: 0x000035C4
		public byte[] ToByteArray()
		{
			if (this.type == ValueHandleType.Base64)
			{
				byte[] array = new byte[this.length];
				this.GetBase64(array, 0, this.length);
				return array;
			}
			if (this.type == ValueHandleType.UTF8 && this.length % 4 == 0)
			{
				try
				{
					int num = this.length / 4 * 3;
					if (this.length > 0 && this.bufferReader.Buffer[this.offset + this.length - 1] == 61)
					{
						num--;
						if (this.bufferReader.Buffer[this.offset + this.length - 2] == 61)
						{
							num--;
						}
					}
					byte[] array2 = new byte[num];
					int bytes = ValueHandle.Base64Encoding.GetBytes(this.bufferReader.Buffer, this.offset, this.length, array2, 0);
					if (bytes != array2.Length)
					{
						byte[] array3 = new byte[bytes];
						Buffer.BlockCopy(array2, 0, array3, 0, bytes);
						array2 = array3;
					}
					return array2;
				}
				catch (FormatException)
				{
				}
			}
			byte[] bytes2;
			try
			{
				bytes2 = ValueHandle.Base64Encoding.GetBytes(XmlConverter.StripWhitespace(this.GetString()));
			}
			catch (FormatException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(ex.Message, ex.InnerException));
			}
			return bytes2;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005510 File Offset: 0x00003710
		public string GetString()
		{
			ValueHandleType valueHandleType = this.type;
			if (valueHandleType == ValueHandleType.UTF8)
			{
				return this.GetCharsText();
			}
			switch (valueHandleType)
			{
			case ValueHandleType.Empty:
				return string.Empty;
			case ValueHandleType.True:
				return "true";
			case ValueHandleType.False:
				return "false";
			case ValueHandleType.Zero:
				return "0";
			case ValueHandleType.One:
				return "1";
			case ValueHandleType.Int8:
			case ValueHandleType.Int16:
			case ValueHandleType.Int32:
				return XmlConverter.ToString(this.ToInt());
			case ValueHandleType.Int64:
				return XmlConverter.ToString(this.GetInt64());
			case ValueHandleType.UInt64:
				return XmlConverter.ToString(this.GetUInt64());
			case ValueHandleType.Single:
				return XmlConverter.ToString(this.GetSingle());
			case ValueHandleType.Double:
				return XmlConverter.ToString(this.GetDouble());
			case ValueHandleType.Decimal:
				return XmlConverter.ToString(this.GetDecimal());
			case ValueHandleType.DateTime:
				return XmlConverter.ToString(this.ToDateTime());
			case ValueHandleType.TimeSpan:
				return XmlConverter.ToString(this.ToTimeSpan());
			case ValueHandleType.Guid:
				return XmlConverter.ToString(this.ToGuid());
			case ValueHandleType.UniqueId:
				return XmlConverter.ToString(this.ToUniqueId());
			case ValueHandleType.UTF8:
				return this.GetCharsText();
			case ValueHandleType.EscapedUTF8:
				return this.GetEscapedCharsText();
			case ValueHandleType.Base64:
				return ValueHandle.Base64Encoding.GetString(this.ToByteArray());
			case ValueHandleType.Dictionary:
				return this.GetDictionaryString().Value;
			case ValueHandleType.List:
				return XmlConverter.ToString(this.ToList());
			case ValueHandleType.Char:
				return this.GetCharText();
			case ValueHandleType.Unicode:
				return this.GetUnicodeCharsText();
			case ValueHandleType.QName:
				return this.GetQNameDictionaryText();
			case ValueHandleType.ConstString:
				return ValueHandle.constStrings[this.offset];
			default:
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000569C File Offset: 0x0000389C
		public bool Equals2(string str, bool checkLower)
		{
			if (this.type != ValueHandleType.UTF8)
			{
				return this.GetString() == str;
			}
			if (this.length != str.Length)
			{
				return false;
			}
			byte[] buffer = this.bufferReader.Buffer;
			for (int i = 0; i < this.length; i++)
			{
				byte b = buffer[i + this.offset];
				if ((char)b != str[i] && (!checkLower || char.ToLowerInvariant((char)b) != str[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005718 File Offset: 0x00003918
		public void Sign(XmlSigningNodeWriter writer)
		{
			switch (this.type)
			{
			case ValueHandleType.Empty:
				return;
			case ValueHandleType.Int8:
			case ValueHandleType.Int16:
			case ValueHandleType.Int32:
				writer.WriteInt32Text(this.ToInt());
				return;
			case ValueHandleType.Int64:
				writer.WriteInt64Text(this.GetInt64());
				return;
			case ValueHandleType.UInt64:
				writer.WriteUInt64Text(this.GetUInt64());
				return;
			case ValueHandleType.Single:
				writer.WriteFloatText(this.GetSingle());
				return;
			case ValueHandleType.Double:
				writer.WriteDoubleText(this.GetDouble());
				return;
			case ValueHandleType.Decimal:
				writer.WriteDecimalText(this.GetDecimal());
				return;
			case ValueHandleType.DateTime:
				writer.WriteDateTimeText(this.ToDateTime());
				return;
			case ValueHandleType.TimeSpan:
				writer.WriteTimeSpanText(this.ToTimeSpan());
				return;
			case ValueHandleType.Guid:
				writer.WriteGuidText(this.ToGuid());
				return;
			case ValueHandleType.UniqueId:
				writer.WriteUniqueIdText(this.ToUniqueId());
				return;
			case ValueHandleType.UTF8:
				writer.WriteEscapedText(this.bufferReader.Buffer, this.offset, this.length);
				return;
			case ValueHandleType.Base64:
				writer.WriteBase64Text(this.bufferReader.Buffer, 0, this.bufferReader.Buffer, this.offset, this.length);
				return;
			}
			writer.WriteEscapedText(this.GetString());
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000585D File Offset: 0x00003A5D
		public object[] ToList()
		{
			return this.bufferReader.GetList(this.offset, this.length);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005878 File Offset: 0x00003A78
		public object ToObject()
		{
			switch (this.type)
			{
			case ValueHandleType.Empty:
			case ValueHandleType.UTF8:
			case ValueHandleType.EscapedUTF8:
			case ValueHandleType.Dictionary:
			case ValueHandleType.Char:
			case ValueHandleType.Unicode:
			case ValueHandleType.ConstString:
				return this.ToString();
			case ValueHandleType.True:
			case ValueHandleType.False:
				return this.ToBoolean();
			case ValueHandleType.Zero:
			case ValueHandleType.One:
			case ValueHandleType.Int8:
			case ValueHandleType.Int16:
			case ValueHandleType.Int32:
				return this.ToInt();
			case ValueHandleType.Int64:
				return this.ToLong();
			case ValueHandleType.UInt64:
				return this.GetUInt64();
			case ValueHandleType.Single:
				return this.ToSingle();
			case ValueHandleType.Double:
				return this.ToDouble();
			case ValueHandleType.Decimal:
				return this.ToDecimal();
			case ValueHandleType.DateTime:
				return this.ToDateTime();
			case ValueHandleType.TimeSpan:
				return this.ToTimeSpan();
			case ValueHandleType.Guid:
				return this.ToGuid();
			case ValueHandleType.UniqueId:
				return this.ToUniqueId();
			case ValueHandleType.Base64:
				return this.ToByteArray();
			case ValueHandleType.List:
				return this.ToList();
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000059A0 File Offset: 0x00003BA0
		public bool TryReadBase64(byte[] buffer, int offset, int count, out int actual)
		{
			if (this.type == ValueHandleType.Base64)
			{
				actual = Math.Min(this.length, count);
				this.GetBase64(buffer, offset, actual);
				this.offset += actual;
				this.length -= actual;
				return true;
			}
			if (this.type == ValueHandleType.UTF8 && count >= 3 && this.length % 4 == 0)
			{
				try
				{
					int num = Math.Min(count / 3 * 4, this.length);
					actual = ValueHandle.Base64Encoding.GetBytes(this.bufferReader.Buffer, this.offset, num, buffer, offset);
					this.offset += num;
					this.length -= num;
					return true;
				}
				catch (FormatException)
				{
				}
			}
			actual = 0;
			return false;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005A78 File Offset: 0x00003C78
		public bool TryReadChars(char[] chars, int offset, int count, out int actual)
		{
			if (this.type == ValueHandleType.Unicode)
			{
				return this.TryReadUnicodeChars(chars, offset, count, out actual);
			}
			if (this.type != ValueHandleType.UTF8)
			{
				actual = 0;
				return false;
			}
			int num = offset;
			int num2 = count;
			byte[] buffer = this.bufferReader.Buffer;
			int num3 = this.offset;
			int num4 = this.length;
			bool flag = false;
			for (;;)
			{
				if (num2 > 0 && num4 > 0)
				{
					byte b = buffer[num3];
					if (b < 128)
					{
						chars[num] = (char)b;
						num3++;
						num4--;
						num++;
						num2--;
						continue;
					}
				}
				if (num2 == 0 || num4 == 0 || flag)
				{
					break;
				}
				UTF8Encoding utf8Encoding = new UTF8Encoding(false, true);
				int num5;
				int num6;
				try
				{
					if (num2 >= utf8Encoding.GetMaxCharCount(num4) || num2 >= utf8Encoding.GetCharCount(buffer, num3, num4))
					{
						num5 = utf8Encoding.GetChars(buffer, num3, num4, chars, num);
						num6 = num4;
					}
					else
					{
						Decoder decoder = utf8Encoding.GetDecoder();
						num6 = Math.Min(num2, num4);
						num5 = decoder.GetChars(buffer, num3, num6, chars, num);
						while (num5 == 0)
						{
							if (num6 >= 3 && num2 < 2)
							{
								flag = true;
								break;
							}
							num5 = decoder.GetChars(buffer, num3 + num6, 1, chars, num);
							num6++;
						}
						num6 = utf8Encoding.GetByteCount(chars, num, num5);
					}
				}
				catch (FormatException ex)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateEncodingException(buffer, num3, num4, ex));
				}
				num3 += num6;
				num4 -= num6;
				num += num5;
				num2 -= num5;
			}
			this.offset = num3;
			this.length = num4;
			actual = count - num2;
			return true;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005BF8 File Offset: 0x00003DF8
		private bool TryReadUnicodeChars(char[] chars, int offset, int count, out int actual)
		{
			int num = Math.Min(count, this.length / 2);
			for (int i = 0; i < num; i++)
			{
				chars[offset + i] = (char)this.bufferReader.GetInt16(this.offset + i * 2);
			}
			this.offset += num * 2;
			this.length -= num * 2;
			actual = num;
			return true;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005C60 File Offset: 0x00003E60
		public bool TryGetDictionaryString(out XmlDictionaryString value)
		{
			if (this.type == ValueHandleType.Dictionary)
			{
				value = this.GetDictionaryString();
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005C7A File Offset: 0x00003E7A
		public bool TryGetByteArrayLength(out int length)
		{
			if (this.type == ValueHandleType.Base64)
			{
				length = this.length;
				return true;
			}
			length = 0;
			return false;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005C94 File Offset: 0x00003E94
		private string GetCharsText()
		{
			if (this.length == 1 && this.bufferReader.GetByte(this.offset) == 49)
			{
				return "1";
			}
			return this.bufferReader.GetString(this.offset, this.length);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005CD1 File Offset: 0x00003ED1
		private string GetUnicodeCharsText()
		{
			return this.bufferReader.GetUnicodeString(this.offset, this.length);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005CEA File Offset: 0x00003EEA
		private string GetEscapedCharsText()
		{
			return this.bufferReader.GetEscapedString(this.offset, this.length);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005D04 File Offset: 0x00003F04
		private string GetCharText()
		{
			int @char = this.GetChar();
			if (@char > 65535)
			{
				SurrogateChar surrogateChar = new SurrogateChar(@char);
				return new string(new char[] { surrogateChar.HighChar, surrogateChar.LowChar }, 0, 2);
			}
			return ((char)@char).ToString();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005D54 File Offset: 0x00003F54
		private int GetChar()
		{
			return this.offset;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005D5C File Offset: 0x00003F5C
		private int GetInt8()
		{
			return this.bufferReader.GetInt8(this.offset);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005D6F File Offset: 0x00003F6F
		private int GetInt16()
		{
			return this.bufferReader.GetInt16(this.offset);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005D82 File Offset: 0x00003F82
		private int GetInt32()
		{
			return this.bufferReader.GetInt32(this.offset);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005D95 File Offset: 0x00003F95
		private long GetInt64()
		{
			return this.bufferReader.GetInt64(this.offset);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005DA8 File Offset: 0x00003FA8
		private ulong GetUInt64()
		{
			return this.bufferReader.GetUInt64(this.offset);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005DBB File Offset: 0x00003FBB
		private float GetSingle()
		{
			return this.bufferReader.GetSingle(this.offset);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005DCE File Offset: 0x00003FCE
		private double GetDouble()
		{
			return this.bufferReader.GetDouble(this.offset);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005DE1 File Offset: 0x00003FE1
		private decimal GetDecimal()
		{
			return this.bufferReader.GetDecimal(this.offset);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005DF4 File Offset: 0x00003FF4
		private UniqueId GetUniqueId()
		{
			return this.bufferReader.GetUniqueId(this.offset);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005E07 File Offset: 0x00004007
		private Guid GetGuid()
		{
			return this.bufferReader.GetGuid(this.offset);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005E1A File Offset: 0x0000401A
		private void GetBase64(byte[] buffer, int offset, int count)
		{
			this.bufferReader.GetBase64(this.offset, buffer, offset, count);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005E30 File Offset: 0x00004030
		private XmlDictionaryString GetDictionaryString()
		{
			return this.bufferReader.GetDictionaryString(this.offset);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005E43 File Offset: 0x00004043
		private string GetQNameDictionaryText()
		{
			return PrefixHandle.GetString(PrefixHandle.GetAlphaPrefix(this.length)) + ":" + this.bufferReader.GetDictionaryString(this.offset);
		}

		// Token: 0x04000091 RID: 145
		private XmlBufferReader bufferReader;

		// Token: 0x04000092 RID: 146
		private ValueHandleType type;

		// Token: 0x04000093 RID: 147
		private int offset;

		// Token: 0x04000094 RID: 148
		private int length;

		// Token: 0x04000095 RID: 149
		private static Base64Encoding base64Encoding;

		// Token: 0x04000096 RID: 150
		private static string[] constStrings = new string[] { "string", "number", "array", "object", "boolean", "null" };
	}
}
