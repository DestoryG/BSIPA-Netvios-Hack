using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200051E RID: 1310
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class BooleanConverter : TypeConverter
	{
		// Token: 0x060031BA RID: 12730 RVA: 0x000DFEAC File Offset: 0x000DE0AC
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000DFECC File Offset: 0x000DE0CC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				try
				{
					return bool.Parse(text);
				}
				catch (FormatException ex)
				{
					throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
					{
						(string)value,
						"Boolean"
					}), ex);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x000DFF40 File Offset: 0x000DE140
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (BooleanConverter.values == null)
			{
				BooleanConverter.values = new TypeConverter.StandardValuesCollection(new object[] { true, false });
			}
			return BooleanConverter.values;
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x000DFF76 File Offset: 0x000DE176
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000DFF79 File Offset: 0x000DE179
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04002931 RID: 10545
		private static volatile TypeConverter.StandardValuesCollection values;
	}
}
