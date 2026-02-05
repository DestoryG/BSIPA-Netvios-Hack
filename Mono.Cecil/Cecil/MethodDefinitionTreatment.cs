using System;

namespace Mono.Cecil
{
	// Token: 0x020000B4 RID: 180
	[Flags]
	internal enum MethodDefinitionTreatment
	{
		// Token: 0x04000263 RID: 611
		None = 0,
		// Token: 0x04000264 RID: 612
		Dispose = 1,
		// Token: 0x04000265 RID: 613
		Abstract = 2,
		// Token: 0x04000266 RID: 614
		Private = 4,
		// Token: 0x04000267 RID: 615
		Public = 8,
		// Token: 0x04000268 RID: 616
		Runtime = 16,
		// Token: 0x04000269 RID: 617
		InternalCall = 32
	}
}
