using System;

namespace System.IO.Compression
{
	// Token: 0x0200042D RID: 1069
	internal class GZipDecoder : IFileFormatReader
	{
		// Token: 0x0600280F RID: 10255 RVA: 0x000B7E94 File Offset: 0x000B6094
		public GZipDecoder()
		{
			this.Reset();
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000B7EA2 File Offset: 0x000B60A2
		public void Reset()
		{
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingID1;
			this.gzipFooterSubstate = GZipDecoder.GzipHeaderState.ReadingCRC;
			this.expectedCrc32 = 0U;
			this.expectedOutputStreamSizeModulo = 0U;
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x000B7EC4 File Offset: 0x000B60C4
		public bool ReadHeader(InputBuffer input)
		{
			int num;
			switch (this.gzipHeaderSubstate)
			{
			case GZipDecoder.GzipHeaderState.ReadingID1:
				num = input.GetBits(8);
				if (num < 0)
				{
					return false;
				}
				if (num != 31)
				{
					throw new InvalidDataException(SR.GetString("CorruptedGZipHeader"));
				}
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingID2;
				break;
			case GZipDecoder.GzipHeaderState.ReadingID2:
				break;
			case GZipDecoder.GzipHeaderState.ReadingCM:
				goto IL_00A5;
			case GZipDecoder.GzipHeaderState.ReadingFLG:
				goto IL_00CE;
			case GZipDecoder.GzipHeaderState.ReadingMMTime:
				goto IL_00F1;
			case GZipDecoder.GzipHeaderState.ReadingXFL:
				goto IL_0128;
			case GZipDecoder.GzipHeaderState.ReadingOS:
				goto IL_013D;
			case GZipDecoder.GzipHeaderState.ReadingXLen1:
				goto IL_0152;
			case GZipDecoder.GzipHeaderState.ReadingXLen2:
				goto IL_017B;
			case GZipDecoder.GzipHeaderState.ReadingXLenData:
				goto IL_01A8;
			case GZipDecoder.GzipHeaderState.ReadingFileName:
				goto IL_01E5;
			case GZipDecoder.GzipHeaderState.ReadingComment:
				goto IL_0212;
			case GZipDecoder.GzipHeaderState.ReadingCRC16Part1:
				goto IL_0240;
			case GZipDecoder.GzipHeaderState.ReadingCRC16Part2:
				goto IL_026A;
			case GZipDecoder.GzipHeaderState.Done:
				return true;
			default:
				throw new InvalidDataException(SR.GetString("UnknownState"));
			}
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			if (num != 139)
			{
				throw new InvalidDataException(SR.GetString("CorruptedGZipHeader"));
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCM;
			IL_00A5:
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			if (num != 8)
			{
				throw new InvalidDataException(SR.GetString("UnknownCompressionMode"));
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingFLG;
			IL_00CE:
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzip_header_flag = num;
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingMMTime;
			this.loopCounter = 0;
			IL_00F1:
			while (this.loopCounter < 4)
			{
				num = input.GetBits(8);
				if (num < 0)
				{
					return false;
				}
				this.loopCounter++;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXFL;
			this.loopCounter = 0;
			IL_0128:
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingOS;
			IL_013D:
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLen1;
			IL_0152:
			if ((this.gzip_header_flag & 4) == 0)
			{
				goto IL_01E5;
			}
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzip_header_xlen = num;
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLen2;
			IL_017B:
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzip_header_xlen |= num << 8;
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLenData;
			this.loopCounter = 0;
			IL_01A8:
			while (this.loopCounter < this.gzip_header_xlen)
			{
				num = input.GetBits(8);
				if (num < 0)
				{
					return false;
				}
				this.loopCounter++;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingFileName;
			this.loopCounter = 0;
			IL_01E5:
			if ((this.gzip_header_flag & 8) == 0)
			{
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingComment;
			}
			else
			{
				for (;;)
				{
					num = input.GetBits(8);
					if (num < 0)
					{
						break;
					}
					if (num == 0)
					{
						goto Block_20;
					}
				}
				return false;
				Block_20:
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingComment;
			}
			IL_0212:
			if ((this.gzip_header_flag & 16) == 0)
			{
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part1;
			}
			else
			{
				for (;;)
				{
					num = input.GetBits(8);
					if (num < 0)
					{
						break;
					}
					if (num == 0)
					{
						goto Block_23;
					}
				}
				return false;
				Block_23:
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part1;
			}
			IL_0240:
			if ((this.gzip_header_flag & 2) == 0)
			{
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.Done;
				return true;
			}
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part2;
			IL_026A:
			num = input.GetBits(8);
			if (num < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.Done;
			return true;
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x000B8164 File Offset: 0x000B6364
		public bool ReadFooter(InputBuffer input)
		{
			input.SkipToByteBoundary();
			if (this.gzipFooterSubstate == GZipDecoder.GzipHeaderState.ReadingCRC)
			{
				while (this.loopCounter < 4)
				{
					int bits = input.GetBits(8);
					if (bits < 0)
					{
						return false;
					}
					this.expectedCrc32 |= (uint)((uint)bits << 8 * this.loopCounter);
					this.loopCounter++;
				}
				this.gzipFooterSubstate = GZipDecoder.GzipHeaderState.ReadingFileSize;
				this.loopCounter = 0;
			}
			if (this.gzipFooterSubstate == GZipDecoder.GzipHeaderState.ReadingFileSize)
			{
				if (this.loopCounter == 0)
				{
					this.expectedOutputStreamSizeModulo = 0U;
				}
				while (this.loopCounter < 4)
				{
					int bits2 = input.GetBits(8);
					if (bits2 < 0)
					{
						return false;
					}
					this.expectedOutputStreamSizeModulo |= (uint)((uint)bits2 << 8 * this.loopCounter);
					this.loopCounter++;
				}
			}
			return true;
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x000B822C File Offset: 0x000B642C
		public void UpdateWithBytesRead(byte[] buffer, int offset, int copied)
		{
			this.actualCrc32 = Crc32Helper.UpdateCrc32(this.actualCrc32, buffer, offset, copied);
			long num = this.actualStreamSizeModulo + (long)((ulong)copied);
			if (num >= 4294967296L)
			{
				num %= 4294967296L;
			}
			this.actualStreamSizeModulo = num;
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000B8276 File Offset: 0x000B6476
		public void Validate()
		{
			if (this.expectedCrc32 != this.actualCrc32)
			{
				throw new InvalidDataException(SR.GetString("InvalidCRC"));
			}
			if (this.actualStreamSizeModulo != (long)((ulong)this.expectedOutputStreamSizeModulo))
			{
				throw new InvalidDataException(SR.GetString("InvalidStreamSize"));
			}
		}

		// Token: 0x040021C8 RID: 8648
		private GZipDecoder.GzipHeaderState gzipHeaderSubstate;

		// Token: 0x040021C9 RID: 8649
		private GZipDecoder.GzipHeaderState gzipFooterSubstate;

		// Token: 0x040021CA RID: 8650
		private int gzip_header_flag;

		// Token: 0x040021CB RID: 8651
		private int gzip_header_xlen;

		// Token: 0x040021CC RID: 8652
		private uint expectedCrc32;

		// Token: 0x040021CD RID: 8653
		private uint expectedOutputStreamSizeModulo;

		// Token: 0x040021CE RID: 8654
		private int loopCounter;

		// Token: 0x040021CF RID: 8655
		private uint actualCrc32;

		// Token: 0x040021D0 RID: 8656
		private long actualStreamSizeModulo;

		// Token: 0x0200082A RID: 2090
		internal enum GzipHeaderState
		{
			// Token: 0x040035C9 RID: 13769
			ReadingID1,
			// Token: 0x040035CA RID: 13770
			ReadingID2,
			// Token: 0x040035CB RID: 13771
			ReadingCM,
			// Token: 0x040035CC RID: 13772
			ReadingFLG,
			// Token: 0x040035CD RID: 13773
			ReadingMMTime,
			// Token: 0x040035CE RID: 13774
			ReadingXFL,
			// Token: 0x040035CF RID: 13775
			ReadingOS,
			// Token: 0x040035D0 RID: 13776
			ReadingXLen1,
			// Token: 0x040035D1 RID: 13777
			ReadingXLen2,
			// Token: 0x040035D2 RID: 13778
			ReadingXLenData,
			// Token: 0x040035D3 RID: 13779
			ReadingFileName,
			// Token: 0x040035D4 RID: 13780
			ReadingComment,
			// Token: 0x040035D5 RID: 13781
			ReadingCRC16Part1,
			// Token: 0x040035D6 RID: 13782
			ReadingCRC16Part2,
			// Token: 0x040035D7 RID: 13783
			Done,
			// Token: 0x040035D8 RID: 13784
			ReadingCRC,
			// Token: 0x040035D9 RID: 13785
			ReadingFileSize
		}

		// Token: 0x0200082B RID: 2091
		[Flags]
		internal enum GZipOptionalHeaderFlags
		{
			// Token: 0x040035DB RID: 13787
			CRCFlag = 2,
			// Token: 0x040035DC RID: 13788
			ExtraFieldsFlag = 4,
			// Token: 0x040035DD RID: 13789
			FileNameFlag = 8,
			// Token: 0x040035DE RID: 13790
			CommentFlag = 16
		}
	}
}
