using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004E9 RID: 1257
	[Serializable]
	public sealed class PerformanceCounterPermission : ResourcePermissionBase
	{
		// Token: 0x06002F77 RID: 12151 RVA: 0x000D6B67 File Offset: 0x000D4D67
		public PerformanceCounterPermission()
		{
			this.SetNames();
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000D6B75 File Offset: 0x000D4D75
		public PerformanceCounterPermission(PermissionState state)
			: base(state)
		{
			this.SetNames();
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000D6B84 File Offset: 0x000D4D84
		public PerformanceCounterPermission(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName)
		{
			this.SetNames();
			this.AddPermissionAccess(new PerformanceCounterPermissionEntry(permissionAccess, machineName, categoryName));
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x000D6BA0 File Offset: 0x000D4DA0
		public PerformanceCounterPermission(PerformanceCounterPermissionEntry[] permissionAccessEntries)
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

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06002F7B RID: 12155 RVA: 0x000D6BDE File Offset: 0x000D4DDE
		public PerformanceCounterPermissionEntryCollection PermissionEntries
		{
			get
			{
				if (this.innerCollection == null)
				{
					this.innerCollection = new PerformanceCounterPermissionEntryCollection(this, base.GetPermissionEntries());
				}
				return this.innerCollection;
			}
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000D6C00 File Offset: 0x000D4E00
		internal void AddPermissionAccess(PerformanceCounterPermissionEntry entry)
		{
			base.AddPermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000D6C0E File Offset: 0x000D4E0E
		internal new void Clear()
		{
			base.Clear();
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000D6C16 File Offset: 0x000D4E16
		internal void RemovePermissionAccess(PerformanceCounterPermissionEntry entry)
		{
			base.RemovePermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000D6C24 File Offset: 0x000D4E24
		private void SetNames()
		{
			base.PermissionAccessType = typeof(PerformanceCounterPermissionAccess);
			base.TagNames = new string[] { "Machine", "Category" };
		}

		// Token: 0x040027F8 RID: 10232
		private PerformanceCounterPermissionEntryCollection innerCollection;
	}
}
