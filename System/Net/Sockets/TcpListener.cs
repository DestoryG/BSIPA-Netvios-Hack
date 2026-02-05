using System;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
	// Token: 0x02000387 RID: 903
	public class TcpListener
	{
		// Token: 0x060021BA RID: 8634 RVA: 0x000A18A4 File Offset: 0x0009FAA4
		public TcpListener(IPEndPoint localEP)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpListener", localEP);
			}
			if (localEP == null)
			{
				throw new ArgumentNullException("localEP");
			}
			this.m_ServerSocketEP = localEP;
			this.m_ServerSocket = new Socket(this.m_ServerSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpListener", null);
			}
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000A1914 File Offset: 0x0009FB14
		public TcpListener(IPAddress localaddr, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpListener", localaddr);
			}
			if (localaddr == null)
			{
				throw new ArgumentNullException("localaddr");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_ServerSocketEP = new IPEndPoint(localaddr, port);
			this.m_ServerSocket = new Socket(this.m_ServerSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpListener", null);
			}
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000A19A0 File Offset: 0x0009FBA0
		[Obsolete("This method has been deprecated. Please use TcpListener(IPAddress localaddr, int port) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public TcpListener(int port)
		{
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_ServerSocketEP = new IPEndPoint(IPAddress.Any, port);
			this.m_ServerSocket = new Socket(this.m_ServerSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000A19F0 File Offset: 0x0009FBF0
		public static TcpListener Create(int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, "TcpListener.Create", "Port: " + port.ToString());
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			TcpListener tcpListener = new TcpListener(IPAddress.IPv6Any, port);
			tcpListener.Server.DualMode = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, "TcpListener.Create", "Port: " + port.ToString());
			}
			return tcpListener;
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060021BE RID: 8638 RVA: 0x000A1A77 File Offset: 0x0009FC77
		public Socket Server
		{
			get
			{
				return this.m_ServerSocket;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x000A1A7F File Offset: 0x0009FC7F
		protected bool Active
		{
			get
			{
				return this.m_Active;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x000A1A87 File Offset: 0x0009FC87
		public EndPoint LocalEndpoint
		{
			get
			{
				if (!this.m_Active)
				{
					return this.m_ServerSocketEP;
				}
				return this.m_ServerSocket.LocalEndPoint;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x000A1AA3 File Offset: 0x0009FCA3
		// (set) Token: 0x060021C2 RID: 8642 RVA: 0x000A1AB0 File Offset: 0x0009FCB0
		public bool ExclusiveAddressUse
		{
			get
			{
				return this.m_ServerSocket.ExclusiveAddressUse;
			}
			set
			{
				if (this.m_Active)
				{
					throw new InvalidOperationException(SR.GetString("net_tcplistener_mustbestopped"));
				}
				this.m_ServerSocket.ExclusiveAddressUse = value;
				this.m_ExclusiveAddressUse = value;
			}
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000A1ADD File Offset: 0x0009FCDD
		public void AllowNatTraversal(bool allowed)
		{
			if (this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_tcplistener_mustbestopped"));
			}
			if (allowed)
			{
				this.m_ServerSocket.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);
				return;
			}
			this.m_ServerSocket.SetIPProtectionLevel(IPProtectionLevel.EdgeRestricted);
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x000A1B15 File Offset: 0x0009FD15
		public void Start()
		{
			this.Start(int.MaxValue);
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x000A1B24 File Offset: 0x0009FD24
		public void Start(int backlog)
		{
			if (backlog > 2147483647 || backlog < 0)
			{
				throw new ArgumentOutOfRangeException("backlog");
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Start", null);
			}
			if (this.m_ServerSocket == null)
			{
				throw new InvalidOperationException(SR.GetString("net_InvalidSocketHandle"));
			}
			if (this.m_Active)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Sockets, this, "Start", null);
				}
				return;
			}
			this.m_ServerSocket.Bind(this.m_ServerSocketEP);
			try
			{
				this.m_ServerSocket.Listen(backlog);
			}
			catch (SocketException)
			{
				this.Stop();
				throw;
			}
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Start", null);
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000A1BF0 File Offset: 0x0009FDF0
		public void Stop()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Stop", null);
			}
			if (this.m_ServerSocket != null)
			{
				this.m_ServerSocket.Close();
				this.m_ServerSocket = null;
			}
			this.m_Active = false;
			this.m_ServerSocket = new Socket(this.m_ServerSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (this.m_ExclusiveAddressUse)
			{
				this.m_ServerSocket.ExclusiveAddressUse = true;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Stop", null);
			}
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x000A1C7A File Offset: 0x0009FE7A
		public bool Pending()
		{
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			return this.m_ServerSocket.Poll(0, SelectMode.SelectRead);
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000A1CA4 File Offset: 0x0009FEA4
		public Socket AcceptSocket()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "AcceptSocket", null);
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			Socket socket = this.m_ServerSocket.Accept();
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "AcceptSocket", socket);
			}
			return socket;
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000A1D08 File Offset: 0x0009FF08
		public TcpClient AcceptTcpClient()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "AcceptTcpClient", null);
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			Socket socket = this.m_ServerSocket.Accept();
			TcpClient tcpClient = new TcpClient(socket);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "AcceptTcpClient", tcpClient);
			}
			return tcpClient;
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000A1D74 File Offset: 0x0009FF74
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginAcceptSocket", null);
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			IAsyncResult asyncResult = this.m_ServerSocket.BeginAccept(callback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginAcceptSocket", null);
			}
			return asyncResult;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000A1DD8 File Offset: 0x0009FFD8
		public Socket EndAcceptSocket(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "EndAcceptSocket", null);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			Socket socket = ((lazyAsyncResult == null) ? null : (lazyAsyncResult.AsyncObject as Socket));
			if (socket == null)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			Socket socket2 = socket.EndAccept(asyncResult);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "EndAcceptSocket", socket2);
			}
			return socket2;
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000A1E60 File Offset: 0x000A0060
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginAcceptTcpClient(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginAcceptTcpClient", null);
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			IAsyncResult asyncResult = this.m_ServerSocket.BeginAccept(callback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginAcceptTcpClient", null);
			}
			return asyncResult;
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x000A1EC4 File Offset: 0x000A00C4
		public TcpClient EndAcceptTcpClient(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "EndAcceptTcpClient", null);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			Socket socket = ((lazyAsyncResult == null) ? null : (lazyAsyncResult.AsyncObject as Socket));
			if (socket == null)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			Socket socket2 = socket.EndAccept(asyncResult);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "EndAcceptTcpClient", socket2);
			}
			return new TcpClient(socket2);
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x000A1F4E File Offset: 0x000A014E
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<Socket> AcceptSocketAsync()
		{
			return Task<Socket>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginAcceptSocket), new Func<IAsyncResult, Socket>(this.EndAcceptSocket), null);
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x000A1F73 File Offset: 0x000A0173
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<TcpClient> AcceptTcpClientAsync()
		{
			return Task<TcpClient>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginAcceptTcpClient), new Func<IAsyncResult, TcpClient>(this.EndAcceptTcpClient), null);
		}

		// Token: 0x04001F3B RID: 7995
		private IPEndPoint m_ServerSocketEP;

		// Token: 0x04001F3C RID: 7996
		private Socket m_ServerSocket;

		// Token: 0x04001F3D RID: 7997
		private bool m_Active;

		// Token: 0x04001F3E RID: 7998
		private bool m_ExclusiveAddressUse;
	}
}
