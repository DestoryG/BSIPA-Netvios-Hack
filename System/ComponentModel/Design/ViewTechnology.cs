using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x02000602 RID: 1538
	[ComVisible(true)]
	public enum ViewTechnology
	{
		// Token: 0x04002B65 RID: 11109
		[Obsolete("This value has been deprecated. Use ViewTechnology.Default instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Passthrough,
		// Token: 0x04002B66 RID: 11110
		[Obsolete("This value has been deprecated. Use ViewTechnology.Default instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		WindowsForms,
		// Token: 0x04002B67 RID: 11111
		Default
	}
}
