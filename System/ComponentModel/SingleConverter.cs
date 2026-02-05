using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005AC RID: 1452
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class SingleConverter : BaseNumberConverter
	{
		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x0600361A RID: 13850 RVA: 0x000EC329 File Offset: 0x000EA529
		internal override bool AllowHex
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x000EC32C File Offset: 0x000EA52C
		internal override Type TargetType
		{
			get
			{
				return typeof(float);
			}
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x000EC338 File Offset: 0x000EA538
		internal override object FromString(string value, int radix)
		{
			return Convert.ToSingle(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000EC34A File Offset: 0x000EA54A
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return float.Parse(value, NumberStyles.Float, formatInfo);
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000EC35D File Offset: 0x000EA55D
		internal override object FromString(string value, CultureInfo culture)
		{
			return float.Parse(value, culture);
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000EC36C File Offset: 0x000EA56C
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((float)value).ToString("R", formatInfo);
		}
	}
}
