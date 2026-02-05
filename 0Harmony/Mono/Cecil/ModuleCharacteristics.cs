using System;

namespace Mono.Cecil
{
	// Token: 0x02000155 RID: 341
	[Flags]
	internal enum ModuleCharacteristics
	{
		// Token: 0x040003FF RID: 1023
		HighEntropyVA = 32,
		// Token: 0x04000400 RID: 1024
		DynamicBase = 64,
		// Token: 0x04000401 RID: 1025
		NoSEH = 1024,
		// Token: 0x04000402 RID: 1026
		NXCompat = 256,
		// Token: 0x04000403 RID: 1027
		AppContainer = 4096,
		// Token: 0x04000404 RID: 1028
		TerminalServerAware = 32768
	}
}
