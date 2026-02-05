using System;

namespace System.IO.Compression
{
	// Token: 0x02000433 RID: 1075
	internal enum InflaterState
	{
		// Token: 0x04002207 RID: 8711
		ReadingHeader,
		// Token: 0x04002208 RID: 8712
		ReadingBFinal = 2,
		// Token: 0x04002209 RID: 8713
		ReadingBType,
		// Token: 0x0400220A RID: 8714
		ReadingNumLitCodes,
		// Token: 0x0400220B RID: 8715
		ReadingNumDistCodes,
		// Token: 0x0400220C RID: 8716
		ReadingNumCodeLengthCodes,
		// Token: 0x0400220D RID: 8717
		ReadingCodeLengthCodes,
		// Token: 0x0400220E RID: 8718
		ReadingTreeCodesBefore,
		// Token: 0x0400220F RID: 8719
		ReadingTreeCodesAfter,
		// Token: 0x04002210 RID: 8720
		DecodeTop,
		// Token: 0x04002211 RID: 8721
		HaveInitialLength,
		// Token: 0x04002212 RID: 8722
		HaveFullLength,
		// Token: 0x04002213 RID: 8723
		HaveDistCode,
		// Token: 0x04002214 RID: 8724
		UncompressedAligning = 15,
		// Token: 0x04002215 RID: 8725
		UncompressedByte1,
		// Token: 0x04002216 RID: 8726
		UncompressedByte2,
		// Token: 0x04002217 RID: 8727
		UncompressedByte3,
		// Token: 0x04002218 RID: 8728
		UncompressedByte4,
		// Token: 0x04002219 RID: 8729
		DecodingUncompressed,
		// Token: 0x0400221A RID: 8730
		StartReadingFooter,
		// Token: 0x0400221B RID: 8731
		ReadingFooter,
		// Token: 0x0400221C RID: 8732
		VerifyingFooter,
		// Token: 0x0400221D RID: 8733
		Done
	}
}
