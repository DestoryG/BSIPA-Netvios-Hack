using System;

namespace Mono.Cecil.PE
{
	// Token: 0x020000D6 RID: 214
	internal enum TextSegment
	{
		// Token: 0x04000375 RID: 885
		ImportAddressTable,
		// Token: 0x04000376 RID: 886
		CLIHeader,
		// Token: 0x04000377 RID: 887
		Code,
		// Token: 0x04000378 RID: 888
		Resources,
		// Token: 0x04000379 RID: 889
		Data,
		// Token: 0x0400037A RID: 890
		StrongNameSignature,
		// Token: 0x0400037B RID: 891
		MetadataHeader,
		// Token: 0x0400037C RID: 892
		TableHeap,
		// Token: 0x0400037D RID: 893
		StringHeap,
		// Token: 0x0400037E RID: 894
		UserStringHeap,
		// Token: 0x0400037F RID: 895
		GuidHeap,
		// Token: 0x04000380 RID: 896
		BlobHeap,
		// Token: 0x04000381 RID: 897
		PdbHeap,
		// Token: 0x04000382 RID: 898
		DebugDirectory,
		// Token: 0x04000383 RID: 899
		ImportDirectory,
		// Token: 0x04000384 RID: 900
		ImportHintNameTable,
		// Token: 0x04000385 RID: 901
		StartupStub
	}
}
