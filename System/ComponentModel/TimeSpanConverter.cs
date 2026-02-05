using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005AF RID: 1455
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class TimeSpanConverter : TypeConverter
	{
		// Token: 0x06003627 RID: 13863 RVA: 0x000EC454 File Offset: 0x000EA654
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000EC472 File Offset: 0x000EA672
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000EC490 File Offset: 0x000EA690
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				try
				{
					return TimeSpan.Parse(text, culture);
				}
				catch (FormatException ex)
				{
					throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
					{
						(string)value,
						"TimeSpan"
					}), ex);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000EC504 File Offset: 0x000EA704
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is TimeSpan)
			{
				MethodInfo method = typeof(TimeSpan).GetMethod("Parse", new Type[] { typeof(string) });
				if (method != null)
				{
					return new InstanceDescriptor(method, new object[] { value.ToString() });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
