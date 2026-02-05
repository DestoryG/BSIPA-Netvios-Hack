using System;

namespace Google.Protobuf
{
	// Token: 0x02000002 RID: 2
	internal static class ByteArray
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		internal static void Copy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
		{
			if (count > 12)
			{
				Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
				return;
			}
			int num = srcOffset + count;
			for (int i = srcOffset; i < num; i++)
			{
				dst[dstOffset++] = src[i];
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000208C File Offset: 0x0000028C
		internal static void Reverse(byte[] bytes)
		{
			int i = 0;
			int num = bytes.Length - 1;
			while (i < num)
			{
				byte b = bytes[i];
				bytes[i] = bytes[num];
				bytes[num] = b;
				i++;
				num--;
			}
		}

		// Token: 0x04000001 RID: 1
		private const int CopyThreshold = 12;
	}
}
