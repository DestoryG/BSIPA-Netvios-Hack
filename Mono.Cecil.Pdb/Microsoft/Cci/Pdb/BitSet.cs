using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000017 RID: 23
	internal struct BitSet
	{
		// Token: 0x0600016F RID: 367 RVA: 0x000042C7 File Offset: 0x000024C7
		internal BitSet(BitAccess bits)
		{
			bits.ReadInt32(out this.size);
			this.words = new uint[this.size];
			bits.ReadUInt32(this.words);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000042F4 File Offset: 0x000024F4
		internal bool IsSet(int index)
		{
			int num = index / 32;
			return num < this.size && (this.words[num] & BitSet.GetBit(index)) > 0U;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00004323 File Offset: 0x00002523
		private static uint GetBit(int index)
		{
			return 1U << index % 32;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000432E File Offset: 0x0000252E
		internal bool IsEmpty
		{
			get
			{
				return this.size == 0;
			}
		}

		// Token: 0x04000022 RID: 34
		private int size;

		// Token: 0x04000023 RID: 35
		private uint[] words;
	}
}
