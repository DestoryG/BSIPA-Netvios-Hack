using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000045 RID: 69
	[Flags]
	internal enum OpSigFlags
	{
		// Token: 0x04000322 RID: 802
		None = 0,
		// Token: 0x04000323 RID: 803
		Convert = 1,
		// Token: 0x04000324 RID: 804
		CanLift = 2,
		// Token: 0x04000325 RID: 805
		AutoLift = 4,
		// Token: 0x04000326 RID: 806
		Value = 7,
		// Token: 0x04000327 RID: 807
		Reference = 1,
		// Token: 0x04000328 RID: 808
		BoolBit = 3
	}
}
