using System;

namespace Mono.Cecil
{
	// Token: 0x0200008A RID: 138
	[Flags]
	public enum MethodImplAttributes : ushort
	{
		// Token: 0x0400014D RID: 333
		CodeTypeMask = 3,
		// Token: 0x0400014E RID: 334
		IL = 0,
		// Token: 0x0400014F RID: 335
		Native = 1,
		// Token: 0x04000150 RID: 336
		OPTIL = 2,
		// Token: 0x04000151 RID: 337
		Runtime = 3,
		// Token: 0x04000152 RID: 338
		ManagedMask = 4,
		// Token: 0x04000153 RID: 339
		Unmanaged = 4,
		// Token: 0x04000154 RID: 340
		Managed = 0,
		// Token: 0x04000155 RID: 341
		ForwardRef = 16,
		// Token: 0x04000156 RID: 342
		PreserveSig = 128,
		// Token: 0x04000157 RID: 343
		InternalCall = 4096,
		// Token: 0x04000158 RID: 344
		Synchronized = 32,
		// Token: 0x04000159 RID: 345
		NoOptimization = 64,
		// Token: 0x0400015A RID: 346
		NoInlining = 8,
		// Token: 0x0400015B RID: 347
		AggressiveInlining = 256
	}
}
