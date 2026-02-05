using System;
using System.Collections;

namespace System.ComponentModel
{
	// Token: 0x02000559 RID: 1369
	public interface IBindingListView : IBindingList, IList, ICollection, IEnumerable
	{
		// Token: 0x0600336B RID: 13163
		void ApplySort(ListSortDescriptionCollection sorts);

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x0600336C RID: 13164
		// (set) Token: 0x0600336D RID: 13165
		string Filter { get; set; }

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x0600336E RID: 13166
		ListSortDescriptionCollection SortDescriptions { get; }

		// Token: 0x0600336F RID: 13167
		void RemoveFilter();

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06003370 RID: 13168
		bool SupportsAdvancedSorting { get; }

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06003371 RID: 13169
		bool SupportsFiltering { get; }
	}
}
