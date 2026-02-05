using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000113 RID: 275
	public struct ImageDebugDirectory
	{
		// Token: 0x0400068E RID: 1678
		public const int Size = 28;

		// Token: 0x0400068F RID: 1679
		public int Characteristics;

		// Token: 0x04000690 RID: 1680
		public int TimeDateStamp;

		// Token: 0x04000691 RID: 1681
		public short MajorVersion;

		// Token: 0x04000692 RID: 1682
		public short MinorVersion;

		// Token: 0x04000693 RID: 1683
		public ImageDebugType Type;

		// Token: 0x04000694 RID: 1684
		public int SizeOfData;

		// Token: 0x04000695 RID: 1685
		public int AddressOfRawData;

		// Token: 0x04000696 RID: 1686
		public int PointerToRawData;
	}
}
