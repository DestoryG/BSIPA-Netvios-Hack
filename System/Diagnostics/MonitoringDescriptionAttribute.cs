using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020004DC RID: 1244
	[AttributeUsage(AttributeTargets.All)]
	public class MonitoringDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06002EED RID: 12013 RVA: 0x000D2A7F File Offset: 0x000D0C7F
		public MonitoringDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06002EEE RID: 12014 RVA: 0x000D2A88 File Offset: 0x000D0C88
		public override string Description
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DescriptionValue = SR.GetString(base.Description);
				}
				return base.Description;
			}
		}

		// Token: 0x04002798 RID: 10136
		private bool replaced;
	}
}
