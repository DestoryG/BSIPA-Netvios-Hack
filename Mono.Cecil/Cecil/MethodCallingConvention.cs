using System;

namespace Mono.Cecil
{
	// Token: 0x02000088 RID: 136
	public enum MethodCallingConvention : byte
	{
		// Token: 0x04000139 RID: 313
		Default,
		// Token: 0x0400013A RID: 314
		C,
		// Token: 0x0400013B RID: 315
		StdCall,
		// Token: 0x0400013C RID: 316
		ThisCall,
		// Token: 0x0400013D RID: 317
		FastCall,
		// Token: 0x0400013E RID: 318
		VarArg,
		// Token: 0x0400013F RID: 319
		Generic = 16
	}
}
