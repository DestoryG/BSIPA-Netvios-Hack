using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200023D RID: 573
	internal struct BitSet
	{
		// Token: 0x060011D6 RID: 4566 RVA: 0x0003A24B File Offset: 0x0003844B
		internal BitSet(BitAccess bits)
		{
			bits.ReadInt32(out this.size);
			this.words = new uint[this.size];
			bits.ReadUInt32(this.words);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0003A278 File Offset: 0x00038478
		internal bool IsSet(int index)
		{
			int num = index / 32;
			return num < this.size && (this.words[num] & BitSet.GetBit(index)) > 0U;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0003A2A7 File Offset: 0x000384A7
		private static uint GetBit(int index)
		{
			return 1U << index % 32;
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x0003A2B2 File Offset: 0x000384B2
		internal bool IsEmpty
		{
			get
			{
				return this.size == 0;
			}
		}

		// Token: 0x04000A3C RID: 2620
		private int size;

		// Token: 0x04000A3D RID: 2621
		private uint[] words;
	}
}
