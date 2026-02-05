using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeatSaverSharp;
using SongCore;
using UnityEngine;

namespace BeatSaverDownloader.Misc
{
	// Token: 0x0200001D RID: 29
	public class SongDownloader : MonoBehaviour
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600013F RID: 319 RVA: 0x00005FD0 File Offset: 0x000041D0
		// (remove) Token: 0x06000140 RID: 320 RVA: 0x00006008 File Offset: 0x00004208
		public event Action<Beatmap> songDownloaded;

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000603D File Offset: 0x0000423D
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00006064 File Offset: 0x00004264
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

		// Token: 0x06000143 RID: 323 RVA: 0x0000606C File Offset: 0x0000426C
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

		// Token: 0x06000144 RID: 324 RVA: 0x000060A0 File Offset: 0x000042A0
		private void SongLoader_SongsLoadedEvent(Loader sender, Dictionary<string, CustomPreviewBeatmapLevel> levels)
		{
			Plugin.log.Debug("Establishing Already Downloaded Songs");
			this._alreadyDownloadedSongs = new HashSet<string>(levels.Values.Select((CustomPreviewBeatmapLevel x) => Collections.hashForLevelID(x.levelID)));
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000060F4 File Offset: 0x000042F4
		public async Task DownloadSong(Beatmap song, CancellationToken token, IProgress<double> progress = null, bool direct = false)
		{
			try
			{
				string customSongsPath = CustomLevelPathHelper.customLevelsDirectoryPath.Replace("CustomLevels", "NetviosBBSDownloadLevels");
				Plugin.log.Info("customSongsPath: " + customSongsPath);
				if (!Directory.Exists(customSongsPath))
				{
					Directory.CreateDirectory(customSongsPath);
				}
				byte[] array = await song.DownloadZip(direct, token, progress).ConfigureAwait(false);
				Plugin.log.Info("Downloaded zip!");
				await this.ExtractZipAsync(song, array, customSongsPath, false).ConfigureAwait(false);
				Action<Beatmap> action = this.songDownloaded;
				if (action != null)
				{
					action(song);
				}
				customSongsPath = null;
			}
			catch (Exception ex)
			{
				if (ex is TaskCanceledException)
				{
					Plugin.log.Warn("Song Download Aborted.");
				}
				else
				{
					Plugin.log.Critical("Failed to download Song!");
				}
				if (this._alreadyDownloadedSongs.Contains(song.Hash))
				{
					this._alreadyDownloadedSongs.Remove(song.Hash);
				}
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006158 File Offset: 0x00004358
		private async Task ExtractZipAsync(Beatmap songInfo, byte[] zip, string customSongsPath, bool overwrite = false)
		{
			Stream zipStream = new MemoryStream(zip);
			try
			{
				Plugin.log.Info("Extracting...");
				SongDownloader._extractingZip = true;
				ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
				string text = string.Concat(new string[]
				{
					songInfo.Key,
					" (",
					songInfo.Metadata.SongName,
					" - ",
					songInfo.Metadata.LevelAuthorName,
					")"
				});
				text = string.Join("", text.Split(Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).ToArray<char>()));
				string path = customSongsPath + "/" + text;
				if (!overwrite && Directory.Exists(path))
				{
					int num = 1;
					while (Directory.Exists(path + string.Format(" ({0})", num)))
					{
						num++;
					}
					path += string.Format(" ({0})", num);
				}
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				Plugin.log.Info(path);
				await Task.Run(delegate
				{
					foreach (ZipArchiveEntry zipArchiveEntry in archive.Entries)
					{
						string text2 = Path.Combine(path, zipArchiveEntry.Name);
						if (overwrite || !File.Exists(text2))
						{
							zipArchiveEntry.ExtractToFile(text2, overwrite);
						}
					}
				}).ConfigureAwait(false);
				archive.Dispose();
			}
			catch (Exception ex)
			{
				Plugin.log.Critical(string.Format("Unable to extract ZIP! Exception: {0}", ex));
				SongDownloader._extractingZip = false;
				return;
			}
			zipStream.Close();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000061B4 File Offset: 0x000043B4
		public void QueuedDownload(string hash)
		{
			if (!SongDownloader.Instance._alreadyDownloadedSongs.Contains(hash))
			{
				SongDownloader.Instance._alreadyDownloadedSongs.Add(hash);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000061D9 File Offset: 0x000043D9
		public bool IsSongDownloaded(string hash)
		{
			return SongDownloader.Instance._alreadyDownloadedSongs.Contains(hash);
		}

		// Token: 0x04000081 RID: 129
		private static SongDownloader _instance;

		// Token: 0x04000082 RID: 130
		private HashSet<string> _alreadyDownloadedSongs;

		// Token: 0x04000083 RID: 131
		private static bool _extractingZip;
	}
}
