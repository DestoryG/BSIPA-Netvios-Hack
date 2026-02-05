using System;
using System.ComponentModel;

namespace System
{
	// Token: 0x02000065 RID: 101
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x0001E98B File Offset: 0x0001CB8B
		public SRCategoryAttribute(string category)
			: base(category)
		{
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001E994 File Offset: 0x0001CB94
		protected override string GetLocalizedString(string value)
		{
			return SR.GetString(value);
		}
	}
}
