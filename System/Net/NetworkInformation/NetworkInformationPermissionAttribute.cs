using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002E1 RID: 737
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class NetworkInformationPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060019EC RID: 6636 RVA: 0x0007E20E File Offset: 0x0007C40E
		public NetworkInformationPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x0007E217 File Offset: 0x0007C417
		// (set) Token: 0x060019EE RID: 6638 RVA: 0x0007E21F File Offset: 0x0007C41F
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

		// Token: 0x060019EF RID: 6639 RVA: 0x0007E228 File Offset: 0x0007C428
		public override IPermission CreatePermission()
		{
			NetworkInformationPermission networkInformationPermission;
			if (base.Unrestricted)
			{
				networkInformationPermission = new NetworkInformationPermission(PermissionState.Unrestricted);
			}
			else
			{
				networkInformationPermission = new NetworkInformationPermission(PermissionState.None);
				if (this.access != null)
				{
					if (string.Compare(this.access, "Read", StringComparison.OrdinalIgnoreCase) == 0)
					{
						networkInformationPermission.AddPermission(NetworkInformationAccess.Read);
					}
					else if (string.Compare(this.access, "Ping", StringComparison.OrdinalIgnoreCase) == 0)
					{
						networkInformationPermission.AddPermission(NetworkInformationAccess.Ping);
					}
					else
					{
						if (string.Compare(this.access, "None", StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Access", this.access }));
						}
						networkInformationPermission.AddPermission(NetworkInformationAccess.None);
					}
				}
			}
			return networkInformationPermission;
		}

		// Token: 0x04001A4B RID: 6731
		private const string strAccess = "Access";

		// Token: 0x04001A4C RID: 6732
		private string access;
	}
}
