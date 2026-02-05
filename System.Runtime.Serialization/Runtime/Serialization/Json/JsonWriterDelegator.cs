using System;
using System.Globalization;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000115 RID: 277
	internal class JsonWriterDelegator : XmlWriterDelegator
	{
		// Token: 0x0600105A RID: 4186 RVA: 0x00042AE7 File Offset: 0x00040CE7
		public JsonWriterDelegator(XmlWriter writer)
			: base(writer)
		{
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00042AF0 File Offset: 0x00040CF0
		public JsonWriterDelegator(XmlWriter writer, DateTimeFormat dateTimeFormat)
			: this(writer)
		{
			this.dateTimeFormat = dateTimeFormat;
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00042B00 File Offset: 0x00040D00
		internal override void WriteChar(char value)
		{
			base.WriteString(XmlConvert.ToString(value));
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00042B0E File Offset: 0x00040D0E
		internal override void WriteBase64(byte[] bytes)
		{
			if (bytes == null)
			{
				return;
			}
			ByteArrayHelperWithString.Instance.WriteArray(base.Writer, bytes, 0, bytes.Length);
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00042B2C File Offset: 0x00040D2C
		internal override void WriteQName(XmlQualifiedName value)
		{
			if (value != XmlQualifiedName.Empty)
			{
				this.writer.WriteString(value.Name);
				this.writer.WriteString(":");
				this.writer.WriteString(value.Namespace);
			}
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00042B78 File Offset: 0x00040D78
		internal override void WriteUnsignedLong(ulong value)
		{
			this.WriteDecimal(value);
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x00042B86 File Offset: 0x00040D86
		internal override void WriteDecimal(decimal value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteDecimal(value);
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00042BA4 File Offset: 0x00040DA4
		internal override void WriteDouble(double value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteDouble(value);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00042BC2 File Offset: 0x00040DC2
		internal override void WriteFloat(float value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteFloat(value);
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00042BE0 File Offset: 0x00040DE0
		internal override void WriteLong(long value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteLong(value);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00042BFE File Offset: 0x00040DFE
		internal override void WriteSignedByte(sbyte value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteSignedByte(value);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x00042C1C File Offset: 0x00040E1C
		internal override void WriteUnsignedInt(uint value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteUnsignedInt(value);
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x00042C3A File Offset: 0x00040E3A
		internal override void WriteUnsignedShort(ushort value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteUnsignedShort(value);
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00042C58 File Offset: 0x00040E58
		internal override void WriteUnsignedByte(byte value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteUnsignedByte(value);
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00042C76 File Offset: 0x00040E76
		internal override void WriteShort(short value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteShort(value);
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00042C94 File Offset: 0x00040E94
		internal override void WriteBoolean(bool value)
		{
			this.writer.WriteAttributeString("type", "boolean");
			base.WriteBoolean(value);
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00042CB2 File Offset: 0x00040EB2
		internal override void WriteInt(int value)
		{
			this.writer.WriteAttributeString("type", "number");
			base.WriteInt(value);
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00042CD0 File Offset: 0x00040ED0
		internal void WriteJsonBooleanArray(bool[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			for (int i = 0; i < value.Length; i++)
			{
				base.WriteBoolean(value[i], itemName, itemNamespace);
			}
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x00042CF8 File Offset: 0x00040EF8
		internal void WriteJsonDateTimeArray(DateTime[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			for (int i = 0; i < value.Length; i++)
			{
				base.WriteDateTime(value[i], itemName, itemNamespace);
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00042D24 File Offset: 0x00040F24
		internal void WriteJsonDecimalArray(decimal[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			for (int i = 0; i < value.Length; i++)
			{
				base.WriteDecimal(value[i], itemName, itemNamespace);
			}
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00042D50 File Offset: 0x00040F50
		internal void WriteJsonInt32Array(int[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			for (int i = 0; i < value.Length; i++)
			{
				base.WriteInt(value[i], itemName, itemNamespace);
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00042D78 File Offset: 0x00040F78
		internal void WriteJsonInt64Array(long[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			for (int i = 0; i < value.Length; i++)
			{
				base.WriteLong(value[i], itemName, itemNamespace);
			}
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00042D9E File Offset: 0x00040F9E
		internal override void WriteDateTime(DateTime value)
		{
			if (this.dateTimeFormat == null)
			{
				this.WriteDateTimeInDefaultFormat(value);
				return;
			}
			this.writer.WriteString(value.ToString(this.dateTimeFormat.FormatString, this.dateTimeFormat.FormatProvider));
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00042DD8 File Offset: 0x00040FD8
		private void WriteDateTimeInDefaultFormat(DateTime value)
		{
			if (value.Kind != DateTimeKind.Utc)
			{
				long num;
				if (!LocalAppContextSwitches.DoNotUseTimeZoneInfo)
				{
					num = value.Ticks - TimeZoneInfo.Local.GetUtcOffset(value).Ticks;
				}
				else
				{
					num = value.Ticks - TimeZone.CurrentTimeZone.GetUtcOffset(value).Ticks;
				}
				if (num > DateTime.MaxValue.Ticks || num < DateTime.MinValue.Ticks)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("JSON DateTime is out of range."), new ArgumentOutOfRangeException("value")));
				}
			}
			this.writer.WriteString("/Date(");
			this.writer.WriteValue((value.ToUniversalTime().Ticks - JsonGlobals.unixEpochTicks) / 10000L);
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
			case DateTimeKind.Local:
			{
				TimeSpan timeSpan;
				if (!LocalAppContextSwitches.DoNotUseTimeZoneInfo)
				{
					timeSpan = TimeZoneInfo.Local.GetUtcOffset(value.ToLocalTime());
				}
				else
				{
					timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset(value.ToLocalTime());
				}
				if (timeSpan.Ticks < 0L)
				{
					this.writer.WriteString("-");
				}
				else
				{
					this.writer.WriteString("+");
				}
				int num2 = Math.Abs(timeSpan.Hours);
				this.writer.WriteString((num2 < 10) ? ("0" + num2) : num2.ToString(CultureInfo.InvariantCulture));
				int num3 = Math.Abs(timeSpan.Minutes);
				this.writer.WriteString((num3 < 10) ? ("0" + num3) : num3.ToString(CultureInfo.InvariantCulture));
				break;
			}
			}
			this.writer.WriteString(")/");
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00042FB0 File Offset: 0x000411B0
		internal void WriteJsonSingleArray(float[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			for (int i = 0; i < value.Length; i++)
			{
				base.WriteFloat(value[i], itemName, itemNamespace);
			}
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00042FD8 File Offset: 0x000411D8
		internal void WriteJsonDoubleArray(double[] value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
		{
			for (int i = 0; i < value.Length; i++)
			{
				base.WriteDouble(value[i], itemName, itemNamespace);
			}
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00042FFE File Offset: 0x000411FE
		internal override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (localName != null && localName.Length == 0)
			{
				base.WriteStartElement("item", "item");
				base.WriteAttributeString(null, "item", null, localName);
				return;
			}
			base.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x04000831 RID: 2097
		private DateTimeFormat dateTimeFormat;
	}
}
