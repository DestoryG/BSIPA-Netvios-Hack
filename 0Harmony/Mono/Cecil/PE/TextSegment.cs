using System;

namespace Mono.Cecil.PE
{
	// Token: 0x02000199 RID: 409
	internal enum TextSegment
	{
		// Token: 0x040005D4 RID: 1492
		ImportAddressTable,
		// Token: 0x040005D5 RID: 1493
		CLIHeader,
		// Token: 0x040005D6 RID: 1494
		Code,
		// Token: 0x040005D7 RID: 1495
		Resources,
		// Token: 0x040005D8 RID: 1496
		Data,
		// Token: 0x040005D9 RID: 1497
		StrongNameSignature,
		// Token: 0x040005DA RID: 1498
		MetadataHeader,
		// Token: 0x040005DB RID: 1499
		TableHeap,
		// Token: 0x040005DC RID: 1500
		StringHeap,
		// Token: 0x040005DD RID: 1501
		UserStringHeap,
		// Token: 0x040005DE RID: 1502
		GuidHeap,
		// Token: 0x040005DF RID: 1503
		BlobHeap,
		// Token: 0x040005E0 RID: 1504
		PdbHeap,
		// Token: 0x040005E1 RID: 1505
		DebugDirectory,
		// Token: 0x040005E2 RID: 1506
		ImportDirectory,
		// Token: 0x040005E3 RID: 1507
		ImportHintNameTable,
		// Token: 0x040005E4 RID: 1508
		StartupStub
	}
}
