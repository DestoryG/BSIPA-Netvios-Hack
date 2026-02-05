using System;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F2 RID: 242
	internal class BitFlagsGenerator
	{
		// Token: 0x06000F22 RID: 3874 RVA: 0x0003DF08 File Offset: 0x0003C108
		public BitFlagsGenerator(int bitCount)
		{
			this.bitCount = bitCount;
			int num = (bitCount + 7) / 8;
			this.locals = new byte[num];
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0003DF34 File Offset: 0x0003C134
		public void Store(int bitIndex, bool value)
		{
			if (value)
			{
				byte[] array = this.locals;
				int byteIndex = BitFlagsGenerator.GetByteIndex(bitIndex);
				array[byteIndex] |= BitFlagsGenerator.GetBitValue(bitIndex);
				return;
			}
			byte[] array2 = this.locals;
			int byteIndex2 = BitFlagsGenerator.GetByteIndex(bitIndex);
			array2[byteIndex2] &= ~BitFlagsGenerator.GetBitValue(bitIndex);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0003DF74 File Offset: 0x0003C174
		public bool Load(int bitIndex)
		{
			byte b = this.locals[BitFlagsGenerator.GetByteIndex(bitIndex)];
			byte bitValue = BitFlagsGenerator.GetBitValue(bitIndex);
			return (b & bitValue) == bitValue;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0003DF9A File Offset: 0x0003C19A
		public byte[] LoadArray()
		{
			return (byte[])this.locals.Clone();
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0003DFAC File Offset: 0x0003C1AC
		public int GetLocalCount()
		{
			return this.locals.Length;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0003DFB6 File Offset: 0x0003C1B6
		public int GetBitCount()
		{
			return this.bitCount;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0003DFBE File Offset: 0x0003C1BE
		public byte GetLocal(int i)
		{
			return this.locals[i];
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0003DFC8 File Offset: 0x0003C1C8
		public static bool IsBitSet(byte[] bytes, int bitIndex)
		{
			int byteIndex = BitFlagsGenerator.GetByteIndex(bitIndex);
			byte bitValue = BitFlagsGenerator.GetBitValue(bitIndex);
			return (bytes[byteIndex] & bitValue) == bitValue;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0003DFEC File Offset: 0x0003C1EC
		public static void SetBit(byte[] bytes, int bitIndex)
		{
			int byteIndex = BitFlagsGenerator.GetByteIndex(bitIndex);
			byte bitValue = BitFlagsGenerator.GetBitValue(bitIndex);
			int num = byteIndex;
			bytes[num] |= bitValue;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0003E014 File Offset: 0x0003C214
		private static int GetByteIndex(int bitIndex)
		{
			return bitIndex >> 3;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0003E019 File Offset: 0x0003C219
		private static byte GetBitValue(int bitIndex)
		{
			return (byte)(1 << (bitIndex & 7));
		}

		// Token: 0x04000599 RID: 1433
		private int bitCount;

		// Token: 0x0400059A RID: 1434
		private byte[] locals;
	}
}
