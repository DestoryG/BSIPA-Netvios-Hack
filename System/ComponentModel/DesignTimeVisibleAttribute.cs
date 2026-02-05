using System;

namespace System.ComponentModel
{
	// Token: 0x02000546 RID: 1350
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class DesignTimeVisibleAttribute : Attribute
	{
		// Token: 0x060032C4 RID: 12996 RVA: 0x000E26E8 File Offset: 0x000E08E8
		public DesignTimeVisibleAttribute(bool visible)
		{
			this.visible = visible;
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x000E26F7 File Offset: 0x000E08F7
		public DesignTimeVisibleAttribute()
		{
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x060032C6 RID: 12998 RVA: 0x000E26FF File Offset: 0x000E08FF
		public bool Visible
		{
			get
			{
				return this.visible;
			}
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x000E2708 File Offset: 0x000E0908
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignTimeVisibleAttribute designTimeVisibleAttribute = obj as DesignTimeVisibleAttribute;
			return designTimeVisibleAttribute != null && designTimeVisibleAttribute.Visible == this.visible;
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x000E2735 File Offset: 0x000E0935
		public override int GetHashCode()
		{
			return typeof(DesignTimeVisibleAttribute).GetHashCode() ^ (this.visible ? (-1) : 0);
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x000E2753 File Offset: 0x000E0953
		public override bool IsDefaultAttribute()
		{
			return this.Visible == DesignTimeVisibleAttribute.Default.Visible;
		}

		// Token: 0x04002990 RID: 10640
		private bool visible;

		// Token: 0x04002991 RID: 10641
		public static readonly DesignTimeVisibleAttribute Yes = new DesignTimeVisibleAttribute(true);

		// Token: 0x04002992 RID: 10642
		public static readonly DesignTimeVisibleAttribute No = new DesignTimeVisibleAttribute(false);

		// Token: 0x04002993 RID: 10643
		public static readonly DesignTimeVisibleAttribute Default = DesignTimeVisibleAttribute.Yes;
	}
}
