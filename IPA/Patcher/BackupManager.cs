using System;
using System.IO;
using System.Linq;

namespace IPA.Patcher
{
	// Token: 0x02000007 RID: 7
	public static class BackupManager
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00003328 File Offset: 0x00001528
		public static BackupUnit FindLatestBackup(PatchContext context)
		{
			new DirectoryInfo(context.BackupPath).Create();
			return (from p in new DirectoryInfo(context.BackupPath).GetDirectories()
				orderby p.Name descending
				select BackupUnit.FromDirectory(p, context)).FirstOrDefault<BackupUnit>();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000033A6 File Offset: 0x000015A6
		public static bool HasBackup(PatchContext context)
		{
			return BackupManager.FindLatestBackup(context) != null;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000033B4 File Offset: 0x000015B4
		public static bool Restore(PatchContext context)
		{
			BackupUnit backupUnit = BackupManager.FindLatestBackup(context);
			if (backupUnit != null)
			{
				backupUnit.Restore();
				backupUnit.Delete();
				BackupManager.DeleteEmptyDirs(context.ProjectRoot);
				return true;
			}
			return false;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000033E8 File Offset: 0x000015E8
		public static void DeleteEmptyDirs(string dir)
		{
			if (string.IsNullOrEmpty(dir))
			{
				throw new ArgumentException("Starting directory is a null reference or an empty string", "dir");
			}
			try
			{
				foreach (string text in Directory.EnumerateDirectories(dir))
				{
					BackupManager.DeleteEmptyDirs(text);
				}
				if (!Directory.EnumerateFileSystemEntries(dir).Any<string>())
				{
					try
					{
						Directory.Delete(dir);
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (DirectoryNotFoundException)
					{
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
		}
	}
}
