using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000CA RID: 202
	internal abstract class MetadataTable
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600056B RID: 1387
		public abstract int Length { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001924E File Offset: 0x0001744E
		public bool IsLarge
		{
			get
			{
				return this.Length > 65535;
			}
		}

		// Token: 0x0600056D RID: 1389
		public abstract void Write(TableHeapBuffer buffer);

		// Token: 0x0600056E RID: 1390
		public abstract void Sort();
	}
}
