using System;
using System.Collections;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021E RID: 542
	internal class NamespaceInfo
	{
		// Token: 0x04000A02 RID: 2562
		public string Name;

		// Token: 0x04000A03 RID: 2563
		public int NamespaceID;

		// Token: 0x04000A04 RID: 2564
		public ArrayList UsingClauses = new ArrayList();
	}
}
