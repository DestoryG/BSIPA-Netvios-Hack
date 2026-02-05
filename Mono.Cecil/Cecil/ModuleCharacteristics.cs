using System;

namespace Mono.Cecil
{
	// Token: 0x0200009B RID: 155
	[Flags]
	public enum ModuleCharacteristics
	{
		// Token: 0x040001C7 RID: 455
		HighEntropyVA = 32,
		// Token: 0x040001C8 RID: 456
		DynamicBase = 64,
		// Token: 0x040001C9 RID: 457
		NoSEH = 1024,
		// Token: 0x040001CA RID: 458
		NXCompat = 256,
		// Token: 0x040001CB RID: 459
		AppContainer = 4096,
		// Token: 0x040001CC RID: 460
		TerminalServerAware = 32768
	}
}
