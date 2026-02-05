using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IPA.Utilities;

namespace CameraPlus
{
	// Token: 0x02000004 RID: 4
	public static class CameraProfiles
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002474 File Offset: 0x00000674
		public static void CreateMainDirectory()
		{
			Directory.CreateDirectory(CameraProfiles.pPath).Attributes = FileAttributes.Hidden | FileAttributes.Directory;
			Directory.CreateDirectory(Path.Combine(CameraProfiles.pPath, "Profiles"));
			DirectoryInfo[] directories = new DirectoryInfo(Path.Combine(CameraProfiles.pPath, "Profiles")).GetDirectories();
			if (directories.Length != 0)
			{
				CameraProfiles.currentlySelected = directories.First<DirectoryInfo>().Name;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024D5 File Offset: 0x000006D5
		public static void SaveCurrent()
		{
			CameraProfiles.DirectoryCopy(CameraProfiles.mPath, Path.Combine(CameraProfiles.pPath, "Profiles", CameraProfiles.GetNextProfileName()), true);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024F8 File Offset: 0x000006F8
		public static void SetNext(string now = null)
		{
			DirectoryInfo[] directories = new DirectoryInfo(Path.Combine(CameraProfiles.pPath, "Profiles")).GetDirectories();
			if (now == null)
			{
				CameraProfiles.currentlySelected = "None";
				if (directories.Length != 0)
				{
					CameraProfiles.currentlySelected = directories.First<DirectoryInfo>().Name;
				}
				return;
			}
			IEnumerable<DirectoryInfo> enumerable = directories.Where((DirectoryInfo x) => x.Name == now);
			if (enumerable.Count<DirectoryInfo>() <= 0)
			{
				CameraProfiles.currentlySelected = "None";
				if (directories.Length != 0)
				{
					CameraProfiles.currentlySelected = directories.First<DirectoryInfo>().Name;
				}
				return;
			}
			int num = directories.ToList<DirectoryInfo>().IndexOf(enumerable.First<DirectoryInfo>());
			if (num < directories.Count<DirectoryInfo>() - 1)
			{
				CameraProfiles.currentlySelected = directories.ElementAtOrDefault(num + 1).Name;
				return;
			}
			CameraProfiles.currentlySelected = directories.ElementAtOrDefault(0).Name;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000025D4 File Offset: 0x000007D4
		public static void TrySetLast(string now = null)
		{
			DirectoryInfo[] directories = new DirectoryInfo(Path.Combine(CameraProfiles.pPath, "Profiles")).GetDirectories();
			if (now == null)
			{
				CameraProfiles.currentlySelected = "None";
				if (directories.Length != 0)
				{
					CameraProfiles.currentlySelected = directories.First<DirectoryInfo>().Name;
				}
				return;
			}
			IEnumerable<DirectoryInfo> enumerable = directories.Where((DirectoryInfo x) => x.Name == now);
			if (enumerable.Count<DirectoryInfo>() <= 0)
			{
				CameraProfiles.currentlySelected = "None";
				if (directories.Length != 0)
				{
					CameraProfiles.currentlySelected = directories.First<DirectoryInfo>().Name;
				}
				return;
			}
			int num = directories.ToList<DirectoryInfo>().IndexOf(enumerable.First<DirectoryInfo>());
			if (num == 0 && directories.Length >= 2)
			{
				CameraProfiles.currentlySelected = directories.ElementAtOrDefault(directories.Count<DirectoryInfo>() - 1).Name;
				return;
			}
			if (num < directories.Count<DirectoryInfo>() && directories.Length >= 2)
			{
				CameraProfiles.currentlySelected = directories.ElementAtOrDefault(num - 1).Name;
				return;
			}
			CameraProfiles.currentlySelected = directories.ElementAtOrDefault(0).Name;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026D4 File Offset: 0x000008D4
		public static void DeleteProfile(string name)
		{
			if (Directory.Exists(Path.Combine(CameraProfiles.pPath, "Profiles", name)))
			{
				Directory.Delete(Path.Combine(CameraProfiles.pPath, "Profiles", name), true);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002704 File Offset: 0x00000904
		public static string GetNextProfileName()
		{
			DirectoryInfo[] directories = new DirectoryInfo(Path.Combine(CameraProfiles.pPath, "Profiles")).GetDirectories();
			int num = 1;
			string text = "CameraPlusProfile";
			foreach (DirectoryInfo directoryInfo in directories)
			{
				text = "CameraPlusProfile" + num.ToString();
				num++;
			}
			return text;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000275C File Offset: 0x0000095C
		public static void SetProfile(string name)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(CameraProfiles.pPath, "Profiles", name));
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo directoryInfo2 = new DirectoryInfo(CameraProfiles.mPath);
			FileInfo[] files = directoryInfo2.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				files[i].Delete();
			}
			DirectoryInfo[] directories = directoryInfo2.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i].Delete(true);
			}
			CameraProfiles.DirectoryCopy(directoryInfo.FullName, CameraProfiles.mPath, true);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000027E4 File Offset: 0x000009E4
		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = Path.Combine(destDirName, fileInfo.Name);
				fileInfo.CopyTo(text, false);
			}
			if (copySubDirs)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string text2 = Path.Combine(destDirName, directoryInfo2.Name);
					CameraProfiles.DirectoryCopy(directoryInfo2.FullName, text2, copySubDirs);
				}
			}
		}

		// Token: 0x0400000E RID: 14
		public static string pPath = Path.Combine(UnityGame.UserDataPath, "." + Plugin.Name.ToLower());

		// Token: 0x0400000F RID: 15
		public static string mPath = Path.Combine(UnityGame.UserDataPath, Plugin.Name);

		// Token: 0x04000010 RID: 16
		public static string currentlySelected = "None";
	}
}
