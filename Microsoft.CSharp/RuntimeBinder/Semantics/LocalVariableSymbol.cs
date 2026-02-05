using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000065 RID: 101
	internal sealed class LocalVariableSymbol : VariableSymbol
	{
		// Token: 0x0600037B RID: 891 RVA: 0x0001663E File Offset: 0x0001483E
		public void SetType(CType pType)
		{
			this.type = pType;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00016647 File Offset: 0x00014847
		public new CType GetType()
		{
			return this.type;
		}

		// Token: 0x040004A8 RID: 1192
		public ExprWrap wrap;

		// Token: 0x040004A9 RID: 1193
		public bool isThis;

		// Token: 0x040004AA RID: 1194
		public bool fUsedInAnonMeth;
	}
}
