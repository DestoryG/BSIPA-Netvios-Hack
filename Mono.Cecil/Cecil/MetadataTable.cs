using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200001B RID: 27
	internal abstract class MetadataTable
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001FA RID: 506
		public abstract int Length { get; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000ADA9 File Offset: 0x00008FA9
		public bool IsLarge
		{
			get
			{
				return this.Length > 65535;
			}
		}

		// Token: 0x060001FC RID: 508
		public abstract void Write(TableHeapBuffer buffer);

		// Token: 0x060001FD RID: 509
		public abstract void Sort();
	}
}
