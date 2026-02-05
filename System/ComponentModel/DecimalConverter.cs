using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200053A RID: 1338
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DecimalConverter : BaseNumberConverter
	{
		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x0600326A RID: 12906 RVA: 0x000E1E94 File Offset: 0x000E0094
		internal override bool AllowHex
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x0600326B RID: 12907 RVA: 0x000E1E97 File Offset: 0x000E0097
		internal override Type TargetType
		{
			get
			{
				return typeof(decimal);
			}
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x000E1EA3 File Offset: 0x000E00A3
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x000E1EC4 File Offset: 0x000E00C4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (!(destinationType == typeof(InstanceDescriptor)) || !(value is decimal))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			object[] array = new object[] { decimal.GetBits((decimal)value) };
			MemberInfo constructor = typeof(decimal).GetConstructor(new Type[] { typeof(int[]) });
			if (constructor != null)
			{
				return new InstanceDescriptor(constructor, array);
			}
			return null;
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000E1F57 File Offset: 0x000E0157
		internal override object FromString(string value, int radix)
		{
			return Convert.ToDecimal(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x000E1F69 File Offset: 0x000E0169
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return decimal.Parse(value, NumberStyles.Float, formatInfo);
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000E1F7C File Offset: 0x000E017C
		internal override object FromString(string value, CultureInfo culture)
		{
			return decimal.Parse(value, culture);
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000E1F8C File Offset: 0x000E018C
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((decimal)value).ToString("G", formatInfo);
		}
	}
}
