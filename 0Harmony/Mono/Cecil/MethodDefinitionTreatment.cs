using System;

namespace Mono.Cecil
{
	// Token: 0x02000170 RID: 368
	[Flags]
	internal enum MethodDefinitionTreatment
	{
		// Token: 0x040004A0 RID: 1184
		None = 0,
		// Token: 0x040004A1 RID: 1185
		Dispose = 1,
		// Token: 0x040004A2 RID: 1186
		Abstract = 2,
		// Token: 0x040004A3 RID: 1187
		Private = 4,
		// Token: 0x040004A4 RID: 1188
		Public = 8,
		// Token: 0x040004A5 RID: 1189
		Runtime = 16,
		// Token: 0x040004A6 RID: 1190
		InternalCall = 32
	}
}
