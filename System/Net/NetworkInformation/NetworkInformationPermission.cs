using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002E2 RID: 738
	[Serializable]
	public sealed class NetworkInformationPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x060019F0 RID: 6640 RVA: 0x0007E2D6 File Offset: 0x0007C4D6
		public NetworkInformationPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.access = NetworkInformationAccess.Read | NetworkInformationAccess.Ping;
				this.unrestricted = true;
				return;
			}
			this.access = NetworkInformationAccess.None;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x0007E2F8 File Offset: 0x0007C4F8
		internal NetworkInformationPermission(bool unrestricted)
		{
			if (unrestricted)
			{
				this.access = NetworkInformationAccess.Read | NetworkInformationAccess.Ping;
				unrestricted = true;
				return;
			}
			this.access = NetworkInformationAccess.None;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0007E315 File Offset: 0x0007C515
		public NetworkInformationPermission(NetworkInformationAccess access)
		{
			this.access = access;
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x0007E324 File Offset: 0x0007C524
		public NetworkInformationAccess Access
		{
			get
			{
				return this.access;
			}
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0007E32C File Offset: 0x0007C52C
		public void AddPermission(NetworkInformationAccess access)
		{
			this.access |= access;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0007E33C File Offset: 0x0007C53C
		public bool IsUnrestricted()
		{
			return this.unrestricted;
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0007E344 File Offset: 0x0007C544
		public override IPermission Copy()
		{
			if (this.unrestricted)
			{
				return new NetworkInformationPermission(true);
			}
			return new NetworkInformationPermission(this.access);
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0007E360 File Offset: 0x0007C560
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			NetworkInformationPermission networkInformationPermission = target as NetworkInformationPermission;
			if (networkInformationPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.unrestricted || networkInformationPermission.IsUnrestricted())
			{
				return new NetworkInformationPermission(true);
			}
			return new NetworkInformationPermission(this.access | networkInformationPermission.access);
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0007E3C0 File Offset: 0x0007C5C0
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			NetworkInformationPermission networkInformationPermission = target as NetworkInformationPermission;
			if (networkInformationPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.unrestricted && networkInformationPermission.IsUnrestricted())
			{
				return new NetworkInformationPermission(true);
			}
			return new NetworkInformationPermission(this.access & networkInformationPermission.access);
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0007E41C File Offset: 0x0007C61C
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.access == NetworkInformationAccess.None;
			}
			NetworkInformationPermission networkInformationPermission = target as NetworkInformationPermission;
			if (networkInformationPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return (!this.unrestricted || networkInformationPermission.IsUnrestricted()) && (this.access & networkInformationPermission.access) == this.access;
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0007E480 File Offset: 0x0007C680
		public override void FromXml(SecurityElement securityElement)
		{
			this.access = NetworkInformationAccess.None;
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("net_not_ipermission"), "securityElement");
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
			if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.access = NetworkInformationAccess.Read | NetworkInformationAccess.Ping;
				this.unrestricted = true;
				return;
			}
			if (securityElement.Children != null)
			{
				foreach (object obj in securityElement.Children)
				{
					SecurityElement securityElement2 = (SecurityElement)obj;
					text2 = securityElement2.Attribute("Access");
					if (string.Compare(text2, "Read", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.access |= NetworkInformationAccess.Read;
					}
					else if (string.Compare(text2, "Ping", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.access |= NetworkInformationAccess.Ping;
					}
				}
			}
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0007E5D8 File Offset: 0x0007C7D8
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
				return securityElement;
			}
			if ((this.access & NetworkInformationAccess.Read) > NetworkInformationAccess.None)
			{
				SecurityElement securityElement2 = new SecurityElement("NetworkInformationAccess");
				securityElement2.AddAttribute("Access", "Read");
				securityElement.AddChild(securityElement2);
			}
			if ((this.access & NetworkInformationAccess.Ping) > NetworkInformationAccess.None)
			{
				SecurityElement securityElement3 = new SecurityElement("NetworkInformationAccess");
				securityElement3.AddAttribute("Access", "Ping");
				securityElement.AddChild(securityElement3);
			}
			return securityElement;
		}

		// Token: 0x04001A4D RID: 6733
		private NetworkInformationAccess access;

		// Token: 0x04001A4E RID: 6734
		private bool unrestricted;
	}
}
