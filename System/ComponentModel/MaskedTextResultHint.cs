using System;

namespace System.ComponentModel
{
	// Token: 0x0200058F RID: 1423
	public enum MaskedTextResultHint
	{
		// Token: 0x04002A0A RID: 10762
		Unknown,
		// Token: 0x04002A0B RID: 10763
		CharacterEscaped,
		// Token: 0x04002A0C RID: 10764
		NoEffect,
		// Token: 0x04002A0D RID: 10765
		SideEffect,
		// Token: 0x04002A0E RID: 10766
		Success,
		// Token: 0x04002A0F RID: 10767
		AsciiCharacterExpected = -1,
		// Token: 0x04002A10 RID: 10768
		AlphanumericCharacterExpected = -2,
		// Token: 0x04002A11 RID: 10769
		DigitExpected = -3,
		// Token: 0x04002A12 RID: 10770
		LetterExpected = -4,
		// Token: 0x04002A13 RID: 10771
		SignedDigitExpected = -5,
		// Token: 0x04002A14 RID: 10772
		InvalidInput = -51,
		// Token: 0x04002A15 RID: 10773
		PromptCharNotAllowed = -52,
		// Token: 0x04002A16 RID: 10774
		UnavailableEditPosition = -53,
		// Token: 0x04002A17 RID: 10775
		NonEditPosition = -54,
		// Token: 0x04002A18 RID: 10776
		PositionOutOfRange = -55
	}
}
