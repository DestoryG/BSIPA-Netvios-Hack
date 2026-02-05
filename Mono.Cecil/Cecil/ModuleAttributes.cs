using System;

namespace Mono.Cecil
{
	// Token: 0x0200009A RID: 154
	[Flags]
	public enum ModuleAttributes
	{
		// Token: 0x040001C1 RID: 449
		ILOnly = 1,
		// Token: 0x040001C2 RID: 450
		Required32Bit = 2,
		// Token: 0x040001C3 RID: 451
		ILLibrary = 4,
		// Token: 0x040001C4 RID: 452
		StrongNameSigned = 8,
		// Token: 0x040001C5 RID: 453
		Preferred32Bit = 131072
	}
}
