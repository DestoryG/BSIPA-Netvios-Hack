using System;

namespace System
{
	// Token: 0x02000050 RID: 80
	[Flags]
	public enum GenericUriParserOptions
	{
		// Token: 0x040004BB RID: 1211
		Default = 0,
		// Token: 0x040004BC RID: 1212
		GenericAuthority = 1,
		// Token: 0x040004BD RID: 1213
		AllowEmptyAuthority = 2,
		// Token: 0x040004BE RID: 1214
		NoUserInfo = 4,
		// Token: 0x040004BF RID: 1215
		NoPort = 8,
		// Token: 0x040004C0 RID: 1216
		NoQuery = 16,
		// Token: 0x040004C1 RID: 1217
		NoFragment = 32,
		// Token: 0x040004C2 RID: 1218
		DontConvertPathBackslashes = 64,
		// Token: 0x040004C3 RID: 1219
		DontCompressPath = 128,
		// Token: 0x040004C4 RID: 1220
		DontUnescapePathDotsAndSlashes = 256,
		// Token: 0x040004C5 RID: 1221
		Idn = 512,
		// Token: 0x040004C6 RID: 1222
		IriParsing = 1024
	}
}
