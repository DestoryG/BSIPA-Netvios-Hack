using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020001BE RID: 446
	internal class HttpRequestStream : Stream
	{
		// Token: 0x06001173 RID: 4467 RVA: 0x0005EC3F File Offset: 0x0005CE3F
		internal HttpRequestStream(HttpListenerContext httpContext)
		{
			this.m_HttpContext = httpContext;
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x0005EC4E File Offset: 0x0005CE4E
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x0005EC51 File Offset: 0x0005CE51
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x0005EC54 File Offset: 0x0005CE54
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x0005EC57 File Offset: 0x0005CE57
		internal bool Closed
		{
			get
			{
				return this.m_Closed;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x0005EC5F File Offset: 0x0005CE5F
		internal bool BufferedDataChunksAvailable
		{
			get
			{
				return this.m_DataChunkIndex > -1;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x0005EC6A File Offset: 0x0005CE6A
		internal HttpListenerContext InternalHttpContext
		{
			get
			{
				return this.m_HttpContext;
			}
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0005EC72 File Offset: 0x0005CE72
		public override void Flush()
		{
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0005EC74 File Offset: 0x0005CE74
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x0005EC7B File Offset: 0x0005CE7B
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x0005EC8C File Offset: 0x0005CE8C
		// (set) Token: 0x0600117E RID: 4478 RVA: 0x0005EC9D File Offset: 0x0005CE9D
		public override long Position
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0005ECAE File Offset: 0x0005CEAE
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0005ECBF File Offset: 0x0005CEBF
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0005ECD0 File Offset: 0x0005CED0
		public unsafe override int Read([In] [Out] byte[] buffer, int offset, int size)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Read", "");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (size == 0 || this.m_Closed)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Read", "dataRead:0");
				}
				return 0;
			}
			uint num = 0U;
			if (this.m_DataChunkIndex != -1)
			{
				num = UnsafeNclNativeMethods.HttpApi.GetChunks(this.m_HttpContext.Request.RequestBuffer, this.m_HttpContext.Request.OriginalBlobAddress, ref this.m_DataChunkIndex, ref this.m_DataChunkOffset, buffer, offset, size);
			}
			if (this.m_DataChunkIndex == -1 && (ulong)num < (ulong)((long)size))
			{
				uint num2 = 0U;
				offset += (int)num;
				size -= (int)num;
				if (size > 131072)
				{
					size = 131072;
				}
				uint num4;
				fixed (byte[] array = buffer)
				{
					byte* ptr;
					if (buffer == null || array.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array[0];
					}
					uint num3 = 0U;
					if (!this.m_InOpaqueMode)
					{
						num3 = 1U;
					}
					num4 = UnsafeNclNativeMethods.HttpApi.HttpReceiveRequestEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, num3, (void*)(ptr + offset), (uint)size, out num2, null);
					num += num2;
				}
				if (num4 != 0U && num4 != 38U)
				{
					Exception ex = new HttpListenerException((int)num4);
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "Read", ex);
					}
					throw ex;
				}
				this.UpdateAfterRead(num4, num);
			}
			if (Logging.On)
			{
				Logging.Dump(Logging.HttpListener, this, "Read", buffer, offset, (int)num);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Read", "dataRead:" + num.ToString());
			}
			return (int)num;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0005EE9B File Offset: 0x0005D09B
		private void UpdateAfterRead(uint statusCode, uint dataRead)
		{
			if (statusCode == 38U || dataRead == 0U)
			{
				this.Close();
			}
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0005EEAC File Offset: 0x0005D0AC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public unsafe override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "BeginRead", "");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (size == 0 || this.m_Closed)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "BeginRead", "");
				}
				HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback);
				httpRequestStreamAsyncResult.InvokeCallback(0U);
				return httpRequestStreamAsyncResult;
			}
			HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult2 = null;
			uint num = 0U;
			if (this.m_DataChunkIndex != -1)
			{
				num = UnsafeNclNativeMethods.HttpApi.GetChunks(this.m_HttpContext.Request.RequestBuffer, this.m_HttpContext.Request.OriginalBlobAddress, ref this.m_DataChunkIndex, ref this.m_DataChunkOffset, buffer, offset, size);
				if (this.m_DataChunkIndex != -1 && (ulong)num == (ulong)((long)size))
				{
					httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, buffer, offset, (uint)size, 0U);
					httpRequestStreamAsyncResult2.InvokeCallback(num);
				}
			}
			if (this.m_DataChunkIndex == -1 && (ulong)num < (ulong)((long)size))
			{
				uint num2 = 0U;
				offset += (int)num;
				size -= (int)num;
				if (size > 131072)
				{
					size = 131072;
				}
				httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, buffer, offset, (uint)size, num);
				uint num4;
				try
				{
					try
					{
						fixed (byte[] array = buffer)
						{
							if (buffer == null || array.Length == 0)
							{
								byte* ptr = null;
							}
							else
							{
								byte* ptr = &array[0];
							}
							this.m_HttpContext.EnsureBoundHandle();
							uint num3 = 0U;
							if (!this.m_InOpaqueMode)
							{
								num3 = 1U;
							}
							num2 = UnsafeNclNativeMethods.HttpApi.HttpReceiveRequestEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, num3, httpRequestStreamAsyncResult2.m_pPinnedBuffer, (uint)size, out num4, httpRequestStreamAsyncResult2.m_pOverlapped);
						}
					}
					finally
					{
						byte[] array = null;
					}
				}
				catch (Exception ex)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "BeginRead", ex);
					}
					httpRequestStreamAsyncResult2.InternalCleanup();
					throw;
				}
				if (num2 != 0U && num2 != 997U)
				{
					httpRequestStreamAsyncResult2.InternalCleanup();
					if (num2 != 38U)
					{
						Exception ex2 = new HttpListenerException((int)num2);
						if (Logging.On)
						{
							Logging.Exception(Logging.HttpListener, this, "BeginRead", ex2);
						}
						httpRequestStreamAsyncResult2.InternalCleanup();
						throw ex2;
					}
					httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, num);
					httpRequestStreamAsyncResult2.InvokeCallback(0U);
				}
				else if (num2 == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
				{
					httpRequestStreamAsyncResult2.IOCompleted(num2, num4);
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "BeginRead", "");
			}
			return httpRequestStreamAsyncResult2;
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0005F130 File Offset: 0x0005D330
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndRead", "");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = asyncResult as HttpRequestStream.HttpRequestStreamAsyncResult;
			if (httpRequestStreamAsyncResult == null || httpRequestStreamAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (httpRequestStreamAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndRead" }));
			}
			httpRequestStreamAsyncResult.EndCalled = true;
			object obj = httpRequestStreamAsyncResult.InternalWaitForCompletion();
			Exception ex = obj as Exception;
			if (ex != null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "EndRead", ex);
				}
				throw ex;
			}
			uint num = (uint)obj;
			this.UpdateAfterRead((uint)httpRequestStreamAsyncResult.ErrorCode, num);
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "EndRead", "");
			}
			return (int)(num + httpRequestStreamAsyncResult.m_dataAlreadyRead);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0005F21F File Offset: 0x0005D41F
		public override void Write(byte[] buffer, int offset, int size)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0005F230 File Offset: 0x0005D430
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0005F241 File Offset: 0x0005D441
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0005F254 File Offset: 0x0005D454
		protected override void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Dispose", "");
			}
			try
			{
				this.m_Closed = true;
			}
			finally
			{
				base.Dispose(disposing);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Dispose", "");
			}
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0005F2BC File Offset: 0x0005D4BC
		internal void SwitchToOpaqueMode()
		{
			this.m_InOpaqueMode = true;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0005F2C5 File Offset: 0x0005D4C5
		internal uint GetChunks(byte[] buffer, int offset, int size)
		{
			return UnsafeNclNativeMethods.HttpApi.GetChunks(this.m_HttpContext.Request.RequestBuffer, this.m_HttpContext.Request.OriginalBlobAddress, ref this.m_DataChunkIndex, ref this.m_DataChunkOffset, buffer, offset, size);
		}

		// Token: 0x04001452 RID: 5202
		private HttpListenerContext m_HttpContext;

		// Token: 0x04001453 RID: 5203
		private uint m_DataChunkOffset;

		// Token: 0x04001454 RID: 5204
		private int m_DataChunkIndex;

		// Token: 0x04001455 RID: 5205
		private bool m_Closed;

		// Token: 0x04001456 RID: 5206
		internal const int MaxReadSize = 131072;

		// Token: 0x04001457 RID: 5207
		private bool m_InOpaqueMode;

		// Token: 0x02000751 RID: 1873
		private class HttpRequestStreamAsyncResult : LazyAsyncResult
		{
			// Token: 0x060041EC RID: 16876 RVA: 0x00111F10 File Offset: 0x00110110
			internal HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback)
				: base(asyncObject, userState, callback)
			{
			}

			// Token: 0x060041ED RID: 16877 RVA: 0x00111F1B File Offset: 0x0011011B
			internal HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, uint dataAlreadyRead)
				: base(asyncObject, userState, callback)
			{
				this.m_dataAlreadyRead = dataAlreadyRead;
			}

			// Token: 0x060041EE RID: 16878 RVA: 0x00111F30 File Offset: 0x00110130
			internal unsafe HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, byte[] buffer, int offset, uint size, uint dataAlreadyRead)
				: base(asyncObject, userState, callback)
			{
				this.m_dataAlreadyRead = dataAlreadyRead;
				this.m_pOverlapped = new Overlapped
				{
					AsyncResult = this
				}.Pack(HttpRequestStream.HttpRequestStreamAsyncResult.s_IOCallback, buffer);
				this.m_pPinnedBuffer = (void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
			}

			// Token: 0x060041EF RID: 16879 RVA: 0x00111F82 File Offset: 0x00110182
			internal void IOCompleted(uint errorCode, uint numBytes)
			{
				HttpRequestStream.HttpRequestStreamAsyncResult.IOCompleted(this, errorCode, numBytes);
			}

			// Token: 0x060041F0 RID: 16880 RVA: 0x00111F8C File Offset: 0x0011018C
			private static void IOCompleted(HttpRequestStream.HttpRequestStreamAsyncResult asyncResult, uint errorCode, uint numBytes)
			{
				object obj = null;
				try
				{
					if (errorCode != 0U && errorCode != 38U)
					{
						asyncResult.ErrorCode = (int)errorCode;
						obj = new HttpListenerException((int)errorCode);
					}
					else
					{
						obj = numBytes;
						if (Logging.On)
						{
							Logging.Dump(Logging.HttpListener, asyncResult, "Callback", (IntPtr)asyncResult.m_pPinnedBuffer, (int)numBytes);
						}
					}
				}
				catch (Exception ex)
				{
					obj = ex;
				}
				asyncResult.InvokeCallback(obj);
			}

			// Token: 0x060041F1 RID: 16881 RVA: 0x00111FFC File Offset: 0x001101FC
			private unsafe static void Callback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
			{
				Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
				HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = overlapped.AsyncResult as HttpRequestStream.HttpRequestStreamAsyncResult;
				HttpRequestStream.HttpRequestStreamAsyncResult.IOCompleted(httpRequestStreamAsyncResult, errorCode, numBytes);
			}

			// Token: 0x060041F2 RID: 16882 RVA: 0x00112024 File Offset: 0x00110224
			protected override void Cleanup()
			{
				base.Cleanup();
				if (this.m_pOverlapped != null)
				{
					Overlapped.Free(this.m_pOverlapped);
				}
			}

			// Token: 0x040031FA RID: 12794
			internal unsafe NativeOverlapped* m_pOverlapped;

			// Token: 0x040031FB RID: 12795
			internal unsafe void* m_pPinnedBuffer;

			// Token: 0x040031FC RID: 12796
			internal uint m_dataAlreadyRead;

			// Token: 0x040031FD RID: 12797
			private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(HttpRequestStream.HttpRequestStreamAsyncResult.Callback);
		}
	}
}
