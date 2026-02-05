using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005B8 RID: 1464
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class UInt16Converter : BaseNumberConverter
	{
		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x060036EB RID: 14059 RVA: 0x000EF58F File Offset: 0x000ED78F
		internal override Type TargetType
		{
			get
			{
				return typeof(ushort);
			}
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x000EF59B File Offset: 0x000ED79B
		internal override object FromString(string value, int radix)
		{
			return Convert.ToUInt16(value, radix);
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x000EF5A9 File Offset: 0x000ED7A9
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return ushort.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x000EF5B8 File Offset: 0x000ED7B8
		internal override object FromString(string value, CultureInfo culture)
		{
			return ushort.Parse(value, culture);
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x000EF5C8 File Offset: 0x000ED7C8
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((ushort)value).ToString("G", formatInfo);
		}
	}
}
