using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020004A4 RID: 1188
	[Flags]
	public enum SourceLevels
	{
		// Token: 0x0400269D RID: 9885
		Off = 0,
		// Token: 0x0400269E RID: 9886
		Critical = 1,
		// Token: 0x0400269F RID: 9887
		Error = 3,
		// Token: 0x040026A0 RID: 9888
		Warning = 7,
		// Token: 0x040026A1 RID: 9889
		Information = 15,
		// Token: 0x040026A2 RID: 9890
		Verbose = 31,
		// Token: 0x040026A3 RID: 9891
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		ActivityTracing = 65280,
		// Token: 0x040026A4 RID: 9892
		All = -1
	}
}
