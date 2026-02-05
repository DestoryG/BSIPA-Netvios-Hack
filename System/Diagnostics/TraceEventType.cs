using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020004AF RID: 1199
	public enum TraceEventType
	{
		// Token: 0x040026C8 RID: 9928
		Critical = 1,
		// Token: 0x040026C9 RID: 9929
		Error,
		// Token: 0x040026CA RID: 9930
		Warning = 4,
		// Token: 0x040026CB RID: 9931
		Information = 8,
		// Token: 0x040026CC RID: 9932
		Verbose = 16,
		// Token: 0x040026CD RID: 9933
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Start = 256,
		// Token: 0x040026CE RID: 9934
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Stop = 512,
		// Token: 0x040026CF RID: 9935
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Suspend = 1024,
		// Token: 0x040026D0 RID: 9936
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Resume = 2048,
		// Token: 0x040026D1 RID: 9937
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Transfer = 4096
	}
}
