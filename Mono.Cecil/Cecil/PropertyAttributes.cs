using System;

namespace Mono.Cecil
{
	// Token: 0x020000A6 RID: 166
	[Flags]
	public enum PropertyAttributes : ushort
	{
		// Token: 0x04000221 RID: 545
		None = 0,
		// Token: 0x04000222 RID: 546
		SpecialName = 512,
		// Token: 0x04000223 RID: 547
		RTSpecialName = 1024,
		// Token: 0x04000224 RID: 548
		HasDefault = 4096,
		// Token: 0x04000225 RID: 549
		Unused = 59903
	}
}
