using System;

namespace System.ComponentModel
{
	// Token: 0x0200058B RID: 1419
	[AttributeUsage(AttributeTargets.All)]
	public sealed class LocalizableAttribute : Attribute
	{
		// Token: 0x0600344C RID: 13388 RVA: 0x000E49FA File Offset: 0x000E2BFA
		public LocalizableAttribute(bool isLocalizable)
		{
			this.isLocalizable = isLocalizable;
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x0600344D RID: 13389 RVA: 0x000E4A09 File Offset: 0x000E2C09
		public bool IsLocalizable
		{
			get
			{
				return this.isLocalizable;
			}
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000E4A11 File Offset: 0x000E2C11
		public override bool IsDefaultAttribute()
		{
			return this.IsLocalizable == LocalizableAttribute.Default.IsLocalizable;
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x000E4A28 File Offset: 0x000E2C28
		public override bool Equals(object obj)
		{
			LocalizableAttribute localizableAttribute = obj as LocalizableAttribute;
			return localizableAttribute != null && localizableAttribute.IsLocalizable == this.isLocalizable;
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000E4A4F File Offset: 0x000E2C4F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040029E0 RID: 10720
		private bool isLocalizable;

		// Token: 0x040029E1 RID: 10721
		public static readonly LocalizableAttribute Yes = new LocalizableAttribute(true);

		// Token: 0x040029E2 RID: 10722
		public static readonly LocalizableAttribute No = new LocalizableAttribute(false);

		// Token: 0x040029E3 RID: 10723
		public static readonly LocalizableAttribute Default = LocalizableAttribute.No;
	}
}
