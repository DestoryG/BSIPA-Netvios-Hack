using System;
using System.ComponentModel;

namespace System.IO
{
	// Token: 0x02000402 RID: 1026
	[AttributeUsage(AttributeTargets.All)]
	public class IODescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06002698 RID: 9880 RVA: 0x000B1A0E File Offset: 0x000AFC0E
		public IODescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06002699 RID: 9881 RVA: 0x000B1A17 File Offset: 0x000AFC17
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

		// Token: 0x040020D6 RID: 8406
		private bool replaced;
	}
}
