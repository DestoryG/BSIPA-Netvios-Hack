using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Ionic.BZip2
{
	// Token: 0x02000049 RID: 73
	public class BZip2OutputStream : Stream
	{
		// Token: 0x0600038F RID: 911 RVA: 0x000154FB File Offset: 0x000136FB
		public BZip2OutputStream(Stream output)
			: this(output, BZip2.MaxBlockSize, false)
		{
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001550A File Offset: 0x0001370A
		public BZip2OutputStream(Stream output, int blockSize)
			: this(output, blockSize, false)
		{
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00015515 File Offset: 0x00013715
		public BZip2OutputStream(Stream output, bool leaveOpen)
			: this(output, BZip2.MaxBlockSize, leaveOpen)
		{
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00015524 File Offset: 0x00013724
		public BZip2OutputStream(Stream output, int blockSize, bool leaveOpen)
		{
			if (blockSize < BZip2.MinBlockSize || blockSize > BZip2.MaxBlockSize)
			{
				string text = string.Format("blockSize={0} is out of range; must be between {1} and {2}", blockSize, BZip2.MinBlockSize, BZip2.MaxBlockSize);
				throw new ArgumentException(text, "blockSize");
			}
			this.output = output;
			if (!this.output.CanWrite)
			{
				throw new ArgumentException("The stream is not writable.", "output");
			}
			this.bw = new BitWriter(this.output);
			this.blockSize100k = blockSize;
			this.compressor = new BZip2Compressor(this.bw, blockSize);
			this.leaveOpen = leaveOpen;
			this.combinedCRC = 0U;
			this.EmitHeader();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000155E4 File Offset: 0x000137E4
		public override void Close()
		{
			if (this.output != null)
			{
				Stream stream = this.output;
				this.Finish();
				if (!this.leaveOpen)
				{
					stream.Close();
				}
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00015614 File Offset: 0x00013814
		public override void Flush()
		{
			if (this.output != null)
			{
				this.bw.Flush();
				this.output.Flush();
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001563C File Offset: 0x0001383C
		private void EmitHeader()
		{
			byte[] array = new byte[] { 66, 90, 104, 0 };
			array[3] = (byte)(48 + this.blockSize100k);
			byte[] array2 = array;
			this.output.Write(array2, 0, array2.Length);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001567C File Offset: 0x0001387C
		private void EmitTrailer()
		{
			this.bw.WriteByte(23);
			this.bw.WriteByte(114);
			this.bw.WriteByte(69);
			this.bw.WriteByte(56);
			this.bw.WriteByte(80);
			this.bw.WriteByte(144);
			this.bw.WriteInt(this.combinedCRC);
			this.bw.FinishAndPad();
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000156F8 File Offset: 0x000138F8
		private void Finish()
		{
			try
			{
				int totalBytesWrittenOut = this.bw.TotalBytesWrittenOut;
				this.compressor.CompressAndWrite();
				this.combinedCRC = (this.combinedCRC << 1) | (this.combinedCRC >> 31);
				this.combinedCRC ^= this.compressor.Crc32;
				this.EmitTrailer();
			}
			finally
			{
				this.output = null;
				this.compressor = null;
				this.bw = null;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0001577C File Offset: 0x0001397C
		public int BlockSize
		{
			get
			{
				return this.blockSize100k;
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00015784 File Offset: 0x00013984
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (offset < 0)
			{
				throw new IndexOutOfRangeException(string.Format("offset ({0}) must be > 0", offset));
			}
			if (count < 0)
			{
				throw new IndexOutOfRangeException(string.Format("count ({0}) must be > 0", count));
			}
			if (offset + count > buffer.Length)
			{
				throw new IndexOutOfRangeException(string.Format("offset({0}) count({1}) bLength({2})", offset, count, buffer.Length));
			}
			if (this.output == null)
			{
				throw new IOException("the stream is not open");
			}
			if (count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = count;
			do
			{
				int num3 = this.compressor.Fill(buffer, offset, num2);
				if (num3 != num2)
				{
					int totalBytesWrittenOut = this.bw.TotalBytesWrittenOut;
					this.compressor.CompressAndWrite();
					this.combinedCRC = (this.combinedCRC << 1) | (this.combinedCRC >> 31);
					this.combinedCRC ^= this.compressor.Crc32;
					offset += num3;
				}
				num2 -= num3;
				num += num3;
			}
			while (num2 > 0);
			this.totalBytesWrittenIn += num;
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00015885 File Offset: 0x00013A85
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00015888 File Offset: 0x00013A88
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0001588B File Offset: 0x00013A8B
		public override bool CanWrite
		{
			get
			{
				if (this.output == null)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.output.CanWrite;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600039D RID: 925 RVA: 0x000158AB File Offset: 0x00013AAB
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600039E RID: 926 RVA: 0x000158B2 File Offset: 0x00013AB2
		// (set) Token: 0x0600039F RID: 927 RVA: 0x000158BB File Offset: 0x00013ABB
		public override long Position
		{
			get
			{
				return (long)this.totalBytesWrittenIn;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000158C2 File Offset: 0x00013AC2
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000158C9 File Offset: 0x00013AC9
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000158D0 File Offset: 0x00013AD0
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000158D8 File Offset: 0x00013AD8
		[Conditional("Trace")]
		private void TraceOutput(BZip2OutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this.desiredTrace) != BZip2OutputStream.TraceBits.None)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = hashCode % 8 + ConsoleColor.Green;
				Console.Write("{0:000} PBOS ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}

		// Token: 0x04000223 RID: 547
		private int totalBytesWrittenIn;

		// Token: 0x04000224 RID: 548
		private bool leaveOpen;

		// Token: 0x04000225 RID: 549
		private BZip2Compressor compressor;

		// Token: 0x04000226 RID: 550
		private uint combinedCRC;

		// Token: 0x04000227 RID: 551
		private Stream output;

		// Token: 0x04000228 RID: 552
		private BitWriter bw;

		// Token: 0x04000229 RID: 553
		private int blockSize100k;

		// Token: 0x0400022A RID: 554
		private BZip2OutputStream.TraceBits desiredTrace = BZip2OutputStream.TraceBits.Crc | BZip2OutputStream.TraceBits.Write;

		// Token: 0x0200004A RID: 74
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x0400022C RID: 556
			None = 0U,
			// Token: 0x0400022D RID: 557
			Crc = 1U,
			// Token: 0x0400022E RID: 558
			Write = 2U,
			// Token: 0x0400022F RID: 559
			All = 4294967295U
		}
	}
}
