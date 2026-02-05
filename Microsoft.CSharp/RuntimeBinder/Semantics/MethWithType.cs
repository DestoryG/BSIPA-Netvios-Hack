using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000B9 RID: 185
	internal sealed class MethWithType : MethPropWithType
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x0001E038 File Offset: 0x0001C238
		public MethWithType()
		{
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001E040 File Offset: 0x0001C240
		public MethWithType(MethodSymbol meth, AggregateType ats)
		{
			base.Set(meth, ats);
		}
	}
}
