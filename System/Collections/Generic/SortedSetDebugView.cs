using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x020003CD RID: 973
	internal class SortedSetDebugView<T>
	{
		// Token: 0x06002539 RID: 9529 RVA: 0x000AD730 File Offset: 0x000AB930
		public SortedSetDebugView(SortedSet<T> set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}
			this.set = set;
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x0600253A RID: 9530 RVA: 0x000AD74D File Offset: 0x000AB94D
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this.set.ToArray();
			}
		}

		// Token: 0x0400203F RID: 8255
		private SortedSet<T> set;
	}
}
