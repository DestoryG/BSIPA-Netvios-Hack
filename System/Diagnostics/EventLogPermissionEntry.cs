using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004D3 RID: 1235
	[Serializable]
	public class EventLogPermissionEntry
	{
		// Token: 0x06002E7B RID: 11899 RVA: 0x000D1984 File Offset: 0x000CFB84
		public EventLogPermissionEntry(EventLogPermissionAccess permissionAccess, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "MachineName", machineName }));
			}
			this.permissionAccess = permissionAccess;
			this.machineName = machineName;
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000D19C4 File Offset: 0x000CFBC4
		internal EventLogPermissionEntry(ResourcePermissionBaseEntry baseEntry)
		{
			this.permissionAccess = (EventLogPermissionAccess)baseEntry.PermissionAccess;
			this.machineName = baseEntry.PermissionAccessPath[0];
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06002E7D RID: 11901 RVA: 0x000D19E6 File Offset: 0x000CFBE6
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06002E7E RID: 11902 RVA: 0x000D19EE File Offset: 0x000CFBEE
		public EventLogPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000D19F8 File Offset: 0x000CFBF8
		internal ResourcePermissionBaseEntry GetBaseEntry()
		{
			return new ResourcePermissionBaseEntry((int)this.PermissionAccess, new string[] { this.MachineName });
		}

		// Token: 0x04002772 RID: 10098
		private string machineName;

		// Token: 0x04002773 RID: 10099
		private EventLogPermissionAccess permissionAccess;
	}
}
