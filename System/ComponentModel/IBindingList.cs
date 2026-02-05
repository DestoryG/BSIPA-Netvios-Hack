using System;
using System.Collections;

namespace System.ComponentModel
{
	// Token: 0x02000558 RID: 1368
	public interface IBindingList : IList, ICollection, IEnumerable
	{
		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600335A RID: 13146
		bool AllowNew { get; }

		// Token: 0x0600335B RID: 13147
		object AddNew();

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x0600335C RID: 13148
		bool AllowEdit { get; }

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x0600335D RID: 13149
		bool AllowRemove { get; }

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x0600335E RID: 13150
		bool SupportsChangeNotification { get; }

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x0600335F RID: 13151
		bool SupportsSearching { get; }

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06003360 RID: 13152
		bool SupportsSorting { get; }

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06003361 RID: 13153
		bool IsSorted { get; }

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06003362 RID: 13154
		PropertyDescriptor SortProperty { get; }

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06003363 RID: 13155
		ListSortDirection SortDirection { get; }

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06003364 RID: 13156
		// (remove) Token: 0x06003365 RID: 13157
		event ListChangedEventHandler ListChanged;

		// Token: 0x06003366 RID: 13158
		void AddIndex(PropertyDescriptor property);

		// Token: 0x06003367 RID: 13159
		void ApplySort(PropertyDescriptor property, ListSortDirection direction);

		// Token: 0x06003368 RID: 13160
		int Find(PropertyDescriptor property, object key);

		// Token: 0x06003369 RID: 13161
		void RemoveIndex(PropertyDescriptor property);

		// Token: 0x0600336A RID: 13162
		void RemoveSort();
	}
}
