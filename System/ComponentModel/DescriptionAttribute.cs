using System;

namespace System.ComponentModel
{
	// Token: 0x02000540 RID: 1344
	[AttributeUsage(AttributeTargets.All)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x0600329D RID: 12957 RVA: 0x000E22C3 File Offset: 0x000E04C3
		public DescriptionAttribute()
			: this(string.Empty)
		{
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x000E22D0 File Offset: 0x000E04D0
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x0600329F RID: 12959 RVA: 0x000E22DF File Offset: 0x000E04DF
		public virtual string Description
		{
			get
			{
				return this.DescriptionValue;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x000E22E7 File Offset: 0x000E04E7
		// (set) Token: 0x060032A1 RID: 12961 RVA: 0x000E22EF File Offset: 0x000E04EF
		protected string DescriptionValue
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x000E22F8 File Offset: 0x000E04F8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DescriptionAttribute descriptionAttribute = obj as DescriptionAttribute;
			return descriptionAttribute != null && descriptionAttribute.Description == this.Description;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x000E2328 File Offset: 0x000E0528
		public override int GetHashCode()
		{
			return this.Description.GetHashCode();
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x000E2335 File Offset: 0x000E0535
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DescriptionAttribute.Default);
		}

		// Token: 0x04002978 RID: 10616
		public static readonly DescriptionAttribute Default = new DescriptionAttribute();

		// Token: 0x04002979 RID: 10617
		private string description;
	}
}
