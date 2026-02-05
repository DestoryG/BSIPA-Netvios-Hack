using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
	// Token: 0x02000230 RID: 560
	internal abstract class WebSocketBase : WebSocket, IDisposable
	{
		// Token: 0x060014BF RID: 5311 RVA: 0x0006CA1C File Offset: 0x0006AC1C
		protected WebSocketBase(Stream innerStream, string subProtocol, TimeSpan keepAliveInterval, WebSocketBuffer internalBuffer)
		{
			WebSocketHelpers.ValidateInnerStream(innerStream);
			WebSocketHelpers.ValidateOptions(subProtocol, internalBuffer.ReceiveBufferSize, internalBuffer.SendBufferSize, keepAliveInterval);
			WebSocketBase.s_LoggingEnabled = Logging.On && Logging.WebSockets.Switch.ShouldTrace(TraceEventType.Critical);
			string text = string.Empty;
			if (WebSocketBase.s_LoggingEnabled)
			{
				text = string.Format(CultureInfo.InvariantCulture, "ReceiveBufferSize: {0}, SendBufferSize: {1},  Protocols: {2}, KeepAliveInterval: {3}, innerStream: {4}, internalBuffer: {5}", new object[]
				{
					internalBuffer.ReceiveBufferSize,
					internalBuffer.SendBufferSize,
					subProtocol,
					keepAliveInterval,
					Logging.GetObjectLogHash(innerStream),
					Logging.GetObjectLogHash(internalBuffer)
				});
				Logging.Enter(Logging.WebSockets, this, "Initialize", text);
			}
			this.m_ThisLock = new object();
			try
			{
				this.m_InnerStream = innerStream;
				this.m_InternalBuffer = internalBuffer;
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Associate(Logging.WebSockets, this, this.m_InnerStream);
					Logging.Associate(Logging.WebSockets, this, this.m_InternalBuffer);
				}
				this.m_CloseOutstandingOperationHelper = new WebSocketBase.OutstandingOperationHelper();
				this.m_CloseOutputOutstandingOperationHelper = new WebSocketBase.OutstandingOperationHelper();
				this.m_ReceiveOutstandingOperationHelper = new WebSocketBase.OutstandingOperationHelper();
				this.m_SendOutstandingOperationHelper = new WebSocketBase.OutstandingOperationHelper();
				this.m_State = WebSocketState.Open;
				this.m_SubProtocol = subProtocol;
				this.m_SendFrameThrottle = new SemaphoreSlim(1, 1);
				this.m_CloseStatus = null;
				this.m_CloseStatusDescription = null;
				this.m_InnerStreamAsWebSocketStream = innerStream as WebSocketBase.IWebSocketStream;
				if (this.m_InnerStreamAsWebSocketStream != null)
				{
					this.m_InnerStreamAsWebSocketStream.SwitchToOpaqueMode(this);
				}
				this.m_KeepAliveTracker = WebSocketBase.KeepAliveTracker.Create(keepAliveInterval);
			}
			finally
			{
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "Initialize", text);
				}
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0006CBDC File Offset: 0x0006ADDC
		internal static bool LoggingEnabled
		{
			get
			{
				return WebSocketBase.s_LoggingEnabled;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x0006CBE5 File Offset: 0x0006ADE5
		public override WebSocketState State
		{
			get
			{
				return this.m_State;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0006CBEF File Offset: 0x0006ADEF
		public override string SubProtocol
		{
			get
			{
				return this.m_SubProtocol;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0006CBF7 File Offset: 0x0006ADF7
		public override WebSocketCloseStatus? CloseStatus
		{
			get
			{
				return this.m_CloseStatus;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0006CBFF File Offset: 0x0006ADFF
		public override string CloseStatusDescription
		{
			get
			{
				return this.m_CloseStatusDescription;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060014C5 RID: 5317 RVA: 0x0006CC07 File Offset: 0x0006AE07
		internal WebSocketBuffer InternalBuffer
		{
			get
			{
				return this.m_InternalBuffer;
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0006CC0F File Offset: 0x0006AE0F
		protected void StartKeepAliveTimer()
		{
			this.m_KeepAliveTracker.StartTimer(this);
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060014C7 RID: 5319
		internal abstract SafeHandle SessionHandle { get; }

		// Token: 0x060014C8 RID: 5320 RVA: 0x0006CC1D File Offset: 0x0006AE1D
		public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
		{
			WebSocketHelpers.ValidateArraySegment<byte>(buffer, "buffer");
			return this.ReceiveAsyncCore(buffer, cancellationToken);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0006CC34 File Offset: 0x0006AE34
		private async Task<WebSocketReceiveResult> ReceiveAsyncCore(ArraySegment<byte> buffer, CancellationToken cancellationToken)
		{
			if (WebSocketBase.s_LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "ReceiveAsync", string.Empty);
			}
			WebSocketReceiveResult webSocketReceiveResult2;
			try
			{
				this.ThrowIfPendingException();
				this.ThrowIfDisposed();
				WebSocket.ThrowOnInvalidState(this.State, new WebSocketState[]
				{
					WebSocketState.Open,
					WebSocketState.CloseSent
				});
				bool ownsCancellationTokenSource = false;
				CancellationToken linkedCancellationToken = CancellationToken.None;
				try
				{
					ownsCancellationTokenSource = this.m_ReceiveOutstandingOperationHelper.TryStartOperation(cancellationToken, out linkedCancellationToken);
					if (!ownsCancellationTokenSource)
					{
						object thisLock = this.m_ThisLock;
						lock (thisLock)
						{
							if (this.m_CloseAsyncStartedReceive)
							{
								throw new InvalidOperationException(SR.GetString("net_WebSockets_ReceiveAsyncDisallowedAfterCloseAsync", new object[] { "CloseAsync", "CloseOutputAsync" }));
							}
							throw new InvalidOperationException(SR.GetString("net_Websockets_AlreadyOneOutstandingOperation", new object[] { "ReceiveAsync" }));
						}
					}
					this.EnsureReceiveOperation();
					WebSocketReceiveResult webSocketReceiveResult = await this.m_ReceiveOperation.Process(new ArraySegment<byte>?(buffer), linkedCancellationToken).SuppressContextFlow<WebSocketReceiveResult>();
					webSocketReceiveResult2 = webSocketReceiveResult;
					if (WebSocketBase.s_LoggingEnabled && webSocketReceiveResult2.Count > 0)
					{
						Logging.Dump(Logging.WebSockets, this, "ReceiveAsync", buffer.Array, buffer.Offset, webSocketReceiveResult2.Count);
					}
				}
				catch (Exception ex)
				{
					bool isCancellationRequested = linkedCancellationToken.IsCancellationRequested;
					this.Abort();
					this.ThrowIfConvertibleException("ReceiveAsync", ex, cancellationToken, isCancellationRequested);
					throw;
				}
				finally
				{
					this.m_ReceiveOutstandingOperationHelper.CompleteOperation(ownsCancellationTokenSource);
				}
				linkedCancellationToken = default(CancellationToken);
			}
			finally
			{
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "ReceiveAsync", string.Empty);
				}
			}
			return webSocketReceiveResult2;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0006CC88 File Offset: 0x0006AE88
		public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			if (messageType != WebSocketMessageType.Binary && messageType != WebSocketMessageType.Text)
			{
				throw new ArgumentException(SR.GetString("net_WebSockets_Argument_InvalidMessageType", new object[]
				{
					messageType,
					"SendAsync",
					WebSocketMessageType.Binary,
					WebSocketMessageType.Text,
					"CloseOutputAsync"
				}), "messageType");
			}
			WebSocketHelpers.ValidateArraySegment<byte>(buffer, "buffer");
			return this.SendAsyncCore(buffer, messageType, endOfMessage, cancellationToken);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0006CCF8 File Offset: 0x0006AEF8
		private async Task SendAsyncCore(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			string inputParameter = string.Empty;
			if (WebSocketBase.s_LoggingEnabled)
			{
				inputParameter = string.Format(CultureInfo.InvariantCulture, "messageType: {0}, endOfMessage: {1}", new object[] { messageType, endOfMessage });
				Logging.Enter(Logging.WebSockets, this, "SendAsync", inputParameter);
			}
			try
			{
				this.ThrowIfPendingException();
				this.ThrowIfDisposed();
				WebSocket.ThrowOnInvalidState(this.State, new WebSocketState[]
				{
					WebSocketState.Open,
					WebSocketState.CloseReceived
				});
				bool ownsCancellationTokenSource = false;
				CancellationToken linkedCancellationToken = CancellationToken.None;
				try
				{
					while (!(ownsCancellationTokenSource = this.m_SendOutstandingOperationHelper.TryStartOperation(cancellationToken, out linkedCancellationToken)))
					{
						SafeHandle sessionHandle = this.SessionHandle;
						Task keepAliveTask;
						lock (sessionHandle)
						{
							keepAliveTask = this.m_KeepAliveTask;
							if (keepAliveTask == null)
							{
								this.m_SendOutstandingOperationHelper.CompleteOperation(ownsCancellationTokenSource);
								if (ownsCancellationTokenSource = this.m_SendOutstandingOperationHelper.TryStartOperation(cancellationToken, out linkedCancellationToken))
								{
									break;
								}
								throw new InvalidOperationException(SR.GetString("net_Websockets_AlreadyOneOutstandingOperation", new object[] { "SendAsync" }));
							}
						}
						await keepAliveTask.SuppressContextFlow();
						this.ThrowIfPendingException();
						this.m_SendOutstandingOperationHelper.CompleteOperation(ownsCancellationTokenSource);
					}
					if (WebSocketBase.s_LoggingEnabled && buffer.Count > 0)
					{
						Logging.Dump(Logging.WebSockets, this, "SendAsync", buffer.Array, buffer.Offset, buffer.Count);
					}
					int offset = buffer.Offset;
					this.EnsureSendOperation();
					this.m_SendOperation.BufferType = WebSocketBase.GetBufferType(messageType, endOfMessage);
					await this.m_SendOperation.Process(new ArraySegment<byte>?(buffer), linkedCancellationToken).SuppressContextFlow<WebSocketReceiveResult>();
				}
				catch (Exception ex)
				{
					bool isCancellationRequested = linkedCancellationToken.IsCancellationRequested;
					this.Abort();
					this.ThrowIfConvertibleException("SendAsync", ex, cancellationToken, isCancellationRequested);
					throw;
				}
				finally
				{
					this.m_SendOutstandingOperationHelper.CompleteOperation(ownsCancellationTokenSource);
				}
				linkedCancellationToken = default(CancellationToken);
			}
			finally
			{
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "SendAsync", inputParameter);
				}
			}
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0006CD5C File Offset: 0x0006AF5C
		private async Task SendFrameAsync(IList<ArraySegment<byte>> sendBuffers, CancellationToken cancellationToken)
		{
			bool sendFrameLockTaken = false;
			try
			{
				await this.m_SendFrameThrottle.WaitAsync(cancellationToken).SuppressContextFlow();
				sendFrameLockTaken = true;
				if (sendBuffers.Count > 1 && this.m_InnerStreamAsWebSocketStream != null && this.m_InnerStreamAsWebSocketStream.SupportsMultipleWrite)
				{
					await this.m_InnerStreamAsWebSocketStream.MultipleWriteAsync(sendBuffers, cancellationToken).SuppressContextFlow();
				}
				else
				{
					foreach (ArraySegment<byte> arraySegment in sendBuffers)
					{
						await this.m_InnerStream.WriteAsync(arraySegment.Array, arraySegment.Offset, arraySegment.Count, cancellationToken).SuppressContextFlow();
					}
					IEnumerator<ArraySegment<byte>> enumerator = null;
				}
			}
			catch (ObjectDisposedException ex)
			{
				throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex);
			}
			catch (NotSupportedException ex2)
			{
				throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex2);
			}
			finally
			{
				if (sendFrameLockTaken)
				{
					this.m_SendFrameThrottle.Release();
				}
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0006CDB0 File Offset: 0x0006AFB0
		public override void Abort()
		{
			if (WebSocketBase.s_LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, this, "Abort", string.Empty);
			}
			bool flag = false;
			bool flag2 = false;
			try
			{
				if (!WebSocket.IsStateTerminal(this.State))
				{
					this.TakeLocks(ref flag, ref flag2);
					if (!WebSocket.IsStateTerminal(this.State))
					{
						this.m_State = WebSocketState.Aborted;
						if (this.SessionHandle != null && !this.SessionHandle.IsClosed && !this.SessionHandle.IsInvalid)
						{
							WebSocketProtocolComponent.WebSocketAbortHandle(this.SessionHandle);
						}
						this.m_ReceiveOutstandingOperationHelper.CancelIO();
						this.m_SendOutstandingOperationHelper.CancelIO();
						this.m_CloseOutputOutstandingOperationHelper.CancelIO();
						this.m_CloseOutstandingOperationHelper.CancelIO();
						if (this.m_InnerStreamAsWebSocketStream != null)
						{
							this.m_InnerStreamAsWebSocketStream.Abort();
						}
						this.CleanUp();
					}
				}
			}
			finally
			{
				this.ReleaseLocks(ref flag, ref flag2);
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "Abort", string.Empty);
				}
			}
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0006CEC4 File Offset: 0x0006B0C4
		public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			WebSocketHelpers.ValidateCloseStatus(closeStatus, statusDescription);
			return this.CloseOutputAsyncCore(closeStatus, statusDescription, cancellationToken);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0006CED8 File Offset: 0x0006B0D8
		private async Task CloseOutputAsyncCore(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			string inputParameter = string.Empty;
			if (WebSocketBase.s_LoggingEnabled)
			{
				inputParameter = string.Format(CultureInfo.InvariantCulture, "closeStatus: {0}, statusDescription: {1}", new object[] { closeStatus, statusDescription });
				Logging.Enter(Logging.WebSockets, this, "CloseOutputAsync", inputParameter);
			}
			try
			{
				this.ThrowIfPendingException();
				if (!WebSocket.IsStateTerminal(this.State))
				{
					this.ThrowIfDisposed();
					bool thisLockTaken = false;
					bool sessionHandleLockTaken = false;
					bool needToCompleteSendOperation = false;
					bool ownsCloseOutputCancellationTokenSource = false;
					bool ownsSendCancellationTokenSource = false;
					CancellationToken linkedCancellationToken = CancellationToken.None;
					try
					{
						this.TakeLocks(ref thisLockTaken, ref sessionHandleLockTaken);
						this.ThrowIfPendingException();
						this.ThrowIfDisposed();
						if (WebSocket.IsStateTerminal(this.State))
						{
							return;
						}
						WebSocket.ThrowOnInvalidState(this.State, new WebSocketState[]
						{
							WebSocketState.Open,
							WebSocketState.CloseReceived
						});
						ownsCloseOutputCancellationTokenSource = this.m_CloseOutputOutstandingOperationHelper.TryStartOperation(cancellationToken, out linkedCancellationToken);
						if (!ownsCloseOutputCancellationTokenSource)
						{
							Task closeOutputTask = this.m_CloseOutputTask;
							if (closeOutputTask != null)
							{
								this.ReleaseLocks(ref thisLockTaken, ref sessionHandleLockTaken);
								await closeOutputTask.SuppressContextFlow();
								this.TakeLocks(ref thisLockTaken, ref sessionHandleLockTaken);
							}
						}
						else
						{
							needToCompleteSendOperation = true;
							while (!(ownsSendCancellationTokenSource = this.m_SendOutstandingOperationHelper.TryStartOperation(cancellationToken, out linkedCancellationToken)))
							{
								if (this.m_KeepAliveTask == null)
								{
									throw new InvalidOperationException(SR.GetString("net_Websockets_AlreadyOneOutstandingOperation", new object[] { "SendAsync" }));
								}
								Task keepAliveTask = this.m_KeepAliveTask;
								this.ReleaseLocks(ref thisLockTaken, ref sessionHandleLockTaken);
								await keepAliveTask.SuppressContextFlow();
								this.TakeLocks(ref thisLockTaken, ref sessionHandleLockTaken);
								this.ThrowIfPendingException();
								this.m_SendOutstandingOperationHelper.CompleteOperation(ownsSendCancellationTokenSource);
							}
							this.EnsureCloseOutputOperation();
							this.m_CloseOutputOperation.CloseStatus = closeStatus;
							this.m_CloseOutputOperation.CloseReason = statusDescription;
							this.m_CloseOutputTask = this.m_CloseOutputOperation.Process(null, linkedCancellationToken);
							this.ReleaseLocks(ref thisLockTaken, ref sessionHandleLockTaken);
							await this.m_CloseOutputTask.SuppressContextFlow();
							this.TakeLocks(ref thisLockTaken, ref sessionHandleLockTaken);
							if (this.OnCloseOutputCompleted())
							{
								bool flag = false;
								try
								{
									flag = await this.StartOnCloseCompleted(thisLockTaken, sessionHandleLockTaken, linkedCancellationToken).SuppressContextFlow<bool>();
								}
								catch (Exception)
								{
									this.ResetFlagsAndTakeLocks(ref thisLockTaken, ref sessionHandleLockTaken);
									throw;
								}
								if (flag)
								{
									this.ResetFlagsAndTakeLocks(ref thisLockTaken, ref sessionHandleLockTaken);
									this.FinishOnCloseCompleted();
								}
							}
						}
					}
					catch (Exception ex)
					{
						bool isCancellationRequested = linkedCancellationToken.IsCancellationRequested;
						this.Abort();
						this.ThrowIfConvertibleException("CloseOutputAsync", ex, cancellationToken, isCancellationRequested);
						throw;
					}
					finally
					{
						this.m_CloseOutputOutstandingOperationHelper.CompleteOperation(ownsCloseOutputCancellationTokenSource);
						if (needToCompleteSendOperation)
						{
							this.m_SendOutstandingOperationHelper.CompleteOperation(ownsSendCancellationTokenSource);
						}
						this.m_CloseOutputTask = null;
						this.ReleaseLocks(ref thisLockTaken, ref sessionHandleLockTaken);
					}
					linkedCancellationToken = default(CancellationToken);
				}
			}
			finally
			{
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "CloseOutputAsync", inputParameter);
				}
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0006CF34 File Offset: 0x0006B134
		private bool OnCloseOutputCompleted()
		{
			if (WebSocket.IsStateTerminal(this.State))
			{
				return false;
			}
			WebSocketState state = this.State;
			if (state != WebSocketState.Open)
			{
				return state == WebSocketState.CloseReceived;
			}
			this.m_State = WebSocketState.CloseSent;
			return false;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0006CF70 File Offset: 0x0006B170
		private async Task<bool> StartOnCloseCompleted(bool thisLockTakenSnapshot, bool sessionHandleLockTakenSnapshot, CancellationToken cancellationToken)
		{
			bool flag;
			if (WebSocket.IsStateTerminal(this.m_State))
			{
				flag = false;
			}
			else
			{
				this.m_State = WebSocketState.Closed;
				if (this.m_InnerStreamAsWebSocketStream != null)
				{
					bool flag2 = thisLockTakenSnapshot;
					bool flag3 = sessionHandleLockTakenSnapshot;
					try
					{
						if (this.m_CloseNetworkConnectionTask == null)
						{
							this.m_CloseNetworkConnectionTask = this.m_InnerStreamAsWebSocketStream.CloseNetworkConnectionAsync(cancellationToken);
						}
						if (flag2 && flag3)
						{
							this.ReleaseLocks(ref flag2, ref flag3);
						}
						else if (flag2)
						{
							WebSocketBase.ReleaseLock(this.m_ThisLock, ref flag2);
						}
						await this.m_CloseNetworkConnectionTask.SuppressContextFlow();
					}
					catch (Exception ex)
					{
						if (!this.CanHandleExceptionDuringClose(ex))
						{
							this.ThrowIfConvertibleException("StartOnCloseCompleted", ex, cancellationToken, cancellationToken.IsCancellationRequested);
							throw;
						}
					}
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0006CFCB File Offset: 0x0006B1CB
		private void FinishOnCloseCompleted()
		{
			this.CleanUp();
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0006CFD3 File Offset: 0x0006B1D3
		public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			WebSocketHelpers.ValidateCloseStatus(closeStatus, statusDescription);
			return this.CloseAsyncCore(closeStatus, statusDescription, cancellationToken);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0006CFE8 File Offset: 0x0006B1E8
		private async Task CloseAsyncCore(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			string inputParameter = string.Empty;
			if (WebSocketBase.s_LoggingEnabled)
			{
				inputParameter = string.Format(CultureInfo.InvariantCulture, "closeStatus: {0}, statusDescription: {1}", new object[] { closeStatus, statusDescription });
				Logging.Enter(Logging.WebSockets, this, "CloseAsync", inputParameter);
			}
			try
			{
				this.ThrowIfPendingException();
				if (!WebSocket.IsStateTerminal(this.State))
				{
					this.ThrowIfDisposed();
					bool lockTaken = false;
					Monitor.Enter(this.m_ThisLock, ref lockTaken);
					bool ownsCloseCancellationTokenSource = false;
					CancellationToken linkedCancellationToken = CancellationToken.None;
					try
					{
						this.ThrowIfPendingException();
						if (WebSocket.IsStateTerminal(this.State))
						{
							return;
						}
						this.ThrowIfDisposed();
						WebSocket.ThrowOnInvalidState(this.State, new WebSocketState[]
						{
							WebSocketState.Open,
							WebSocketState.CloseReceived,
							WebSocketState.CloseSent
						});
						ownsCloseCancellationTokenSource = this.m_CloseOutstandingOperationHelper.TryStartOperation(cancellationToken, out linkedCancellationToken);
						Task task;
						if (ownsCloseCancellationTokenSource)
						{
							task = this.m_CloseOutputTask;
							if (task == null && this.State != WebSocketState.CloseSent)
							{
								if (this.m_CloseReceivedTaskCompletionSource == null)
								{
									this.m_CloseReceivedTaskCompletionSource = new TaskCompletionSource<object>();
								}
								WebSocketBase.ReleaseLock(this.m_ThisLock, ref lockTaken);
								task = this.CloseOutputAsync(closeStatus, statusDescription, linkedCancellationToken);
							}
						}
						else
						{
							task = this.m_CloseReceivedTaskCompletionSource.Task;
						}
						if (task != null)
						{
							WebSocketBase.ReleaseLock(this.m_ThisLock, ref lockTaken);
							try
							{
								await task.SuppressContextFlow();
							}
							catch (Exception ex)
							{
								Monitor.Enter(this.m_ThisLock, ref lockTaken);
								if (!this.CanHandleExceptionDuringClose(ex))
								{
									this.ThrowIfConvertibleException("CloseOutputAsync", ex, cancellationToken, linkedCancellationToken.IsCancellationRequested);
									throw;
								}
							}
							if (!lockTaken)
							{
								Monitor.Enter(this.m_ThisLock, ref lockTaken);
							}
						}
						if (this.OnCloseOutputCompleted())
						{
							bool flag = false;
							try
							{
								flag = await this.StartOnCloseCompleted(lockTaken, false, linkedCancellationToken).SuppressContextFlow<bool>();
							}
							catch (Exception)
							{
								this.ResetFlagAndTakeLock(this.m_ThisLock, ref lockTaken);
								throw;
							}
							if (flag)
							{
								this.ResetFlagAndTakeLock(this.m_ThisLock, ref lockTaken);
								this.FinishOnCloseCompleted();
							}
						}
						if (WebSocket.IsStateTerminal(this.State))
						{
							return;
						}
						linkedCancellationToken = CancellationToken.None;
						bool flag2 = this.m_ReceiveOutstandingOperationHelper.TryStartOperation(cancellationToken, out linkedCancellationToken);
						if (flag2)
						{
							this.m_CloseAsyncStartedReceive = true;
							ArraySegment<byte> closeMessageBuffer = new ArraySegment<byte>(new byte[256]);
							this.EnsureReceiveOperation();
							Task<WebSocketReceiveResult> task2 = this.m_ReceiveOperation.Process(new ArraySegment<byte>?(closeMessageBuffer), linkedCancellationToken);
							WebSocketBase.ReleaseLock(this.m_ThisLock, ref lockTaken);
							WebSocketReceiveResult receiveResult = null;
							try
							{
								receiveResult = await task2.SuppressContextFlow<WebSocketReceiveResult>();
							}
							catch (Exception ex2)
							{
								Monitor.Enter(this.m_ThisLock, ref lockTaken);
								if (!this.CanHandleExceptionDuringClose(ex2))
								{
									this.ThrowIfConvertibleException("CloseAsync", ex2, cancellationToken, linkedCancellationToken.IsCancellationRequested);
									throw;
								}
							}
							if (receiveResult != null)
							{
								if (WebSocketBase.s_LoggingEnabled && receiveResult.Count > 0)
								{
									Logging.Dump(Logging.WebSockets, this, "ReceiveAsync", closeMessageBuffer.Array, closeMessageBuffer.Offset, receiveResult.Count);
								}
								if (receiveResult.MessageType != WebSocketMessageType.Close)
								{
									throw new WebSocketException(WebSocketError.InvalidMessageType, SR.GetString("net_WebSockets_InvalidMessageType", new object[]
									{
										typeof(WebSocket).Name + ".CloseAsync",
										typeof(WebSocket).Name + ".CloseOutputAsync",
										receiveResult.MessageType
									}));
								}
							}
							closeMessageBuffer = default(ArraySegment<byte>);
							receiveResult = null;
						}
						else
						{
							this.m_ReceiveOutstandingOperationHelper.CompleteOperation(flag2);
							WebSocketBase.ReleaseLock(this.m_ThisLock, ref lockTaken);
							await this.m_CloseReceivedTaskCompletionSource.Task.SuppressContextFlow<object>();
						}
						if (!lockTaken)
						{
							Monitor.Enter(this.m_ThisLock, ref lockTaken);
						}
						if (!WebSocket.IsStateTerminal(this.State))
						{
							bool ownsSendCancellationSource = false;
							try
							{
								ownsSendCancellationSource = this.m_SendOutstandingOperationHelper.TryStartOperation(cancellationToken, out linkedCancellationToken);
								bool flag3 = false;
								try
								{
									flag3 = await this.StartOnCloseCompleted(lockTaken, false, linkedCancellationToken).SuppressContextFlow<bool>();
								}
								catch (Exception)
								{
									this.ResetFlagAndTakeLock(this.m_ThisLock, ref lockTaken);
									throw;
								}
								if (flag3)
								{
									this.ResetFlagAndTakeLock(this.m_ThisLock, ref lockTaken);
									this.FinishOnCloseCompleted();
								}
							}
							finally
							{
								this.m_SendOutstandingOperationHelper.CompleteOperation(ownsSendCancellationSource);
							}
						}
					}
					catch (Exception ex3)
					{
						bool isCancellationRequested = linkedCancellationToken.IsCancellationRequested;
						this.Abort();
						this.ThrowIfConvertibleException("CloseAsync", ex3, cancellationToken, isCancellationRequested);
						throw;
					}
					finally
					{
						this.m_CloseOutstandingOperationHelper.CompleteOperation(ownsCloseCancellationTokenSource);
						WebSocketBase.ReleaseLock(this.m_ThisLock, ref lockTaken);
					}
					linkedCancellationToken = default(CancellationToken);
				}
			}
			finally
			{
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, this, "CloseAsync", inputParameter);
				}
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0006D044 File Offset: 0x0006B244
		public override void Dispose()
		{
			if (this.m_IsDisposed)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			try
			{
				this.TakeLocks(ref flag, ref flag2);
				if (!this.m_IsDisposed)
				{
					if (!WebSocket.IsStateTerminal(this.State))
					{
						this.Abort();
					}
					else
					{
						this.CleanUp();
					}
					this.m_IsDisposed = true;
				}
			}
			finally
			{
				this.ReleaseLocks(ref flag, ref flag2);
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0006D0B8 File Offset: 0x0006B2B8
		private void ResetFlagAndTakeLock(object lockObject, ref bool thisLockTaken)
		{
			thisLockTaken = false;
			Monitor.Enter(lockObject, ref thisLockTaken);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0006D0C4 File Offset: 0x0006B2C4
		private void ResetFlagsAndTakeLocks(ref bool thisLockTaken, ref bool sessionHandleLockTaken)
		{
			thisLockTaken = false;
			sessionHandleLockTaken = false;
			this.TakeLocks(ref thisLockTaken, ref sessionHandleLockTaken);
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0006D0D4 File Offset: 0x0006B2D4
		private void TakeLocks(ref bool thisLockTaken, ref bool sessionHandleLockTaken)
		{
			Monitor.Enter(this.SessionHandle, ref sessionHandleLockTaken);
			Monitor.Enter(this.m_ThisLock, ref thisLockTaken);
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0006D0F0 File Offset: 0x0006B2F0
		private void ReleaseLocks(ref bool thisLockTaken, ref bool sessionHandleLockTaken)
		{
			if (thisLockTaken | sessionHandleLockTaken)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					if (thisLockTaken)
					{
						Monitor.Exit(this.m_ThisLock);
						thisLockTaken = false;
					}
					if (sessionHandleLockTaken)
					{
						Monitor.Exit(this.SessionHandle);
						sessionHandleLockTaken = false;
					}
				}
			}
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0006D140 File Offset: 0x0006B340
		private void EnsureReceiveOperation()
		{
			if (this.m_ReceiveOperation == null)
			{
				object thisLock = this.m_ThisLock;
				lock (thisLock)
				{
					if (this.m_ReceiveOperation == null)
					{
						this.m_ReceiveOperation = new WebSocketBase.WebSocketOperation.ReceiveOperation(this);
					}
				}
			}
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0006D19C File Offset: 0x0006B39C
		private void EnsureSendOperation()
		{
			if (this.m_SendOperation == null)
			{
				object thisLock = this.m_ThisLock;
				lock (thisLock)
				{
					if (this.m_SendOperation == null)
					{
						this.m_SendOperation = new WebSocketBase.WebSocketOperation.SendOperation(this);
					}
				}
			}
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0006D1F8 File Offset: 0x0006B3F8
		private void EnsureKeepAliveOperation()
		{
			if (this.m_KeepAliveOperation == null)
			{
				object thisLock = this.m_ThisLock;
				lock (thisLock)
				{
					if (this.m_KeepAliveOperation == null)
					{
						this.m_KeepAliveOperation = new WebSocketBase.WebSocketOperation.SendOperation(this)
						{
							BufferType = (WebSocketProtocolComponent.BufferType)2147483654U
						};
					}
				}
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0006D264 File Offset: 0x0006B464
		private void EnsureCloseOutputOperation()
		{
			if (this.m_CloseOutputOperation == null)
			{
				object thisLock = this.m_ThisLock;
				lock (thisLock)
				{
					if (this.m_CloseOutputOperation == null)
					{
						this.m_CloseOutputOperation = new WebSocketBase.WebSocketOperation.CloseOutputOperation(this);
					}
				}
			}
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0006D2C0 File Offset: 0x0006B4C0
		private static void ReleaseLock(object lockObject, ref bool lockTaken)
		{
			if (lockTaken)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					Monitor.Exit(lockObject);
					lockTaken = false;
				}
			}
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0006D2F4 File Offset: 0x0006B4F4
		private static WebSocketProtocolComponent.BufferType GetBufferType(WebSocketMessageType messageType, bool endOfMessage)
		{
			if (messageType == WebSocketMessageType.Text)
			{
				if (endOfMessage)
				{
					return (WebSocketProtocolComponent.BufferType)2147483648U;
				}
				return (WebSocketProtocolComponent.BufferType)2147483649U;
			}
			else
			{
				if (endOfMessage)
				{
					return (WebSocketProtocolComponent.BufferType)2147483650U;
				}
				return (WebSocketProtocolComponent.BufferType)2147483651U;
			}
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0006D318 File Offset: 0x0006B518
		private static WebSocketMessageType GetMessageType(WebSocketProtocolComponent.BufferType bufferType)
		{
			switch (bufferType)
			{
			case (WebSocketProtocolComponent.BufferType)2147483648U:
			case (WebSocketProtocolComponent.BufferType)2147483649U:
				return WebSocketMessageType.Text;
			case (WebSocketProtocolComponent.BufferType)2147483650U:
			case (WebSocketProtocolComponent.BufferType)2147483651U:
				return WebSocketMessageType.Binary;
			case (WebSocketProtocolComponent.BufferType)2147483652U:
				return WebSocketMessageType.Close;
			default:
				throw new WebSocketException(WebSocketError.NativeError, SR.GetString("net_WebSockets_InvalidBufferType", new object[]
				{
					bufferType,
					(WebSocketProtocolComponent.BufferType)2147483652U,
					(WebSocketProtocolComponent.BufferType)2147483651U,
					(WebSocketProtocolComponent.BufferType)2147483650U,
					(WebSocketProtocolComponent.BufferType)2147483649U,
					(WebSocketProtocolComponent.BufferType)2147483648U
				}));
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0006D3AD File Offset: 0x0006B5AD
		internal void ValidateNativeBuffers(WebSocketProtocolComponent.Action action, WebSocketProtocolComponent.BufferType bufferType, WebSocketProtocolComponent.Buffer[] dataBuffers, uint dataBufferCount)
		{
			this.m_InternalBuffer.ValidateNativeBuffers(action, bufferType, dataBuffers, dataBufferCount);
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0006D3C0 File Offset: 0x0006B5C0
		internal void ThrowIfClosedOrAborted()
		{
			if (this.State == WebSocketState.Closed || this.State == WebSocketState.Aborted)
			{
				throw new WebSocketException(WebSocketError.InvalidState, SR.GetString("net_WebSockets_InvalidState_ClosedOrAborted", new object[]
				{
					base.GetType().FullName,
					this.State
				}));
			}
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0006D413 File Offset: 0x0006B613
		private void ThrowIfAborted(bool aborted, Exception innerException)
		{
			if (aborted)
			{
				throw new WebSocketException(WebSocketError.InvalidState, SR.GetString("net_WebSockets_InvalidState_ClosedOrAborted", new object[]
				{
					base.GetType().FullName,
					WebSocketState.Aborted
				}), innerException);
			}
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0006D448 File Offset: 0x0006B648
		private bool CanHandleExceptionDuringClose(Exception error)
		{
			return this.State == WebSocketState.Closed && (error is OperationCanceledException || error is WebSocketException || error is SocketException || error is HttpListenerException || error is IOException);
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0006D480 File Offset: 0x0006B680
		private void ThrowIfConvertibleException(string methodName, Exception exception, CancellationToken cancellationToken, bool aborted)
		{
			if (WebSocketBase.s_LoggingEnabled && !string.IsNullOrEmpty(methodName))
			{
				Logging.Exception(Logging.WebSockets, this, methodName, exception);
			}
			OperationCanceledException ex = exception as OperationCanceledException;
			if (ex != null)
			{
				if (cancellationToken.IsCancellationRequested || !aborted)
				{
					return;
				}
				this.ThrowIfAborted(aborted, exception);
			}
			WebSocketException ex2 = exception as WebSocketException;
			if (ex2 != null)
			{
				cancellationToken.ThrowIfCancellationRequested();
				this.ThrowIfAborted(aborted, ex2);
				return;
			}
			SocketException ex3 = exception as SocketException;
			if (ex3 != null)
			{
				ex2 = new WebSocketException(ex3.NativeErrorCode, ex3);
			}
			HttpListenerException ex4 = exception as HttpListenerException;
			if (ex4 != null)
			{
				ex2 = new WebSocketException(ex4.ErrorCode, ex4);
			}
			IOException ex5 = exception as IOException;
			if (ex5 != null)
			{
				ex3 = exception.InnerException as SocketException;
				if (ex3 != null)
				{
					ex2 = new WebSocketException(ex3.NativeErrorCode, ex5);
				}
			}
			if (ex2 != null)
			{
				cancellationToken.ThrowIfCancellationRequested();
				this.ThrowIfAborted(aborted, ex2);
				throw ex2;
			}
			AggregateException ex6 = exception as AggregateException;
			if (ex6 != null)
			{
				ReadOnlyCollection<Exception> innerExceptions = ex6.Flatten().InnerExceptions;
				if (innerExceptions.Count == 0)
				{
					return;
				}
				foreach (Exception ex7 in innerExceptions)
				{
					this.ThrowIfConvertibleException(null, ex7, cancellationToken, aborted);
				}
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0006D5C0 File Offset: 0x0006B7C0
		private void CleanUp()
		{
			if (this.m_CleanedUp)
			{
				return;
			}
			this.m_CleanedUp = true;
			if (this.SessionHandle != null)
			{
				this.SessionHandle.Dispose();
			}
			if (this.m_InternalBuffer != null)
			{
				this.m_InternalBuffer.Dispose(this.State);
			}
			if (this.m_ReceiveOutstandingOperationHelper != null)
			{
				this.m_ReceiveOutstandingOperationHelper.Dispose();
			}
			if (this.m_SendOutstandingOperationHelper != null)
			{
				this.m_SendOutstandingOperationHelper.Dispose();
			}
			if (this.m_CloseOutputOutstandingOperationHelper != null)
			{
				this.m_CloseOutputOutstandingOperationHelper.Dispose();
			}
			if (this.m_CloseOutstandingOperationHelper != null)
			{
				this.m_CloseOutstandingOperationHelper.Dispose();
			}
			if (this.m_InnerStream != null)
			{
				try
				{
					this.m_InnerStream.Close();
				}
				catch (ObjectDisposedException)
				{
				}
				catch (IOException)
				{
				}
				catch (SocketException)
				{
				}
				catch (HttpListenerException)
				{
				}
			}
			this.m_KeepAliveTracker.Dispose();
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0006D6BC File Offset: 0x0006B8BC
		private void OnBackgroundTaskException(Exception exception)
		{
			if (Interlocked.CompareExchange<Exception>(ref this.m_PendingException, exception, null) == null)
			{
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Exception(Logging.WebSockets, this, "Fault", exception);
				}
				this.Abort();
			}
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0006D6F0 File Offset: 0x0006B8F0
		private void ThrowIfPendingException()
		{
			Exception ex = Interlocked.Exchange<Exception>(ref this.m_PendingException, null);
			if (ex != null)
			{
				throw new WebSocketException(WebSocketError.Faulted, ex);
			}
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0006D715 File Offset: 0x0006B915
		private void ThrowIfDisposed()
		{
			if (this.m_IsDisposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0006D734 File Offset: 0x0006B934
		private void UpdateReceiveState(int newReceiveState, int expectedReceiveState)
		{
			int num = Interlocked.Exchange(ref this.m_ReceiveState, newReceiveState);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0006D754 File Offset: 0x0006B954
		private bool StartOnCloseReceived(ref bool thisLockTaken)
		{
			this.ThrowIfDisposed();
			if (WebSocket.IsStateTerminal(this.State) || this.State == WebSocketState.CloseReceived)
			{
				return false;
			}
			Monitor.Enter(this.m_ThisLock, ref thisLockTaken);
			if (WebSocket.IsStateTerminal(this.State) || this.State == WebSocketState.CloseReceived)
			{
				return false;
			}
			if (this.State == WebSocketState.Open)
			{
				this.m_State = WebSocketState.CloseReceived;
				if (this.m_CloseReceivedTaskCompletionSource == null)
				{
					this.m_CloseReceivedTaskCompletionSource = new TaskCompletionSource<object>();
				}
				return false;
			}
			return true;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0006D7D0 File Offset: 0x0006B9D0
		private void FinishOnCloseReceived(WebSocketCloseStatus closeStatus, string closeStatusDescription)
		{
			if (this.m_CloseReceivedTaskCompletionSource != null)
			{
				this.m_CloseReceivedTaskCompletionSource.TrySetResult(null);
			}
			this.m_CloseStatus = new WebSocketCloseStatus?(closeStatus);
			this.m_CloseStatusDescription = closeStatusDescription;
			if (WebSocketBase.s_LoggingEnabled)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "closeStatus: {0}, closeStatusDescription: {1}, m_State: {2}", new object[] { closeStatus, closeStatusDescription, this.m_State });
				Logging.PrintInfo(Logging.WebSockets, this, "FinishOnCloseReceived", text);
			}
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0006D858 File Offset: 0x0006BA58
		private static async void OnKeepAlive(object sender)
		{
			WebSocketBase thisPtr = sender as WebSocketBase;
			bool lockTaken = false;
			if (WebSocketBase.s_LoggingEnabled)
			{
				Logging.Enter(Logging.WebSockets, thisPtr, "OnKeepAlive", string.Empty);
			}
			CancellationToken linkedCancellationToken = CancellationToken.None;
			try
			{
				Monitor.Enter(thisPtr.SessionHandle, ref lockTaken);
				if (!thisPtr.m_IsDisposed && thisPtr.m_State == WebSocketState.Open && thisPtr.m_CloseOutputTask == null)
				{
					if (thisPtr.m_KeepAliveTracker.ShouldSendKeepAlive())
					{
						bool ownsCancellationTokenSource = false;
						try
						{
							ownsCancellationTokenSource = thisPtr.m_SendOutstandingOperationHelper.TryStartOperation(CancellationToken.None, out linkedCancellationToken);
							if (ownsCancellationTokenSource)
							{
								thisPtr.EnsureKeepAliveOperation();
								thisPtr.m_KeepAliveTask = thisPtr.m_KeepAliveOperation.Process(null, linkedCancellationToken);
								WebSocketBase.ReleaseLock(thisPtr.SessionHandle, ref lockTaken);
								await thisPtr.m_KeepAliveTask.SuppressContextFlow();
							}
						}
						finally
						{
							if (!lockTaken)
							{
								Monitor.Enter(thisPtr.SessionHandle, ref lockTaken);
							}
							thisPtr.m_SendOutstandingOperationHelper.CompleteOperation(ownsCancellationTokenSource);
							thisPtr.m_KeepAliveTask = null;
						}
						thisPtr.m_KeepAliveTracker.ResetTimer();
					}
				}
			}
			catch (Exception ex)
			{
				try
				{
					thisPtr.ThrowIfConvertibleException("OnKeepAlive", ex, CancellationToken.None, linkedCancellationToken.IsCancellationRequested);
					throw;
				}
				catch (Exception ex2)
				{
					thisPtr.OnBackgroundTaskException(ex2);
				}
			}
			finally
			{
				WebSocketBase.ReleaseLock(thisPtr.SessionHandle, ref lockTaken);
				if (WebSocketBase.s_LoggingEnabled)
				{
					Logging.Exit(Logging.WebSockets, thisPtr, "OnKeepAlive", string.Empty);
				}
			}
		}

		// Token: 0x0400164C RID: 5708
		private static volatile bool s_LoggingEnabled;

		// Token: 0x0400164D RID: 5709
		private readonly WebSocketBase.OutstandingOperationHelper m_CloseOutstandingOperationHelper;

		// Token: 0x0400164E RID: 5710
		private readonly WebSocketBase.OutstandingOperationHelper m_CloseOutputOutstandingOperationHelper;

		// Token: 0x0400164F RID: 5711
		private readonly WebSocketBase.OutstandingOperationHelper m_ReceiveOutstandingOperationHelper;

		// Token: 0x04001650 RID: 5712
		private readonly WebSocketBase.OutstandingOperationHelper m_SendOutstandingOperationHelper;

		// Token: 0x04001651 RID: 5713
		private readonly Stream m_InnerStream;

		// Token: 0x04001652 RID: 5714
		private readonly WebSocketBase.IWebSocketStream m_InnerStreamAsWebSocketStream;

		// Token: 0x04001653 RID: 5715
		private readonly string m_SubProtocol;

		// Token: 0x04001654 RID: 5716
		private readonly SemaphoreSlim m_SendFrameThrottle;

		// Token: 0x04001655 RID: 5717
		private readonly object m_ThisLock;

		// Token: 0x04001656 RID: 5718
		private readonly WebSocketBuffer m_InternalBuffer;

		// Token: 0x04001657 RID: 5719
		private readonly WebSocketBase.KeepAliveTracker m_KeepAliveTracker;

		// Token: 0x04001658 RID: 5720
		private volatile bool m_CleanedUp;

		// Token: 0x04001659 RID: 5721
		private volatile TaskCompletionSource<object> m_CloseReceivedTaskCompletionSource;

		// Token: 0x0400165A RID: 5722
		private volatile Task m_CloseOutputTask;

		// Token: 0x0400165B RID: 5723
		private volatile bool m_IsDisposed;

		// Token: 0x0400165C RID: 5724
		private volatile Task m_CloseNetworkConnectionTask;

		// Token: 0x0400165D RID: 5725
		private volatile bool m_CloseAsyncStartedReceive;

		// Token: 0x0400165E RID: 5726
		private volatile WebSocketState m_State;

		// Token: 0x0400165F RID: 5727
		private volatile Task m_KeepAliveTask;

		// Token: 0x04001660 RID: 5728
		private volatile WebSocketBase.WebSocketOperation.ReceiveOperation m_ReceiveOperation;

		// Token: 0x04001661 RID: 5729
		private volatile WebSocketBase.WebSocketOperation.SendOperation m_SendOperation;

		// Token: 0x04001662 RID: 5730
		private volatile WebSocketBase.WebSocketOperation.SendOperation m_KeepAliveOperation;

		// Token: 0x04001663 RID: 5731
		private volatile WebSocketBase.WebSocketOperation.CloseOutputOperation m_CloseOutputOperation;

		// Token: 0x04001664 RID: 5732
		private WebSocketCloseStatus? m_CloseStatus;

		// Token: 0x04001665 RID: 5733
		private string m_CloseStatusDescription;

		// Token: 0x04001666 RID: 5734
		private int m_ReceiveState;

		// Token: 0x04001667 RID: 5735
		private Exception m_PendingException;

		// Token: 0x0200076C RID: 1900
		private abstract class WebSocketOperation
		{
			// Token: 0x17000F27 RID: 3879
			// (get) Token: 0x06004254 RID: 16980 RVA: 0x001133BE File Offset: 0x001115BE
			// (set) Token: 0x06004255 RID: 16981 RVA: 0x001133C6 File Offset: 0x001115C6
			protected bool AsyncOperationCompleted { get; set; }

			// Token: 0x06004256 RID: 16982 RVA: 0x001133CF File Offset: 0x001115CF
			internal WebSocketOperation(WebSocketBase webSocket)
			{
				this.m_WebSocket = webSocket;
				this.AsyncOperationCompleted = false;
			}

			// Token: 0x17000F28 RID: 3880
			// (get) Token: 0x06004257 RID: 16983 RVA: 0x001133E5 File Offset: 0x001115E5
			// (set) Token: 0x06004258 RID: 16984 RVA: 0x001133ED File Offset: 0x001115ED
			public WebSocketReceiveResult ReceiveResult { get; protected set; }

			// Token: 0x17000F29 RID: 3881
			// (get) Token: 0x06004259 RID: 16985
			protected abstract int BufferCount { get; }

			// Token: 0x17000F2A RID: 3882
			// (get) Token: 0x0600425A RID: 16986
			protected abstract WebSocketProtocolComponent.ActionQueue ActionQueue { get; }

			// Token: 0x0600425B RID: 16987
			protected abstract void Initialize(ArraySegment<byte>? buffer, CancellationToken cancellationToken);

			// Token: 0x0600425C RID: 16988
			protected abstract bool ShouldContinue(CancellationToken cancellationToken);

			// Token: 0x0600425D RID: 16989
			protected abstract bool ProcessAction_NoAction();

			// Token: 0x0600425E RID: 16990 RVA: 0x001133F6 File Offset: 0x001115F6
			protected virtual void ProcessAction_IndicateReceiveComplete(ArraySegment<byte>? buffer, WebSocketProtocolComponent.BufferType bufferType, WebSocketProtocolComponent.Action action, WebSocketProtocolComponent.Buffer[] dataBuffers, uint dataBufferCount, IntPtr actionContext)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600425F RID: 16991
			protected abstract void Cleanup();

			// Token: 0x06004260 RID: 16992 RVA: 0x00113400 File Offset: 0x00111600
			internal async Task<WebSocketReceiveResult> Process(ArraySegment<byte>? buffer, CancellationToken cancellationToken)
			{
				bool sessionHandleLockTaken = false;
				this.AsyncOperationCompleted = false;
				this.ReceiveResult = null;
				try
				{
					Monitor.Enter(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
					this.m_WebSocket.ThrowIfPendingException();
					this.Initialize(buffer, cancellationToken);
					while (this.ShouldContinue(cancellationToken))
					{
						bool completed = false;
						while (!completed)
						{
							WebSocketProtocolComponent.Buffer[] array = new WebSocketProtocolComponent.Buffer[this.BufferCount];
							uint bufferCount = (uint)this.BufferCount;
							this.m_WebSocket.ThrowIfDisposed();
							WebSocketProtocolComponent.Action action;
							WebSocketProtocolComponent.BufferType bufferType;
							IntPtr actionContext;
							WebSocketProtocolComponent.WebSocketGetAction(this.m_WebSocket, this.ActionQueue, array, ref bufferCount, out action, out bufferType, out actionContext);
							switch (action)
							{
							case WebSocketProtocolComponent.Action.NoAction:
								if (this.ProcessAction_NoAction())
								{
									bool thisLockTaken = false;
									try
									{
										if (this.m_WebSocket.StartOnCloseReceived(ref thisLockTaken))
										{
											WebSocketBase.ReleaseLock(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
											bool flag = false;
											try
											{
												bool flag2 = await this.m_WebSocket.StartOnCloseCompleted(thisLockTaken, sessionHandleLockTaken, cancellationToken).SuppressContextFlow<bool>();
												flag = flag2;
											}
											catch (Exception)
											{
												this.m_WebSocket.ResetFlagAndTakeLock(this.m_WebSocket.m_ThisLock, ref thisLockTaken);
												throw;
											}
											if (flag)
											{
												this.m_WebSocket.ResetFlagAndTakeLock(this.m_WebSocket.m_ThisLock, ref thisLockTaken);
												this.m_WebSocket.FinishOnCloseCompleted();
											}
										}
										this.m_WebSocket.FinishOnCloseReceived(this.ReceiveResult.CloseStatus.Value, this.ReceiveResult.CloseStatusDescription);
									}
									finally
									{
										if (thisLockTaken)
										{
											WebSocketBase.ReleaseLock(this.m_WebSocket.m_ThisLock, ref thisLockTaken);
										}
									}
								}
								completed = true;
								continue;
							case WebSocketProtocolComponent.Action.SendToNetwork:
							{
								int bytesSent = 0;
								try
								{
									if (this.m_WebSocket.State != WebSocketState.CloseSent || (bufferType != (WebSocketProtocolComponent.BufferType)2147483653U && bufferType != (WebSocketProtocolComponent.BufferType)2147483654U))
									{
										if (bufferCount != 0U)
										{
											List<ArraySegment<byte>> list = new List<ArraySegment<byte>>((int)bufferCount);
											int sendBufferSize = 0;
											ArraySegment<byte> arraySegment = this.m_WebSocket.m_InternalBuffer.ConvertNativeBuffer(action, array[0], bufferType);
											list.Add(arraySegment);
											sendBufferSize += arraySegment.Count;
											if (bufferCount == 2U)
											{
												ArraySegment<byte> arraySegment2 = ((!this.m_WebSocket.m_InternalBuffer.IsPinnedSendPayloadBuffer(array[1], bufferType)) ? this.m_WebSocket.m_InternalBuffer.ConvertNativeBuffer(action, array[1], bufferType) : this.m_WebSocket.m_InternalBuffer.ConvertPinnedSendPayloadFromNative(array[1], bufferType));
												list.Add(arraySegment2);
												sendBufferSize += arraySegment2.Count;
											}
											WebSocketBase.ReleaseLock(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
											WebSocketHelpers.ThrowIfConnectionAborted(this.m_WebSocket.m_InnerStream, false);
											await this.m_WebSocket.SendFrameAsync(list, cancellationToken).SuppressContextFlow();
											Monitor.Enter(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
											this.m_WebSocket.ThrowIfPendingException();
											bytesSent += sendBufferSize;
											this.m_WebSocket.m_KeepAliveTracker.OnDataSent();
										}
									}
									continue;
								}
								finally
								{
									WebSocketProtocolComponent.WebSocketCompleteAction(this.m_WebSocket, actionContext, bytesSent);
								}
								goto IL_0693;
							}
							case WebSocketProtocolComponent.Action.IndicateSendComplete:
								break;
							case WebSocketProtocolComponent.Action.ReceiveFromNetwork:
							{
								int count = 0;
								try
								{
									ArraySegment<byte> arraySegment3 = this.m_WebSocket.m_InternalBuffer.ConvertNativeBuffer(action, array[0], bufferType);
									WebSocketBase.ReleaseLock(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
									WebSocketHelpers.ThrowIfConnectionAborted(this.m_WebSocket.m_InnerStream, true);
									try
									{
										count = await this.m_WebSocket.m_InnerStream.ReadAsync(arraySegment3.Array, arraySegment3.Offset, arraySegment3.Count, cancellationToken).SuppressContextFlow<int>();
										this.m_WebSocket.m_KeepAliveTracker.OnDataReceived();
									}
									catch (ObjectDisposedException ex)
									{
										throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex);
									}
									catch (NotSupportedException ex2)
									{
										throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex2);
									}
									Monitor.Enter(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
									this.m_WebSocket.ThrowIfPendingException();
									if (count == 0)
									{
										throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely);
									}
									continue;
								}
								finally
								{
									WebSocketProtocolComponent.WebSocketCompleteAction(this.m_WebSocket, actionContext, count);
								}
								break;
							}
							case WebSocketProtocolComponent.Action.IndicateReceiveComplete:
								this.ProcessAction_IndicateReceiveComplete(buffer, bufferType, action, array, bufferCount, actionContext);
								continue;
							default:
								goto IL_0693;
							}
							WebSocketProtocolComponent.WebSocketCompleteAction(this.m_WebSocket, actionContext, 0);
							this.AsyncOperationCompleted = true;
							WebSocketBase.ReleaseLock(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
							await this.m_WebSocket.m_InnerStream.FlushAsync().SuppressContextFlow();
							Monitor.Enter(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
							continue;
							IL_0693:
							string text = string.Format(CultureInfo.InvariantCulture, "Invalid action '{0}' returned from WebSocketGetAction.", new object[] { action });
							throw new InvalidOperationException();
						}
						WebSocketBase.ReleaseLock(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
						Monitor.Enter(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
					}
				}
				finally
				{
					this.Cleanup();
					WebSocketBase.ReleaseLock(this.m_WebSocket.SessionHandle, ref sessionHandleLockTaken);
				}
				return this.ReceiveResult;
			}

			// Token: 0x0400325D RID: 12893
			private readonly WebSocketBase m_WebSocket;

			// Token: 0x02000919 RID: 2329
			public class ReceiveOperation : WebSocketBase.WebSocketOperation
			{
				// Token: 0x06004644 RID: 17988 RVA: 0x001255D8 File Offset: 0x001237D8
				public ReceiveOperation(WebSocketBase webSocket)
					: base(webSocket)
				{
				}

				// Token: 0x17000FDB RID: 4059
				// (get) Token: 0x06004645 RID: 17989 RVA: 0x001255E1 File Offset: 0x001237E1
				protected override WebSocketProtocolComponent.ActionQueue ActionQueue
				{
					get
					{
						return WebSocketProtocolComponent.ActionQueue.Receive;
					}
				}

				// Token: 0x17000FDC RID: 4060
				// (get) Token: 0x06004646 RID: 17990 RVA: 0x001255E4 File Offset: 0x001237E4
				protected override int BufferCount
				{
					get
					{
						return 1;
					}
				}

				// Token: 0x06004647 RID: 17991 RVA: 0x001255E8 File Offset: 0x001237E8
				protected override void Initialize(ArraySegment<byte>? buffer, CancellationToken cancellationToken)
				{
					this.m_PongReceived = false;
					this.m_ReceiveCompleted = false;
					this.m_WebSocket.ThrowIfDisposed();
					switch (Interlocked.CompareExchange(ref this.m_WebSocket.m_ReceiveState, 1, 0))
					{
					case 0:
						this.m_ReceiveState = 1;
						return;
					case 1:
						break;
					case 2:
					{
						WebSocketReceiveResult webSocketReceiveResult;
						if (!this.m_WebSocket.m_InternalBuffer.ReceiveFromBufferedPayload(buffer.Value, out webSocketReceiveResult))
						{
							this.m_WebSocket.UpdateReceiveState(0, 2);
						}
						base.ReceiveResult = webSocketReceiveResult;
						this.m_ReceiveCompleted = true;
						break;
					}
					default:
						return;
					}
				}

				// Token: 0x06004648 RID: 17992 RVA: 0x00125672 File Offset: 0x00123872
				protected override void Cleanup()
				{
				}

				// Token: 0x06004649 RID: 17993 RVA: 0x00125674 File Offset: 0x00123874
				protected override bool ShouldContinue(CancellationToken cancellationToken)
				{
					cancellationToken.ThrowIfCancellationRequested();
					if (this.m_ReceiveCompleted)
					{
						return false;
					}
					this.m_WebSocket.ThrowIfDisposed();
					this.m_WebSocket.ThrowIfPendingException();
					WebSocketProtocolComponent.WebSocketReceive(this.m_WebSocket);
					return true;
				}

				// Token: 0x0600464A RID: 17994 RVA: 0x001256A9 File Offset: 0x001238A9
				protected override bool ProcessAction_NoAction()
				{
					if (this.m_PongReceived)
					{
						this.m_ReceiveCompleted = false;
						this.m_PongReceived = false;
						return false;
					}
					this.m_ReceiveCompleted = true;
					return base.ReceiveResult.MessageType == WebSocketMessageType.Close;
				}

				// Token: 0x0600464B RID: 17995 RVA: 0x001256DC File Offset: 0x001238DC
				protected override void ProcessAction_IndicateReceiveComplete(ArraySegment<byte>? buffer, WebSocketProtocolComponent.BufferType bufferType, WebSocketProtocolComponent.Action action, WebSocketProtocolComponent.Buffer[] dataBuffers, uint dataBufferCount, IntPtr actionContext)
				{
					int num = 0;
					this.m_PongReceived = false;
					if (bufferType == (WebSocketProtocolComponent.BufferType)2147483653U)
					{
						this.m_PongReceived = true;
						WebSocketProtocolComponent.WebSocketCompleteAction(this.m_WebSocket, actionContext, num);
						return;
					}
					WebSocketReceiveResult webSocketReceiveResult;
					try
					{
						WebSocketMessageType messageType = WebSocketBase.GetMessageType(bufferType);
						int num2 = 0;
						if (bufferType == (WebSocketProtocolComponent.BufferType)2147483652U)
						{
							ArraySegment<byte> arraySegment = WebSocketHelpers.EmptyPayload;
							WebSocketCloseStatus webSocketCloseStatus;
							string text;
							this.m_WebSocket.m_InternalBuffer.ConvertCloseBuffer(action, dataBuffers[0], out webSocketCloseStatus, out text);
							webSocketReceiveResult = new WebSocketReceiveResult(num, messageType, true, new WebSocketCloseStatus?(webSocketCloseStatus), text);
						}
						else
						{
							ArraySegment<byte> arraySegment = this.m_WebSocket.m_InternalBuffer.ConvertNativeBuffer(action, dataBuffers[0], bufferType);
							bool flag = bufferType == (WebSocketProtocolComponent.BufferType)2147483650U || bufferType == (WebSocketProtocolComponent.BufferType)2147483648U || bufferType == (WebSocketProtocolComponent.BufferType)2147483652U;
							if (arraySegment.Count > buffer.Value.Count)
							{
								this.m_WebSocket.m_InternalBuffer.BufferPayload(arraySegment, buffer.Value.Count, messageType, flag);
								num2 = 2;
								flag = false;
							}
							num = Math.Min(arraySegment.Count, buffer.Value.Count);
							if (num > 0)
							{
								Buffer.BlockCopy(arraySegment.Array, arraySegment.Offset, buffer.Value.Array, buffer.Value.Offset, num);
							}
							webSocketReceiveResult = new WebSocketReceiveResult(num, messageType, flag);
						}
						this.m_WebSocket.UpdateReceiveState(num2, this.m_ReceiveState);
					}
					finally
					{
						WebSocketProtocolComponent.WebSocketCompleteAction(this.m_WebSocket, actionContext, num);
					}
					base.ReceiveResult = webSocketReceiveResult;
				}

				// Token: 0x04003D75 RID: 15733
				private int m_ReceiveState;

				// Token: 0x04003D76 RID: 15734
				private bool m_PongReceived;

				// Token: 0x04003D77 RID: 15735
				private bool m_ReceiveCompleted;
			}

			// Token: 0x0200091A RID: 2330
			public class SendOperation : WebSocketBase.WebSocketOperation
			{
				// Token: 0x0600464C RID: 17996 RVA: 0x00125880 File Offset: 0x00123A80
				public SendOperation(WebSocketBase webSocket)
					: base(webSocket)
				{
				}

				// Token: 0x17000FDD RID: 4061
				// (get) Token: 0x0600464D RID: 17997 RVA: 0x00125889 File Offset: 0x00123A89
				protected override WebSocketProtocolComponent.ActionQueue ActionQueue
				{
					get
					{
						return WebSocketProtocolComponent.ActionQueue.Send;
					}
				}

				// Token: 0x17000FDE RID: 4062
				// (get) Token: 0x0600464E RID: 17998 RVA: 0x0012588C File Offset: 0x00123A8C
				protected override int BufferCount
				{
					get
					{
						return 2;
					}
				}

				// Token: 0x0600464F RID: 17999 RVA: 0x00125890 File Offset: 0x00123A90
				protected virtual WebSocketProtocolComponent.Buffer? CreateBuffer(ArraySegment<byte>? buffer)
				{
					if (buffer == null)
					{
						return null;
					}
					WebSocketProtocolComponent.Buffer buffer2 = default(WebSocketProtocolComponent.Buffer);
					this.m_WebSocket.m_InternalBuffer.PinSendBuffer(buffer.Value, out this.m_BufferHasBeenPinned);
					buffer2.Data.BufferData = this.m_WebSocket.m_InternalBuffer.ConvertPinnedSendPayloadToNative(buffer.Value);
					buffer2.Data.BufferLength = (uint)buffer.Value.Count;
					return new WebSocketProtocolComponent.Buffer?(buffer2);
				}

				// Token: 0x06004650 RID: 18000 RVA: 0x00125919 File Offset: 0x00123B19
				protected override bool ProcessAction_NoAction()
				{
					return false;
				}

				// Token: 0x06004651 RID: 18001 RVA: 0x0012591C File Offset: 0x00123B1C
				protected override void Cleanup()
				{
					if (this.m_BufferHasBeenPinned)
					{
						this.m_BufferHasBeenPinned = false;
						this.m_WebSocket.m_InternalBuffer.ReleasePinnedSendBuffer();
					}
				}

				// Token: 0x17000FDF RID: 4063
				// (get) Token: 0x06004652 RID: 18002 RVA: 0x0012593D File Offset: 0x00123B3D
				// (set) Token: 0x06004653 RID: 18003 RVA: 0x00125945 File Offset: 0x00123B45
				internal WebSocketProtocolComponent.BufferType BufferType { get; set; }

				// Token: 0x06004654 RID: 18004 RVA: 0x00125950 File Offset: 0x00123B50
				protected override void Initialize(ArraySegment<byte>? buffer, CancellationToken cancellationToken)
				{
					this.m_WebSocket.ThrowIfDisposed();
					this.m_WebSocket.ThrowIfPendingException();
					WebSocketProtocolComponent.Buffer? buffer2 = this.CreateBuffer(buffer);
					if (buffer2 != null)
					{
						WebSocketProtocolComponent.WebSocketSend(this.m_WebSocket, this.BufferType, buffer2.Value);
						return;
					}
					WebSocketProtocolComponent.WebSocketSendWithoutBody(this.m_WebSocket, this.BufferType);
				}

				// Token: 0x06004655 RID: 18005 RVA: 0x001259AE File Offset: 0x00123BAE
				protected override bool ShouldContinue(CancellationToken cancellationToken)
				{
					if (base.AsyncOperationCompleted)
					{
						return false;
					}
					cancellationToken.ThrowIfCancellationRequested();
					return true;
				}

				// Token: 0x04003D78 RID: 15736
				protected bool m_BufferHasBeenPinned;
			}

			// Token: 0x0200091B RID: 2331
			public class CloseOutputOperation : WebSocketBase.WebSocketOperation.SendOperation
			{
				// Token: 0x06004656 RID: 18006 RVA: 0x001259C2 File Offset: 0x00123BC2
				public CloseOutputOperation(WebSocketBase webSocket)
					: base(webSocket)
				{
					base.BufferType = (WebSocketProtocolComponent.BufferType)2147483652U;
				}

				// Token: 0x17000FE0 RID: 4064
				// (get) Token: 0x06004657 RID: 18007 RVA: 0x001259D6 File Offset: 0x00123BD6
				// (set) Token: 0x06004658 RID: 18008 RVA: 0x001259DE File Offset: 0x00123BDE
				internal WebSocketCloseStatus CloseStatus { get; set; }

				// Token: 0x17000FE1 RID: 4065
				// (get) Token: 0x06004659 RID: 18009 RVA: 0x001259E7 File Offset: 0x00123BE7
				// (set) Token: 0x0600465A RID: 18010 RVA: 0x001259EF File Offset: 0x00123BEF
				internal string CloseReason { get; set; }

				// Token: 0x0600465B RID: 18011 RVA: 0x001259F8 File Offset: 0x00123BF8
				protected override WebSocketProtocolComponent.Buffer? CreateBuffer(ArraySegment<byte>? buffer)
				{
					this.m_WebSocket.ThrowIfDisposed();
					this.m_WebSocket.ThrowIfPendingException();
					if (this.CloseStatus == WebSocketCloseStatus.Empty)
					{
						return null;
					}
					WebSocketProtocolComponent.Buffer buffer2 = default(WebSocketProtocolComponent.Buffer);
					if (this.CloseReason != null)
					{
						byte[] bytes = Encoding.UTF8.GetBytes(this.CloseReason);
						ArraySegment<byte> arraySegment = new ArraySegment<byte>(bytes, 0, Math.Min(123, bytes.Length));
						this.m_WebSocket.m_InternalBuffer.PinSendBuffer(arraySegment, out this.m_BufferHasBeenPinned);
						buffer2.CloseStatus.ReasonData = this.m_WebSocket.m_InternalBuffer.ConvertPinnedSendPayloadToNative(arraySegment);
						buffer2.CloseStatus.ReasonLength = (uint)arraySegment.Count;
					}
					buffer2.CloseStatus.CloseStatus = (ushort)this.CloseStatus;
					return new WebSocketProtocolComponent.Buffer?(buffer2);
				}
			}
		}

		// Token: 0x0200076D RID: 1901
		private abstract class KeepAliveTracker : IDisposable
		{
			// Token: 0x06004261 RID: 16993
			public abstract void OnDataReceived();

			// Token: 0x06004262 RID: 16994
			public abstract void OnDataSent();

			// Token: 0x06004263 RID: 16995
			public abstract void Dispose();

			// Token: 0x06004264 RID: 16996
			public abstract void StartTimer(WebSocketBase webSocket);

			// Token: 0x06004265 RID: 16997
			public abstract void ResetTimer();

			// Token: 0x06004266 RID: 16998
			public abstract bool ShouldSendKeepAlive();

			// Token: 0x06004267 RID: 16999 RVA: 0x00113453 File Offset: 0x00111653
			public static WebSocketBase.KeepAliveTracker Create(TimeSpan keepAliveInterval)
			{
				if ((int)keepAliveInterval.TotalMilliseconds > 0)
				{
					return new WebSocketBase.KeepAliveTracker.DefaultKeepAliveTracker(keepAliveInterval);
				}
				return new WebSocketBase.KeepAliveTracker.DisabledKeepAliveTracker();
			}

			// Token: 0x0200091D RID: 2333
			private class DisabledKeepAliveTracker : WebSocketBase.KeepAliveTracker
			{
				// Token: 0x0600465E RID: 18014 RVA: 0x00126302 File Offset: 0x00124502
				public override void OnDataReceived()
				{
				}

				// Token: 0x0600465F RID: 18015 RVA: 0x00126304 File Offset: 0x00124504
				public override void OnDataSent()
				{
				}

				// Token: 0x06004660 RID: 18016 RVA: 0x00126306 File Offset: 0x00124506
				public override void ResetTimer()
				{
				}

				// Token: 0x06004661 RID: 18017 RVA: 0x00126308 File Offset: 0x00124508
				public override void StartTimer(WebSocketBase webSocket)
				{
				}

				// Token: 0x06004662 RID: 18018 RVA: 0x0012630A File Offset: 0x0012450A
				public override bool ShouldSendKeepAlive()
				{
					return false;
				}

				// Token: 0x06004663 RID: 18019 RVA: 0x0012630D File Offset: 0x0012450D
				public override void Dispose()
				{
				}
			}

			// Token: 0x0200091E RID: 2334
			private class DefaultKeepAliveTracker : WebSocketBase.KeepAliveTracker
			{
				// Token: 0x06004665 RID: 18021 RVA: 0x00126317 File Offset: 0x00124517
				public DefaultKeepAliveTracker(TimeSpan keepAliveInterval)
				{
					this.m_KeepAliveInterval = keepAliveInterval;
					this.m_LastSendActivity = new Stopwatch();
					this.m_LastReceiveActivity = new Stopwatch();
				}

				// Token: 0x06004666 RID: 18022 RVA: 0x0012633C File Offset: 0x0012453C
				public override void OnDataReceived()
				{
					this.m_LastReceiveActivity.Restart();
				}

				// Token: 0x06004667 RID: 18023 RVA: 0x00126349 File Offset: 0x00124549
				public override void OnDataSent()
				{
					this.m_LastSendActivity.Restart();
				}

				// Token: 0x06004668 RID: 18024 RVA: 0x00126358 File Offset: 0x00124558
				public override void ResetTimer()
				{
					this.ResetTimer((int)this.m_KeepAliveInterval.TotalMilliseconds);
				}

				// Token: 0x06004669 RID: 18025 RVA: 0x0012637C File Offset: 0x0012457C
				public override void StartTimer(WebSocketBase webSocket)
				{
					int num = (int)this.m_KeepAliveInterval.TotalMilliseconds;
					if (ExecutionContext.IsFlowSuppressed())
					{
						this.m_KeepAliveTimer = new Timer(WebSocketBase.KeepAliveTracker.DefaultKeepAliveTracker.s_KeepAliveTimerElapsedCallback, webSocket, -1, -1);
						this.m_KeepAliveTimer.Change(num, -1);
						return;
					}
					using (ExecutionContext.SuppressFlow())
					{
						this.m_KeepAliveTimer = new Timer(WebSocketBase.KeepAliveTracker.DefaultKeepAliveTracker.s_KeepAliveTimerElapsedCallback, webSocket, -1, -1);
						this.m_KeepAliveTimer.Change(num, -1);
					}
				}

				// Token: 0x0600466A RID: 18026 RVA: 0x0012640C File Offset: 0x0012460C
				public override bool ShouldSendKeepAlive()
				{
					TimeSpan idleTime = this.GetIdleTime();
					if (idleTime >= this.m_KeepAliveInterval)
					{
						return true;
					}
					this.ResetTimer((int)(this.m_KeepAliveInterval - idleTime).TotalMilliseconds);
					return false;
				}

				// Token: 0x0600466B RID: 18027 RVA: 0x0012644C File Offset: 0x0012464C
				public override void Dispose()
				{
					this.m_KeepAliveTimer.Dispose();
				}

				// Token: 0x0600466C RID: 18028 RVA: 0x00126459 File Offset: 0x00124659
				private void ResetTimer(int dueInMilliseconds)
				{
					this.m_KeepAliveTimer.Change(dueInMilliseconds, -1);
				}

				// Token: 0x0600466D RID: 18029 RVA: 0x0012646C File Offset: 0x0012466C
				private TimeSpan GetIdleTime()
				{
					TimeSpan timeElapsed = this.GetTimeElapsed(this.m_LastSendActivity);
					TimeSpan timeElapsed2 = this.GetTimeElapsed(this.m_LastReceiveActivity);
					if (timeElapsed2 < timeElapsed)
					{
						return timeElapsed2;
					}
					return timeElapsed;
				}

				// Token: 0x0600466E RID: 18030 RVA: 0x0012649F File Offset: 0x0012469F
				private TimeSpan GetTimeElapsed(Stopwatch watch)
				{
					if (watch.IsRunning)
					{
						return watch.Elapsed;
					}
					return this.m_KeepAliveInterval;
				}

				// Token: 0x04003D8B RID: 15755
				private static readonly TimerCallback s_KeepAliveTimerElapsedCallback = new TimerCallback(WebSocketBase.OnKeepAlive);

				// Token: 0x04003D8C RID: 15756
				private readonly TimeSpan m_KeepAliveInterval;

				// Token: 0x04003D8D RID: 15757
				private readonly Stopwatch m_LastSendActivity;

				// Token: 0x04003D8E RID: 15758
				private readonly Stopwatch m_LastReceiveActivity;

				// Token: 0x04003D8F RID: 15759
				private Timer m_KeepAliveTimer;
			}
		}

		// Token: 0x0200076E RID: 1902
		private class OutstandingOperationHelper : IDisposable
		{
			// Token: 0x06004269 RID: 17001 RVA: 0x00113474 File Offset: 0x00111674
			public bool TryStartOperation(CancellationToken userCancellationToken, out CancellationToken linkedCancellationToken)
			{
				linkedCancellationToken = CancellationToken.None;
				this.ThrowIfDisposed();
				object thisLock = this.m_ThisLock;
				bool flag2;
				lock (thisLock)
				{
					int num = this.m_OperationsOutstanding + 1;
					this.m_OperationsOutstanding = num;
					int num2 = num;
					if (num2 == 1)
					{
						linkedCancellationToken = this.CreateLinkedCancellationToken(userCancellationToken);
						flag2 = true;
					}
					else
					{
						flag2 = false;
					}
				}
				return flag2;
			}

			// Token: 0x0600426A RID: 17002 RVA: 0x001134F0 File Offset: 0x001116F0
			public void CompleteOperation(bool ownsCancellationTokenSource)
			{
				if (this.m_IsDisposed)
				{
					return;
				}
				CancellationTokenSource cancellationTokenSource = null;
				object thisLock = this.m_ThisLock;
				lock (thisLock)
				{
					this.m_OperationsOutstanding--;
					if (ownsCancellationTokenSource)
					{
						cancellationTokenSource = this.m_CancellationTokenSource;
						this.m_CancellationTokenSource = null;
					}
				}
				if (cancellationTokenSource != null)
				{
					cancellationTokenSource.Dispose();
				}
			}

			// Token: 0x0600426B RID: 17003 RVA: 0x00113568 File Offset: 0x00111768
			private CancellationToken CreateLinkedCancellationToken(CancellationToken cancellationToken)
			{
				CancellationTokenSource cancellationTokenSource;
				if (cancellationToken == CancellationToken.None)
				{
					cancellationTokenSource = new CancellationTokenSource();
				}
				else
				{
					cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource().Token);
				}
				this.m_CancellationTokenSource = cancellationTokenSource;
				return cancellationTokenSource.Token;
			}

			// Token: 0x0600426C RID: 17004 RVA: 0x001135AC File Offset: 0x001117AC
			public void CancelIO()
			{
				CancellationTokenSource cancellationTokenSource = null;
				object thisLock = this.m_ThisLock;
				lock (thisLock)
				{
					if (this.m_OperationsOutstanding == 0)
					{
						return;
					}
					cancellationTokenSource = this.m_CancellationTokenSource;
				}
				if (cancellationTokenSource != null)
				{
					try
					{
						cancellationTokenSource.Cancel();
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}

			// Token: 0x0600426D RID: 17005 RVA: 0x00113618 File Offset: 0x00111818
			public void Dispose()
			{
				if (this.m_IsDisposed)
				{
					return;
				}
				CancellationTokenSource cancellationTokenSource = null;
				object thisLock = this.m_ThisLock;
				lock (thisLock)
				{
					if (this.m_IsDisposed)
					{
						return;
					}
					this.m_IsDisposed = true;
					cancellationTokenSource = this.m_CancellationTokenSource;
					this.m_CancellationTokenSource = null;
				}
				if (cancellationTokenSource != null)
				{
					cancellationTokenSource.Dispose();
				}
			}

			// Token: 0x0600426E RID: 17006 RVA: 0x00113690 File Offset: 0x00111890
			private void ThrowIfDisposed()
			{
				if (this.m_IsDisposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
			}

			// Token: 0x0400325F RID: 12895
			private volatile int m_OperationsOutstanding;

			// Token: 0x04003260 RID: 12896
			private volatile CancellationTokenSource m_CancellationTokenSource;

			// Token: 0x04003261 RID: 12897
			private volatile bool m_IsDisposed;

			// Token: 0x04003262 RID: 12898
			private readonly object m_ThisLock = new object();
		}

		// Token: 0x0200076F RID: 1903
		internal interface IWebSocketStream
		{
			// Token: 0x06004270 RID: 17008
			void SwitchToOpaqueMode(WebSocketBase webSocket);

			// Token: 0x06004271 RID: 17009
			void Abort();

			// Token: 0x17000F2B RID: 3883
			// (get) Token: 0x06004272 RID: 17010
			bool SupportsMultipleWrite { get; }

			// Token: 0x06004273 RID: 17011
			Task MultipleWriteAsync(IList<ArraySegment<byte>> buffers, CancellationToken cancellationToken);

			// Token: 0x06004274 RID: 17012
			Task CloseNetworkConnectionAsync(CancellationToken cancellationToken);
		}

		// Token: 0x02000770 RID: 1904
		private static class ReceiveState
		{
			// Token: 0x04003263 RID: 12899
			internal const int SendOperation = -1;

			// Token: 0x04003264 RID: 12900
			internal const int Idle = 0;

			// Token: 0x04003265 RID: 12901
			internal const int Application = 1;

			// Token: 0x04003266 RID: 12902
			internal const int PayloadAvailable = 2;
		}

		// Token: 0x02000771 RID: 1905
		internal static class Methods
		{
			// Token: 0x04003267 RID: 12903
			internal const string ReceiveAsync = "ReceiveAsync";

			// Token: 0x04003268 RID: 12904
			internal const string SendAsync = "SendAsync";

			// Token: 0x04003269 RID: 12905
			internal const string CloseAsync = "CloseAsync";

			// Token: 0x0400326A RID: 12906
			internal const string CloseOutputAsync = "CloseOutputAsync";

			// Token: 0x0400326B RID: 12907
			internal const string Abort = "Abort";

			// Token: 0x0400326C RID: 12908
			internal const string Initialize = "Initialize";

			// Token: 0x0400326D RID: 12909
			internal const string Fault = "Fault";

			// Token: 0x0400326E RID: 12910
			internal const string StartOnCloseCompleted = "StartOnCloseCompleted";

			// Token: 0x0400326F RID: 12911
			internal const string FinishOnCloseReceived = "FinishOnCloseReceived";

			// Token: 0x04003270 RID: 12912
			internal const string OnKeepAlive = "OnKeepAlive";
		}
	}
}
