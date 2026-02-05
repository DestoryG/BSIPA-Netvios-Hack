using System;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
	// Token: 0x02000389 RID: 905
	public class UdpClient : IDisposable
	{
		// Token: 0x060021D0 RID: 8656 RVA: 0x000A1F98 File Offset: 0x000A0198
		public UdpClient()
			: this(AddressFamily.InterNetwork)
		{
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000A1FA4 File Offset: 0x000A01A4
		public UdpClient(AddressFamily family)
		{
			this.m_Buffer = new byte[65536];
			this.m_Family = AddressFamily.InterNetwork;
			base..ctor();
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_family", new object[] { "UDP" }), "family");
			}
			this.m_Family = family;
			this.createClientSocket();
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000A2007 File Offset: 0x000A0207
		public UdpClient(int port)
			: this(port, AddressFamily.InterNetwork)
		{
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000A2014 File Offset: 0x000A0214
		public UdpClient(int port, AddressFamily family)
		{
			this.m_Buffer = new byte[65536];
			this.m_Family = AddressFamily.InterNetwork;
			base..ctor();
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_family"), "family");
			}
			this.m_Family = family;
			IPEndPoint ipendPoint;
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				ipendPoint = new IPEndPoint(IPAddress.Any, port);
			}
			else
			{
				ipendPoint = new IPEndPoint(IPAddress.IPv6Any, port);
			}
			this.createClientSocket();
			this.Client.Bind(ipendPoint);
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000A20AC File Offset: 0x000A02AC
		public UdpClient(IPEndPoint localEP)
		{
			this.m_Buffer = new byte[65536];
			this.m_Family = AddressFamily.InterNetwork;
			base..ctor();
			if (localEP == null)
			{
				throw new ArgumentNullException("localEP");
			}
			this.m_Family = localEP.AddressFamily;
			this.createClientSocket();
			this.Client.Bind(localEP);
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x000A2104 File Offset: 0x000A0304
		public UdpClient(string hostname, int port)
		{
			this.m_Buffer = new byte[65536];
			this.m_Family = AddressFamily.InterNetwork;
			base..ctor();
			if (hostname == null)
			{
				throw new ArgumentNullException("hostname");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.Connect(hostname, port);
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060021D6 RID: 8662 RVA: 0x000A2157 File Offset: 0x000A0357
		// (set) Token: 0x060021D7 RID: 8663 RVA: 0x000A215F File Offset: 0x000A035F
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

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x000A2168 File Offset: 0x000A0368
		// (set) Token: 0x060021D9 RID: 8665 RVA: 0x000A2170 File Offset: 0x000A0370
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

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x000A2179 File Offset: 0x000A0379
		public int Available
		{
			get
			{
				return this.m_ClientSocket.Available;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x000A2186 File Offset: 0x000A0386
		// (set) Token: 0x060021DC RID: 8668 RVA: 0x000A2193 File Offset: 0x000A0393
		public short Ttl
		{
			get
			{
				return this.m_ClientSocket.Ttl;
			}
			set
			{
				this.m_ClientSocket.Ttl = value;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x000A21A1 File Offset: 0x000A03A1
		// (set) Token: 0x060021DE RID: 8670 RVA: 0x000A21AE File Offset: 0x000A03AE
		public bool DontFragment
		{
			get
			{
				return this.m_ClientSocket.DontFragment;
			}
			set
			{
				this.m_ClientSocket.DontFragment = value;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x000A21BC File Offset: 0x000A03BC
		// (set) Token: 0x060021E0 RID: 8672 RVA: 0x000A21C9 File Offset: 0x000A03C9
		public bool MulticastLoopback
		{
			get
			{
				return this.m_ClientSocket.MulticastLoopback;
			}
			set
			{
				this.m_ClientSocket.MulticastLoopback = value;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x000A21D7 File Offset: 0x000A03D7
		// (set) Token: 0x060021E2 RID: 8674 RVA: 0x000A21E4 File Offset: 0x000A03E4
		public bool EnableBroadcast
		{
			get
			{
				return this.m_ClientSocket.EnableBroadcast;
			}
			set
			{
				this.m_ClientSocket.EnableBroadcast = value;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x000A21F2 File Offset: 0x000A03F2
		// (set) Token: 0x060021E4 RID: 8676 RVA: 0x000A21FF File Offset: 0x000A03FF
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

		// Token: 0x060021E5 RID: 8677 RVA: 0x000A220D File Offset: 0x000A040D
		public void AllowNatTraversal(bool allowed)
		{
			if (allowed)
			{
				this.m_ClientSocket.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);
				return;
			}
			this.m_ClientSocket.SetIPProtectionLevel(IPProtectionLevel.EdgeRestricted);
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x000A222D File Offset: 0x000A042D
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x000A2238 File Offset: 0x000A0438
		private void FreeResources()
		{
			if (this.m_CleanedUp)
			{
				return;
			}
			Socket client = this.Client;
			if (client != null)
			{
				client.InternalShutdown(SocketShutdown.Both);
				client.Close();
				this.Client = null;
			}
			this.m_CleanedUp = true;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000A2273 File Offset: 0x000A0473
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000A227C File Offset: 0x000A047C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.FreeResources();
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000A2290 File Offset: 0x000A0490
		public void Connect(string hostname, int port)
		{
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
						socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					}
					if (Socket.OSSupportsIPv6)
					{
						socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
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
						if (NclUtilities.IsFatal(ex2))
						{
							throw;
						}
						ex = ex2;
					}
				}
			}
			catch (Exception ex3)
			{
				if (NclUtilities.IsFatal(ex3))
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
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000A2438 File Offset: 0x000A0638
		public void Connect(IPAddress addr, int port)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (addr == null)
			{
				throw new ArgumentNullException("addr");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			IPEndPoint ipendPoint = new IPEndPoint(addr, port);
			this.Connect(ipendPoint);
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000A2490 File Offset: 0x000A0690
		public void Connect(IPEndPoint endPoint)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			this.CheckForBroadcast(endPoint.Address);
			this.Client.Connect(endPoint);
			this.m_Active = true;
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x000A24E3 File Offset: 0x000A06E3
		private void CheckForBroadcast(IPAddress ipAddress)
		{
			if (this.Client != null && !this.m_IsBroadcast && ipAddress.IsBroadcast)
			{
				this.m_IsBroadcast = true;
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
			}
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000A2518 File Offset: 0x000A0718
		public int Send(byte[] dgram, int bytes, IPEndPoint endPoint)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (dgram == null)
			{
				throw new ArgumentNullException("dgram");
			}
			if (this.m_Active && endPoint != null)
			{
				throw new InvalidOperationException(SR.GetString("net_udpconnected"));
			}
			if (endPoint == null)
			{
				return this.Client.Send(dgram, 0, bytes, SocketFlags.None);
			}
			this.CheckForBroadcast(endPoint.Address);
			return this.Client.SendTo(dgram, 0, bytes, SocketFlags.None, endPoint);
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000A2598 File Offset: 0x000A0798
		public int Send(byte[] dgram, int bytes, string hostname, int port)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (dgram == null)
			{
				throw new ArgumentNullException("dgram");
			}
			if (this.m_Active && (hostname != null || port != 0))
			{
				throw new InvalidOperationException(SR.GetString("net_udpconnected"));
			}
			if (hostname == null || port == 0)
			{
				return this.Client.Send(dgram, 0, bytes, SocketFlags.None);
			}
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
			int num = 0;
			while (num < hostAddresses.Length && hostAddresses[num].AddressFamily != this.m_Family)
			{
				num++;
			}
			if (hostAddresses.Length == 0 || num == hostAddresses.Length)
			{
				throw new ArgumentException(SR.GetString("net_invalidAddressList"), "hostname");
			}
			this.CheckForBroadcast(hostAddresses[num]);
			IPEndPoint ipendPoint = new IPEndPoint(hostAddresses[num], port);
			return this.Client.SendTo(dgram, 0, bytes, SocketFlags.None, ipendPoint);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000A266C File Offset: 0x000A086C
		public int Send(byte[] dgram, int bytes)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (dgram == null)
			{
				throw new ArgumentNullException("dgram");
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_notconnected"));
			}
			return this.Client.Send(dgram, 0, bytes, SocketFlags.None);
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x000A26C8 File Offset: 0x000A08C8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] datagram, int bytes, IPEndPoint endPoint, AsyncCallback requestCallback, object state)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (datagram == null)
			{
				throw new ArgumentNullException("datagram");
			}
			if (bytes > datagram.Length || bytes < 0)
			{
				throw new ArgumentOutOfRangeException("bytes");
			}
			if (this.m_Active && endPoint != null)
			{
				throw new InvalidOperationException(SR.GetString("net_udpconnected"));
			}
			if (endPoint == null)
			{
				return this.Client.BeginSend(datagram, 0, bytes, SocketFlags.None, requestCallback, state);
			}
			this.CheckForBroadcast(endPoint.Address);
			return this.Client.BeginSendTo(datagram, 0, bytes, SocketFlags.None, endPoint, requestCallback, state);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x000A2764 File Offset: 0x000A0964
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] datagram, int bytes, string hostname, int port, AsyncCallback requestCallback, object state)
		{
			if (this.m_Active && (hostname != null || port != 0))
			{
				throw new InvalidOperationException(SR.GetString("net_udpconnected"));
			}
			IPEndPoint ipendPoint = null;
			if (hostname != null && port != 0)
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
				int num = 0;
				while (num < hostAddresses.Length && hostAddresses[num].AddressFamily != this.m_Family)
				{
					num++;
				}
				if (hostAddresses.Length == 0 || num == hostAddresses.Length)
				{
					throw new ArgumentException(SR.GetString("net_invalidAddressList"), "hostname");
				}
				this.CheckForBroadcast(hostAddresses[num]);
				ipendPoint = new IPEndPoint(hostAddresses[num], port);
			}
			return this.BeginSend(datagram, bytes, ipendPoint, requestCallback, state);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x000A27FE File Offset: 0x000A09FE
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] datagram, int bytes, AsyncCallback requestCallback, object state)
		{
			return this.BeginSend(datagram, bytes, null, requestCallback, state);
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000A280C File Offset: 0x000A0A0C
		public int EndSend(IAsyncResult asyncResult)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this.m_Active)
			{
				return this.Client.EndSend(asyncResult);
			}
			return this.Client.EndSendTo(asyncResult);
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000A2848 File Offset: 0x000A0A48
		public byte[] Receive(ref IPEndPoint remoteEP)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			EndPoint endPoint;
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				endPoint = IPEndPoint.Any;
			}
			else
			{
				endPoint = IPEndPoint.IPv6Any;
			}
			int num = this.Client.ReceiveFrom(this.m_Buffer, 65536, SocketFlags.None, ref endPoint);
			remoteEP = (IPEndPoint)endPoint;
			byte[] array = new byte[num];
			Buffer.BlockCopy(this.m_Buffer, 0, array, 0, num);
			return array;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000A28C0 File Offset: 0x000A0AC0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceive(AsyncCallback requestCallback, object state)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			EndPoint endPoint;
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				endPoint = IPEndPoint.Any;
			}
			else
			{
				endPoint = IPEndPoint.IPv6Any;
			}
			return this.Client.BeginReceiveFrom(this.m_Buffer, 0, 65536, SocketFlags.None, ref endPoint, requestCallback, state);
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000A291C File Offset: 0x000A0B1C
		public byte[] EndReceive(IAsyncResult asyncResult, ref IPEndPoint remoteEP)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			EndPoint endPoint;
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				endPoint = IPEndPoint.Any;
			}
			else
			{
				endPoint = IPEndPoint.IPv6Any;
			}
			int num = this.Client.EndReceiveFrom(asyncResult, ref endPoint);
			remoteEP = (IPEndPoint)endPoint;
			byte[] array = new byte[num];
			Buffer.BlockCopy(this.m_Buffer, 0, array, 0, num);
			return array;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000A2988 File Offset: 0x000A0B88
		public void JoinMulticastGroup(IPAddress multicastAddr)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (multicastAddr.AddressFamily != this.m_Family)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_multicast_family", new object[] { "UDP" }), "multicastAddr");
			}
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				MulticastOption multicastOption = new MulticastOption(multicastAddr);
				this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
				return;
			}
			IPv6MulticastOption pv6MulticastOption = new IPv6MulticastOption(multicastAddr);
			this.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, pv6MulticastOption);
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000A2A24 File Offset: 0x000A0C24
		public void JoinMulticastGroup(IPAddress multicastAddr, IPAddress localAddress)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this.m_Family != AddressFamily.InterNetwork)
			{
				throw new SocketException(SocketError.OperationNotSupported);
			}
			MulticastOption multicastOption = new MulticastOption(multicastAddr, localAddress);
			this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000A2A78 File Offset: 0x000A0C78
		public void JoinMulticastGroup(int ifindex, IPAddress multicastAddr)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (ifindex < 0)
			{
				throw new ArgumentException(SR.GetString("net_value_cannot_be_negative"), "ifindex");
			}
			if (this.m_Family != AddressFamily.InterNetworkV6)
			{
				throw new SocketException(SocketError.OperationNotSupported);
			}
			IPv6MulticastOption pv6MulticastOption = new IPv6MulticastOption(multicastAddr, (long)ifindex);
			this.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, pv6MulticastOption);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000A2AF4 File Offset: 0x000A0CF4
		public void JoinMulticastGroup(IPAddress multicastAddr, int timeToLive)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (!ValidationHelper.ValidateRange(timeToLive, 0, 255))
			{
				throw new ArgumentOutOfRangeException("timeToLive");
			}
			this.JoinMulticastGroup(multicastAddr);
			this.Client.SetSocketOption((this.m_Family == AddressFamily.InterNetwork) ? SocketOptionLevel.IP : SocketOptionLevel.IPv6, SocketOptionName.MulticastTimeToLive, timeToLive);
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000A2B64 File Offset: 0x000A0D64
		public void DropMulticastGroup(IPAddress multicastAddr)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (multicastAddr.AddressFamily != this.m_Family)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_multicast_family", new object[] { "UDP" }), "multicastAddr");
			}
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				MulticastOption multicastOption = new MulticastOption(multicastAddr);
				this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, multicastOption);
				return;
			}
			IPv6MulticastOption pv6MulticastOption = new IPv6MulticastOption(multicastAddr);
			this.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, pv6MulticastOption);
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000A2C00 File Offset: 0x000A0E00
		public void DropMulticastGroup(IPAddress multicastAddr, int ifindex)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (ifindex < 0)
			{
				throw new ArgumentException(SR.GetString("net_value_cannot_be_negative"), "ifindex");
			}
			if (this.m_Family != AddressFamily.InterNetworkV6)
			{
				throw new SocketException(SocketError.OperationNotSupported);
			}
			IPv6MulticastOption pv6MulticastOption = new IPv6MulticastOption(multicastAddr, (long)ifindex);
			this.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, pv6MulticastOption);
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000A2C7B File Offset: 0x000A0E7B
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<int> SendAsync(byte[] datagram, int bytes)
		{
			return Task<int>.Factory.FromAsync<byte[], int>(new Func<byte[], int, AsyncCallback, object, IAsyncResult>(this.BeginSend), new Func<IAsyncResult, int>(this.EndSend), datagram, bytes, null);
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x000A2CA2 File Offset: 0x000A0EA2
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<int> SendAsync(byte[] datagram, int bytes, IPEndPoint endPoint)
		{
			return Task<int>.Factory.FromAsync<byte[], int, IPEndPoint>(new Func<byte[], int, IPEndPoint, AsyncCallback, object, IAsyncResult>(this.BeginSend), new Func<IAsyncResult, int>(this.EndSend), datagram, bytes, endPoint, null);
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000A2CCC File Offset: 0x000A0ECC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<int> SendAsync(byte[] datagram, int bytes, string hostname, int port)
		{
			return Task<int>.Factory.FromAsync((AsyncCallback callback, object state) => this.BeginSend(datagram, bytes, hostname, port, callback, state), new Func<IAsyncResult, int>(this.EndSend), null);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000A2D26 File Offset: 0x000A0F26
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<UdpReceiveResult> ReceiveAsync()
		{
			return Task<UdpReceiveResult>.Factory.FromAsync((AsyncCallback callback, object state) => this.BeginReceive(callback, state), delegate(IAsyncResult ar)
			{
				IPEndPoint ipendPoint = null;
				byte[] array = this.EndReceive(ar, ref ipendPoint);
				return new UdpReceiveResult(array, ipendPoint);
			}, null);
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000A2D4B File Offset: 0x000A0F4B
		private void createClientSocket()
		{
			this.Client = new Socket(this.m_Family, SocketType.Dgram, ProtocolType.Udp);
		}

		// Token: 0x04001F46 RID: 8006
		private const int MaxUDPSize = 65536;

		// Token: 0x04001F47 RID: 8007
		private Socket m_ClientSocket;

		// Token: 0x04001F48 RID: 8008
		private bool m_Active;

		// Token: 0x04001F49 RID: 8009
		private byte[] m_Buffer;

		// Token: 0x04001F4A RID: 8010
		private AddressFamily m_Family;

		// Token: 0x04001F4B RID: 8011
		private bool m_CleanedUp;

		// Token: 0x04001F4C RID: 8012
		private bool m_IsBroadcast;
	}
}
