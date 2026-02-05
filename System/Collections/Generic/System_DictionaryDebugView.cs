using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x020003BF RID: 959
	internal sealed class System_DictionaryDebugView<K, V>
	{
		// Token: 0x06002413 RID: 9235 RVA: 0x000A9160 File Offset: 0x000A7360
		public System_DictionaryDebugView(IDictionary<K, V> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.dict = dictionary;
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x000A9180 File Offset: 0x000A7380
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<K, V>[] Items
		{
			get
			{
				KeyValuePair<K, V>[] array = new KeyValuePair<K, V>[this.dict.Count];
				this.dict.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04001FF5 RID: 8181
		private IDictionary<K, V> dict;
	}
}
