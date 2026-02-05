using System;

namespace Mono.Cecil
{
	// Token: 0x0200013E RID: 318
	[Flags]
	internal enum MethodAttributes : ushort
	{
		// Token: 0x0400033B RID: 827
		MemberAccessMask = 7,
		// Token: 0x0400033C RID: 828
		CompilerControlled = 0,
		// Token: 0x0400033D RID: 829
		Private = 1,
		// Token: 0x0400033E RID: 830
		FamANDAssem = 2,
		// Token: 0x0400033F RID: 831
		Assembly = 3,
		// Token: 0x04000340 RID: 832
		Family = 4,
		// Token: 0x04000341 RID: 833
		FamORAssem = 5,
		// Token: 0x04000342 RID: 834
		Public = 6,
		// Token: 0x04000343 RID: 835
		Static = 16,
		// Token: 0x04000344 RID: 836
		Final = 32,
		// Token: 0x04000345 RID: 837
		Virtual = 64,
		// Token: 0x04000346 RID: 838
		HideBySig = 128,
		// Token: 0x04000347 RID: 839
		VtableLayoutMask = 256,
		// Token: 0x04000348 RID: 840
		ReuseSlot = 0,
		// Token: 0x04000349 RID: 841
		NewSlot = 256,
		// Token: 0x0400034A RID: 842
		CheckAccessOnOverride = 512,
		// Token: 0x0400034B RID: 843
		Abstract = 1024,
		// Token: 0x0400034C RID: 844
		SpecialName = 2048,
		// Token: 0x0400034D RID: 845
		PInvokeImpl = 8192,
		// Token: 0x0400034E RID: 846
		UnmanagedExport = 8,
		// Token: 0x0400034F RID: 847
		RTSpecialName = 4096,
		// Token: 0x04000350 RID: 848
		HasSecurity = 16384,
		// Token: 0x04000351 RID: 849
		RequireSecObject = 32768
	}
}
