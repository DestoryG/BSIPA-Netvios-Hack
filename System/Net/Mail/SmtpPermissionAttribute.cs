using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x02000291 RID: 657
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SmtpPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06001876 RID: 6262 RVA: 0x0007C599 File Offset: 0x0007A799
		public SmtpPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x0007C5A2 File Offset: 0x0007A7A2
		// (set) Token: 0x06001878 RID: 6264 RVA: 0x0007C5AA File Offset: 0x0007A7AA
		public string Access
		{
			get
			{
				return this.access;
			}
			set
			{
				this.access = value;
			}
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0007C5B4 File Offset: 0x0007A7B4
		public override IPermission CreatePermission()
		{
			SmtpPermission smtpPermission;
			if (base.Unrestricted)
			{
				smtpPermission = new SmtpPermission(PermissionState.Unrestricted);
			}
			else
			{
				smtpPermission = new SmtpPermission(PermissionState.None);
				if (this.access != null)
				{
					if (string.Compare(this.access, "Connect", StringComparison.OrdinalIgnoreCase) == 0)
					{
						smtpPermission.AddPermission(SmtpAccess.Connect);
					}
					else if (string.Compare(this.access, "ConnectToUnrestrictedPort", StringComparison.OrdinalIgnoreCase) == 0)
					{
						smtpPermission.AddPermission(SmtpAccess.ConnectToUnrestrictedPort);
					}
					else
					{
						if (string.Compare(this.access, "None", StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Access", this.access }));
						}
						smtpPermission.AddPermission(SmtpAccess.None);
					}
				}
			}
			return smtpPermission;
		}

		// Token: 0x04001859 RID: 6233
		private const string strAccess = "Access";

		// Token: 0x0400185A RID: 6234
		private string access;
	}
}
