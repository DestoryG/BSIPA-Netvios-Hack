using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005AD RID: 1453
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class StringConverter : TypeConverter
	{
		// Token: 0x06003621 RID: 13857 RVA: 0x000EC395 File Offset: 0x000EA595
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x000EC3B3 File Offset: 0x000EA5B3
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return (string)value;
			}
			if (value == null)
			{
				return "";
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
