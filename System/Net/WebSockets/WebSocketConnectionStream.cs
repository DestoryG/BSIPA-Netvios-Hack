using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
	// Token: 0x02000233 RID: 563
	internal class WebSocketConnectionStream : BufferedReadStream, WebSocketBase.IWebSocketStream
	{
		// Token: 0x06001511 RID: 5393 RVA: 0x0006E3A4 File Offset: 0x0006C5A4
		public WebSocketConnectionStream(ConnectStream connectStream, string connectionGroupName)
			: base(new WebSocketConnectionStream.WebSocketConnection(connectStream.Connection), false)
		{
			this.m_ConnectStream = connectStream;
			this.m_ConnectionGroupName = connectionGroupName;
			this.m_CloseConnectStreamLock = new object();
			this.m_IsFastPathAllowed = this.m_ConnectStream.Connection.NetworkStream.GetType() == WebSocketConnectionStream.s_NetworkStreamType;
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Associate(Logging.WebSockets, this, this.m_ConnectStream.Connection);
			}
			this.ConsumeConnectStreamBuffer(connectStream);
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0006E425 File Offset: 0x0006C625
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x0006E428 File Offset: 0x0006C628
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0006E42B File Offset: 0x0006C62B
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x0006E42E File Offset: 0x0006C62E
		public bool SupportsMultipleWrite
		{
			get
			{
				return ((WebSocketConnectionStream.WebSocketConnection)base.BaseStream).SupportsMultipleWrite;
			}
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0006E440 File Offset: 0x0006C640
		public async Task CloseNetworkConnectionAsync(CancellationToken cancellationToken)
		{
			await Task.Yield();
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "CloseNetworkConnectionAsync", string.Empty);
			}
			CancellationTokenSource reasonableTimeoutCancellationTokenSource = null;
			CancellationTokenSource linkedCancellationTokenSource = null;
			CancellationToken cancellationToken2 = CancellationToken.None;
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			int bytesRead = 0;
			try
			{
				reasonableTimeoutCancellationTokenSource = new CancellationTokenSource(1000);
				linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(reasonableTimeoutCancellationTokenSource.Token, cancellationToken);
				cancellationToken2 = linkedCancellationTokenSource.Token;
				cancellationTokenRegistration = cancellationToken2.Register(WebSocketConnectionStream.s_OnCancel, this, false);
				WebSocketHelpers.ThrowIfConnectionAborted(this.m_ConnectStream.Connection, true);
				byte[] buffer = new byte[1];
				if (this.m_WebSocketConnection != null && this.m_InOpaqueMode)
				{
					bytesRead = await this.m_WebSocketConnection.ReadAsyncCore(buffer, 0, 1, cancellationToken2, true).SuppressContextFlow<int>();
				}
				else
				{
					bytesRead = await base.ReadAsync(buffer, 0, 1, cancellationToken2).SuppressContextFlow<int>();
				}
				if (bytesRead != 0)
				{
					if (WebSocketBase.LoggingEnabled)
					{
						Logging.Dump(Logging.WebSockets, this, "CloseNetworkConnectionAsync", buffer, 0, bytesRead);
					}
					throw new WebSocketException(WebSocketError.NotAWebSocket);
				}
				buffer = null;
			}
			catch (Exception ex)
			{
				if (!WebSocketConnectionStream.s_CanHandleException(ex))
				{
					throw;
				}
				cancellationToken.ThrowIfCancellationRequested();
			}
			finally
			{
				cancellationTokenRegistration.Dispose();
				if (linkedCancellationTokenSource != null)
				{
					linkedCancellationTokenSource.Dispose();
				}
				if (reasonableTimeoutCancellationTokenSource != null)
				{
					reasonableTimeoutCancellationTokenSource.Dispose();
				}
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "CloseNetworkConnectionAsync", bytesRead);
				}
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0006E48C File Offset: 0x0006C68C
		public override void Close()
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "Close", string.Empty);
			}
			try
			{
				object closeConnectStreamLock = this.m_CloseConnectStreamLock;
				lock (closeConnectStreamLock)
				{
					this.m_ConnectStream.Connection.ServicePoint.CloseConnectionGroup(this.m_ConnectionGroupName);
				}
				base.Close();
			}
			finally
			{
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "Close", string.Empty);
				}
			}
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0006E530 File Offset: 0x0006C730
		public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "ReadAsync", WebSocketHelpers.GetTraceMsgForParameters(offset, count, cancellationToken));
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			int bytesRead = 0;
			try
			{
				if (cancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration = cancellationToken.Register(WebSocketConnectionStream.s_OnCancel, this, false);
				}
				WebSocketHelpers.ThrowIfConnectionAborted(this.m_ConnectStream.Connection, true);
				int num = await base.ReadAsync(buffer, offset, count, cancellationToken).SuppressContextFlow<int>();
				bytesRead = num;
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Dump(Logging.WebSockets, this, "ReadAsync", buffer, offset, bytesRead);
				}
			}
			catch (Exception ex)
			{
				if (WebSocketConnectionStream.s_CanHandleException(ex))
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
					Logging.Exit(Logging.WebSockets, this, "ReadAsync", bytesRead);
				}
			}
			return bytesRead;
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0006E594 File Offset: 0x0006C794
		public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "WriteAsync", WebSocketHelpers.GetTraceMsgForParameters(offset, count, cancellationToken));
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			try
			{
				if (cancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration = cancellationToken.Register(WebSocketConnectionStream.s_OnCancel, this, false);
				}
				WebSocketHelpers.ThrowIfConnectionAborted(this.m_ConnectStream.Connection, false);
				await base.WriteAsync(buffer, offset, count, cancellationToken).SuppressContextFlow();
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Dump(Logging.WebSockets, this, "WriteAsync", buffer, offset, count);
				}
			}
			catch (Exception ex)
			{
				if (WebSocketConnectionStream.s_CanHandleException(ex))
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
					Logging.Exit(Logging.WebSockets, this, "WriteAsync", string.Empty);
				}
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0006E5F8 File Offset: 0x0006C7F8
		public void SwitchToOpaqueMode(WebSocketBase webSocket)
		{
			if (this.m_InOpaqueMode)
			{
				throw new InvalidOperationException();
			}
			this.m_WebSocketConnection = base.BaseStream as WebSocketConnectionStream.WebSocketConnection;
			if (this.m_WebSocketConnection != null && this.m_IsFastPathAllowed)
			{
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Associate(Logging.WebSockets, this, this.m_WebSocketConnection);
				}
				this.m_WebSocketConnection.SwitchToOpaqueMode(webSocket);
				this.m_InOpaqueMode = true;
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0006E660 File Offset: 0x0006C860
		public async Task MultipleWriteAsync(IList<ArraySegment<byte>> sendBuffers, CancellationToken cancellationToken)
		{
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "MultipleWriteAsync", string.Empty);
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			try
			{
				if (cancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration = cancellationToken.Register(WebSocketConnectionStream.s_OnCancel, this, false);
				}
				WebSocketHelpers.ThrowIfConnectionAborted(this.m_ConnectStream.Connection, false);
				await ((WebSocketBase.IWebSocketStream)base.BaseStream).MultipleWriteAsync(sendBuffers, cancellationToken).SuppressContextFlow();
				if (WebSocketBase.LoggingEnabled)
				{
					foreach (ArraySegment<byte> arraySegment in sendBuffers)
					{
						Logging.Dump(Logging.WebSockets, this, "MultipleWriteAsync", arraySegment.Array, arraySegment.Offset, arraySegment.Count);
					}
				}
			}
			catch (Exception ex)
			{
				if (WebSocketConnectionStream.s_CanHandleException(ex))
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
					Logging.Exit(Logging.WebSockets, this, "MultipleWriteAsync", string.Empty);
				}
			}
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0006E6B3 File Offset: 0x0006C8B3
		private static bool CanHandleException(Exception error)
		{
			return error is SocketException || error is ObjectDisposedException || error is WebException || error is IOException;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0006E6D8 File Offset: 0x0006C8D8
		private static void OnCancel(object state)
		{
			WebSocketConnectionStream webSocketConnectionStream = state as WebSocketConnectionStream;
			if (WebSocketBase.LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, state, "OnCancel", string.Empty);
			}
			try
			{
				object closeConnectStreamLock = webSocketConnectionStream.m_CloseConnectStreamLock;
				lock (closeConnectStreamLock)
				{
					webSocketConnectionStream.m_ConnectStream.Connection.NetworkStream.InternalAbortSocket();
					((ICloseEx)webSocketConnectionStream.m_ConnectStream).CloseEx(CloseExState.Abort);
				}
				webSocketConnectionStream.CancelWebSocketConnection();
			}
			catch
			{
			}
			finally
			{
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, state, "OnCancel", string.Empty);
				}
			}
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0006E798 File Offset: 0x0006C998
		private void CancelWebSocketConnection()
		{
			if (this.m_InOpaqueMode)
			{
				WebSocketConnectionStream.WebSocketConnection webSocketConnection = (WebSocketConnectionStream.WebSocketConnection)base.BaseStream;
				WebSocketConnectionStream.s_OnCancelWebSocketConnection(webSocketConnection);
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0006E7C4 File Offset: 0x0006C9C4
		public void Abort()
		{
			WebSocketConnectionStream.OnCancel(this);
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0006E7CC File Offset: 0x0006C9CC
		private void ConsumeConnectStreamBuffer(ConnectStream connectStream)
		{
			if (connectStream.Eof)
			{
				return;
			}
			byte[] array = new byte[1024];
			int num = 0;
			int num2 = array.Length;
			int num3;
			while ((num3 = connectStream.FillFromBufferedData(array, ref num, ref num2)) > 0)
			{
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Dump(Logging.WebSockets, this, "ConsumeConnectStreamBuffer", array, 0, num3);
				}
				base.Append(array, 0, num3);
				num = 0;
				num2 = array.Length;
			}
		}

		// Token: 0x0400168B RID: 5771
		private static readonly Func<Exception, bool> s_CanHandleException = new Func<Exception, bool>(WebSocketConnectionStream.CanHandleException);

		// Token: 0x0400168C RID: 5772
		private static readonly Action<object> s_OnCancel = new Action<object>(WebSocketConnectionStream.OnCancel);

		// Token: 0x0400168D RID: 5773
		private static readonly Action<object> s_OnCancelWebSocketConnection = new Action<object>(WebSocketConnectionStream.WebSocketConnection.OnCancel);

		// Token: 0x0400168E RID: 5774
		private static readonly Type s_NetworkStreamType = typeof(NetworkStream);

		// Token: 0x0400168F RID: 5775
		private readonly ConnectStream m_ConnectStream;

		// Token: 0x04001690 RID: 5776
		private readonly string m_ConnectionGroupName;

		// Token: 0x04001691 RID: 5777
		private readonly bool m_IsFastPathAllowed;

		// Token: 0x04001692 RID: 5778
		private readonly object m_CloseConnectStreamLock;

		// Token: 0x04001693 RID: 5779
		private bool m_InOpaqueMode;

		// Token: 0x04001694 RID: 5780
		private WebSocketConnectionStream.WebSocketConnection m_WebSocketConnection;

		// Token: 0x0200077A RID: 1914
		private static class Methods
		{
			// Token: 0x040032BE RID: 12990
			public const string Close = "Close";

			// Token: 0x040032BF RID: 12991
			public const string CloseNetworkConnectionAsync = "CloseNetworkConnectionAsync";

			// Token: 0x040032C0 RID: 12992
			public const string OnCancel = "OnCancel";

			// Token: 0x040032C1 RID: 12993
			public const string ReadAsync = "ReadAsync";

			// Token: 0x040032C2 RID: 12994
			public const string WriteAsync = "WriteAsync";

			// Token: 0x040032C3 RID: 12995
			public const string MultipleWriteAsync = "MultipleWriteAsync";
		}

		// Token: 0x0200077B RID: 1915
		private class WebSocketConnection : DelegatedStream, WebSocketBase.IWebSocketStream
		{
			// Token: 0x06004283 RID: 17027 RVA: 0x001154F6 File Offset: 0x001136F6
			internal WebSocketConnection(Connection connection)
				: base(connection)
			{
				this.m_InnerStream = connection;
				this.m_InOpaqueMode = false;
				this.m_SupportsMultipleWrites = connection.NetworkStream.GetType().Assembly == WebSocketConnectionStream.s_NetworkStreamType.Assembly;
			}

			// Token: 0x17000F2C RID: 3884
			// (get) Token: 0x06004284 RID: 17028 RVA: 0x00115532 File Offset: 0x00113732
			internal Socket InnerSocket
			{
				get
				{
					return this.GetInnerSocket(false);
				}
			}

			// Token: 0x17000F2D RID: 3885
			// (get) Token: 0x06004285 RID: 17029 RVA: 0x0011553B File Offset: 0x0011373B
			public override bool CanSeek
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000F2E RID: 3886
			// (get) Token: 0x06004286 RID: 17030 RVA: 0x0011553E File Offset: 0x0011373E
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F2F RID: 3887
			// (get) Token: 0x06004287 RID: 17031 RVA: 0x00115541 File Offset: 0x00113741
			public override bool CanWrite
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F30 RID: 3888
			// (get) Token: 0x06004288 RID: 17032 RVA: 0x00115544 File Offset: 0x00113744
			public bool SupportsMultipleWrite
			{
				get
				{
					return this.m_SupportsMultipleWrites;
				}
			}

			// Token: 0x06004289 RID: 17033 RVA: 0x0011554C File Offset: 0x0011374C
			public Task CloseNetworkConnectionAsync(CancellationToken cancellationToken)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600428A RID: 17034 RVA: 0x00115554 File Offset: 0x00113754
			public override void Close()
			{
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Enter(Logging.WebSockets, this, "Close", string.Empty);
				}
				try
				{
					base.Close();
					if (Interlocked.Increment(ref this.m_CleanedUp) == 1)
					{
						if (this.m_WriteEventArgs != null)
						{
							this.m_WriteEventArgs.Completed -= WebSocketConnectionStream.WebSocketConnection.s_OnWriteCompleted;
							this.m_WriteEventArgs.Dispose();
						}
						if (this.m_ReadEventArgs != null)
						{
							this.m_ReadEventArgs.Completed -= WebSocketConnectionStream.WebSocketConnection.s_OnReadCompleted;
							this.m_ReadEventArgs.Dispose();
						}
					}
				}
				finally
				{
					if (WebSocketBase.LoggingEnabled)
					{
						Logging.Exit(Logging.WebSockets, this, "Close", string.Empty);
					}
				}
			}

			// Token: 0x0600428B RID: 17035 RVA: 0x00115608 File Offset: 0x00113808
			internal Socket GetInnerSocket(bool skipStateCheck)
			{
				if (!skipStateCheck)
				{
					this.m_WebSocket.ThrowIfClosedOrAborted();
				}
				Socket internalSocket;
				try
				{
					internalSocket = this.m_InnerStream.NetworkStream.InternalSocket;
				}
				catch (ObjectDisposedException)
				{
					this.m_WebSocket.ThrowIfClosedOrAborted();
					throw;
				}
				return internalSocket;
			}

			// Token: 0x0600428C RID: 17036 RVA: 0x00115658 File Offset: 0x00113858
			private static IAsyncResult BeginMultipleWrite(IList<ArraySegment<byte>> sendBuffers, AsyncCallback callback, object asyncState)
			{
				WebSocketConnectionStream.WebSocketConnection webSocketConnection = asyncState as WebSocketConnectionStream.WebSocketConnection;
				BufferOffsetSize[] array = new BufferOffsetSize[sendBuffers.Count];
				for (int i = 0; i < sendBuffers.Count; i++)
				{
					ArraySegment<byte> arraySegment = sendBuffers[i];
					array[i] = new BufferOffsetSize(arraySegment.Array, arraySegment.Offset, arraySegment.Count, false);
				}
				WebSocketHelpers.ThrowIfConnectionAborted(webSocketConnection.m_InnerStream, false);
				return webSocketConnection.m_InnerStream.NetworkStream.BeginMultipleWrite(array, callback, asyncState);
			}

			// Token: 0x0600428D RID: 17037 RVA: 0x001156D0 File Offset: 0x001138D0
			private static void EndMultipleWrite(IAsyncResult asyncResult)
			{
				WebSocketConnectionStream.WebSocketConnection webSocketConnection = asyncResult.AsyncState as WebSocketConnectionStream.WebSocketConnection;
				WebSocketHelpers.ThrowIfConnectionAborted(webSocketConnection.m_InnerStream, false);
				webSocketConnection.m_InnerStream.NetworkStream.EndMultipleWrite(asyncResult);
			}

			// Token: 0x0600428E RID: 17038 RVA: 0x00115708 File Offset: 0x00113908
			public Task MultipleWriteAsync(IList<ArraySegment<byte>> sendBuffers, CancellationToken cancellationToken)
			{
				if (!this.m_InOpaqueMode)
				{
					return Task.Factory.FromAsync<IList<ArraySegment<byte>>>(WebSocketConnectionStream.WebSocketConnection.s_BeginMultipleWrite, WebSocketConnectionStream.WebSocketConnection.s_EndMultipleWrite, sendBuffers, this);
				}
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Enter(Logging.WebSockets, this, "MultipleWriteAsync", string.Empty);
				}
				bool flag = false;
				Task task;
				try
				{
					cancellationToken.ThrowIfCancellationRequested();
					WebSocketHelpers.ThrowIfConnectionAborted(this.m_InnerStream, false);
					this.m_WriteTaskCompletionSource = new TaskCompletionSource<object>();
					this.m_WriteEventArgs.SetBuffer(null, 0, 0);
					this.m_WriteEventArgs.BufferList = sendBuffers;
					flag = this.InnerSocket.SendAsync(this.m_WriteEventArgs);
					if (!flag)
					{
						if (this.m_WriteEventArgs.SocketError != SocketError.Success)
						{
							throw new SocketException(this.m_WriteEventArgs.SocketError);
						}
						task = Task.CompletedTask;
					}
					else
					{
						task = this.m_WriteTaskCompletionSource.Task;
					}
				}
				finally
				{
					if (WebSocketBase.LoggingEnabled)
					{
						Logging.Exit(Logging.WebSockets, this, "MultipleWriteAsync", flag);
					}
				}
				return task;
			}

			// Token: 0x0600428F RID: 17039 RVA: 0x00115804 File Offset: 0x00113A04
			public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
			{
				WebSocketHelpers.ValidateBuffer(buffer, offset, count);
				if (!this.m_InOpaqueMode)
				{
					return base.WriteAsync(buffer, offset, count, cancellationToken);
				}
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Enter(Logging.WebSockets, this, "WriteAsync", WebSocketHelpers.GetTraceMsgForParameters(offset, count, cancellationToken));
				}
				bool flag = false;
				Task task;
				try
				{
					cancellationToken.ThrowIfCancellationRequested();
					WebSocketHelpers.ThrowIfConnectionAborted(this.m_InnerStream, false);
					this.m_WriteTaskCompletionSource = new TaskCompletionSource<object>();
					this.m_WriteEventArgs.BufferList = null;
					this.m_WriteEventArgs.SetBuffer(buffer, offset, count);
					flag = this.InnerSocket.SendAsync(this.m_WriteEventArgs);
					if (!flag)
					{
						if (this.m_WriteEventArgs.SocketError != SocketError.Success)
						{
							throw new SocketException(this.m_WriteEventArgs.SocketError);
						}
						task = Task.CompletedTask;
					}
					else
					{
						task = this.m_WriteTaskCompletionSource.Task;
					}
				}
				finally
				{
					if (WebSocketBase.LoggingEnabled)
					{
						Logging.Exit(Logging.WebSockets, this, "WriteAsync", flag);
					}
				}
				return task;
			}

			// Token: 0x06004290 RID: 17040 RVA: 0x00115900 File Offset: 0x00113B00
			public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
			{
				WebSocketHelpers.ValidateBuffer(buffer, offset, count);
				if (!this.m_InOpaqueMode)
				{
					return base.ReadAsync(buffer, offset, count, cancellationToken);
				}
				return this.ReadAsyncCore(buffer, offset, count, cancellationToken, false);
			}

			// Token: 0x06004291 RID: 17041 RVA: 0x0011592C File Offset: 0x00113B2C
			internal Task<int> ReadAsyncCore(byte[] buffer, int offset, int count, CancellationToken cancellationToken, bool ignoreReadError)
			{
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Enter(Logging.WebSockets, this, "ReadAsyncCore", WebSocketHelpers.GetTraceMsgForParameters(offset, count, cancellationToken));
				}
				bool flag = false;
				this.m_IgnoreReadError = ignoreReadError;
				Task<int> task;
				try
				{
					cancellationToken.ThrowIfCancellationRequested();
					WebSocketHelpers.ThrowIfConnectionAborted(this.m_InnerStream, true);
					this.m_ReadTaskCompletionSource = new TaskCompletionSource<int>();
					this.m_ReadEventArgs.SetBuffer(buffer, offset, count);
					Socket socket;
					if (ignoreReadError)
					{
						socket = this.GetInnerSocket(true);
					}
					else
					{
						socket = this.InnerSocket;
					}
					flag = socket.ReceiveAsync(this.m_ReadEventArgs);
					if (!flag)
					{
						if (this.m_ReadEventArgs.SocketError != SocketError.Success)
						{
							if (!this.m_IgnoreReadError)
							{
								throw new SocketException(this.m_ReadEventArgs.SocketError);
							}
							task = Task.FromResult<int>(0);
						}
						else
						{
							task = Task.FromResult<int>(this.m_ReadEventArgs.BytesTransferred);
						}
					}
					else
					{
						task = this.m_ReadTaskCompletionSource.Task;
					}
				}
				finally
				{
					if (WebSocketBase.LoggingEnabled)
					{
						Logging.Exit(Logging.WebSockets, this, "ReadAsyncCore", flag);
					}
				}
				return task;
			}

			// Token: 0x06004292 RID: 17042 RVA: 0x00115A34 File Offset: 0x00113C34
			public override Task FlushAsync(CancellationToken cancellationToken)
			{
				if (!this.m_InOpaqueMode)
				{
					return base.FlushAsync(cancellationToken);
				}
				cancellationToken.ThrowIfCancellationRequested();
				return Task.CompletedTask;
			}

			// Token: 0x06004293 RID: 17043 RVA: 0x00115A52 File Offset: 0x00113C52
			public void Abort()
			{
			}

			// Token: 0x06004294 RID: 17044 RVA: 0x00115A54 File Offset: 0x00113C54
			internal static void OnCancel(object state)
			{
				WebSocketConnectionStream.WebSocketConnection webSocketConnection = state as WebSocketConnectionStream.WebSocketConnection;
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Enter(Logging.WebSockets, webSocketConnection, "OnCancel", string.Empty);
				}
				try
				{
					TaskCompletionSource<int> readTaskCompletionSource = webSocketConnection.m_ReadTaskCompletionSource;
					if (readTaskCompletionSource != null)
					{
						readTaskCompletionSource.TrySetCanceled();
					}
					TaskCompletionSource<object> writeTaskCompletionSource = webSocketConnection.m_WriteTaskCompletionSource;
					if (writeTaskCompletionSource != null)
					{
						writeTaskCompletionSource.TrySetCanceled();
					}
				}
				finally
				{
					if (WebSocketBase.LoggingEnabled)
					{
						Logging.Exit(Logging.WebSockets, webSocketConnection, "OnCancel", string.Empty);
					}
				}
			}

			// Token: 0x06004295 RID: 17045 RVA: 0x00115AD8 File Offset: 0x00113CD8
			public void SwitchToOpaqueMode(WebSocketBase webSocket)
			{
				this.m_WebSocket = webSocket;
				this.m_InOpaqueMode = true;
				this.m_ReadEventArgs = new SocketAsyncEventArgs();
				this.m_ReadEventArgs.UserToken = this;
				this.m_ReadEventArgs.Completed += WebSocketConnectionStream.WebSocketConnection.s_OnReadCompleted;
				this.m_WriteEventArgs = new SocketAsyncEventArgs();
				this.m_WriteEventArgs.UserToken = this;
				this.m_WriteEventArgs.Completed += WebSocketConnectionStream.WebSocketConnection.s_OnWriteCompleted;
			}

			// Token: 0x06004296 RID: 17046 RVA: 0x00115B41 File Offset: 0x00113D41
			private static string GetIOCompletionTraceMsg(SocketAsyncEventArgs eventArgs)
			{
				return string.Format(CultureInfo.InvariantCulture, "LastOperation: {0}, SocketError: {1}", new object[] { eventArgs.LastOperation, eventArgs.SocketError });
			}

			// Token: 0x06004297 RID: 17047 RVA: 0x00115B74 File Offset: 0x00113D74
			private static void OnWriteCompleted(object sender, SocketAsyncEventArgs eventArgs)
			{
				WebSocketConnectionStream.WebSocketConnection webSocketConnection = eventArgs.UserToken as WebSocketConnectionStream.WebSocketConnection;
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Enter(Logging.WebSockets, webSocketConnection, "OnWriteCompleted", WebSocketConnectionStream.WebSocketConnection.GetIOCompletionTraceMsg(eventArgs));
				}
				if (eventArgs.SocketError != SocketError.Success)
				{
					webSocketConnection.m_WriteTaskCompletionSource.TrySetException(new SocketException(eventArgs.SocketError));
				}
				else
				{
					webSocketConnection.m_WriteTaskCompletionSource.TrySetResult(null);
				}
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, webSocketConnection, "OnWriteCompleted", string.Empty);
				}
			}

			// Token: 0x06004298 RID: 17048 RVA: 0x00115BF4 File Offset: 0x00113DF4
			private static void OnReadCompleted(object sender, SocketAsyncEventArgs eventArgs)
			{
				WebSocketConnectionStream.WebSocketConnection webSocketConnection = eventArgs.UserToken as WebSocketConnectionStream.WebSocketConnection;
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Enter(Logging.WebSockets, webSocketConnection, "OnReadCompleted", WebSocketConnectionStream.WebSocketConnection.GetIOCompletionTraceMsg(eventArgs));
				}
				if (eventArgs.SocketError != SocketError.Success)
				{
					if (!webSocketConnection.m_IgnoreReadError)
					{
						webSocketConnection.m_ReadTaskCompletionSource.TrySetException(new SocketException(eventArgs.SocketError));
					}
					else
					{
						webSocketConnection.m_ReadTaskCompletionSource.TrySetResult(0);
					}
				}
				else
				{
					webSocketConnection.m_ReadTaskCompletionSource.TrySetResult(eventArgs.BytesTransferred);
				}
				if (WebSocketBase.LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, webSocketConnection, "OnReadCompleted", string.Empty);
				}
			}

			// Token: 0x040032C4 RID: 12996
			private static readonly EventHandler<SocketAsyncEventArgs> s_OnReadCompleted = new EventHandler<SocketAsyncEventArgs>(WebSocketConnectionStream.WebSocketConnection.OnReadCompleted);

			// Token: 0x040032C5 RID: 12997
			private static readonly EventHandler<SocketAsyncEventArgs> s_OnWriteCompleted = new EventHandler<SocketAsyncEventArgs>(WebSocketConnectionStream.WebSocketConnection.OnWriteCompleted);

			// Token: 0x040032C6 RID: 12998
			private static readonly Func<IList<ArraySegment<byte>>, AsyncCallback, object, IAsyncResult> s_BeginMultipleWrite = new Func<IList<ArraySegment<byte>>, AsyncCallback, object, IAsyncResult>(WebSocketConnectionStream.WebSocketConnection.BeginMultipleWrite);

			// Token: 0x040032C7 RID: 12999
			private static readonly Action<IAsyncResult> s_EndMultipleWrite = new Action<IAsyncResult>(WebSocketConnectionStream.WebSocketConnection.EndMultipleWrite);

			// Token: 0x040032C8 RID: 13000
			private readonly Connection m_InnerStream;

			// Token: 0x040032C9 RID: 13001
			private readonly bool m_SupportsMultipleWrites;

			// Token: 0x040032CA RID: 13002
			private bool m_InOpaqueMode;

			// Token: 0x040032CB RID: 13003
			private WebSocketBase m_WebSocket;

			// Token: 0x040032CC RID: 13004
			private SocketAsyncEventArgs m_WriteEventArgs;

			// Token: 0x040032CD RID: 13005
			private SocketAsyncEventArgs m_ReadEventArgs;

			// Token: 0x040032CE RID: 13006
			private TaskCompletionSource<object> m_WriteTaskCompletionSource;

			// Token: 0x040032CF RID: 13007
			private TaskCompletionSource<int> m_ReadTaskCompletionSource;

			// Token: 0x040032D0 RID: 13008
			private int m_CleanedUp;

			// Token: 0x040032D1 RID: 13009
			private bool m_IgnoreReadError;

			// Token: 0x0200091F RID: 2335
			private static class Methods
			{
				// Token: 0x04003D90 RID: 15760
				public const string Close = "Close";

				// Token: 0x04003D91 RID: 15761
				public const string OnCancel = "OnCancel";

				// Token: 0x04003D92 RID: 15762
				public const string OnReadCompleted = "OnReadCompleted";

				// Token: 0x04003D93 RID: 15763
				public const string OnWriteCompleted = "OnWriteCompleted";

				// Token: 0x04003D94 RID: 15764
				public const string ReadAsyncCore = "ReadAsyncCore";

				// Token: 0x04003D95 RID: 15765
				public const string WriteAsync = "WriteAsync";

				// Token: 0x04003D96 RID: 15766
				public const string MultipleWriteAsync = "MultipleWriteAsync";
			}
		}
	}
}
