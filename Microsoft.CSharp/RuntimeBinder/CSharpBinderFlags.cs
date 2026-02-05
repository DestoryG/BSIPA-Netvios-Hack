using System;
using System.ComponentModel;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200000C RID: 12
	[Flags]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public enum CSharpBinderFlags
	{
		// Token: 0x040000A5 RID: 165
		None = 0,
		// Token: 0x040000A6 RID: 166
		CheckedContext = 1,
		// Token: 0x040000A7 RID: 167
		InvokeSimpleName = 2,
		// Token: 0x040000A8 RID: 168
		InvokeSpecialName = 4,
		// Token: 0x040000A9 RID: 169
		BinaryOperationLogical = 8,
		// Token: 0x040000AA RID: 170
		ConvertExplicit = 16,
		// Token: 0x040000AB RID: 171
		ConvertArrayIndex = 32,
		// Token: 0x040000AC RID: 172
		ResultIndexed = 64,
		// Token: 0x040000AD RID: 173
		ValueFromCompoundAssignment = 128,
		// Token: 0x040000AE RID: 174
		ResultDiscarded = 256
	}
}
