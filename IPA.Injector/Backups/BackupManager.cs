using System;
using System.IO;
using System.Linq;

namespace IPA.Injector.Backups
{
	// Token: 0x02000009 RID: 9
	internal static class BackupManager
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00003350 File Offset: 0x00001550
		public static BackupUnit FindLatestBackup(string dir)
		{
			new DirectoryInfo(dir).Create();
			return (from p in new DirectoryInfo(dir).GetDirectories()
				orderby p.Name descending
				select BackupUnit.FromDirectory(p, dir)).FirstOrDefault<BackupUnit>();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000033C4 File Offset: 0x000015C4
		public static bool HasBackup(string dir)
		{
			return BackupManager.FindLatestBackup(dir) != null;
		}
	}
}
