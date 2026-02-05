using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000D2 RID: 210
	internal struct DbiDbgHdr
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00004468 File Offset: 0x00002668
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

		// Token: 0x04000482 RID: 1154
		internal ushort snFPO;

		// Token: 0x04000483 RID: 1155
		internal ushort snException;

		// Token: 0x04000484 RID: 1156
		internal ushort snFixup;

		// Token: 0x04000485 RID: 1157
		internal ushort snOmapToSrc;

		// Token: 0x04000486 RID: 1158
		internal ushort snOmapFromSrc;

		// Token: 0x04000487 RID: 1159
		internal ushort snSectionHdr;

		// Token: 0x04000488 RID: 1160
		internal ushort snTokenRidMap;

		// Token: 0x04000489 RID: 1161
		internal ushort snXdata;

		// Token: 0x0400048A RID: 1162
		internal ushort snPdata;

		// Token: 0x0400048B RID: 1163
		internal ushort snNewFPO;

		// Token: 0x0400048C RID: 1164
		internal ushort snSectionHdrOrig;
	}
}
