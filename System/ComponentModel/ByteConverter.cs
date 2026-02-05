using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000520 RID: 1312
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ByteConverter : BaseNumberConverter
	{
		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x000E000D File Offset: 0x000DE20D
		internal override Type TargetType
		{
			get
			{
				return typeof(byte);
			}
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x000E0019 File Offset: 0x000DE219
		internal override object FromString(string value, int radix)
		{
			return Convert.ToByte(value, radix);
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x000E0027 File Offset: 0x000DE227
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return byte.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x000E0036 File Offset: 0x000DE236
		internal override object FromString(string value, CultureInfo culture)
		{
			return byte.Parse(value, culture);
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x000E0044 File Offset: 0x000DE244
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((byte)value).ToString("G", formatInfo);
		}
	}
}
