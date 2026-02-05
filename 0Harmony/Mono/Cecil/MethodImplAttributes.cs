using System;

namespace Mono.Cecil
{
	// Token: 0x02000142 RID: 322
	[Flags]
	internal enum MethodImplAttributes : ushort
	{
		// Token: 0x0400036D RID: 877
		CodeTypeMask = 3,
		// Token: 0x0400036E RID: 878
		IL = 0,
		// Token: 0x0400036F RID: 879
		Native = 1,
		// Token: 0x04000370 RID: 880
		OPTIL = 2,
		// Token: 0x04000371 RID: 881
		Runtime = 3,
		// Token: 0x04000372 RID: 882
		ManagedMask = 4,
		// Token: 0x04000373 RID: 883
		Unmanaged = 4,
		// Token: 0x04000374 RID: 884
		Managed = 0,
		// Token: 0x04000375 RID: 885
		ForwardRef = 16,
		// Token: 0x04000376 RID: 886
		PreserveSig = 128,
		// Token: 0x04000377 RID: 887
		InternalCall = 4096,
		// Token: 0x04000378 RID: 888
		Synchronized = 32,
		// Token: 0x04000379 RID: 889
		NoOptimization = 64,
		// Token: 0x0400037A RID: 890
		NoInlining = 8,
		// Token: 0x0400037B RID: 891
		AggressiveInlining = 256
	}
}
