using System;
using System.Collections.Generic;

namespace Microsoft.Cci
{
	// Token: 0x02000012 RID: 18
	public interface INamespaceScope
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600014A RID: 330
		IEnumerable<IUsedNamespace> UsedNamespaces { get; }
	}
}
