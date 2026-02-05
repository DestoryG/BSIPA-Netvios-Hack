using System;

namespace Mono.Cecil
{
	// Token: 0x02000147 RID: 327
	internal interface IModifierType
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060009A6 RID: 2470
		TypeReference ModifierType { get; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060009A7 RID: 2471
		TypeReference ElementType { get; }
	}
}
