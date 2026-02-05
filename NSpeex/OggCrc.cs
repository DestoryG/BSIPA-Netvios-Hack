using System;
using System.Security.Cryptography;

namespace NSpeex
{
	// Token: 0x0200001B RID: 27
	public class OggCrc : HashAlgorithm
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00008DA4 File Offset: 0x00006FA4
		static OggCrc()
		{
			uint num = 0U;
			while ((ulong)num < (ulong)((long)OggCrc.lookupTable.Length))
			{
				uint num2 = num << 24;
				for (int i = 0; i < 8; i++)
				{
					if ((num2 & 2147483648U) != 0U)
					{
						num2 = (num2 << 1) ^ 79764919U;
					}
					else
					{
						num2 <<= 1;
					}
				}
				OggCrc.lookupTable[(int)((UIntPtr)num)] = num2;
				num += 1U;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00008E07 File Offset: 0x00007007
		public override void Initialize()
		{
			this.hash = 0U;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00008E10 File Offset: 0x00007010
		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{
			for (int i = 0; i < cbSize; i++)
			{
				this.hash = (this.hash << 8) ^ OggCrc.lookupTable[(int)((UIntPtr)(((uint)array[i + ibStart] ^ (this.hash >> 24)) & 255U))];
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00008E54 File Offset: 0x00007054
		protected override byte[] HashFinal()
		{
			return new byte[]
			{
				(byte)(this.hash & 255U),
				(byte)((this.hash >> 8) & 255U),
				(byte)((this.hash >> 16) & 255U),
				(byte)((this.hash >> 24) & 255U)
			};
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00008EB1 File Offset: 0x000070B1
		public override int HashSize
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x040000DA RID: 218
		private const uint Polynomial = 79764919U;

		// Token: 0x040000DB RID: 219
		private const uint Seed = 0U;

		// Token: 0x040000DC RID: 220
		private static uint[] lookupTable = new uint[256];

		// Token: 0x040000DD RID: 221
		private uint hash;
	}
}
