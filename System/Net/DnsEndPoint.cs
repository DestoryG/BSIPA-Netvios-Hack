using System;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x020000E0 RID: 224
	[global::__DynamicallyInvokable]
	public class DnsEndPoint : EndPoint
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x0002B0C7 File Offset: 0x000292C7
		[global::__DynamicallyInvokable]
		public DnsEndPoint(string host, int port)
			: this(host, port, AddressFamily.Unspecified)
		{
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002B0D4 File Offset: 0x000292D4
		[global::__DynamicallyInvokable]
		public DnsEndPoint(string host, int port, AddressFamily addressFamily)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (string.IsNullOrEmpty(host))
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "host" }));
			}
			if (port < 0 || port > 65535)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (addressFamily != AddressFamily.InterNetwork && addressFamily != AddressFamily.InterNetworkV6 && addressFamily != AddressFamily.Unspecified)
			{
				throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue_all"), "addressFamily");
			}
			this.m_Host = host;
			this.m_Port = port;
			this.m_Family = addressFamily;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002B168 File Offset: 0x00029368
		[global::__DynamicallyInvokable]
		public override bool Equals(object comparand)
		{
			DnsEndPoint dnsEndPoint = comparand as DnsEndPoint;
			return dnsEndPoint != null && (this.m_Family == dnsEndPoint.m_Family && this.m_Port == dnsEndPoint.m_Port) && this.m_Host == dnsEndPoint.m_Host;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0002B1B0 File Offset: 0x000293B0
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			return StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.ToString());
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0002B1C4 File Offset: 0x000293C4
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.m_Family.ToString(),
				"/",
				this.m_Host,
				":",
				this.m_Port.ToString()
			});
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0002B217 File Offset: 0x00029417
		[global::__DynamicallyInvokable]
		public string Host
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Host;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0002B21F File Offset: 0x0002941F
		[global::__DynamicallyInvokable]
		public override AddressFamily AddressFamily
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Family;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0002B227 File Offset: 0x00029427
		[global::__DynamicallyInvokable]
		public int Port
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Port;
			}
		}

		// Token: 0x04000D2D RID: 3373
		private string m_Host;

		// Token: 0x04000D2E RID: 3374
		private int m_Port;

		// Token: 0x04000D2F RID: 3375
		private AddressFamily m_Family;
	}
}
