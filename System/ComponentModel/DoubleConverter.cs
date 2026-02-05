using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000548 RID: 1352
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DoubleConverter : BaseNumberConverter
	{
		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x000E2816 File Offset: 0x000E0A16
		internal override bool AllowHex
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x000E2819 File Offset: 0x000E0A19
		internal override Type TargetType
		{
			get
			{
				return typeof(double);
			}
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000E2825 File Offset: 0x000E0A25
		internal override object FromString(string value, int radix)
		{
			return Convert.ToDouble(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000E2837 File Offset: 0x000E0A37
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return double.Parse(value, NumberStyles.Float, formatInfo);
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000E284A File Offset: 0x000E0A4A
		internal override object FromString(string value, CultureInfo culture)
		{
			return double.Parse(value, culture);
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x000E2858 File Offset: 0x000E0A58
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((double)value).ToString("R", formatInfo);
		}
	}
}
