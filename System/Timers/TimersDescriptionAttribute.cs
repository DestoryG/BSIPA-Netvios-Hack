using System;
using System.ComponentModel;

namespace System.Timers
{
	// Token: 0x0200006E RID: 110
	[AttributeUsage(AttributeTargets.All)]
	public class TimersDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x0001F414 File Offset: 0x0001D614
		public TimersDescriptionAttribute(string description)
			: base(description)
		{
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001F41D File Offset: 0x0001D61D
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

		// Token: 0x04000BDC RID: 3036
		private bool replaced;
	}
}
