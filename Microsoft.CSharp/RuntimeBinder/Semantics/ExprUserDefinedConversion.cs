using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200009B RID: 155
	internal sealed class ExprUserDefinedConversion : Expr
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x0001886F File Offset: 0x00016A6F
		public ExprUserDefinedConversion(Expr argument, Expr call, MethWithInst method)
			: base(ExpressionKind.UserDefinedConversion)
		{
			this.Argument = argument;
			this.UserDefinedCall = call;
			this.UserDefinedCallMethod = method;
			if (call.HasError)
			{
				base.SetError();
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0001889C File Offset: 0x00016A9C
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x000188A4 File Offset: 0x00016AA4
		public Expr Argument { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x000188AD File Offset: 0x00016AAD
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x000188B8 File Offset: 0x00016AB8
		public Expr UserDefinedCall
		{
			get
			{
				return this._userDefinedCall;
			}
			set
			{
				this._userDefinedCall = value;
				base.Type = value.Type;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x000188DA File Offset: 0x00016ADA
		public MethWithInst UserDefinedCallMethod { get; }

		// Token: 0x04000561 RID: 1377
		private Expr _userDefinedCall;
	}
}
