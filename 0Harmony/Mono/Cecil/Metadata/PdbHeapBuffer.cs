using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A5 RID: 421
	internal sealed class PdbHeapBuffer : HeapBuffer
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x00010910 File Offset: 0x0000EB10
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0002DCC7 File Offset: 0x0002BEC7
		public PdbHeapBuffer()
			: base(0)
		{
		}
	}
}
