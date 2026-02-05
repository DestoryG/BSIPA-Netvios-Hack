using System;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
	// Token: 0x02000386 RID: 902
	public class TcpClient : IDisposable
	{
		// Token: 0x0600218F RID: 8591 RVA: 0x000A0DB8 File Offset: 0x0009EFB8
		public TcpClient(IPEndPoint localEP)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", localEP);
			}
			if (localEP == null)
			{
				throw new ArgumentNullException("localEP");
			}
			this.m_Family = localEP.AddressFamily;
			this.initialize();
			this.Client.Bind(localEP);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", "");
			}
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000A0E32 File Offset: 0x0009F032
		public TcpClient()
			: this(AddressFamily.InterNetwork)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", null);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", null);
			}
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000A0E6C File Offset: 0x0009F06C
		public TcpClient(AddressFamily family)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", family);
			}
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_family", new object[] { "TCP" }), "family");
			}
			this.m_Family = family;
			this.initialize();
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", null);
			}
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x000A0EF4 File Offset: 0x0009F0F4
		public TcpClient(string hostname, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", hostname);
			}
			if (hostname == null)
			{
				throw new ArgumentNullException("hostname");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			try
			{
				this.Connect(hostname, port);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (this.m_ClientSocket != null)
				{
					this.m_ClientSocket.Close();
				}
				throw ex;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", null);
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x000A0FAC File Offset: 0x0009F1AC
		internal TcpClient(Socket acceptedSocket)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", acceptedSocket);
			}
			this.Client = acceptedSocket;
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", null);
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000A1004 File Offset: 0x0009F204
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x000A100C File Offset: 0x0009F20C
		public Socket Client
		{
			get
			{
				return this.m_ClientSocket;
			}
			set
			{
				this.m_ClientSocket = value;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x000A1015 File Offset: 0x0009F215
		// (set) Token: 0x06002197 RID: 8599 RVA: 0x000A101D File Offset: 0x0009F21D
		protected bool Active
		{
			get
			{
				return this.m_Active;
			}
			set
			{
				this.m_Active = value;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x000A1026 File Offset: 0x0009F226
		public int Available
		{
			get
			{
				return this.m_ClientSocket.Available;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x000A1033 File Offset: 0x0009F233
		public bool Connected
		{
			get
			{
				return this.m_ClientSocket.Connected;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x000A1040 File Offset: 0x0009F240
		// (set) Token: 0x0600219B RID: 8603 RVA: 0x000A104D File Offset: 0x0009F24D
		public bool ExclusiveAddressUse
		{
			get
			{
				return this.m_ClientSocket.ExclusiveAddressUse;
			}
			set
			{
				this.m_ClientSocket.ExclusiveAddressUse = value;
			}
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000A105C File Offset: 0x0009F25C
		public void Connect(string hostname, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", hostname);
			}
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (hostname == null)
			{
				throw new ArgumentNullException("hostname");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (this.m_Active)
			{
				throw new SocketException(SocketError.IsConnected);
			}
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
			Exception ex = null;
			Socket socket = null;
			Socket socket2 = null;
			try
			{
				if (this.m_ClientSocket == null)
				{
					if (Socket.OSSupportsIPv4)
					{
						socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					}
					if (Socket.OSSupportsIPv6)
					{
						socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
					}
				}
				foreach (IPAddress ipaddress in hostAddresses)
				{
					try
					{
						if (this.m_ClientSocket == null)
						{
							if (ipaddress.AddressFamily == AddressFamily.InterNetwork && socket2 != null)
							{
								socket2.Connect(ipaddress, port);
								this.m_ClientSocket = socket2;
								if (socket != null)
								{
									socket.Close();
								}
							}
							else if (socket != null)
							{
								socket.Connect(ipaddress, port);
								this.m_ClientSocket = socket;
								if (socket2 != null)
								{
									socket2.Close();
								}
							}
							this.m_Family = ipaddress.AddressFamily;
							this.m_Active = true;
							break;
						}
						if (ipaddress.AddressFamily == this.m_Family)
						{
							this.Connect(new IPEndPoint(ipaddress, port));
							this.m_Active = true;
							break;
						}
					}
					catch (Exception ex2)
					{
						if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
						{
							throw;
						}
						ex = ex2;
					}
				}
			}
			catch (Exception ex3)
			{
				if (ex3 is ThreadAbortException || ex3 is StackOverflowException || ex3 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex3;
			}
			finally
			{
				if (!this.m_Active)
				{
					if (socket != null)
					{
						socket.Close();
					}
					if (socket2 != null)
					{
						socket2.Close();
					}
					if (ex != null)
					{
						throw ex;
					}
					throw new SocketException(SocketError.NotConnected);
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000A128C File Offset: 0x0009F48C
		public void Connect(IPAddress address, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", address);
			}
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			IPEndPoint ipendPoint = new IPEndPoint(address, port);
			this.Connect(ipendPoint);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000A1314 File Offset: 0x0009F514
		public void Connect(IPEndPoint remoteEP)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", remoteEP);
			}
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			this.Client.Connect(remoteEP);
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000A138C File Offset: 0x0009F58C
		public void Connect(IPAddress[] ipAddresses, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", ipAddresses);
			}
			this.Client.Connect(ipAddresses, port);
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000A13E0 File Offset: 0x0009F5E0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", host);
			}
			IAsyncResult asyncResult = this.Client.BeginConnect(host, port, requestCallback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", null);
			}
			return asyncResult;
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000A1430 File Offset: 0x0009F630
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", address);
			}
			IAsyncResult asyncResult = this.Client.BeginConnect(address, port, requestCallback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", null);
			}
			return asyncResult;
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000A1480 File Offset: 0x0009F680
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", addresses);
			}
			IAsyncResult asyncResult = this.Client.BeginConnect(addresses, port, requestCallback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", null);
			}
			return asyncResult;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000A14D0 File Offset: 0x0009F6D0
		public void EndConnect(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "EndConnect", asyncResult);
			}
			this.Client.EndConnect(asyncResult);
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "EndConnect", null);
			}
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000A1520 File Offset: 0x0009F720
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task ConnectAsync(IPAddress address, int port)
		{
			return Task.Factory.FromAsync<IPAddress, int>(new Func<IPAddress, int, AsyncCallback, object, IAsyncResult>(this.BeginConnect), new Action<IAsyncResult>(this.EndConnect), address, port, null);
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000A1547 File Offset: 0x0009F747
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task ConnectAsync(string host, int port)
		{
			return Task.Factory.FromAsync<string, int>(new Func<string, int, AsyncCallback, object, IAsyncResult>(this.BeginConnect), new Action<IAsyncResult>(this.EndConnect), host, port, null);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000A156E File Offset: 0x0009F76E
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task ConnectAsync(IPAddress[] addresses, int port)
		{
			return Task.Factory.FromAsync<IPAddress[], int>(new Func<IPAddress[], int, AsyncCallback, object, IAsyncResult>(this.BeginConnect), new Action<IAsyncResult>(this.EndConnect), addresses, port, null);
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000A1598 File Offset: 0x0009F798
		public NetworkStream GetStream()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "GetStream", "");
			}
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!this.Client.Connected)
			{
				throw new InvalidOperationException(SR.GetString("net_notconnected"));
			}
			if (this.m_DataStream == null)
			{
				this.m_DataStream = new NetworkStream(this.Client, true);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "GetStream", this.m_DataStream);
			}
			return this.m_DataStream;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000A1634 File Offset: 0x0009F834
		public void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Close", "");
			}
			((IDisposable)this).Dispose();
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Close", "");
			}
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000A1674 File Offset: 0x0009F874
		protected virtual void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Dispose", "");
			}
			if (this.m_CleanedUp)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Sockets, this, "Dispose", "");
				}
				return;
			}
			if (disposing)
			{
				IDisposable dataStream = this.m_DataStream;
				if (dataStream != null)
				{
					dataStream.Dispose();
				}
				else
				{
					Socket client = this.Client;
					if (client != null)
					{
						try
						{
							client.InternalShutdown(SocketShutdown.Both);
						}
						finally
						{
							client.Close();
							this.Client = null;
						}
					}
				}
				GC.SuppressFinalize(this);
			}
			this.m_CleanedUp = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Dispose", "");
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000A1734 File Offset: 0x0009F934
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000A1740 File Offset: 0x0009F940
		~TcpClient()
		{
			this.Dispose(false);
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060021AC RID: 8620 RVA: 0x000A1770 File Offset: 0x0009F970
		// (set) Token: 0x060021AD RID: 8621 RVA: 0x000A1782 File Offset: 0x0009F982
		public int ReceiveBufferSize
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x000A179A File Offset: 0x0009F99A
		// (set) Token: 0x060021AF RID: 8623 RVA: 0x000A17AC File Offset: 0x0009F9AC
		public int SendBufferSize
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060021B0 RID: 8624 RVA: 0x000A17C4 File Offset: 0x0009F9C4
		// (set) Token: 0x060021B1 RID: 8625 RVA: 0x000A17D6 File Offset: 0x0009F9D6
		public int ReceiveTimeout
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060021B2 RID: 8626 RVA: 0x000A17EE File Offset: 0x0009F9EE
		// (set) Token: 0x060021B3 RID: 8627 RVA: 0x000A1800 File Offset: 0x0009FA00
		public int SendTimeout
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x000A1818 File Offset: 0x0009FA18
		// (set) Token: 0x060021B5 RID: 8629 RVA: 0x000A1834 File Offset: 0x0009FA34
		public LingerOption LingerState
		{
			get
			{
				return (LingerOption)this.Client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x000A184C File Offset: 0x0009FA4C
		// (set) Token: 0x060021B7 RID: 8631 RVA: 0x000A185B File Offset: 0x0009FA5B
		public bool NoDelay
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Tcp, SocketOptionName.Debug) != 0;
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
			}
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000A1871 File Offset: 0x0009FA71
		private void initialize()
		{
			this.Client = new Socket(this.m_Family, SocketType.Stream, ProtocolType.Tcp);
			this.m_Active = false;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000A188D File Offset: 0x0009FA8D
		private int numericOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
		{
			return (int)this.Client.GetSocketOption(optionLevel, optionName);
		}

		// Token: 0x04001F36 RID: 7990
		private Socket m_ClientSocket;

		// Token: 0x04001F37 RID: 7991
		private bool m_Active;

		// Token: 0x04001F38 RID: 7992
		private NetworkStream m_DataStream;

		// Token: 0x04001F39 RID: 7993
		private AddressFamily m_Family = AddressFamily.InterNetwork;

		// Token: 0x04001F3A RID: 7994
		private bool m_CleanedUp;
	}
}
