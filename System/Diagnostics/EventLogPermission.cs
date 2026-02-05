using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004D0 RID: 1232
	[Serializable]
	public sealed class EventLogPermission : ResourcePermissionBase
	{
		// Token: 0x06002E6C RID: 11884 RVA: 0x000D1815 File Offset: 0x000CFA15
		public EventLogPermission()
		{
			this.SetNames();
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000D1823 File Offset: 0x000CFA23
		public EventLogPermission(PermissionState state)
			: base(state)
		{
			this.SetNames();
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000D1832 File Offset: 0x000CFA32
		public EventLogPermission(EventLogPermissionAccess permissionAccess, string machineName)
		{
			this.SetNames();
			this.AddPermissionAccess(new EventLogPermissionEntry(permissionAccess, machineName));
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000D1850 File Offset: 0x000CFA50
		public EventLogPermission(EventLogPermissionEntry[] permissionAccessEntries)
		{
			if (permissionAccessEntries == null)
			{
				throw new ArgumentNullException("permissionAccessEntries");
			}
			this.SetNames();
			for (int i = 0; i < permissionAccessEntries.Length; i++)
			{
				this.AddPermissionAccess(permissionAccessEntries[i]);
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06002E70 RID: 11888 RVA: 0x000D188E File Offset: 0x000CFA8E
		public EventLogPermissionEntryCollection PermissionEntries
		{
			get
			{
				if (this.innerCollection == null)
				{
					this.innerCollection = new EventLogPermissionEntryCollection(this, base.GetPermissionEntries());
				}
				return this.innerCollection;
			}
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000D18B0 File Offset: 0x000CFAB0
		internal void AddPermissionAccess(EventLogPermissionEntry entry)
		{
			base.AddPermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x000D18BE File Offset: 0x000CFABE
		internal new void Clear()
		{
			base.Clear();
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000D18C6 File Offset: 0x000CFAC6
		internal void RemovePermissionAccess(EventLogPermissionEntry entry)
		{
			base.RemovePermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000D18D4 File Offset: 0x000CFAD4
		private void SetNames()
		{
			base.PermissionAccessType = typeof(EventLogPermissionAccess);
			base.TagNames = new string[] { "Machine" };
		}

		// Token: 0x04002768 RID: 10088
		private EventLogPermissionEntryCollection innerCollection;
	}
}
