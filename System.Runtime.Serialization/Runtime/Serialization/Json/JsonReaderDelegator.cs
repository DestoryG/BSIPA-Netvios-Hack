using System;
using System.Globalization;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000111 RID: 273
	internal class JsonReaderDelegator : XmlReaderDelegator
	{
		// Token: 0x0600103B RID: 4155 RVA: 0x00042418 File Offset: 0x00040618
		public JsonReaderDelegator(XmlReader reader)
			: base(reader)
		{
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x00042421 File Offset: 0x00040621
		public JsonReaderDelegator(XmlReader reader, DateTimeFormat dateTimeFormat)
			: this(reader)
		{
			this.dateTimeFormat = dateTimeFormat;
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x00042431 File Offset: 0x00040631
		internal XmlDictionaryReaderQuotas ReaderQuotas
		{
			get
			{
				if (this.dictionaryReader == null)
				{
					return null;
				}
				return this.dictionaryReader.Quotas;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x00042448 File Offset: 0x00040648
		private JsonReaderDelegator.DateTimeArrayJsonHelperWithString DateTimeArrayHelper
		{
			get
			{
				if (this.dateTimeArrayHelper == null)
				{
					this.dateTimeArrayHelper = new JsonReaderDelegator.DateTimeArrayJsonHelperWithString(this.dateTimeFormat);
				}
				return this.dateTimeArrayHelper;
			}
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0004246C File Offset: 0x0004066C
		internal static XmlQualifiedName ParseQualifiedName(string qname)
		{
			string text2;
			string text;
			if (string.IsNullOrEmpty(qname))
			{
				text = (text2 = string.Empty);
			}
			else
			{
				qname = qname.Trim();
				int num = qname.IndexOf(':');
				if (num >= 0)
				{
					text2 = qname.Substring(0, num);
					text = qname.Substring(num + 1);
				}
				else
				{
					text2 = qname;
					text = string.Empty;
				}
			}
			return new XmlQualifiedName(text2, text);
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x000424C4 File Offset: 0x000406C4
		internal override char ReadContentAsChar()
		{
			return XmlConvert.ToChar(base.ReadContentAsString());
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x000424D1 File Offset: 0x000406D1
		internal override XmlQualifiedName ReadContentAsQName()
		{
			return JsonReaderDelegator.ParseQualifiedName(base.ReadContentAsString());
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x000424DE File Offset: 0x000406DE
		internal override char ReadElementContentAsChar()
		{
			return XmlConvert.ToChar(base.ReadElementContentAsString());
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x000424EC File Offset: 0x000406EC
		internal override byte[] ReadContentAsBase64()
		{
			if (this.isEndOfEmptyElement)
			{
				return new byte[0];
			}
			byte[] array;
			if (this.dictionaryReader == null)
			{
				XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(this.reader);
				array = ByteArrayHelperWithString.Instance.ReadArray(xmlDictionaryReader, "item", string.Empty, xmlDictionaryReader.Quotas.MaxArrayLength);
			}
			else
			{
				array = ByteArrayHelperWithString.Instance.ReadArray(this.dictionaryReader, "item", string.Empty, this.dictionaryReader.Quotas.MaxArrayLength);
			}
			return array;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0004256C File Offset: 0x0004076C
		internal override byte[] ReadElementContentAsBase64()
		{
			if (this.isEndOfEmptyElement)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Start element expected. Found {0}.", new object[] { "EndElement" })));
			}
			byte[] array;
			if (this.reader.IsStartElement() && this.reader.IsEmptyElement)
			{
				this.reader.Read();
				array = new byte[0];
			}
			else
			{
				this.reader.ReadStartElement();
				array = this.ReadContentAsBase64();
				this.reader.ReadEndElement();
			}
			return array;
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x000425F4 File Offset: 0x000407F4
		internal override DateTime ReadContentAsDateTime()
		{
			return JsonReaderDelegator.ParseJsonDate(base.ReadContentAsString(), this.dateTimeFormat);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00042607 File Offset: 0x00040807
		internal static DateTime ParseJsonDate(string originalDateTimeValue, DateTimeFormat dateTimeFormat)
		{
			if (dateTimeFormat == null)
			{
				return JsonReaderDelegator.ParseJsonDateInDefaultFormat(originalDateTimeValue);
			}
			return DateTime.ParseExact(originalDateTimeValue, dateTimeFormat.FormatString, dateTimeFormat.FormatProvider, dateTimeFormat.DateTimeStyles);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0004262C File Offset: 0x0004082C
		internal static DateTime ParseJsonDateInDefaultFormat(string originalDateTimeValue)
		{
			string text;
			if (!string.IsNullOrEmpty(originalDateTimeValue))
			{
				text = originalDateTimeValue.Trim();
			}
			else
			{
				text = originalDateTimeValue;
			}
			if (string.IsNullOrEmpty(text) || !text.StartsWith("/Date(", StringComparison.Ordinal) || !text.EndsWith(")/", StringComparison.Ordinal))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Invalid JSON dateTime string is specified: original value '{0}', start guide writer: {1}, end guard writer: {2}.", new object[] { originalDateTimeValue, "\\/Date(", ")\\/" })));
			}
			string text2 = text.Substring(6, text.Length - 8);
			DateTimeKind dateTimeKind = DateTimeKind.Utc;
			int num = text2.IndexOf('+', 1);
			if (num == -1)
			{
				num = text2.IndexOf('-', 1);
			}
			if (num != -1)
			{
				dateTimeKind = DateTimeKind.Local;
				text2 = text2.Substring(0, num);
			}
			long num2;
			try
			{
				num2 = long.Parse(text2, CultureInfo.InvariantCulture);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text2, "Int64", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text2, "Int64", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text2, "Int64", ex3));
			}
			long num3 = num2 * 10000L + JsonGlobals.unixEpochTicks;
			DateTime dateTime2;
			try
			{
				DateTime dateTime = new DateTime(num3, DateTimeKind.Utc);
				switch (dateTimeKind)
				{
				case DateTimeKind.Unspecified:
					return DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Unspecified);
				case DateTimeKind.Local:
					return dateTime.ToLocalTime();
				}
				dateTime2 = dateTime;
			}
			catch (ArgumentException ex4)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text2, "DateTime", ex4));
			}
			return dateTime2;
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x000427C8 File Offset: 0x000409C8
		internal override DateTime ReadElementContentAsDateTime()
		{
			return JsonReaderDelegator.ParseJsonDate(base.ReadElementContentAsString(), this.dateTimeFormat);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x000427DC File Offset: 0x000409DC
		internal bool TryReadJsonDateTimeArray(XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, int arrayLength, out DateTime[] array)
		{
			if (this.dictionaryReader == null || arrayLength != -1)
			{
				array = null;
				return false;
			}
			array = this.DateTimeArrayHelper.ReadArray(this.dictionaryReader, XmlDictionaryString.GetString(itemName), XmlDictionaryString.GetString(itemNamespace), base.GetArrayLengthQuota(context));
			context.IncrementItemCount(array.Length);
			return true;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00042830 File Offset: 0x00040A30
		internal override ulong ReadContentAsUnsignedLong()
		{
			string text = this.reader.ReadContentAsString();
			if (text == null || text.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(XmlObjectSerializer.TryAddLineInfo(this, SR.GetString("The value '{0}' cannot be parsed as the type '{1}'.", new object[] { text, "UInt64" }))));
			}
			ulong num;
			try
			{
				num = ulong.Parse(text, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "UInt64", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "UInt64", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "UInt64", ex3));
			}
			return num;
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000428FC File Offset: 0x00040AFC
		internal override ulong ReadElementContentAsUnsignedLong()
		{
			if (this.isEndOfEmptyElement)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Start element expected. Found {0}.", new object[] { "EndElement" })));
			}
			string text = this.reader.ReadElementContentAsString();
			if (text == null || text.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(XmlObjectSerializer.TryAddLineInfo(this, SR.GetString("The value '{0}' cannot be parsed as the type '{1}'.", new object[] { text, "UInt64" }))));
			}
			ulong num;
			try
			{
				num = ulong.Parse(text, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "UInt64", ex));
			}
			catch (FormatException ex2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "UInt64", ex2));
			}
			catch (OverflowException ex3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(text, "UInt64", ex3));
			}
			return num;
		}

		// Token: 0x0400082E RID: 2094
		private DateTimeFormat dateTimeFormat;

		// Token: 0x0400082F RID: 2095
		private JsonReaderDelegator.DateTimeArrayJsonHelperWithString dateTimeArrayHelper;

		// Token: 0x0200018E RID: 398
		private class DateTimeArrayJsonHelperWithString : ArrayHelper<string, DateTime>
		{
			// Token: 0x06001530 RID: 5424 RVA: 0x0005530E File Offset: 0x0005350E
			public DateTimeArrayJsonHelperWithString(DateTimeFormat dateTimeFormat)
			{
				this.dateTimeFormat = dateTimeFormat;
			}

			// Token: 0x06001531 RID: 5425 RVA: 0x00055320 File Offset: 0x00053520
			protected override int ReadArray(XmlDictionaryReader reader, string localName, string namespaceUri, DateTime[] array, int offset, int count)
			{
				XmlJsonReader.CheckArray(array, offset, count);
				int num = 0;
				while (num < count && reader.IsStartElement("item", string.Empty))
				{
					array[offset + num] = JsonReaderDelegator.ParseJsonDate(reader.ReadElementContentAsString(), this.dateTimeFormat);
					num++;
				}
				return num;
			}

			// Token: 0x06001532 RID: 5426 RVA: 0x00055374 File Offset: 0x00053574
			protected override void WriteArray(XmlDictionaryWriter writer, string prefix, string localName, string namespaceUri, DateTime[] array, int offset, int count)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotImplementedException());
			}

			// Token: 0x04000A59 RID: 2649
			private DateTimeFormat dateTimeFormat;
		}
	}
}
