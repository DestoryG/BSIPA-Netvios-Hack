using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x0200015F RID: 351
	[FriendAccessAllowed]
	public class ServicePoint
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x000410FE File Offset: 0x0003F2FE
		internal string LookupString
		{
			get
			{
				return this.m_LookupString;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00041106 File Offset: 0x0003F306
		internal string Hostname
		{
			get
			{
				return this.m_HostName;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x0004110E File Offset: 0x0003F30E
		internal bool IsTrustedHost
		{
			get
			{
				return this.m_IsTrustedHost;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00041116 File Offset: 0x0003F316
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0004111E File Offset: 0x0003F31E
		public BindIPEndPoint BindIPEndPointDelegate
		{
			get
			{
				return this.m_BindIPEndPointDelegate;
			}
			set
			{
				ExceptionHelper.InfrastructurePermission.Demand();
				this.m_BindIPEndPointDelegate = value;
			}
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00041134 File Offset: 0x0003F334
		internal ServicePoint(Uri address, TimerThread.Queue defaultIdlingQueue, int defaultConnectionLimit, string lookupString, bool userChangedLimit, bool proxyServicePoint)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "ServicePoint", address.DnsSafeHost + ":" + address.Port.ToString());
			}
			this.m_ProxyServicePoint = proxyServicePoint;
			this.m_Address = address;
			this.m_ConnectionName = address.Scheme;
			this.m_Host = address.DnsSafeHost;
			this.m_Port = address.Port;
			this.m_IdlingQueue = defaultIdlingQueue;
			this.m_ConnectionLimit = defaultConnectionLimit;
			this.m_HostLoopbackGuess = TriState.Unspecified;
			this.m_LookupString = lookupString;
			this.m_UserChangedLimit = userChangedLimit;
			this.m_UseNagleAlgorithm = ServicePointManager.UseNagleAlgorithm;
			this.m_Expect100Continue = ServicePointManager.Expect100Continue;
			this.m_ConnectionGroupList = new Hashtable(10);
			this.m_ConnectionLeaseTimeout = -1;
			this.m_ReceiveBufferSize = -1;
			this.m_UseTcpKeepAlive = ServicePointManager.s_UseTcpKeepAlive;
			this.m_TcpKeepAliveTime = ServicePointManager.s_TcpKeepAliveTime;
			this.m_TcpKeepAliveInterval = ServicePointManager.s_TcpKeepAliveInterval;
			this.m_Understands100Continue = true;
			this.m_HttpBehaviour = HttpBehaviour.Unknown;
			this.m_IdleSince = DateTime.Now;
			this.m_ExpiringTimer = this.m_IdlingQueue.CreateTimer(ServicePointManager.IdleServicePointTimeoutDelegate, this);
			this.m_IdleConnectionGroupTimeoutDelegate = new TimerThread.Callback(this.IdleConnectionGroupTimeoutCallback);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0004128C File Offset: 0x0003F48C
		internal ServicePoint(string host, int port, TimerThread.Queue defaultIdlingQueue, int defaultConnectionLimit, string lookupString, bool userChangedLimit, bool proxyServicePoint)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "ServicePoint", host + ":" + port.ToString());
			}
			this.m_ProxyServicePoint = proxyServicePoint;
			this.m_ConnectionName = "ByHost:" + host + ":" + port.ToString(CultureInfo.InvariantCulture);
			this.m_IdlingQueue = defaultIdlingQueue;
			this.m_ConnectionLimit = defaultConnectionLimit;
			this.m_HostLoopbackGuess = TriState.Unspecified;
			this.m_LookupString = lookupString;
			this.m_UserChangedLimit = userChangedLimit;
			this.m_ConnectionGroupList = new Hashtable(10);
			this.m_ConnectionLeaseTimeout = -1;
			this.m_ReceiveBufferSize = -1;
			this.m_Host = host;
			this.m_Port = port;
			this.m_HostMode = true;
			this.m_IdleSince = DateTime.Now;
			this.m_ExpiringTimer = this.m_IdlingQueue.CreateTimer(ServicePointManager.IdleServicePointTimeoutDelegate, this);
			this.m_IdleConnectionGroupTimeoutDelegate = new TimerThread.Callback(this.IdleConnectionGroupTimeoutCallback);
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00041399 File Offset: 0x0003F599
		internal object CachedChannelBinding
		{
			get
			{
				return this.m_CachedChannelBinding;
			}
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x000413A1 File Offset: 0x0003F5A1
		internal void SetCachedChannelBinding(Uri uri, ChannelBinding binding)
		{
			if (uri.Scheme == Uri.UriSchemeHttps)
			{
				this.m_CachedChannelBinding = ((binding != null) ? binding : DBNull.Value);
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x000413C8 File Offset: 0x0003F5C8
		private ConnectionGroup FindConnectionGroup(string connName, bool dontCreate)
		{
			string text = ConnectionGroup.MakeQueryStr(connName);
			ConnectionGroup connectionGroup = this.m_ConnectionGroupList[text] as ConnectionGroup;
			if (connectionGroup == null && !dontCreate)
			{
				connectionGroup = new ConnectionGroup(this, connName);
				this.m_ConnectionGroupList[text] = connectionGroup;
			}
			return connectionGroup;
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0004140C File Offset: 0x0003F60C
		internal Socket GetConnection(PooledStream PooledStream, object owner, bool async, out IPAddress address, ref Socket abortSocket, ref Socket abortSocket6)
		{
			Socket socket = null;
			Socket socket2 = null;
			Socket socket3 = null;
			Exception ex = null;
			address = null;
			if (Socket.OSSupportsIPv4)
			{
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			}
			if (Socket.OSSupportsIPv6)
			{
				socket2 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
			}
			abortSocket = socket;
			abortSocket6 = socket2;
			ServicePoint.ConnectSocketState connectSocketState = null;
			if (async)
			{
				connectSocketState = new ServicePoint.ConnectSocketState(this, PooledStream, owner, socket, socket2);
			}
			WebExceptionStatus webExceptionStatus = this.ConnectSocket(socket, socket2, ref socket3, ref address, connectSocketState, out ex);
			if (webExceptionStatus == WebExceptionStatus.Pending)
			{
				return null;
			}
			if (webExceptionStatus != WebExceptionStatus.Success)
			{
				throw new WebException(NetRes.GetWebStatusString(webExceptionStatus), (webExceptionStatus == WebExceptionStatus.ProxyNameResolutionFailure || webExceptionStatus == WebExceptionStatus.NameResolutionFailure) ? this.Host : null, ex, webExceptionStatus, null, WebExceptionInternalStatus.ServicePointFatal);
			}
			if (socket3 == null)
			{
				throw new IOException(global::System.SR.GetString("net_io_transportfailure"));
			}
			this.CompleteGetConnection(socket, socket2, socket3, address);
			return socket3;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000414C8 File Offset: 0x0003F6C8
		private void CompleteGetConnection(Socket socket, Socket socket6, Socket finalSocket, IPAddress address)
		{
			if (finalSocket.AddressFamily == AddressFamily.InterNetwork)
			{
				if (socket6 != null)
				{
					socket6.Close();
					socket6 = null;
				}
			}
			else if (socket != null)
			{
				socket.Close();
				socket = null;
			}
			if (!this.UseNagleAlgorithm)
			{
				finalSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, 1);
			}
			if (this.ReceiveBufferSize != -1)
			{
				finalSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, this.ReceiveBufferSize);
			}
			if (this.m_UseTcpKeepAlive)
			{
				byte[] array = new byte[]
				{
					1,
					0,
					0,
					0,
					(byte)(this.m_TcpKeepAliveTime & 255),
					(byte)((this.m_TcpKeepAliveTime >> 8) & 255),
					(byte)((this.m_TcpKeepAliveTime >> 16) & 255),
					(byte)((this.m_TcpKeepAliveTime >> 24) & 255),
					(byte)(this.m_TcpKeepAliveInterval & 255),
					(byte)((this.m_TcpKeepAliveInterval >> 8) & 255),
					(byte)((this.m_TcpKeepAliveInterval >> 16) & 255),
					(byte)((this.m_TcpKeepAliveInterval >> 24) & 255)
				};
				finalSocket.IOControl((IOControlCode)((ulong)(-1744830460)), array, null);
			}
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000415E1 File Offset: 0x0003F7E1
		internal virtual void SubmitRequest(HttpWebRequest request)
		{
			this.SubmitRequest(request, null);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x000415EC File Offset: 0x0003F7EC
		internal void SubmitRequest(HttpWebRequest request, string connName)
		{
			bool flag = false;
			ConnectionGroup connectionGroup;
			lock (this)
			{
				connectionGroup = this.FindConnectionGroup(connName, false);
			}
			for (;;)
			{
				Connection connection = connectionGroup.FindConnection(request, connName, out flag);
				if (connection == null)
				{
					break;
				}
				if (connection.SubmitRequest(request, flag))
				{
					return;
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00041648 File Offset: 0x0003F848
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x00041650 File Offset: 0x0003F850
		public int ConnectionLeaseTimeout
		{
			get
			{
				return this.m_ConnectionLeaseTimeout;
			}
			set
			{
				if (!ValidationHelper.ValidateRange(value, -1, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != this.m_ConnectionLeaseTimeout)
				{
					this.m_ConnectionLeaseTimeout = value;
					this.m_ConnectionLeaseTimerQueue = null;
				}
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x00041684 File Offset: 0x0003F884
		internal TimerThread.Queue ConnectionLeaseTimerQueue
		{
			get
			{
				if (this.m_ConnectionLeaseTimerQueue == null)
				{
					TimerThread.Queue orCreateQueue = TimerThread.GetOrCreateQueue(this.ConnectionLeaseTimeout);
					this.m_ConnectionLeaseTimerQueue = orCreateQueue;
				}
				return this.m_ConnectionLeaseTimerQueue;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x000416B4 File Offset: 0x0003F8B4
		public Uri Address
		{
			get
			{
				if (this.m_HostMode)
				{
					throw new NotSupportedException(global::System.SR.GetString("net_servicePointAddressNotSupportedInHostMode"));
				}
				if (this.m_ProxyServicePoint)
				{
					ExceptionHelper.WebPermissionUnrestricted.Demand();
				}
				return this.m_Address;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x000416E6 File Offset: 0x0003F8E6
		internal Uri InternalAddress
		{
			get
			{
				return this.m_Address;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x000416EE File Offset: 0x0003F8EE
		internal string Host
		{
			get
			{
				if (this.m_HostMode)
				{
					return this.m_Host;
				}
				return this.m_Address.Host;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0004170A File Offset: 0x0003F90A
		internal int Port
		{
			get
			{
				return this.m_Port;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00041712 File Offset: 0x0003F912
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x00041720 File Offset: 0x0003F920
		public int MaxIdleTime
		{
			get
			{
				return this.m_IdlingQueue.Duration;
			}
			set
			{
				if (!ValidationHelper.ValidateRange(value, -1, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value == this.m_IdlingQueue.Duration)
				{
					return;
				}
				object expiringTimerLock = this.m_ExpiringTimerLock;
				lock (expiringTimerLock)
				{
					if (this.m_ExpiringTimer == null || this.m_ExpiringTimer.Cancel())
					{
						this.m_IdlingQueue = TimerThread.GetOrCreateQueue(value);
						if (this.m_ExpiringTimer != null)
						{
							double totalMilliseconds = (DateTime.Now - this.m_IdleSince).TotalMilliseconds;
							int num = ((totalMilliseconds >= 2147483647.0) ? int.MaxValue : ((int)totalMilliseconds));
							int num2 = ((value == -1) ? (-1) : ((num >= value) ? 0 : (value - num)));
							this.m_ExpiringTimer = TimerThread.CreateQueue(num2).CreateTimer(ServicePointManager.IdleServicePointTimeoutDelegate, this);
						}
					}
				}
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00041808 File Offset: 0x0003FA08
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x00041810 File Offset: 0x0003FA10
		public bool UseNagleAlgorithm
		{
			get
			{
				return this.m_UseNagleAlgorithm;
			}
			set
			{
				this.m_UseNagleAlgorithm = value;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00041819 File Offset: 0x0003FA19
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x00041821 File Offset: 0x0003FA21
		public int ReceiveBufferSize
		{
			get
			{
				return this.m_ReceiveBufferSize;
			}
			set
			{
				if (!ValidationHelper.ValidateRange(value, -1, 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_ReceiveBufferSize = value;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0004184C File Offset: 0x0003FA4C
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x00041843 File Offset: 0x0003FA43
		public bool Expect100Continue
		{
			get
			{
				return this.m_Expect100Continue;
			}
			set
			{
				this.m_Expect100Continue = value;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00041854 File Offset: 0x0003FA54
		public DateTime IdleSince
		{
			get
			{
				return this.m_IdleSince;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x0004185C File Offset: 0x0003FA5C
		public virtual Version ProtocolVersion
		{
			get
			{
				if (this.m_HttpBehaviour <= HttpBehaviour.HTTP10 && this.m_HttpBehaviour != HttpBehaviour.Unknown)
				{
					return HttpVersion.Version10;
				}
				return HttpVersion.Version11;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0004187A File Offset: 0x0003FA7A
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x00041882 File Offset: 0x0003FA82
		internal HttpBehaviour HttpBehaviour
		{
			get
			{
				return this.m_HttpBehaviour;
			}
			set
			{
				this.m_HttpBehaviour = value;
				this.m_Understands100Continue = this.m_Understands100Continue && (this.m_HttpBehaviour > HttpBehaviour.HTTP10 || this.m_HttpBehaviour == HttpBehaviour.Unknown);
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x000418B1 File Offset: 0x0003FAB1
		public string ConnectionName
		{
			get
			{
				return this.m_ConnectionName;
			}
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000418B9 File Offset: 0x0003FAB9
		public bool CloseConnectionGroup(string connectionGroupName)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "CloseConnectionGroup", connectionGroupName);
			}
			return this.CloseConnectionGroupHelper(connectionGroupName, false);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x000418DB File Offset: 0x0003FADB
		internal void CloseConnectionGroupInternal(string connectionGroupName)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "CloseConnectionGroupInternal", connectionGroupName);
			}
			this.CloseConnectionGroupHelper(connectionGroupName, true);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00041900 File Offset: 0x0003FB00
		private bool CloseConnectionGroupHelper(string connectionGroupName, bool closeInternal)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "CloseConnectionGroupHelper", "connectionGroupName=" + connectionGroupName + ", closeInternal=" + closeInternal.ToString());
			}
			string text = HttpWebRequest.GenerateConnectionGroup(connectionGroupName, false, false).ToString();
			string text2 = HttpWebRequest.GenerateConnectionGroup(connectionGroupName, true, false).ToString();
			string text3 = HttpWebRequest.GenerateConnectionGroup(connectionGroupName, false, true).ToString();
			string text4 = HttpWebRequest.GenerateConnectionGroup(connectionGroupName, true, true).ToString();
			List<string> list = null;
			lock (this)
			{
				foreach (object obj in this.m_ConnectionGroupList.Keys)
				{
					string text5 = obj as string;
					if (text5.StartsWith(text3, StringComparison.Ordinal) || text5.StartsWith(text4, StringComparison.Ordinal))
					{
						if (closeInternal)
						{
							if (list == null)
							{
								list = new List<string>();
							}
							list.Add(text5);
						}
					}
					else if ((text5.StartsWith(text, StringComparison.Ordinal) || text5.StartsWith(text2, StringComparison.Ordinal)) && !closeInternal)
					{
						if (list == null)
						{
							list = new List<string>();
						}
						list.Add(text5);
					}
				}
			}
			bool flag2 = false;
			if (list != null)
			{
				foreach (string text6 in list)
				{
					if (this.ReleaseConnectionGroup(text6))
					{
						flag2 = true;
					}
				}
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "CloseConnectionGroupHelper, returning", flag2.ToString());
			}
			return flag2;
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00041ABC File Offset: 0x0003FCBC
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x00041BA0 File Offset: 0x0003FDA0
		public int ConnectionLimit
		{
			get
			{
				if (!this.m_UserChangedLimit && this.m_IPAddressInfoList == null && this.m_HostLoopbackGuess == TriState.Unspecified)
				{
					lock (this)
					{
						if (!this.m_UserChangedLimit && this.m_IPAddressInfoList == null && this.m_HostLoopbackGuess == TriState.Unspecified)
						{
							IPAddress ipaddress = null;
							if (IPAddress.TryParse(this.m_Host, out ipaddress))
							{
								this.m_HostLoopbackGuess = (ServicePoint.IsAddressListLoopback(new IPAddress[] { ipaddress }) ? TriState.True : TriState.False);
							}
							else
							{
								this.m_HostLoopbackGuess = (NclUtilities.GuessWhetherHostIsLoopback(this.m_Host) ? TriState.True : TriState.False);
							}
						}
					}
				}
				if (!this.m_UserChangedLimit && !((this.m_IPAddressInfoList == null) ? (this.m_HostLoopbackGuess != TriState.True) : (!this.m_IPAddressesAreLoopback)))
				{
					return int.MaxValue;
				}
				return this.m_ConnectionLimit;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (!this.m_UserChangedLimit || this.m_ConnectionLimit != value)
				{
					lock (this)
					{
						if (!this.m_UserChangedLimit || this.m_ConnectionLimit != value)
						{
							this.m_ConnectionLimit = value;
							this.m_UserChangedLimit = true;
							this.ResolveConnectionLimit();
						}
					}
				}
			}
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00041C1C File Offset: 0x0003FE1C
		private void ResolveConnectionLimit()
		{
			int connectionLimit = this.ConnectionLimit;
			foreach (object obj in this.m_ConnectionGroupList.Values)
			{
				ConnectionGroup connectionGroup = (ConnectionGroup)obj;
				connectionGroup.ConnectionLimit = connectionLimit;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00041C84 File Offset: 0x0003FE84
		public int CurrentConnections
		{
			get
			{
				int num = 0;
				lock (this)
				{
					foreach (object obj in this.m_ConnectionGroupList.Values)
					{
						ConnectionGroup connectionGroup = (ConnectionGroup)obj;
						num += connectionGroup.CurrentConnections;
					}
				}
				return num;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00041D10 File Offset: 0x0003FF10
		public X509Certificate Certificate
		{
			get
			{
				object serverCertificateOrBytes = this.m_ServerCertificateOrBytes;
				if (serverCertificateOrBytes != null && serverCertificateOrBytes.GetType() == typeof(byte[]))
				{
					return (X509Certificate)(this.m_ServerCertificateOrBytes = new X509Certificate((byte[])serverCertificateOrBytes));
				}
				return serverCertificateOrBytes as X509Certificate;
			}
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00041D5E File Offset: 0x0003FF5E
		internal void UpdateServerCertificate(X509Certificate certificate)
		{
			if (certificate != null)
			{
				this.m_ServerCertificateOrBytes = certificate.GetRawCertData();
				return;
			}
			this.m_ServerCertificateOrBytes = null;
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x00041D78 File Offset: 0x0003FF78
		public X509Certificate ClientCertificate
		{
			get
			{
				object clientCertificateOrBytes = this.m_ClientCertificateOrBytes;
				if (clientCertificateOrBytes != null && clientCertificateOrBytes.GetType() == typeof(byte[]))
				{
					return (X509Certificate)(this.m_ClientCertificateOrBytes = new X509Certificate((byte[])clientCertificateOrBytes));
				}
				return clientCertificateOrBytes as X509Certificate;
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00041DC6 File Offset: 0x0003FFC6
		internal void UpdateClientCertificate(X509Certificate certificate)
		{
			if (certificate != null)
			{
				this.m_ClientCertificateOrBytes = certificate.GetRawCertData();
				return;
			}
			this.m_ClientCertificateOrBytes = null;
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00041DDF File Offset: 0x0003FFDF
		public bool SupportsPipelining
		{
			get
			{
				return this.m_HttpBehaviour > HttpBehaviour.HTTP10 || this.m_HttpBehaviour == HttpBehaviour.Unknown;
			}
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00041DF8 File Offset: 0x0003FFF8
		public void SetTcpKeepAlive(bool enabled, int keepAliveTime, int keepAliveInterval)
		{
			if (!enabled)
			{
				this.m_UseTcpKeepAlive = false;
				this.m_TcpKeepAliveTime = 0;
				this.m_TcpKeepAliveInterval = 0;
				return;
			}
			this.m_UseTcpKeepAlive = true;
			if (keepAliveTime <= 0)
			{
				throw new ArgumentOutOfRangeException("keepAliveTime");
			}
			if (keepAliveInterval <= 0)
			{
				throw new ArgumentOutOfRangeException("keepAliveInterval");
			}
			this.m_TcpKeepAliveTime = keepAliveTime;
			this.m_TcpKeepAliveInterval = keepAliveInterval;
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00041E5A File Offset: 0x0004005A
		// (set) Token: 0x06000C4C RID: 3148 RVA: 0x00041E51 File Offset: 0x00040051
		internal bool Understands100Continue
		{
			get
			{
				return this.m_Understands100Continue;
			}
			set
			{
				this.m_Understands100Continue = value;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00041E62 File Offset: 0x00040062
		internal bool InternalProxyServicePoint
		{
			get
			{
				return this.m_ProxyServicePoint;
			}
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00041E6C File Offset: 0x0004006C
		internal void IncrementConnection()
		{
			object expiringTimerLock = this.m_ExpiringTimerLock;
			lock (expiringTimerLock)
			{
				this.m_CurrentConnections++;
				if (this.m_CurrentConnections == 1)
				{
					this.m_ExpiringTimer.Cancel();
					this.m_ExpiringTimer = null;
				}
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00041ED0 File Offset: 0x000400D0
		internal void DecrementConnection()
		{
			object expiringTimerLock = this.m_ExpiringTimerLock;
			lock (expiringTimerLock)
			{
				this.m_CurrentConnections--;
				if (this.m_CurrentConnections == 0)
				{
					this.m_IdleSince = DateTime.Now;
					this.m_ExpiringTimer = this.m_IdlingQueue.CreateTimer(ServicePointManager.IdleServicePointTimeoutDelegate, this);
				}
				else if (this.m_CurrentConnections < 0)
				{
					this.m_CurrentConnections = 0;
				}
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00041F54 File Offset: 0x00040154
		internal RemoteCertValidationCallback SetupHandshakeDoneProcedure(TlsStream secureStream, object request)
		{
			return ServicePoint.HandshakeDoneProcedure.CreateAdapter(this, secureStream, request);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00041F60 File Offset: 0x00040160
		private void IdleConnectionGroupTimeoutCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			ConnectionGroup connectionGroup = (ConnectionGroup)context;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, global::System.SR.GetString("net_log_closed_idle", new object[]
				{
					"ConnectionGroup",
					connectionGroup.GetHashCode()
				}));
			}
			this.ReleaseConnectionGroup(connectionGroup.Name);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00041FB9 File Offset: 0x000401B9
		internal TimerThread.Timer CreateConnectionGroupTimer(ConnectionGroup connectionGroup)
		{
			return this.m_IdlingQueue.CreateTimer(this.m_IdleConnectionGroupTimeoutDelegate, connectionGroup);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00041FD0 File Offset: 0x000401D0
		internal bool ReleaseConnectionGroup(string connName)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "ReleaseConnectionGroup", connName);
			}
			ConnectionGroup connectionGroup = null;
			lock (this)
			{
				connectionGroup = this.FindConnectionGroup(connName, true);
				if (connectionGroup == null)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, "ReleaseConnectionGroup, returning", "false");
					}
					return false;
				}
				connectionGroup.CancelIdleTimer();
				this.m_ConnectionGroupList.Remove(connName);
			}
			connectionGroup.DisableKeepAliveOnConnections();
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "ReleaseConnectionGroup, returning", "true");
			}
			return true;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00042084 File Offset: 0x00040284
		internal void ReleaseAllConnectionGroups()
		{
			ArrayList arrayList = new ArrayList(this.m_ConnectionGroupList.Count);
			lock (this)
			{
				foreach (object obj in this.m_ConnectionGroupList.Values)
				{
					ConnectionGroup connectionGroup = (ConnectionGroup)obj;
					arrayList.Add(connectionGroup);
				}
				this.m_ConnectionGroupList.Clear();
			}
			foreach (object obj2 in arrayList)
			{
				ConnectionGroup connectionGroup2 = (ConnectionGroup)obj2;
				connectionGroup2.DisableKeepAliveOnConnections();
			}
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00042174 File Offset: 0x00040374
		private static void ConnectSocketCallback(IAsyncResult asyncResult)
		{
			ServicePoint.ConnectSocketState connectSocketState = (ServicePoint.ConnectSocketState)asyncResult.AsyncState;
			Socket socket = null;
			IPAddress ipaddress = null;
			Exception ex = null;
			Exception ex2 = null;
			WebExceptionStatus webExceptionStatus = WebExceptionStatus.ConnectFailure;
			try
			{
				webExceptionStatus = connectSocketState.servicePoint.ConnectSocketInternal(connectSocketState.connectFailure, connectSocketState.s4, connectSocketState.s6, ref socket, ref ipaddress, connectSocketState, asyncResult, out ex);
			}
			catch (SocketException ex3)
			{
				ex2 = ex3;
			}
			catch (ObjectDisposedException ex4)
			{
				ex2 = ex4;
			}
			if (webExceptionStatus == WebExceptionStatus.Pending)
			{
				return;
			}
			if (webExceptionStatus == WebExceptionStatus.Success)
			{
				try
				{
					connectSocketState.servicePoint.CompleteGetConnection(connectSocketState.s4, connectSocketState.s6, socket, ipaddress);
					goto IL_00B3;
				}
				catch (SocketException ex5)
				{
					ex2 = ex5;
					goto IL_00B3;
				}
				catch (ObjectDisposedException ex6)
				{
					ex2 = ex6;
					goto IL_00B3;
				}
			}
			ex2 = new WebException(NetRes.GetWebStatusString(webExceptionStatus), (webExceptionStatus == WebExceptionStatus.ProxyNameResolutionFailure || webExceptionStatus == WebExceptionStatus.NameResolutionFailure) ? connectSocketState.servicePoint.Host : null, ex, webExceptionStatus, null, WebExceptionInternalStatus.ServicePointFatal);
			IL_00B3:
			try
			{
				connectSocketState.pooledStream.ConnectionCallback(connectSocketState.owner, ex2, socket, ipaddress);
			}
			catch
			{
				if (socket == null || !socket.CleanedUp)
				{
					throw;
				}
			}
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0004229C File Offset: 0x0004049C
		private void BindUsingDelegate(Socket socket, IPEndPoint remoteIPEndPoint)
		{
			IPEndPoint ipendPoint = new IPEndPoint(remoteIPEndPoint.Address, remoteIPEndPoint.Port);
			int i;
			for (i = 0; i < 2147483647; i++)
			{
				IPEndPoint ipendPoint2 = this.BindIPEndPointDelegate(this, ipendPoint, i);
				if (ipendPoint2 == null)
				{
					break;
				}
				try
				{
					socket.InternalBind(ipendPoint2);
					break;
				}
				catch
				{
				}
			}
			if (i == 2147483647)
			{
				throw new OverflowException("Reached maximum number of BindIPEndPointDelegate retries");
			}
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00042310 File Offset: 0x00040510
		private void SetUnicastReusePortForSocket(Socket socket)
		{
			bool flag = (ServicePointManager.ReusePortSupported == null || ServicePointManager.ReusePortSupported.Value) && ServicePointManager.ReusePort;
			if (flag)
			{
				try
				{
					socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseUnicastPort, 1);
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, global::System.SR.GetString("net_log_set_socketoption_reuseport", new object[]
						{
							"Socket",
							socket.GetHashCode()
						}));
					}
					ServicePointManager.ReusePortSupported = new bool?(true);
				}
				catch (SocketException)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, global::System.SR.GetString("net_log_set_socketoption_reuseport_not_supported", new object[]
						{
							"Socket",
							socket.GetHashCode()
						}));
					}
					ServicePointManager.ReusePortSupported = new bool?(false);
				}
				catch (Exception ex)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, global::System.SR.GetString("net_log_unexpected_exception", new object[] { ex.Message }));
					}
				}
			}
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00042434 File Offset: 0x00040634
		private WebExceptionStatus ConnectSocketInternal(bool connectFailure, Socket s4, Socket s6, ref Socket socket, ref IPAddress address, ServicePoint.ConnectSocketState state, IAsyncResult asyncResult, out Exception exception)
		{
			exception = null;
			IPAddress[] array = null;
			for (int i = 0; i < 2; i++)
			{
				int j = 0;
				int num;
				if (asyncResult == null)
				{
					array = this.GetIPAddressInfoList(out num, array);
					if (array == null)
					{
						break;
					}
					if (array.Length == 0)
					{
						break;
					}
				}
				else
				{
					array = state.addresses;
					num = state.currentIndex;
					j = state.i;
					i = state.unsuccessfulAttempts;
				}
				while (j < array.Length)
				{
					IPAddress ipaddress = array[num];
					try
					{
						IPEndPoint ipendPoint = new IPEndPoint(ipaddress, this.m_Port);
						Socket socket2;
						if (ipendPoint.Address.AddressFamily == AddressFamily.InterNetwork)
						{
							socket2 = s4;
						}
						else
						{
							socket2 = s6;
						}
						if (state != null)
						{
							if (asyncResult == null)
							{
								state.addresses = array;
								state.currentIndex = num;
								state.i = j;
								state.unsuccessfulAttempts = i;
								state.connectFailure = connectFailure;
								if (!socket2.IsBound)
								{
									if (ServicePointManager.ReusePort)
									{
										this.SetUnicastReusePortForSocket(socket2);
									}
									if (this.BindIPEndPointDelegate != null)
									{
										this.BindUsingDelegate(socket2, ipendPoint);
									}
								}
								socket2.UnsafeBeginConnect(ipendPoint, ServicePoint.m_ConnectCallbackDelegate, state);
								return WebExceptionStatus.Pending;
							}
							IAsyncResult asyncResult2 = asyncResult;
							asyncResult = null;
							socket2.EndConnect(asyncResult2);
						}
						else
						{
							if (!socket2.IsBound)
							{
								if (ServicePointManager.ReusePort)
								{
									this.SetUnicastReusePortForSocket(socket2);
								}
								if (this.BindIPEndPointDelegate != null)
								{
									this.BindUsingDelegate(socket2, ipendPoint);
								}
							}
							bool flag = false;
							SafeCloseSocket safeHandle = socket2.SafeHandle;
							try
							{
								if (ServicePointManager.UseSafeSynchronousClose)
								{
									safeHandle.DangerousAddRef(ref flag);
								}
								socket2.InternalConnect(ipendPoint);
							}
							finally
							{
								if (flag)
								{
									safeHandle.DangerousRelease();
								}
							}
						}
						socket = socket2;
						address = ipaddress;
						exception = null;
						this.UpdateCurrentIndex(array, num);
						return WebExceptionStatus.Success;
					}
					catch (ObjectDisposedException)
					{
						return WebExceptionStatus.RequestCanceled;
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						exception = ex;
						connectFailure = true;
					}
					num++;
					if (num >= array.Length)
					{
						num = 0;
					}
					j++;
				}
			}
			this.Failed(array);
			if (connectFailure)
			{
				return WebExceptionStatus.ConnectFailure;
			}
			if (!this.InternalProxyServicePoint)
			{
				return WebExceptionStatus.NameResolutionFailure;
			}
			return WebExceptionStatus.ProxyNameResolutionFailure;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0004266C File Offset: 0x0004086C
		private WebExceptionStatus ConnectSocket(Socket s4, Socket s6, ref Socket socket, ref IPAddress address, ServicePoint.ConnectSocketState state, out Exception exception)
		{
			return this.ConnectSocketInternal(false, s4, s6, ref socket, ref address, state, null, out exception);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0004268C File Offset: 0x0004088C
		[Conditional("DEBUG")]
		internal void DebugMembers(int requestHash)
		{
			foreach (object obj in this.m_ConnectionGroupList.Values)
			{
				ConnectionGroup connectionGroup = (ConnectionGroup)obj;
				if (connectionGroup != null)
				{
					try
					{
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000426F8 File Offset: 0x000408F8
		private void Failed(IPAddress[] addresses)
		{
			if (addresses == this.m_IPAddressInfoList)
			{
				lock (this)
				{
					if (addresses == this.m_IPAddressInfoList)
					{
						this.m_AddressListFailed = true;
					}
				}
			}
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00042748 File Offset: 0x00040948
		private void UpdateCurrentIndex(IPAddress[] addresses, int currentIndex)
		{
			if (addresses == this.m_IPAddressInfoList && (this.m_CurrentAddressInfoIndex != currentIndex || !this.m_ConnectedSinceDns))
			{
				lock (this)
				{
					if (addresses == this.m_IPAddressInfoList)
					{
						if (!ServicePointManager.EnableDnsRoundRobin)
						{
							this.m_CurrentAddressInfoIndex = currentIndex;
						}
						this.m_ConnectedSinceDns = true;
					}
				}
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x000427B8 File Offset: 0x000409B8
		private bool HasTimedOut
		{
			get
			{
				int dnsRefreshTimeout = ServicePointManager.DnsRefreshTimeout;
				return dnsRefreshTimeout != -1 && this.m_LastDnsResolve + new TimeSpan(0, 0, 0, 0, dnsRefreshTimeout) < DateTime.UtcNow;
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x000427F0 File Offset: 0x000409F0
		private IPAddress[] GetIPAddressInfoList(out int currentIndex, IPAddress[] addresses)
		{
			IPHostEntry iphostEntry = null;
			currentIndex = 0;
			bool flag = false;
			bool flag2 = false;
			lock (this)
			{
				if (addresses != null && !this.m_ConnectedSinceDns && !this.m_AddressListFailed && addresses == this.m_IPAddressInfoList)
				{
					return null;
				}
				if (this.m_IPAddressInfoList == null || this.m_AddressListFailed || addresses == this.m_IPAddressInfoList || this.HasTimedOut)
				{
					this.m_CurrentAddressInfoIndex = 0;
					this.m_ConnectedSinceDns = false;
					this.m_AddressListFailed = false;
					this.m_LastDnsResolve = DateTime.UtcNow;
					flag = true;
				}
			}
			if (flag)
			{
				try
				{
					flag2 = !Dns.TryInternalResolve(this.m_Host, out iphostEntry);
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					flag2 = true;
				}
			}
			lock (this)
			{
				if (flag)
				{
					this.m_IPAddressInfoList = null;
					if (!flag2 && iphostEntry != null && iphostEntry.AddressList != null && iphostEntry.AddressList.Length != 0)
					{
						this.SetAddressList(iphostEntry);
					}
				}
				if (this.m_IPAddressInfoList != null && this.m_IPAddressInfoList.Length != 0)
				{
					currentIndex = this.m_CurrentAddressInfoIndex;
					if (ServicePointManager.EnableDnsRoundRobin)
					{
						this.m_CurrentAddressInfoIndex++;
						if (this.m_CurrentAddressInfoIndex >= this.m_IPAddressInfoList.Length)
						{
							this.m_CurrentAddressInfoIndex = 0;
						}
					}
					return this.m_IPAddressInfoList;
				}
			}
			return null;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00042970 File Offset: 0x00040B70
		private void SetAddressList(IPHostEntry ipHostEntry)
		{
			bool ipaddressesAreLoopback = this.m_IPAddressesAreLoopback;
			bool flag = this.m_IPAddressInfoList == null;
			this.m_IPAddressesAreLoopback = ServicePoint.IsAddressListLoopback(ipHostEntry.AddressList);
			this.m_IPAddressInfoList = ipHostEntry.AddressList;
			this.m_HostName = ipHostEntry.HostName;
			this.m_IsTrustedHost = ipHostEntry.isTrustedHost;
			if (flag || ipaddressesAreLoopback != this.m_IPAddressesAreLoopback)
			{
				this.ResolveConnectionLimit();
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000429D8 File Offset: 0x00040BD8
		private static bool IsAddressListLoopback(IPAddress[] addressList)
		{
			IPAddress[] array = null;
			try
			{
				array = NclUtilities.LocalAddresses;
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, global::System.SR.GetString("net_log_retrieving_localhost_exception", new object[] { ex }));
					Logging.PrintWarning(Logging.Web, global::System.SR.GetString("net_log_resolved_servicepoint_may_not_be_remote_server"));
				}
			}
			int i;
			for (i = 0; i < addressList.Length; i++)
			{
				if (!IPAddress.IsLoopback(addressList[i]))
				{
					if (array == null)
					{
						break;
					}
					int num = 0;
					while (num < array.Length && !addressList[i].Equals(array[num]))
					{
						num++;
					}
					if (num >= array.Length)
					{
						break;
					}
				}
			}
			return i == addressList.Length;
		}

		// Token: 0x04001146 RID: 4422
		internal const int LoopbackConnectionLimit = 2147483647;

		// Token: 0x04001147 RID: 4423
		private int m_ConnectionLeaseTimeout;

		// Token: 0x04001148 RID: 4424
		private TimerThread.Queue m_ConnectionLeaseTimerQueue;

		// Token: 0x04001149 RID: 4425
		private bool m_ProxyServicePoint;

		// Token: 0x0400114A RID: 4426
		private bool m_UserChangedLimit;

		// Token: 0x0400114B RID: 4427
		private bool m_UseNagleAlgorithm;

		// Token: 0x0400114C RID: 4428
		private TriState m_HostLoopbackGuess;

		// Token: 0x0400114D RID: 4429
		private int m_ReceiveBufferSize;

		// Token: 0x0400114E RID: 4430
		private bool m_Expect100Continue;

		// Token: 0x0400114F RID: 4431
		private bool m_Understands100Continue;

		// Token: 0x04001150 RID: 4432
		private HttpBehaviour m_HttpBehaviour;

		// Token: 0x04001151 RID: 4433
		private string m_LookupString;

		// Token: 0x04001152 RID: 4434
		private int m_ConnectionLimit;

		// Token: 0x04001153 RID: 4435
		private Hashtable m_ConnectionGroupList;

		// Token: 0x04001154 RID: 4436
		private Uri m_Address;

		// Token: 0x04001155 RID: 4437
		private string m_Host;

		// Token: 0x04001156 RID: 4438
		private int m_Port;

		// Token: 0x04001157 RID: 4439
		private TimerThread.Queue m_IdlingQueue;

		// Token: 0x04001158 RID: 4440
		private TimerThread.Timer m_ExpiringTimer;

		// Token: 0x04001159 RID: 4441
		private DateTime m_IdleSince;

		// Token: 0x0400115A RID: 4442
		private string m_ConnectionName;

		// Token: 0x0400115B RID: 4443
		private int m_CurrentConnections;

		// Token: 0x0400115C RID: 4444
		private bool m_HostMode;

		// Token: 0x0400115D RID: 4445
		private BindIPEndPoint m_BindIPEndPointDelegate;

		// Token: 0x0400115E RID: 4446
		private object m_CachedChannelBinding;

		// Token: 0x0400115F RID: 4447
		private static readonly AsyncCallback m_ConnectCallbackDelegate = new AsyncCallback(ServicePoint.ConnectSocketCallback);

		// Token: 0x04001160 RID: 4448
		private readonly TimerThread.Callback m_IdleConnectionGroupTimeoutDelegate;

		// Token: 0x04001161 RID: 4449
		private readonly object m_ExpiringTimerLock = new object();

		// Token: 0x04001162 RID: 4450
		private object m_ServerCertificateOrBytes;

		// Token: 0x04001163 RID: 4451
		private object m_ClientCertificateOrBytes;

		// Token: 0x04001164 RID: 4452
		private bool m_UseTcpKeepAlive;

		// Token: 0x04001165 RID: 4453
		private int m_TcpKeepAliveTime;

		// Token: 0x04001166 RID: 4454
		private int m_TcpKeepAliveInterval;

		// Token: 0x04001167 RID: 4455
		private string m_HostName = string.Empty;

		// Token: 0x04001168 RID: 4456
		private bool m_IsTrustedHost = true;

		// Token: 0x04001169 RID: 4457
		private IPAddress[] m_IPAddressInfoList;

		// Token: 0x0400116A RID: 4458
		private int m_CurrentAddressInfoIndex;

		// Token: 0x0400116B RID: 4459
		private bool m_ConnectedSinceDns;

		// Token: 0x0400116C RID: 4460
		private bool m_AddressListFailed;

		// Token: 0x0400116D RID: 4461
		private DateTime m_LastDnsResolve;

		// Token: 0x0400116E RID: 4462
		private bool m_IPAddressesAreLoopback;

		// Token: 0x0200070F RID: 1807
		private class HandshakeDoneProcedure
		{
			// Token: 0x0600409C RID: 16540 RVA: 0x0010EE14 File Offset: 0x0010D014
			internal static RemoteCertValidationCallback CreateAdapter(ServicePoint serviePoint, TlsStream secureStream, object request)
			{
				ServicePoint.HandshakeDoneProcedure handshakeDoneProcedure = new ServicePoint.HandshakeDoneProcedure(serviePoint, secureStream, request);
				return new RemoteCertValidationCallback(handshakeDoneProcedure.CertValidationCallback);
			}

			// Token: 0x0600409D RID: 16541 RVA: 0x0010EE36 File Offset: 0x0010D036
			private HandshakeDoneProcedure(ServicePoint serviePoint, TlsStream secureStream, object request)
			{
				this.m_ServicePoint = serviePoint;
				this.m_SecureStream = secureStream;
				this.m_Request = request;
			}

			// Token: 0x0600409E RID: 16542 RVA: 0x0010EE54 File Offset: 0x0010D054
			private bool CertValidationCallback(string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				this.m_ServicePoint.UpdateServerCertificate(certificate);
				this.m_ServicePoint.UpdateClientCertificate(this.m_SecureStream.ClientCertificate);
				bool flag = true;
				HttpWebRequest httpWebRequest = this.m_Request as HttpWebRequest;
				if (httpWebRequest != null && httpWebRequest.ServerCertValidationCallback != null)
				{
					return httpWebRequest.ServerCertValidationCallback.Invoke(this.m_Request, certificate, chain, sslPolicyErrors);
				}
				if (ServicePointManager.GetLegacyCertificatePolicy() != null && this.m_Request is WebRequest)
				{
					flag = false;
					bool flag2 = ServicePointManager.CertPolicyValidationCallback.Invoke(hostName, this.m_ServicePoint, certificate, (WebRequest)this.m_Request, chain, sslPolicyErrors);
					if (!flag2 && (!ServicePointManager.CertPolicyValidationCallback.UsesDefault || ServicePointManager.ServerCertificateValidationCallback == null))
					{
						return flag2;
					}
				}
				if (ServicePointManager.ServerCertificateValidationCallback != null)
				{
					return ServicePointManager.ServerCertValidationCallback.Invoke(this.m_Request, certificate, chain, sslPolicyErrors);
				}
				return !flag || sslPolicyErrors == SslPolicyErrors.None;
			}

			// Token: 0x04003107 RID: 12551
			private TlsStream m_SecureStream;

			// Token: 0x04003108 RID: 12552
			private object m_Request;

			// Token: 0x04003109 RID: 12553
			private ServicePoint m_ServicePoint;
		}

		// Token: 0x02000710 RID: 1808
		private class ConnectSocketState
		{
			// Token: 0x0600409F RID: 16543 RVA: 0x0010EF29 File Offset: 0x0010D129
			internal ConnectSocketState(ServicePoint servicePoint, PooledStream pooledStream, object owner, Socket s4, Socket s6)
			{
				this.servicePoint = servicePoint;
				this.pooledStream = pooledStream;
				this.owner = owner;
				this.s4 = s4;
				this.s6 = s6;
			}

			// Token: 0x0400310A RID: 12554
			internal ServicePoint servicePoint;

			// Token: 0x0400310B RID: 12555
			internal Socket s4;

			// Token: 0x0400310C RID: 12556
			internal Socket s6;

			// Token: 0x0400310D RID: 12557
			internal object owner;

			// Token: 0x0400310E RID: 12558
			internal IPAddress[] addresses;

			// Token: 0x0400310F RID: 12559
			internal int currentIndex;

			// Token: 0x04003110 RID: 12560
			internal int i;

			// Token: 0x04003111 RID: 12561
			internal int unsuccessfulAttempts;

			// Token: 0x04003112 RID: 12562
			internal bool connectFailure;

			// Token: 0x04003113 RID: 12563
			internal PooledStream pooledStream;
		}
	}
}
