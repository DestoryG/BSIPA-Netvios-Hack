using System;

namespace System.IO
{
	// Token: 0x020003FD RID: 1021
	public class FileSystemEventArgs : EventArgs
	{
		// Token: 0x06002657 RID: 9815 RVA: 0x000B0A85 File Offset: 0x000AEC85
		public FileSystemEventArgs(WatcherChangeTypes changeType, string directory, string name)
		{
			this.changeType = changeType;
			this.name = name;
			if (!directory.EndsWith("\\", StringComparison.Ordinal))
			{
				directory += "\\";
			}
			this.fullPath = directory + name;
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x000B0AC3 File Offset: 0x000AECC3
		public WatcherChangeTypes ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x000B0ACB File Offset: 0x000AECCB
		public string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x0600265A RID: 9818 RVA: 0x000B0AD3 File Offset: 0x000AECD3
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x040020AD RID: 8365
		private WatcherChangeTypes changeType;

		// Token: 0x040020AE RID: 8366
		private string name;

		// Token: 0x040020AF RID: 8367
		private string fullPath;
	}
}
