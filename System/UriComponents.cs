using System;

namespace System
{
	// Token: 0x02000049 RID: 73
	[Flags]
	[global::__DynamicallyInvokable]
	public enum UriComponents
	{
		// Token: 0x04000480 RID: 1152
		[global::__DynamicallyInvokable]
		Scheme = 1,
		// Token: 0x04000481 RID: 1153
		[global::__DynamicallyInvokable]
		UserInfo = 2,
		// Token: 0x04000482 RID: 1154
		[global::__DynamicallyInvokable]
		Host = 4,
		// Token: 0x04000483 RID: 1155
		[global::__DynamicallyInvokable]
		Port = 8,
		// Token: 0x04000484 RID: 1156
		[global::__DynamicallyInvokable]
		Path = 16,
		// Token: 0x04000485 RID: 1157
		[global::__DynamicallyInvokable]
		Query = 32,
		// Token: 0x04000486 RID: 1158
		[global::__DynamicallyInvokable]
		Fragment = 64,
		// Token: 0x04000487 RID: 1159
		[global::__DynamicallyInvokable]
		StrongPort = 128,
		// Token: 0x04000488 RID: 1160
		[global::__DynamicallyInvokable]
		NormalizedHost = 256,
		// Token: 0x04000489 RID: 1161
		[global::__DynamicallyInvokable]
		KeepDelimiter = 1073741824,
		// Token: 0x0400048A RID: 1162
		[global::__DynamicallyInvokable]
		SerializationInfoString = -2147483648,
		// Token: 0x0400048B RID: 1163
		[global::__DynamicallyInvokable]
		AbsoluteUri = 127,
		// Token: 0x0400048C RID: 1164
		[global::__DynamicallyInvokable]
		HostAndPort = 132,
		// Token: 0x0400048D RID: 1165
		[global::__DynamicallyInvokable]
		StrongAuthority = 134,
		// Token: 0x0400048E RID: 1166
		[global::__DynamicallyInvokable]
		SchemeAndServer = 13,
		// Token: 0x0400048F RID: 1167
		[global::__DynamicallyInvokable]
		HttpRequestUrl = 61,
		// Token: 0x04000490 RID: 1168
		[global::__DynamicallyInvokable]
		PathAndQuery = 48
	}
}
