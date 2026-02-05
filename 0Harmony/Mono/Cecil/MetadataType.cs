using System;

namespace Mono.Cecil
{
	// Token: 0x0200017C RID: 380
	internal enum MetadataType : byte
	{
		// Token: 0x04000500 RID: 1280
		Void = 1,
		// Token: 0x04000501 RID: 1281
		Boolean,
		// Token: 0x04000502 RID: 1282
		Char,
		// Token: 0x04000503 RID: 1283
		SByte,
		// Token: 0x04000504 RID: 1284
		Byte,
		// Token: 0x04000505 RID: 1285
		Int16,
		// Token: 0x04000506 RID: 1286
		UInt16,
		// Token: 0x04000507 RID: 1287
		Int32,
		// Token: 0x04000508 RID: 1288
		UInt32,
		// Token: 0x04000509 RID: 1289
		Int64,
		// Token: 0x0400050A RID: 1290
		UInt64,
		// Token: 0x0400050B RID: 1291
		Single,
		// Token: 0x0400050C RID: 1292
		Double,
		// Token: 0x0400050D RID: 1293
		String,
		// Token: 0x0400050E RID: 1294
		Pointer,
		// Token: 0x0400050F RID: 1295
		ByReference,
		// Token: 0x04000510 RID: 1296
		ValueType,
		// Token: 0x04000511 RID: 1297
		Class,
		// Token: 0x04000512 RID: 1298
		Var,
		// Token: 0x04000513 RID: 1299
		Array,
		// Token: 0x04000514 RID: 1300
		GenericInstance,
		// Token: 0x04000515 RID: 1301
		TypedByReference,
		// Token: 0x04000516 RID: 1302
		IntPtr = 24,
		// Token: 0x04000517 RID: 1303
		UIntPtr,
		// Token: 0x04000518 RID: 1304
		FunctionPointer = 27,
		// Token: 0x04000519 RID: 1305
		Object,
		// Token: 0x0400051A RID: 1306
		MVar = 30,
		// Token: 0x0400051B RID: 1307
		RequiredModifier,
		// Token: 0x0400051C RID: 1308
		OptionalModifier,
		// Token: 0x0400051D RID: 1309
		Sentinel = 65,
		// Token: 0x0400051E RID: 1310
		Pinned = 69
	}
}
