using System;

namespace Mono.Cecil
{
	// Token: 0x0200013C RID: 316
	internal struct Range
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x00022378 File Offset: 0x00020578
		public Range(uint index, uint length)
		{
			this.Start = index;
			this.Length = length;
		}

		// Token: 0x0400031A RID: 794
		public uint Start;

		// Token: 0x0400031B RID: 795
		public uint Length;
	}
}
