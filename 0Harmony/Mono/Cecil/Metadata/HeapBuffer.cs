using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200019F RID: 415
	internal abstract class HeapBuffer : ByteBuffer
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x0002D907 File Offset: 0x0002BB07
		public bool IsLarge
		{
			get
			{
				return this.length > 65535;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000D45 RID: 3397
		public abstract bool IsEmpty { get; }

		// Token: 0x06000D46 RID: 3398 RVA: 0x0002D916 File Offset: 0x0002BB16
		protected HeapBuffer(int length)
			: base(length)
		{
		}
	}
}
