using System;

namespace Mono.Cecil
{
	// Token: 0x02000107 RID: 263
	[Flags]
	internal enum EventAttributes : ushort
	{
		// Token: 0x040002A2 RID: 674
		None = 0,
		// Token: 0x040002A3 RID: 675
		SpecialName = 512,
		// Token: 0x040002A4 RID: 676
		RTSpecialName = 1024
	}
}
