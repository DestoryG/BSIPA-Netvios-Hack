using System;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x02000404 RID: 1028
	public class RenamedEventArgs : FileSystemEventArgs
	{
		// Token: 0x0600269B RID: 9883 RVA: 0x000B1D4D File Offset: 0x000AFF4D
		public RenamedEventArgs(WatcherChangeTypes changeType, string directory, string name, string oldName)
			: base(changeType, directory, name)
		{
			if (!directory.EndsWith("\\", StringComparison.Ordinal))
			{
				directory += "\\";
			}
			this.oldName = oldName;
			this.oldFullPath = directory + oldName;
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600269C RID: 9884 RVA: 0x000B1D89 File Offset: 0x000AFF89
		public string OldFullPath
		{
			get
			{
				new FileIOPermission(FileIOPermissionAccess.Read, Path.GetPathRoot(this.oldFullPath)).Demand();
				return this.oldFullPath;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x0600269D RID: 9885 RVA: 0x000B1DA7 File Offset: 0x000AFFA7
		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		// Token: 0x040020DB RID: 8411
		private string oldName;

		// Token: 0x040020DC RID: 8412
		private string oldFullPath;
	}
}
