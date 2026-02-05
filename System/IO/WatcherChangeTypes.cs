using System;

namespace System.IO
{
	// Token: 0x02000407 RID: 1031
	[Flags]
	public enum WatcherChangeTypes
	{
		// Token: 0x040020E3 RID: 8419
		Created = 1,
		// Token: 0x040020E4 RID: 8420
		Deleted = 2,
		// Token: 0x040020E5 RID: 8421
		Changed = 4,
		// Token: 0x040020E6 RID: 8422
		Renamed = 8,
		// Token: 0x040020E7 RID: 8423
		All = 15
	}
}
