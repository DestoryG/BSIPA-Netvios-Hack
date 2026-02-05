using System;

namespace Mono.Cecil
{
	// Token: 0x02000056 RID: 86
	[Flags]
	public enum EventAttributes : ushort
	{
		// Token: 0x04000098 RID: 152
		None = 0,
		// Token: 0x04000099 RID: 153
		SpecialName = 512,
		// Token: 0x0400009A RID: 154
		RTSpecialName = 1024
	}
}
