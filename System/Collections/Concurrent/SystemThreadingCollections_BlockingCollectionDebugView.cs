using System;
using System.Diagnostics;

namespace System.Collections.Concurrent
{
	// Token: 0x020003D1 RID: 977
	internal sealed class SystemThreadingCollections_BlockingCollectionDebugView<T>
	{
		// Token: 0x06002587 RID: 9607 RVA: 0x000AE714 File Offset: 0x000AC914
		public SystemThreadingCollections_BlockingCollectionDebugView(BlockingCollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.m_blockingCollection = collection;
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06002588 RID: 9608 RVA: 0x000AE731 File Offset: 0x000AC931
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this.m_blockingCollection.ToArray();
			}
		}

		// Token: 0x04002050 RID: 8272
		private BlockingCollection<T> m_blockingCollection;
	}
}
