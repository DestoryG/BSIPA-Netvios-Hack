using System;

namespace System.IO.Compression
{
	// Token: 0x0200041C RID: 1052
	internal class CopyEncoder
	{
		// Token: 0x06002762 RID: 10082 RVA: 0x000B57E8 File Offset: 0x000B39E8
		public void GetBlock(DeflateInput input, OutputBuffer output, bool isFinal)
		{
			int num = 0;
			if (input != null)
			{
				num = Math.Min(input.Count, output.FreeBytes - 5 - output.BitsInBuffer);
				if (num > 65531)
				{
					num = 65531;
				}
			}
			if (isFinal)
			{
				output.WriteBits(3, 1U);
			}
			else
			{
				output.WriteBits(3, 0U);
			}
			output.FlushBits();
			this.WriteLenNLen((ushort)num, output);
			if (input != null && num > 0)
			{
				output.WriteBytes(input.Buffer, input.StartIndex, num);
				input.ConsumeBytes(num);
			}
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000B5868 File Offset: 0x000B3A68
		private void WriteLenNLen(ushort len, OutputBuffer output)
		{
			output.WriteUInt16(len);
			ushort num = ~len;
			output.WriteUInt16(num);
		}

		// Token: 0x04002162 RID: 8546
		private const int PaddingSize = 5;

		// Token: 0x04002163 RID: 8547
		private const int MaxUncompressedBlockSize = 65536;
	}
}
