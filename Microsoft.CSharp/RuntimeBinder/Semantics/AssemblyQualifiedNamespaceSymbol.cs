using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000061 RID: 97
	internal sealed class AssemblyQualifiedNamespaceSymbol : ParentSymbol
	{
		// Token: 0x06000370 RID: 880 RVA: 0x000165C3 File Offset: 0x000147C3
		public NamespaceSymbol GetNS()
		{
			return this.parent as NamespaceSymbol;
		}
	}
}
