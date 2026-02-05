using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005B9 RID: 1465
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class UInt32Converter : BaseNumberConverter
	{
		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x060036F1 RID: 14065 RVA: 0x000EF5F1 File Offset: 0x000ED7F1
		internal override Type TargetType
		{
			get
			{
				return typeof(uint);
			}
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x000EF5FD File Offset: 0x000ED7FD
		internal override object FromString(string value, int radix)
		{
			return Convert.ToUInt32(value, radix);
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x000EF60B File Offset: 0x000ED80B
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return uint.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x000EF61A File Offset: 0x000ED81A
		internal override object FromString(string value, CultureInfo culture)
		{
			return uint.Parse(value, culture);
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x000EF628 File Offset: 0x000ED828
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((uint)value).ToString("G", formatInfo);
		}
	}
}
