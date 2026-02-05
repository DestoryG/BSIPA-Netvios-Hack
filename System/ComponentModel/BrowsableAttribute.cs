using System;

namespace System.ComponentModel
{
	// Token: 0x0200051F RID: 1311
	[AttributeUsage(AttributeTargets.All)]
	public sealed class BrowsableAttribute : Attribute
	{
		// Token: 0x060031C0 RID: 12736 RVA: 0x000DFF84 File Offset: 0x000DE184
		public BrowsableAttribute(bool browsable)
		{
			this.browsable = browsable;
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060031C1 RID: 12737 RVA: 0x000DFF9A File Offset: 0x000DE19A
		public bool Browsable
		{
			get
			{
				return this.browsable;
			}
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x000DFFA4 File Offset: 0x000DE1A4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			BrowsableAttribute browsableAttribute = obj as BrowsableAttribute;
			return browsableAttribute != null && browsableAttribute.Browsable == this.browsable;
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x000DFFD1 File Offset: 0x000DE1D1
		public override int GetHashCode()
		{
			return this.browsable.GetHashCode();
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x000DFFDE File Offset: 0x000DE1DE
		public override bool IsDefaultAttribute()
		{
			return this.Equals(BrowsableAttribute.Default);
		}

		// Token: 0x04002932 RID: 10546
		public static readonly BrowsableAttribute Yes = new BrowsableAttribute(true);

		// Token: 0x04002933 RID: 10547
		public static readonly BrowsableAttribute No = new BrowsableAttribute(false);

		// Token: 0x04002934 RID: 10548
		public static readonly BrowsableAttribute Default = BrowsableAttribute.Yes;

		// Token: 0x04002935 RID: 10549
		private bool browsable = true;
	}
}
