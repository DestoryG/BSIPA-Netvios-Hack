using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000032 RID: 50
	internal sealed class CandidateFunctionMember
	{
		// Token: 0x06000234 RID: 564 RVA: 0x000115E7 File Offset: 0x0000F7E7
		public CandidateFunctionMember(MethPropWithInst mpwi, TypeArray @params, byte ctypeLift, bool fExpanded)
		{
			this.mpwi = mpwi;
			this.@params = @params;
			this.ctypeLift = ctypeLift;
			this.fExpanded = fExpanded;
		}

		// Token: 0x04000297 RID: 663
		public MethPropWithInst mpwi;

		// Token: 0x04000298 RID: 664
		public TypeArray @params;

		// Token: 0x04000299 RID: 665
		public byte ctypeLift;

		// Token: 0x0400029A RID: 666
		public bool fExpanded;
	}
}
