using System;
using System.Globalization;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200007D RID: 125
	[DataContract(Name = "DateTimeOffset", Namespace = "http://schemas.datacontract.org/2004/07/System")]
	internal struct DateTimeOffsetAdapter
	{
		// Token: 0x0600093E RID: 2366 RVA: 0x00029865 File Offset: 0x00027A65
		public DateTimeOffsetAdapter(DateTime dateTime, short offsetMinutes)
		{
			this.utcDateTime = dateTime;
			this.offsetMinutes = offsetMinutes;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00029875 File Offset: 0x00027A75
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0002987D File Offset: 0x00027A7D
		[DataMember(Name = "DateTime", IsRequired = true)]
		public DateTime UtcDateTime
		{
			get
			{
				return this.utcDateTime;
			}
			set
			{
				this.utcDateTime = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00029886 File Offset: 0x00027A86
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x0002988E File Offset: 0x00027A8E
		[DataMember(Name = "OffsetMinutes", IsRequired = true)]
		public short OffsetMinutes
		{
			get
			{
				return this.offsetMinutes;
			}
			set
			{
				this.offsetMinutes = value;
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00029898 File Offset: 0x00027A98
		public static DateTimeOffset GetDateTimeOffset(DateTimeOffsetAdapter value)
		{
			DateTimeOffset dateTimeOffset;
			try
			{
				if (value.UtcDateTime.Kind == DateTimeKind.Unspecified)
				{
					dateTimeOffset = new DateTimeOffset(value.UtcDateTime, new TimeSpan(0, (int)value.OffsetMinutes, 0));
				}
				else
				{
					DateTimeOffset dateTimeOffset2 = new DateTimeOffset(value.UtcDateTime);
					dateTimeOffset = dateTimeOffset2.ToOffset(new TimeSpan(0, (int)value.OffsetMinutes, 0));
				}
			}
			catch (ArgumentException ex)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(value.ToString(CultureInfo.InvariantCulture), "DateTimeOffset", ex));
			}
			return dateTimeOffset;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002992C File Offset: 0x00027B2C
		public static DateTimeOffsetAdapter GetDateTimeOffsetAdapter(DateTimeOffset value)
		{
			return new DateTimeOffsetAdapter(value.UtcDateTime, (short)value.Offset.TotalMinutes);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00029955 File Offset: 0x00027B55
		public string ToString(IFormatProvider provider)
		{
			return string.Concat(new object[] { "DateTime: ", this.UtcDateTime, ", Offset: ", this.OffsetMinutes });
		}

		// Token: 0x04000352 RID: 850
		private DateTime utcDateTime;

		// Token: 0x04000353 RID: 851
		private short offsetMinutes;
	}
}
