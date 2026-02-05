using System;

namespace System.ComponentModel
{
	// Token: 0x020005C0 RID: 1472
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ParenthesizePropertyNameAttribute : Attribute
	{
		// Token: 0x0600371E RID: 14110 RVA: 0x000EFA57 File Offset: 0x000EDC57
		public ParenthesizePropertyNameAttribute()
			: this(false)
		{
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000EFA60 File Offset: 0x000EDC60
		public ParenthesizePropertyNameAttribute(bool needParenthesis)
		{
			this.needParenthesis = needParenthesis;
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06003720 RID: 14112 RVA: 0x000EFA6F File Offset: 0x000EDC6F
		public bool NeedParenthesis
		{
			get
			{
				return this.needParenthesis;
			}
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000EFA77 File Offset: 0x000EDC77
		public override bool Equals(object o)
		{
			return o is ParenthesizePropertyNameAttribute && ((ParenthesizePropertyNameAttribute)o).NeedParenthesis == this.needParenthesis;
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000EFA96 File Offset: 0x000EDC96
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000EFA9E File Offset: 0x000EDC9E
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ParenthesizePropertyNameAttribute.Default);
		}

		// Token: 0x04002AC5 RID: 10949
		public static readonly ParenthesizePropertyNameAttribute Default = new ParenthesizePropertyNameAttribute();

		// Token: 0x04002AC6 RID: 10950
		private bool needParenthesis;
	}
}
