using System;

namespace System.Net.Mime
{
	// Token: 0x0200023D RID: 573
	internal class Base64WriteStateInfo : WriteStateInfoBase
	{
		// Token: 0x060015B2 RID: 5554 RVA: 0x000707CA File Offset: 0x0006E9CA
		internal Base64WriteStateInfo()
		{
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000707D2 File Offset: 0x0006E9D2
		internal Base64WriteStateInfo(int bufferSize, byte[] header, byte[] footer, int maxLineLength, int mimeHeaderLength)
			: base(bufferSize, header, footer, maxLineLength, mimeHeaderLength)
		{
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x000707E1 File Offset: 0x0006E9E1
		// (set) Token: 0x060015B5 RID: 5557 RVA: 0x000707E9 File Offset: 0x0006E9E9
		internal int Padding { get; set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x000707F2 File Offset: 0x0006E9F2
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x000707FA File Offset: 0x0006E9FA
		internal byte LastBits { get; set; }
	}
}
