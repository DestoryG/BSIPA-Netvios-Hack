using System;

namespace SimpleJSON
{
	// Token: 0x02000004 RID: 4
	public enum JSONNodeType
	{
		// Token: 0x04000002 RID: 2
		Array = 1,
		// Token: 0x04000003 RID: 3
		Object,
		// Token: 0x04000004 RID: 4
		String,
		// Token: 0x04000005 RID: 5
		Number,
		// Token: 0x04000006 RID: 6
		NullValue,
		// Token: 0x04000007 RID: 7
		Boolean,
		// Token: 0x04000008 RID: 8
		None,
		// Token: 0x04000009 RID: 9
		Custom = 255
	}
}
