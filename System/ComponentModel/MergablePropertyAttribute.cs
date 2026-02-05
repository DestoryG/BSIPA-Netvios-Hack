using System;

namespace System.ComponentModel
{
	// Token: 0x02000591 RID: 1425
	[AttributeUsage(AttributeTargets.All)]
	public sealed class MergablePropertyAttribute : Attribute
	{
		// Token: 0x060034F2 RID: 13554 RVA: 0x000E7256 File Offset: 0x000E5456
		public MergablePropertyAttribute(bool allowMerge)
		{
			this.allowMerge = allowMerge;
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x060034F3 RID: 13555 RVA: 0x000E7265 File Offset: 0x000E5465
		public bool AllowMerge
		{
			get
			{
				return this.allowMerge;
			}
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000E7270 File Offset: 0x000E5470
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			MergablePropertyAttribute mergablePropertyAttribute = obj as MergablePropertyAttribute;
			return mergablePropertyAttribute != null && mergablePropertyAttribute.AllowMerge == this.allowMerge;
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x000E729D File Offset: 0x000E549D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000E72A5 File Offset: 0x000E54A5
		public override bool IsDefaultAttribute()
		{
			return this.Equals(MergablePropertyAttribute.Default);
		}

		// Token: 0x04002A25 RID: 10789
		public static readonly MergablePropertyAttribute Yes = new MergablePropertyAttribute(true);

		// Token: 0x04002A26 RID: 10790
		public static readonly MergablePropertyAttribute No = new MergablePropertyAttribute(false);

		// Token: 0x04002A27 RID: 10791
		public static readonly MergablePropertyAttribute Default = MergablePropertyAttribute.Yes;

		// Token: 0x04002A28 RID: 10792
		private bool allowMerge;
	}
}
