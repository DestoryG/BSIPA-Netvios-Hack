using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000538 RID: 1336
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DateTimeConverter : TypeConverter
	{
		// Token: 0x06003260 RID: 12896 RVA: 0x000E179D File Offset: 0x000DF99D
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x000E17BB File Offset: 0x000DF9BB
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x000E17DC File Offset: 0x000DF9DC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return DateTime.MinValue;
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
						return DateTime.Parse(text, dateTimeFormatInfo);
					}
					return DateTime.Parse(text, culture);
				}
				catch (FormatException ex)
				{
					throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
					{
						(string)value,
						"DateTime"
					}), ex);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x000E1894 File Offset: 0x000DFA94
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(string)) || !(value is DateTime))
			{
				if (destinationType == typeof(InstanceDescriptor) && value is DateTime)
				{
					DateTime dateTime = (DateTime)value;
					if (dateTime.Ticks == 0L)
					{
						ConstructorInfo constructor = typeof(DateTime).GetConstructor(new Type[] { typeof(long) });
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[] { dateTime.Ticks });
						}
					}
					ConstructorInfo constructor2 = typeof(DateTime).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int)
					});
					if (constructor2 != null)
					{
						return new InstanceDescriptor(constructor2, new object[] { dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond });
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			DateTime dateTime2 = (DateTime)value;
			if (dateTime2 == DateTime.MinValue)
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
				if (dateTime2.TimeOfDay.TotalSeconds == 0.0)
				{
					text = dateTimeFormatInfo.ShortDatePattern;
				}
				else
				{
					text = dateTimeFormatInfo.ShortDatePattern + " " + dateTimeFormatInfo.ShortTimePattern;
				}
				return dateTime2.ToString(text, CultureInfo.CurrentCulture);
			}
			if (dateTime2.TimeOfDay.TotalSeconds == 0.0)
			{
				return dateTime2.ToString("yyyy-MM-dd", culture);
			}
			return dateTime2.ToString(culture);
		}
	}
}
