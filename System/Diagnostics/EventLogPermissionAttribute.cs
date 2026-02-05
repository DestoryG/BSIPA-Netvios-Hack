using System;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004D2 RID: 1234
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public class EventLogPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002E75 RID: 11893 RVA: 0x000D18FA File Offset: 0x000CFAFA
		public EventLogPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this.machineName = ".";
			this.permissionAccess = EventLogPermissionAccess.Write;
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06002E76 RID: 11894 RVA: 0x000D1916 File Offset: 0x000CFB16
		// (set) Token: 0x06002E77 RID: 11895 RVA: 0x000D191E File Offset: 0x000CFB1E
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[] { "MachineName", value }));
				}
				this.machineName = value;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06002E78 RID: 11896 RVA: 0x000D1951 File Offset: 0x000CFB51
		// (set) Token: 0x06002E79 RID: 11897 RVA: 0x000D1959 File Offset: 0x000CFB59
		public EventLogPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
			set
			{
				this.permissionAccess = value;
			}
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000D1962 File Offset: 0x000CFB62
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new EventLogPermission(PermissionState.Unrestricted);
			}
			return new EventLogPermission(this.PermissionAccess, this.MachineName);
		}

		// Token: 0x04002770 RID: 10096
		private string machineName;

		// Token: 0x04002771 RID: 10097
		private EventLogPermissionAccess permissionAccess;
	}
}
