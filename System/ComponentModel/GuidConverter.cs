using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000555 RID: 1365
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class GuidConverter : TypeConverter
	{
		// Token: 0x0600334D RID: 13133 RVA: 0x000E3AD0 File Offset: 0x000E1CD0
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000E3AEE File Offset: 0x000E1CEE
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x000E3B0C File Offset: 0x000E1D0C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				return new Guid(text);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x000E3B44 File Offset: 0x000E1D44
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is Guid)
			{
				ConstructorInfo constructor = typeof(Guid).GetConstructor(new Type[] { typeof(string) });
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[] { value.ToString() });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
