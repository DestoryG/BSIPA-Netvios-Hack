using System;

namespace System.ComponentModel
{
	// Token: 0x020005C4 RID: 1476
	[AttributeUsage(AttributeTargets.All)]
	public sealed class RefreshPropertiesAttribute : Attribute
	{
		// Token: 0x06003733 RID: 14131 RVA: 0x000EFE55 File Offset: 0x000EE055
		public RefreshPropertiesAttribute(RefreshProperties refresh)
		{
			this.refresh = refresh;
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003734 RID: 14132 RVA: 0x000EFE64 File Offset: 0x000EE064
		public RefreshProperties RefreshProperties
		{
			get
			{
				return this.refresh;
			}
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x000EFE6C File Offset: 0x000EE06C
		public override bool Equals(object value)
		{
			return value is RefreshPropertiesAttribute && ((RefreshPropertiesAttribute)value).RefreshProperties == this.refresh;
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x000EFE8B File Offset: 0x000EE08B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x000EFE93 File Offset: 0x000EE093
		public override bool IsDefaultAttribute()
		{
			return this.Equals(RefreshPropertiesAttribute.Default);
		}

		// Token: 0x04002AD3 RID: 10963
		public static readonly RefreshPropertiesAttribute All = new RefreshPropertiesAttribute(RefreshProperties.All);

		// Token: 0x04002AD4 RID: 10964
		public static readonly RefreshPropertiesAttribute Repaint = new RefreshPropertiesAttribute(RefreshProperties.Repaint);

		// Token: 0x04002AD5 RID: 10965
		public static readonly RefreshPropertiesAttribute Default = new RefreshPropertiesAttribute(RefreshProperties.None);

		// Token: 0x04002AD6 RID: 10966
		private RefreshProperties refresh;
	}
}
