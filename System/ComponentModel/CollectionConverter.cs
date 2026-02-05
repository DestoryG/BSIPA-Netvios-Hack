using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000528 RID: 1320
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CollectionConverter : TypeConverter
	{
		// Token: 0x060031F4 RID: 12788 RVA: 0x000E0440 File Offset: 0x000DE640
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is ICollection)
			{
				return SR.GetString("CollectionConverterText");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000E0493 File Offset: 0x000DE693
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return null;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x000E0496 File Offset: 0x000DE696
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}
	}
}
