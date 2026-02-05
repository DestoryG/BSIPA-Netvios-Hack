using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000033 RID: 51
	internal static class XmlConverter
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00012C65 File Offset: 0x00010E65
		public static Base64Encoding Base64Encoding
		{
			get
			{
				if (XmlConverter.base64Encoding == null)
				{
					XmlConverter.base64Encoding = new Base64Encoding();
				}
				return XmlConverter.base64Encoding;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00012C7D File Offset: 0x00010E7D
		private static UTF8Encoding UTF8Encoding
		{
			get
			{
				if (XmlConverter.utf8Encoding == null)
				{
					XmlConverter.utf8Encoding = new UTF8Encoding(false, true);
				}
				return XmlConverter.utf8Encoding;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00012C97 File Offset: 0x00010E97
		private static UnicodeEncoding UnicodeEncoding
		{
			get
			{
				if (XmlConverter.unicodeEncoding == null)
				{
					XmlConverter.unicodeEncoding = new UnicodeEncoding(false, false, true);
				}
				return XmlConverter.unicodeEncoding;
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00012CB4 File Offset: 0x00010EB4
		public static bool ToBoolean(string value)
		{
			bool flag;
			try
			{
				flag = XmlConvert.ToBoolean(value);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Boolean", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Boolean", ex2));
			}
			return flag;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00012D10 File Offset: 0x00010F10
		public static bool ToBoolean(byte[] buffer, int offset, int count)
		{
			if (count == 1)
			{
				byte b = buffer[offset];
				if (b == 49)
				{
					return true;
				}
				if (b == 48)
				{
					return false;
				}
			}
			return XmlConverter.ToBoolean(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00012D40 File Offset: 0x00010F40
		public static int ToInt32(string value)
		{
			int num;
			try
			{
				num = XmlConvert.ToInt32(value);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int32", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int32", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int32", ex3));
			}
			return num;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00012DB8 File Offset: 0x00010FB8
		public static int ToInt32(byte[] buffer, int offset, int count)
		{
			int num;
			if (XmlConverter.TryParseInt32(buffer, offset, count, out num))
			{
				return num;
			}
			return XmlConverter.ToInt32(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00012DE0 File Offset: 0x00010FE0
		public static long ToInt64(string value)
		{
			long num;
			try
			{
				num = XmlConvert.ToInt64(value);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int64", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int64", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Int64", ex3));
			}
			return num;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00012E58 File Offset: 0x00011058
		public static long ToInt64(byte[] buffer, int offset, int count)
		{
			long num;
			if (XmlConverter.TryParseInt64(buffer, offset, count, out num))
			{
				return num;
			}
			return XmlConverter.ToInt64(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00012E80 File Offset: 0x00011080
		public static float ToSingle(string value)
		{
			float num;
			try
			{
				num = XmlConvert.ToSingle(value);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "float", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "float", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "float", ex3));
			}
			return num;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00012EF8 File Offset: 0x000110F8
		public static float ToSingle(byte[] buffer, int offset, int count)
		{
			float num;
			if (XmlConverter.TryParseSingle(buffer, offset, count, out num))
			{
				return num;
			}
			return XmlConverter.ToSingle(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00012F20 File Offset: 0x00011120
		public static double ToDouble(string value)
		{
			double num;
			try
			{
				num = XmlConvert.ToDouble(value);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "double", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "double", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "double", ex3));
			}
			return num;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00012F98 File Offset: 0x00011198
		public static double ToDouble(byte[] buffer, int offset, int count)
		{
			double num;
			if (XmlConverter.TryParseDouble(buffer, offset, count, out num))
			{
				return num;
			}
			return XmlConverter.ToDouble(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00012FC0 File Offset: 0x000111C0
		public static decimal ToDecimal(string value)
		{
			decimal num;
			try
			{
				num = XmlConvert.ToDecimal(value);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "decimal", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "decimal", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "decimal", ex3));
			}
			return num;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00013038 File Offset: 0x00011238
		public static decimal ToDecimal(byte[] buffer, int offset, int count)
		{
			return XmlConverter.ToDecimal(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00013048 File Offset: 0x00011248
		public static DateTime ToDateTime(long value)
		{
			DateTime dateTime;
			try
			{
				dateTime = DateTime.FromBinary(value);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(XmlConverter.ToString(value), "DateTime", ex));
			}
			return dateTime;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00013088 File Offset: 0x00011288
		public static DateTime ToDateTime(string value)
		{
			DateTime dateTime;
			try
			{
				dateTime = XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "DateTime", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "DateTime", ex2));
			}
			return dateTime;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000130E4 File Offset: 0x000112E4
		public static DateTime ToDateTime(byte[] buffer, int offset, int count)
		{
			DateTime dateTime;
			if (XmlConverter.TryParseDateTime(buffer, offset, count, out dateTime))
			{
				return dateTime;
			}
			return XmlConverter.ToDateTime(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0001310C File Offset: 0x0001130C
		public static UniqueId ToUniqueId(string value)
		{
			UniqueId uniqueId;
			try
			{
				uniqueId = new UniqueId(XmlConverter.Trim(value));
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "UniqueId", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "UniqueId", ex2));
			}
			return uniqueId;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0001316C File Offset: 0x0001136C
		public static UniqueId ToUniqueId(byte[] buffer, int offset, int count)
		{
			return XmlConverter.ToUniqueId(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001317C File Offset: 0x0001137C
		public static TimeSpan ToTimeSpan(string value)
		{
			TimeSpan timeSpan;
			try
			{
				timeSpan = XmlConvert.ToTimeSpan(value);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "TimeSpan", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "TimeSpan", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "TimeSpan", ex3));
			}
			return timeSpan;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000131F4 File Offset: 0x000113F4
		public static TimeSpan ToTimeSpan(byte[] buffer, int offset, int count)
		{
			return XmlConverter.ToTimeSpan(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00013204 File Offset: 0x00011404
		public static Guid ToGuid(string value)
		{
			Guid guid;
			try
			{
				guid = Guid.Parse(XmlConverter.Trim(value));
			}
			catch (FormatException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Guid", ex));
			}
			catch (ArgumentException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Guid", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "Guid", ex3));
			}
			return guid;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00013284 File Offset: 0x00011484
		public static Guid ToGuid(byte[] buffer, int offset, int count)
		{
			return XmlConverter.ToGuid(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00013294 File Offset: 0x00011494
		public static ulong ToUInt64(string value)
		{
			ulong num;
			try
			{
				num = ulong.Parse(value, NumberFormatInfo.InvariantInfo);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "UInt64", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "UInt64", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value, "UInt64", ex3));
			}
			return num;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00013314 File Offset: 0x00011514
		public static ulong ToUInt64(byte[] buffer, int offset, int count)
		{
			return XmlConverter.ToUInt64(XmlConverter.ToString(buffer, offset, count));
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00013324 File Offset: 0x00011524
		public static string ToString(byte[] buffer, int offset, int count)
		{
			string @string;
			try
			{
				@string = XmlConverter.UTF8Encoding.GetString(buffer, offset, count);
			}
			catch (DecoderFallbackException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateEncodingException(buffer, offset, count, ex));
			}
			return @string;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00013364 File Offset: 0x00011564
		public static string ToStringUnicode(byte[] buffer, int offset, int count)
		{
			string @string;
			try
			{
				@string = XmlConverter.UnicodeEncoding.GetString(buffer, offset, count);
			}
			catch (DecoderFallbackException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateEncodingException(buffer, offset, count, ex));
			}
			return @string;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000133A4 File Offset: 0x000115A4
		public static byte[] ToBytes(string value)
		{
			byte[] bytes;
			try
			{
				bytes = XmlConverter.UTF8Encoding.GetBytes(value);
			}
			catch (DecoderFallbackException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateEncodingException(value, ex));
			}
			return bytes;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000133E0 File Offset: 0x000115E0
		public static int ToChars(byte[] buffer, int offset, int count, char[] chars, int charOffset)
		{
			int chars2;
			try
			{
				chars2 = XmlConverter.UTF8Encoding.GetChars(buffer, offset, count, chars, charOffset);
			}
			catch (DecoderFallbackException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateEncodingException(buffer, offset, count, ex));
			}
			return chars2;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00013424 File Offset: 0x00011624
		public static string ToString(bool value)
		{
			if (!value)
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00013434 File Offset: 0x00011634
		public static string ToString(int value)
		{
			return XmlConvert.ToString(value);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001343C File Offset: 0x0001163C
		public static string ToString(long value)
		{
			return XmlConvert.ToString(value);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00013444 File Offset: 0x00011644
		public static string ToString(float value)
		{
			return XmlConvert.ToString(value);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001344C File Offset: 0x0001164C
		public static string ToString(double value)
		{
			return XmlConvert.ToString(value);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00013454 File Offset: 0x00011654
		public static string ToString(decimal value)
		{
			return XmlConvert.ToString(value);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001345C File Offset: 0x0001165C
		public static string ToString(TimeSpan value)
		{
			return XmlConvert.ToString(value);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00013464 File Offset: 0x00011664
		public static string ToString(UniqueId value)
		{
			return value.ToString();
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0001346C File Offset: 0x0001166C
		public static string ToString(Guid value)
		{
			return value.ToString();
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001347B File Offset: 0x0001167B
		public static string ToString(ulong value)
		{
			return value.ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001348C File Offset: 0x0001168C
		public static string ToString(DateTime value)
		{
			byte[] array = new byte[64];
			int num = XmlConverter.ToChars(value, array, 0);
			return XmlConverter.ToString(array, 0, num);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x000134B4 File Offset: 0x000116B4
		private static string ToString(object value)
		{
			if (value is int)
			{
				return XmlConverter.ToString((int)value);
			}
			if (value is long)
			{
				return XmlConverter.ToString((long)value);
			}
			if (value is float)
			{
				return XmlConverter.ToString((float)value);
			}
			if (value is double)
			{
				return XmlConverter.ToString((double)value);
			}
			if (value is decimal)
			{
				return XmlConverter.ToString((decimal)value);
			}
			if (value is TimeSpan)
			{
				return XmlConverter.ToString((TimeSpan)value);
			}
			if (value is UniqueId)
			{
				return XmlConverter.ToString((UniqueId)value);
			}
			if (value is Guid)
			{
				return XmlConverter.ToString((Guid)value);
			}
			if (value is ulong)
			{
				return XmlConverter.ToString((ulong)value);
			}
			if (value is DateTime)
			{
				return XmlConverter.ToString((DateTime)value);
			}
			if (value is bool)
			{
				return XmlConverter.ToString((bool)value);
			}
			return value.ToString();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x000135A4 File Offset: 0x000117A4
		public static string ToString(object[] objects)
		{
			if (objects.Length == 0)
			{
				return string.Empty;
			}
			string text = XmlConverter.ToString(objects[0]);
			if (objects.Length > 1)
			{
				StringBuilder stringBuilder = new StringBuilder(text);
				for (int i = 1; i < objects.Length; i++)
				{
					stringBuilder.Append(' ');
					stringBuilder.Append(XmlConverter.ToString(objects[i]));
				}
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00013600 File Offset: 0x00011800
		public static void ToQualifiedName(string qname, out string prefix, out string localName)
		{
			int num = qname.IndexOf(':');
			if (num < 0)
			{
				prefix = string.Empty;
				localName = XmlConverter.Trim(qname);
				return;
			}
			if (num == qname.Length - 1)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Expected XML qualified name. Found '{0}'.", new object[] { qname })));
			}
			prefix = XmlConverter.Trim(qname.Substring(0, num));
			localName = XmlConverter.Trim(qname.Substring(num + 1));
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00013674 File Offset: 0x00011874
		private static bool TryParseInt32(byte[] chars, int offset, int count, out int result)
		{
			result = 0;
			if (count == 0)
			{
				return false;
			}
			int num = 0;
			int num2 = offset + count;
			if (chars[offset] == 45)
			{
				if (count == 1)
				{
					return false;
				}
				for (int i = offset + 1; i < num2; i++)
				{
					int num3 = (int)(chars[i] - 48);
					if (num3 > 9)
					{
						return false;
					}
					if (num < -214748364)
					{
						return false;
					}
					num *= 10;
					if (num < -2147483648 + num3)
					{
						return false;
					}
					num -= num3;
				}
			}
			else
			{
				for (int j = offset; j < num2; j++)
				{
					int num4 = (int)(chars[j] - 48);
					if (num4 > 9)
					{
						return false;
					}
					if (num > 214748364)
					{
						return false;
					}
					num *= 10;
					if (num > 2147483647 - num4)
					{
						return false;
					}
					num += num4;
				}
			}
			result = num;
			return true;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00013720 File Offset: 0x00011920
		private static bool TryParseInt64(byte[] chars, int offset, int count, out long result)
		{
			result = 0L;
			if (count >= 11)
			{
				long num = 0L;
				int num2 = offset + count;
				if (chars[offset] == 45)
				{
					if (count == 1)
					{
						return false;
					}
					for (int i = offset + 1; i < num2; i++)
					{
						int num3 = (int)(chars[i] - 48);
						if (num3 > 9)
						{
							return false;
						}
						if (num < -922337203685477580L)
						{
							return false;
						}
						num *= 10L;
						if (num < -9223372036854775808L + (long)num3)
						{
							return false;
						}
						num -= (long)num3;
					}
				}
				else
				{
					for (int j = offset; j < num2; j++)
					{
						int num4 = (int)(chars[j] - 48);
						if (num4 > 9)
						{
							return false;
						}
						if (num > 922337203685477580L)
						{
							return false;
						}
						num *= 10L;
						if (num > 9223372036854775807L - (long)num4)
						{
							return false;
						}
						num += (long)num4;
					}
				}
				result = num;
				return true;
			}
			int num5;
			if (!XmlConverter.TryParseInt32(chars, offset, count, out num5))
			{
				return false;
			}
			result = (long)num5;
			return true;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000137FC File Offset: 0x000119FC
		private static bool TryParseSingle(byte[] chars, int offset, int count, out float result)
		{
			result = 0f;
			int num = offset + count;
			bool flag = false;
			if (offset < num && chars[offset] == 45)
			{
				flag = true;
				offset++;
				count--;
			}
			if (count < 1 || count > 10)
			{
				return false;
			}
			int num2 = 0;
			while (offset < num)
			{
				int num3 = (int)(chars[offset] - 48);
				if (num3 == -2)
				{
					offset++;
					int num4 = 1;
					while (offset < num)
					{
						num3 = (int)(chars[offset] - 48);
						if (num3 >= 10)
						{
							return false;
						}
						num4 *= 10;
						num2 = num2 * 10 + num3;
						offset++;
					}
					if (count > 8)
					{
						result = (float)((double)num2 / (double)num4);
					}
					else
					{
						result = (float)num2 / (float)num4;
					}
					if (flag)
					{
						result = -result;
					}
					return true;
				}
				if (num3 >= 10)
				{
					return false;
				}
				num2 = num2 * 10 + num3;
				offset++;
			}
			if (count == 10)
			{
				return false;
			}
			if (flag)
			{
				result = (float)(-(float)num2);
			}
			else
			{
				result = (float)num2;
			}
			return true;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000138C8 File Offset: 0x00011AC8
		private static bool TryParseDouble(byte[] chars, int offset, int count, out double result)
		{
			result = 0.0;
			int num = offset + count;
			bool flag = false;
			if (offset < num && chars[offset] == 45)
			{
				flag = true;
				offset++;
				count--;
			}
			if (count < 1 || count > 10)
			{
				return false;
			}
			int num2 = 0;
			while (offset < num)
			{
				int num3 = (int)(chars[offset] - 48);
				if (num3 == -2)
				{
					offset++;
					int num4 = 1;
					while (offset < num)
					{
						num3 = (int)(chars[offset] - 48);
						if (num3 >= 10)
						{
							return false;
						}
						num4 *= 10;
						num2 = num2 * 10 + num3;
						offset++;
					}
					if (flag)
					{
						result = -(double)num2 / (double)num4;
					}
					else
					{
						result = (double)num2 / (double)num4;
					}
					return true;
				}
				if (num3 >= 10)
				{
					return false;
				}
				num2 = num2 * 10 + num3;
				offset++;
			}
			if (count == 10)
			{
				return false;
			}
			if (flag)
			{
				result = (double)(-(double)num2);
			}
			else
			{
				result = (double)num2;
			}
			return true;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001398C File Offset: 0x00011B8C
		private static int ToInt32D2(byte[] chars, int offset)
		{
			byte b = chars[offset] - 48;
			byte b2 = chars[offset + 1] - 48;
			if (b > 9 || b2 > 9)
			{
				return -1;
			}
			return (int)(10 * b + b2);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000139BD File Offset: 0x00011BBD
		private static int ToInt32D4(byte[] chars, int offset, int count)
		{
			return XmlConverter.ToInt32D7(chars, offset, count);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000139C8 File Offset: 0x00011BC8
		private static int ToInt32D7(byte[] chars, int offset, int count)
		{
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				byte b = chars[offset + i] - 48;
				if (b > 9)
				{
					return -1;
				}
				num = num * 10 + (int)b;
			}
			return num;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000139FC File Offset: 0x00011BFC
		private static bool TryParseDateTime(byte[] chars, int offset, int count, out DateTime result)
		{
			int num = offset + count;
			result = DateTime.MaxValue;
			if (count < 19)
			{
				return false;
			}
			if (chars[offset + 4] != 45 || chars[offset + 7] != 45 || chars[offset + 10] != 84 || chars[offset + 13] != 58 || chars[offset + 16] != 58)
			{
				return false;
			}
			int num2 = XmlConverter.ToInt32D4(chars, offset, 4);
			int num3 = XmlConverter.ToInt32D2(chars, offset + 5);
			int num4 = XmlConverter.ToInt32D2(chars, offset + 8);
			int num5 = XmlConverter.ToInt32D2(chars, offset + 11);
			int num6 = XmlConverter.ToInt32D2(chars, offset + 14);
			int num7 = XmlConverter.ToInt32D2(chars, offset + 17);
			if ((num2 | num3 | num4 | num5 | num6 | num7) < 0)
			{
				return false;
			}
			DateTimeKind dateTimeKind = DateTimeKind.Unspecified;
			offset += 19;
			int num8 = 0;
			if (offset < num && chars[offset] == 46)
			{
				offset++;
				int num9 = offset;
				while (offset < num)
				{
					byte b = chars[offset];
					if (b < 48 || b > 57)
					{
						break;
					}
					offset++;
				}
				int num10 = offset - num9;
				if (num10 < 1 || num10 > 7)
				{
					return false;
				}
				num8 = XmlConverter.ToInt32D7(chars, num9, num10);
				if (num8 < 0)
				{
					return false;
				}
				for (int i = num10; i < 7; i++)
				{
					num8 *= 10;
				}
			}
			bool flag = false;
			int num11 = 0;
			int num12 = 0;
			if (offset < num)
			{
				byte b2 = chars[offset];
				if (b2 == 90)
				{
					offset++;
					dateTimeKind = DateTimeKind.Utc;
				}
				else if (b2 == 43 || b2 == 45)
				{
					offset++;
					if (offset + 5 > num || chars[offset + 2] != 58)
					{
						return false;
					}
					dateTimeKind = DateTimeKind.Utc;
					flag = true;
					num11 = XmlConverter.ToInt32D2(chars, offset);
					num12 = XmlConverter.ToInt32D2(chars, offset + 3);
					if ((num11 | num12) < 0)
					{
						return false;
					}
					if (b2 == 43)
					{
						num11 = -num11;
						num12 = -num12;
					}
					offset += 5;
				}
			}
			if (offset < num)
			{
				return false;
			}
			DateTime dateTime;
			try
			{
				dateTime = new DateTime(num2, num3, num4, num5, num6, num7, dateTimeKind);
			}
			catch (ArgumentException)
			{
				return false;
			}
			if (num8 > 0)
			{
				dateTime = dateTime.AddTicks((long)num8);
			}
			if (flag)
			{
				try
				{
					TimeSpan timeSpan = new TimeSpan(num11, num12, 0);
					if ((num11 >= 0 && dateTime < DateTime.MaxValue - timeSpan) || (num11 < 0 && dateTime > DateTime.MinValue - timeSpan))
					{
						dateTime = dateTime.Add(timeSpan).ToLocalTime();
					}
					else
					{
						dateTime = dateTime.ToLocalTime().Add(timeSpan);
					}
				}
				catch (ArgumentOutOfRangeException)
				{
					return false;
				}
			}
			result = dateTime;
			return true;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00013C70 File Offset: 0x00011E70
		public static int ToChars(bool value, byte[] buffer, int offset)
		{
			if (value)
			{
				buffer[offset] = 116;
				buffer[offset + 1] = 114;
				buffer[offset + 2] = 117;
				buffer[offset + 3] = 101;
				return 4;
			}
			buffer[offset] = 102;
			buffer[offset + 1] = 97;
			buffer[offset + 2] = 108;
			buffer[offset + 3] = 115;
			buffer[offset + 4] = 101;
			return 5;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00013CC0 File Offset: 0x00011EC0
		public static int ToCharsR(int value, byte[] chars, int offset)
		{
			int num = 0;
			if (value >= 0)
			{
				while (value >= 10)
				{
					int num2 = value / 10;
					num++;
					chars[--offset] = (byte)(48 + (value - num2 * 10));
					value = num2;
				}
				chars[--offset] = (byte)(48 + value);
				num++;
			}
			else
			{
				while (value <= -10)
				{
					int num3 = value / 10;
					num++;
					chars[--offset] = (byte)(48 - (value - num3 * 10));
					value = num3;
				}
				chars[--offset] = (byte)(48 - value);
				chars[--offset] = 45;
				num += 2;
			}
			return num;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00013D4C File Offset: 0x00011F4C
		public static int ToChars(int value, byte[] chars, int offset)
		{
			int num = XmlConverter.ToCharsR(value, chars, offset + 16);
			Buffer.BlockCopy(chars, offset + 16 - num, chars, offset, num);
			return num;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00013D78 File Offset: 0x00011F78
		public static int ToCharsR(long value, byte[] chars, int offset)
		{
			int num = 0;
			if (value >= 0L)
			{
				while (value > 2147483647L)
				{
					long num2 = value / 10L;
					num++;
					chars[--offset] = (byte)(48 + (int)(value - num2 * 10L));
					value = num2;
				}
			}
			else
			{
				while (value < -2147483648L)
				{
					long num3 = value / 10L;
					num++;
					chars[--offset] = (byte)(48 - (int)(value - num3 * 10L));
					value = num3;
				}
			}
			return num + XmlConverter.ToCharsR((int)value, chars, offset);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00013DF0 File Offset: 0x00011FF0
		public static int ToChars(long value, byte[] chars, int offset)
		{
			int num = XmlConverter.ToCharsR(value, chars, offset + 32);
			Buffer.BlockCopy(chars, offset + 32 - num, chars, offset, num);
			return num;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00013E1C File Offset: 0x0001201C
		[SecuritySafeCritical]
		private unsafe static bool IsNegativeZero(float value)
		{
			float num = -0f;
			return *(int*)(&value) == *(int*)(&num);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00013E3C File Offset: 0x0001203C
		[SecuritySafeCritical]
		private unsafe static bool IsNegativeZero(double value)
		{
			double num = -0.0;
			return *(long*)(&value) == *(long*)(&num);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00013E5D File Offset: 0x0001205D
		private static int ToInfinity(bool isNegative, byte[] buffer, int offset)
		{
			if (isNegative)
			{
				buffer[offset] = 45;
				buffer[offset + 1] = 73;
				buffer[offset + 2] = 78;
				buffer[offset + 3] = 70;
				return 4;
			}
			buffer[offset] = 73;
			buffer[offset + 1] = 78;
			buffer[offset + 2] = 70;
			return 3;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00013E92 File Offset: 0x00012092
		private static int ToZero(bool isNegative, byte[] buffer, int offset)
		{
			if (isNegative)
			{
				buffer[offset] = 45;
				buffer[offset + 1] = 48;
				return 2;
			}
			buffer[offset] = 48;
			return 1;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00013EAC File Offset: 0x000120AC
		public static int ToChars(double value, byte[] buffer, int offset)
		{
			if (double.IsInfinity(value))
			{
				return XmlConverter.ToInfinity(double.IsNegativeInfinity(value), buffer, offset);
			}
			if (value == 0.0)
			{
				return XmlConverter.ToZero(XmlConverter.IsNegativeZero(value), buffer, offset);
			}
			return XmlConverter.ToAsciiChars(value.ToString("R", NumberFormatInfo.InvariantInfo), buffer, offset);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00013F04 File Offset: 0x00012104
		public static int ToChars(float value, byte[] buffer, int offset)
		{
			if (float.IsInfinity(value))
			{
				return XmlConverter.ToInfinity(float.IsNegativeInfinity(value), buffer, offset);
			}
			if ((double)value == 0.0)
			{
				return XmlConverter.ToZero(XmlConverter.IsNegativeZero(value), buffer, offset);
			}
			return XmlConverter.ToAsciiChars(value.ToString("R", NumberFormatInfo.InvariantInfo), buffer, offset);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00013F5A File Offset: 0x0001215A
		public static int ToChars(decimal value, byte[] buffer, int offset)
		{
			return XmlConverter.ToAsciiChars(value.ToString(null, NumberFormatInfo.InvariantInfo), buffer, offset);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00013F70 File Offset: 0x00012170
		public static int ToChars(ulong value, byte[] buffer, int offset)
		{
			return XmlConverter.ToAsciiChars(value.ToString(null, NumberFormatInfo.InvariantInfo), buffer, offset);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00013F88 File Offset: 0x00012188
		private static int ToAsciiChars(string s, byte[] buffer, int offset)
		{
			for (int i = 0; i < s.Length; i++)
			{
				buffer[offset++] = (byte)s[i];
			}
			return s.Length;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00013FBC File Offset: 0x000121BC
		private static int ToCharsD2(int value, byte[] chars, int offset)
		{
			if (value < 10)
			{
				chars[offset] = 48;
				chars[offset + 1] = (byte)(48 + value);
			}
			else
			{
				int num = value / 10;
				chars[offset] = (byte)(48 + num);
				chars[offset + 1] = (byte)(48 + value - num * 10);
			}
			return 2;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00013FFC File Offset: 0x000121FC
		private static int ToCharsD4(int value, byte[] chars, int offset)
		{
			XmlConverter.ToCharsD2(value / 100, chars, offset);
			XmlConverter.ToCharsD2(value % 100, chars, offset + 2);
			return 4;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001401C File Offset: 0x0001221C
		private static int ToCharsD7(int value, byte[] chars, int offset)
		{
			int num = 7 - XmlConverter.ToCharsR(value, chars, offset + 7);
			for (int i = 0; i < num; i++)
			{
				chars[offset + i] = 48;
			}
			int num2 = 7;
			while (num2 > 0 && chars[offset + num2 - 1] == 48)
			{
				num2--;
			}
			return num2;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00014064 File Offset: 0x00012264
		public static int ToChars(DateTime value, byte[] chars, int offset)
		{
			int num = offset;
			offset += XmlConverter.ToCharsD4(value.Year, chars, offset);
			chars[offset++] = 45;
			offset += XmlConverter.ToCharsD2(value.Month, chars, offset);
			chars[offset++] = 45;
			offset += XmlConverter.ToCharsD2(value.Day, chars, offset);
			chars[offset++] = 84;
			offset += XmlConverter.ToCharsD2(value.Hour, chars, offset);
			chars[offset++] = 58;
			offset += XmlConverter.ToCharsD2(value.Minute, chars, offset);
			chars[offset++] = 58;
			offset += XmlConverter.ToCharsD2(value.Second, chars, offset);
			int num2 = (int)(value.Ticks % 10000000L);
			if (num2 != 0)
			{
				chars[offset++] = 46;
				offset += XmlConverter.ToCharsD7(num2, chars, offset);
			}
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				break;
			case DateTimeKind.Utc:
				chars[offset++] = 90;
				break;
			case DateTimeKind.Local:
			{
				TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
				if (utcOffset.Ticks < 0L)
				{
					chars[offset++] = 45;
				}
				else
				{
					chars[offset++] = 43;
				}
				offset += XmlConverter.ToCharsD2(Math.Abs(utcOffset.Hours), chars, offset);
				chars[offset++] = 58;
				offset += XmlConverter.ToCharsD2(Math.Abs(utcOffset.Minutes), chars, offset);
				break;
			}
			default:
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
			}
			return offset - num;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000141D8 File Offset: 0x000123D8
		public static bool IsWhitespace(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!XmlConverter.IsWhitespace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00014207 File Offset: 0x00012407
		public static bool IsWhitespace(char ch)
		{
			return ch <= ' ' && (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n');
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00014228 File Offset: 0x00012428
		public static string StripWhitespace(string s)
		{
			int num = s.Length;
			for (int i = 0; i < s.Length; i++)
			{
				if (XmlConverter.IsWhitespace(s[i]))
				{
					num--;
				}
			}
			if (num == s.Length)
			{
				return s;
			}
			char[] array = new char[num];
			num = 0;
			foreach (char c in s)
			{
				if (!XmlConverter.IsWhitespace(c))
				{
					array[num++] = c;
				}
			}
			return new string(array);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000142A8 File Offset: 0x000124A8
		private static string Trim(string s)
		{
			int num = 0;
			while (num < s.Length && XmlConverter.IsWhitespace(s[num]))
			{
				num++;
			}
			int num2 = s.Length;
			while (num2 > 0 && XmlConverter.IsWhitespace(s[num2 - 1]))
			{
				num2--;
			}
			if (num == 0 && num2 == s.Length)
			{
				return s;
			}
			if (num2 == 0)
			{
				return string.Empty;
			}
			return s.Substring(num, num2 - num);
		}

		// Token: 0x040001D3 RID: 467
		public const int MaxDateTimeChars = 64;

		// Token: 0x040001D4 RID: 468
		public const int MaxInt32Chars = 16;

		// Token: 0x040001D5 RID: 469
		public const int MaxInt64Chars = 32;

		// Token: 0x040001D6 RID: 470
		public const int MaxBoolChars = 5;

		// Token: 0x040001D7 RID: 471
		public const int MaxFloatChars = 16;

		// Token: 0x040001D8 RID: 472
		public const int MaxDoubleChars = 32;

		// Token: 0x040001D9 RID: 473
		public const int MaxDecimalChars = 40;

		// Token: 0x040001DA RID: 474
		public const int MaxUInt64Chars = 32;

		// Token: 0x040001DB RID: 475
		public const int MaxPrimitiveChars = 64;

		// Token: 0x040001DC RID: 476
		private static char[] whiteSpaceChars = new char[] { ' ', '\t', '\n', '\r' };

		// Token: 0x040001DD RID: 477
		private static UTF8Encoding utf8Encoding;

		// Token: 0x040001DE RID: 478
		private static UnicodeEncoding unicodeEncoding;

		// Token: 0x040001DF RID: 479
		private static Base64Encoding base64Encoding;
	}
}
