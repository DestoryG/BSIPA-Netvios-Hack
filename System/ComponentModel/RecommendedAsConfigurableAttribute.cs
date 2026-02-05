using System;

namespace System.ComponentModel
{
	// Token: 0x020005A0 RID: 1440
	[AttributeUsage(AttributeTargets.Property)]
	[Obsolete("Use System.ComponentModel.SettingsBindableAttribute instead to work with the new settings model.")]
	public class RecommendedAsConfigurableAttribute : Attribute
	{
		// Token: 0x0600358C RID: 13708 RVA: 0x000E8917 File Offset: 0x000E6B17
		public RecommendedAsConfigurableAttribute(bool recommendedAsConfigurable)
		{
			this.recommendedAsConfigurable = recommendedAsConfigurable;
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x0600358D RID: 13709 RVA: 0x000E8926 File Offset: 0x000E6B26
		public bool RecommendedAsConfigurable
		{
			get
			{
				return this.recommendedAsConfigurable;
			}
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000E8930 File Offset: 0x000E6B30
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RecommendedAsConfigurableAttribute recommendedAsConfigurableAttribute = obj as RecommendedAsConfigurableAttribute;
			return recommendedAsConfigurableAttribute != null && recommendedAsConfigurableAttribute.RecommendedAsConfigurable == this.recommendedAsConfigurable;
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000E895D File Offset: 0x000E6B5D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x000E8965 File Offset: 0x000E6B65
		public override bool IsDefaultAttribute()
		{
			return !this.recommendedAsConfigurable;
		}

		// Token: 0x04002A4B RID: 10827
		private bool recommendedAsConfigurable;

		// Token: 0x04002A4C RID: 10828
		public static readonly RecommendedAsConfigurableAttribute No = new RecommendedAsConfigurableAttribute(false);

		// Token: 0x04002A4D RID: 10829
		public static readonly RecommendedAsConfigurableAttribute Yes = new RecommendedAsConfigurableAttribute(true);

		// Token: 0x04002A4E RID: 10830
		public static readonly RecommendedAsConfigurableAttribute Default = RecommendedAsConfigurableAttribute.No;
	}
}
