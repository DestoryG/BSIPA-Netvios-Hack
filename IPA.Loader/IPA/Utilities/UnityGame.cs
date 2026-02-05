using System;
using System.IO;
using System.Reflection;
using System.Threading;
using IPA.Config;
using IPA.Logging;
using UnityEngine;

namespace IPA.Utilities
{
	/// <summary>
	/// Provides some basic utility methods and properties of Beat Saber
	/// </summary>
	// Token: 0x02000015 RID: 21
	public static class UnityGame
	{
		/// <summary>
		/// Provides the current game version.
		/// </summary>
		/// <value>the SemVer version of the game</value>
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002E8E File Offset: 0x0000108E
		public static AlmostVersion GameVersion
		{
			get
			{
				AlmostVersion almostVersion;
				if ((almostVersion = UnityGame._gameVersion) == null)
				{
					almostVersion = (UnityGame._gameVersion = new AlmostVersion(UnityGame.ApplicationVersionProxy));
				}
				return almostVersion;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002EA9 File Offset: 0x000010A9
		internal static void SetEarlyGameVersion(AlmostVersion ver)
		{
			UnityGame._gameVersion = ver;
			Logger.log.Debug(string.Format("GameVersion set early to {0}", ver));
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002EC6 File Offset: 0x000010C6
		private static string ApplicationVersionProxy
		{
			get
			{
				return Application.version;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002ED0 File Offset: 0x000010D0
		internal static void EnsureRuntimeGameVersion()
		{
			try
			{
				AlmostVersion rtVer = new AlmostVersion(UnityGame.ApplicationVersionProxy);
				if (!rtVer.Equals(UnityGame._gameVersion))
				{
					Logger.log.Warn(string.Format("Early version {0} parsed from game files doesn't match runtime version {1}!", UnityGame._gameVersion, rtVer));
					UnityGame._gameVersion = rtVer;
				}
			}
			catch (MissingMethodException e)
			{
				Logger.log.Error("Application.version was not found! Cannot check early parsed version");
				if (SelfConfig.Debug_.ShowHandledErrorStackTraces_)
				{
					Logger.log.Error(e);
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002F4C File Offset: 0x0000114C
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002F53 File Offset: 0x00001153
		internal static bool IsGameVersionBoundary { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002F5B File Offset: 0x0000115B
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002F62 File Offset: 0x00001162
		internal static AlmostVersion OldVersion { get; private set; }

		// Token: 0x06000045 RID: 69 RVA: 0x00002F6C File Offset: 0x0000116C
		internal static void CheckGameVersionBoundary()
		{
			AlmostVersion gameVer = UnityGame.GameVersion;
			string lastVerS = SelfConfig.LastGameVersion_;
			UnityGame.OldVersion = ((lastVerS != null) ? new AlmostVersion(lastVerS, gameVer) : null);
			UnityGame.IsGameVersionBoundary = UnityGame.OldVersion != null && gameVer != UnityGame.OldVersion;
			SelfConfig.Instance.LastGameVersion = gameVer.ToString();
		}

		/// <summary>
		/// Checks if the currently running code is running on the Unity main thread.
		/// </summary>
		/// <value><see langword="true" /> if the curent thread is the Unity main thread, <see langword="false" /> otherwise</value>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002FC8 File Offset: 0x000011C8
		public static bool OnMainThread
		{
			get
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				Thread thread = UnityGame.mainThread;
				int? num = ((thread != null) ? new int?(thread.ManagedThreadId) : null);
				return (managedThreadId == num.GetValueOrDefault()) & (num != null);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000300F File Offset: 0x0000120F
		internal static void SetMainThread()
		{
			UnityGame.mainThread = Thread.CurrentThread;
		}

		/// <summary>
		/// Gets the <see cref="T:IPA.Utilities.UnityGame.Release" /> type of this installation of Beat Saber
		/// </summary>
		/// <remarks>
		/// This only gives a
		/// </remarks>
		/// <value>the type of release this is</value>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000048 RID: 72 RVA: 0x0000301C File Offset: 0x0000121C
		public static UnityGame.Release ReleaseType
		{
			get
			{
				UnityGame.Release? releaseCache = UnityGame._releaseCache;
				return ((releaseCache != null) ? releaseCache : (UnityGame._releaseCache = new UnityGame.Release?(UnityGame.CheckIsSteam() ? UnityGame.Release.Steam : UnityGame.Release.Other))).Value;
			}
		}

		/// <summary>
		/// Gets the path to the game's install directory.
		/// </summary>
		/// <value>the path of the game install directory</value>
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003059 File Offset: 0x00001259
		public static string InstallPath
		{
			get
			{
				if (UnityGame._installRoot == null)
				{
					UnityGame._installRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));
				}
				return UnityGame._installRoot;
			}
		}

		/// <summary>
		/// The path to the `Libs` folder. Use only if necessary.
		/// </summary>
		/// <value>the path to the library directory</value>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000308F File Offset: 0x0000128F
		public static string LibraryPath
		{
			get
			{
				return Path.Combine(UnityGame.InstallPath, "Libs");
			}
		}

		/// <summary>
		/// The path to the `Libs\Native` folder. Use only if necessary.
		/// </summary>
		/// <value>the path to the native library directory</value>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000030A0 File Offset: 0x000012A0
		public static string NativeLibraryPath
		{
			get
			{
				return Path.Combine(UnityGame.LibraryPath, "Native");
			}
		}

		/// <summary>
		/// The directory to load plugins from.
		/// </summary>
		/// <value>the path to the plugin directory</value>
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004C RID: 76 RVA: 0x000030B1 File Offset: 0x000012B1
		public static string PluginsPath
		{
			get
			{
				return Path.Combine(UnityGame.InstallPath, "Plugins");
			}
		}

		/// <summary>
		/// The path to the `UserData` folder.
		/// </summary>
		/// <value>the path to the user data directory</value>
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000030C2 File Offset: 0x000012C2
		public static string UserDataPath
		{
			get
			{
				return Path.Combine(UnityGame.InstallPath, "UserData");
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000030D4 File Offset: 0x000012D4
		private static bool CheckIsSteam()
		{
			DirectoryInfo installDirInfo = new DirectoryInfo(UnityGame.InstallPath);
			DirectoryInfo parent = installDirInfo.Parent;
			if (((parent != null) ? parent.Name : null) == "common")
			{
				DirectoryInfo parent2 = installDirInfo.Parent;
				string text;
				if (parent2 == null)
				{
					text = null;
				}
				else
				{
					DirectoryInfo parent3 = parent2.Parent;
					text = ((parent3 != null) ? parent3.Name : null);
				}
				return text == "steamapps";
			}
			return false;
		}

		// Token: 0x04000019 RID: 25
		private static AlmostVersion _gameVersion;

		// Token: 0x0400001C RID: 28
		private static Thread mainThread;

		// Token: 0x0400001D RID: 29
		private static UnityGame.Release? _releaseCache;

		// Token: 0x0400001E RID: 30
		private static string _installRoot;

		/// <summary>
		/// The different types of releases of the game.
		/// </summary>
		// Token: 0x020000BB RID: 187
		public enum Release
		{
			/// <summary>
			/// Indicates a Steam release.
			/// </summary>
			// Token: 0x040001A0 RID: 416
			Steam,
			/// <summary>
			/// Indicates a non-Steam release.
			/// </summary>
			// Token: 0x040001A1 RID: 417
			Other
		}
	}
}
