using System;
using System.IO;
using Newtonsoft.Json;

namespace CustomAvatar.Utilities
{
	// Token: 0x02000021 RID: 33
	internal static class SettingsManager
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000045CA File Offset: 0x000027CA
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000045D1 File Offset: 0x000027D1
		public static Settings settings { get; private set; }

		// Token: 0x06000071 RID: 113 RVA: 0x000045DC File Offset: 0x000027DC
		public static void LoadSettings()
		{
			Plugin.logger.Info("Loading settings from " + SettingsManager.kSettingsPath);
			bool flag = !File.Exists(SettingsManager.kSettingsPath);
			if (flag)
			{
				Plugin.logger.Info("File does not exist, using default settings");
				SettingsManager.settings = new Settings();
			}
			else
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(SettingsManager.kSettingsPath))
					{
						using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
						{
							JsonSerializer serializer = SettingsManager.GetSerializer();
							SettingsManager.settings = serializer.Deserialize<Settings>(jsonTextReader) ?? new Settings();
						}
					}
				}
				catch (Exception ex)
				{
					Plugin.logger.Error("Failed to load settings from file, using default settings");
					Plugin.logger.Error(ex);
					SettingsManager.settings = new Settings();
				}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000046D8 File Offset: 0x000028D8
		public static void SaveSettings()
		{
			Plugin.logger.Info("Saving settings to " + SettingsManager.kSettingsPath);
			using (StreamWriter streamWriter = new StreamWriter(SettingsManager.kSettingsPath))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
				{
					JsonSerializer serializer = SettingsManager.GetSerializer();
					serializer.Serialize(jsonTextWriter, SettingsManager.settings);
					jsonTextWriter.Flush();
				}
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004764 File Offset: 0x00002964
		private static JsonSerializer GetSerializer()
		{
			return new JsonSerializer
			{
				Formatting = Formatting.Indented,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				Converters = 
				{
					new Vector3JsonConverter(),
					new QuaternionJsonConverter(),
					new PoseJsonConverter()
				}
			};
		}

		// Token: 0x0400011E RID: 286
		public static readonly string kSettingsPath = Path.Combine(Environment.CurrentDirectory, "UserData", "CustomAvatars.json");
	}
}
