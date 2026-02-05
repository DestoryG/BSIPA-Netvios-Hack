using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E1 RID: 225
	internal sealed class PdbHeapBuffer : HeapBuffer
	{
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x000026DB File Offset: 0x000008DB
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001EB2F File Offset: 0x0001CD2F
		public PdbHeapBuffer()
			: base(0)
		{
		}
	}
}
