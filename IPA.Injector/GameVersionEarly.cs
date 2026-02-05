using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IPA.Utilities;
using SemVer;

namespace IPA.Injector
{
	// Token: 0x02000004 RID: 4
	internal static class GameVersionEarly
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002232 File Offset: 0x00000432
		internal static string ResolveDataPath(string installDir)
		{
			return Directory.EnumerateDirectories(installDir, "*_Data").First<string>();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002244 File Offset: 0x00000444
		internal static string GlobalGameManagers(string installDir)
		{
			return Path.Combine(GameVersionEarly.ResolveDataPath(installDir), "globalgamemanagers");
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002258 File Offset: 0x00000458
		internal static string GetGameVersion()
		{
			string mgr = GameVersionEarly.GlobalGameManagers(UnityGame.InstallPath);
			string @string;
			using (FileStream stream = File.OpenRead(mgr))
			{
				using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
				{
					int pos = 0;
					while (stream.Position < stream.Length && pos < "public.app-category.games".Length)
					{
						if ((char)reader.ReadByte() == "public.app-category.games"[pos])
						{
							pos++;
						}
						else
						{
							pos = 0;
						}
					}
					if (stream.Position == stream.Length)
					{
						throw new KeyNotFoundException("Could not find key 'public.app-category.games' in " + mgr);
					}
					int offset = 136 - "public.app-category.games".Length - 4;
					stream.Seek((long)offset, SeekOrigin.Current);
					int strlen = reader.ReadInt32();
					byte[] strbytes = reader.ReadBytes(strlen);
					@string = Encoding.UTF8.GetString(strbytes);
				}
			}
			return @string;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002350 File Offset: 0x00000550
		internal static global::SemVer.Version SafeParseVersion()
		{
			return new global::SemVer.Version(GameVersionEarly.GetGameVersion(), true);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000235D File Offset: 0x0000055D
		private static void _Load()
		{
			UnityGame.SetEarlyGameVersion(GameVersionEarly.SafeParseVersion());
			UnityGame.CheckGameVersionBoundary();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002373 File Offset: 0x00000573
		internal static void Load()
		{
			Type.GetType("SemVer.Version, SemVer", false);
			GameVersionEarly._Load();
		}
	}
}
