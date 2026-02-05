using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CustomAvatar
{
	// Token: 0x02000011 RID: 17
	[JsonConverter(typeof(StringEnumConverter))]
	internal enum AvatarResizeMode
	{
		// Token: 0x04000050 RID: 80
		ArmSpan,
		// Token: 0x04000051 RID: 81
		Height,
		// Token: 0x04000052 RID: 82
		None
	}
}
