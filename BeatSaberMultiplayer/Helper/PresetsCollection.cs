using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeatSaberMultiplayer.Data;
using IPA.Logging;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000077 RID: 119
	public static class PresetsCollection
	{
		// Token: 0x06000872 RID: 2162 RVA: 0x000242E8 File Offset: 0x000224E8
		public static void ReloadPresets()
		{
			try
			{
				PresetsCollection.loadedPresets.Clear();
				if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData")))
				{
					Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData"));
				}
				if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, "UserData"), "RoomPresets")))
				{
					Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, "UserData"), "RoomPresets"));
				}
				foreach (string text in Directory.GetFiles(Path.Combine(Path.Combine(Environment.CurrentDirectory, "UserData"), "RoomPresets"), "*.json").ToList<string>())
				{
					try
					{
						RoomPreset roomPreset = RoomPreset.LoadPreset(text);
						PresetsCollection.loadedPresets.Add(roomPreset);
					}
					catch (Exception ex)
					{
						Logger.log.Error(string.Format("Unable to parse preset @ {0}! Exception: {1}", text, ex));
					}
				}
			}
			catch (Exception ex2)
			{
				Logger log = Logger.log;
				string text2 = "Unable to load presets! Exception: ";
				Exception ex3 = ex2;
				log.Error(text2 + ((ex3 != null) ? ex3.ToString() : null));
			}
		}

		// Token: 0x0400044B RID: 1099
		public static List<RoomPreset> loadedPresets = new List<RoomPreset>();
	}
}
