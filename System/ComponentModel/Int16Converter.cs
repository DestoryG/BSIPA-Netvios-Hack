using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200056E RID: 1390
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class Int16Converter : BaseNumberConverter
	{
		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x000E3D18 File Offset: 0x000E1F18
		internal override Type TargetType
		{
			get
			{
				return typeof(short);
			}
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x000E3D24 File Offset: 0x000E1F24
		internal override object FromString(string value, int radix)
		{
			return Convert.ToInt16(value, radix);
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000E3D32 File Offset: 0x000E1F32
		internal override object FromString(string value, CultureInfo culture)
		{
			return short.Parse(value, culture);
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x000E3D40 File Offset: 0x000E1F40
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return short.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x000E3D50 File Offset: 0x000E1F50
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((short)value).ToString("G", formatInfo);
		}
	}
}
