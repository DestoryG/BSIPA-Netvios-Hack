using System;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x02000078 RID: 120
	public struct OffsetTable
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000C431 File Offset: 0x0000A631
		// (set) Token: 0x0600020D RID: 525 RVA: 0x0000C439 File Offset: 0x0000A639
		public uint SFNTVersion { readonly get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000C442 File Offset: 0x0000A642
		// (set) Token: 0x0600020F RID: 527 RVA: 0x0000C44A File Offset: 0x0000A64A
		public ushort NumTables { readonly get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000C453 File Offset: 0x0000A653
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000C45B File Offset: 0x0000A65B
		public ushort SearchRange { readonly get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000C464 File Offset: 0x0000A664
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000C46C File Offset: 0x0000A66C
		public ushort EntrySelector { readonly get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000C475 File Offset: 0x0000A675
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000C47D File Offset: 0x0000A67D
		public ushort RangeShift { readonly get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000C486 File Offset: 0x0000A686
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000C48E File Offset: 0x0000A68E
		public long TablesStart { readonly get; set; }

		// Token: 0x06000218 RID: 536 RVA: 0x0000C498 File Offset: 0x0000A698
		public bool Validate()
		{
			uint num = NumericHelpers.NextPow2((uint)this.NumTables) << 1;
			return this.SearchRange == (ushort)num * 16 && this.EntrySelector == (ushort)NumericHelpers.Log2(num) && this.RangeShift == this.NumTables * 16 - this.SearchRange;
		}

		// Token: 0x04000064 RID: 100
		public const uint TrueTypeOnlyVersion = 65536U;

		// Token: 0x04000065 RID: 101
		public const uint OpenTypeCFFVersion = 1330926671U;
	}
}
