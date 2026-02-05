using System;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x02000076 RID: 118
	public struct CollectionHeader
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000C2F1 File Offset: 0x0000A4F1
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000C2F9 File Offset: 0x0000A4F9
		public OpenTypeTag TTCTag { readonly get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000C302 File Offset: 0x0000A502
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000C30A File Offset: 0x0000A50A
		public ushort MajorVersion { readonly get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000C313 File Offset: 0x0000A513
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000C31B File Offset: 0x0000A51B
		public ushort MinorVersion { readonly get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000C324 File Offset: 0x0000A524
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000C32C File Offset: 0x0000A52C
		public uint NumFonts { readonly get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000C335 File Offset: 0x0000A535
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000C33D File Offset: 0x0000A53D
		public uint[] OffsetTable { readonly get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000C346 File Offset: 0x0000A546
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000C34E File Offset: 0x0000A54E
		public uint DSIGTag { readonly get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000C357 File Offset: 0x0000A557
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000C35F File Offset: 0x0000A55F
		public uint DSIGLength { readonly get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000C368 File Offset: 0x0000A568
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000C370 File Offset: 0x0000A570
		public uint DSIGOffset { readonly get; set; }

		// Token: 0x04000059 RID: 89
		public static readonly OpenTypeTag TTCExpectedTag = OpenTypeTag.FromString("ttcf");

		// Token: 0x0400005A RID: 90
		public const uint TTCTagBEInt = 1953784678U;

		// Token: 0x04000060 RID: 96
		public const uint DSIGTagValue = 1146308935U;
	}
}
