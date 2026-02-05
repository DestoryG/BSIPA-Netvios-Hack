using System;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x0200007A RID: 122
	public enum JSONNodeType
	{
		// Token: 0x04000451 RID: 1105
		Array = 1,
		// Token: 0x04000452 RID: 1106
		Object,
		// Token: 0x04000453 RID: 1107
		String,
		// Token: 0x04000454 RID: 1108
		Number,
		// Token: 0x04000455 RID: 1109
		NullValue,
		// Token: 0x04000456 RID: 1110
		Boolean,
		// Token: 0x04000457 RID: 1111
		None,
		// Token: 0x04000458 RID: 1112
		Custom = 255
	}
}
