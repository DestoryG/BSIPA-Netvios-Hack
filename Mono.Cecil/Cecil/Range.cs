using System;

namespace Mono.Cecil
{
	// Token: 0x02000085 RID: 133
	internal struct Range
	{
		// Token: 0x06000522 RID: 1314 RVA: 0x00013B4C File Offset: 0x00011D4C
		public Range(uint index, uint length)
		{
			this.Start = index;
			this.Length = length;
		}

		// Token: 0x04000100 RID: 256
		public uint Start;

		// Token: 0x04000101 RID: 257
		public uint Length;
	}
}
