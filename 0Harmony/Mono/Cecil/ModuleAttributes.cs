using System;

namespace Mono.Cecil
{
	// Token: 0x02000154 RID: 340
	[Flags]
	internal enum ModuleAttributes
	{
		// Token: 0x040003F9 RID: 1017
		ILOnly = 1,
		// Token: 0x040003FA RID: 1018
		Required32Bit = 2,
		// Token: 0x040003FB RID: 1019
		ILLibrary = 4,
		// Token: 0x040003FC RID: 1020
		StrongNameSigned = 8,
		// Token: 0x040003FD RID: 1021
		Preferred32Bit = 131072
	}
}
