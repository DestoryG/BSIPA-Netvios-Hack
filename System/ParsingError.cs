using System;

namespace System
{
	// Token: 0x0200004C RID: 76
	internal enum ParsingError
	{
		// Token: 0x0400049A RID: 1178
		None,
		// Token: 0x0400049B RID: 1179
		BadFormat,
		// Token: 0x0400049C RID: 1180
		BadScheme,
		// Token: 0x0400049D RID: 1181
		BadAuthority,
		// Token: 0x0400049E RID: 1182
		EmptyUriString,
		// Token: 0x0400049F RID: 1183
		LastRelativeUriOkErrIndex = 4,
		// Token: 0x040004A0 RID: 1184
		SchemeLimit,
		// Token: 0x040004A1 RID: 1185
		SizeLimit,
		// Token: 0x040004A2 RID: 1186
		MustRootedPath,
		// Token: 0x040004A3 RID: 1187
		BadHostName,
		// Token: 0x040004A4 RID: 1188
		NonEmptyHost,
		// Token: 0x040004A5 RID: 1189
		BadPort,
		// Token: 0x040004A6 RID: 1190
		BadAuthorityTerminator,
		// Token: 0x040004A7 RID: 1191
		CannotCreateRelative
	}
}
