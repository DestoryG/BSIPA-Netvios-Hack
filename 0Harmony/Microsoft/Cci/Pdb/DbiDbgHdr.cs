using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002F9 RID: 761
	internal struct DbiDbgHdr
	{
		// Token: 0x060011DF RID: 4575 RVA: 0x0003A3EC File Offset: 0x000385EC
		internal DbiDbgHdr(BitAccess bits)
		{
			bits.ReadUInt16(out this.snFPO);
			bits.ReadUInt16(out this.snException);
			bits.ReadUInt16(out this.snFixup);
			bits.ReadUInt16(out this.snOmapToSrc);
			bits.ReadUInt16(out this.snOmapFromSrc);
			bits.ReadUInt16(out this.snSectionHdr);
			bits.ReadUInt16(out this.snTokenRidMap);
			bits.ReadUInt16(out this.snXdata);
			bits.ReadUInt16(out this.snPdata);
			bits.ReadUInt16(out this.snNewFPO);
			bits.ReadUInt16(out this.snSectionHdrOrig);
		}

		// Token: 0x04000E9E RID: 3742
		internal ushort snFPO;

		// Token: 0x04000E9F RID: 3743
		internal ushort snException;

		// Token: 0x04000EA0 RID: 3744
		internal ushort snFixup;

		// Token: 0x04000EA1 RID: 3745
		internal ushort snOmapToSrc;

		// Token: 0x04000EA2 RID: 3746
		internal ushort snOmapFromSrc;

		// Token: 0x04000EA3 RID: 3747
		internal ushort snSectionHdr;

		// Token: 0x04000EA4 RID: 3748
		internal ushort snTokenRidMap;

		// Token: 0x04000EA5 RID: 3749
		internal ushort snXdata;

		// Token: 0x04000EA6 RID: 3750
		internal ushort snPdata;

		// Token: 0x04000EA7 RID: 3751
		internal ushort snNewFPO;

		// Token: 0x04000EA8 RID: 3752
		internal ushort snSectionHdrOrig;
	}
}
