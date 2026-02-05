using System;

namespace System.ComponentModel
{
	// Token: 0x02000595 RID: 1429
	[AttributeUsage(AttributeTargets.All)]
	public sealed class PasswordPropertyTextAttribute : Attribute
	{
		// Token: 0x06003513 RID: 13587 RVA: 0x000E77E6 File Offset: 0x000E59E6
		public PasswordPropertyTextAttribute()
			: this(false)
		{
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x000E77EF File Offset: 0x000E59EF
		public PasswordPropertyTextAttribute(bool password)
		{
			this._password = password;
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x000E77FE File Offset: 0x000E59FE
		public bool Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000E7806 File Offset: 0x000E5A06
		public override bool Equals(object o)
		{
			return o is PasswordPropertyTextAttribute && ((PasswordPropertyTextAttribute)o).Password == this._password;
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000E7825 File Offset: 0x000E5A25
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000E782D File Offset: 0x000E5A2D
		public override bool IsDefaultAttribute()
		{
			return this.Equals(PasswordPropertyTextAttribute.Default);
		}

		// Token: 0x04002A2D RID: 10797
		public static readonly PasswordPropertyTextAttribute Yes = new PasswordPropertyTextAttribute(true);

		// Token: 0x04002A2E RID: 10798
		public static readonly PasswordPropertyTextAttribute No = new PasswordPropertyTextAttribute(false);

		// Token: 0x04002A2F RID: 10799
		public static readonly PasswordPropertyTextAttribute Default = PasswordPropertyTextAttribute.No;

		// Token: 0x04002A30 RID: 10800
		private bool _password;
	}
}
