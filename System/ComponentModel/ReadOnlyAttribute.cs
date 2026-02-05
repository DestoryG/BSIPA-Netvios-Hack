using System;

namespace System.ComponentModel
{
	// Token: 0x0200059F RID: 1439
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ReadOnlyAttribute : Attribute
	{
		// Token: 0x06003586 RID: 13702 RVA: 0x000E8894 File Offset: 0x000E6A94
		public ReadOnlyAttribute(bool isReadOnly)
		{
			this.isReadOnly = isReadOnly;
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06003587 RID: 13703 RVA: 0x000E88A3 File Offset: 0x000E6AA3
		public bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x000E88AC File Offset: 0x000E6AAC
		public override bool Equals(object value)
		{
			if (this == value)
			{
				return true;
			}
			ReadOnlyAttribute readOnlyAttribute = value as ReadOnlyAttribute;
			return readOnlyAttribute != null && readOnlyAttribute.IsReadOnly == this.IsReadOnly;
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000E88D9 File Offset: 0x000E6AD9
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000E88E1 File Offset: 0x000E6AE1
		public override bool IsDefaultAttribute()
		{
			return this.IsReadOnly == ReadOnlyAttribute.Default.IsReadOnly;
		}

		// Token: 0x04002A47 RID: 10823
		private bool isReadOnly;

		// Token: 0x04002A48 RID: 10824
		public static readonly ReadOnlyAttribute Yes = new ReadOnlyAttribute(true);

		// Token: 0x04002A49 RID: 10825
		public static readonly ReadOnlyAttribute No = new ReadOnlyAttribute(false);

		// Token: 0x04002A4A RID: 10826
		public static readonly ReadOnlyAttribute Default = ReadOnlyAttribute.No;
	}
}
