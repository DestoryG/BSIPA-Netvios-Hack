using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FE RID: 766
	internal interface INamespaceScope
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060011E5 RID: 4581
		IEnumerable<IUsedNamespace> UsedNamespaces { get; }
	}
}
