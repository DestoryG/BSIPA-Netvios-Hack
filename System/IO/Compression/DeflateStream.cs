using System;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.IO.Compression
{
	// Token: 0x02000426 RID: 1062
	[global::__DynamicallyInvokable]
	public class DeflateStream : Stream
	{
		// Token: 0x060027B1 RID: 10161 RVA: 0x000B679D File Offset: 0x000B499D
		[global::__DynamicallyInvokable]
		public DeflateStream(Stream stream, CompressionMode mode)
			: this(stream, mode, false)
		{
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000B67A8 File Offset: 0x000B49A8
		internal DeflateStream(Stream stream, bool leaveOpen, IFileFormatReader reader)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException(SR.GetString("NotReadableStream"), "stream");
			}
			this.inflater = DeflateStream.CreateInflater(reader);
			this.m_CallBack = new AsyncCallback(this.ReadCallback);
			this._stream = stream;
			this._mode = CompressionMode.Decompress;
			this._leaveOpen = leaveOpen;
			this.buffer = new byte[8192];
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000B682C File Offset: 0x000B4A2C
		[global::__DynamicallyInvokable]
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (CompressionMode.Compress != mode && mode != CompressionMode.Decompress)
			{
				throw new ArgumentException(SR.GetString("ArgumentOutOfRange_Enum"), "mode");
			}
			this._stream = stream;
			this._mode = mode;
			this._leaveOpen = leaveOpen;
			CompressionMode mode2 = this._mode;
			if (mode2 != CompressionMode.Decompress)
			{
				if (mode2 == CompressionMode.Compress)
				{
					if (!this._stream.CanWrite)
					{
						throw new ArgumentException(SR.GetString("NotWriteableStream"), "stream");
					}
					this.deflater = DeflateStream.CreateDeflater(null);
					this.m_AsyncWriterDelegate = new DeflateStream.AsyncWriteDelegate(this.InternalWrite);
					this.m_CallBack = new AsyncCallback(this.WriteCallback);
				}
			}
			else
			{
				if (!this._stream.CanRead)
				{
					throw new ArgumentException(SR.GetString("NotReadableStream"), "stream");
				}
				this.inflater = DeflateStream.CreateInflater(null);
				this.m_CallBack = new AsyncCallback(this.ReadCallback);
			}
			this.buffer = new byte[8192];
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000B693D File Offset: 0x000B4B3D
		[global::__DynamicallyInvokable]
		public DeflateStream(Stream stream, CompressionLevel compressionLevel)
			: this(stream, compressionLevel, false)
		{
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000B6948 File Offset: 0x000B4B48
		[global::__DynamicallyInvokable]
		public DeflateStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException(SR.GetString("NotWriteableStream"), "stream");
			}
			this._stream = stream;
			this._mode = CompressionMode.Compress;
			this._leaveOpen = leaveOpen;
			this.deflater = DeflateStream.CreateDeflater(new CompressionLevel?(compressionLevel));
			this.m_AsyncWriterDelegate = new DeflateStream.AsyncWriteDelegate(this.InternalWrite);
			this.m_CallBack = new AsyncCallback(this.WriteCallback);
			this.buffer = new byte[8192];
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000B69E0 File Offset: 0x000B4BE0
		private static IDeflater CreateDeflater(CompressionLevel? compressionLevel)
		{
			DeflateStream.WorkerType workerType = DeflateStream.GetDeflaterType();
			if (workerType == DeflateStream.WorkerType.Managed)
			{
				return new DeflaterManaged();
			}
			if (workerType != DeflateStream.WorkerType.ZLib)
			{
				throw new SystemException("Program entered an unexpected state.");
			}
			if (compressionLevel != null)
			{
				return new DeflaterZLib(compressionLevel.Value);
			}
			return new DeflaterZLib();
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000B6A28 File Offset: 0x000B4C28
		private static IInflater CreateInflater(IFileFormatReader reader = null)
		{
			DeflateStream.WorkerType workerType = DeflateStream.GetInflaterType();
			if (workerType == DeflateStream.WorkerType.Managed)
			{
				return new Inflater(reader);
			}
			if (workerType != DeflateStream.WorkerType.ZLib)
			{
				throw new SystemException("Program entered an unexpected state.");
			}
			if (reader == null)
			{
				return new InflaterZlib(-15);
			}
			return new InflaterZlib(47);
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000B6A68 File Offset: 0x000B4C68
		[SecuritySafeCritical]
		private static DeflateStream.WorkerType GetDeflaterType()
		{
			if (DeflateStream.WorkerType.Unknown != DeflateStream.deflaterType)
			{
				return DeflateStream.deflaterType;
			}
			if (CLRConfig.CheckLegacyManagedDeflateStream())
			{
				return DeflateStream.deflaterType = DeflateStream.WorkerType.Managed;
			}
			if (!CompatibilitySwitches.IsNetFx45LegacyManagedDeflateStream)
			{
				return DeflateStream.deflaterType = DeflateStream.WorkerType.ZLib;
			}
			return DeflateStream.deflaterType = DeflateStream.WorkerType.Managed;
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000B6AA7 File Offset: 0x000B4CA7
		[SecuritySafeCritical]
		private static DeflateStream.WorkerType GetInflaterType()
		{
			if (DeflateStream.WorkerType.Unknown != DeflateStream.inflaterType)
			{
				return DeflateStream.inflaterType;
			}
			if (!LocalAppContextSwitches.DoNotUseNativeZipLibraryForDecompression)
			{
				return DeflateStream.inflaterType = DeflateStream.WorkerType.ZLib;
			}
			return DeflateStream.inflaterType = DeflateStream.WorkerType.Managed;
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000B6AD5 File Offset: 0x000B4CD5
		internal void SetFileFormatWriter(IFileFormatWriter writer)
		{
			if (writer != null)
			{
				this.formatWriter = writer;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060027BB RID: 10171 RVA: 0x000B6AE1 File Offset: 0x000B4CE1
		[global::__DynamicallyInvokable]
		public Stream BaseStream
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._stream;
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x000B6AE9 File Offset: 0x000B4CE9
		[global::__DynamicallyInvokable]
		public override bool CanRead
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._stream != null && this._mode == CompressionMode.Decompress && this._stream.CanRead;
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060027BD RID: 10173 RVA: 0x000B6B0A File Offset: 0x000B4D0A
		[global::__DynamicallyInvokable]
		public override bool CanWrite
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._stream != null && this._mode == CompressionMode.Compress && this._stream.CanWrite;
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060027BE RID: 10174 RVA: 0x000B6B2C File Offset: 0x000B4D2C
		[global::__DynamicallyInvokable]
		public override bool CanSeek
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x000B6B2F File Offset: 0x000B4D2F
		[global::__DynamicallyInvokable]
		public override long Length
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x000B6B40 File Offset: 0x000B4D40
		// (set) Token: 0x060027C1 RID: 10177 RVA: 0x000B6B51 File Offset: 0x000B4D51
		[global::__DynamicallyInvokable]
		public override long Position
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
			[global::__DynamicallyInvokable]
			set
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000B6B62 File Offset: 0x000B4D62
		[global::__DynamicallyInvokable]
		public override void Flush()
		{
			this.EnsureNotDisposed();
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000B6B6A File Offset: 0x000B4D6A
		[global::__DynamicallyInvokable]
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("NotSupported"));
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000B6B7B File Offset: 0x000B4D7B
		[global::__DynamicallyInvokable]
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("NotSupported"));
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000B6B8C File Offset: 0x000B4D8C
		[global::__DynamicallyInvokable]
		public override int Read(byte[] array, int offset, int count)
		{
			this.EnsureDecompressionMode();
			this.ValidateParameters(array, offset, count);
			this.EnsureNotDisposed();
			int num = offset;
			int num2 = count;
			for (;;)
			{
				int num3 = this.inflater.Inflate(array, num, num2);
				num += num3;
				num2 -= num3;
				if (num2 == 0 || this.inflater.Finished())
				{
					break;
				}
				int num4 = this._stream.Read(this.buffer, 0, this.buffer.Length);
				if (num4 == 0)
				{
					break;
				}
				this.inflater.SetInput(this.buffer, 0, num4);
			}
			return count - num2;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000B6C10 File Offset: 0x000B4E10
		private void ValidateParameters(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("InvalidArgumentOffsetCount"));
			}
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x000B6C61 File Offset: 0x000B4E61
		private void EnsureNotDisposed()
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x000B6C7C File Offset: 0x000B4E7C
		private void EnsureDecompressionMode()
		{
			if (this._mode != CompressionMode.Decompress)
			{
				throw new InvalidOperationException(SR.GetString("CannotReadFromDeflateStream"));
			}
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x000B6C96 File Offset: 0x000B4E96
		private void EnsureCompressionMode()
		{
			if (this._mode != CompressionMode.Compress)
			{
				throw new InvalidOperationException(SR.GetString("CannotWriteToDeflateStream"));
			}
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000B6CB4 File Offset: 0x000B4EB4
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this.EnsureDecompressionMode();
			if (this.asyncOperations != 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidBeginCall"));
			}
			this.ValidateParameters(array, offset, count);
			this.EnsureNotDisposed();
			Interlocked.Increment(ref this.asyncOperations);
			IAsyncResult asyncResult;
			try
			{
				DeflateStreamAsyncResult deflateStreamAsyncResult = new DeflateStreamAsyncResult(this, asyncState, asyncCallback, array, offset, count);
				deflateStreamAsyncResult.isWrite = false;
				int num = this.inflater.Inflate(array, offset, count);
				if (num != 0)
				{
					deflateStreamAsyncResult.InvokeCallback(true, num);
					asyncResult = deflateStreamAsyncResult;
				}
				else if (this.inflater.Finished())
				{
					deflateStreamAsyncResult.InvokeCallback(true, 0);
					asyncResult = deflateStreamAsyncResult;
				}
				else
				{
					this._stream.BeginRead(this.buffer, 0, this.buffer.Length, this.m_CallBack, deflateStreamAsyncResult);
					deflateStreamAsyncResult.m_CompletedSynchronously &= deflateStreamAsyncResult.IsCompleted;
					asyncResult = deflateStreamAsyncResult;
				}
			}
			catch
			{
				Interlocked.Decrement(ref this.asyncOperations);
				throw;
			}
			return asyncResult;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x000B6DAC File Offset: 0x000B4FAC
		private void ReadCallback(IAsyncResult baseStreamResult)
		{
			DeflateStreamAsyncResult deflateStreamAsyncResult = (DeflateStreamAsyncResult)baseStreamResult.AsyncState;
			deflateStreamAsyncResult.m_CompletedSynchronously &= baseStreamResult.CompletedSynchronously;
			try
			{
				this.EnsureNotDisposed();
				int num = this._stream.EndRead(baseStreamResult);
				if (num <= 0)
				{
					deflateStreamAsyncResult.InvokeCallback(0);
				}
				else
				{
					this.inflater.SetInput(this.buffer, 0, num);
					num = this.inflater.Inflate(deflateStreamAsyncResult.buffer, deflateStreamAsyncResult.offset, deflateStreamAsyncResult.count);
					if (num == 0 && !this.inflater.Finished())
					{
						this._stream.BeginRead(this.buffer, 0, this.buffer.Length, this.m_CallBack, deflateStreamAsyncResult);
					}
					else
					{
						deflateStreamAsyncResult.InvokeCallback(num);
					}
				}
			}
			catch (Exception ex)
			{
				deflateStreamAsyncResult.InvokeCallback(ex);
			}
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x000B6E8C File Offset: 0x000B508C
		[global::__DynamicallyInvokable]
		public override int EndRead(IAsyncResult asyncResult)
		{
			this.EnsureDecompressionMode();
			this.CheckEndXxxxLegalStateAndParams(asyncResult);
			DeflateStreamAsyncResult deflateStreamAsyncResult = (DeflateStreamAsyncResult)asyncResult;
			this.AwaitAsyncResultCompletion(deflateStreamAsyncResult);
			Exception ex = deflateStreamAsyncResult.Result as Exception;
			if (ex != null)
			{
				throw ex;
			}
			return (int)deflateStreamAsyncResult.Result;
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000B6ED0 File Offset: 0x000B50D0
		[global::__DynamicallyInvokable]
		public override void Write(byte[] array, int offset, int count)
		{
			this.EnsureCompressionMode();
			this.ValidateParameters(array, offset, count);
			this.EnsureNotDisposed();
			this.InternalWrite(array, offset, count, false);
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000B6EF1 File Offset: 0x000B50F1
		internal void InternalWrite(byte[] array, int offset, int count, bool isAsync)
		{
			this.DoMaintenance(array, offset, count);
			this.WriteDeflaterOutput(isAsync);
			this.deflater.SetInput(array, offset, count);
			this.WriteDeflaterOutput(isAsync);
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000B6F1C File Offset: 0x000B511C
		private void WriteDeflaterOutput(bool isAsync)
		{
			while (!this.deflater.NeedsInput())
			{
				int deflateOutput = this.deflater.GetDeflateOutput(this.buffer);
				if (deflateOutput > 0)
				{
					this.DoWrite(this.buffer, 0, deflateOutput, isAsync);
				}
			}
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000B6F60 File Offset: 0x000B5160
		private void DoWrite(byte[] array, int offset, int count, bool isAsync)
		{
			if (isAsync)
			{
				IAsyncResult asyncResult = this._stream.BeginWrite(array, offset, count, null, null);
				this._stream.EndWrite(asyncResult);
				return;
			}
			this._stream.Write(array, offset, count);
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000B6FA0 File Offset: 0x000B51A0
		private void DoMaintenance(byte[] array, int offset, int count)
		{
			if (count <= 0)
			{
				return;
			}
			this.wroteBytes = true;
			if (this.formatWriter == null)
			{
				return;
			}
			if (!this.wroteHeader)
			{
				byte[] header = this.formatWriter.GetHeader();
				this._stream.Write(header, 0, header.Length);
				this.wroteHeader = true;
			}
			this.formatWriter.UpdateWithBytesRead(array, offset, count);
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000B6FFC File Offset: 0x000B51FC
		private void PurgeBuffers(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			if (this._stream == null)
			{
				return;
			}
			this.Flush();
			if (this._mode != CompressionMode.Compress)
			{
				return;
			}
			if (this.wroteBytes)
			{
				this.WriteDeflaterOutput(false);
				bool flag;
				do
				{
					int num;
					flag = this.deflater.Finish(this.buffer, out num);
					if (num > 0)
					{
						this.DoWrite(this.buffer, 0, num, false);
					}
				}
				while (!flag);
			}
			if (this.formatWriter != null && this.wroteHeader)
			{
				byte[] footer = this.formatWriter.GetFooter();
				this._stream.Write(footer, 0, footer.Length);
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000B708C File Offset: 0x000B528C
		[global::__DynamicallyInvokable]
		protected override void Dispose(bool disposing)
		{
			try
			{
				this.PurgeBuffers(disposing);
			}
			finally
			{
				try
				{
					if (disposing && !this._leaveOpen && this._stream != null)
					{
						this._stream.Close();
					}
				}
				finally
				{
					this._stream = null;
					try
					{
						if (this.deflater != null)
						{
							this.deflater.Dispose();
						}
						if (this.inflater != null)
						{
							this.inflater.Dispose();
						}
					}
					finally
					{
						this.inflater = null;
						this.deflater = null;
						base.Dispose(disposing);
					}
				}
			}
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000B7134 File Offset: 0x000B5334
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this.EnsureCompressionMode();
			if (this.asyncOperations != 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidBeginCall"));
			}
			this.ValidateParameters(array, offset, count);
			this.EnsureNotDisposed();
			Interlocked.Increment(ref this.asyncOperations);
			IAsyncResult asyncResult;
			try
			{
				DeflateStreamAsyncResult deflateStreamAsyncResult = new DeflateStreamAsyncResult(this, asyncState, asyncCallback, array, offset, count);
				deflateStreamAsyncResult.isWrite = true;
				this.m_AsyncWriterDelegate.BeginInvoke(array, offset, count, true, this.m_CallBack, deflateStreamAsyncResult);
				deflateStreamAsyncResult.m_CompletedSynchronously &= deflateStreamAsyncResult.IsCompleted;
				asyncResult = deflateStreamAsyncResult;
			}
			catch
			{
				Interlocked.Decrement(ref this.asyncOperations);
				throw;
			}
			return asyncResult;
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000B71E0 File Offset: 0x000B53E0
		private void WriteCallback(IAsyncResult asyncResult)
		{
			DeflateStreamAsyncResult deflateStreamAsyncResult = (DeflateStreamAsyncResult)asyncResult.AsyncState;
			deflateStreamAsyncResult.m_CompletedSynchronously &= asyncResult.CompletedSynchronously;
			try
			{
				this.m_AsyncWriterDelegate.EndInvoke(asyncResult);
			}
			catch (Exception ex)
			{
				deflateStreamAsyncResult.InvokeCallback(ex);
				return;
			}
			deflateStreamAsyncResult.InvokeCallback(null);
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x000B723C File Offset: 0x000B543C
		[global::__DynamicallyInvokable]
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.EnsureCompressionMode();
			this.CheckEndXxxxLegalStateAndParams(asyncResult);
			DeflateStreamAsyncResult deflateStreamAsyncResult = (DeflateStreamAsyncResult)asyncResult;
			this.AwaitAsyncResultCompletion(deflateStreamAsyncResult);
			Exception ex = deflateStreamAsyncResult.Result as Exception;
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x000B7278 File Offset: 0x000B5478
		private void CheckEndXxxxLegalStateAndParams(IAsyncResult asyncResult)
		{
			if (this.asyncOperations != 1)
			{
				throw new InvalidOperationException(SR.GetString("InvalidEndCall"));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			this.EnsureNotDisposed();
			if (!(asyncResult is DeflateStreamAsyncResult))
			{
				throw new ArgumentNullException("asyncResult");
			}
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000B72C8 File Offset: 0x000B54C8
		private void AwaitAsyncResultCompletion(DeflateStreamAsyncResult asyncResult)
		{
			try
			{
				if (!asyncResult.IsCompleted)
				{
					asyncResult.AsyncWaitHandle.WaitOne();
				}
			}
			finally
			{
				Interlocked.Decrement(ref this.asyncOperations);
				asyncResult.Close();
			}
		}

		// Token: 0x04002186 RID: 8582
		internal const int DefaultBufferSize = 8192;

		// Token: 0x04002187 RID: 8583
		private const int WindowSizeUpperBound = 47;

		// Token: 0x04002188 RID: 8584
		private Stream _stream;

		// Token: 0x04002189 RID: 8585
		private CompressionMode _mode;

		// Token: 0x0400218A RID: 8586
		private bool _leaveOpen;

		// Token: 0x0400218B RID: 8587
		private IInflater inflater;

		// Token: 0x0400218C RID: 8588
		private IDeflater deflater;

		// Token: 0x0400218D RID: 8589
		private byte[] buffer;

		// Token: 0x0400218E RID: 8590
		private int asyncOperations;

		// Token: 0x0400218F RID: 8591
		private readonly AsyncCallback m_CallBack;

		// Token: 0x04002190 RID: 8592
		private readonly DeflateStream.AsyncWriteDelegate m_AsyncWriterDelegate;

		// Token: 0x04002191 RID: 8593
		private IFileFormatWriter formatWriter;

		// Token: 0x04002192 RID: 8594
		private bool wroteHeader;

		// Token: 0x04002193 RID: 8595
		private bool wroteBytes;

		// Token: 0x04002194 RID: 8596
		private static volatile DeflateStream.WorkerType deflaterType = DeflateStream.WorkerType.Unknown;

		// Token: 0x04002195 RID: 8597
		private static volatile DeflateStream.WorkerType inflaterType = DeflateStream.WorkerType.Unknown;

		// Token: 0x02000828 RID: 2088
		// (Invoke) Token: 0x06004540 RID: 17728
		internal delegate void AsyncWriteDelegate(byte[] array, int offset, int count, bool isAsync);

		// Token: 0x02000829 RID: 2089
		private enum WorkerType : byte
		{
			// Token: 0x040035C5 RID: 13765
			Managed,
			// Token: 0x040035C6 RID: 13766
			ZLib,
			// Token: 0x040035C7 RID: 13767
			Unknown
		}
	}
}
