using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004EC RID: 1260
	[Serializable]
	public class PerformanceCounterPermissionEntry
	{
		// Token: 0x06002F88 RID: 12168 RVA: 0x000D6D0C File Offset: 0x000D4F0C
		public PerformanceCounterPermissionEntry(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if ((permissionAccess & (PerformanceCounterPermissionAccess)(-8)) != PerformanceCounterPermissionAccess.None)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "permissionAccess", permissionAccess }));
			}
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "MachineName", machineName }));
			}
			this.permissionAccess = permissionAccess;
			this.machineName = machineName;
			this.categoryName = categoryName;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000D6DA7 File Offset: 0x000D4FA7
		internal PerformanceCounterPermissionEntry(ResourcePermissionBaseEntry baseEntry)
		{
			this.permissionAccess = (PerformanceCounterPermissionAccess)baseEntry.PermissionAccess;
			this.machineName = baseEntry.PermissionAccessPath[0];
			this.categoryName = baseEntry.PermissionAccessPath[1];
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x000D6DD7 File Offset: 0x000D4FD7
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x000D6DDF File Offset: 0x000D4FDF
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x000D6DE7 File Offset: 0x000D4FE7
		public PerformanceCounterPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000D6DF0 File Offset: 0x000D4FF0
		internal ResourcePermissionBaseEntry GetBaseEntry()
		{
			return new ResourcePermissionBaseEntry((int)this.PermissionAccess, new string[] { this.MachineName, this.CategoryName });
		}

		// Token: 0x04002803 RID: 10243
		private string categoryName;

		// Token: 0x04002804 RID: 10244
		private string machineName;

		// Token: 0x04002805 RID: 10245
		private PerformanceCounterPermissionAccess permissionAccess;
	}
}
