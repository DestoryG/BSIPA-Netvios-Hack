using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SongCore;
using UnityEngine;
using UnityEngine.Networking;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000085 RID: 133
	public class SongDownloader : MonoBehaviour
	{
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x0600093A RID: 2362 RVA: 0x00025B60 File Offset: 0x00023D60
		// (remove) Token: 0x0600093B RID: 2363 RVA: 0x00025B98 File Offset: 0x00023D98
		public event Action<Song> songDownloaded;

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x00025BCD File Offset: 0x00023DCD
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x00025BF4 File Offset: 0x00023DF4
		public static SongDownloader Instance
		{
			get
			{
				if (!SongDownloader._instance)
				{
					SongDownloader._instance = new GameObject("SongDownloader").AddComponent<SongDownloader>();
				}
				return SongDownloader._instance;
			}
			private set
			{
				SongDownloader._instance = value;
			}
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00025BFC File Offset: 0x00023DFC
		public static UnityWebRequest GetRequestForUrl(string url)
		{
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
			unityWebRequest.SetRequestHeader("User-Agent", SongDownloader.UserAgent);
			return unityWebRequest;
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00025C14 File Offset: 0x00023E14
		public static string UserAgent { get; } = string.Format("{0}/{1}", Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly().GetName().Version);

		// Token: 0x06000940 RID: 2368 RVA: 0x00025C1B File Offset: 0x00023E1B
		public void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
			if (!Loader.AreSongsLoaded)
			{
				Loader.SongsLoadedEvent += this.SongLoader_SongsLoadedEvent;
				return;
			}
			this.SongLoader_SongsLoadedEvent(null, Loader.CustomLevels);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00025C4D File Offset: 0x00023E4D
		private void SongLoader_SongsLoadedEvent(Loader sender, Dictionary<string, CustomPreviewBeatmapLevel> levels)
		{
			this._alreadyDownloadedSongs = levels.Values.Select((CustomPreviewBeatmapLevel x) => new Song(x)).ToList<Song>();
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00025C84 File Offset: 0x00023E84
		public void DownloadSong(Song songInfo, Action<bool> downloadedCallback, Action<float> progressChangedCallback)
		{
			base.StartCoroutine(this.DownloadSongCoroutine(songInfo, downloadedCallback, progressChangedCallback));
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00025C96 File Offset: 0x00023E96
		public IEnumerator DownloadSongCoroutine(Song songInfo, Action<bool> downloadedCallback, Action<float> progressChangedCallback)
		{
			songInfo.songQueueState = SongQueueState.Downloading;
			if (Collections.songWithHashPresent(songInfo.hash.ToUpper()))
			{
				songInfo.downloadingProgress = 1f;
				yield return new WaitForSeconds(0.1f);
				songInfo.songQueueState = SongQueueState.Downloaded;
				Action<Song> action = this.songDownloaded;
				if (action != null)
				{
					action(songInfo);
				}
				if (downloadedCallback != null)
				{
					downloadedCallback(true);
				}
				yield break;
			}
			bool timeout = false;
			float time = 0f;
			UnityWebRequest www;
			UnityWebRequestAsyncOperation asyncRequest;
			try
			{
				www = SongDownloader.GetRequestForUrl(songInfo.downloadURL);
				asyncRequest = www.SendWebRequest();
				goto IL_01D5;
			}
			catch (Exception ex)
			{
				Logger.log.Error(ex);
				songInfo.songQueueState = SongQueueState.Error;
				songInfo.downloadingProgress = 1f;
				if (downloadedCallback != null)
				{
					downloadedCallback(false);
				}
				yield break;
			}
			IL_013A:
			yield return null;
			time += Time.deltaTime;
			if (time >= 5f && asyncRequest.progress <= 1E-45f)
			{
				www.Abort();
				timeout = true;
				Logger.log.Error("Connection timed out!");
			}
			songInfo.downloadingProgress = asyncRequest.progress;
			if (progressChangedCallback != null)
			{
				progressChangedCallback(asyncRequest.progress);
			}
			IL_01D5:
			if ((asyncRequest.isDone && songInfo.downloadingProgress >= 1f) || songInfo.songQueueState == SongQueueState.Error)
			{
				if (songInfo.songQueueState == SongQueueState.Error && (!asyncRequest.isDone || songInfo.downloadingProgress < 1f))
				{
					www.Abort();
				}
				if (www.isNetworkError || www.isHttpError || timeout || songInfo.songQueueState == SongQueueState.Error)
				{
					songInfo.songQueueState = SongQueueState.Error;
					Logger.log.Error("Unable to download song! " + (www.isNetworkError ? ("Network error: " + www.error) : (www.isHttpError ? ("HTTP error: " + www.error) : "Unknown error")));
					if (downloadedCallback != null)
					{
						downloadedCallback(false);
					}
				}
				else
				{
					SongDownloader.<>c__DisplayClass17_0 CS$<>8__locals1 = new SongDownloader.<>c__DisplayClass17_0();
					string customSongsPath = "";
					byte[] data = www.downloadHandler.data;
					Stream zipStream = null;
					try
					{
						customSongsPath = CustomLevelPathHelper.customLevelsDirectoryPath;
						if (!Directory.Exists(customSongsPath))
						{
							Directory.CreateDirectory(customSongsPath);
						}
						zipStream = new MemoryStream(data);
					}
					catch (Exception ex2)
					{
						Logger.log.Critical(ex2);
						songInfo.songQueueState = SongQueueState.Error;
						if (downloadedCallback != null)
						{
							downloadedCallback(false);
						}
						yield break;
					}
					yield return new WaitWhile(() => SongDownloader._extractingZip);
					CS$<>8__locals1.extract = this.ExtractZipAsync(songInfo, zipStream, customSongsPath);
					yield return new WaitWhile(() => !CS$<>8__locals1.extract.IsCompleted);
					Action<Song> action2 = this.songDownloaded;
					if (action2 != null)
					{
						action2(songInfo);
					}
					if (downloadedCallback != null)
					{
						downloadedCallback(true);
					}
					CS$<>8__locals1 = null;
					customSongsPath = null;
					zipStream = null;
				}
				yield break;
			}
			goto IL_013A;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00025CBC File Offset: 0x00023EBC
		private async Task ExtractZipAsync(Song songInfo, Stream zipStream, string customSongsPath)
		{
			try
			{
				SongDownloader.<>c__DisplayClass18_0 CS$<>8__locals1 = new SongDownloader.<>c__DisplayClass18_0();
				SongDownloader._extractingZip = true;
				CS$<>8__locals1.archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
				string text = string.Concat(new string[] { songInfo.key, " (", songInfo.songName, " - ", songInfo.levelAuthorName, ")" });
				text = string.Join("", text.Split(Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).ToArray<char>()));
				CS$<>8__locals1.path = customSongsPath + "/" + text;
				if (Directory.Exists(CS$<>8__locals1.path))
				{
					int num = 1;
					while (Directory.Exists(CS$<>8__locals1.path + string.Format(" ({0})", num)))
					{
						num++;
					}
					CS$<>8__locals1.path += string.Format(" ({0})", num);
				}
				await Task.Run(delegate
				{
					CS$<>8__locals1.archive.ExtractToDirectory(CS$<>8__locals1.path);
				}).ConfigureAwait(false);
				CS$<>8__locals1.archive.Dispose();
				songInfo.path = CS$<>8__locals1.path;
				CS$<>8__locals1 = null;
			}
			catch (Exception ex)
			{
				Logger.log.Critical(string.Format("Unable to extract ZIP! Exception: {0}", ex));
				songInfo.songQueueState = SongQueueState.Error;
				SongDownloader._extractingZip = false;
				return;
			}
			zipStream.Close();
			if (string.IsNullOrEmpty(songInfo.path))
			{
				songInfo.path = customSongsPath;
			}
			SongDownloader._extractingZip = false;
			songInfo.songQueueState = SongQueueState.Downloaded;
			this._alreadyDownloadedSongs.Add(songInfo);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00025D18 File Offset: 0x00023F18
		public bool IsSongDownloaded(Song song)
		{
			return Loader.AreSongsLoaded && this._alreadyDownloadedSongs.Any((Song x) => x.Compare(song));
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00025D54 File Offset: 0x00023F54
		public static CustomPreviewBeatmapLevel GetLevel(string levelId)
		{
			if (SongDownloader._beatmapLevelsModel == null)
			{
				SongDownloader._beatmapLevelsModel = Resources.FindObjectsOfTypeAll<BeatmapLevelsModel>().First<BeatmapLevelsModel>();
			}
			return SongDownloader._beatmapLevelsModel.allLoadedBeatmapLevelPackCollection.beatmapLevelPacks.SelectMany((IBeatmapLevelPack x) => x.beatmapLevelCollection.beatmapLevels).FirstOrDefault((IPreviewBeatmapLevel x) => x.levelID == levelId) as CustomPreviewBeatmapLevel;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00025DD4 File Offset: 0x00023FD4
		public static bool CreateMD5FromFile(string path, out string hash)
		{
			hash = "";
			if (!File.Exists(path))
			{
				return false;
			}
			bool flag;
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream = File.OpenRead(path))
				{
					byte[] array = md.ComputeHash(fileStream);
					StringBuilder stringBuilder = new StringBuilder();
					foreach (byte b in array)
					{
						stringBuilder.Append(b.ToString("X2"));
					}
					hash = stringBuilder.ToString();
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00025E78 File Offset: 0x00024078
		public void RequestSongByLevelID(string levelId, Action<Song> callback)
		{
			base.StartCoroutine(this.RequestSongByLevelIDCoroutine(levelId, callback));
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00025E89 File Offset: 0x00024089
		public IEnumerator RequestSongByLevelIDCoroutine(string levelId, Action<Song> callback)
		{
			UnityWebRequest wwwId = SongDownloader.GetRequestForUrl(PluginSetting.BeatSaverURL + "/api/maps/by-hash/" + levelId.ToLower());
			wwwId.timeout = 10;
			yield return wwwId.SendWebRequest();
			if (wwwId.isNetworkError || wwwId.isHttpError)
			{
				Logger.log.Error(string.Concat(new string[]
				{
					"Unable to fetch song by hash! ",
					wwwId.error,
					"\nURL:",
					PluginSetting.BeatSaverURL,
					"/api/maps/by-hash/",
					levelId.ToLower()
				}));
				if (callback != null)
				{
					callback(null);
				}
			}
			else
			{
				JObject jobject = JObject.Parse(wwwId.downloadHandler.text);
				if (jobject.Children().Count<JToken>() == 0)
				{
					Logger.log.Error("Song " + levelId + " doesn't exist on BeatSaver!");
					if (callback != null)
					{
						callback(null);
					}
					yield break;
				}
				Song song = Song.FromSearchNode(jobject);
				if (callback != null)
				{
					callback(song);
				}
			}
			yield break;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00025E9F File Offset: 0x0002409F
		public void RequestSongByKey(string key, Action<Song> callback)
		{
			base.StartCoroutine(this.RequestSongByKeyCoroutine(key, callback));
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00025EB0 File Offset: 0x000240B0
		public IEnumerator RequestSongByKeyCoroutine(string key, Action<Song> callback)
		{
			UnityWebRequest wwwId = SongDownloader.GetRequestForUrl(PluginSetting.BeatSaverURL + "/api/maps/detail/" + key.ToLower());
			wwwId.timeout = 10;
			yield return wwwId.SendWebRequest();
			if (wwwId.isNetworkError || wwwId.isHttpError)
			{
				Logger.log.Error(wwwId.error);
			}
			else
			{
				Song song = new Song(JObject.Parse(wwwId.downloadHandler.text), false);
				if (callback != null)
				{
					callback(song);
				}
			}
			yield break;
		}

		// Token: 0x0400046C RID: 1132
		private static SongDownloader _instance = null;

		// Token: 0x0400046D RID: 1133
		private List<Song> _alreadyDownloadedSongs;

		// Token: 0x0400046E RID: 1134
		private static bool _extractingZip;

		// Token: 0x0400046F RID: 1135
		private static BeatmapLevelsModel _beatmapLevelsModel;
	}
}
