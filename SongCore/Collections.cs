using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using IPA.Logging;
using Newtonsoft.Json;
using SongCore.Data;
using SongCore.UI;
using SongCore.Utilities;
using UnityEngine;

namespace SongCore
{
	// Token: 0x0200000F RID: 15
	public static class Collections
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003373 File Offset: 0x00001573
		public static ReadOnlyCollection<string> capabilities
		{
			get
			{
				return Collections._capabilities.AsReadOnly();
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000337F File Offset: 0x0000157F
		public static ReadOnlyCollection<BeatmapCharacteristicSO> customCharacteristics
		{
			get
			{
				return Collections._customCharacteristics.AsReadOnly();
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000338B File Offset: 0x0000158B
		public static bool songWithHashPresent(string hash)
		{
			return Collections.hashLevelDictionary.ContainsKey(hash.ToUpper());
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000033A4 File Offset: 0x000015A4
		public static string hashForLevelID(string levelID)
		{
			string text;
			if (Collections.levelHashDictionary.TryGetValue(levelID, out text))
			{
				return text;
			}
			return "";
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000033C8 File Offset: 0x000015C8
		public static List<string> levelIDsForHash(string hash)
		{
			List<string> list;
			if (Collections.hashLevelDictionary.TryGetValue(hash.ToUpper(), out list))
			{
				return list;
			}
			return new List<string>();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000033F0 File Offset: 0x000015F0
		public static void AddSong(string levelID, string path)
		{
			if (!Collections.customSongsData.ContainsKey(levelID))
			{
				Collections.customSongsData.Add(levelID, new ExtraSongData(levelID, path));
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003414 File Offset: 0x00001614
		public static ExtraSongData RetrieveExtraSongData(string levelID, string loadIfNullPath = "")
		{
			if (Collections.customSongsData.ContainsKey(levelID))
			{
				return Collections.customSongsData[levelID];
			}
			if (!string.IsNullOrWhiteSpace(loadIfNullPath))
			{
				Collections.AddSong(levelID, loadIfNullPath);
				if (Collections.customSongsData.ContainsKey(levelID))
				{
					return Collections.customSongsData[levelID];
				}
			}
			return null;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003464 File Offset: 0x00001664
		public static ExtraSongData.DifficultyData RetrieveDifficultyData(IDifficultyBeatmap beatmap)
		{
			ExtraSongData extraSongData = null;
			if (beatmap.level is CustomPreviewBeatmapLevel)
			{
				CustomPreviewBeatmapLevel customPreviewBeatmapLevel = beatmap.level as CustomPreviewBeatmapLevel;
				extraSongData = Collections.RetrieveExtraSongData(Hashing.GetCustomLevelHash(customPreviewBeatmapLevel), customPreviewBeatmapLevel.customLevelPath);
			}
			if (extraSongData == null)
			{
				return null;
			}
			return extraSongData._difficulties.FirstOrDefault((ExtraSongData.DifficultyData x) => x._difficulty == beatmap.difficulty && (x._beatmapCharacteristicName == beatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.characteristicNameLocalizationKey || x._beatmapCharacteristicName == beatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName));
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000034D1 File Offset: 0x000016D1
		public static void LoadExtraSongData()
		{
			Collections.customSongsData = JsonConvert.DeserializeObject<Dictionary<string, ExtraSongData>>(File.ReadAllText(Collections.dataPath));
			if (Collections.customSongsData == null)
			{
				Collections.customSongsData = new Dictionary<string, ExtraSongData>();
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000034F8 File Offset: 0x000016F8
		public static void SaveExtraSongData()
		{
			File.WriteAllText(Collections.dataPath, JsonConvert.SerializeObject(Collections.customSongsData, Formatting.None));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000350F File Offset: 0x0000170F
		public static void RegisterCapability(string capability)
		{
			if (!Collections._capabilities.Contains(capability))
			{
				Collections._capabilities.Add(capability);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000352C File Offset: 0x0000172C
		public static BeatmapCharacteristicSO RegisterCustomCharacteristic(Sprite Icon, string CharacteristicName, string HintText, string SerializedName, string CompoundIdPartName, bool requires360Movement = false, bool containsRotationEvents = false, int sortingOrder = 99)
		{
			BeatmapCharacteristicSO newChar = ScriptableObject.CreateInstance<BeatmapCharacteristicSO>();
			newChar.SetField("_icon", Icon);
			newChar.SetField("_descriptionLocalizationKey", HintText);
			newChar.SetField("_serializedName", SerializedName);
			newChar.SetField("_characteristicNameLocalizationKey", CharacteristicName);
			newChar.SetField("_compoundIdPartName", CompoundIdPartName);
			newChar.SetField("_requires360Movement", requires360Movement);
			newChar.SetField("_containsRotationEvents", containsRotationEvents);
			newChar.SetField("_sortingOrder", sortingOrder);
			if (!Collections._customCharacteristics.Any((BeatmapCharacteristicSO x) => x.serializedName == newChar.serializedName))
			{
				Collections._customCharacteristics.Add(newChar);
				return newChar;
			}
			return null;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003618 File Offset: 0x00001818
		public static SeperateSongFolder AddSeperateSongFolder(string name, string folderPath, FolderLevelPack pack, Sprite image = null, bool wip = false)
		{
			BasicUI.GetIcons();
			if (!Directory.Exists(folderPath))
			{
				try
				{
					Directory.CreateDirectory(folderPath);
				}
				catch (Exception ex)
				{
					Logger logger = Logging.logger;
					string text = "Failed to make folder for: ";
					string text2 = "\n";
					Exception ex2 = ex;
					logger.Error(text + name + text2 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			ModSeperateSongFolder modSeperateSongFolder = new ModSeperateSongFolder(new SongFolderEntry(name, folderPath, pack, "", wip), (image == null) ? BasicUI.FolderIcon : image);
			if (Loader.SeperateSongFolders == null)
			{
				Loader.SeperateSongFolders = new List<SeperateSongFolder>();
			}
			Loader.SeperateSongFolders.Add(modSeperateSongFolder);
			return modSeperateSongFolder;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000036B8 File Offset: 0x000018B8
		public static void DeregisterizeCapability(string capability)
		{
			Collections._capabilities.Remove(capability);
		}

		// Token: 0x0400001A RID: 26
		internal static CustomBeatmapLevelPack WipLevelPack;

		// Token: 0x0400001B RID: 27
		internal static string dataPath = Path.Combine(Application.persistentDataPath, "SongCoreExtraData.dat");

		// Token: 0x0400001C RID: 28
		internal static Dictionary<string, ExtraSongData> customSongsData = new Dictionary<string, ExtraSongData>();

		// Token: 0x0400001D RID: 29
		internal static Dictionary<string, string> levelHashDictionary = new Dictionary<string, string>();

		// Token: 0x0400001E RID: 30
		internal static Dictionary<string, List<string>> hashLevelDictionary = new Dictionary<string, List<string>>();

		// Token: 0x0400001F RID: 31
		private static List<string> _capabilities = new List<string>();

		// Token: 0x04000020 RID: 32
		private static List<BeatmapCharacteristicSO> _customCharacteristics = new List<BeatmapCharacteristicSO>();
	}
}
