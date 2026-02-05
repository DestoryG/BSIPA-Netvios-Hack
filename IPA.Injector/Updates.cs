using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using IPA.Logging;
using IPA.Utilities;

namespace IPA.Injector
{
	// Token: 0x02000007 RID: 7
	internal static class Updates
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002C2A File Offset: 0x00000E2A
		public static void InstallPendingUpdates()
		{
			Updates.InstallPendingSelfUpdates();
			Updates.InstallPendingModUpdates();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002C38 File Offset: 0x00000E38
		private static void InstallPendingSelfUpdates()
		{
			string path = Path.Combine(UnityGame.InstallPath, "IPA.exe");
			if (!File.Exists(path))
			{
				return;
			}
			Version version = new Version(FileVersionInfo.GetVersionInfo(path).FileVersion);
			Version selfVersion = Assembly.GetExecutingAssembly().GetName().Version;
			if (version > selfVersion)
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = path,
					Arguments = string.Format("\"-nw={0},s={1}\"", Process.GetCurrentProcess().Id, string.Join(" ", Environment.GetCommandLineArgs().Skip(1).StrJP()).Replace("\\", "\\\\").Replace(",", "\\,")),
					UseShellExecute = false
				});
				Logger.updater.Info("Updating BSIPA...");
				Environment.Exit(0);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002D10 File Offset: 0x00000F10
		private static void InstallPendingModUpdates()
		{
			string pendingDir = Path.Combine(UnityGame.InstallPath, "IPA", "Pending");
			if (!Directory.Exists(pendingDir))
			{
				return;
			}
			Logger.updater.Info("Installing pending updates");
			string[] toDelete = new string[0];
			string delFn = Path.Combine(pendingDir, "$$delete");
			if (File.Exists(delFn))
			{
				toDelete = File.ReadAllLines(delFn);
				File.Delete(delFn);
			}
			foreach (string file in toDelete)
			{
				try
				{
					File.Delete(Path.Combine(UnityGame.InstallPath, file));
				}
				catch (Exception e)
				{
					Logger.updater.Error("While trying to install pending updates: Error deleting file marked for deletion");
					Exception e7;
					Logger.updater.Error(e7);
				}
			}
			string path;
			if (Directory.Exists(path = Path.Combine(pendingDir, "IPA")))
			{
				Stack<string> dirs = new Stack<string>(20);
				dirs.Push(path);
				while (dirs.Count > 0)
				{
					string currentDir = dirs.Pop();
					string[] subDirs;
					string[] files;
					try
					{
						subDirs = Directory.GetDirectories(currentDir);
						files = Directory.GetFiles(currentDir);
					}
					catch (UnauthorizedAccessException e2)
					{
						Logger.updater.Error(e2);
						continue;
					}
					catch (DirectoryNotFoundException e3)
					{
						Logger.updater.Error(e3);
						continue;
					}
					foreach (string file2 in files)
					{
						try
						{
							if (!Utils.GetRelativePath(file2, path).Split(new char[] { Path.PathSeparator }).Contains("Pending"))
							{
								File.Delete(file2);
							}
						}
						catch (FileNotFoundException e4)
						{
							Logger.updater.Error(e4);
						}
					}
					foreach (string str in subDirs)
					{
						dirs.Push(str);
					}
				}
			}
			if (File.Exists(path = Path.Combine(pendingDir, "IPA.exe")))
			{
				File.Delete(path);
				if (File.Exists(path = Path.Combine(pendingDir, "Mono.Cecil.dll")))
				{
					File.Delete(path);
				}
			}
			try
			{
				Utils.CopyAll(new DirectoryInfo(pendingDir), new DirectoryInfo(UnityGame.InstallPath), "", delegate(Exception e, FileInfo f)
				{
					Logger.updater.Error("Error copying file " + Utils.GetRelativePath(f.FullName, pendingDir) + " from Pending:");
					Logger.updater.Error(e);
					return true;
				});
			}
			catch (Exception e5)
			{
				Logger.updater.Error("While trying to install pending updates: Error copying files in");
				Logger.updater.Error(e5);
			}
			try
			{
				Directory.Delete(pendingDir, true);
			}
			catch (Exception e6)
			{
				Logger.updater.Error("Something went wrong performing an operation that should never fail!");
				Logger.updater.Error(e6);
			}
		}

		// Token: 0x04000006 RID: 6
		private const string DeleteFileName = "$$delete";
	}
}
