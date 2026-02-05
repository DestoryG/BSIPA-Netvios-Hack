using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000524 RID: 1316
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CharConverter : TypeConverter
	{
		// Token: 0x060031E9 RID: 12777 RVA: 0x000E033C File Offset: 0x000DE53C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x000E035A File Offset: 0x000DE55A
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is char && (char)value == '\0')
			{
				return "";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x000E0390 File Offset: 0x000DE590
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text = (string)value;
			if (text.Length > 1)
			{
				text = text.Trim();
			}
			if (text == null || text.Length <= 0)
			{
				return '\0';
			}
			if (text.Length != 1)
			{
				throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[] { text, "Char" }));
			}
			return text[0];
		}
	}
}
