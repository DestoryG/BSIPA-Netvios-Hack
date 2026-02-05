using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FC RID: 764
	internal struct DbiSecCon
	{
		// Token: 0x060011E2 RID: 4578 RVA: 0x0003A65C File Offset: 0x0003885C
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

		// Token: 0x04000ECA RID: 3786
		internal short section;

		// Token: 0x04000ECB RID: 3787
		internal short pad1;

		// Token: 0x04000ECC RID: 3788
		internal int offset;

		// Token: 0x04000ECD RID: 3789
		internal int size;

		// Token: 0x04000ECE RID: 3790
		internal uint flags;

		// Token: 0x04000ECF RID: 3791
		internal short module;

		// Token: 0x04000ED0 RID: 3792
		internal short pad2;

		// Token: 0x04000ED1 RID: 3793
		internal uint dataCrc;

		// Token: 0x04000ED2 RID: 3794
		internal uint relocCrc;
	}
}
