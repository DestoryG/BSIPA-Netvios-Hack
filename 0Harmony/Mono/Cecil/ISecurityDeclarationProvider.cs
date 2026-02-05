using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000168 RID: 360
	internal interface ISecurityDeclarationProvider : IMetadataTokenProvider
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000B25 RID: 2853
		bool HasSecurityDeclarations { get; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000B26 RID: 2854
		Collection<SecurityDeclaration> SecurityDeclarations { get; }
	}
}
