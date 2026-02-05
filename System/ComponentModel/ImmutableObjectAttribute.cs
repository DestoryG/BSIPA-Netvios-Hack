using System;

namespace System.ComponentModel
{
	// Token: 0x02000565 RID: 1381
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ImmutableObjectAttribute : Attribute
	{
		// Token: 0x060033A0 RID: 13216 RVA: 0x000E3BFF File Offset: 0x000E1DFF
		public ImmutableObjectAttribute(bool immutable)
		{
			this.immutable = immutable;
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x000E3C15 File Offset: 0x000E1E15
		public bool Immutable
		{
			get
			{
				return this.immutable;
			}
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x000E3C20 File Offset: 0x000E1E20
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ImmutableObjectAttribute immutableObjectAttribute = obj as ImmutableObjectAttribute;
			return immutableObjectAttribute != null && immutableObjectAttribute.Immutable == this.immutable;
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x000E3C4D File Offset: 0x000E1E4D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x000E3C55 File Offset: 0x000E1E55
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ImmutableObjectAttribute.Default);
		}

		// Token: 0x040029B2 RID: 10674
		public static readonly ImmutableObjectAttribute Yes = new ImmutableObjectAttribute(true);

		// Token: 0x040029B3 RID: 10675
		public static readonly ImmutableObjectAttribute No = new ImmutableObjectAttribute(false);

		// Token: 0x040029B4 RID: 10676
		public static readonly ImmutableObjectAttribute Default = ImmutableObjectAttribute.No;

		// Token: 0x040029B5 RID: 10677
		private bool immutable = true;
	}
}
