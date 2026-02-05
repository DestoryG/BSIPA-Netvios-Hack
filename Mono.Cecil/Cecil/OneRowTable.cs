using System;

namespace Mono.Cecil
{
	// Token: 0x0200001C RID: 28
	internal abstract class OneRowTable<TRow> : MetadataTable where TRow : struct
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public sealed override int Length
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00002A0D File Offset: 0x00000C0D
		public sealed override void Sort()
		{
		}

		// Token: 0x04000041 RID: 65
		internal TRow row;
	}
}
