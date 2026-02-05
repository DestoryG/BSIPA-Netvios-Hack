using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x020003BC RID: 956
	internal sealed class System_CollectionDebugView<T>
	{
		// Token: 0x0600240D RID: 9229 RVA: 0x000A90C2 File Offset: 0x000A72C2
		public System_CollectionDebugView(ICollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.collection = collection;
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x000A90E0 File Offset: 0x000A72E0
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				T[] array = new T[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04001FF2 RID: 8178
		private ICollection<T> collection;
	}
}
