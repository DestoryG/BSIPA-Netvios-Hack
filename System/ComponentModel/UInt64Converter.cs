using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005BA RID: 1466
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class UInt64Converter : BaseNumberConverter
	{
		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x060036F7 RID: 14071 RVA: 0x000EF651 File Offset: 0x000ED851
		internal override Type TargetType
		{
			get
			{
				return typeof(ulong);
			}
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x000EF65D File Offset: 0x000ED85D
		internal override object FromString(string value, int radix)
		{
			return Convert.ToUInt64(value, radix);
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x000EF66B File Offset: 0x000ED86B
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return ulong.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x000EF67A File Offset: 0x000ED87A
		internal override object FromString(string value, CultureInfo culture)
		{
			return ulong.Parse(value, culture);
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x000EF688 File Offset: 0x000ED888
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((ulong)value).ToString("G", formatInfo);
		}
	}
}
