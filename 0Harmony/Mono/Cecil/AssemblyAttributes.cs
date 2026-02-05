using System;

namespace Mono.Cecil
{
	// Token: 0x020000BD RID: 189
	[Flags]
	internal enum AssemblyAttributes : uint
	{
		// Token: 0x04000224 RID: 548
		PublicKey = 1U,
		// Token: 0x04000225 RID: 549
		SideBySideCompatible = 0U,
		// Token: 0x04000226 RID: 550
		Retargetable = 256U,
		// Token: 0x04000227 RID: 551
		WindowsRuntime = 512U,
		// Token: 0x04000228 RID: 552
		DisableJITCompileOptimizer = 16384U,
		// Token: 0x04000229 RID: 553
		EnableJITCompileTracking = 32768U
	}
}
