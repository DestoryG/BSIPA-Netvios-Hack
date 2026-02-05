using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020000E2 RID: 226
	[Serializable]
	public sealed class DnsPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x060007C0 RID: 1984 RVA: 0x0002B24F File Offset: 0x0002944F
		public DnsPermission(PermissionState state)
		{
			this.m_noRestriction = state == PermissionState.Unrestricted;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0002B261 File Offset: 0x00029461
		internal DnsPermission(bool free)
		{
			this.m_noRestriction = free;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0002B270 File Offset: 0x00029470
		public bool IsUnrestricted()
		{
			return this.m_noRestriction;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002B278 File Offset: 0x00029478
		public override IPermission Copy()
		{
			return new DnsPermission(this.m_noRestriction);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002B288 File Offset: 0x00029488
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return new DnsPermission(this.m_noRestriction || dnsPermission.m_noRestriction);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0002B2D4 File Offset: 0x000294D4
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.m_noRestriction && dnsPermission.m_noRestriction)
			{
				return new DnsPermission(true);
			}
			return null;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002B320 File Offset: 0x00029520
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return !this.m_noRestriction;
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return !this.m_noRestriction || dnsPermission.m_noRestriction;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002B36C File Offset: 0x0002956C
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("net_no_classname"), "securityElement");
			}
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(SR.GetString("net_no_classname"), "securityElement");
			}
			if (text.IndexOf(base.GetType().FullName) < 0)
			{
				throw new ArgumentException(SR.GetString("net_no_typename"), "securityElement");
			}
			string text2 = securityElement.Attribute("Unrestricted");
			this.m_noRestriction = text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002B424 File Offset: 0x00029624
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.m_noRestriction)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x04000D30 RID: 3376
		private bool m_noRestriction;
	}
}
