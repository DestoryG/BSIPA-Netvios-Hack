using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000589 RID: 1417
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ListSortDescriptionCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06003438 RID: 13368 RVA: 0x000E48C4 File Offset: 0x000E2AC4
		public ListSortDescriptionCollection()
		{
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000E48D8 File Offset: 0x000E2AD8
		public ListSortDescriptionCollection(ListSortDescription[] sorts)
		{
			if (sorts != null)
			{
				for (int i = 0; i < sorts.Length; i++)
				{
					this.sorts.Add(sorts[i]);
				}
			}
		}

		// Token: 0x17000CC2 RID: 3266
		public ListSortDescription this[int index]
		{
			get
			{
				return (ListSortDescription)this.sorts[index];
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x0600343C RID: 13372 RVA: 0x000E493A File Offset: 0x000E2B3A
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x000E493D File Offset: 0x000E2B3D
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
			}
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000E495A File Offset: 0x000E2B5A
		int IList.Add(object value)
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000E496B File Offset: 0x000E2B6B
		void IList.Clear()
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000E497C File Offset: 0x000E2B7C
		public bool Contains(object value)
		{
			return ((IList)this.sorts).Contains(value);
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000E498A File Offset: 0x000E2B8A
		public int IndexOf(object value)
		{
			return ((IList)this.sorts).IndexOf(value);
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000E4998 File Offset: 0x000E2B98
		void IList.Insert(int index, object value)
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x000E49A9 File Offset: 0x000E2BA9
		void IList.Remove(object value)
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000E49BA File Offset: 0x000E2BBA
		void IList.RemoveAt(int index)
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06003447 RID: 13383 RVA: 0x000E49CB File Offset: 0x000E2BCB
		public int Count
		{
			get
			{
				return this.sorts.Count;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x000E49D8 File Offset: 0x000E2BD8
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06003449 RID: 13385 RVA: 0x000E49DB File Offset: 0x000E2BDB
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000E49DE File Offset: 0x000E2BDE
		public void CopyTo(Array array, int index)
		{
			this.sorts.CopyTo(array, index);
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000E49ED File Offset: 0x000E2BED
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.sorts.GetEnumerator();
		}

		// Token: 0x040029DC RID: 10716
		private ArrayList sorts = new ArrayList();
	}
}
