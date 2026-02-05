using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000066 RID: 102
	public interface IGenericInstance : IMetadataTokenProvider
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600045E RID: 1118
		bool HasGenericArguments { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600045F RID: 1119
		Collection<TypeReference> GenericArguments { get; }
	}
}
