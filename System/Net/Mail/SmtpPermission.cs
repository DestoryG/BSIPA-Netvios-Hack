using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x02000292 RID: 658
	[Serializable]
	public sealed class SmtpPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x0600187A RID: 6266 RVA: 0x0007C662 File Offset: 0x0007A862
		public SmtpPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.access = SmtpAccess.ConnectToUnrestrictedPort;
				this.unrestricted = true;
				return;
			}
			this.access = SmtpAccess.None;
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0007C684 File Offset: 0x0007A884
		public SmtpPermission(bool unrestricted)
		{
			if (unrestricted)
			{
				this.access = SmtpAccess.ConnectToUnrestrictedPort;
				this.unrestricted = true;
				return;
			}
			this.access = SmtpAccess.None;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0007C6A5 File Offset: 0x0007A8A5
		public SmtpPermission(SmtpAccess access)
		{
			this.access = access;
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x0007C6B4 File Offset: 0x0007A8B4
		public SmtpAccess Access
		{
			get
			{
				return this.access;
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0007C6BC File Offset: 0x0007A8BC
		public void AddPermission(SmtpAccess access)
		{
			if (access > this.access)
			{
				this.access = access;
			}
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x0007C6CE File Offset: 0x0007A8CE
		public bool IsUnrestricted()
		{
			return this.unrestricted;
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0007C6D6 File Offset: 0x0007A8D6
		public override IPermission Copy()
		{
			if (this.unrestricted)
			{
				return new SmtpPermission(true);
			}
			return new SmtpPermission(this.access);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0007C6F4 File Offset: 0x0007A8F4
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			SmtpPermission smtpPermission = target as SmtpPermission;
			if (smtpPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.unrestricted || smtpPermission.IsUnrestricted())
			{
				return new SmtpPermission(true);
			}
			return new SmtpPermission((this.access > smtpPermission.access) ? this.access : smtpPermission.access);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0007C764 File Offset: 0x0007A964
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			SmtpPermission smtpPermission = target as SmtpPermission;
			if (smtpPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.IsUnrestricted() && smtpPermission.IsUnrestricted())
			{
				return new SmtpPermission(true);
			}
			return new SmtpPermission((this.access < smtpPermission.access) ? this.access : smtpPermission.access);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x0007C7D0 File Offset: 0x0007A9D0
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.access == SmtpAccess.None;
			}
			SmtpPermission smtpPermission = target as SmtpPermission;
			if (smtpPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return (!this.unrestricted || smtpPermission.IsUnrestricted()) && smtpPermission.access >= this.access;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0007C82C File Offset: 0x0007AA2C
		public override void FromXml(SecurityElement securityElement)
		{
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
				this.access = SmtpAccess.ConnectToUnrestrictedPort;
				this.unrestricted = true;
				return;
			}
			text2 = securityElement.Attribute("Access");
			if (text2 == null)
			{
				return;
			}
			if (string.Compare(text2, "Connect", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.access = SmtpAccess.Connect;
				return;
			}
			if (string.Compare(text2, "ConnectToUnrestrictedPort", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.access = SmtpAccess.ConnectToUnrestrictedPort;
				return;
			}
			if (string.Compare(text2, "None", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.access = SmtpAccess.None;
				return;
			}
			throw new ArgumentException(SR.GetString("net_perm_invalid_val_in_element"), "Access");
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0007C950 File Offset: 0x0007AB50
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
			if (this.access == SmtpAccess.Connect)
			{
				securityElement.AddAttribute("Access", "Connect");
			}
			else if (this.access == SmtpAccess.ConnectToUnrestrictedPort)
			{
				securityElement.AddAttribute("Access", "ConnectToUnrestrictedPort");
			}
			return securityElement;
		}

		// Token: 0x0400185B RID: 6235
		private SmtpAccess access;

		// Token: 0x0400185C RID: 6236
		private bool unrestricted;
	}
}
