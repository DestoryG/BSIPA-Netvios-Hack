using System;

namespace System.IO.Compression
{
	// Token: 0x0200042F RID: 1071
	internal static class GZipConstants
	{
		// Token: 0x040021D2 RID: 8658
		internal const int CompressionLevel_3 = 3;

		// Token: 0x040021D3 RID: 8659
		internal const int CompressionLevel_10 = 10;

		// Token: 0x040021D4 RID: 8660
		internal const long FileLengthModulo = 4294967296L;

		// Token: 0x040021D5 RID: 8661
		internal const byte ID1 = 31;

		// Token: 0x040021D6 RID: 8662
		internal const byte ID2 = 139;

		// Token: 0x040021D7 RID: 8663
		internal const byte Deflate = 8;

		// Token: 0x040021D8 RID: 8664
		internal const int Xfl_HeaderPos = 8;

		// Token: 0x040021D9 RID: 8665
		internal const byte Xfl_FastestAlgorithm = 4;

		// Token: 0x040021DA RID: 8666
		internal const byte Xfl_MaxCompressionSlowestAlgorithm = 2;
	}
}
