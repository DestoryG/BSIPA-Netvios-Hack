using System;

namespace System.ComponentModel
{
	// Token: 0x02000544 RID: 1348
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
	public sealed class DesignerSerializationVisibilityAttribute : Attribute
	{
		// Token: 0x060032B8 RID: 12984 RVA: 0x000E25D9 File Offset: 0x000E07D9
		public DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility visibility)
		{
			this.visibility = visibility;
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x000E25E8 File Offset: 0x000E07E8
		public DesignerSerializationVisibility Visibility
		{
			get
			{
				return this.visibility;
			}
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000E25F0 File Offset: 0x000E07F0
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignerSerializationVisibilityAttribute designerSerializationVisibilityAttribute = obj as DesignerSerializationVisibilityAttribute;
			return designerSerializationVisibilityAttribute != null && designerSerializationVisibilityAttribute.Visibility == this.visibility;
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000E261D File Offset: 0x000E081D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x000E2625 File Offset: 0x000E0825
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DesignerSerializationVisibilityAttribute.Default);
		}

		// Token: 0x04002987 RID: 10631
		public static readonly DesignerSerializationVisibilityAttribute Content = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content);

		// Token: 0x04002988 RID: 10632
		public static readonly DesignerSerializationVisibilityAttribute Hidden = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden);

		// Token: 0x04002989 RID: 10633
		public static readonly DesignerSerializationVisibilityAttribute Visible = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible);

		// Token: 0x0400298A RID: 10634
		public static readonly DesignerSerializationVisibilityAttribute Default = DesignerSerializationVisibilityAttribute.Visible;

		// Token: 0x0400298B RID: 10635
		private DesignerSerializationVisibility visibility;
	}
}
