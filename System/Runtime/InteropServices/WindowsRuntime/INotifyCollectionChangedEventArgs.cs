using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003E8 RID: 1000
	[Guid("4cf68d33-e3f2-4964-b85e-945b4f7e2f21")]
	[ComImport]
	internal interface INotifyCollectionChangedEventArgs
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06002615 RID: 9749
		NotifyCollectionChangedAction Action { get; }

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06002616 RID: 9750
		IList NewItems { get; }

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002617 RID: 9751
		IList OldItems { get; }

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06002618 RID: 9752
		int NewStartingIndex { get; }

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06002619 RID: 9753
		int OldStartingIndex { get; }
	}
}
