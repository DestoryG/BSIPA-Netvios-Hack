using System;
using System.Diagnostics;

namespace System.Collections.Concurrent
{
	// Token: 0x020003D3 RID: 979
	internal sealed class SystemThreadingCollection_IProducerConsumerCollectionDebugView<T>
	{
		// Token: 0x060025AA RID: 9642 RVA: 0x000AF03C File Offset: 0x000AD23C
		public SystemThreadingCollection_IProducerConsumerCollectionDebugView(IProducerConsumerCollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.m_collection = collection;
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x000AF059 File Offset: 0x000AD259
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this.m_collection.ToArray();
			}
		}

		// Token: 0x04002056 RID: 8278
		private IProducerConsumerCollection<T> m_collection;
	}
}
