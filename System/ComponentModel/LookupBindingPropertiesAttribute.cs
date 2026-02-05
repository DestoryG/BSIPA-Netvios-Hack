using System;

namespace System.ComponentModel
{
	// Token: 0x0200058C RID: 1420
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class LookupBindingPropertiesAttribute : Attribute
	{
		// Token: 0x06003452 RID: 13394 RVA: 0x000E4A79 File Offset: 0x000E2C79
		public LookupBindingPropertiesAttribute()
		{
			this.dataSource = null;
			this.displayMember = null;
			this.valueMember = null;
			this.lookupMember = null;
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x000E4A9D File Offset: 0x000E2C9D
		public LookupBindingPropertiesAttribute(string dataSource, string displayMember, string valueMember, string lookupMember)
		{
			this.dataSource = dataSource;
			this.displayMember = displayMember;
			this.valueMember = valueMember;
			this.lookupMember = lookupMember;
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06003454 RID: 13396 RVA: 0x000E4AC2 File Offset: 0x000E2CC2
		public string DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06003455 RID: 13397 RVA: 0x000E4ACA File Offset: 0x000E2CCA
		public string DisplayMember
		{
			get
			{
				return this.displayMember;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x000E4AD2 File Offset: 0x000E2CD2
		public string ValueMember
		{
			get
			{
				return this.valueMember;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06003457 RID: 13399 RVA: 0x000E4ADA File Offset: 0x000E2CDA
		public string LookupMember
		{
			get
			{
				return this.lookupMember;
			}
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x000E4AE4 File Offset: 0x000E2CE4
		public override bool Equals(object obj)
		{
			LookupBindingPropertiesAttribute lookupBindingPropertiesAttribute = obj as LookupBindingPropertiesAttribute;
			return lookupBindingPropertiesAttribute != null && lookupBindingPropertiesAttribute.DataSource == this.dataSource && lookupBindingPropertiesAttribute.displayMember == this.displayMember && lookupBindingPropertiesAttribute.valueMember == this.valueMember && lookupBindingPropertiesAttribute.lookupMember == this.lookupMember;
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x000E4B47 File Offset: 0x000E2D47
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040029E4 RID: 10724
		private readonly string dataSource;

		// Token: 0x040029E5 RID: 10725
		private readonly string displayMember;

		// Token: 0x040029E6 RID: 10726
		private readonly string valueMember;

		// Token: 0x040029E7 RID: 10727
		private readonly string lookupMember;

		// Token: 0x040029E8 RID: 10728
		public static readonly LookupBindingPropertiesAttribute Default = new LookupBindingPropertiesAttribute();
	}
}
