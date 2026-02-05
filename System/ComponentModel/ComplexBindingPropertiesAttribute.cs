using System;

namespace System.ComponentModel
{
	// Token: 0x02000529 RID: 1321
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ComplexBindingPropertiesAttribute : Attribute
	{
		// Token: 0x060031F8 RID: 12792 RVA: 0x000E04A1 File Offset: 0x000DE6A1
		public ComplexBindingPropertiesAttribute()
		{
			this.dataSource = null;
			this.dataMember = null;
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000E04B7 File Offset: 0x000DE6B7
		public ComplexBindingPropertiesAttribute(string dataSource)
		{
			this.dataSource = dataSource;
			this.dataMember = null;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000E04CD File Offset: 0x000DE6CD
		public ComplexBindingPropertiesAttribute(string dataSource, string dataMember)
		{
			this.dataSource = dataSource;
			this.dataMember = dataMember;
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x000E04E3 File Offset: 0x000DE6E3
		public string DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x060031FC RID: 12796 RVA: 0x000E04EB File Offset: 0x000DE6EB
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x000E04F4 File Offset: 0x000DE6F4
		public override bool Equals(object obj)
		{
			ComplexBindingPropertiesAttribute complexBindingPropertiesAttribute = obj as ComplexBindingPropertiesAttribute;
			return complexBindingPropertiesAttribute != null && complexBindingPropertiesAttribute.DataSource == this.dataSource && complexBindingPropertiesAttribute.DataMember == this.dataMember;
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x000E0531 File Offset: 0x000DE731
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400294D RID: 10573
		private readonly string dataSource;

		// Token: 0x0400294E RID: 10574
		private readonly string dataMember;

		// Token: 0x0400294F RID: 10575
		public static readonly ComplexBindingPropertiesAttribute Default = new ComplexBindingPropertiesAttribute();
	}
}
