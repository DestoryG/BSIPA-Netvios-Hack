using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Ionic.BZip2
{
	// Token: 0x0200004C RID: 76
	public class ParallelBZip2OutputStream : Stream
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x0001596F File Offset: 0x00013B6F
		public ParallelBZip2OutputStream(Stream output)
			: this(output, BZip2.MaxBlockSize, false)
		{
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001597E File Offset: 0x00013B7E
		public ParallelBZip2OutputStream(Stream output, int blockSize)
			: this(output, blockSize, false)
		{
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00015989 File Offset: 0x00013B89
		public ParallelBZip2OutputStream(Stream output, bool leaveOpen)
			: this(output, BZip2.MaxBlockSize, leaveOpen)
		{
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00015998 File Offset: 0x00013B98
		public ParallelBZip2OutputStream(Stream output, int blockSize, bool leaveOpen)
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
			this.leaveOpen = leaveOpen;
			this.combinedCRC = 0U;
			this.MaxWorkers = 16;
			this.EmitHeader();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00015A6C File Offset: 0x00013C6C
		private void InitializePoolOfWorkItems()
		{
			this.toWrite = new Queue<int>();
			this.toFill = new Queue<int>();
			this.pool = new List<WorkItem>();
			int num = ParallelBZip2OutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this.MaxWorkers);
			for (int i = 0; i < num; i++)
			{
				this.pool.Add(new WorkItem(i, this.blockSize100k));
				this.toFill.Enqueue(i);
			}
			this.newlyCompressedBlob = new AutoResetEvent(false);
			this.currentlyFilling = -1;
			this.lastFilled = -1;
			this.lastWritten = -1;
			this.latestCompressed = -1;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00015B0A File Offset: 0x00013D0A
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00015B12 File Offset: 0x00013D12
		public int MaxWorkers
		{
			get
			{
				return this._maxWorkers;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxWorkers", "Value must be 4 or greater.");
				}
				this._maxWorkers = value;
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00015B30 File Offset: 0x00013D30
		public override void Close()
		{
			if (this.pendingException != null)
			{
				this.handlingException = true;
				Exception ex = this.pendingException;
				this.pendingException = null;
				throw ex;
			}
			if (this.handlingException)
			{
				return;
			}
			if (this.output == null)
			{
				return;
			}
			Stream stream = this.output;
			try
			{
				this.FlushOutput(true);
			}
			finally
			{
				this.output = null;
				this.bw = null;
			}
			if (!this.leaveOpen)
			{
				stream.Close();
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00015BB4 File Offset: 0x00013DB4
		private void FlushOutput(bool lastInput)
		{
			if (this.emitting)
			{
				return;
			}
			if (this.currentlyFilling >= 0)
			{
				WorkItem workItem = this.pool[this.currentlyFilling];
				this.CompressOne(workItem);
				this.currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this.EmitTrailer();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00015C0D File Offset: 0x00013E0D
		public override void Flush()
		{
			if (this.output != null)
			{
				this.FlushOutput(false);
				this.bw.Flush();
				this.output.Flush();
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00015C3C File Offset: 0x00013E3C
		private void EmitHeader()
		{
			byte[] array = new byte[] { 66, 90, 104, 0 };
			array[3] = (byte)(48 + this.blockSize100k);
			byte[] array2 = array;
			this.output.Write(array2, 0, array2.Length);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00015C7C File Offset: 0x00013E7C
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

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00015CF6 File Offset: 0x00013EF6
		public int BlockSize
		{
			get
			{
				return this.blockSize100k;
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00015D00 File Offset: 0x00013F00
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool flag = false;
			if (this.output == null)
			{
				throw new IOException("the stream is not open");
			}
			if (this.pendingException != null)
			{
				this.handlingException = true;
				Exception ex = this.pendingException;
				this.pendingException = null;
				throw ex;
			}
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
			if (count == 0)
			{
				return;
			}
			if (!this.firstWriteDone)
			{
				this.InitializePoolOfWorkItems();
				this.firstWriteDone = true;
			}
			int num = 0;
			int num2 = count;
			for (;;)
			{
				this.EmitPendingBuffers(false, flag);
				flag = false;
				int num3;
				if (this.currentlyFilling >= 0)
				{
					num3 = this.currentlyFilling;
					goto IL_0106;
				}
				if (this.toFill.Count != 0)
				{
					num3 = this.toFill.Dequeue();
					this.lastFilled++;
					goto IL_0106;
				}
				flag = true;
				IL_0179:
				if (num2 <= 0)
				{
					goto Block_12;
				}
				continue;
				IL_0106:
				WorkItem workItem = this.pool[num3];
				workItem.ordinal = this.lastFilled;
				int num4 = workItem.Compressor.Fill(buffer, offset, num2);
				if (num4 != num2)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this.CompressOne), workItem))
					{
						break;
					}
					this.currentlyFilling = -1;
					offset += num4;
				}
				else
				{
					this.currentlyFilling = num3;
				}
				num2 -= num4;
				num += num4;
				goto IL_0179;
			}
			throw new Exception("Cannot enqueue workitem");
			Block_12:
			this.totalBytesWrittenIn += (long)num;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00015E9C File Offset: 0x0001409C
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this.newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = (doAll ? 200 : (mustWait ? (-1) : 0));
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this.toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this.toWrite.Count > 0)
							{
								num3 = this.toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this.toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this.pool[num3];
							if (workItem.ordinal != this.lastWritten + 1)
							{
								lock (this.toWrite)
								{
									this.toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this.newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								BitWriter bitWriter = workItem.bw;
								bitWriter.Flush();
								MemoryStream ms = workItem.ms;
								ms.Seek(0L, SeekOrigin.Begin);
								long num4 = 0L;
								byte[] array = new byte[1024];
								int num5;
								while ((num5 = ms.Read(array, 0, array.Length)) > 0)
								{
									for (int i = 0; i < num5; i++)
									{
										this.bw.WriteByte(array[i]);
									}
									num4 += (long)num5;
								}
								if (bitWriter.NumRemainingBits > 0)
								{
									this.bw.WriteBits(bitWriter.NumRemainingBits, (uint)bitWriter.RemainingBits);
								}
								this.combinedCRC = (this.combinedCRC << 1) | (this.combinedCRC >> 31);
								this.combinedCRC ^= workItem.Compressor.Crc32;
								this.totalBytesWrittenOut += num4;
								bitWriter.Reset();
								this.lastWritten = workItem.ordinal;
								workItem.ordinal = -1;
								this.toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && this.lastWritten != this.latestCompressed);
			this.emitting = false;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000160D4 File Offset: 0x000142D4
		private void CompressOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				workItem.Compressor.CompressAndWrite();
				lock (this.latestLock)
				{
					if (workItem.ordinal > this.latestCompressed)
					{
						this.latestCompressed = workItem.ordinal;
					}
				}
				lock (this.toWrite)
				{
					this.toWrite.Enqueue(workItem.index);
				}
				this.newlyCompressedBlob.Set();
			}
			catch (Exception ex)
			{
				lock (this.eLock)
				{
					if (this.pendingException != null)
					{
						this.pendingException = ex;
					}
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000161BC File Offset: 0x000143BC
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x000161BF File Offset: 0x000143BF
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x000161C2 File Offset: 0x000143C2
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

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000161E2 File Offset: 0x000143E2
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003BB RID: 955 RVA: 0x000161E9 File Offset: 0x000143E9
		// (set) Token: 0x060003BC RID: 956 RVA: 0x000161F1 File Offset: 0x000143F1
		public override long Position
		{
			get
			{
				return this.totalBytesWrittenIn;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003BD RID: 957 RVA: 0x000161F8 File Offset: 0x000143F8
		public long BytesWrittenOut
		{
			get
			{
				return this.totalBytesWrittenOut;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00016200 File Offset: 0x00014400
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00016207 File Offset: 0x00014407
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001620E File Offset: 0x0001440E
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00016218 File Offset: 0x00014418
		[Conditional("Trace")]
		private void TraceOutput(ParallelBZip2OutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this.desiredTrace) != ParallelBZip2OutputStream.TraceBits.None)
			{
				lock (this.outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.Green;
					Console.Write("{0:000} PBOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x04000235 RID: 565
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x04000236 RID: 566
		private int _maxWorkers;

		// Token: 0x04000237 RID: 567
		private bool firstWriteDone;

		// Token: 0x04000238 RID: 568
		private int lastFilled;

		// Token: 0x04000239 RID: 569
		private int lastWritten;

		// Token: 0x0400023A RID: 570
		private int latestCompressed;

		// Token: 0x0400023B RID: 571
		private int currentlyFilling;

		// Token: 0x0400023C RID: 572
		private volatile Exception pendingException;

		// Token: 0x0400023D RID: 573
		private bool handlingException;

		// Token: 0x0400023E RID: 574
		private bool emitting;

		// Token: 0x0400023F RID: 575
		private Queue<int> toWrite;

		// Token: 0x04000240 RID: 576
		private Queue<int> toFill;

		// Token: 0x04000241 RID: 577
		private List<WorkItem> pool;

		// Token: 0x04000242 RID: 578
		private object latestLock = new object();

		// Token: 0x04000243 RID: 579
		private object eLock = new object();

		// Token: 0x04000244 RID: 580
		private object outputLock = new object();

		// Token: 0x04000245 RID: 581
		private AutoResetEvent newlyCompressedBlob;

		// Token: 0x04000246 RID: 582
		private long totalBytesWrittenIn;

		// Token: 0x04000247 RID: 583
		private long totalBytesWrittenOut;

		// Token: 0x04000248 RID: 584
		private bool leaveOpen;

		// Token: 0x04000249 RID: 585
		private uint combinedCRC;

		// Token: 0x0400024A RID: 586
		private Stream output;

		// Token: 0x0400024B RID: 587
		private BitWriter bw;

		// Token: 0x0400024C RID: 588
		private int blockSize100k;

		// Token: 0x0400024D RID: 589
		private ParallelBZip2OutputStream.TraceBits desiredTrace = ParallelBZip2OutputStream.TraceBits.Crc | ParallelBZip2OutputStream.TraceBits.Write;

		// Token: 0x0200004D RID: 77
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x0400024F RID: 591
			None = 0U,
			// Token: 0x04000250 RID: 592
			Crc = 1U,
			// Token: 0x04000251 RID: 593
			Write = 2U,
			// Token: 0x04000252 RID: 594
			All = 4294967295U
		}
	}
}
