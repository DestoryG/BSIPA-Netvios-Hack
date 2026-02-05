using System;

namespace System.Diagnostics
{
	// Token: 0x020004EA RID: 1258
	[Flags]
	public enum PerformanceCounterPermissionAccess
	{
		// Token: 0x040027FA RID: 10234
		[Obsolete("This member has been deprecated.  Use System.Diagnostics.PerformanceCounter.PerformanceCounterPermissionAccess.Read instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Browse = 1,
		// Token: 0x040027FB RID: 10235
		[Obsolete("This member has been deprecated.  Use System.Diagnostics.PerformanceCounter.PerformanceCounterPermissionAccess.Write instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Instrument = 3,
		// Token: 0x040027FC RID: 10236
		None = 0,
		// Token: 0x040027FD RID: 10237
		Read = 1,
		// Token: 0x040027FE RID: 10238
		Write = 2,
		// Token: 0x040027FF RID: 10239
		Administer = 7
	}
}
