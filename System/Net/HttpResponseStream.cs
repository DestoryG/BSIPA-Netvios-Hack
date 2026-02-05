using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020001BF RID: 447
	internal class HttpResponseStream : Stream
	{
		// Token: 0x0600118B RID: 4491 RVA: 0x0005F2FB File Offset: 0x0005D4FB
		internal HttpResponseStream(HttpListenerContext httpContext)
		{
			this.m_HttpContext = httpContext;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0005F31C File Offset: 0x0005D51C
		internal UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS ComputeLeftToWrite()
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
			if (!this.m_HttpContext.Response.ComputedHeaders)
			{
				http_FLAGS = this.m_HttpContext.Response.ComputeHeaders();
			}
			if (this.m_LeftToWrite == -9223372036854775808L)
			{
				UnsafeNclNativeMethods.HttpApi.HTTP_VERB knownMethod = this.m_HttpContext.GetKnownMethod();
				this.m_LeftToWrite = ((knownMethod != UnsafeNclNativeMethods.HttpApi.HTTP_VERB.HttpVerbHEAD) ? this.m_HttpContext.Response.ContentLength64 : 0L);
			}
			return http_FLAGS;
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x0005F38A File Offset: 0x0005D58A
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x0005F38D File Offset: 0x0005D58D
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x0005F390 File Offset: 0x0005D590
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x0005F393 File Offset: 0x0005D593
		internal bool Closed
		{
			get
			{
				return this.m_Closed;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x0005F39B File Offset: 0x0005D59B
		internal HttpListenerContext InternalHttpContext
		{
			get
			{
				return this.m_HttpContext;
			}
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0005F3A3 File Offset: 0x0005D5A3
		internal void SetClosedFlag()
		{
			this.m_Closed = true;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0005F3AC File Offset: 0x0005D5AC
		public override void Flush()
		{
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0005F3AE File Offset: 0x0005D5AE
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x0005F3B5 File Offset: 0x0005D5B5
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x0005F3C6 File Offset: 0x0005D5C6
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x0005F3D7 File Offset: 0x0005D5D7
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

		// Token: 0x06001198 RID: 4504 RVA: 0x0005F3E8 File Offset: 0x0005D5E8
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0005F3F9 File Offset: 0x0005D5F9
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0005F40A File Offset: 0x0005D60A
		public override int Read([In] [Out] byte[] buffer, int offset, int size)
		{
			throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0005F41B File Offset: 0x0005D61B
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0005F42C File Offset: 0x0005D62C
		public override int EndRead(IAsyncResult asyncResult)
		{
			throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0005F440 File Offset: 0x0005D640
		public unsafe override void Write(byte[] buffer, int offset, int size)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Write", "");
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
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = this.ComputeLeftToWrite();
			if (this.m_Closed || (size == 0 && this.m_LeftToWrite != 0L))
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Write", "");
				}
				return;
			}
			if (this.m_LeftToWrite >= 0L && (long)size > this.m_LeftToWrite)
			{
				throw new ProtocolViolationException(SR.GetString("net_entitytoobig"));
			}
			uint num = (uint)size;
			SafeLocalFree safeLocalFree = null;
			IntPtr intPtr = IntPtr.Zero;
			bool sentHeaders = this.m_HttpContext.Response.SentHeaders;
			uint num2;
			try
			{
				if (size == 0)
				{
					num2 = this.m_HttpContext.Response.SendHeaders(null, null, http_FLAGS, false);
				}
				else
				{
					try
					{
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
							byte* ptr2 = ptr;
							if (this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked)
							{
								string text = size.ToString("x", CultureInfo.InvariantCulture);
								num += (uint)(text.Length + 4);
								safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
								intPtr = safeLocalFree.DangerousGetHandle();
								for (int i = 0; i < text.Length; i++)
								{
									Marshal.WriteByte(intPtr, i, (byte)text[i]);
								}
								Marshal.WriteInt16(intPtr, text.Length, 2573);
								Marshal.Copy(buffer, offset, IntPtrHelper.Add(intPtr, text.Length + 2), size);
								Marshal.WriteInt16(intPtr, (int)(num - 2U), 2573);
								ptr2 = (byte*)(void*)intPtr;
								offset = 0;
							}
							UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK http_DATA_CHUNK = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
							http_DATA_CHUNK.DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
							http_DATA_CHUNK.pBuffer = ptr2 + offset;
							http_DATA_CHUNK.BufferLength = num;
							http_FLAGS |= ((this.m_LeftToWrite == (long)size) ? UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE : UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_MORE_DATA);
							if (!sentHeaders)
							{
								num2 = this.m_HttpContext.Response.SendHeaders(&http_DATA_CHUNK, null, http_FLAGS, false);
							}
							else
							{
								num2 = UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, (uint)http_FLAGS, 1, &http_DATA_CHUNK, null, SafeLocalFree.Zero, 0U, null, null);
								if (this.m_HttpContext.Listener.IgnoreWriteExceptions)
								{
									num2 = 0U;
								}
							}
						}
					}
					finally
					{
						byte[] array = null;
					}
				}
			}
			finally
			{
				if (safeLocalFree != null)
				{
					safeLocalFree.Close();
				}
			}
			if (num2 != 0U && num2 != 38U)
			{
				Exception ex = new HttpListenerException((int)num2);
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Write", ex);
				}
				this.m_Closed = true;
				this.m_HttpContext.Abort();
				throw ex;
			}
			this.UpdateAfterWrite(num);
			if (Logging.On)
			{
				Logging.Dump(Logging.HttpListener, this, "Write", buffer, offset, (int)num);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Write", "");
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0005F760 File Offset: 0x0005D960
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public unsafe override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
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
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = this.ComputeLeftToWrite();
			if (this.m_Closed || (size == 0 && this.m_LeftToWrite != 0L))
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "BeginWrite", "");
				}
				HttpResponseStreamAsyncResult httpResponseStreamAsyncResult = new HttpResponseStreamAsyncResult(this, state, callback);
				httpResponseStreamAsyncResult.InvokeCallback(0U);
				return httpResponseStreamAsyncResult;
			}
			if (this.m_LeftToWrite >= 0L && (long)size > this.m_LeftToWrite)
			{
				throw new ProtocolViolationException(SR.GetString("net_entitytoobig"));
			}
			uint num = 0U;
			http_FLAGS |= ((this.m_LeftToWrite == (long)size) ? UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE : UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_MORE_DATA);
			bool sentHeaders = this.m_HttpContext.Response.SentHeaders;
			HttpResponseStreamAsyncResult httpResponseStreamAsyncResult2 = new HttpResponseStreamAsyncResult(this, state, callback, buffer, offset, size, this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked, sentHeaders);
			this.UpdateAfterWrite((uint)((this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked) ? 0 : size));
			uint num2;
			try
			{
				if (!sentHeaders)
				{
					num2 = this.m_HttpContext.Response.SendHeaders(null, httpResponseStreamAsyncResult2, http_FLAGS, false);
				}
				else
				{
					this.m_HttpContext.EnsureBoundHandle();
					num2 = UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, (uint)http_FLAGS, httpResponseStreamAsyncResult2.dataChunkCount, httpResponseStreamAsyncResult2.pDataChunks, &num, SafeLocalFree.Zero, 0U, httpResponseStreamAsyncResult2.m_pOverlapped, null);
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "BeginWrite", ex);
				}
				httpResponseStreamAsyncResult2.InternalCleanup();
				this.m_Closed = true;
				this.m_HttpContext.Abort();
				throw;
			}
			if (num2 != 0U && num2 != 997U)
			{
				httpResponseStreamAsyncResult2.InternalCleanup();
				if (!this.m_HttpContext.Listener.IgnoreWriteExceptions || !sentHeaders)
				{
					Exception ex2 = new HttpListenerException((int)num2);
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "BeginWrite", ex2);
					}
					this.m_Closed = true;
					this.m_HttpContext.Abort();
					throw ex2;
				}
			}
			if (num2 == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
			{
				httpResponseStreamAsyncResult2.IOCompleted(num2, num);
			}
			if ((http_FLAGS & UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_MORE_DATA) == UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE)
			{
				this.m_LastWrite = httpResponseStreamAsyncResult2;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "BeginWrite", "");
			}
			return httpResponseStreamAsyncResult2;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0005F9C8 File Offset: 0x0005DBC8
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndWrite", "");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			HttpResponseStreamAsyncResult httpResponseStreamAsyncResult = asyncResult as HttpResponseStreamAsyncResult;
			if (httpResponseStreamAsyncResult == null || httpResponseStreamAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (httpResponseStreamAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndWrite" }));
			}
			httpResponseStreamAsyncResult.EndCalled = true;
			object obj = httpResponseStreamAsyncResult.InternalWaitForCompletion();
			Exception ex = obj as Exception;
			if (ex != null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "EndWrite", ex);
				}
				this.m_Closed = true;
				this.m_HttpContext.Abort();
				throw ex;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "EndWrite", "");
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0005FAAD File Offset: 0x0005DCAD
		private void UpdateAfterWrite(uint dataWritten)
		{
			if (!this.m_InOpaqueMode)
			{
				if (this.m_LeftToWrite > 0L)
				{
					this.m_LeftToWrite -= (long)((ulong)dataWritten);
				}
				if (this.m_LeftToWrite == 0L)
				{
					this.m_Closed = true;
				}
			}
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0005FAE0 File Offset: 0x0005DCE0
		protected unsafe override void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", "");
			}
			try
			{
				if (disposing)
				{
					if (this.m_Closed)
					{
						if (Logging.On)
						{
							Logging.Exit(Logging.HttpListener, this, "Close", "");
						}
						return;
					}
					this.m_Closed = true;
					UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = this.ComputeLeftToWrite();
					if (this.m_LeftToWrite > 0L && !this.m_InOpaqueMode)
					{
						throw new InvalidOperationException(SR.GetString("net_io_notenoughbyteswritten"));
					}
					bool sentHeaders = this.m_HttpContext.Response.SentHeaders;
					if (sentHeaders && this.m_LeftToWrite == 0L)
					{
						if (Logging.On)
						{
							Logging.Exit(Logging.HttpListener, this, "Close", "");
						}
						return;
					}
					uint num = 0U;
					if ((this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked || this.m_HttpContext.Response.BoundaryType == BoundaryType.None) && string.Compare(this.m_HttpContext.Request.HttpMethod, "HEAD", StringComparison.OrdinalIgnoreCase) != 0)
					{
						if (this.m_HttpContext.Response.BoundaryType == BoundaryType.None)
						{
							http_FLAGS |= UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_RECEIVE_REQUEST_FLAG_COPY_BODY;
						}
						try
						{
							byte[] array;
							void* ptr;
							if ((array = NclConstants.ChunkTerminator) == null || array.Length == 0)
							{
								ptr = null;
							}
							else
							{
								ptr = (void*)(&array[0]);
							}
							UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK* ptr2 = null;
							if (this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked)
							{
								UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK http_DATA_CHUNK = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
								http_DATA_CHUNK.DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
								http_DATA_CHUNK.pBuffer = (byte*)ptr;
								http_DATA_CHUNK.BufferLength = (uint)NclConstants.ChunkTerminator.Length;
								ptr2 = &http_DATA_CHUNK;
							}
							if (!sentHeaders)
							{
								num = this.m_HttpContext.Response.SendHeaders(ptr2, null, http_FLAGS, false);
								goto IL_0200;
							}
							num = UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, (uint)http_FLAGS, (ptr2 != null) ? 1 : 0, ptr2, null, SafeLocalFree.Zero, 0U, null, null);
							if (this.m_HttpContext.Listener.IgnoreWriteExceptions)
							{
								num = 0U;
							}
							goto IL_0200;
						}
						finally
						{
							byte[] array = null;
						}
					}
					if (!sentHeaders)
					{
						num = this.m_HttpContext.Response.SendHeaders(null, null, http_FLAGS, false);
					}
					IL_0200:
					if (num != 0U && num != 38U)
					{
						Exception ex = new HttpListenerException((int)num);
						if (Logging.On)
						{
							Logging.Exception(Logging.HttpListener, this, "Close", ex);
						}
						this.m_HttpContext.Abort();
						throw ex;
					}
					this.m_LeftToWrite = 0L;
				}
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

		// Token: 0x060011A2 RID: 4514 RVA: 0x0005FD88 File Offset: 0x0005DF88
		internal void SwitchToOpaqueMode()
		{
			this.m_InOpaqueMode = true;
			this.m_LeftToWrite = long.MaxValue;
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0005FDA0 File Offset: 0x0005DFA0
		internal void CancelLastWrite(CriticalHandle requestQueueHandle)
		{
			HttpResponseStreamAsyncResult lastWrite = this.m_LastWrite;
			if (lastWrite != null && !lastWrite.IsCompleted)
			{
				UnsafeNclNativeMethods.CancelIoEx(requestQueueHandle, lastWrite.m_pOverlapped);
			}
		}

		// Token: 0x04001458 RID: 5208
		private HttpListenerContext m_HttpContext;

		// Token: 0x04001459 RID: 5209
		private long m_LeftToWrite = long.MinValue;

		// Token: 0x0400145A RID: 5210
		private bool m_Closed;

		// Token: 0x0400145B RID: 5211
		private bool m_InOpaqueMode;

		// Token: 0x0400145C RID: 5212
		private HttpResponseStreamAsyncResult m_LastWrite;
	}
}
