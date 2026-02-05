using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using IPA.Utilities;

namespace IPA.Injector
{
	// Token: 0x02000002 RID: 2
	internal class AntiPiracy
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool IsInvalid(string path)
		{
			string dataPlugins = Path.Combine(GameVersionEarly.ResolveDataPath(path), "Plugins");
			try
			{
				string userDir = AntiPiracy.GetPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), (AntiPiracy.KnownFolderFlags)2147500032U);
				string userDir2 = AntiPiracy.GetPath(new Guid("7d83ee9b-2244-4e70-b1f5-5393042af1e4"), (AntiPiracy.KnownFolderFlags)2147500032U);
				string curdir = Environment.CurrentDirectory;
				if (curdir.IsSubPathOf(userDir) || curdir.IsSubPathOf(userDir2))
				{
					return false;
				}
			}
			catch
			{
			}
			return File.Exists(Path.Combine(path, "IGG-GAMES.COM.url")) || File.Exists(Path.Combine(path, "SmartSteamEmu.ini")) || File.Exists(Path.Combine(path, "GAMESTORRENT.CO.url")) || File.Exists(Path.Combine(dataPlugins, "BSteam crack.dll")) || File.Exists(Path.Combine(dataPlugins, "HUHUVR_steam_api64.dll")) || Directory.GetFiles(dataPlugins, "*.ini", SearchOption.TopDirectoryOnly).Length != 0;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000213C File Offset: 0x0000033C
		private static string GetPath(Guid guid, AntiPiracy.KnownFolderFlags flags)
		{
			IntPtr outPath;
			if (AntiPiracy.SHGetKnownFolderPath(guid, (uint)flags, WindowsIdentity.GetCurrent().Token, out outPath) >= 0)
			{
				string text = Marshal.PtrToStringUni(outPath);
				Marshal.FreeCoTaskMem(outPath);
				return text;
			}
			return "";
		}

		// Token: 0x06000003 RID: 3
		[DllImport("Shell32.dll")]
		private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);

		// Token: 0x0200000B RID: 11
		[Flags]
		private enum KnownFolderFlags : uint
		{
			// Token: 0x04000010 RID: 16
			None = 0U,
			// Token: 0x04000011 RID: 17
			SimpleIDList = 256U,
			// Token: 0x04000012 RID: 18
			NotParentRelative = 512U,
			// Token: 0x04000013 RID: 19
			DefaultPath = 1024U,
			// Token: 0x04000014 RID: 20
			Init = 2048U,
			// Token: 0x04000015 RID: 21
			NoAlias = 4096U,
			// Token: 0x04000016 RID: 22
			DontUnexpand = 8192U,
			// Token: 0x04000017 RID: 23
			DontVerify = 16384U,
			// Token: 0x04000018 RID: 24
			Create = 32768U,
			// Token: 0x04000019 RID: 25
			NoAppcontainerRedirection = 65536U,
			// Token: 0x0400001A RID: 26
			AliasOnly = 2147483648U
		}
	}
}
