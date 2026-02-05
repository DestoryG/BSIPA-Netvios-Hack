using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000570 RID: 1392
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class Int64Converter : BaseNumberConverter
	{
		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x060033C6 RID: 13254 RVA: 0x000E3DD9 File Offset: 0x000E1FD9
		internal override Type TargetType
		{
			get
			{
				return typeof(long);
			}
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000E3DE5 File Offset: 0x000E1FE5
		internal override object FromString(string value, int radix)
		{
			return Convert.ToInt64(value, radix);
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x000E3DF3 File Offset: 0x000E1FF3
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return long.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x000E3E02 File Offset: 0x000E2002
		internal override object FromString(string value, CultureInfo culture)
		{
			return long.Parse(value, culture);
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x000E3E10 File Offset: 0x000E2010
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((long)value).ToString("G", formatInfo);
		}
	}
}
