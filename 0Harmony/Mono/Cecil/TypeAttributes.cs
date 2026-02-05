using System;

namespace Mono.Cecil
{
	// Token: 0x02000174 RID: 372
	[Flags]
	internal enum TypeAttributes : uint
	{
		// Token: 0x040004B4 RID: 1204
		VisibilityMask = 7U,
		// Token: 0x040004B5 RID: 1205
		NotPublic = 0U,
		// Token: 0x040004B6 RID: 1206
		Public = 1U,
		// Token: 0x040004B7 RID: 1207
		NestedPublic = 2U,
		// Token: 0x040004B8 RID: 1208
		NestedPrivate = 3U,
		// Token: 0x040004B9 RID: 1209
		NestedFamily = 4U,
		// Token: 0x040004BA RID: 1210
		NestedAssembly = 5U,
		// Token: 0x040004BB RID: 1211
		NestedFamANDAssem = 6U,
		// Token: 0x040004BC RID: 1212
		NestedFamORAssem = 7U,
		// Token: 0x040004BD RID: 1213
		LayoutMask = 24U,
		// Token: 0x040004BE RID: 1214
		AutoLayout = 0U,
		// Token: 0x040004BF RID: 1215
		SequentialLayout = 8U,
		// Token: 0x040004C0 RID: 1216
		ExplicitLayout = 16U,
		// Token: 0x040004C1 RID: 1217
		ClassSemanticMask = 32U,
		// Token: 0x040004C2 RID: 1218
		Class = 0U,
		// Token: 0x040004C3 RID: 1219
		Interface = 32U,
		// Token: 0x040004C4 RID: 1220
		Abstract = 128U,
		// Token: 0x040004C5 RID: 1221
		Sealed = 256U,
		// Token: 0x040004C6 RID: 1222
		SpecialName = 1024U,
		// Token: 0x040004C7 RID: 1223
		Import = 4096U,
		// Token: 0x040004C8 RID: 1224
		Serializable = 8192U,
		// Token: 0x040004C9 RID: 1225
		WindowsRuntime = 16384U,
		// Token: 0x040004CA RID: 1226
		StringFormatMask = 196608U,
		// Token: 0x040004CB RID: 1227
		AnsiClass = 0U,
		// Token: 0x040004CC RID: 1228
		UnicodeClass = 65536U,
		// Token: 0x040004CD RID: 1229
		AutoClass = 131072U,
		// Token: 0x040004CE RID: 1230
		BeforeFieldInit = 1048576U,
		// Token: 0x040004CF RID: 1231
		RTSpecialName = 2048U,
		// Token: 0x040004D0 RID: 1232
		HasSecurity = 262144U,
		// Token: 0x040004D1 RID: 1233
		Forwarder = 2097152U
	}
}
