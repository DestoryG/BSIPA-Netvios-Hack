using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000313 RID: 787
	internal class PdbSlot
	{
		// Token: 0x0600122A RID: 4650 RVA: 0x0003CC4C File Offset: 0x0003AE4C
		internal PdbSlot(uint slot, uint typeToken, string name, ushort flags)
		{
			this.slot = slot;
			this.typeToken = typeToken;
			this.name = name;
			this.flags = flags;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0003CC74 File Offset: 0x0003AE74
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

		// Token: 0x04000F1C RID: 3868
		internal uint slot;

		// Token: 0x04000F1D RID: 3869
		internal uint typeToken;

		// Token: 0x04000F1E RID: 3870
		internal string name;

		// Token: 0x04000F1F RID: 3871
		internal ushort flags;
	}
}
