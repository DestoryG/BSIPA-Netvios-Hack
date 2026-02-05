using System;

namespace System.Diagnostics
{
	// Token: 0x020004D1 RID: 1233
	[Flags]
	public enum EventLogPermissionAccess
	{
		// Token: 0x0400276A RID: 10090
		None = 0,
		// Token: 0x0400276B RID: 10091
		Write = 16,
		// Token: 0x0400276C RID: 10092
		Administer = 48,
		// Token: 0x0400276D RID: 10093
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Administer instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Browse = 2,
		// Token: 0x0400276E RID: 10094
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Write instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Instrument = 6,
		// Token: 0x0400276F RID: 10095
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Administer instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Audit = 10
	}
}
