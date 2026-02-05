using System;

namespace Mono.Cecil
{
	// Token: 0x0200016E RID: 366
	[Flags]
	internal enum TypeDefinitionTreatment
	{
		// Token: 0x04000490 RID: 1168
		None = 0,
		// Token: 0x04000491 RID: 1169
		KindMask = 15,
		// Token: 0x04000492 RID: 1170
		NormalType = 1,
		// Token: 0x04000493 RID: 1171
		NormalAttribute = 2,
		// Token: 0x04000494 RID: 1172
		UnmangleWindowsRuntimeName = 3,
		// Token: 0x04000495 RID: 1173
		PrefixWindowsRuntimeName = 4,
		// Token: 0x04000496 RID: 1174
		RedirectToClrType = 5,
		// Token: 0x04000497 RID: 1175
		RedirectToClrAttribute = 6,
		// Token: 0x04000498 RID: 1176
		Abstract = 16,
		// Token: 0x04000499 RID: 1177
		Internal = 32
	}
}
