using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200011C RID: 284
	internal interface IGenericInstance : IMetadataTokenProvider
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060007F0 RID: 2032
		bool HasGenericArguments { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060007F1 RID: 2033
		Collection<TypeReference> GenericArguments { get; }
	}
}
