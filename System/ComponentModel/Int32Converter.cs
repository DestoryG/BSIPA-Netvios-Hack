using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200056F RID: 1391
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class Int32Converter : BaseNumberConverter
	{
		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x000E3D79 File Offset: 0x000E1F79
		internal override Type TargetType
		{
			get
			{
				return typeof(int);
			}
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x000E3D85 File Offset: 0x000E1F85
		internal override object FromString(string value, int radix)
		{
			return Convert.ToInt32(value, radix);
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000E3D93 File Offset: 0x000E1F93
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return int.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000E3DA2 File Offset: 0x000E1FA2
		internal override object FromString(string value, CultureInfo culture)
		{
			return int.Parse(value, culture);
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x000E3DB0 File Offset: 0x000E1FB0
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((int)value).ToString("G", formatInfo);
		}
	}
}
