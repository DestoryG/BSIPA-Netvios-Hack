using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200005F RID: 95
	internal sealed class SubstContext
	{
		// Token: 0x06000338 RID: 824 RVA: 0x000161A9 File Offset: 0x000143A9
		public SubstContext(TypeArray typeArgsCls, TypeArray typeArgsMeth, SubstTypeFlags grfst)
		{
			this.Init(typeArgsCls, typeArgsMeth, grfst);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000161BA File Offset: 0x000143BA
		public SubstContext(AggregateType type)
			: this(type, null, SubstTypeFlags.NormNone)
		{
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000161C5 File Offset: 0x000143C5
		public SubstContext(AggregateType type, TypeArray typeArgsMeth)
			: this(type, typeArgsMeth, SubstTypeFlags.NormNone)
		{
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000161D0 File Offset: 0x000143D0
		private SubstContext(AggregateType type, TypeArray typeArgsMeth, SubstTypeFlags grfst)
		{
			this.Init((type != null) ? type.GetTypeArgsAll() : null, typeArgsMeth, grfst);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000161EC File Offset: 0x000143EC
		public SubstContext(CType[] prgtypeCls, int ctypeCls, CType[] prgtypeMeth, int ctypeMeth)
			: this(prgtypeCls, ctypeCls, prgtypeMeth, ctypeMeth, SubstTypeFlags.NormNone)
		{
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000161FA File Offset: 0x000143FA
		private SubstContext(CType[] prgtypeCls, int ctypeCls, CType[] prgtypeMeth, int ctypeMeth, SubstTypeFlags grfst)
		{
			this.prgtypeCls = prgtypeCls;
			this.ctypeCls = ctypeCls;
			this.prgtypeMeth = prgtypeMeth;
			this.ctypeMeth = ctypeMeth;
			this.grfst = grfst;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00016227 File Offset: 0x00014427
		public bool FNop()
		{
			return this.ctypeCls == 0 && this.ctypeMeth == 0 && (this.grfst & SubstTypeFlags.NormAll) == SubstTypeFlags.NormNone;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00016248 File Offset: 0x00014448
		private void Init(TypeArray typeArgsCls, TypeArray typeArgsMeth, SubstTypeFlags grfst)
		{
			if (typeArgsCls != null)
			{
				this.ctypeCls = typeArgsCls.Count;
				this.prgtypeCls = typeArgsCls.Items;
			}
			else
			{
				this.ctypeCls = 0;
				this.prgtypeCls = null;
			}
			if (typeArgsMeth != null)
			{
				this.ctypeMeth = typeArgsMeth.Count;
				this.prgtypeMeth = typeArgsMeth.Items;
			}
			else
			{
				this.ctypeMeth = 0;
				this.prgtypeMeth = null;
			}
			this.grfst = grfst;
		}

		// Token: 0x04000484 RID: 1156
		public CType[] prgtypeCls;

		// Token: 0x04000485 RID: 1157
		public int ctypeCls;

		// Token: 0x04000486 RID: 1158
		public CType[] prgtypeMeth;

		// Token: 0x04000487 RID: 1159
		public int ctypeMeth;

		// Token: 0x04000488 RID: 1160
		public SubstTypeFlags grfst;
	}
}
