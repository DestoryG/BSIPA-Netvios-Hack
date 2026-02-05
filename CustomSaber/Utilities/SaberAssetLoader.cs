using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomSaber.Data;
using CustomSaber.Settings;
using UnityEngine;

namespace CustomSaber.Utilities
{
	// Token: 0x02000010 RID: 16
	public class SaberAssetLoader
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002AC0 File Offset: 0x00000CC0
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002AC7 File Offset: 0x00000CC7
		public static bool IsLoaded { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002ACF File Offset: 0x00000CCF
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002AD6 File Offset: 0x00000CD6
		public static int SelectedSaber { get; internal set; } = 0;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002ADE File Offset: 0x00000CDE
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002AE5 File Offset: 0x00000CE5
		public static IList<CustomSaberData> CustomSabers { get; private set; } = new List<CustomSaberData>();

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002AED File Offset: 0x00000CED
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public static IEnumerable<string> CustomSaberFiles { get; private set; } = Enumerable.Empty<string>();

		// Token: 0x0600003D RID: 61 RVA: 0x00002AFC File Offset: 0x00000CFC
		internal static void Load()
		{
			DateTime now = DateTime.Now;
			bool flag = !SaberAssetLoader.IsLoaded;
			if (flag)
			{
				Directory.CreateDirectory(Plugin.PluginAssetPath);
				IEnumerable<string> enumerable = new List<string> { "*.saber" };
				SaberAssetLoader.CustomSaberFiles = Utils.GetFileNames(Plugin.PluginAssetPath, enumerable, SearchOption.AllDirectories, true);
				Logger.log.Debug(string.Format("{0} external saber(s) found.", SaberAssetLoader.CustomSaberFiles.Count<string>()));
				SaberAssetLoader.CustomSabers = SaberAssetLoader.LoadCustomSabers(SaberAssetLoader.CustomSaberFiles, SaberAssetLoader.DefaultSaber);
				Logger.log.Debug(string.Format("{0} total saber(s) loaded in {1} seconds.", SaberAssetLoader.CustomSabers.Count, (DateTime.Now - now).TotalSeconds));
				bool flag2 = Configuration.CurrentlySelectedSaber != null;
				if (flag2)
				{
					for (int i = 0; i < SaberAssetLoader.CustomSabers.Count; i++)
					{
						bool flag3 = SaberAssetLoader.CustomSabers[i].FileName == Configuration.CurrentlySelectedSaber;
						if (flag3)
						{
							SaberAssetLoader.SelectedSaber = i;
							break;
						}
					}
				}
				SaberAssetLoader.IsLoaded = true;
				Action action = SaberAssetLoader.customSabersLoaded;
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002C3B File Offset: 0x00000E3B
		internal static void Reload()
		{
			Logger.log.Debug("Reloading the SaberAssetLoader");
			SaberAssetLoader.Clear();
			SaberAssetLoader.Load();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002C5C File Offset: 0x00000E5C
		internal static void Clear()
		{
			int count = SaberAssetLoader.CustomSabers.Count;
			for (int i = 0; i < count; i++)
			{
				bool flag = i == 0;
				if (flag)
				{
					SaberAssetLoader.DefaultSaber = SaberAssetLoader.CustomSabers[i];
				}
				SaberAssetLoader.CustomSabers[i].Destroy();
				SaberAssetLoader.CustomSabers[i] = null;
			}
			SaberAssetLoader.IsLoaded = false;
			SaberAssetLoader.SelectedSaber = 0;
			SaberAssetLoader.CustomSabers = new List<CustomSaberData>();
			SaberAssetLoader.CustomSaberFiles = Enumerable.Empty<string>();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002CE4 File Offset: 0x00000EE4
		private static IList<CustomSaberData> LoadCustomSabers(IEnumerable<string> customSaberFiles, CustomSaberData defaultSaber)
		{
			IList<CustomSaberData> list = new List<CustomSaberData>();
			bool flag = defaultSaber != null;
			if (flag)
			{
				list.Add(defaultSaber);
			}
			else
			{
				list.Add(new CustomSaberData("DefaultSabers"));
			}
			foreach (string text in customSaberFiles)
			{
				try
				{
					CustomSaberData customSaberData = new CustomSaberData(text);
					bool flag2 = customSaberData != null;
					if (flag2)
					{
						list.Add(customSaberData);
					}
				}
				catch (Exception ex)
				{
					Logger.log.Warn("Failed to Load Custom Saber with name '" + text + "'.");
					Logger.log.Warn(ex);
				}
			}
			return list;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002DBC File Offset: 0x00000FBC
		public static CustomSaberData GetRandomSaber()
		{
			Random.InitState(DateTime.Now.Millisecond);
			return SaberAssetLoader.CustomSabers[Random.Range(0, SaberAssetLoader.CustomSabers.Count)];
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002DFC File Offset: 0x00000FFC
		public static int DeleteCurrentSaber()
		{
			bool flag = SaberAssetLoader.SelectedSaber == 0;
			int num;
			if (flag)
			{
				num = 0;
			}
			else
			{
				string text = Path.Combine(Plugin.PluginAssetPath, SaberAssetLoader.CustomSabers[SaberAssetLoader.SelectedSaber].FileName);
				int selectedSaber = SaberAssetLoader.SelectedSaber;
				try
				{
					File.Delete(text);
				}
				catch (Exception ex)
				{
					Logger.log.Error("Failed to delete Saber: " + SaberAssetLoader.CustomSabers[SaberAssetLoader.SelectedSaber].Descriptor.SaberName + " - " + text);
					Logger.log.Error(ex.Message + " - " + ex.StackTrace);
					return 0;
				}
				SaberAssetLoader.CustomSabers[SaberAssetLoader.SelectedSaber].Destroy();
				SaberAssetLoader.CustomSabers.RemoveAt(SaberAssetLoader.SelectedSaber);
				SaberAssetLoader.SelectedSaber = ((SaberAssetLoader.SelectedSaber < SaberAssetLoader.CustomSabers.Count - 1) ? SaberAssetLoader.SelectedSaber : (SaberAssetLoader.SelectedSaber - 1));
				Configuration.CurrentlySelectedSaber = SaberAssetLoader.CustomSabers[SaberAssetLoader.SelectedSaber].FileName;
				num = selectedSaber;
			}
			return num;
		}

		// Token: 0x0400003B RID: 59
		public static Action customSabersLoaded;

		// Token: 0x0400003C RID: 60
		private static CustomSaberData DefaultSaber;
	}
}
