using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x0200037C RID: 892
	public class SocketAsyncEventArgs : EventArgs, IDisposable
	{
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06002134 RID: 8500 RVA: 0x0009F048 File Offset: 0x0009D248
		// (remove) Token: 0x06002135 RID: 8501 RVA: 0x0009F080 File Offset: 0x0009D280
		private event EventHandler<SocketAsyncEventArgs> m_Completed;

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002136 RID: 8502 RVA: 0x0009F0B5 File Offset: 0x0009D2B5
		// (set) Token: 0x06002137 RID: 8503 RVA: 0x0009F0BD File Offset: 0x0009D2BD
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public SocketClientAccessPolicyProtocol SocketClientAccessPolicyProtocol { get; set; }

		// Token: 0x06002138 RID: 8504 RVA: 0x0009F0C6 File Offset: 0x0009D2C6
		public SocketAsyncEventArgs()
		{
			this.m_ExecutionCallback = new ContextCallback(this.ExecutionCallback);
			this.m_SendPacketsSendSize = 0;
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002139 RID: 8505 RVA: 0x0009F0E7 File Offset: 0x0009D2E7
		// (set) Token: 0x0600213A RID: 8506 RVA: 0x0009F0EF File Offset: 0x0009D2EF
		public Socket AcceptSocket
		{
			get
			{
				return this.m_AcceptSocket;
			}
			set
			{
				this.m_AcceptSocket = value;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x0009F0F8 File Offset: 0x0009D2F8
		public Socket ConnectSocket
		{
			get
			{
				return this.m_ConnectSocket;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x0600213C RID: 8508 RVA: 0x0009F100 File Offset: 0x0009D300
		public byte[] Buffer
		{
			get
			{
				return this.m_Buffer;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x0009F108 File Offset: 0x0009D308
		public int Offset
		{
			get
			{
				return this.m_Offset;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x0600213E RID: 8510 RVA: 0x0009F110 File Offset: 0x0009D310
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x0009F118 File Offset: 0x0009D318
		// (set) Token: 0x06002140 RID: 8512 RVA: 0x0009F120 File Offset: 0x0009D320
		public IList<ArraySegment<byte>> BufferList
		{
			get
			{
				return this.m_BufferList;
			}
			set
			{
				this.StartConfiguring();
				try
				{
					if (value != null && this.m_Buffer != null)
					{
						throw new ArgumentException(SR.GetString("net_ambiguousbuffers", new object[] { "Buffer" }));
					}
					this.m_BufferList = value;
					this.m_BufferListChanged = true;
					this.CheckPinMultipleBuffers();
				}
				finally
				{
					this.Complete();
				}
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002141 RID: 8513 RVA: 0x0009F18C File Offset: 0x0009D38C
		public int BytesTransferred
		{
			get
			{
				return this.m_BytesTransferred;
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06002142 RID: 8514 RVA: 0x0009F194 File Offset: 0x0009D394
		// (remove) Token: 0x06002143 RID: 8515 RVA: 0x0009F1A4 File Offset: 0x0009D3A4
		public event EventHandler<SocketAsyncEventArgs> Completed
		{
			add
			{
				this.m_Completed += value;
				this.m_CompletedChanged = true;
			}
			remove
			{
				this.m_Completed -= value;
				this.m_CompletedChanged = true;
			}
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0009F1B4 File Offset: 0x0009D3B4
		protected virtual void OnCompleted(SocketAsyncEventArgs e)
		{
			EventHandler<SocketAsyncEventArgs> completed = this.m_Completed;
			if (completed != null)
			{
				completed(e.m_CurrentSocket, e);
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x0009F1D8 File Offset: 0x0009D3D8
		// (set) Token: 0x06002146 RID: 8518 RVA: 0x0009F1E0 File Offset: 0x0009D3E0
		public bool DisconnectReuseSocket
		{
			get
			{
				return this.m_DisconnectReuseSocket;
			}
			set
			{
				this.m_DisconnectReuseSocket = value;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002147 RID: 8519 RVA: 0x0009F1E9 File Offset: 0x0009D3E9
		public SocketAsyncOperation LastOperation
		{
			get
			{
				return this.m_CompletedOperation;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002148 RID: 8520 RVA: 0x0009F1F1 File Offset: 0x0009D3F1
		public IPPacketInformation ReceiveMessageFromPacketInfo
		{
			get
			{
				return this.m_ReceiveMessageFromPacketInfo;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x0009F1F9 File Offset: 0x0009D3F9
		// (set) Token: 0x0600214A RID: 8522 RVA: 0x0009F201 File Offset: 0x0009D401
		public EndPoint RemoteEndPoint
		{
			get
			{
				return this.m_RemoteEndPoint;
			}
			set
			{
				this.m_RemoteEndPoint = value;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x0009F20A File Offset: 0x0009D40A
		// (set) Token: 0x0600214C RID: 8524 RVA: 0x0009F214 File Offset: 0x0009D414
		public SendPacketsElement[] SendPacketsElements
		{
			get
			{
				return this.m_SendPacketsElements;
			}
			set
			{
				this.StartConfiguring();
				try
				{
					this.m_SendPacketsElements = value;
					this.m_SendPacketsElementsInternal = null;
				}
				finally
				{
					this.Complete();
				}
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x0009F250 File Offset: 0x0009D450
		// (set) Token: 0x0600214E RID: 8526 RVA: 0x0009F258 File Offset: 0x0009D458
		public TransmitFileOptions SendPacketsFlags
		{
			get
			{
				return this.m_SendPacketsFlags;
			}
			set
			{
				this.m_SendPacketsFlags = value;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x0009F261 File Offset: 0x0009D461
		// (set) Token: 0x06002150 RID: 8528 RVA: 0x0009F269 File Offset: 0x0009D469
		public int SendPacketsSendSize
		{
			get
			{
				return this.m_SendPacketsSendSize;
			}
			set
			{
				this.m_SendPacketsSendSize = value;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x0009F272 File Offset: 0x0009D472
		// (set) Token: 0x06002152 RID: 8530 RVA: 0x0009F27A File Offset: 0x0009D47A
		public SocketError SocketError
		{
			get
			{
				return this.m_SocketError;
			}
			set
			{
				this.m_SocketError = value;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002153 RID: 8531 RVA: 0x0009F283 File Offset: 0x0009D483
		public Exception ConnectByNameError
		{
			get
			{
				return this.m_ConnectByNameError;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002154 RID: 8532 RVA: 0x0009F28B File Offset: 0x0009D48B
		// (set) Token: 0x06002155 RID: 8533 RVA: 0x0009F293 File Offset: 0x0009D493
		public SocketFlags SocketFlags
		{
			get
			{
				return this.m_SocketFlags;
			}
			set
			{
				this.m_SocketFlags = value;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002156 RID: 8534 RVA: 0x0009F29C File Offset: 0x0009D49C
		// (set) Token: 0x06002157 RID: 8535 RVA: 0x0009F2A4 File Offset: 0x0009D4A4
		public object UserToken
		{
			get
			{
				return this.m_UserToken;
			}
			set
			{
				this.m_UserToken = value;
			}
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x0009F2AD File Offset: 0x0009D4AD
		public void SetBuffer(byte[] buffer, int offset, int count)
		{
			this.SetBufferInternal(buffer, offset, count);
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x0009F2B8 File Offset: 0x0009D4B8
		public void SetBuffer(int offset, int count)
		{
			this.SetBufferInternal(this.m_Buffer, offset, count);
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x0009F2C8 File Offset: 0x0009D4C8
		private void SetBufferInternal(byte[] buffer, int offset, int count)
		{
			this.StartConfiguring();
			try
			{
				if (buffer == null)
				{
					this.m_Buffer = null;
					this.m_Offset = 0;
					this.m_Count = 0;
				}
				else
				{
					if (this.m_BufferList != null)
					{
						throw new ArgumentException(SR.GetString("net_ambiguousbuffers", new object[] { "BufferList" }));
					}
					if (offset < 0 || offset > buffer.Length)
					{
						throw new ArgumentOutOfRangeException("offset");
					}
					if (count < 0 || count > buffer.Length - offset)
					{
						throw new ArgumentOutOfRangeException("count");
					}
					this.m_Buffer = buffer;
					this.m_Offset = offset;
					this.m_Count = count;
				}
				this.CheckPinSingleBuffer(true);
			}
			finally
			{
				this.Complete();
			}
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x0009F37C File Offset: 0x0009D57C
		internal void SetResults(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.m_SocketError = socketError;
			this.m_ConnectByNameError = null;
			this.m_BytesTransferred = bytesTransferred;
			this.m_SocketFlags = flags;
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x0009F39C File Offset: 0x0009D59C
		internal void SetResults(Exception exception, int bytesTransferred, SocketFlags flags)
		{
			this.m_ConnectByNameError = exception;
			this.m_BytesTransferred = bytesTransferred;
			this.m_SocketFlags = flags;
			if (exception == null)
			{
				this.m_SocketError = SocketError.Success;
				return;
			}
			SocketException ex = exception as SocketException;
			if (ex != null)
			{
				this.m_SocketError = ex.SocketErrorCode;
				return;
			}
			this.m_SocketError = SocketError.SocketError;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x0009F3E7 File Offset: 0x0009D5E7
		private void ExecutionCallback(object ignored)
		{
			this.OnCompleted(this);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x0009F3F0 File Offset: 0x0009D5F0
		internal void Complete()
		{
			this.m_Operating = 0;
			if (this.m_DisposeCalled)
			{
				this.Dispose();
			}
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x0009F407 File Offset: 0x0009D607
		public void Dispose()
		{
			this.m_DisposeCalled = true;
			if (Interlocked.CompareExchange(ref this.m_Operating, 2, 0) != 0)
			{
				return;
			}
			this.FreeOverlapped(false);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x0009F430 File Offset: 0x0009D630
		~SocketAsyncEventArgs()
		{
			this.FreeOverlapped(true);
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x0009F460 File Offset: 0x0009D660
		private void StartConfiguring()
		{
			int num = Interlocked.CompareExchange(ref this.m_Operating, -1, 0);
			if (num == 1 || num == -1)
			{
				throw new InvalidOperationException(SR.GetString("net_socketopinprogress"));
			}
			if (num == 2)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x0009F4A8 File Offset: 0x0009D6A8
		internal void StartOperationCommon(Socket socket)
		{
			if (Interlocked.CompareExchange(ref this.m_Operating, 1, 0) == 0)
			{
				if (ExecutionContext.IsFlowSuppressed())
				{
					this.m_Context = null;
					this.m_ContextCopy = null;
				}
				else
				{
					if (this.m_CompletedChanged || socket != this.m_CurrentSocket)
					{
						this.m_CompletedChanged = false;
						this.m_Context = null;
						this.m_ContextCopy = null;
					}
					if (this.m_Context == null)
					{
						this.m_Context = ExecutionContext.Capture();
					}
					if (this.m_Context != null)
					{
						this.m_ContextCopy = this.m_Context.CreateCopy();
					}
				}
				this.m_CurrentSocket = socket;
				return;
			}
			if (this.m_DisposeCalled)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			throw new InvalidOperationException(SR.GetString("net_socketopinprogress"));
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x0009F560 File Offset: 0x0009D760
		internal void StartOperationAccept()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Accept;
			this.m_AcceptAddressBufferCount = 2 * (this.m_CurrentSocket.m_RightEndPoint.Serialize().Size + 16);
			if (this.m_Buffer != null)
			{
				if (this.m_Count < this.m_AcceptAddressBufferCount)
				{
					throw new ArgumentException(SR.GetString("net_buffercounttoosmall", new object[] { "Count" }));
				}
			}
			else
			{
				if (this.m_AcceptBuffer == null || this.m_AcceptBuffer.Length < this.m_AcceptAddressBufferCount)
				{
					this.m_AcceptBuffer = new byte[this.m_AcceptAddressBufferCount];
				}
				this.CheckPinSingleBuffer(false);
			}
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x0009F5F8 File Offset: 0x0009D7F8
		internal void StartOperationConnect()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Connect;
			this.m_MultipleConnect = null;
			this.m_ConnectSocket = null;
			this.PinSocketAddressBuffer();
			this.CheckPinNoBuffer();
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0009F61B File Offset: 0x0009D81B
		internal void StartOperationWrapperConnect(MultipleConnectAsync args)
		{
			this.m_CompletedOperation = SocketAsyncOperation.Connect;
			this.m_MultipleConnect = args;
			this.m_ConnectSocket = null;
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0009F632 File Offset: 0x0009D832
		internal void CancelConnectAsync()
		{
			if (this.m_Operating == 1 && this.m_CompletedOperation == SocketAsyncOperation.Connect)
			{
				if (this.m_MultipleConnect != null)
				{
					this.m_MultipleConnect.Cancel();
					return;
				}
				this.m_CurrentSocket.Close();
			}
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0009F665 File Offset: 0x0009D865
		internal void StartOperationDisconnect()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Disconnect;
			this.CheckPinNoBuffer();
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0009F674 File Offset: 0x0009D874
		internal void StartOperationReceive()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Receive;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0009F67D File Offset: 0x0009D87D
		internal void StartOperationReceiveFrom()
		{
			this.m_CompletedOperation = SocketAsyncOperation.ReceiveFrom;
			this.PinSocketAddressBuffer();
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x0009F68C File Offset: 0x0009D88C
		internal unsafe void StartOperationReceiveMessageFrom()
		{
			this.m_CompletedOperation = SocketAsyncOperation.ReceiveMessageFrom;
			this.PinSocketAddressBuffer();
			if (this.m_WSAMessageBuffer == null)
			{
				this.m_WSAMessageBuffer = new byte[SocketAsyncEventArgs.s_WSAMsgSize];
				this.m_WSAMessageBufferGCHandle = GCHandle.Alloc(this.m_WSAMessageBuffer, GCHandleType.Pinned);
				this.m_PtrWSAMessageBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSAMessageBuffer, 0);
			}
			IPAddress ipaddress = ((this.m_SocketAddress.Family == AddressFamily.InterNetworkV6) ? this.m_SocketAddress.GetIPAddress() : null);
			bool flag = this.m_CurrentSocket.AddressFamily == AddressFamily.InterNetwork || (ipaddress != null && ipaddress.IsIPv4MappedToIPv6);
			bool flag2 = this.m_CurrentSocket.AddressFamily == AddressFamily.InterNetworkV6;
			if (flag && (this.m_ControlBuffer == null || this.m_ControlBuffer.Length != SocketAsyncEventArgs.s_ControlDataSize))
			{
				if (this.m_ControlBufferGCHandle.IsAllocated)
				{
					this.m_ControlBufferGCHandle.Free();
				}
				this.m_ControlBuffer = new byte[SocketAsyncEventArgs.s_ControlDataSize];
			}
			else if (flag2 && (this.m_ControlBuffer == null || this.m_ControlBuffer.Length != SocketAsyncEventArgs.s_ControlDataIPv6Size))
			{
				if (this.m_ControlBufferGCHandle.IsAllocated)
				{
					this.m_ControlBufferGCHandle.Free();
				}
				this.m_ControlBuffer = new byte[SocketAsyncEventArgs.s_ControlDataIPv6Size];
			}
			if (!this.m_ControlBufferGCHandle.IsAllocated)
			{
				this.m_ControlBufferGCHandle = GCHandle.Alloc(this.m_ControlBuffer, GCHandleType.Pinned);
				this.m_PtrControlBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_ControlBuffer, 0);
			}
			if (this.m_Buffer != null)
			{
				if (this.m_WSARecvMsgWSABufferArray == null)
				{
					this.m_WSARecvMsgWSABufferArray = new WSABuffer[1];
				}
				this.m_WSARecvMsgWSABufferArray[0].Pointer = this.m_PtrSingleBuffer;
				this.m_WSARecvMsgWSABufferArray[0].Length = this.m_Count;
				this.m_WSARecvMsgWSABufferArrayGCHandle = GCHandle.Alloc(this.m_WSARecvMsgWSABufferArray, GCHandleType.Pinned);
				this.m_PtrWSARecvMsgWSABufferArray = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSARecvMsgWSABufferArray, 0);
			}
			else
			{
				this.m_WSARecvMsgWSABufferArrayGCHandle = GCHandle.Alloc(this.m_WSABufferArray, GCHandleType.Pinned);
				this.m_PtrWSARecvMsgWSABufferArray = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSABufferArray, 0);
			}
			UnsafeNclNativeMethods.OSSOCK.WSAMsg* ptr = (UnsafeNclNativeMethods.OSSOCK.WSAMsg*)(void*)this.m_PtrWSAMessageBuffer;
			ptr->socketAddress = this.m_PtrSocketAddressBuffer;
			ptr->addressLength = (uint)this.m_SocketAddress.Size;
			ptr->buffers = this.m_PtrWSARecvMsgWSABufferArray;
			if (this.m_Buffer != null)
			{
				ptr->count = 1U;
			}
			else
			{
				ptr->count = (uint)this.m_WSABufferArray.Length;
			}
			if (this.m_ControlBuffer != null)
			{
				ptr->controlBuffer.Pointer = this.m_PtrControlBuffer;
				ptr->controlBuffer.Length = this.m_ControlBuffer.Length;
			}
			ptr->flags = this.m_SocketFlags;
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x0009F905 File Offset: 0x0009DB05
		internal void StartOperationSend()
		{
			this.m_CompletedOperation = SocketAsyncOperation.Send;
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x0009F910 File Offset: 0x0009DB10
		internal void StartOperationSendPackets()
		{
			this.m_CompletedOperation = SocketAsyncOperation.SendPackets;
			if (this.m_SendPacketsElements != null)
			{
				this.m_SendPacketsElementsInternal = (SendPacketsElement[])this.m_SendPacketsElements.Clone();
			}
			this.m_SendPacketsElementsFileCount = 0;
			this.m_SendPacketsElementsBufferCount = 0;
			foreach (SendPacketsElement sendPacketsElement in this.m_SendPacketsElementsInternal)
			{
				if (sendPacketsElement != null)
				{
					if (sendPacketsElement.m_FilePath != null)
					{
						this.m_SendPacketsElementsFileCount++;
					}
					if (sendPacketsElement.m_Buffer != null && sendPacketsElement.m_Count > 0)
					{
						this.m_SendPacketsElementsBufferCount++;
					}
				}
			}
			if (this.m_SendPacketsElementsFileCount > 0)
			{
				this.m_SendPacketsFileStreams = new FileStream[this.m_SendPacketsElementsFileCount];
				this.m_SendPacketsFileHandles = new SafeHandle[this.m_SendPacketsElementsFileCount];
				int num = 0;
				foreach (SendPacketsElement sendPacketsElement2 in this.m_SendPacketsElementsInternal)
				{
					if (sendPacketsElement2 != null && sendPacketsElement2.m_FilePath != null)
					{
						Exception ex = null;
						try
						{
							this.m_SendPacketsFileStreams[num] = new FileStream(sendPacketsElement2.m_FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
						}
						catch (Exception ex2)
						{
							ex = ex2;
						}
						if (ex != null)
						{
							for (int k = 0; k < this.m_SendPacketsElementsFileCount; k++)
							{
								this.m_SendPacketsFileHandles[k] = null;
								if (this.m_SendPacketsFileStreams[k] != null)
								{
									this.m_SendPacketsFileStreams[k].Close();
									this.m_SendPacketsFileStreams[k] = null;
								}
							}
							throw ex;
						}
						ExceptionHelper.UnmanagedPermission.Assert();
						try
						{
							this.m_SendPacketsFileHandles[num] = this.m_SendPacketsFileStreams[num].SafeFileHandle;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						num++;
					}
				}
			}
			this.CheckPinSendPackets();
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x0009FAC8 File Offset: 0x0009DCC8
		internal void StartOperationSendTo()
		{
			this.m_CompletedOperation = SocketAsyncOperation.SendTo;
			this.PinSocketAddressBuffer();
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x0009FAD8 File Offset: 0x0009DCD8
		private void CheckPinNoBuffer()
		{
			if (this.m_PinState == SocketAsyncEventArgs.PinState.None)
			{
				this.SetupOverlappedSingle(true);
			}
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x0009FAEC File Offset: 0x0009DCEC
		private void CheckPinSingleBuffer(bool pinUsersBuffer)
		{
			if (pinUsersBuffer)
			{
				if (this.m_Buffer == null)
				{
					if (this.m_PinState == SocketAsyncEventArgs.PinState.SingleBuffer)
					{
						this.FreeOverlapped(false);
						return;
					}
				}
				else
				{
					if (this.m_PinState != SocketAsyncEventArgs.PinState.SingleBuffer || this.m_PinnedSingleBuffer != this.m_Buffer)
					{
						this.FreeOverlapped(false);
						this.SetupOverlappedSingle(true);
						return;
					}
					if (this.m_Offset != this.m_PinnedSingleBufferOffset)
					{
						this.m_PinnedSingleBufferOffset = this.m_Offset;
						this.m_PtrSingleBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_Buffer, this.m_Offset);
						this.m_WSABuffer.Pointer = this.m_PtrSingleBuffer;
					}
					if (this.m_Count != this.m_PinnedSingleBufferCount)
					{
						this.m_PinnedSingleBufferCount = this.m_Count;
						this.m_WSABuffer.Length = this.m_Count;
						return;
					}
				}
			}
			else if (this.m_PinState != SocketAsyncEventArgs.PinState.SingleAcceptBuffer || this.m_PinnedSingleBuffer != this.m_AcceptBuffer)
			{
				this.FreeOverlapped(false);
				this.SetupOverlappedSingle(false);
			}
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x0009FBD4 File Offset: 0x0009DDD4
		private void CheckPinMultipleBuffers()
		{
			if (this.m_BufferList == null)
			{
				if (this.m_PinState == SocketAsyncEventArgs.PinState.MultipleBuffer)
				{
					this.FreeOverlapped(false);
					return;
				}
			}
			else if (this.m_PinState != SocketAsyncEventArgs.PinState.MultipleBuffer || this.m_BufferListChanged)
			{
				this.m_BufferListChanged = false;
				this.FreeOverlapped(false);
				try
				{
					this.SetupOverlappedMultiple();
				}
				catch (Exception)
				{
					this.FreeOverlapped(false);
					throw;
				}
			}
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x0009FC3C File Offset: 0x0009DE3C
		private void CheckPinSendPackets()
		{
			if (this.m_PinState != SocketAsyncEventArgs.PinState.None)
			{
				this.FreeOverlapped(false);
			}
			this.SetupOverlappedSendPackets();
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x0009FC54 File Offset: 0x0009DE54
		private void PinSocketAddressBuffer()
		{
			if (this.m_PinnedSocketAddress == this.m_SocketAddress)
			{
				return;
			}
			if (this.m_SocketAddressGCHandle.IsAllocated)
			{
				this.m_SocketAddressGCHandle.Free();
			}
			this.m_SocketAddressGCHandle = GCHandle.Alloc(this.m_SocketAddress.m_Buffer, GCHandleType.Pinned);
			this.m_SocketAddress.CopyAddressSizeIntoBuffer();
			this.m_PtrSocketAddressBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, 0);
			this.m_PtrSocketAddressBufferSize = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, this.m_SocketAddress.GetAddressSizeOffset());
			this.m_PinnedSocketAddress = this.m_SocketAddress;
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x0009FCF0 File Offset: 0x0009DEF0
		private void FreeOverlapped(bool checkForShutdown)
		{
			if (!checkForShutdown || !NclUtilities.HasShutdownStarted)
			{
				if (this.m_PtrNativeOverlapped != null && !this.m_PtrNativeOverlapped.IsInvalid)
				{
					this.m_PtrNativeOverlapped.Dispose();
					this.m_PtrNativeOverlapped = null;
					this.m_Overlapped = null;
					this.m_PinState = SocketAsyncEventArgs.PinState.None;
					this.m_PinnedAcceptBuffer = null;
					this.m_PinnedSingleBuffer = null;
					this.m_PinnedSingleBufferOffset = 0;
					this.m_PinnedSingleBufferCount = 0;
				}
				if (this.m_SocketAddressGCHandle.IsAllocated)
				{
					this.m_SocketAddressGCHandle.Free();
				}
				if (this.m_WSAMessageBufferGCHandle.IsAllocated)
				{
					this.m_WSAMessageBufferGCHandle.Free();
				}
				if (this.m_WSARecvMsgWSABufferArrayGCHandle.IsAllocated)
				{
					this.m_WSARecvMsgWSABufferArrayGCHandle.Free();
				}
				if (this.m_ControlBufferGCHandle.IsAllocated)
				{
					this.m_ControlBufferGCHandle.Free();
				}
			}
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x0009FDBC File Offset: 0x0009DFBC
		private void SetupOverlappedSingle(bool pinSingleBuffer)
		{
			this.m_Overlapped = new Overlapped();
			if (!pinSingleBuffer)
			{
				this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), this.m_AcceptBuffer));
				this.m_PinnedAcceptBuffer = this.m_AcceptBuffer;
				this.m_PtrAcceptBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_AcceptBuffer, 0);
				this.m_PtrSingleBuffer = IntPtr.Zero;
				this.m_PinState = SocketAsyncEventArgs.PinState.SingleAcceptBuffer;
				return;
			}
			if (this.m_Buffer != null)
			{
				this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), this.m_Buffer));
				this.m_PinnedSingleBuffer = this.m_Buffer;
				this.m_PinnedSingleBufferOffset = this.m_Offset;
				this.m_PinnedSingleBufferCount = this.m_Count;
				this.m_PtrSingleBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_Buffer, this.m_Offset);
				this.m_PtrAcceptBuffer = IntPtr.Zero;
				this.m_WSABuffer.Pointer = this.m_PtrSingleBuffer;
				this.m_WSABuffer.Length = this.m_Count;
				this.m_PinState = SocketAsyncEventArgs.PinState.SingleBuffer;
				return;
			}
			this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), null));
			this.m_PinnedSingleBuffer = null;
			this.m_PinnedSingleBufferOffset = 0;
			this.m_PinnedSingleBufferCount = 0;
			this.m_PtrSingleBuffer = IntPtr.Zero;
			this.m_PtrAcceptBuffer = IntPtr.Zero;
			this.m_WSABuffer.Pointer = this.m_PtrSingleBuffer;
			this.m_WSABuffer.Length = this.m_Count;
			this.m_PinState = SocketAsyncEventArgs.PinState.NoBuffer;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x0009FF50 File Offset: 0x0009E150
		private void SetupOverlappedMultiple()
		{
			ArraySegment<byte>[] array = new ArraySegment<byte>[this.m_BufferList.Count];
			this.m_BufferList.CopyTo(array, 0);
			this.m_Overlapped = new Overlapped();
			if (this.m_ObjectsToPin == null || this.m_ObjectsToPin.Length != array.Length)
			{
				this.m_ObjectsToPin = new object[array.Length];
			}
			bool flag = false;
			if (this.m_WSABufferArray == null || this.m_WSABufferArray.Length != array.Length)
			{
				flag = true;
			}
			for (int i = 0; i < array.Length; i++)
			{
				this.m_ObjectsToPin[i] = array[i].Array;
				if (!flag && array[i].Count != this.m_WSABufferArray[i].Length)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.m_WSABufferArray = new WSABuffer[array.Length];
			}
			this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), this.m_ObjectsToPin));
			for (int j = 0; j < array.Length; j++)
			{
				ArraySegment<byte> arraySegment = array[j];
				ValidationHelper.ValidateSegment(arraySegment);
				this.m_WSABufferArray[j].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(arraySegment.Array, arraySegment.Offset);
				this.m_WSABufferArray[j].Length = arraySegment.Count;
			}
			this.m_PinState = SocketAsyncEventArgs.PinState.MultipleBuffer;
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000A00A4 File Offset: 0x0009E2A4
		private void SetupOverlappedSendPackets()
		{
			this.m_Overlapped = new Overlapped();
			this.m_SendPacketsDescriptor = new UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElement[this.m_SendPacketsElementsFileCount + this.m_SendPacketsElementsBufferCount];
			if (this.m_ObjectsToPin == null || this.m_ObjectsToPin.Length != this.m_SendPacketsElementsBufferCount + 1)
			{
				this.m_ObjectsToPin = new object[this.m_SendPacketsElementsBufferCount + 1];
			}
			this.m_ObjectsToPin[0] = this.m_SendPacketsDescriptor;
			int num = 1;
			foreach (SendPacketsElement sendPacketsElement in this.m_SendPacketsElementsInternal)
			{
				if (sendPacketsElement != null && sendPacketsElement.m_Buffer != null && sendPacketsElement.m_Count > 0)
				{
					this.m_ObjectsToPin[num] = sendPacketsElement.m_Buffer;
					num++;
				}
			}
			this.m_PtrNativeOverlapped = new SafeNativeOverlapped(this.m_Overlapped.UnsafePack(new IOCompletionCallback(this.CompletionPortCallback), this.m_ObjectsToPin));
			this.m_PtrSendPacketsDescriptor = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SendPacketsDescriptor, 0);
			int num2 = 0;
			int num3 = 0;
			foreach (SendPacketsElement sendPacketsElement2 in this.m_SendPacketsElementsInternal)
			{
				if (sendPacketsElement2 != null)
				{
					if (sendPacketsElement2.m_Buffer != null && sendPacketsElement2.m_Count > 0)
					{
						this.m_SendPacketsDescriptor[num2].buffer = Marshal.UnsafeAddrOfPinnedArrayElement(sendPacketsElement2.m_Buffer, sendPacketsElement2.m_Offset);
						this.m_SendPacketsDescriptor[num2].length = (uint)sendPacketsElement2.m_Count;
						this.m_SendPacketsDescriptor[num2].flags = sendPacketsElement2.m_Flags;
						num2++;
					}
					else if (sendPacketsElement2.m_FilePath != null)
					{
						this.m_SendPacketsDescriptor[num2].fileHandle = this.m_SendPacketsFileHandles[num3].DangerousGetHandle();
						this.m_SendPacketsDescriptor[num2].fileOffset = (long)sendPacketsElement2.m_Offset;
						this.m_SendPacketsDescriptor[num2].length = (uint)sendPacketsElement2.m_Count;
						this.m_SendPacketsDescriptor[num2].flags = sendPacketsElement2.m_Flags;
						num3++;
						num2++;
					}
				}
			}
			this.m_PinState = SocketAsyncEventArgs.PinState.SendPackets;
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000A02B4 File Offset: 0x0009E4B4
		internal void LogBuffer(int size)
		{
			switch (this.m_PinState)
			{
			case SocketAsyncEventArgs.PinState.SingleAcceptBuffer:
				Logging.Dump(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation.ToString() + "Async)", this.m_AcceptBuffer, 0, size);
				return;
			case SocketAsyncEventArgs.PinState.SingleBuffer:
				Logging.Dump(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation.ToString() + "Async)", this.m_Buffer, this.m_Offset, size);
				return;
			case SocketAsyncEventArgs.PinState.MultipleBuffer:
				foreach (WSABuffer wsabuffer in this.m_WSABufferArray)
				{
					Logging.Dump(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation.ToString() + "Async)", wsabuffer.Pointer, Math.Min(wsabuffer.Length, size));
					if ((size -= wsabuffer.Length) <= 0)
					{
						break;
					}
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000A03C4 File Offset: 0x0009E5C4
		internal void LogSendPacketsBuffers(int size)
		{
			foreach (SendPacketsElement sendPacketsElement in this.m_SendPacketsElementsInternal)
			{
				if (sendPacketsElement != null)
				{
					if (sendPacketsElement.m_Buffer != null && sendPacketsElement.m_Count > 0)
					{
						Logging.Dump(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation.ToString() + "Async)Buffer", sendPacketsElement.m_Buffer, sendPacketsElement.m_Offset, Math.Min(sendPacketsElement.m_Count, size));
					}
					else if (sendPacketsElement.m_FilePath != null)
					{
						Logging.PrintInfo(Logging.Sockets, this.m_CurrentSocket, "FinishOperation(" + this.m_CompletedOperation.ToString() + "Async)", SR.GetString("net_log_socket_not_logged_file", new object[] { sendPacketsElement.m_FilePath }));
					}
				}
			}
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000A04A4 File Offset: 0x0009E6A4
		internal void UpdatePerfCounters(int size, bool sendOp)
		{
			if (sendOp)
			{
				NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesSent, (long)size);
				if (this.m_CurrentSocket.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsSent);
					return;
				}
			}
			else
			{
				NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesReceived, (long)size);
				if (this.m_CurrentSocket.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsReceived);
				}
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000A0501 File Offset: 0x0009E701
		internal void FinishOperationSyncFailure(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(socketError, bytesTransferred, flags);
			if (this.m_CurrentSocket != null)
			{
				this.m_CurrentSocket.UpdateStatusAfterSocketError(socketError);
			}
			this.Complete();
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000A0526 File Offset: 0x0009E726
		internal void FinishConnectByNameSyncFailure(Exception exception, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(exception, bytesTransferred, flags);
			if (this.m_CurrentSocket != null)
			{
				this.m_CurrentSocket.UpdateStatusAfterSocketError(this.m_SocketError);
			}
			this.Complete();
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000A0550 File Offset: 0x0009E750
		internal void FinishOperationAsyncFailure(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(socketError, bytesTransferred, flags);
			if (this.m_CurrentSocket != null)
			{
				this.m_CurrentSocket.UpdateStatusAfterSocketError(socketError);
			}
			this.Complete();
			if (this.m_Context == null)
			{
				this.OnCompleted(this);
				return;
			}
			ExecutionContext.Run(this.m_ContextCopy, this.m_ExecutionCallback, null);
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000A05A4 File Offset: 0x0009E7A4
		internal void FinishOperationAsyncFailure(Exception exception, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(exception, bytesTransferred, flags);
			if (this.m_CurrentSocket != null)
			{
				this.m_CurrentSocket.UpdateStatusAfterSocketError(this.m_SocketError);
			}
			this.Complete();
			if (this.m_Context == null)
			{
				this.OnCompleted(this);
				return;
			}
			ExecutionContext.Run(this.m_ContextCopy, this.m_ExecutionCallback, null);
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000A05FC File Offset: 0x0009E7FC
		internal void FinishWrapperConnectSuccess(Socket connectSocket, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(SocketError.Success, bytesTransferred, flags);
			this.m_CurrentSocket = connectSocket;
			this.m_ConnectSocket = connectSocket;
			this.Complete();
			if (this.m_ContextCopy == null)
			{
				this.OnCompleted(this);
				return;
			}
			ExecutionContext.Run(this.m_ContextCopy, this.m_ExecutionCallback, null);
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000A0648 File Offset: 0x0009E848
		internal unsafe void FinishOperationSuccess(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(socketError, bytesTransferred, flags);
			SocketAddress socketAddress2;
			switch (this.m_CompletedOperation)
			{
			case SocketAsyncOperation.Accept:
			{
				if (bytesTransferred > 0)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						this.LogBuffer(bytesTransferred);
					}
					if (Socket.s_PerfCountersEnabled)
					{
						this.UpdatePerfCounters(bytesTransferred, false);
					}
				}
				SocketAddress socketAddress = this.m_CurrentSocket.m_RightEndPoint.Serialize();
				try
				{
					IntPtr intPtr;
					int num;
					IntPtr intPtr2;
					this.m_CurrentSocket.GetAcceptExSockaddrs((this.m_PtrSingleBuffer != IntPtr.Zero) ? this.m_PtrSingleBuffer : this.m_PtrAcceptBuffer, (this.m_Count != 0) ? (this.m_Count - this.m_AcceptAddressBufferCount) : 0, this.m_AcceptAddressBufferCount / 2, this.m_AcceptAddressBufferCount / 2, out intPtr, out num, out intPtr2, out socketAddress.m_Size);
					Marshal.Copy(intPtr2, socketAddress.m_Buffer, 0, socketAddress.m_Size);
					IntPtr intPtr3 = this.m_CurrentSocket.SafeHandle.DangerousGetHandle();
					socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_AcceptSocket.SafeHandle, SocketOptionLevel.Socket, SocketOptionName.UpdateAcceptContext, ref intPtr3, Marshal.SizeOf(intPtr3));
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
				}
				catch (ObjectDisposedException)
				{
					socketError = SocketError.OperationAborted;
				}
				if (socketError != SocketError.Success)
				{
					this.SetResults(socketError, bytesTransferred, SocketFlags.None);
					this.m_AcceptSocket = null;
					goto IL_0593;
				}
				this.m_AcceptSocket = this.m_CurrentSocket.UpdateAcceptSocket(this.m_AcceptSocket, this.m_CurrentSocket.m_RightEndPoint.Create(socketAddress), false);
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					Logging.PrintInfo(Logging.Sockets, this.m_AcceptSocket, SR.GetString("net_log_socket_accepted", new object[]
					{
						this.m_AcceptSocket.RemoteEndPoint,
						this.m_AcceptSocket.LocalEndPoint
					}));
					goto IL_0593;
				}
				goto IL_0593;
			}
			case SocketAsyncOperation.Connect:
				if (bytesTransferred > 0)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						this.LogBuffer(bytesTransferred);
					}
					if (Socket.s_PerfCountersEnabled)
					{
						this.UpdatePerfCounters(bytesTransferred, true);
					}
				}
				try
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_CurrentSocket.SafeHandle, SocketOptionLevel.Socket, SocketOptionName.UpdateConnectContext, null, 0);
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
				}
				catch (ObjectDisposedException)
				{
					socketError = SocketError.OperationAborted;
				}
				if (socketError == SocketError.Success)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						Logging.PrintInfo(Logging.Sockets, this.m_CurrentSocket, SR.GetString("net_log_socket_connected", new object[]
						{
							this.m_CurrentSocket.LocalEndPoint,
							this.m_CurrentSocket.RemoteEndPoint
						}));
					}
					this.m_CurrentSocket.SetToConnected();
					this.m_ConnectSocket = this.m_CurrentSocket;
					goto IL_0593;
				}
				goto IL_0593;
			case SocketAsyncOperation.Disconnect:
				this.m_CurrentSocket.SetToDisconnected();
				this.m_CurrentSocket.m_RemoteEndPoint = null;
				goto IL_0593;
			case SocketAsyncOperation.Receive:
				if (bytesTransferred <= 0)
				{
					goto IL_0593;
				}
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					this.LogBuffer(bytesTransferred);
				}
				if (Socket.s_PerfCountersEnabled)
				{
					this.UpdatePerfCounters(bytesTransferred, false);
					goto IL_0593;
				}
				goto IL_0593;
			case SocketAsyncOperation.ReceiveFrom:
				if (bytesTransferred > 0)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						this.LogBuffer(bytesTransferred);
					}
					if (Socket.s_PerfCountersEnabled)
					{
						this.UpdatePerfCounters(bytesTransferred, false);
					}
				}
				this.m_SocketAddress.SetSize(this.m_PtrSocketAddressBufferSize);
				socketAddress2 = this.m_RemoteEndPoint.Serialize();
				if (socketAddress2.Equals(this.m_SocketAddress))
				{
					goto IL_0593;
				}
				try
				{
					this.m_RemoteEndPoint = this.m_RemoteEndPoint.Create(this.m_SocketAddress);
					goto IL_0593;
				}
				catch
				{
					goto IL_0593;
				}
				break;
			case SocketAsyncOperation.ReceiveMessageFrom:
				break;
			case SocketAsyncOperation.Send:
				if (bytesTransferred <= 0)
				{
					goto IL_0593;
				}
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					this.LogBuffer(bytesTransferred);
				}
				if (Socket.s_PerfCountersEnabled)
				{
					this.UpdatePerfCounters(bytesTransferred, true);
					goto IL_0593;
				}
				goto IL_0593;
			case SocketAsyncOperation.SendPackets:
				if (bytesTransferred > 0)
				{
					if (SocketAsyncEventArgs.s_LoggingEnabled)
					{
						this.LogSendPacketsBuffers(bytesTransferred);
					}
					if (Socket.s_PerfCountersEnabled)
					{
						this.UpdatePerfCounters(bytesTransferred, true);
					}
				}
				if (this.m_SendPacketsFileStreams != null)
				{
					for (int i = 0; i < this.m_SendPacketsElementsFileCount; i++)
					{
						this.m_SendPacketsFileHandles[i] = null;
						if (this.m_SendPacketsFileStreams[i] != null)
						{
							this.m_SendPacketsFileStreams[i].Close();
							this.m_SendPacketsFileStreams[i] = null;
						}
					}
				}
				this.m_SendPacketsFileStreams = null;
				this.m_SendPacketsFileHandles = null;
				goto IL_0593;
			case SocketAsyncOperation.SendTo:
				if (bytesTransferred <= 0)
				{
					goto IL_0593;
				}
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					this.LogBuffer(bytesTransferred);
				}
				if (Socket.s_PerfCountersEnabled)
				{
					this.UpdatePerfCounters(bytesTransferred, true);
					goto IL_0593;
				}
				goto IL_0593;
			default:
				goto IL_0593;
			}
			if (bytesTransferred > 0)
			{
				if (SocketAsyncEventArgs.s_LoggingEnabled)
				{
					this.LogBuffer(bytesTransferred);
				}
				if (Socket.s_PerfCountersEnabled)
				{
					this.UpdatePerfCounters(bytesTransferred, false);
				}
			}
			this.m_SocketAddress.SetSize(this.m_PtrSocketAddressBufferSize);
			socketAddress2 = this.m_RemoteEndPoint.Serialize();
			if (!socketAddress2.Equals(this.m_SocketAddress))
			{
				try
				{
					this.m_RemoteEndPoint = this.m_RemoteEndPoint.Create(this.m_SocketAddress);
				}
				catch
				{
				}
			}
			IPAddress ipaddress = null;
			UnsafeNclNativeMethods.OSSOCK.WSAMsg* ptr = (UnsafeNclNativeMethods.OSSOCK.WSAMsg*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSAMessageBuffer, 0);
			if (this.m_ControlBuffer.Length == SocketAsyncEventArgs.s_ControlDataSize)
			{
				UnsafeNclNativeMethods.OSSOCK.ControlData controlData = (UnsafeNclNativeMethods.OSSOCK.ControlData)Marshal.PtrToStructure(ptr->controlBuffer.Pointer, typeof(UnsafeNclNativeMethods.OSSOCK.ControlData));
				if (controlData.length != UIntPtr.Zero)
				{
					ipaddress = new IPAddress((long)((ulong)controlData.address));
				}
				this.m_ReceiveMessageFromPacketInfo = new IPPacketInformation((ipaddress != null) ? ipaddress : IPAddress.None, (int)controlData.index);
			}
			else if (this.m_ControlBuffer.Length == SocketAsyncEventArgs.s_ControlDataIPv6Size)
			{
				UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6 controlDataIPv = (UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6)Marshal.PtrToStructure(ptr->controlBuffer.Pointer, typeof(UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6));
				if (controlDataIPv.length != UIntPtr.Zero)
				{
					ipaddress = new IPAddress(controlDataIPv.address);
				}
				this.m_ReceiveMessageFromPacketInfo = new IPPacketInformation((ipaddress != null) ? ipaddress : IPAddress.IPv6None, (int)controlDataIPv.index);
			}
			else
			{
				this.m_ReceiveMessageFromPacketInfo = default(IPPacketInformation);
			}
			IL_0593:
			if (socketError != SocketError.Success)
			{
				this.SetResults(socketError, bytesTransferred, flags);
				this.m_CurrentSocket.UpdateStatusAfterSocketError(socketError);
			}
			this.Complete();
			if (this.m_ContextCopy == null)
			{
				this.OnCompleted(this);
				return;
			}
			ExecutionContext.Run(this.m_ContextCopy, this.m_ExecutionCallback, null);
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x000A0C5C File Offset: 0x0009EE5C
		private unsafe void CompletionPortCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			SocketFlags socketFlags = SocketFlags.None;
			SocketError socketError = (SocketError)errorCode;
			if (socketError == SocketError.Success)
			{
				this.FinishOperationSuccess(socketError, (int)numBytes, socketFlags);
				return;
			}
			if (socketError != SocketError.OperationAborted)
			{
				if (this.m_CurrentSocket.CleanedUp)
				{
					socketError = SocketError.OperationAborted;
				}
				else
				{
					try
					{
						bool flag = UnsafeNclNativeMethods.OSSOCK.WSAGetOverlappedResult(this.m_CurrentSocket.SafeHandle, this.m_PtrNativeOverlapped, out numBytes, false, out socketFlags);
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
					catch
					{
						socketError = SocketError.OperationAborted;
					}
				}
			}
			this.FinishOperationAsyncFailure(socketError, (int)numBytes, socketFlags);
		}

		// Token: 0x04001E6D RID: 7789
		internal static readonly int s_ControlDataSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.ControlData));

		// Token: 0x04001E6E RID: 7790
		internal static readonly int s_ControlDataIPv6Size = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6));

		// Token: 0x04001E6F RID: 7791
		internal static readonly int s_WSAMsgSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.WSAMsg));

		// Token: 0x04001E70 RID: 7792
		internal Socket m_AcceptSocket;

		// Token: 0x04001E71 RID: 7793
		private Socket m_ConnectSocket;

		// Token: 0x04001E72 RID: 7794
		internal byte[] m_Buffer;

		// Token: 0x04001E73 RID: 7795
		internal WSABuffer m_WSABuffer;

		// Token: 0x04001E74 RID: 7796
		internal IntPtr m_PtrSingleBuffer;

		// Token: 0x04001E75 RID: 7797
		internal int m_Count;

		// Token: 0x04001E76 RID: 7798
		internal int m_Offset;

		// Token: 0x04001E77 RID: 7799
		internal IList<ArraySegment<byte>> m_BufferList;

		// Token: 0x04001E78 RID: 7800
		private bool m_BufferListChanged;

		// Token: 0x04001E79 RID: 7801
		internal WSABuffer[] m_WSABufferArray;

		// Token: 0x04001E7A RID: 7802
		private int m_BytesTransferred;

		// Token: 0x04001E7C RID: 7804
		private bool m_CompletedChanged;

		// Token: 0x04001E7D RID: 7805
		private bool m_DisconnectReuseSocket;

		// Token: 0x04001E7E RID: 7806
		private SocketAsyncOperation m_CompletedOperation;

		// Token: 0x04001E7F RID: 7807
		private IPPacketInformation m_ReceiveMessageFromPacketInfo;

		// Token: 0x04001E80 RID: 7808
		private EndPoint m_RemoteEndPoint;

		// Token: 0x04001E81 RID: 7809
		internal TransmitFileOptions m_SendPacketsFlags;

		// Token: 0x04001E82 RID: 7810
		internal int m_SendPacketsSendSize;

		// Token: 0x04001E83 RID: 7811
		internal SendPacketsElement[] m_SendPacketsElements;

		// Token: 0x04001E84 RID: 7812
		private SendPacketsElement[] m_SendPacketsElementsInternal;

		// Token: 0x04001E85 RID: 7813
		internal int m_SendPacketsElementsFileCount;

		// Token: 0x04001E86 RID: 7814
		internal int m_SendPacketsElementsBufferCount;

		// Token: 0x04001E87 RID: 7815
		private SocketError m_SocketError;

		// Token: 0x04001E88 RID: 7816
		private Exception m_ConnectByNameError;

		// Token: 0x04001E89 RID: 7817
		internal SocketFlags m_SocketFlags;

		// Token: 0x04001E8A RID: 7818
		private object m_UserToken;

		// Token: 0x04001E8B RID: 7819
		internal byte[] m_AcceptBuffer;

		// Token: 0x04001E8C RID: 7820
		internal int m_AcceptAddressBufferCount;

		// Token: 0x04001E8D RID: 7821
		internal IntPtr m_PtrAcceptBuffer;

		// Token: 0x04001E8E RID: 7822
		internal SocketAddress m_SocketAddress;

		// Token: 0x04001E8F RID: 7823
		private GCHandle m_SocketAddressGCHandle;

		// Token: 0x04001E90 RID: 7824
		private SocketAddress m_PinnedSocketAddress;

		// Token: 0x04001E91 RID: 7825
		internal IntPtr m_PtrSocketAddressBuffer;

		// Token: 0x04001E92 RID: 7826
		internal IntPtr m_PtrSocketAddressBufferSize;

		// Token: 0x04001E93 RID: 7827
		private byte[] m_WSAMessageBuffer;

		// Token: 0x04001E94 RID: 7828
		private GCHandle m_WSAMessageBufferGCHandle;

		// Token: 0x04001E95 RID: 7829
		internal IntPtr m_PtrWSAMessageBuffer;

		// Token: 0x04001E96 RID: 7830
		private byte[] m_ControlBuffer;

		// Token: 0x04001E97 RID: 7831
		private GCHandle m_ControlBufferGCHandle;

		// Token: 0x04001E98 RID: 7832
		internal IntPtr m_PtrControlBuffer;

		// Token: 0x04001E99 RID: 7833
		private WSABuffer[] m_WSARecvMsgWSABufferArray;

		// Token: 0x04001E9A RID: 7834
		private GCHandle m_WSARecvMsgWSABufferArrayGCHandle;

		// Token: 0x04001E9B RID: 7835
		private IntPtr m_PtrWSARecvMsgWSABufferArray;

		// Token: 0x04001E9C RID: 7836
		internal FileStream[] m_SendPacketsFileStreams;

		// Token: 0x04001E9D RID: 7837
		internal SafeHandle[] m_SendPacketsFileHandles;

		// Token: 0x04001E9E RID: 7838
		internal UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElement[] m_SendPacketsDescriptor;

		// Token: 0x04001E9F RID: 7839
		internal IntPtr m_PtrSendPacketsDescriptor;

		// Token: 0x04001EA0 RID: 7840
		private ExecutionContext m_Context;

		// Token: 0x04001EA1 RID: 7841
		private ExecutionContext m_ContextCopy;

		// Token: 0x04001EA2 RID: 7842
		private ContextCallback m_ExecutionCallback;

		// Token: 0x04001EA3 RID: 7843
		private Socket m_CurrentSocket;

		// Token: 0x04001EA4 RID: 7844
		private bool m_DisposeCalled;

		// Token: 0x04001EA5 RID: 7845
		private const int Configuring = -1;

		// Token: 0x04001EA6 RID: 7846
		private const int Free = 0;

		// Token: 0x04001EA7 RID: 7847
		private const int InProgress = 1;

		// Token: 0x04001EA8 RID: 7848
		private const int Disposed = 2;

		// Token: 0x04001EA9 RID: 7849
		private int m_Operating;

		// Token: 0x04001EAA RID: 7850
		internal SafeNativeOverlapped m_PtrNativeOverlapped;

		// Token: 0x04001EAB RID: 7851
		private Overlapped m_Overlapped;

		// Token: 0x04001EAC RID: 7852
		private object[] m_ObjectsToPin;

		// Token: 0x04001EAD RID: 7853
		private SocketAsyncEventArgs.PinState m_PinState;

		// Token: 0x04001EAE RID: 7854
		private byte[] m_PinnedAcceptBuffer;

		// Token: 0x04001EAF RID: 7855
		private byte[] m_PinnedSingleBuffer;

		// Token: 0x04001EB0 RID: 7856
		private int m_PinnedSingleBufferOffset;

		// Token: 0x04001EB1 RID: 7857
		private int m_PinnedSingleBufferCount;

		// Token: 0x04001EB2 RID: 7858
		private MultipleConnectAsync m_MultipleConnect;

		// Token: 0x04001EB3 RID: 7859
		private static bool s_LoggingEnabled = Logging.On;

		// Token: 0x020007DF RID: 2015
		private enum PinState
		{
			// Token: 0x040034C7 RID: 13511
			None,
			// Token: 0x040034C8 RID: 13512
			NoBuffer,
			// Token: 0x040034C9 RID: 13513
			SingleAcceptBuffer,
			// Token: 0x040034CA RID: 13514
			SingleBuffer,
			// Token: 0x040034CB RID: 13515
			MultipleBuffer,
			// Token: 0x040034CC RID: 13516
			SendPackets
		}
	}
}
