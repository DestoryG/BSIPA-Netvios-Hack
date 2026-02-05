using System;

namespace Mono.Cecil.PE
{
	// Token: 0x02000193 RID: 403
	internal struct DataDirectory
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0002AB24 File Offset: 0x00028D24
		public bool IsZero
		{
			get
			{
				return this.VirtualAddress == 0U && this.Size == 0U;
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0002AB39 File Offset: 0x00028D39
		public DataDirectory(uint rva, uint size)
		{
			this.VirtualAddress = rva;
			this.Size = size;
		}

		// Token: 0x04000598 RID: 1432
		public readonly uint VirtualAddress;

		// Token: 0x04000599 RID: 1433
		public readonly uint Size;
	}
}
