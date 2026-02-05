using System;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x02000081 RID: 129
	public struct TableRecord
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000D0EC File Offset: 0x0000B2EC
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0000D0F4 File Offset: 0x0000B2F4
		public OpenTypeTag TableTag { readonly get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000D0FD File Offset: 0x0000B2FD
		// (set) Token: 0x06000275 RID: 629 RVA: 0x0000D105 File Offset: 0x0000B305
		public uint Checksum { readonly get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000D10E File Offset: 0x0000B30E
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0000D116 File Offset: 0x0000B316
		public uint Offset { readonly get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000D11F File Offset: 0x0000B31F
		// (set) Token: 0x06000279 RID: 633 RVA: 0x0000D127 File Offset: 0x0000B327
		public uint Length { readonly get; set; }

		// Token: 0x0600027A RID: 634 RVA: 0x0000D130 File Offset: 0x0000B330
		public bool Validate()
		{
			return this.TableTag.Validate();
		}
	}
}
