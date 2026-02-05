using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000D5 RID: 213
	internal struct DbiSecCon
	{
		// Token: 0x0600017B RID: 379 RVA: 0x000046D8 File Offset: 0x000028D8
		internal DbiSecCon(BitAccess bits)
		{
			bits.ReadInt16(out this.section);
			bits.ReadInt16(out this.pad1);
			bits.ReadInt32(out this.offset);
			bits.ReadInt32(out this.size);
			bits.ReadUInt32(out this.flags);
			bits.ReadInt16(out this.module);
			bits.ReadInt16(out this.pad2);
			bits.ReadUInt32(out this.dataCrc);
			bits.ReadUInt32(out this.relocCrc);
		}

		// Token: 0x040004AE RID: 1198
		internal short section;

		// Token: 0x040004AF RID: 1199
		internal short pad1;

		// Token: 0x040004B0 RID: 1200
		internal int offset;

		// Token: 0x040004B1 RID: 1201
		internal int size;

		// Token: 0x040004B2 RID: 1202
		internal uint flags;

		// Token: 0x040004B3 RID: 1203
		internal short module;

		// Token: 0x040004B4 RID: 1204
		internal short pad2;

		// Token: 0x040004B5 RID: 1205
		internal uint dataCrc;

		// Token: 0x040004B6 RID: 1206
		internal uint relocCrc;
	}
}
