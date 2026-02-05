using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x020004CE RID: 1230
	public class EventLogEntryCollection : ICollection, IEnumerable
	{
		// Token: 0x06002E63 RID: 11875 RVA: 0x000D179F File Offset: 0x000CF99F
		internal EventLogEntryCollection(EventLogInternal log)
		{
			this.log = log;
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06002E64 RID: 11876 RVA: 0x000D17AE File Offset: 0x000CF9AE
		public int Count
		{
			get
			{
				return this.log.EntryCount;
			}
		}

		// Token: 0x17000B37 RID: 2871
		public virtual EventLogEntry this[int index]
		{
			get
			{
				return this.log.GetEntryAt(index);
			}
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000D17C9 File Offset: 0x000CF9C9
		public void CopyTo(EventLogEntry[] entries, int index)
		{
			((ICollection)this).CopyTo(entries, index);
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000D17D3 File Offset: 0x000CF9D3
		public IEnumerator GetEnumerator()
		{
			return new EventLogEntryCollection.EntriesEnumerator(this);
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000D17DB File Offset: 0x000CF9DB
		internal EventLogEntry GetEntryAtNoThrow(int index)
		{
			return this.log.GetEntryAtNoThrow(index);
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06002E69 RID: 11881 RVA: 0x000D17E9 File Offset: 0x000CF9E9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06002E6A RID: 11882 RVA: 0x000D17EC File Offset: 0x000CF9EC
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000D17F0 File Offset: 0x000CF9F0
		void ICollection.CopyTo(Array array, int index)
		{
			EventLogEntry[] allEntries = this.log.GetAllEntries();
			Array.Copy(allEntries, 0, array, index, allEntries.Length);
		}

		// Token: 0x04002761 RID: 10081
		private EventLogInternal log;

		// Token: 0x0200087F RID: 2175
		private class EntriesEnumerator : IEnumerator
		{
			// Token: 0x06004573 RID: 17779 RVA: 0x00121973 File Offset: 0x0011FB73
			internal EntriesEnumerator(EventLogEntryCollection entries)
			{
				this.entries = entries;
			}

			// Token: 0x17000FB4 RID: 4020
			// (get) Token: 0x06004574 RID: 17780 RVA: 0x00121989 File Offset: 0x0011FB89
			public object Current
			{
				get
				{
					if (this.cachedEntry == null)
					{
						throw new InvalidOperationException(SR.GetString("NoCurrentEntry"));
					}
					return this.cachedEntry;
				}
			}

			// Token: 0x06004575 RID: 17781 RVA: 0x001219A9 File Offset: 0x0011FBA9
			public bool MoveNext()
			{
				this.num++;
				this.cachedEntry = this.entries.GetEntryAtNoThrow(this.num);
				return this.cachedEntry != null;
			}

			// Token: 0x06004576 RID: 17782 RVA: 0x001219D9 File Offset: 0x0011FBD9
			public void Reset()
			{
				this.num = -1;
			}

			// Token: 0x04003736 RID: 14134
			private EventLogEntryCollection entries;

			// Token: 0x04003737 RID: 14135
			private int num = -1;

			// Token: 0x04003738 RID: 14136
			private EventLogEntry cachedEntry;
		}
	}
}
