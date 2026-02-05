using System;

namespace System.Security.Permissions
{
	// Token: 0x0200048B RID: 1163
	[Serializable]
	public class ResourcePermissionBaseEntry
	{
		// Token: 0x06002B27 RID: 11047 RVA: 0x000C47FD File Offset: 0x000C29FD
		public ResourcePermissionBaseEntry()
		{
			this.permissionAccess = 0;
			this.accessPath = new string[0];
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000C4818 File Offset: 0x000C2A18
		public ResourcePermissionBaseEntry(int permissionAccess, string[] permissionAccessPath)
		{
			if (permissionAccessPath == null)
			{
				throw new ArgumentNullException("permissionAccessPath");
			}
			this.permissionAccess = permissionAccess;
			this.accessPath = permissionAccessPath;
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06002B29 RID: 11049 RVA: 0x000C483C File Offset: 0x000C2A3C
		public int PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x000C4844 File Offset: 0x000C2A44
		public string[] PermissionAccessPath
		{
			get
			{
				return this.accessPath;
			}
		}

		// Token: 0x04002669 RID: 9833
		private string[] accessPath;

		// Token: 0x0400266A RID: 9834
		private int permissionAccess;
	}
}
