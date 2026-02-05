using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000093 RID: 147
	internal sealed class ExprNamedArgumentSpecification : Expr
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x0001867E File Offset: 0x0001687E
		public ExprNamedArgumentSpecification(Name name, Expr value)
			: base(ExpressionKind.NamedArgumentSpecification)
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00018696 File Offset: 0x00016896
		public Name Name { get; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001869E File Offset: 0x0001689E
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x000186A8 File Offset: 0x000168A8
		public Expr Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				base.Type = value.Type;
			}
		}

		// Token: 0x04000554 RID: 1364
		private Expr _value;
	}
}
