using System;

namespace Mono.Cecil
{
	// Token: 0x02000010 RID: 16
	[Flags]
	public enum AssemblyAttributes : uint
	{
		// Token: 0x04000021 RID: 33
		PublicKey = 1U,
		// Token: 0x04000022 RID: 34
		SideBySideCompatible = 0U,
		// Token: 0x04000023 RID: 35
		Retargetable = 256U,
		// Token: 0x04000024 RID: 36
		WindowsRuntime = 512U,
		// Token: 0x04000025 RID: 37
		DisableJITCompileOptimizer = 16384U,
		// Token: 0x04000026 RID: 38
		EnableJITCompileTracking = 32768U
	}
}
