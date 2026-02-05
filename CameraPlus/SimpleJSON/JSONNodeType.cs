using System;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x02000013 RID: 19
	public enum JSONNodeType
	{
		// Token: 0x0400009E RID: 158
		Array = 1,
		// Token: 0x0400009F RID: 159
		Object,
		// Token: 0x040000A0 RID: 160
		String,
		// Token: 0x040000A1 RID: 161
		Number,
		// Token: 0x040000A2 RID: 162
		NullValue,
		// Token: 0x040000A3 RID: 163
		Boolean,
		// Token: 0x040000A4 RID: 164
		None,
		// Token: 0x040000A5 RID: 165
		Custom = 255
	}
}
