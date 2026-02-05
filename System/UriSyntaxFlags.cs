using System;

namespace System
{
	// Token: 0x02000046 RID: 70
	[Flags]
	internal enum UriSyntaxFlags
	{
		// Token: 0x0400045D RID: 1117
		None = 0,
		// Token: 0x0400045E RID: 1118
		MustHaveAuthority = 1,
		// Token: 0x0400045F RID: 1119
		OptionalAuthority = 2,
		// Token: 0x04000460 RID: 1120
		MayHaveUserInfo = 4,
		// Token: 0x04000461 RID: 1121
		MayHavePort = 8,
		// Token: 0x04000462 RID: 1122
		MayHavePath = 16,
		// Token: 0x04000463 RID: 1123
		MayHaveQuery = 32,
		// Token: 0x04000464 RID: 1124
		MayHaveFragment = 64,
		// Token: 0x04000465 RID: 1125
		AllowEmptyHost = 128,
		// Token: 0x04000466 RID: 1126
		AllowUncHost = 256,
		// Token: 0x04000467 RID: 1127
		AllowDnsHost = 512,
		// Token: 0x04000468 RID: 1128
		AllowIPv4Host = 1024,
		// Token: 0x04000469 RID: 1129
		AllowIPv6Host = 2048,
		// Token: 0x0400046A RID: 1130
		AllowAnInternetHost = 3584,
		// Token: 0x0400046B RID: 1131
		AllowAnyOtherHost = 4096,
		// Token: 0x0400046C RID: 1132
		FileLikeUri = 8192,
		// Token: 0x0400046D RID: 1133
		MailToLikeUri = 16384,
		// Token: 0x0400046E RID: 1134
		V1_UnknownUri = 65536,
		// Token: 0x0400046F RID: 1135
		SimpleUserSyntax = 131072,
		// Token: 0x04000470 RID: 1136
		BuiltInSyntax = 262144,
		// Token: 0x04000471 RID: 1137
		ParserSchemeOnly = 524288,
		// Token: 0x04000472 RID: 1138
		AllowDOSPath = 1048576,
		// Token: 0x04000473 RID: 1139
		PathIsRooted = 2097152,
		// Token: 0x04000474 RID: 1140
		ConvertPathSlashes = 4194304,
		// Token: 0x04000475 RID: 1141
		CompressPath = 8388608,
		// Token: 0x04000476 RID: 1142
		CanonicalizeAsFilePath = 16777216,
		// Token: 0x04000477 RID: 1143
		UnEscapeDotsAndSlashes = 33554432,
		// Token: 0x04000478 RID: 1144
		AllowIdn = 67108864,
		// Token: 0x04000479 RID: 1145
		AllowIriParsing = 268435456
	}
}
