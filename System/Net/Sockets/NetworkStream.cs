using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
	// Token: 0x02000370 RID: 880
	public class NetworkStream : Stream
	{
		// Token: 0x06001FE0 RID: 8160 RVA: 0x00094F30 File Offset: 0x00093130
		internal NetworkStream()
		{
			this.m_OwnsSocket = true;
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00094F54 File Offset: 0x00093154
		public NetworkStream(Socket socket)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this.InitNetworkStream(socket, FileAccess.ReadWrite);
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x00094F87 File Offset: 0x00093187
		public NetworkStream(Socket socket, bool ownsSocket)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this.InitNetworkStream(socket, FileAccess.ReadWrite);
			this.m_OwnsSocket = ownsSocket;
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x00094FC4 File Offset: 0x000931C4
		internal NetworkStream(NetworkStream networkStream, bool ownsSocket)
		{
			Socket socket = networkStream.Socket;
			if (socket == null)
			{
				throw new ArgumentNullException("networkStream");
			}
			this.InitNetworkStream(socket, FileAccess.ReadWrite);
			this.m_OwnsSocket = ownsSocket;
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x00095010 File Offset: 0x00093210
		public NetworkStream(Socket socket, FileAccess access)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this.InitNetworkStream(socket, access);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x00095043 File Offset: 0x00093243
		public NetworkStream(Socket socket, FileAccess access, bool ownsSocket)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this.InitNetworkStream(socket, access);
			this.m_OwnsSocket = ownsSocket;
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06001FE6 RID: 8166 RVA: 0x0009507D File Offset: 0x0009327D
		protected Socket Socket
		{
			get
			{
				return this.m_StreamSocket;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x00095088 File Offset: 0x00093288
		internal Socket InternalSocket
		{
			get
			{
				Socket streamSocket = this.m_StreamSocket;
				if (this.m_CleanedUp || streamSocket == null)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				return streamSocket;
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000950BC File Offset: 0x000932BC
		internal void InternalAbortSocket()
		{
			if (!this.m_OwnsSocket)
			{
				throw new InvalidOperationException();
			}
			Socket streamSocket = this.m_StreamSocket;
			if (this.m_CleanedUp || streamSocket == null)
			{
				return;
			}
			try
			{
				streamSocket.Close(0);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x0009510C File Offset: 0x0009330C
		internal void ConvertToNotSocketOwner()
		{
			this.m_OwnsSocket = false;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06001FEA RID: 8170 RVA: 0x0009511B File Offset: 0x0009331B
		// (set) Token: 0x06001FEB RID: 8171 RVA: 0x00095123 File Offset: 0x00093323
		protected bool Readable
		{
			get
			{
				return this.m_Readable;
			}
			set
			{
				this.m_Readable = value;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06001FEC RID: 8172 RVA: 0x0009512C File Offset: 0x0009332C
		// (set) Token: 0x06001FED RID: 8173 RVA: 0x00095134 File Offset: 0x00093334
		protected bool Writeable
		{
			get
			{
				return this.m_Writeable;
			}
			set
			{
				this.m_Writeable = value;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06001FEE RID: 8174 RVA: 0x0009513D File Offset: 0x0009333D
		public override bool CanRead
		{
			get
			{
				return this.m_Readable;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x00095145 File Offset: 0x00093345
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x00095148 File Offset: 0x00093348
		public override bool CanWrite
		{
			get
			{
				return this.m_Writeable;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x00095150 File Offset: 0x00093350
		public override bool CanTimeout
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06001FF2 RID: 8178 RVA: 0x00095154 File Offset: 0x00093354
		// (set) Token: 0x06001FF3 RID: 8179 RVA: 0x00095182 File Offset: 0x00093382
		public override int ReadTimeout
		{
			get
			{
				int num = (int)this.m_StreamSocket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
				if (num == 0)
				{
					return -1;
				}
				return num;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this.SetSocketTimeoutOption(SocketShutdown.Receive, value, false);
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x000951AC File Offset: 0x000933AC
		// (set) Token: 0x06001FF5 RID: 8181 RVA: 0x000951DA File Offset: 0x000933DA
		public override int WriteTimeout
		{
			get
			{
				int num = (int)this.m_StreamSocket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
				if (num == 0)
				{
					return -1;
				}
				return num;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this.SetSocketTimeoutOption(SocketShutdown.Send, value, false);
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x00095204 File Offset: 0x00093404
		public virtual bool DataAvailable
		{
			get
			{
				if (this.m_CleanedUp)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				Socket streamSocket = this.m_StreamSocket;
				if (streamSocket == null)
				{
					throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
				}
				return streamSocket.Available != 0;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x00095262 File Offset: 0x00093462
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x00095273 File Offset: 0x00093473
		// (set) Token: 0x06001FF9 RID: 8185 RVA: 0x00095284 File Offset: 0x00093484
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

		// Token: 0x06001FFA RID: 8186 RVA: 0x00095295 File Offset: 0x00093495
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x000952A8 File Offset: 0x000934A8
		internal void InitNetworkStream(Socket socket, FileAccess Access)
		{
			if (!socket.Blocking)
			{
				throw new IOException(SR.GetString("net_sockets_blocking"));
			}
			if (!socket.Connected)
			{
				throw new IOException(SR.GetString("net_notconnected"));
			}
			if (socket.SocketType != SocketType.Stream)
			{
				throw new IOException(SR.GetString("net_notstream"));
			}
			this.m_StreamSocket = socket;
			switch (Access)
			{
			case FileAccess.Read:
				this.m_Readable = true;
				return;
			case FileAccess.Write:
				this.m_Writeable = true;
				return;
			}
			this.m_Readable = true;
			this.m_Writeable = true;
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0009533C File Offset: 0x0009353C
		internal bool PollRead()
		{
			if (this.m_CleanedUp)
			{
				return false;
			}
			Socket streamSocket = this.m_StreamSocket;
			return streamSocket != null && streamSocket.Poll(0, SelectMode.SelectRead);
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0009536C File Offset: 0x0009356C
		internal bool Poll(int microSeconds, SelectMode mode)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			return streamSocket.Poll(microSeconds, mode);
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000953CC File Offset: 0x000935CC
		public override int Read([In] [Out] byte[] buffer, int offset, int size)
		{
			bool canRead = this.CanRead;
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!canRead)
			{
				throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
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
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			int num2;
			try
			{
				int num = streamSocket.Receive(buffer, offset, size, SocketFlags.None);
				num2 = num;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { ex.Message }), ex);
			}
			return num2;
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000954D8 File Offset: 0x000936D8
		public override void Write(byte[] buffer, int offset, int size)
		{
			bool canWrite = this.CanWrite;
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!canWrite)
			{
				throw new InvalidOperationException(SR.GetString("net_readonlystream"));
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
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			try
			{
				streamSocket.Send(buffer, offset, size, SocketFlags.None);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { ex.Message }), ex);
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x000955DC File Offset: 0x000937DC
		public void Close(int timeout)
		{
			if (timeout < -1)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			this.m_CloseTimeout = timeout;
			this.Close();
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000955FC File Offset: 0x000937FC
		protected override void Dispose(bool disposing)
		{
			bool cleanedUp = this.m_CleanedUp;
			this.m_CleanedUp = true;
			if (!cleanedUp && disposing && this.m_StreamSocket != null)
			{
				this.m_Readable = false;
				this.m_Writeable = false;
				if (this.m_OwnsSocket)
				{
					Socket streamSocket = this.m_StreamSocket;
					if (streamSocket != null)
					{
						streamSocket.InternalShutdown(SocketShutdown.Both);
						streamSocket.Close(this.m_CloseTimeout);
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x00095668 File Offset: 0x00093868
		~NetworkStream()
		{
			this.Dispose(false);
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x00095698 File Offset: 0x00093898
		internal bool Connected
		{
			get
			{
				Socket streamSocket = this.m_StreamSocket;
				return !this.m_CleanedUp && streamSocket != null && streamSocket.Connected;
			}
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x000956C4 File Offset: 0x000938C4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			bool canRead = this.CanRead;
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!canRead)
			{
				throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
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
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			IAsyncResult asyncResult2;
			try
			{
				IAsyncResult asyncResult = streamSocket.BeginReceive(buffer, offset, size, SocketFlags.None, callback, state);
				asyncResult2 = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { ex.Message }), ex);
			}
			return asyncResult2;
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x000957D4 File Offset: 0x000939D4
		internal virtual IAsyncResult UnsafeBeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			bool canRead = this.CanRead;
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!canRead)
			{
				throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			IAsyncResult asyncResult2;
			try
			{
				IAsyncResult asyncResult = streamSocket.UnsafeBeginReceive(buffer, offset, size, SocketFlags.None, callback, state);
				asyncResult2 = asyncResult;
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { ex.Message }), ex);
			}
			return asyncResult2;
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00095898 File Offset: 0x00093A98
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			int num2;
			try
			{
				int num = streamSocket.EndReceive(asyncResult);
				num2 = num;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { ex.Message }), ex);
			}
			return num2;
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00095954 File Offset: 0x00093B54
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			bool canWrite = this.CanWrite;
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!canWrite)
			{
				throw new InvalidOperationException(SR.GetString("net_readonlystream"));
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
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			IAsyncResult asyncResult2;
			try
			{
				IAsyncResult asyncResult = streamSocket.BeginSend(buffer, offset, size, SocketFlags.None, callback, state);
				asyncResult2 = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { ex.Message }), ex);
			}
			return asyncResult2;
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x00095A64 File Offset: 0x00093C64
		internal virtual IAsyncResult UnsafeBeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			bool canWrite = this.CanWrite;
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!canWrite)
			{
				throw new InvalidOperationException(SR.GetString("net_readonlystream"));
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			IAsyncResult asyncResult2;
			try
			{
				IAsyncResult asyncResult = streamSocket.UnsafeBeginSend(buffer, offset, size, SocketFlags.None, callback, state);
				asyncResult2 = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { ex.Message }), ex);
			}
			return asyncResult2;
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00095B3C File Offset: 0x00093D3C
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			try
			{
				streamSocket.EndSend(asyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { ex.Message }), ex);
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x00095BF8 File Offset: 0x00093DF8
		internal virtual void MultipleWrite(BufferOffsetSize[] buffers)
		{
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			try
			{
				streamSocket.MultipleSend(buffers, SocketFlags.None);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { ex.Message }), ex);
			}
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x00095C98 File Offset: 0x00093E98
		internal virtual IAsyncResult BeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			IAsyncResult asyncResult2;
			try
			{
				IAsyncResult asyncResult = streamSocket.BeginMultipleSend(buffers, SocketFlags.None, callback, state);
				asyncResult2 = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { ex.Message }), ex);
			}
			return asyncResult2;
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x00095D3C File Offset: 0x00093F3C
		internal virtual IAsyncResult UnsafeBeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			IAsyncResult asyncResult2;
			try
			{
				IAsyncResult asyncResult = streamSocket.UnsafeBeginMultipleSend(buffers, SocketFlags.None, callback, state);
				asyncResult2 = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { ex.Message }), ex);
			}
			return asyncResult2;
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00095DE0 File Offset: 0x00093FE0
		internal virtual void EndMultipleWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			try
			{
				streamSocket.EndMultipleSend(asyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[] { ex.Message }), ex);
			}
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x00095E80 File Offset: 0x00094080
		public override void Flush()
		{
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x00095E82 File Offset: 0x00094082
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x00095E89 File Offset: 0x00094089
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x00095E9C File Offset: 0x0009409C
		internal void SetSocketTimeoutOption(SocketShutdown mode, int timeout, bool silent)
		{
			if (timeout < 0)
			{
				timeout = 0;
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				return;
			}
			if ((mode == SocketShutdown.Send || mode == SocketShutdown.Both) && timeout != this.m_CurrentWriteTimeout)
			{
				streamSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, timeout, silent);
				this.m_CurrentWriteTimeout = timeout;
			}
			if ((mode == SocketShutdown.Receive || mode == SocketShutdown.Both) && timeout != this.m_CurrentReadTimeout)
			{
				streamSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout, silent);
				this.m_CurrentReadTimeout = timeout;
			}
		}

		// Token: 0x04001DDE RID: 7646
		private Socket m_StreamSocket;

		// Token: 0x04001DDF RID: 7647
		private bool m_Readable;

		// Token: 0x04001DE0 RID: 7648
		private bool m_Writeable;

		// Token: 0x04001DE1 RID: 7649
		private bool m_OwnsSocket;

		// Token: 0x04001DE2 RID: 7650
		private int m_CloseTimeout = -1;

		// Token: 0x04001DE3 RID: 7651
		private volatile bool m_CleanedUp;

		// Token: 0x04001DE4 RID: 7652
		private int m_CurrentReadTimeout = -1;

		// Token: 0x04001DE5 RID: 7653
		private int m_CurrentWriteTimeout = -1;
	}
}
