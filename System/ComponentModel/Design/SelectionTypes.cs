using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005FD RID: 1533
	[Flags]
	[ComVisible(true)]
	public enum SelectionTypes
	{
		// Token: 0x04002B0F RID: 11023
		Auto = 1,
		// Token: 0x04002B10 RID: 11024
		[Obsolete("This value has been deprecated. Use SelectionTypes.Auto instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Normal = 1,
		// Token: 0x04002B11 RID: 11025
		Replace = 2,
		// Token: 0x04002B12 RID: 11026
		[Obsolete("This value has been deprecated.  It is no longer supported. http://go.microsoft.com/fwlink/?linkid=14202")]
		MouseDown = 4,
		// Token: 0x04002B13 RID: 11027
		[Obsolete("This value has been deprecated.  It is no longer supported. http://go.microsoft.com/fwlink/?linkid=14202")]
		MouseUp = 8,
		// Token: 0x04002B14 RID: 11028
		[Obsolete("This value has been deprecated. Use SelectionTypes.Primary instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Click = 16,
		// Token: 0x04002B15 RID: 11029
		Primary = 16,
		// Token: 0x04002B16 RID: 11030
		Toggle = 32,
		// Token: 0x04002B17 RID: 11031
		Add = 64,
		// Token: 0x04002B18 RID: 11032
		Remove = 128,
		// Token: 0x04002B19 RID: 11033
		[Obsolete("This value has been deprecated. Use Enum class methods to determine valid values, or use a type converter. http://go.microsoft.com/fwlink/?linkid=14202")]
		Valid = 31
	}
}
