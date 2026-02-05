using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D7 RID: 471
	internal struct ImageDebugDirectory
	{
		// Token: 0x040008ED RID: 2285
		public const int Size = 28;

		// Token: 0x040008EE RID: 2286
		public int Characteristics;

		// Token: 0x040008EF RID: 2287
		public int TimeDateStamp;

		// Token: 0x040008F0 RID: 2288
		public short MajorVersion;

		// Token: 0x040008F1 RID: 2289
		public short MinorVersion;

		// Token: 0x040008F2 RID: 2290
		public ImageDebugType Type;

		// Token: 0x040008F3 RID: 2291
		public int SizeOfData;

		// Token: 0x040008F4 RID: 2292
		public int AddressOfRawData;

		// Token: 0x040008F5 RID: 2293
		public int PointerToRawData;
	}
}
