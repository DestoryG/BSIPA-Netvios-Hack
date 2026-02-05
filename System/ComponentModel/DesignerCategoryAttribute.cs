using System;

namespace System.ComponentModel
{
	// Token: 0x02000542 RID: 1346
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class DesignerCategoryAttribute : Attribute
	{
		// Token: 0x060032B0 RID: 12976 RVA: 0x000E24F4 File Offset: 0x000E06F4
		public DesignerCategoryAttribute()
		{
			this.category = string.Empty;
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x000E2507 File Offset: 0x000E0707
		public DesignerCategoryAttribute(string category)
		{
			this.category = category;
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x000E2516 File Offset: 0x000E0716
		public string Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x000E251E File Offset: 0x000E071E
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					this.typeId = base.GetType().FullName + this.Category;
				}
				return this.typeId;
			}
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x000E254C File Offset: 0x000E074C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignerCategoryAttribute designerCategoryAttribute = obj as DesignerCategoryAttribute;
			return designerCategoryAttribute != null && designerCategoryAttribute.category == this.category;
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x000E257C File Offset: 0x000E077C
		public override int GetHashCode()
		{
			return this.category.GetHashCode();
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000E2589 File Offset: 0x000E0789
		public override bool IsDefaultAttribute()
		{
			return this.category.Equals(DesignerCategoryAttribute.Default.Category);
		}

		// Token: 0x0400297D RID: 10621
		private string category;

		// Token: 0x0400297E RID: 10622
		private string typeId;

		// Token: 0x0400297F RID: 10623
		public static readonly DesignerCategoryAttribute Component = new DesignerCategoryAttribute("Component");

		// Token: 0x04002980 RID: 10624
		public static readonly DesignerCategoryAttribute Default = new DesignerCategoryAttribute();

		// Token: 0x04002981 RID: 10625
		public static readonly DesignerCategoryAttribute Form = new DesignerCategoryAttribute("Form");

		// Token: 0x04002982 RID: 10626
		public static readonly DesignerCategoryAttribute Generic = new DesignerCategoryAttribute("Designer");
	}
}
