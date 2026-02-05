using System;

namespace Mono.Cecil
{
	// Token: 0x02000069 RID: 105
	internal interface IGenericContext
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000465 RID: 1125
		bool IsDefinition { get; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000466 RID: 1126
		IGenericParameterProvider Type { get; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000467 RID: 1127
		IGenericParameterProvider Method { get; }
	}
}
