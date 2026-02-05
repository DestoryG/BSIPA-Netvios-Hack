using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200066B RID: 1643
	[ComVisible(true)]
	[Serializable]
	public enum MemberAttributes
	{
		// Token: 0x04002C4A RID: 11338
		Abstract = 1,
		// Token: 0x04002C4B RID: 11339
		Final,
		// Token: 0x04002C4C RID: 11340
		Static,
		// Token: 0x04002C4D RID: 11341
		Override,
		// Token: 0x04002C4E RID: 11342
		Const,
		// Token: 0x04002C4F RID: 11343
		New = 16,
		// Token: 0x04002C50 RID: 11344
		Overloaded = 256,
		// Token: 0x04002C51 RID: 11345
		Assembly = 4096,
		// Token: 0x04002C52 RID: 11346
		FamilyAndAssembly = 8192,
		// Token: 0x04002C53 RID: 11347
		Family = 12288,
		// Token: 0x04002C54 RID: 11348
		FamilyOrAssembly = 16384,
		// Token: 0x04002C55 RID: 11349
		Private = 20480,
		// Token: 0x04002C56 RID: 11350
		Public = 24576,
		// Token: 0x04002C57 RID: 11351
		AccessMask = 61440,
		// Token: 0x04002C58 RID: 11352
		ScopeMask = 15,
		// Token: 0x04002C59 RID: 11353
		VTableMask = 240
	}
}
