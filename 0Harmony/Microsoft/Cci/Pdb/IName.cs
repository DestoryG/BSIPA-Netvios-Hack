using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000300 RID: 768
	internal interface IName
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060011E8 RID: 4584
		int UniqueKey { get; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060011E9 RID: 4585
		int UniqueKeyIgnoringCase { get; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060011EA RID: 4586
		string Value { get; }
	}
}
