using System;

namespace System.IO.Compression
{
	// Token: 0x02000421 RID: 1057
	internal class DeflaterManaged : IDeflater, IDisposable
	{
		// Token: 0x0600277A RID: 10106 RVA: 0x000B597A File Offset: 0x000B3B7A
		internal DeflaterManaged()
		{
			this.deflateEncoder = new FastEncoder();
			this.copyEncoder = new CopyEncoder();
			this.input = new DeflateInput();
			this.output = new OutputBuffer();
			this.processingState = DeflaterManaged.DeflaterState.NotStarted;
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000B59B5 File Offset: 0x000B3BB5
		private bool NeedsInput()
		{
			return ((IDeflater)this).NeedsInput();
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000B59BD File Offset: 0x000B3BBD
		bool IDeflater.NeedsInput()
		{
			return this.input.Count == 0 && this.deflateEncoder.BytesInHistory == 0;
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000B59DC File Offset: 0x000B3BDC
		void IDeflater.SetInput(byte[] inputBuffer, int startIndex, int count)
		{
			this.input.Buffer = inputBuffer;
			this.input.Count = count;
			this.input.StartIndex = startIndex;
			if (count > 0 && count < 256)
			{
				DeflaterManaged.DeflaterState deflaterState = this.processingState;
				if (deflaterState != DeflaterManaged.DeflaterState.NotStarted)
				{
					if (deflaterState == DeflaterManaged.DeflaterState.CompressThenCheck)
					{
						this.processingState = DeflaterManaged.DeflaterState.HandlingSmallData;
						return;
					}
					if (deflaterState != DeflaterManaged.DeflaterState.CheckingForIncompressible)
					{
						return;
					}
				}
				this.processingState = DeflaterManaged.DeflaterState.StartingSmallData;
				return;
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000B5A3C File Offset: 0x000B3C3C
		int IDeflater.GetDeflateOutput(byte[] outputBuffer)
		{
			this.output.UpdateBuffer(outputBuffer);
			switch (this.processingState)
			{
			case DeflaterManaged.DeflaterState.NotStarted:
			{
				DeflateInput.InputState inputState = this.input.DumpState();
				OutputBuffer.BufferState bufferState = this.output.DumpState();
				this.deflateEncoder.GetBlockHeader(this.output);
				this.deflateEncoder.GetCompressedData(this.input, this.output);
				if (!this.UseCompressed(this.deflateEncoder.LastCompressionRatio))
				{
					this.input.RestoreState(inputState);
					this.output.RestoreState(bufferState);
					this.copyEncoder.GetBlock(this.input, this.output, false);
					this.FlushInputWindows();
					this.processingState = DeflaterManaged.DeflaterState.CheckingForIncompressible;
					goto IL_023A;
				}
				this.processingState = DeflaterManaged.DeflaterState.CompressThenCheck;
				goto IL_023A;
			}
			case DeflaterManaged.DeflaterState.SlowDownForIncompressible1:
				this.deflateEncoder.GetBlockFooter(this.output);
				this.processingState = DeflaterManaged.DeflaterState.SlowDownForIncompressible2;
				break;
			case DeflaterManaged.DeflaterState.SlowDownForIncompressible2:
				break;
			case DeflaterManaged.DeflaterState.StartingSmallData:
				this.deflateEncoder.GetBlockHeader(this.output);
				this.processingState = DeflaterManaged.DeflaterState.HandlingSmallData;
				goto IL_0223;
			case DeflaterManaged.DeflaterState.CompressThenCheck:
				this.deflateEncoder.GetCompressedData(this.input, this.output);
				if (!this.UseCompressed(this.deflateEncoder.LastCompressionRatio))
				{
					this.processingState = DeflaterManaged.DeflaterState.SlowDownForIncompressible1;
					this.inputFromHistory = this.deflateEncoder.UnprocessedInput;
					goto IL_023A;
				}
				goto IL_023A;
			case DeflaterManaged.DeflaterState.CheckingForIncompressible:
			{
				DeflateInput.InputState inputState2 = this.input.DumpState();
				OutputBuffer.BufferState bufferState2 = this.output.DumpState();
				this.deflateEncoder.GetBlock(this.input, this.output, 8072);
				if (!this.UseCompressed(this.deflateEncoder.LastCompressionRatio))
				{
					this.input.RestoreState(inputState2);
					this.output.RestoreState(bufferState2);
					this.copyEncoder.GetBlock(this.input, this.output, false);
					this.FlushInputWindows();
					goto IL_023A;
				}
				goto IL_023A;
			}
			case DeflaterManaged.DeflaterState.HandlingSmallData:
				goto IL_0223;
			default:
				goto IL_023A;
			}
			if (this.inputFromHistory.Count > 0)
			{
				this.copyEncoder.GetBlock(this.inputFromHistory, this.output, false);
			}
			if (this.inputFromHistory.Count == 0)
			{
				this.deflateEncoder.FlushInput();
				this.processingState = DeflaterManaged.DeflaterState.CheckingForIncompressible;
				goto IL_023A;
			}
			goto IL_023A;
			IL_0223:
			this.deflateEncoder.GetCompressedData(this.input, this.output);
			IL_023A:
			return this.output.BytesWritten;
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000B5C90 File Offset: 0x000B3E90
		bool IDeflater.Finish(byte[] outputBuffer, out int bytesRead)
		{
			if (this.processingState == DeflaterManaged.DeflaterState.NotStarted)
			{
				bytesRead = 0;
				return true;
			}
			this.output.UpdateBuffer(outputBuffer);
			if (this.processingState == DeflaterManaged.DeflaterState.CompressThenCheck || this.processingState == DeflaterManaged.DeflaterState.HandlingSmallData || this.processingState == DeflaterManaged.DeflaterState.SlowDownForIncompressible1)
			{
				this.deflateEncoder.GetBlockFooter(this.output);
			}
			this.WriteFinal();
			bytesRead = this.output.BytesWritten;
			return true;
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000B5CF6 File Offset: 0x000B3EF6
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000B5CF8 File Offset: 0x000B3EF8
		protected void Dispose(bool disposing)
		{
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000B5CFA File Offset: 0x000B3EFA
		private bool UseCompressed(double ratio)
		{
			return ratio <= 1.0;
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000B5D0B File Offset: 0x000B3F0B
		private void FlushInputWindows()
		{
			this.deflateEncoder.FlushInput();
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000B5D18 File Offset: 0x000B3F18
		private void WriteFinal()
		{
			this.copyEncoder.GetBlock(null, this.output, true);
		}

		// Token: 0x04002168 RID: 8552
		private const int MinBlockSize = 256;

		// Token: 0x04002169 RID: 8553
		private const int MaxHeaderFooterGoo = 120;

		// Token: 0x0400216A RID: 8554
		private const int CleanCopySize = 8072;

		// Token: 0x0400216B RID: 8555
		private const double BadCompressionThreshold = 1.0;

		// Token: 0x0400216C RID: 8556
		private FastEncoder deflateEncoder;

		// Token: 0x0400216D RID: 8557
		private CopyEncoder copyEncoder;

		// Token: 0x0400216E RID: 8558
		private DeflateInput input;

		// Token: 0x0400216F RID: 8559
		private OutputBuffer output;

		// Token: 0x04002170 RID: 8560
		private DeflaterManaged.DeflaterState processingState;

		// Token: 0x04002171 RID: 8561
		private DeflateInput inputFromHistory;

		// Token: 0x02000817 RID: 2071
		private enum DeflaterState
		{
			// Token: 0x0400358E RID: 13710
			NotStarted,
			// Token: 0x0400358F RID: 13711
			SlowDownForIncompressible1,
			// Token: 0x04003590 RID: 13712
			SlowDownForIncompressible2,
			// Token: 0x04003591 RID: 13713
			StartingSmallData,
			// Token: 0x04003592 RID: 13714
			CompressThenCheck,
			// Token: 0x04003593 RID: 13715
			CheckingForIncompressible,
			// Token: 0x04003594 RID: 13716
			HandlingSmallData
		}
	}
}
