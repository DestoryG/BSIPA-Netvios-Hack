using System;

namespace Mono.Cecil
{
	// Token: 0x0200013F RID: 319
	internal enum MethodCallingConvention : byte
	{
		// Token: 0x04000353 RID: 851
		Default,
		// Token: 0x04000354 RID: 852
		C,
		// Token: 0x04000355 RID: 853
		StdCall,
		// Token: 0x04000356 RID: 854
		ThisCall,
		// Token: 0x04000357 RID: 855
		FastCall,
		// Token: 0x04000358 RID: 856
		VarArg,
		// Token: 0x04000359 RID: 857
		Generic = 16
	}
}
