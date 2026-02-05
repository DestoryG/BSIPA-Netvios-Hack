using System;

namespace Mono.Cecil.PE
{
	// Token: 0x020000D1 RID: 209
	internal struct DataDirectory
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x0001BA24 File Offset: 0x00019C24
		public bool IsZero
		{
			get
			{
				return this.VirtualAddress == 0U && this.Size == 0U;
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001BA39 File Offset: 0x00019C39
		public DataDirectory(uint rva, uint size)
		{
			this.VirtualAddress = rva;
			this.Size = size;
		}

		// Token: 0x0400033B RID: 827
		public readonly uint VirtualAddress;

		// Token: 0x0400033C RID: 828
		public readonly uint Size;
	}
}
