using System;
using System.Reflection;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200003A RID: 58
	internal sealed class AggregateDeclaration : ParentSymbol
	{
		// Token: 0x0600025D RID: 605 RVA: 0x00011D87 File Offset: 0x0000FF87
		public AggregateSymbol Agg()
		{
			return this.bag as AggregateSymbol;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00011D94 File Offset: 0x0000FF94
		public Assembly GetAssembly()
		{
			return this.Agg().AssociatedAssembly;
		}

		// Token: 0x040002BA RID: 698
		public NamespaceOrAggregateSymbol bag;

		// Token: 0x040002BB RID: 699
		public AggregateDeclaration declNext;
	}
}
