using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000588 RID: 1416
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ListSortDescription
	{
		// Token: 0x06003433 RID: 13363 RVA: 0x000E488C File Offset: 0x000E2A8C
		public ListSortDescription(PropertyDescriptor property, ListSortDirection direction)
		{
			this.property = property;
			this.sortDirection = direction;
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06003434 RID: 13364 RVA: 0x000E48A2 File Offset: 0x000E2AA2
		// (set) Token: 0x06003435 RID: 13365 RVA: 0x000E48AA File Offset: 0x000E2AAA
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.property;
			}
			set
			{
				this.property = value;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06003436 RID: 13366 RVA: 0x000E48B3 File Offset: 0x000E2AB3
		// (set) Token: 0x06003437 RID: 13367 RVA: 0x000E48BB File Offset: 0x000E2ABB
		public ListSortDirection SortDirection
		{
			get
			{
				return this.sortDirection;
			}
			set
			{
				this.sortDirection = value;
			}
		}

		// Token: 0x040029DA RID: 10714
		private PropertyDescriptor property;

		// Token: 0x040029DB RID: 10715
		private ListSortDirection sortDirection;
	}
}
