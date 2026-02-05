using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x02000185 RID: 389
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class WebPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x0004B862 File Offset: 0x00049A62
		public WebPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0004B86B File Offset: 0x00049A6B
		// (set) Token: 0x06000E76 RID: 3702 RVA: 0x0004B878 File Offset: 0x00049A78
		public string Connect
		{
			get
			{
				return this.m_connect as string;
			}
			set
			{
				if (this.m_connect != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Connect", value }), "value");
				}
				this.m_connect = value;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0004B8B0 File Offset: 0x00049AB0
		// (set) Token: 0x06000E78 RID: 3704 RVA: 0x0004B8BD File Offset: 0x00049ABD
		public string Accept
		{
			get
			{
				return this.m_accept as string;
			}
			set
			{
				if (this.m_accept != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Accept", value }), "value");
				}
				this.m_accept = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0004B8F5 File Offset: 0x00049AF5
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x0004B934 File Offset: 0x00049B34
		public string ConnectPattern
		{
			get
			{
				if (this.m_connect is DelayedRegex)
				{
					return this.m_connect.ToString();
				}
				if (!(this.m_connect is bool) || !(bool)this.m_connect)
				{
					return null;
				}
				return ".*";
			}
			set
			{
				if (this.m_connect != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "ConnectPatern", value }), "value");
				}
				if (value == ".*")
				{
					this.m_connect = true;
					return;
				}
				this.m_connect = new DelayedRegex(value);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0004B996 File Offset: 0x00049B96
		// (set) Token: 0x06000E7C RID: 3708 RVA: 0x0004B9D4 File Offset: 0x00049BD4
		public string AcceptPattern
		{
			get
			{
				if (this.m_accept is DelayedRegex)
				{
					return this.m_accept.ToString();
				}
				if (!(this.m_accept is bool) || !(bool)this.m_accept)
				{
					return null;
				}
				return ".*";
			}
			set
			{
				if (this.m_accept != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "AcceptPattern", value }), "value");
				}
				if (value == ".*")
				{
					this.m_accept = true;
					return;
				}
				this.m_accept = new DelayedRegex(value);
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0004BA38 File Offset: 0x00049C38
		public override IPermission CreatePermission()
		{
			WebPermission webPermission;
			if (base.Unrestricted)
			{
				webPermission = new WebPermission(PermissionState.Unrestricted);
			}
			else
			{
				NetworkAccess networkAccess = (NetworkAccess)0;
				if (this.m_connect is bool)
				{
					if ((bool)this.m_connect)
					{
						networkAccess |= NetworkAccess.Connect;
					}
					this.m_connect = null;
				}
				if (this.m_accept is bool)
				{
					if ((bool)this.m_accept)
					{
						networkAccess |= NetworkAccess.Accept;
					}
					this.m_accept = null;
				}
				webPermission = new WebPermission(networkAccess);
				if (this.m_accept != null)
				{
					if (this.m_accept is DelayedRegex)
					{
						webPermission.AddAsPattern(NetworkAccess.Accept, (DelayedRegex)this.m_accept);
					}
					else
					{
						webPermission.AddPermission(NetworkAccess.Accept, (string)this.m_accept);
					}
				}
				if (this.m_connect != null)
				{
					if (this.m_connect is DelayedRegex)
					{
						webPermission.AddAsPattern(NetworkAccess.Connect, (DelayedRegex)this.m_connect);
					}
					else
					{
						webPermission.AddPermission(NetworkAccess.Connect, (string)this.m_connect);
					}
				}
			}
			return webPermission;
		}

		// Token: 0x0400126D RID: 4717
		private object m_accept;

		// Token: 0x0400126E RID: 4718
		private object m_connect;
	}
}
