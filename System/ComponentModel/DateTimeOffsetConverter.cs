using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000539 RID: 1337
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DateTimeOffsetConverter : TypeConverter
	{
		// Token: 0x06003265 RID: 12901 RVA: 0x000E1B02 File Offset: 0x000DFD02
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x000E1B20 File Offset: 0x000DFD20
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000E1B40 File Offset: 0x000DFD40
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return DateTimeOffset.MinValue;
				}
				try
				{
					DateTimeFormatInfo dateTimeFormatInfo = null;
					if (culture != null)
					{
						dateTimeFormatInfo = (DateTimeFormatInfo)culture.GetFormat(typeof(DateTimeFormatInfo));
					}
					if (dateTimeFormatInfo != null)
					{
						return DateTimeOffset.Parse(text, dateTimeFormatInfo);
					}
					return DateTimeOffset.Parse(text, culture);
				}
				catch (FormatException ex)
				{
					throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
					{
						(string)value,
						"DateTimeOffset"
					}), ex);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x000E1BF8 File Offset: 0x000DFDF8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(string)) || !(value is DateTimeOffset))
			{
				if (destinationType == typeof(InstanceDescriptor) && value is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
					if (dateTimeOffset.Ticks == 0L)
					{
						ConstructorInfo constructor = typeof(DateTimeOffset).GetConstructor(new Type[] { typeof(long) });
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[] { dateTimeOffset.Ticks });
						}
					}
					ConstructorInfo constructor2 = typeof(DateTimeOffset).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(TimeSpan)
					});
					if (constructor2 != null)
					{
						return new InstanceDescriptor(constructor2, new object[] { dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, dateTimeOffset.Minute, dateTimeOffset.Second, dateTimeOffset.Millisecond, dateTimeOffset.Offset });
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			DateTimeOffset dateTimeOffset2 = (DateTimeOffset)value;
			if (dateTimeOffset2 == DateTimeOffset.MinValue)
			{
				return string.Empty;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)culture.GetFormat(typeof(DateTimeFormatInfo));
			if (culture != CultureInfo.InvariantCulture)
			{
				string text;
				if (dateTimeOffset2.TimeOfDay.TotalSeconds == 0.0)
				{
					text = dateTimeFormatInfo.ShortDatePattern + " zzz";
				}
				else
				{
					text = dateTimeFormatInfo.ShortDatePattern + " " + dateTimeFormatInfo.ShortTimePattern + " zzz";
				}
				return dateTimeOffset2.ToString(text, CultureInfo.CurrentCulture);
			}
			if (dateTimeOffset2.TimeOfDay.TotalSeconds == 0.0)
			{
				return dateTimeOffset2.ToString("yyyy-MM-dd zzz", culture);
			}
			return dateTimeOffset2.ToString(culture);
		}
	}
}
