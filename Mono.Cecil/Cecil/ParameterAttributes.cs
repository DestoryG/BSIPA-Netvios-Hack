using System;

namespace Mono.Cecil
{
	// Token: 0x0200009E RID: 158
	[Flags]
	public enum ParameterAttributes : ushort
	{
		// Token: 0x040001F6 RID: 502
		None = 0,
		// Token: 0x040001F7 RID: 503
		In = 1,
		// Token: 0x040001F8 RID: 504
		Out = 2,
		// Token: 0x040001F9 RID: 505
		Lcid = 4,
		// Token: 0x040001FA RID: 506
		Retval = 8,
		// Token: 0x040001FB RID: 507
		Optional = 16,
		// Token: 0x040001FC RID: 508
		HasDefault = 4096,
		// Token: 0x040001FD RID: 509
		HasFieldMarshal = 8192,
		// Token: 0x040001FE RID: 510
		Unused = 53216
	}
}
