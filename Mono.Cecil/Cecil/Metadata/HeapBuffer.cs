using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000DC RID: 220
	internal abstract class HeapBuffer : ByteBuffer
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0001E7F3 File Offset: 0x0001C9F3
		public bool IsLarge
		{
			get
			{
				return this.length > 65535;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000963 RID: 2403
		public abstract bool IsEmpty { get; }

		// Token: 0x06000964 RID: 2404 RVA: 0x0001E802 File Offset: 0x0001CA02
		protected HeapBuffer(int length)
			: base(length)
		{
		}
	}
}
