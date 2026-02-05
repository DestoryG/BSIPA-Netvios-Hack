using System;

namespace System.ComponentModel
{
	// Token: 0x02000545 RID: 1349
	[AttributeUsage(AttributeTargets.All)]
	public sealed class DesignOnlyAttribute : Attribute
	{
		// Token: 0x060032BE RID: 12990 RVA: 0x000E265F File Offset: 0x000E085F
		public DesignOnlyAttribute(bool isDesignOnly)
		{
			this.isDesignOnly = isDesignOnly;
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x000E266E File Offset: 0x000E086E
		public bool IsDesignOnly
		{
			get
			{
				return this.isDesignOnly;
			}
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x000E2676 File Offset: 0x000E0876
		public override bool IsDefaultAttribute()
		{
			return this.IsDesignOnly == DesignOnlyAttribute.Default.IsDesignOnly;
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x000E268C File Offset: 0x000E088C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignOnlyAttribute designOnlyAttribute = obj as DesignOnlyAttribute;
			return designOnlyAttribute != null && designOnlyAttribute.isDesignOnly == this.isDesignOnly;
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x000E26B9 File Offset: 0x000E08B9
		public override int GetHashCode()
		{
			return this.isDesignOnly.GetHashCode();
		}

		// Token: 0x0400298C RID: 10636
		private bool isDesignOnly;

		// Token: 0x0400298D RID: 10637
		public static readonly DesignOnlyAttribute Yes = new DesignOnlyAttribute(true);

		// Token: 0x0400298E RID: 10638
		public static readonly DesignOnlyAttribute No = new DesignOnlyAttribute(false);

		// Token: 0x0400298F RID: 10639
		public static readonly DesignOnlyAttribute Default = DesignOnlyAttribute.No;
	}
}
