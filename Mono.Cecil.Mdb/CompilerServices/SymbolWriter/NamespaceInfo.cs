using System;
using System.Collections;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200001B RID: 27
	internal class NamespaceInfo
	{
		// Token: 0x0400009C RID: 156
		public string Name;

		// Token: 0x0400009D RID: 157
		public int NamespaceID;

		// Token: 0x0400009E RID: 158
		public ArrayList UsingClauses = new ArrayList();
	}
}
