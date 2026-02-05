using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000BE RID: 190
	internal sealed class MethWithInst : MethPropWithInst
	{
		// Token: 0x06000663 RID: 1635 RVA: 0x0001E106 File Offset: 0x0001C306
		public MethWithInst(MethodSymbol meth, AggregateType ats)
			: this(meth, ats, null)
		{
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001E111 File Offset: 0x0001C311
		public MethWithInst(MethodSymbol meth, AggregateType ats, TypeArray typeArgs)
		{
			base.Set(meth, ats, typeArgs);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001E122 File Offset: 0x0001C322
		public MethWithInst(MethPropWithInst mpwi)
		{
			base.Set(mpwi.Sym as MethodSymbol, mpwi.Ats, mpwi.TypeArgs);
		}
	}
}
