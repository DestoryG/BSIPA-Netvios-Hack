using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000162 RID: 354
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SocketPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x00044125 File Offset: 0x00042325
		public SocketPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x0004412E File Offset: 0x0004232E
		// (set) Token: 0x06000CCB RID: 3275 RVA: 0x00044136 File Offset: 0x00042336
		public string Access
		{
			get
			{
				return this.m_access;
			}
			set
			{
				if (this.m_access != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Access", value }), "value");
				}
				this.m_access = value;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0004416E File Offset: 0x0004236E
		// (set) Token: 0x06000CCD RID: 3277 RVA: 0x00044176 File Offset: 0x00042376
		public string Host
		{
			get
			{
				return this.m_host;
			}
			set
			{
				if (this.m_host != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Host", value }), "value");
				}
				this.m_host = value;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000441AE File Offset: 0x000423AE
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x000441B6 File Offset: 0x000423B6
		public string Transport
		{
			get
			{
				return this.m_transport;
			}
			set
			{
				if (this.m_transport != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Transport", value }), "value");
				}
				this.m_transport = value;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x000441EE File Offset: 0x000423EE
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x000441F6 File Offset: 0x000423F6
		public string Port
		{
			get
			{
				return this.m_port;
			}
			set
			{
				if (this.m_port != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Port", value }), "value");
				}
				this.m_port = value;
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00044230 File Offset: 0x00042430
		public override IPermission CreatePermission()
		{
			SocketPermission socketPermission;
			if (base.Unrestricted)
			{
				socketPermission = new SocketPermission(PermissionState.Unrestricted);
			}
			else
			{
				socketPermission = new SocketPermission(PermissionState.None);
				if (this.m_access == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[] { "Access" }));
				}
				if (this.m_host == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[] { "Host" }));
				}
				if (this.m_transport == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[] { "Transport" }));
				}
				if (this.m_port == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[] { "Port" }));
				}
				this.ParseAddPermissions(socketPermission);
			}
			return socketPermission;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000442FC File Offset: 0x000424FC
		private void ParseAddPermissions(SocketPermission perm)
		{
			NetworkAccess networkAccess;
			if (string.Compare(this.m_access, "Connect", StringComparison.OrdinalIgnoreCase) == 0)
			{
				networkAccess = NetworkAccess.Connect;
			}
			else
			{
				if (string.Compare(this.m_access, "Accept", StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Access", this.m_access }));
				}
				networkAccess = NetworkAccess.Accept;
			}
			TransportType transportType;
			try
			{
				transportType = (TransportType)Enum.Parse(typeof(TransportType), this.m_transport, true);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Transport", this.m_transport }), ex);
			}
			if (string.Compare(this.m_port, "All", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_port = "-1";
			}
			int num;
			try
			{
				num = int.Parse(this.m_port, NumberFormatInfo.InvariantInfo);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Port", this.m_port }), ex2);
			}
			if (!ValidationHelper.ValidateTcpPort(num) && num != -1)
			{
				throw new ArgumentOutOfRangeException("port", num, SR.GetString("net_perm_invalid_val", new object[] { "Port", this.m_port }));
			}
			perm.AddPermission(networkAccess, transportType, this.m_host, num);
		}

		// Token: 0x040011B8 RID: 4536
		private string m_access;

		// Token: 0x040011B9 RID: 4537
		private string m_host;

		// Token: 0x040011BA RID: 4538
		private string m_port;

		// Token: 0x040011BB RID: 4539
		private string m_transport;

		// Token: 0x040011BC RID: 4540
		private const string strAccess = "Access";

		// Token: 0x040011BD RID: 4541
		private const string strConnect = "Connect";

		// Token: 0x040011BE RID: 4542
		private const string strAccept = "Accept";

		// Token: 0x040011BF RID: 4543
		private const string strHost = "Host";

		// Token: 0x040011C0 RID: 4544
		private const string strTransport = "Transport";

		// Token: 0x040011C1 RID: 4545
		private const string strPort = "Port";
	}
}
