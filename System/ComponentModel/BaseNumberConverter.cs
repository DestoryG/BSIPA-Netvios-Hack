using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000519 RID: 1305
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class BaseNumberConverter : TypeConverter
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06003169 RID: 12649 RVA: 0x000DF4FE File Offset: 0x000DD6FE
		internal virtual bool AllowHex
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x0600316A RID: 12650
		internal abstract Type TargetType { get; }

		// Token: 0x0600316B RID: 12651
		internal abstract object FromString(string value, int radix);

		// Token: 0x0600316C RID: 12652
		internal abstract object FromString(string value, NumberFormatInfo formatInfo);

		// Token: 0x0600316D RID: 12653
		internal abstract object FromString(string value, CultureInfo culture);

		// Token: 0x0600316E RID: 12654 RVA: 0x000DF501 File Offset: 0x000DD701
		internal virtual Exception FromStringError(string failedText, Exception innerException)
		{
			return new Exception(SR.GetString("ConvertInvalidPrimitive", new object[]
			{
				failedText,
				this.TargetType.Name
			}), innerException);
		}

		// Token: 0x0600316F RID: 12655
		internal abstract string ToString(object value, NumberFormatInfo formatInfo);

		// Token: 0x06003170 RID: 12656 RVA: 0x000DF52B File Offset: 0x000DD72B
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x000DF54C File Offset: 0x000DD74C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				try
				{
					if (this.AllowHex && text[0] == '#')
					{
						return this.FromString(text.Substring(1), 16);
					}
					if ((this.AllowHex && text.StartsWith("0x")) || text.StartsWith("0X") || text.StartsWith("&h") || text.StartsWith("&H"))
					{
						return this.FromString(text.Substring(2), 16);
					}
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					NumberFormatInfo numberFormatInfo = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
					return this.FromString(text, numberFormatInfo);
				}
				catch (Exception ex)
				{
					throw this.FromStringError(text, ex);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x000DF638 File Offset: 0x000DD838
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value != null && this.TargetType.IsInstanceOfType(value))
			{
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				NumberFormatInfo numberFormatInfo = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
				return this.ToString(value, numberFormatInfo);
			}
			if (destinationType.IsPrimitive)
			{
				return Convert.ChangeType(value, destinationType, culture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x000DF6C5 File Offset: 0x000DD8C5
		public override bool CanConvertTo(ITypeDescriptorContext context, Type t)
		{
			return base.CanConvertTo(context, t) || t.IsPrimitive;
		}
	}
}
