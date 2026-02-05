using System;
using System.ComponentModel;

namespace System
{
	// Token: 0x02000064 RID: 100
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000455 RID: 1109 RVA: 0x0001E95A File Offset: 0x0001CB5A
		public SRDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0001E963 File Offset: 0x0001CB63
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

		// Token: 0x04000534 RID: 1332
		private bool replaced;
	}
}
