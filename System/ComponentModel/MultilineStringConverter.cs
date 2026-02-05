using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000592 RID: 1426
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class MultilineStringConverter : TypeConverter
	{
		// Token: 0x060034F8 RID: 13560 RVA: 0x000E72D4 File Offset: 0x000E54D4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is string)
			{
				return SR.GetString("MultilineStringConverterText");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x000E7327 File Offset: 0x000E5527
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return null;
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x000E732A File Offset: 0x000E552A
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}
	}
}
