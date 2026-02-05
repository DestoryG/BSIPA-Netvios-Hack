using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000AD RID: 173
	public interface ISecurityDeclarationProvider : IMetadataTokenProvider
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600076B RID: 1899
		bool HasSecurityDeclarations { get; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600076C RID: 1900
		Collection<SecurityDeclaration> SecurityDeclarations { get; }
	}
}
