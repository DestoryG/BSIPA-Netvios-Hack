using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000BA RID: 186
	internal sealed class PropWithType : MethPropWithType
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x0001E050 File Offset: 0x0001C250
		public PropWithType(PropertySymbol prop, AggregateType ats)
		{
			base.Set(prop, ats);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001E060 File Offset: 0x0001C260
		public PropWithType(SymWithType swt)
		{
			base.Set(swt.Sym as PropertySymbol, swt.Ats);
		}
	}
}
