using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x020003C1 RID: 961
	internal sealed class System_DictionaryValueCollectionDebugView<TKey, TValue>
	{
		// Token: 0x06002417 RID: 9239 RVA: 0x000A91F0 File Offset: 0x000A73F0
		public System_DictionaryValueCollectionDebugView(ICollection<TValue> collection)
		{
			if (collection == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x000A9208 File Offset: 0x000A7408
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public TValue[] Items
		{
			get
			{
				TValue[] array = new TValue[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04001FF7 RID: 8183
		private ICollection<TValue> collection;
	}
}
