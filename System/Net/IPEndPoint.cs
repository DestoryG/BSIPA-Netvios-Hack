using System;
using System.Globalization;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x0200014C RID: 332
	[global::__DynamicallyInvokable]
	[Serializable]
	public class IPEndPoint : EndPoint
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0003F317 File Offset: 0x0003D517
		[global::__DynamicallyInvokable]
		public override AddressFamily AddressFamily
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Address.AddressFamily;
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0003F324 File Offset: 0x0003D524
		[global::__DynamicallyInvokable]
		public IPEndPoint(long address, int port)
		{
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_Port = port;
			this.m_Address = new IPAddress(address);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0003F352 File Offset: 0x0003D552
		[global::__DynamicallyInvokable]
		public IPEndPoint(IPAddress address, int port)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_Port = port;
			this.m_Address = address;
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0003F389 File Offset: 0x0003D589
		// (set) Token: 0x06000B96 RID: 2966 RVA: 0x0003F391 File Offset: 0x0003D591
		[global::__DynamicallyInvokable]
		public IPAddress Address
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Address;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_Address = value;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0003F39A File Offset: 0x0003D59A
		// (set) Token: 0x06000B98 RID: 2968 RVA: 0x0003F3A2 File Offset: 0x0003D5A2
		[global::__DynamicallyInvokable]
		public int Port
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Port;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (!ValidationHelper.ValidateTcpPort(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_Port = value;
			}
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0003F3C0 File Offset: 0x0003D5C0
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			string text;
			if (this.m_Address.AddressFamily == AddressFamily.InterNetworkV6)
			{
				text = "[{0}]:{1}";
			}
			else
			{
				text = "{0}:{1}";
			}
			return string.Format(text, this.m_Address.ToString(), this.Port.ToString(NumberFormatInfo.InvariantInfo));
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0003F40E File Offset: 0x0003D60E
		[global::__DynamicallyInvokable]
		public override SocketAddress Serialize()
		{
			return new SocketAddress(this.Address, this.Port);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0003F424 File Offset: 0x0003D624
		[global::__DynamicallyInvokable]
		public override EndPoint Create(SocketAddress socketAddress)
		{
			if (socketAddress.Family != this.AddressFamily)
			{
				throw new ArgumentException(SR.GetString("net_InvalidAddressFamily", new object[]
				{
					socketAddress.Family.ToString(),
					base.GetType().FullName,
					this.AddressFamily.ToString()
				}), "socketAddress");
			}
			if (socketAddress.Size < 8)
			{
				throw new ArgumentException(SR.GetString("net_InvalidSocketAddressSize", new object[]
				{
					socketAddress.GetType().FullName,
					base.GetType().FullName
				}), "socketAddress");
			}
			return socketAddress.GetIPEndPoint();
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0003F4DC File Offset: 0x0003D6DC
		[global::__DynamicallyInvokable]
		public override bool Equals(object comparand)
		{
			return comparand is IPEndPoint && ((IPEndPoint)comparand).m_Address.Equals(this.m_Address) && ((IPEndPoint)comparand).m_Port == this.m_Port;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0003F515 File Offset: 0x0003D715
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			return this.m_Address.GetHashCode() ^ this.m_Port;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0003F529 File Offset: 0x0003D729
		internal IPEndPoint Snapshot()
		{
			return new IPEndPoint(this.Address.Snapshot(), this.Port);
		}

		// Token: 0x04001101 RID: 4353
		[global::__DynamicallyInvokable]
		public const int MinPort = 0;

		// Token: 0x04001102 RID: 4354
		[global::__DynamicallyInvokable]
		public const int MaxPort = 65535;

		// Token: 0x04001103 RID: 4355
		private IPAddress m_Address;

		// Token: 0x04001104 RID: 4356
		private int m_Port;

		// Token: 0x04001105 RID: 4357
		internal const int AnyPort = 0;

		// Token: 0x04001106 RID: 4358
		internal static IPEndPoint Any = new IPEndPoint(IPAddress.Any, 0);

		// Token: 0x04001107 RID: 4359
		internal static IPEndPoint IPv6Any = new IPEndPoint(IPAddress.IPv6Any, 0);
	}
}
