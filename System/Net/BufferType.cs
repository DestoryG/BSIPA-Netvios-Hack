using System;

namespace System.Net
{
	// Token: 0x02000122 RID: 290
	internal enum BufferType
	{
		// Token: 0x04000FCE RID: 4046
		Empty,
		// Token: 0x04000FCF RID: 4047
		Data,
		// Token: 0x04000FD0 RID: 4048
		Token,
		// Token: 0x04000FD1 RID: 4049
		Parameters,
		// Token: 0x04000FD2 RID: 4050
		Missing,
		// Token: 0x04000FD3 RID: 4051
		Extra,
		// Token: 0x04000FD4 RID: 4052
		Trailer,
		// Token: 0x04000FD5 RID: 4053
		Header,
		// Token: 0x04000FD6 RID: 4054
		Padding = 9,
		// Token: 0x04000FD7 RID: 4055
		Stream,
		// Token: 0x04000FD8 RID: 4056
		ChannelBindings = 14,
		// Token: 0x04000FD9 RID: 4057
		TargetHost = 16,
		// Token: 0x04000FDA RID: 4058
		ReadOnlyFlag = -2147483648,
		// Token: 0x04000FDB RID: 4059
		ReadOnlyWithChecksum = 268435456
	}
}
