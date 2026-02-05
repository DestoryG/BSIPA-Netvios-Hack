using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000E3 RID: 227
	internal class PdbScope
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x00006793 File Offset: 0x00004993
		internal PdbScope(uint address, uint length, PdbSlot[] slots, PdbConstant[] constants, string[] usedNamespaces)
		{
			this.constants = constants;
			this.slots = slots;
			this.scopes = new PdbScope[0];
			this.usedNamespaces = usedNamespaces;
			this.address = address;
			this.offset = 0U;
			this.length = length;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000067D4 File Offset: 0x000049D4
		internal PdbScope(uint funcOffset, BlockSym32 block, BitAccess bits, out uint typind)
		{
			this.address = block.off;
			this.offset = block.off - funcOffset;
			this.length = block.len;
			typind = 0U;
			int num;
			int num2;
			int num3;
			int num4;
			PdbFunction.CountScopesAndSlots(bits, block.end, out num, out num2, out num3, out num4);
			this.constants = new PdbConstant[num];
			this.scopes = new PdbScope[num2];
			this.slots = new PdbSlot[num3];
			this.usedNamespaces = new string[num4];
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			while ((long)bits.Position < (long)((ulong)block.end))
			{
				ushort num9;
				bits.ReadUInt16(out num9);
				int position = bits.Position;
				int num10 = bits.Position + (int)num9;
				bits.Position = position;
				ushort num11;
				bits.ReadUInt16(out num11);
				SYM sym = (SYM)num11;
				if (sym <= SYM.S_BLOCK32)
				{
					if (sym == SYM.S_END)
					{
						bits.Position = num10;
						continue;
					}
					if (sym == SYM.S_BLOCK32)
					{
						BlockSym32 blockSym = default(BlockSym32);
						bits.ReadUInt32(out blockSym.parent);
						bits.ReadUInt32(out blockSym.end);
						bits.ReadUInt32(out blockSym.len);
						bits.ReadUInt32(out blockSym.off);
						bits.ReadUInt16(out blockSym.seg);
						bits.SkipCString(out blockSym.name);
						bits.Position = num10;
						this.scopes[num6++] = new PdbScope(funcOffset, blockSym, bits, out typind);
						continue;
					}
				}
				else
				{
					if (sym == SYM.S_MANSLOT)
					{
						this.slots[num7++] = new PdbSlot(bits);
						bits.Position = num10;
						continue;
					}
					if (sym == SYM.S_UNAMESPACE)
					{
						bits.ReadCString(out this.usedNamespaces[num8++]);
						bits.Position = num10;
						continue;
					}
					if (sym == SYM.S_MANCONSTANT)
					{
						this.constants[num5++] = new PdbConstant(bits);
						bits.Position = num10;
						continue;
					}
				}
				bits.Position = num10;
			}
			if ((long)bits.Position != (long)((ulong)block.end))
			{
				throw new Exception("Not at S_END");
			}
			ushort num12;
			bits.ReadUInt16(out num12);
			ushort num13;
			bits.ReadUInt16(out num13);
			if (num13 != 6)
			{
				throw new Exception("Missing S_END");
			}
		}

		// Token: 0x040004EC RID: 1260
		internal PdbConstant[] constants;

		// Token: 0x040004ED RID: 1261
		internal PdbSlot[] slots;

		// Token: 0x040004EE RID: 1262
		internal PdbScope[] scopes;

		// Token: 0x040004EF RID: 1263
		internal string[] usedNamespaces;

		// Token: 0x040004F0 RID: 1264
		internal uint address;

		// Token: 0x040004F1 RID: 1265
		internal uint offset;

		// Token: 0x040004F2 RID: 1266
		internal uint length;
	}
}
