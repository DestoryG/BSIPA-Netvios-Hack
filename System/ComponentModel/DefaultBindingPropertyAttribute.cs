using System;

namespace System.ComponentModel
{
	// Token: 0x0200053B RID: 1339
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefaultBindingPropertyAttribute : Attribute
	{
		// Token: 0x06003273 RID: 12915 RVA: 0x000E1FB5 File Offset: 0x000E01B5
		public DefaultBindingPropertyAttribute()
		{
			this.name = null;
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x000E1FC4 File Offset: 0x000E01C4
		public DefaultBindingPropertyAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x000E1FD3 File Offset: 0x000E01D3
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000E1FDC File Offset: 0x000E01DC
		public override bool Equals(object obj)
		{
			DefaultBindingPropertyAttribute defaultBindingPropertyAttribute = obj as DefaultBindingPropertyAttribute;
			return defaultBindingPropertyAttribute != null && defaultBindingPropertyAttribute.Name == this.name;
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x000E2006 File Offset: 0x000E0206
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04002970 RID: 10608
		private readonly string name;

		// Token: 0x04002971 RID: 10609
		public static readonly DefaultBindingPropertyAttribute Default = new DefaultBindingPropertyAttribute();
	}
}
