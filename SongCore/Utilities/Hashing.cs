using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using SongCore.Data;
using UnityEngine;

namespace SongCore.Utilities
{
	// Token: 0x02000015 RID: 21
	public class Hashing
	{
		// Token: 0x0600011A RID: 282 RVA: 0x00005528 File Offset: 0x00003728
		public static void ReadCachedSongHashes()
		{
			if (File.Exists(Hashing.cachedHashDataPath))
			{
				Hashing.cachedSongHashData = JsonConvert.DeserializeObject<Dictionary<string, SongHashData>>(File.ReadAllText(Hashing.cachedHashDataPath));
				if (Hashing.cachedSongHashData == null)
				{
					Hashing.cachedSongHashData = new Dictionary<string, SongHashData>();
				}
				Logging.Log(string.Format("Finished reading cached hashes for {0} songs!", Hashing.cachedSongHashData.Count));
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005584 File Offset: 0x00003784
		public static void UpdateCachedHashes(HashSet<string> currentSongPaths)
		{
			foreach (KeyValuePair<string, SongHashData> keyValuePair in Hashing.cachedSongHashData.ToArray<KeyValuePair<string, SongHashData>>())
			{
				if (!currentSongPaths.Contains(keyValuePair.Key))
				{
					Hashing.cachedSongHashData.Remove(keyValuePair.Key);
				}
			}
			Logging.Log(string.Format("Updating cached hashes for {0} songs!", Hashing.cachedSongHashData.Count));
			File.WriteAllText(Hashing.cachedHashDataPath, JsonConvert.SerializeObject(Hashing.cachedSongHashData));
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005608 File Offset: 0x00003808
		private static long GetDirectoryHash(string directory)
		{
			long num = 0L;
			foreach (FileInfo fileInfo in new DirectoryInfo(directory).GetFiles())
			{
				num ^= fileInfo.CreationTimeUtc.ToFileTimeUtc();
				num ^= fileInfo.LastWriteTimeUtc.ToFileTimeUtc();
				num ^= (long)fileInfo.Name.GetHashCode();
				num ^= fileInfo.Length;
			}
			return num;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005674 File Offset: 0x00003874
		private static bool GetCachedSongData(string customLevelPath, out long directoryHash, out string cachedSongHash)
		{
			directoryHash = Hashing.GetDirectoryHash(customLevelPath);
			cachedSongHash = string.Empty;
			SongHashData songHashData;
			if (Hashing.cachedSongHashData.TryGetValue(customLevelPath, out songHashData) && songHashData.directoryHash == directoryHash)
			{
				cachedSongHash = songHashData.songHash;
				return true;
			}
			return false;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000056B4 File Offset: 0x000038B4
		public static string GetCustomLevelHash(CustomPreviewBeatmapLevel level)
		{
			long num;
			string text;
			if (Hashing.GetCachedSongData(level.customLevelPath, out num, out text))
			{
				return text;
			}
			List<byte> list = new List<byte>();
			list.AddRange(File.ReadAllBytes(level.customLevelPath + "/info.dat"));
			for (int i = 0; i < level.standardLevelInfoSaveData.difficultyBeatmapSets.Length; i++)
			{
				for (int j = 0; j < level.standardLevelInfoSaveData.difficultyBeatmapSets[i].difficultyBeatmaps.Length; j++)
				{
					if (File.Exists(level.customLevelPath + "/" + level.standardLevelInfoSaveData.difficultyBeatmapSets[i].difficultyBeatmaps[j].beatmapFilename))
					{
						list.AddRange(File.ReadAllBytes(level.customLevelPath + "/" + level.standardLevelInfoSaveData.difficultyBeatmapSets[i].difficultyBeatmaps[j].beatmapFilename));
					}
				}
			}
			string text2 = Hashing.CreateSha1FromBytes(list.ToArray()).ToLower();
			Hashing.cachedSongHashData[level.customLevelPath] = new SongHashData(num, text2);
			return text2;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000057D0 File Offset: 0x000039D0
		public static string GetCustomLevelHash(StandardLevelInfoSaveData level, string customLevelPath)
		{
			long num;
			string text;
			if (Hashing.GetCachedSongData(customLevelPath, out num, out text))
			{
				return text;
			}
			byte[] array = new byte[0];
			array = array.Concat(File.ReadAllBytes(customLevelPath + "/info.dat")).ToArray<byte>();
			for (int i = 0; i < level.difficultyBeatmapSets.Length; i++)
			{
				for (int j = 0; j < level.difficultyBeatmapSets[i].difficultyBeatmaps.Length; j++)
				{
					if (File.Exists(customLevelPath + "/" + level.difficultyBeatmapSets[i].difficultyBeatmaps[j].beatmapFilename))
					{
						array = array.Concat(File.ReadAllBytes(customLevelPath + "/" + level.difficultyBeatmapSets[i].difficultyBeatmaps[j].beatmapFilename)).ToArray<byte>();
					}
				}
			}
			string text2 = Hashing.CreateSha1FromBytes(array.ToArray<byte>()).ToLower();
			Hashing.cachedSongHashData[customLevelPath] = new SongHashData(num, text2);
			return text2;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000058C8 File Offset: 0x00003AC8
		public static string GetHash(StandardLevelInfoSaveData level, string customLevelPath)
		{
			string text = "";
			text += File.ReadAllText(customLevelPath + "/info.dat");
			for (int i = 0; i < level.difficultyBeatmapSets.Length; i++)
			{
				for (int j = 0; j < level.difficultyBeatmapSets[i].difficultyBeatmaps.Length; j++)
				{
					if (File.Exists(customLevelPath + "/" + level.difficultyBeatmapSets[i].difficultyBeatmaps[j].beatmapFilename))
					{
						text += File.ReadAllText(customLevelPath + "/" + level.difficultyBeatmapSets[i].difficultyBeatmaps[j].beatmapFilename);
					}
				}
			}
			return Hashing.CreatHashFromString(text).ToLower();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000597C File Offset: 0x00003B7C
		public static string GetCustomLevelHash(CustomBeatmapLevel level)
		{
			long num;
			string text;
			if (Hashing.GetCachedSongData(level.customLevelPath, out num, out text))
			{
				return text;
			}
			byte[] array = new byte[0];
			array = array.Concat(File.ReadAllBytes(level.customLevelPath + "/info.dat")).ToArray<byte>();
			for (int i = 0; i < level.standardLevelInfoSaveData.difficultyBeatmapSets.Length; i++)
			{
				for (int j = 0; j < level.standardLevelInfoSaveData.difficultyBeatmapSets[i].difficultyBeatmaps.Length; j++)
				{
					if (File.Exists(level.customLevelPath + "/" + level.standardLevelInfoSaveData.difficultyBeatmapSets[i].difficultyBeatmaps[j].beatmapFilename))
					{
						array = array.Concat(File.ReadAllBytes(level.customLevelPath + "/" + level.standardLevelInfoSaveData.difficultyBeatmapSets[i].difficultyBeatmaps[j].beatmapFilename)).ToArray<byte>();
					}
				}
			}
			string text2 = Hashing.CreateSha1FromBytes(array.ToArray<byte>()).ToLower();
			Hashing.cachedSongHashData[level.customLevelPath] = new SongHashData(num, text2);
			return text2;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005AA4 File Offset: 0x00003CA4
		public static string CreateSha1FromString(string input)
		{
			string text;
			using (SHA1 sha = SHA1.Create())
			{
				byte[] bytes = Encoding.ASCII.GetBytes(input);
				text = BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", string.Empty);
			}
			return text;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005AFC File Offset: 0x00003CFC
		public static string CreateSha1FromBytes(byte[] input)
		{
			string text;
			using (SHA1 sha = SHA1.Create())
			{
				text = BitConverter.ToString(sha.ComputeHash(input)).Replace("-", string.Empty);
			}
			return text;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005B4C File Offset: 0x00003D4C
		public static bool CreateSha1FromFile(string path, out string hash)
		{
			hash = "";
			if (!File.Exists(path))
			{
				return false;
			}
			bool flag;
			using (SHA1 sha = SHA1.Create())
			{
				using (FileStream fileStream = File.OpenRead(path))
				{
					byte[] array = sha.ComputeHash(fileStream);
					hash = BitConverter.ToString(array).Replace("-", string.Empty);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005BCC File Offset: 0x00003DCC
		public static string CreatHashFromString(string input)
		{
			string text;
			using (SHA1Managed sha1Managed = new SHA1Managed())
			{
				byte[] array = sha1Managed.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
				foreach (byte b in array)
				{
					stringBuilder.Append(b.ToString("X2"));
				}
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x04000069 RID: 105
		internal static Dictionary<string, SongHashData> cachedSongHashData = new Dictionary<string, SongHashData>();

		// Token: 0x0400006A RID: 106
		internal static string cachedHashDataPath = Path.Combine(Application.persistentDataPath, "SongHashData.dat");
	}
}
