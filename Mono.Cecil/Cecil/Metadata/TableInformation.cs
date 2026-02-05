using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000F0 RID: 240
	internal struct TableInformation
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001EE15 File Offset: 0x0001D015
		public bool IsLarge
		{
			get
			{
				return this.Length > 65535U;
			}
		}

		// Token: 0x04000424 RID: 1060
		public uint Offset;

		// Token: 0x04000425 RID: 1061
		public uint Length;

		// Token: 0x04000426 RID: 1062
		public uint RowSize;
	}
}
