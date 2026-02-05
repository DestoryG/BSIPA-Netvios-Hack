using System;

namespace System.IO.Compression
{
	// Token: 0x02000428 RID: 1064
	internal class FastEncoder
	{
		// Token: 0x060027E5 RID: 10213 RVA: 0x000B746F File Offset: 0x000B566F
		public FastEncoder()
		{
			this.inputWindow = new FastEncoderWindow();
			this.currentMatch = new Match();
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000B748D File Offset: 0x000B568D
		internal int BytesInHistory
		{
			get
			{
				return this.inputWindow.BytesAvailable;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060027E7 RID: 10215 RVA: 0x000B749A File Offset: 0x000B569A
		internal DeflateInput UnprocessedInput
		{
			get
			{
				return this.inputWindow.UnprocessedInput;
			}
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x000B74A7 File Offset: 0x000B56A7
		internal void FlushInput()
		{
			this.inputWindow.FlushWindow();
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060027E9 RID: 10217 RVA: 0x000B74B4 File Offset: 0x000B56B4
		internal double LastCompressionRatio
		{
			get
			{
				return this.lastCompressionRatio;
			}
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x000B74BC File Offset: 0x000B56BC
		internal void GetBlock(DeflateInput input, OutputBuffer output, int maxBytesToCopy)
		{
			FastEncoder.WriteDeflatePreamble(output);
			this.GetCompressedOutput(input, output, maxBytesToCopy);
			this.WriteEndOfBlock(output);
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000B74D4 File Offset: 0x000B56D4
		internal void GetCompressedData(DeflateInput input, OutputBuffer output)
		{
			this.GetCompressedOutput(input, output, -1);
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000B74DF File Offset: 0x000B56DF
		internal void GetBlockHeader(OutputBuffer output)
		{
			FastEncoder.WriteDeflatePreamble(output);
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000B74E7 File Offset: 0x000B56E7
		internal void GetBlockFooter(OutputBuffer output)
		{
			this.WriteEndOfBlock(output);
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000B74F0 File Offset: 0x000B56F0
		private void GetCompressedOutput(DeflateInput input, OutputBuffer output, int maxBytesToCopy)
		{
			int bytesWritten = output.BytesWritten;
			int num = 0;
			int num2 = this.BytesInHistory + input.Count;
			do
			{
				int num3 = ((input.Count < this.inputWindow.FreeWindowSpace) ? input.Count : this.inputWindow.FreeWindowSpace);
				if (maxBytesToCopy >= 1)
				{
					num3 = Math.Min(num3, maxBytesToCopy - num);
				}
				if (num3 > 0)
				{
					this.inputWindow.CopyBytes(input.Buffer, input.StartIndex, num3);
					input.ConsumeBytes(num3);
					num += num3;
				}
				this.GetCompressedOutput(output);
			}
			while (this.SafeToWriteTo(output) && this.InputAvailable(input) && (maxBytesToCopy < 1 || num < maxBytesToCopy));
			int bytesWritten2 = output.BytesWritten;
			int num4 = bytesWritten2 - bytesWritten;
			int num5 = this.BytesInHistory + input.Count;
			int num6 = num2 - num5;
			if (num4 != 0)
			{
				this.lastCompressionRatio = (double)num4 / (double)num6;
			}
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000B75D0 File Offset: 0x000B57D0
		private void GetCompressedOutput(OutputBuffer output)
		{
			while (this.inputWindow.BytesAvailable > 0 && this.SafeToWriteTo(output))
			{
				this.inputWindow.GetNextSymbolOrMatch(this.currentMatch);
				if (this.currentMatch.State == MatchState.HasSymbol)
				{
					FastEncoder.WriteChar(this.currentMatch.Symbol, output);
				}
				else if (this.currentMatch.State == MatchState.HasMatch)
				{
					FastEncoder.WriteMatch(this.currentMatch.Length, this.currentMatch.Position, output);
				}
				else
				{
					FastEncoder.WriteChar(this.currentMatch.Symbol, output);
					FastEncoder.WriteMatch(this.currentMatch.Length, this.currentMatch.Position, output);
				}
			}
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000B7688 File Offset: 0x000B5888
		private bool InputAvailable(DeflateInput input)
		{
			return input.Count > 0 || this.BytesInHistory > 0;
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000B769E File Offset: 0x000B589E
		private bool SafeToWriteTo(OutputBuffer output)
		{
			return output.FreeBytes > 16;
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000B76AC File Offset: 0x000B58AC
		private void WriteEndOfBlock(OutputBuffer output)
		{
			uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[256];
			int num2 = (int)(num & 31U);
			output.WriteBits(num2, num >> 5);
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000B76D4 File Offset: 0x000B58D4
		internal static void WriteMatch(int matchLen, int matchPos, OutputBuffer output)
		{
			uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[254 + matchLen];
			int num2 = (int)(num & 31U);
			if (num2 <= 16)
			{
				output.WriteBits(num2, num >> 5);
			}
			else
			{
				output.WriteBits(16, (num >> 5) & 65535U);
				output.WriteBits(num2 - 16, num >> 21);
			}
			num = FastEncoderStatics.FastEncoderDistanceCodeInfo[FastEncoderStatics.GetSlot(matchPos)];
			output.WriteBits((int)(num & 15U), num >> 8);
			int num3 = (int)((num >> 4) & 15U);
			if (num3 != 0)
			{
				output.WriteBits(num3, (uint)(matchPos & (int)FastEncoderStatics.BitMask[num3]));
			}
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000B7758 File Offset: 0x000B5958
		internal static void WriteChar(byte b, OutputBuffer output)
		{
			uint num = FastEncoderStatics.FastEncoderLiteralCodeInfo[(int)b];
			output.WriteBits((int)(num & 31U), num >> 5);
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000B777A File Offset: 0x000B597A
		internal static void WriteDeflatePreamble(OutputBuffer output)
		{
			output.WriteBytes(FastEncoderStatics.FastEncoderTreeStructureData, 0, FastEncoderStatics.FastEncoderTreeStructureData.Length);
			output.WriteBits(9, 34U);
		}

		// Token: 0x040021A2 RID: 8610
		private FastEncoderWindow inputWindow;

		// Token: 0x040021A3 RID: 8611
		private Match currentMatch;

		// Token: 0x040021A4 RID: 8612
		private double lastCompressionRatio;
	}
}
