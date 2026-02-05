using System;

namespace Mono.Cecil
{
	// Token: 0x020000CB RID: 203
	internal abstract class OneRowTable<TRow> : MetadataTable where TRow : struct
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00010F39 File Offset: 0x0000F139
		public sealed override int Length
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00010C51 File Offset: 0x0000EE51
		public sealed override void Sort()
		{
		}

		// Token: 0x04000249 RID: 585
		internal TRow row;
	}
}
