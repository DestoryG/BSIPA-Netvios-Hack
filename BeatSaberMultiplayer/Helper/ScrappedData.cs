using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BeatSaberMultiplayer.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000079 RID: 121
	public class ScrappedData : MonoBehaviour
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0002463E File Offset: 0x0002283E
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x00024674 File Offset: 0x00022874
		public static ScrappedData Instance
		{
			get
			{
				if (!ScrappedData._instance)
				{
					ScrappedData._instance = new GameObject("ScrappedData").AddComponent<ScrappedData>();
					Object.DontDestroyOnLoad(ScrappedData._instance.gameObject);
				}
				return ScrappedData._instance;
			}
			private set
			{
				ScrappedData._instance = value;
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0002467C File Offset: 0x0002287C
		public void DownloadScrappedData(Action<List<ScrappedSong>> callback)
		{
			base.StartCoroutine(this.DownloadScrappedDataCoroutine(callback));
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0002468C File Offset: 0x0002288C
		public IEnumerator DownloadScrappedDataCoroutine(Action<List<ScrappedSong>> callback)
		{
			bool timeout = false;
			float time = 0f;
			UnityWebRequest www;
			UnityWebRequestAsyncOperation asyncRequest;
			try
			{
				www = SongDownloader.GetRequestForUrl(ScrappedData.scrappedDataURL);
				asyncRequest = www.SendWebRequest();
				goto IL_00F0;
			}
			catch (Exception ex)
			{
				Logger.log.Error(ex);
				yield break;
			}
			IL_0082:
			yield return null;
			time += Time.deltaTime;
			if (time >= 5f && asyncRequest.progress <= 1E-45f)
			{
				www.Abort();
				timeout = true;
				Logger.log.Error("Connection timed out!");
			}
			IL_00F0:
			if (asyncRequest.isDone)
			{
				if (www.isNetworkError || www.isHttpError || timeout)
				{
					Logger.log.Error("Unable to download scrapped data! " + (www.isNetworkError ? ("Network error: " + www.error) : (www.isHttpError ? ("HTTP error: " + www.error) : "Unknown error")));
				}
				else
				{
					Logger.log.Info("Received response from github.com!");
					Task parsing = new Task(delegate
					{
						ScrappedData.Songs = JsonConvert.DeserializeObject<List<ScrappedSong>>(www.downloadHandler.text);
					});
					parsing.ConfigureAwait(false);
					Logger.log.Info("Parsing scrapped data...");
					Stopwatch timer = new Stopwatch();
					timer.Start();
					parsing.Start();
					yield return new WaitUntil(() => parsing.IsCompleted);
					timer.Stop();
					ScrappedData.Downloaded = true;
					if (callback != null)
					{
						callback(ScrappedData.Songs);
					}
					Logger.log.Info("Scrapped data parsed! Time: " + timer.Elapsed.TotalSeconds.ToString("0.00") + "s");
					timer = null;
				}
				yield break;
			}
			goto IL_0082;
		}

		// Token: 0x0400044C RID: 1100
		private static ScrappedData _instance = null;

		// Token: 0x0400044D RID: 1101
		public static List<ScrappedSong> Songs = new List<ScrappedSong>();

		// Token: 0x0400044E RID: 1102
		public static bool Downloaded;

		// Token: 0x0400044F RID: 1103
		public static string scrappedDataURL = "https://raw.githubusercontent.com/andruzzzhka/BeatSaberScrappedData/master/combinedScrappedData.json";
	}
}
