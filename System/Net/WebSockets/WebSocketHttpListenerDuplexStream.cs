using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
	// Token: 0x02000238 RID: 568
	internal class WebSocketHttpListenerDuplexStream : Stream, WebSocketBase.IWebSocketStream
	{
		// Token: 0x0600155C RID: 5468 RVA: 0x0006F343 File Offset: 0x0006D543
		public WebSocketHttpListenerDuplexStream(HttpRequestStream inputStream, HttpResponseStream outputStream, HttpListenerContext context)
		{
			this.m_InputStream = inputStream;
			this.m_OutputStream = outputStream;
			this.m_Context = context;
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Associate(Logging.WebSockets, inputStream, this);
				Logging.Associate(Logging.WebSockets, outputStream, this);
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x0600155D RID: 5469 RVA: 0x0006F37F File Offset: 0x0006D57F
		public override bool CanRead
		{
			get
			{
				return this.m_InputStream.CanRead;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x0006F38C File Offset: 0x0006D58C
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x0006F38F File Offset: 0x0006D58F
		public override bool CanTimeout
		{
			get
			{
				return this.m_InputStream.CanTimeout && this.m_OutputStream.CanTimeout;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x0006F3AB File Offset: 0x0006D5AB
		public override bool CanWrite
		{
			get
			{
				return this.m_OutputStream.CanWrite;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x0006F3B8 File Offset: 0x0006D5B8
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x0006F3C9 File Offset: 0x0006D5C9
		// (set) Token: 0x06001563 RID: 5475 RVA: 0x0006F3DA File Offset: 0x0006D5DA
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

		// Token: 0x06001564 RID: 5476 RVA: 0x0006F3EB File Offset: 0x0006D5EB
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.m_InputStream.Read(buffer, offset, count);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0006F3FB File Offset: 0x0006D5FB
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			WebSocketHelpers.ValidateBuffer(buffer, offset, count);
			return this.ReadAsyncCore(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0006F410 File Offset: 0x0006D610
		private async Task<int> ReadAsyncCore(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "ReadAsyncCore", WebSocketHelpers.GetTraceMsgForParameters(offset, count, cancellationToken));
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			int bytesRead = 0;
			try
			{
				if (cancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration = cancellationToken.Register(WebSocketHttpListenerDuplexStream.s_OnCancel, this, false);
				}
				if (!this.m_InOpaqueMode)
				{
					int num = await this.m_InputStream.ReadAsync(buffer, offset, count, cancellationToken).SuppressContextFlow<int>();
					bytesRead = num;
				}
				else
				{
					this.m_ReadTaskCompletionSource = new TaskCompletionSource<int>();
					this.m_ReadEventArgs.SetBuffer(buffer, offset, count);
					if (!this.ReadAsyncFast(this.m_ReadEventArgs))
					{
						if (this.m_ReadEventArgs.Exception != null)
						{
							throw this.m_ReadEventArgs.Exception;
						}
						bytesRead = this.m_ReadEventArgs.BytesTransferred;
					}
					else
					{
						bytesRead = await this.m_ReadTaskCompletionSource.Task.SuppressContextFlow<int>();
					}
				}
			}
			catch (Exception ex)
			{
				if (WebSocketHttpListenerDuplexStream.s_CanHandleException(ex))
				{
					cancellationToken.ThrowIfCancellationRequested();
				}
				throw;
			}
			finally
			{
				cancellationTokenRegistration.Dispose();
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "ReadAsyncCore", bytesRead);
				}
			}
			return bytesRead;
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0006F474 File Offset: 0x0006D674
		private unsafe bool ReadAsyncFast(WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs eventArgs)
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "ReadAsyncFast", string.Empty);
			}
			eventArgs.StartOperationCommon(this);
			eventArgs.StartOperationReceive();
			bool flag = false;
			try
			{
				if (eventArgs.Count == 0 || this.m_InputStream.Closed)
				{
					eventArgs.FinishOperationSuccess(0, true);
					return false;
				}
				uint num = 0U;
				int num2 = eventArgs.Offset;
				int num3 = eventArgs.Count;
				if (this.m_InputStream.BufferedDataChunksAvailable)
				{
					num = this.m_InputStream.GetChunks(eventArgs.Buffer, eventArgs.Offset, eventArgs.Count);
					if (this.m_InputStream.BufferedDataChunksAvailable && (ulong)num == (ulong)((long)eventArgs.Count))
					{
						eventArgs.FinishOperationSuccess(eventArgs.Count, true);
						return false;
					}
				}
				if (num != 0U)
				{
					num2 += (int)num;
					num3 -= (int)num;
					if (num3 > 131072)
					{
						num3 = 131072;
					}
					eventArgs.SetBuffer(eventArgs.Buffer, num2, num3);
				}
				else if (num3 > 131072)
				{
					num3 = 131072;
					eventArgs.SetBuffer(eventArgs.Buffer, num2, num3);
				}
				this.m_InputStream.InternalHttpContext.EnsureBoundHandle();
				uint num4 = 0U;
				uint num5 = 0U;
				uint num6 = UnsafeNclNativeMethods.HttpApi.HttpReceiveRequestEntityBody2(this.m_InputStream.InternalHttpContext.RequestQueueHandle, this.m_InputStream.InternalHttpContext.RequestId, num4, (void*)this.m_WebSocket.InternalBuffer.ToIntPtr(eventArgs.Offset), (uint)eventArgs.Count, out num5, eventArgs.NativeOverlapped);
				if (num6 != 0U && num6 != 997U && num6 != 38U)
				{
					throw new HttpListenerException((int)num6);
				}
				if (num6 == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
				{
					eventArgs.FinishOperationSuccess((int)num5, true);
					flag = false;
				}
				else if (num6 == 38U)
				{
					eventArgs.FinishOperationSuccess(0, true);
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			catch (Exception ex)
			{
				this.m_ReadEventArgs.FinishOperationFailure(ex, true);
				this.m_OutputStream.SetClosedFlag();
				this.m_OutputStream.InternalHttpContext.Abort();
				throw;
			}
			finally
			{
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "ReadAsyncFast", flag);
				}
			}
			return flag;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0006F6B8 File Offset: 0x0006D8B8
		public override int ReadByte()
		{
			return this.m_InputStream.ReadByte();
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001569 RID: 5481 RVA: 0x0006F6C5 File Offset: 0x0006D8C5
		public bool SupportsMultipleWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0006F6C8 File Offset: 0x0006D8C8
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.m_InputStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0006F6DC File Offset: 0x0006D8DC
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.m_InputStream.EndRead(asyncResult);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0006F6EC File Offset: 0x0006D8EC
		public Task MultipleWriteAsync(IList<ArraySegment<byte>> sendBuffers, CancellationToken cancellationToken)
		{
			if (sendBuffers.Count == 1)
			{
				ArraySegment<byte> arraySegment = sendBuffers[0];
				return this.WriteAsync(arraySegment.Array, arraySegment.Offset, arraySegment.Count, cancellationToken);
			}
			return this.MultipleWriteAsyncCore(sendBuffers, cancellationToken);
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0006F730 File Offset: 0x0006D930
		private async Task MultipleWriteAsyncCore(IList<ArraySegment<byte>> sendBuffers, CancellationToken cancellationToken)
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "MultipleWriteAsyncCore", string.Empty);
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			try
			{
				if (cancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration = cancellationToken.Register(WebSocketHttpListenerDuplexStream.s_OnCancel, this, false);
				}
				this.m_WriteTaskCompletionSource = new TaskCompletionSource<object>();
				this.m_WriteEventArgs.SetBuffer(null, 0, 0);
				this.m_WriteEventArgs.BufferList = sendBuffers;
				if (this.WriteAsyncFast(this.m_WriteEventArgs))
				{
					await this.m_WriteTaskCompletionSource.Task.SuppressContextFlow<object>();
				}
			}
			catch (Exception ex)
			{
				if (WebSocketHttpListenerDuplexStream.s_CanHandleException(ex))
				{
					cancellationToken.ThrowIfCancellationRequested();
				}
				throw;
			}
			finally
			{
				cancellationTokenRegistration.Dispose();
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "MultipleWriteAsyncCore", string.Empty);
				}
			}
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0006F783 File Offset: 0x0006D983
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.m_OutputStream.Write(buffer, offset, count);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0006F793 File Offset: 0x0006D993
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			WebSocketHelpers.ValidateBuffer(buffer, offset, count);
			return this.WriteAsyncCore(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0006F7A8 File Offset: 0x0006D9A8
		private async Task WriteAsyncCore(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "WriteAsyncCore", WebSocketHelpers.GetTraceMsgForParameters(offset, count, cancellationToken));
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			try
			{
				if (cancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration = cancellationToken.Register(WebSocketHttpListenerDuplexStream.s_OnCancel, this, false);
				}
				if (!this.m_InOpaqueMode)
				{
					await this.m_OutputStream.WriteAsync(buffer, offset, count, cancellationToken).SuppressContextFlow();
				}
				else
				{
					this.m_WriteTaskCompletionSource = new TaskCompletionSource<object>();
					this.m_WriteEventArgs.BufferList = null;
					this.m_WriteEventArgs.SetBuffer(buffer, offset, count);
					if (this.WriteAsyncFast(this.m_WriteEventArgs))
					{
						await this.m_WriteTaskCompletionSource.Task.SuppressContextFlow<object>();
					}
				}
			}
			catch (Exception ex)
			{
				if (WebSocketHttpListenerDuplexStream.s_CanHandleException(ex))
				{
					cancellationToken.ThrowIfCancellationRequested();
				}
				throw;
			}
			finally
			{
				cancellationTokenRegistration.Dispose();
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "WriteAsyncCore", string.Empty);
				}
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0006F80C File Offset: 0x0006DA0C
		private bool WriteAsyncFast(WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs eventArgs)
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "WriteAsyncFast", string.Empty);
			}
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
			eventArgs.StartOperationCommon(this);
			eventArgs.StartOperationSend();
			bool flag = false;
			try
			{
				if (this.m_OutputStream.Closed || (eventArgs.Buffer != null && eventArgs.Count == 0))
				{
					eventArgs.FinishOperationSuccess(eventArgs.Count, true);
					return false;
				}
				if (eventArgs.ShouldCloseOutput)
				{
					http_FLAGS |= UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_RECEIVE_REQUEST_FLAG_COPY_BODY;
				}
				else
				{
					http_FLAGS |= UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_MORE_DATA;
					http_FLAGS |= UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_BUFFER_DATA;
				}
				this.m_OutputStream.InternalHttpContext.EnsureBoundHandle();
				uint num2;
				uint num = UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody2(this.m_OutputStream.InternalHttpContext.RequestQueueHandle, this.m_OutputStream.InternalHttpContext.RequestId, (uint)http_FLAGS, eventArgs.EntityChunkCount, eventArgs.EntityChunks, out num2, SafeLocalFree.Zero, 0U, eventArgs.NativeOverlapped, IntPtr.Zero);
				if (num != 0U && num != 997U)
				{
					throw new HttpListenerException((int)num);
				}
				if (num == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
				{
					eventArgs.FinishOperationSuccess((int)num2, true);
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			catch (Exception ex)
			{
				this.m_WriteEventArgs.FinishOperationFailure(ex, true);
				this.m_OutputStream.SetClosedFlag();
				this.m_OutputStream.InternalHttpContext.Abort();
				throw;
			}
			finally
			{
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "WriteAsyncFast", flag);
				}
			}
			return flag;
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0006F97C File Offset: 0x0006DB7C
		public override void WriteByte(byte value)
		{
			this.m_OutputStream.WriteByte(value);
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0006F98A File Offset: 0x0006DB8A
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.m_OutputStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0006F99E File Offset: 0x0006DB9E
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_OutputStream.EndWrite(asyncResult);
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0006F9AC File Offset: 0x0006DBAC
		public override void Flush()
		{
			this.m_OutputStream.Flush();
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0006F9B9 File Offset: 0x0006DBB9
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this.m_OutputStream.FlushAsync(cancellationToken);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0006F9C7 File Offset: 0x0006DBC7
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0006F9D8 File Offset: 0x0006DBD8
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0006F9EC File Offset: 0x0006DBEC
		public async Task CloseNetworkConnectionAsync(CancellationToken cancellationToken)
		{
			await Task.Yield();
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "CloseNetworkConnectionAsync", string.Empty);
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			try
			{
				if (cancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration = cancellationToken.Register(WebSocketHttpListenerDuplexStream.s_OnCancel, this, false);
				}
				this.m_WriteTaskCompletionSource = new TaskCompletionSource<object>();
				this.m_WriteEventArgs.SetShouldCloseOutput();
				if (this.WriteAsyncFast(this.m_WriteEventArgs))
				{
					await this.m_WriteTaskCompletionSource.Task.SuppressContextFlow<object>();
				}
			}
			catch (Exception ex)
			{
				if (!WebSocketHttpListenerDuplexStream.s_CanHandleException(ex))
				{
					throw;
				}
				cancellationToken.ThrowIfCancellationRequested();
			}
			finally
			{
				cancellationTokenRegistration.Dispose();
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "CloseNetworkConnectionAsync", string.Empty);
				}
			}
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0006FA38 File Offset: 0x0006DC38
		protected override void Dispose(bool disposing)
		{
			if (disposing && Interlocked.Exchange(ref this.m_CleanedUp, 1) == 0)
			{
				if (this.m_ReadTaskCompletionSource != null)
				{
					this.m_ReadTaskCompletionSource.TrySetCanceled();
				}
				if (this.m_WriteTaskCompletionSource != null)
				{
					this.m_WriteTaskCompletionSource.TrySetCanceled();
				}
				if (this.m_ReadEventArgs != null)
				{
					this.m_ReadEventArgs.Dispose();
				}
				if (this.m_WriteEventArgs != null)
				{
					this.m_WriteEventArgs.Dispose();
				}
				try
				{
					this.m_InputStream.Close();
				}
				finally
				{
					this.m_OutputStream.Close();
				}
			}
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0006FAD0 File Offset: 0x0006DCD0
		public void Abort()
		{
			WebSocketHttpListenerDuplexStream.OnCancel(this);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0006FAD8 File Offset: 0x0006DCD8
		private static bool CanHandleException(Exception error)
		{
			return error is HttpListenerException || error is ObjectDisposedException || error is IOException;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0006FAF8 File Offset: 0x0006DCF8
		private static void OnCancel(object state)
		{
			WebSocketHttpListenerDuplexStream webSocketHttpListenerDuplexStream = state as WebSocketHttpListenerDuplexStream;
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, state, "OnCancel", string.Empty);
			}
			try
			{
				webSocketHttpListenerDuplexStream.m_OutputStream.SetClosedFlag();
				webSocketHttpListenerDuplexStream.m_Context.Abort();
			}
			catch
			{
			}
			TaskCompletionSource<int> readTaskCompletionSource = webSocketHttpListenerDuplexStream.m_ReadTaskCompletionSource;
			if (readTaskCompletionSource != null)
			{
				readTaskCompletionSource.TrySetCanceled();
			}
			TaskCompletionSource<object> writeTaskCompletionSource = webSocketHttpListenerDuplexStream.m_WriteTaskCompletionSource;
			if (writeTaskCompletionSource != null)
			{
				writeTaskCompletionSource.TrySetCanceled();
			}
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Exit(Logging.WebSockets, state, "OnCancel", string.Empty);
			}
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0006FB94 File Offset: 0x0006DD94
		public void SwitchToOpaqueMode(WebSocketBase webSocket)
		{
			if (this.m_InOpaqueMode)
			{
				throw new InvalidOperationException();
			}
			this.m_WebSocket = webSocket;
			this.m_InOpaqueMode = true;
			this.m_ReadEventArgs = new WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs(webSocket, this);
			this.m_ReadEventArgs.Completed += WebSocketHttpListenerDuplexStream.s_OnReadCompleted;
			this.m_WriteEventArgs = new WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs(webSocket, this);
			this.m_WriteEventArgs.Completed += WebSocketHttpListenerDuplexStream.s_OnWriteCompleted;
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Associate(Logging.WebSockets, this, webSocket);
			}
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0006FC0C File Offset: 0x0006DE0C
		private static void OnWriteCompleted(object sender, WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs eventArgs)
		{
			WebSocketHttpListenerDuplexStream currentStream = eventArgs.CurrentStream;
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, currentStream, "OnWriteCompleted", string.Empty);
			}
			if (eventArgs.Exception != null)
			{
				currentStream.m_WriteTaskCompletionSource.TrySetException(eventArgs.Exception);
			}
			else
			{
				currentStream.m_WriteTaskCompletionSource.TrySetResult(null);
			}
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Exit(Logging.WebSockets, currentStream, "OnWriteCompleted", string.Empty);
			}
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0006FC84 File Offset: 0x0006DE84
		private static void OnReadCompleted(object sender, WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs eventArgs)
		{
			WebSocketHttpListenerDuplexStream currentStream = eventArgs.CurrentStream;
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, currentStream, "OnReadCompleted", string.Empty);
			}
			if (eventArgs.Exception != null)
			{
				currentStream.m_ReadTaskCompletionSource.TrySetException(eventArgs.Exception);
			}
			else
			{
				currentStream.m_ReadTaskCompletionSource.TrySetResult(eventArgs.BytesTransferred);
			}
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Exit(Logging.WebSockets, currentStream, "OnReadCompleted", string.Empty);
			}
		}

		// Token: 0x040016AF RID: 5807
		private static readonly EventHandler<WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs> s_OnReadCompleted = new EventHandler<WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs>(WebSocketHttpListenerDuplexStream.OnReadCompleted);

		// Token: 0x040016B0 RID: 5808
		private static readonly EventHandler<WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs> s_OnWriteCompleted = new EventHandler<WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs>(WebSocketHttpListenerDuplexStream.OnWriteCompleted);

		// Token: 0x040016B1 RID: 5809
		private static readonly Func<Exception, bool> s_CanHandleException = new Func<Exception, bool>(WebSocketHttpListenerDuplexStream.CanHandleException);

		// Token: 0x040016B2 RID: 5810
		private static readonly Action<object> s_OnCancel = new Action<object>(WebSocketHttpListenerDuplexStream.OnCancel);

		// Token: 0x040016B3 RID: 5811
		private readonly HttpRequestStream m_InputStream;

		// Token: 0x040016B4 RID: 5812
		private readonly HttpResponseStream m_OutputStream;

		// Token: 0x040016B5 RID: 5813
		private HttpListenerContext m_Context;

		// Token: 0x040016B6 RID: 5814
		private bool m_InOpaqueMode;

		// Token: 0x040016B7 RID: 5815
		private WebSocketBase m_WebSocket;

		// Token: 0x040016B8 RID: 5816
		private WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs m_WriteEventArgs;

		// Token: 0x040016B9 RID: 5817
		private WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs m_ReadEventArgs;

		// Token: 0x040016BA RID: 5818
		private TaskCompletionSource<object> m_WriteTaskCompletionSource;

		// Token: 0x040016BB RID: 5819
		private TaskCompletionSource<int> m_ReadTaskCompletionSource;

		// Token: 0x040016BC RID: 5820
		private int m_CleanedUp;

		// Token: 0x02000782 RID: 1922
		internal class HttpListenerAsyncEventArgs : EventArgs, IDisposable
		{
			// Token: 0x14000072 RID: 114
			// (add) Token: 0x060042A4 RID: 17060 RVA: 0x00116C28 File Offset: 0x00114E28
			// (remove) Token: 0x060042A5 RID: 17061 RVA: 0x00116C60 File Offset: 0x00114E60
			private event EventHandler<WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs> m_Completed;

			// Token: 0x060042A6 RID: 17062 RVA: 0x00116C95 File Offset: 0x00114E95
			public HttpListenerAsyncEventArgs(WebSocketBase webSocket, WebSocketHttpListenerDuplexStream stream)
			{
				this.m_WebSocket = webSocket;
				this.m_CurrentStream = stream;
				this.m_AllocateOverlappedOnDemand = LocalAppContextSwitches.AllocateOverlappedOnDemand;
				if (!this.m_AllocateOverlappedOnDemand)
				{
					this.InitializeOverlapped();
				}
			}

			// Token: 0x17000F31 RID: 3889
			// (get) Token: 0x060042A7 RID: 17063 RVA: 0x00116CC4 File Offset: 0x00114EC4
			public int BytesTransferred
			{
				get
				{
					return this.m_BytesTransferred;
				}
			}

			// Token: 0x17000F32 RID: 3890
			// (get) Token: 0x060042A8 RID: 17064 RVA: 0x00116CCC File Offset: 0x00114ECC
			public byte[] Buffer
			{
				get
				{
					return this.m_Buffer;
				}
			}

			// Token: 0x17000F33 RID: 3891
			// (get) Token: 0x060042A9 RID: 17065 RVA: 0x00116CD4 File Offset: 0x00114ED4
			// (set) Token: 0x060042AA RID: 17066 RVA: 0x00116CDC File Offset: 0x00114EDC
			public IList<ArraySegment<byte>> BufferList
			{
				get
				{
					return this.m_BufferList;
				}
				set
				{
					this.m_BufferList = value;
				}
			}

			// Token: 0x17000F34 RID: 3892
			// (get) Token: 0x060042AB RID: 17067 RVA: 0x00116CE5 File Offset: 0x00114EE5
			public bool ShouldCloseOutput
			{
				get
				{
					return this.m_ShouldCloseOutput;
				}
			}

			// Token: 0x17000F35 RID: 3893
			// (get) Token: 0x060042AC RID: 17068 RVA: 0x00116CED File Offset: 0x00114EED
			public int Offset
			{
				get
				{
					return this.m_Offset;
				}
			}

			// Token: 0x17000F36 RID: 3894
			// (get) Token: 0x060042AD RID: 17069 RVA: 0x00116CF5 File Offset: 0x00114EF5
			public int Count
			{
				get
				{
					return this.m_Count;
				}
			}

			// Token: 0x17000F37 RID: 3895
			// (get) Token: 0x060042AE RID: 17070 RVA: 0x00116CFD File Offset: 0x00114EFD
			public Exception Exception
			{
				get
				{
					return this.m_Exception;
				}
			}

			// Token: 0x17000F38 RID: 3896
			// (get) Token: 0x060042AF RID: 17071 RVA: 0x00116D05 File Offset: 0x00114F05
			public ushort EntityChunkCount
			{
				get
				{
					if (this.m_DataChunks == null)
					{
						return 0;
					}
					return this.m_DataChunkCount;
				}
			}

			// Token: 0x17000F39 RID: 3897
			// (get) Token: 0x060042B0 RID: 17072 RVA: 0x00116D17 File Offset: 0x00114F17
			public SafeNativeOverlapped NativeOverlapped
			{
				get
				{
					return this.m_PtrNativeOverlapped;
				}
			}

			// Token: 0x17000F3A RID: 3898
			// (get) Token: 0x060042B1 RID: 17073 RVA: 0x00116D1F File Offset: 0x00114F1F
			public IntPtr EntityChunks
			{
				get
				{
					if (this.m_DataChunks == null)
					{
						return IntPtr.Zero;
					}
					return Marshal.UnsafeAddrOfPinnedArrayElement(this.m_DataChunks, 0);
				}
			}

			// Token: 0x17000F3B RID: 3899
			// (get) Token: 0x060042B2 RID: 17074 RVA: 0x00116D3B File Offset: 0x00114F3B
			public WebSocketHttpListenerDuplexStream CurrentStream
			{
				get
				{
					return this.m_CurrentStream;
				}
			}

			// Token: 0x14000073 RID: 115
			// (add) Token: 0x060042B3 RID: 17075 RVA: 0x00116D43 File Offset: 0x00114F43
			// (remove) Token: 0x060042B4 RID: 17076 RVA: 0x00116D4C File Offset: 0x00114F4C
			public event EventHandler<WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs> Completed
			{
				add
				{
					this.m_Completed += value;
				}
				remove
				{
					this.m_Completed -= value;
				}
			}

			// Token: 0x060042B5 RID: 17077 RVA: 0x00116D58 File Offset: 0x00114F58
			protected virtual void OnCompleted(WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs e)
			{
				EventHandler<WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs> completed = this.m_Completed;
				if (completed != null)
				{
					completed(e.m_CurrentStream, e);
				}
			}

			// Token: 0x060042B6 RID: 17078 RVA: 0x00116D7C File Offset: 0x00114F7C
			public void SetShouldCloseOutput()
			{
				this.m_BufferList = null;
				this.m_Buffer = null;
				this.m_ShouldCloseOutput = true;
			}

			// Token: 0x060042B7 RID: 17079 RVA: 0x00116D93 File Offset: 0x00114F93
			public void Dispose()
			{
				this.m_DisposeCalled = true;
				if (Interlocked.CompareExchange(ref this.m_Operating, 2, 0) != 0)
				{
					return;
				}
				if (!this.m_AllocateOverlappedOnDemand)
				{
					this.FreeOverlapped(false);
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x060042B8 RID: 17080 RVA: 0x00116DC4 File Offset: 0x00114FC4
			~HttpListenerAsyncEventArgs()
			{
				if (!this.m_AllocateOverlappedOnDemand)
				{
					this.FreeOverlapped(true);
				}
			}

			// Token: 0x060042B9 RID: 17081 RVA: 0x00116DFC File Offset: 0x00114FFC
			private void InitializeOverlapped()
			{
				this.m_Overlapped = new Overlapped();
				this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), null));
			}

			// Token: 0x060042BA RID: 17082 RVA: 0x00116E2C File Offset: 0x0011502C
			private void FreeOverlapped(bool checkForShutdown)
			{
				if (!checkForShutdown || !NclUtilities.HasShutdownStarted)
				{
					if (this.m_PtrNativeOverlapped != null && !this.m_PtrNativeOverlapped.IsInvalid)
					{
						this.m_PtrNativeOverlapped.Dispose();
					}
					if (this.m_DataChunksGCHandle.IsAllocated)
					{
						this.m_DataChunksGCHandle.Free();
						if (this.m_AllocateOverlappedOnDemand)
						{
							this.m_DataChunks = null;
						}
					}
				}
			}

			// Token: 0x060042BB RID: 17083 RVA: 0x00116E8C File Offset: 0x0011508C
			internal void StartOperationCommon(WebSocketHttpListenerDuplexStream currentStream)
			{
				if (Interlocked.CompareExchange(ref this.m_Operating, 1, 0) == 0)
				{
					if (this.m_AllocateOverlappedOnDemand)
					{
						this.InitializeOverlapped();
					}
					else
					{
						this.NativeOverlapped.ReinitializeNativeOverlapped();
					}
					this.m_Exception = null;
					this.m_BytesTransferred = 0;
					return;
				}
				if (this.m_DisposeCalled)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				throw new InvalidOperationException();
			}

			// Token: 0x060042BC RID: 17084 RVA: 0x00116EF0 File Offset: 0x001150F0
			internal void StartOperationReceive()
			{
				this.m_CompletedOperation = WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs.HttpListenerAsyncOperation.Receive;
			}

			// Token: 0x060042BD RID: 17085 RVA: 0x00116EF9 File Offset: 0x001150F9
			internal void StartOperationSend()
			{
				this.UpdateDataChunk();
				this.m_CompletedOperation = WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs.HttpListenerAsyncOperation.Send;
			}

			// Token: 0x060042BE RID: 17086 RVA: 0x00116F08 File Offset: 0x00115108
			public void SetBuffer(byte[] buffer, int offset, int count)
			{
				this.m_Buffer = buffer;
				this.m_Offset = offset;
				this.m_Count = count;
			}

			// Token: 0x060042BF RID: 17087 RVA: 0x00116F20 File Offset: 0x00115120
			private void UpdateDataChunk()
			{
				if (this.m_DataChunks == null)
				{
					this.m_DataChunks = new UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK[2];
					this.m_DataChunksGCHandle = GCHandle.Alloc(this.m_DataChunks, GCHandleType.Pinned);
					this.m_DataChunks[0] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
					this.m_DataChunks[0].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
					this.m_DataChunks[1] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
					this.m_DataChunks[1].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				}
				if (this.m_Buffer != null)
				{
					this.UpdateDataChunk(0, this.m_Buffer, this.m_Offset, this.m_Count);
					this.UpdateDataChunk(1, null, 0, 0);
					this.m_DataChunkCount = 1;
					return;
				}
				if (this.m_BufferList != null)
				{
					this.UpdateDataChunk(0, this.m_BufferList[0].Array, this.m_BufferList[0].Offset, this.m_BufferList[0].Count);
					this.UpdateDataChunk(1, this.m_BufferList[1].Array, this.m_BufferList[1].Offset, this.m_BufferList[1].Count);
					this.m_DataChunkCount = 2;
					return;
				}
				this.m_DataChunks = null;
			}

			// Token: 0x060042C0 RID: 17088 RVA: 0x00117070 File Offset: 0x00115270
			private unsafe void UpdateDataChunk(int index, byte[] buffer, int offset, int count)
			{
				if (buffer == null)
				{
					this.m_DataChunks[index].pBuffer = null;
					this.m_DataChunks[index].BufferLength = 0U;
					return;
				}
				if (this.m_WebSocket.InternalBuffer.IsInternalBuffer(buffer, offset, count))
				{
					this.m_DataChunks[index].pBuffer = (byte*)(void*)this.m_WebSocket.InternalBuffer.ToIntPtr(offset);
				}
				else
				{
					this.m_DataChunks[index].pBuffer = (byte*)(void*)this.m_WebSocket.InternalBuffer.ConvertPinnedSendPayloadToNative(buffer, offset, count);
				}
				this.m_DataChunks[index].BufferLength = (uint)count;
			}

			// Token: 0x060042C1 RID: 17089 RVA: 0x00117122 File Offset: 0x00115322
			internal void Complete()
			{
				if (this.m_AllocateOverlappedOnDemand)
				{
					this.FreeOverlapped(false);
					Interlocked.Exchange(ref this.m_Operating, 0);
				}
				else
				{
					this.m_Operating = 0;
				}
				if (this.m_DisposeCalled)
				{
					this.Dispose();
				}
			}

			// Token: 0x060042C2 RID: 17090 RVA: 0x00117157 File Offset: 0x00115357
			private void SetResults(Exception exception, int bytesTransferred)
			{
				this.m_Exception = exception;
				this.m_BytesTransferred = bytesTransferred;
			}

			// Token: 0x060042C3 RID: 17091 RVA: 0x00117168 File Offset: 0x00115368
			internal void FinishOperationFailure(Exception exception, bool syncCompletion)
			{
				this.SetResults(exception, 0);
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.PrintError(Logging.WebSockets, this.m_CurrentStream, (this.m_CompletedOperation == WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs.HttpListenerAsyncOperation.Receive) ? "ReadAsyncFast" : "WriteAsyncFast", exception.ToString());
				}
				this.Complete();
				this.OnCompleted(this);
			}

			// Token: 0x060042C4 RID: 17092 RVA: 0x001171BC File Offset: 0x001153BC
			internal void FinishOperationSuccess(int bytesTransferred, bool syncCompletion)
			{
				this.SetResults(null, bytesTransferred);
				if (WebSocketBase.LoggingEnabled)
				{
					if (this.m_Buffer != null)
					{
						Logging.Dump(Logging.WebSockets, this.m_CurrentStream, (this.m_CompletedOperation == WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs.HttpListenerAsyncOperation.Receive) ? "ReadAsyncFast" : "WriteAsyncFast", this.m_Buffer, this.m_Offset, bytesTransferred);
					}
					else
					{
						if (this.m_BufferList != null)
						{
							using (IEnumerator<ArraySegment<byte>> enumerator = this.BufferList.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									ArraySegment<byte> arraySegment = enumerator.Current;
									Logging.Dump(Logging.WebSockets, this, "WriteAsyncFast", arraySegment.Array, arraySegment.Offset, arraySegment.Count);
								}
								goto IL_00EA;
							}
						}
						Logging.PrintLine(Logging.WebSockets, TraceEventType.Verbose, 0, string.Format(CultureInfo.InvariantCulture, "Output channel closed for {0}#{1}", new object[]
						{
							this.m_CurrentStream.GetType().Name,
							ValidationHelper.HashString(this.m_CurrentStream)
						}));
					}
				}
				IL_00EA:
				if (this.m_ShouldCloseOutput)
				{
					this.m_CurrentStream.m_OutputStream.SetClosedFlag();
				}
				this.Complete();
				this.OnCompleted(this);
			}

			// Token: 0x060042C5 RID: 17093 RVA: 0x001172E8 File Offset: 0x001154E8
			private unsafe void CompletionPortCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
			{
				if (errorCode == 0U || errorCode == 38U)
				{
					this.FinishOperationSuccess((int)numBytes, false);
					return;
				}
				this.FinishOperationFailure(new HttpListenerException((int)errorCode), false);
			}

			// Token: 0x04003307 RID: 13063
			private const int Free = 0;

			// Token: 0x04003308 RID: 13064
			private const int InProgress = 1;

			// Token: 0x04003309 RID: 13065
			private const int Disposed = 2;

			// Token: 0x0400330A RID: 13066
			private int m_Operating;

			// Token: 0x0400330B RID: 13067
			private bool m_DisposeCalled;

			// Token: 0x0400330C RID: 13068
			private SafeNativeOverlapped m_PtrNativeOverlapped;

			// Token: 0x0400330D RID: 13069
			private Overlapped m_Overlapped;

			// Token: 0x0400330F RID: 13071
			private byte[] m_Buffer;

			// Token: 0x04003310 RID: 13072
			private IList<ArraySegment<byte>> m_BufferList;

			// Token: 0x04003311 RID: 13073
			private int m_Count;

			// Token: 0x04003312 RID: 13074
			private int m_Offset;

			// Token: 0x04003313 RID: 13075
			private int m_BytesTransferred;

			// Token: 0x04003314 RID: 13076
			private WebSocketHttpListenerDuplexStream.HttpListenerAsyncEventArgs.HttpListenerAsyncOperation m_CompletedOperation;

			// Token: 0x04003315 RID: 13077
			private UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK[] m_DataChunks;

			// Token: 0x04003316 RID: 13078
			private GCHandle m_DataChunksGCHandle;

			// Token: 0x04003317 RID: 13079
			private ushort m_DataChunkCount;

			// Token: 0x04003318 RID: 13080
			private Exception m_Exception;

			// Token: 0x04003319 RID: 13081
			private bool m_ShouldCloseOutput;

			// Token: 0x0400331A RID: 13082
			private readonly WebSocketBase m_WebSocket;

			// Token: 0x0400331B RID: 13083
			private readonly WebSocketHttpListenerDuplexStream m_CurrentStream;

			// Token: 0x0400331C RID: 13084
			private readonly bool m_AllocateOverlappedOnDemand;

			// Token: 0x02000920 RID: 2336
			public enum HttpListenerAsyncOperation
			{
				// Token: 0x04003D98 RID: 15768
				None,
				// Token: 0x04003D99 RID: 15769
				Receive,
				// Token: 0x04003D9A RID: 15770
				Send
			}
		}

		// Token: 0x02000783 RID: 1923
		private static class Methods
		{
			// Token: 0x0400331D RID: 13085
			public const string CloseNetworkConnectionAsync = "CloseNetworkConnectionAsync";

			// Token: 0x0400331E RID: 13086
			public const string OnCancel = "OnCancel";

			// Token: 0x0400331F RID: 13087
			public const string OnReadCompleted = "OnReadCompleted";

			// Token: 0x04003320 RID: 13088
			public const string OnWriteCompleted = "OnWriteCompleted";

			// Token: 0x04003321 RID: 13089
			public const string ReadAsyncFast = "ReadAsyncFast";

			// Token: 0x04003322 RID: 13090
			public const string ReadAsyncCore = "ReadAsyncCore";

			// Token: 0x04003323 RID: 13091
			public const string WriteAsyncFast = "WriteAsyncFast";

			// Token: 0x04003324 RID: 13092
			public const string WriteAsyncCore = "WriteAsyncCore";

			// Token: 0x04003325 RID: 13093
			public const string MultipleWriteAsyncCore = "MultipleWriteAsyncCore";
		}
	}
}
