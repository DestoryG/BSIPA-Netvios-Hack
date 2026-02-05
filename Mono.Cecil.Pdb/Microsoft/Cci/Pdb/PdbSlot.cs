using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000E4 RID: 228
	internal class PdbSlot
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00006A14 File Offset: 0x00004C14
		internal PdbSlot(BitAccess bits)
		{
			AttrSlotSym attrSlotSym;
			bits.ReadUInt32(out attrSlotSym.index);
			bits.ReadUInt32(out attrSlotSym.typind);
			bits.ReadUInt32(out attrSlotSym.offCod);
			bits.ReadUInt16(out attrSlotSym.segCod);
			bits.ReadUInt16(out attrSlotSym.flags);
			bits.ReadCString(out attrSlotSym.name);
			this.slot = attrSlotSym.index;
			this.typeToken = attrSlotSym.typind;
			this.name = attrSlotSym.name;
			this.flags = attrSlotSym.flags;
		}

		// Token: 0x040004F3 RID: 1267
		internal uint slot;

		// Token: 0x040004F4 RID: 1268
		internal uint typeToken;

		// Token: 0x040004F5 RID: 1269
		internal string name;

		// Token: 0x040004F6 RID: 1270
		internal ushort flags;
	}
}
