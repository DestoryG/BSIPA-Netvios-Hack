using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x0200005E RID: 94
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x06000453 RID: 1107 RVA: 0x0001EC4A File Offset: 0x0001CE4A
		public ParallelDeflateOutputStream(Stream stream)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0001EC56 File Offset: 0x0001CE56
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level)
			: this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001EC62 File Offset: 0x0001CE62
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001EC6E File Offset: 0x0001CE6E
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0001EC7C File Offset: 0x0001CE7C
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0001ECEB File Offset: 0x0001CEEB
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0001ECF3 File Offset: 0x0001CEF3
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0001ECFC File Offset: 0x0001CEFC
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0001ED04 File Offset: 0x0001CF04
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0001ED21 File Offset: 0x0001CF21
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x0001ED29 File Offset: 0x0001CF29
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0001ED4A File Offset: 0x0001CF4A
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0001ED52 File Offset: 0x0001CF52
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001ED5C File Offset: 0x0001CF5C
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001EE14 File Offset: 0x0001D014
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool flag = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			for (;;)
			{
				this.EmitPendingBuffers(false, flag);
				flag = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_009A;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_009A;
				}
				flag = true;
				IL_014C:
				if (count <= 0)
				{
					return;
				}
				continue;
				IL_009A:
				WorkItem workItem = this._pool[num];
				int num2 = ((workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable));
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
					{
						break;
					}
					this._currentlyFilling = -1;
				}
				else
				{
					this._currentlyFilling = num;
				}
				goto IL_014C;
			}
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001EF74 File Offset: 0x0001D174
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001F030 File Offset: 0x0001D230
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem workItem = this._pool[this._currentlyFilling];
				this._DeflateOne(workItem);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001F098 File Offset: 0x0001D298
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001F0DC File Offset: 0x0001D2DC
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001F141 File Offset: 0x0001D341
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001F157 File Offset: 0x0001D357
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001F160 File Offset: 0x0001D360
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem workItem in this._pool)
			{
				this._toFill.Enqueue(workItem.index);
				workItem.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001F228 File Offset: 0x0001D428
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = (doAll ? 200 : (mustWait ? (-1) : 0));
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								lock (this._toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
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
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
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
			while (doAll && this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001F3C0 File Offset: 0x0001D5C0
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				int index = workItem.index;
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				lock (this._latestLock)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				lock (this._toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception ex)
			{
				lock (this._eLock)
				{
					if (this._pendingException != null)
					{
						this._pendingException = ex;
					}
				}
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001F4D4 File Offset: 0x0001D6D4
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001F548 File Offset: 0x0001D748
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				lock (this._outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001F5BB File Offset: 0x0001D7BB
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x0001F5BE File Offset: 0x0001D7BE
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0001F5CB File Offset: 0x0001D7CB
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x0001F5D2 File Offset: 0x0001D7D2
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x0001F5DF File Offset: 0x0001D7DF
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001F5E6 File Offset: 0x0001D7E6
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001F5ED File Offset: 0x0001D7ED
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001F5F4 File Offset: 0x0001D7F4
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000330 RID: 816
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x04000331 RID: 817
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x04000332 RID: 818
		private List<WorkItem> _pool;

		// Token: 0x04000333 RID: 819
		private bool _leaveOpen;

		// Token: 0x04000334 RID: 820
		private bool emitting;

		// Token: 0x04000335 RID: 821
		private Stream _outStream;

		// Token: 0x04000336 RID: 822
		private int _maxBufferPairs;

		// Token: 0x04000337 RID: 823
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x04000338 RID: 824
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x04000339 RID: 825
		private object _outputLock = new object();

		// Token: 0x0400033A RID: 826
		private bool _isClosed;

		// Token: 0x0400033B RID: 827
		private bool _firstWriteDone;

		// Token: 0x0400033C RID: 828
		private int _currentlyFilling;

		// Token: 0x0400033D RID: 829
		private int _lastFilled;

		// Token: 0x0400033E RID: 830
		private int _lastWritten;

		// Token: 0x0400033F RID: 831
		private int _latestCompressed;

		// Token: 0x04000340 RID: 832
		private int _Crc32;

		// Token: 0x04000341 RID: 833
		private CRC32 _runningCrc;

		// Token: 0x04000342 RID: 834
		private object _latestLock = new object();

		// Token: 0x04000343 RID: 835
		private Queue<int> _toWrite;

		// Token: 0x04000344 RID: 836
		private Queue<int> _toFill;

		// Token: 0x04000345 RID: 837
		private long _totalBytesProcessed;

		// Token: 0x04000346 RID: 838
		private CompressionLevel _compressLevel;

		// Token: 0x04000347 RID: 839
		private volatile Exception _pendingException;

		// Token: 0x04000348 RID: 840
		private bool _handlingException;

		// Token: 0x04000349 RID: 841
		private object _eLock = new object();

		// Token: 0x0400034A RID: 842
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x0200005F RID: 95
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x0400034D RID: 845
			None = 0U,
			// Token: 0x0400034E RID: 846
			NotUsed1 = 1U,
			// Token: 0x0400034F RID: 847
			EmitLock = 2U,
			// Token: 0x04000350 RID: 848
			EmitEnter = 4U,
			// Token: 0x04000351 RID: 849
			EmitBegin = 8U,
			// Token: 0x04000352 RID: 850
			EmitDone = 16U,
			// Token: 0x04000353 RID: 851
			EmitSkip = 32U,
			// Token: 0x04000354 RID: 852
			EmitAll = 58U,
			// Token: 0x04000355 RID: 853
			Flush = 64U,
			// Token: 0x04000356 RID: 854
			Lifecycle = 128U,
			// Token: 0x04000357 RID: 855
			Session = 256U,
			// Token: 0x04000358 RID: 856
			Synch = 512U,
			// Token: 0x04000359 RID: 857
			Instance = 1024U,
			// Token: 0x0400035A RID: 858
			Compress = 2048U,
			// Token: 0x0400035B RID: 859
			Write = 4096U,
			// Token: 0x0400035C RID: 860
			WriteEnter = 8192U,
			// Token: 0x0400035D RID: 861
			WriteTake = 16384U,
			// Token: 0x0400035E RID: 862
			All = 4294967295U
		}
	}
}
