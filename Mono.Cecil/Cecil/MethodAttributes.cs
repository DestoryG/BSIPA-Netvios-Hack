using System;

namespace Mono.Cecil
{
	// Token: 0x02000087 RID: 135
	[Flags]
	public enum MethodAttributes : ushort
	{
		// Token: 0x04000121 RID: 289
		MemberAccessMask = 7,
		// Token: 0x04000122 RID: 290
		CompilerControlled = 0,
		// Token: 0x04000123 RID: 291
		Private = 1,
		// Token: 0x04000124 RID: 292
		FamANDAssem = 2,
		// Token: 0x04000125 RID: 293
		Assembly = 3,
		// Token: 0x04000126 RID: 294
		Family = 4,
		// Token: 0x04000127 RID: 295
		FamORAssem = 5,
		// Token: 0x04000128 RID: 296
		Public = 6,
		// Token: 0x04000129 RID: 297
		Static = 16,
		// Token: 0x0400012A RID: 298
		Final = 32,
		// Token: 0x0400012B RID: 299
		Virtual = 64,
		// Token: 0x0400012C RID: 300
		HideBySig = 128,
		// Token: 0x0400012D RID: 301
		VtableLayoutMask = 256,
		// Token: 0x0400012E RID: 302
		ReuseSlot = 0,
		// Token: 0x0400012F RID: 303
		NewSlot = 256,
		// Token: 0x04000130 RID: 304
		CheckAccessOnOverride = 512,
		// Token: 0x04000131 RID: 305
		Abstract = 1024,
		// Token: 0x04000132 RID: 306
		SpecialName = 2048,
		// Token: 0x04000133 RID: 307
		PInvokeImpl = 8192,
		// Token: 0x04000134 RID: 308
		UnmanagedExport = 8,
		// Token: 0x04000135 RID: 309
		RTSpecialName = 4096,
		// Token: 0x04000136 RID: 310
		HasSecurity = 16384,
		// Token: 0x04000137 RID: 311
		RequireSecObject = 32768
	}
}
