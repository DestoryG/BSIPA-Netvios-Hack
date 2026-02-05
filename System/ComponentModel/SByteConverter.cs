using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005AA RID: 1450
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class SByteConverter : BaseNumberConverter
	{
		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x0600360F RID: 13839 RVA: 0x000EC264 File Offset: 0x000EA464
		internal override Type TargetType
		{
			get
			{
				return typeof(sbyte);
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000EC270 File Offset: 0x000EA470
		internal override object FromString(string value, int radix)
		{
			return Convert.ToSByte(value, radix);
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000EC27E File Offset: 0x000EA47E
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return sbyte.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000EC28D File Offset: 0x000EA48D
		internal override object FromString(string value, CultureInfo culture)
		{
			return sbyte.Parse(value, culture);
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000EC29C File Offset: 0x000EA49C
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((sbyte)value).ToString("G", formatInfo);
		}
	}
}
