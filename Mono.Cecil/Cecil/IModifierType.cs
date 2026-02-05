using System;

namespace Mono.Cecil
{
	// Token: 0x0200008F RID: 143
	public interface IModifierType
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600060D RID: 1549
		TypeReference ModifierType { get; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600060E RID: 1550
		TypeReference ElementType { get; }
	}
}
