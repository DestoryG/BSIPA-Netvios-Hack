using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x020003C0 RID: 960
	internal sealed class System_DictionaryKeyCollectionDebugView<TKey, TValue>
	{
		// Token: 0x06002415 RID: 9237 RVA: 0x000A91AC File Offset: 0x000A73AC
		public System_DictionaryKeyCollectionDebugView(ICollection<TKey> collection)
		{
			if (collection == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x000A91C4 File Offset: 0x000A73C4
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public TKey[] Items
		{
			get
			{
				TKey[] array = new TKey[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04001FF6 RID: 8182
		private ICollection<TKey> collection;
	}
}
