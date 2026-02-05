using System;

namespace System.ComponentModel
{
	// Token: 0x0200053D RID: 1341
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefaultPropertyAttribute : Attribute
	{
		// Token: 0x0600327E RID: 12926 RVA: 0x000E2073 File Offset: 0x000E0273
		public DefaultPropertyAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x0600327F RID: 12927 RVA: 0x000E2082 File Offset: 0x000E0282
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x000E208C File Offset: 0x000E028C
		public override bool Equals(object obj)
		{
			DefaultPropertyAttribute defaultPropertyAttribute = obj as DefaultPropertyAttribute;
			return defaultPropertyAttribute != null && defaultPropertyAttribute.Name == this.name;
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x000E20B6 File Offset: 0x000E02B6
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04002974 RID: 10612
		private readonly string name;

		// Token: 0x04002975 RID: 10613
		public static readonly DefaultPropertyAttribute Default = new DefaultPropertyAttribute(null);
	}
}
