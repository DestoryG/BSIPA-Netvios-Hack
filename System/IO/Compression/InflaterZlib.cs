using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.IO.Compression
{
	// Token: 0x02000423 RID: 1059
	internal class InflaterZlib : IInflater, IDisposable
	{
		// Token: 0x06002793 RID: 10131 RVA: 0x000B61C4 File Offset: 0x000B43C4
		internal InflaterZlib(int windowBits)
		{
			this._finished = false;
			this._isDisposed = false;
			this.InflateInit(windowBits);
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06002794 RID: 10132 RVA: 0x000B61EC File Offset: 0x000B43EC
		public int AvailableOutput
		{
			get
			{
				return (int)this._zlibStream.AvailOut;
			}
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000B61F9 File Offset: 0x000B43F9
		public bool Finished()
		{
			return this._finished;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000B6204 File Offset: 0x000B4404
		public int Inflate(byte[] bytes, int offset, int length)
		{
			if (length == 0)
			{
				return 0;
			}
			int num2;
			try
			{
				int num;
				ZLibNative.ErrorCode errorCode = this.ReadInflateOutput(bytes, offset, length, ZLibNative.FlushCode.NoFlush, out num);
				if (errorCode == ZLibNative.ErrorCode.StreamEnd)
				{
					this._finished = true;
				}
				num2 = num;
			}
			finally
			{
				if (this._zlibStream.AvailIn == 0U && this._inputBufferHandle.IsAllocated)
				{
					this.DeallocateInputBufferHandle();
				}
			}
			return num2;
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000B6268 File Offset: 0x000B4468
		public bool NeedsInput()
		{
			return this._zlibStream.AvailIn == 0U;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000B6278 File Offset: 0x000B4478
		public void SetInput(byte[] inputBuffer, int startIndex, int count)
		{
			if (count == 0)
			{
				return;
			}
			object syncLock = this._syncLock;
			lock (syncLock)
			{
				this._inputBufferHandle = GCHandle.Alloc(inputBuffer, GCHandleType.Pinned);
				this._isValid = 1;
				this._zlibStream.NextIn = this._inputBufferHandle.AddrOfPinnedObject() + startIndex;
				this._zlibStream.AvailIn = (uint)count;
				this._finished = false;
			}
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000B62FC File Offset: 0x000B44FC
		[SecuritySafeCritical]
		protected virtual void Dispose(bool disposing)
		{
			if (!this._isDisposed)
			{
				if (disposing)
				{
					this._zlibStream.Dispose();
				}
				if (this._inputBufferHandle.IsAllocated)
				{
					this.DeallocateInputBufferHandle();
				}
				this._isDisposed = true;
			}
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000B632E File Offset: 0x000B452E
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000B6340 File Offset: 0x000B4540
		~InflaterZlib()
		{
			if (!Environment.HasShutdownStarted)
			{
				this.Dispose(false);
			}
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000B6378 File Offset: 0x000B4578
		[SecuritySafeCritical]
		private void InflateInit(int windowBits)
		{
			ZLibNative.ErrorCode errorCode;
			try
			{
				errorCode = ZLibNative.CreateZLibStreamForInflate(out this._zlibStream, windowBits);
			}
			catch (Exception ex)
			{
				throw new ZLibException(SR.GetString("ZLibErrorDLLLoadError"), ex);
			}
			switch (errorCode)
			{
			case ZLibNative.ErrorCode.VersionError:
				throw new ZLibException(SR.GetString("ZLibErrorVersionMismatch"), "inflateInit2_", (int)errorCode, this._zlibStream.GetErrorMessage());
			case ZLibNative.ErrorCode.MemError:
				throw new ZLibException(SR.GetString("ZLibErrorNotEnoughMemory"), "inflateInit2_", (int)errorCode, this._zlibStream.GetErrorMessage());
			case ZLibNative.ErrorCode.StreamError:
				throw new ZLibException(SR.GetString("ZLibErrorIncorrectInitParameters"), "inflateInit2_", (int)errorCode, this._zlibStream.GetErrorMessage());
			case ZLibNative.ErrorCode.Ok:
				return;
			}
			throw new ZLibException(SR.GetString("ZLibErrorUnexpected"), "inflateInit2_", (int)errorCode, this._zlibStream.GetErrorMessage());
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000B6464 File Offset: 0x000B4664
		private unsafe ZLibNative.ErrorCode ReadInflateOutput(byte[] outputBuffer, int offset, int length, ZLibNative.FlushCode flushCode, out int bytesRead)
		{
			object syncLock = this._syncLock;
			ZLibNative.ErrorCode errorCode2;
			lock (syncLock)
			{
				byte* ptr;
				if (outputBuffer == null || outputBuffer.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &outputBuffer[0];
				}
				this._zlibStream.NextOut = (IntPtr)((void*)ptr) + offset;
				this._zlibStream.AvailOut = (uint)length;
				ZLibNative.ErrorCode errorCode = this.Inflate(flushCode);
				bytesRead = length - (int)this._zlibStream.AvailOut;
				errorCode2 = errorCode;
			}
			return errorCode2;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000B64FC File Offset: 0x000B46FC
		[SecuritySafeCritical]
		private ZLibNative.ErrorCode Inflate(ZLibNative.FlushCode flushCode)
		{
			ZLibNative.ErrorCode errorCode;
			try
			{
				errorCode = this._zlibStream.Inflate(flushCode);
			}
			catch (Exception ex)
			{
				throw new ZLibException(SR.GetString("ZLibErrorDLLLoadError"), ex);
			}
			switch (errorCode)
			{
			case ZLibNative.ErrorCode.BufError:
				return errorCode;
			case ZLibNative.ErrorCode.MemError:
				throw new ZLibException(SR.GetString("ZLibErrorNotEnoughMemory"), "inflate_", (int)errorCode, this._zlibStream.GetErrorMessage());
			case ZLibNative.ErrorCode.DataError:
				throw new InvalidDataException(SR.GetString("GenericInvalidData"));
			case ZLibNative.ErrorCode.StreamError:
				throw new ZLibException(SR.GetString("ZLibErrorInconsistentStream"), "inflate_", (int)errorCode, this._zlibStream.GetErrorMessage());
			case ZLibNative.ErrorCode.Ok:
			case ZLibNative.ErrorCode.StreamEnd:
				return errorCode;
			}
			throw new ZLibException(SR.GetString("ZLibErrorUnexpected"), "inflate_", (int)errorCode, this._zlibStream.GetErrorMessage());
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000B65D8 File Offset: 0x000B47D8
		private void DeallocateInputBufferHandle()
		{
			object syncLock = this._syncLock;
			lock (syncLock)
			{
				this._zlibStream.AvailIn = 0U;
				this._zlibStream.NextIn = ZLibNative.ZNullPtr;
				if (Interlocked.Exchange(ref this._isValid, 0) != 0)
				{
					this._inputBufferHandle.Free();
				}
			}
		}

		// Token: 0x04002177 RID: 8567
		private bool _finished;

		// Token: 0x04002178 RID: 8568
		private bool _isDisposed;

		// Token: 0x04002179 RID: 8569
		private ZLibNative.ZLibStreamHandle _zlibStream;

		// Token: 0x0400217A RID: 8570
		private GCHandle _inputBufferHandle;

		// Token: 0x0400217B RID: 8571
		private readonly object _syncLock = new object();

		// Token: 0x0400217C RID: 8572
		private int _isValid;
	}
}
