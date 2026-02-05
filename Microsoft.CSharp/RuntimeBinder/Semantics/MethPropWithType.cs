using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000B8 RID: 184
	internal class MethPropWithType : SymWithType
	{
		// Token: 0x06000654 RID: 1620 RVA: 0x0001E020 File Offset: 0x0001C220
		public MethPropWithType()
		{
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001E028 File Offset: 0x0001C228
		public MethPropWithType(MethodOrPropertySymbol mps, AggregateType ats)
		{
			base.Set(mps, ats);
		}
	}
}
