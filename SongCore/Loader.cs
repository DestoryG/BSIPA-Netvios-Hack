using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BS_Utils.Gameplay;
using BS_Utils.Utilities;
using IPA.Logging;
using Newtonsoft.Json;
using SimpleJSON;
using SongCore.Data;
using SongCore.OverrideClasses;
using SongCore.UI;
using SongCore.Utilities;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace SongCore
{
	// Token: 0x02000012 RID: 18
	public class Loader : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000C4 RID: 196 RVA: 0x00003A8C File Offset: 0x00001C8C
		// (remove) Token: 0x060000C5 RID: 197 RVA: 0x00003AC0 File Offset: 0x00001CC0
		public static event Action<Loader> LoadingStartedEvent;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000C6 RID: 198 RVA: 0x00003AF4 File Offset: 0x00001CF4
		// (remove) Token: 0x060000C7 RID: 199 RVA: 0x00003B28 File Offset: 0x00001D28
		public static event Action<Loader, Dictionary<string, CustomPreviewBeatmapLevel>> SongsLoadedEvent;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000C8 RID: 200 RVA: 0x00003B5C File Offset: 0x00001D5C
		// (remove) Token: 0x060000C9 RID: 201 RVA: 0x00003B90 File Offset: 0x00001D90
		public static event Action OnLevelPacksRefreshed;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000CA RID: 202 RVA: 0x00003BC4 File Offset: 0x00001DC4
		// (remove) Token: 0x060000CB RID: 203 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public static event Action DeletingSong;

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003C2B File Offset: 0x00001E2B
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00003C32 File Offset: 0x00001E32
		public static SongCoreCustomLevelCollection CustomLevelsCollection { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003C3A File Offset: 0x00001E3A
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00003C41 File Offset: 0x00001E41
		public static SongCoreCustomLevelCollection WIPLevelsCollection { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003C49 File Offset: 0x00001E49
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00003C50 File Offset: 0x00001E50
		public static SongCoreCustomLevelCollection CachedWIPLevelCollection { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003C58 File Offset: 0x00001E58
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00003C5F File Offset: 0x00001E5F
		public static SongCoreCustomBeatmapLevelPack CustomLevelsPack { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003C67 File Offset: 0x00001E67
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00003C6E File Offset: 0x00001E6E
		public static SongCoreCustomBeatmapLevelPack WIPLevelsPack { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00003C76 File Offset: 0x00001E76
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00003C7D File Offset: 0x00001E7D
		public static SongCoreCustomBeatmapLevelPack CachedWIPLevelsPack { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003C85 File Offset: 0x00001E85
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00003C8C File Offset: 0x00001E8C
		public static SongCoreBeatmapLevelPackCollectionSO CustomBeatmapLevelPackCollectionSO { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00003C94 File Offset: 0x00001E94
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00003C9B File Offset: 0x00001E9B
		public static SongCoreOstBeatmapLevelPackCollectionSO OstBeatmapLevelPackCollectionSO { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00003CA3 File Offset: 0x00001EA3
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00003CAA File Offset: 0x00001EAA
		public static bool AreSongsLoaded { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00003CB2 File Offset: 0x00001EB2
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00003CB9 File Offset: 0x00001EB9
		public static bool AreSongsLoading { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003CC1 File Offset: 0x00001EC1
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00003CC8 File Offset: 0x00001EC8
		public static float LoadingProgress { get; internal set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public static BeatmapLevelsModel BeatmapLevelsModelSO
		{
			get
			{
				if (Loader._beatmapLevelsModel == null)
				{
					Loader._beatmapLevelsModel = Resources.FindObjectsOfTypeAll<BeatmapLevelsModel>().FirstOrDefault<BeatmapLevelsModel>();
				}
				return Loader._beatmapLevelsModel;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003CF3 File Offset: 0x00001EF3
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00003CFA File Offset: 0x00001EFA
		public static CachedMediaAsyncLoader cachedMediaAsyncLoaderSO { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003D02 File Offset: 0x00001F02
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00003D09 File Offset: 0x00001F09
		public static BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection { get; private set; }

		// Token: 0x060000E7 RID: 231 RVA: 0x00003D11 File Offset: 0x00001F11
		public static void OnLoad()
		{
			if (Loader.Instance != null)
			{
				Loader._beatmapLevelsModel = null;
				Loader.Instance.RefreshLevelPacks();
				return;
			}
			new GameObject("SongCore Loader").AddComponent<Loader>();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00003D41 File Offset: 0x00001F41
		private void Awake()
		{
			Loader.Instance = this;
			this._progressBar = ProgressBar.Create();
			this.MenuLoaded();
			Hashing.ReadCachedSongHashes();
			Object.DontDestroyOnLoad(base.gameObject);
			BSEvents.menuSceneLoaded += this.MenuLoaded;
			this.Initialize();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00003D81 File Offset: 0x00001F81
		private IEnumerator InitNetviosLevels()
		{
			using (UnityWebRequest www = UnityWebRequest.Get(NetviosConfig.GetAPINetviosLevelsURI("v1")))
			{
				Logging.logger.Info("Start download netvios levels");
				yield return www.SendWebRequest();
				if (www.isNetworkError || www.isHttpError)
				{
					Logging.logger.Warn("download netvios levels error:" + www.error);
				}
				else
				{
					List<Loader.ValidSongDownloadData> list = JsonConvert.DeserializeObject<List<Loader.ValidSongDownloadData>>(JSON.Parse(www.downloadHandler.text).ToString());
					Loader.NetviosLevelHashs = new string[list.Count];
					int num = 0;
					foreach (Loader.ValidSongDownloadData validSongDownloadData in list)
					{
						Loader.NetviosLevelHashs[num] = validSongDownloadData.hash;
						num++;
					}
				}
				Logging.logger.Info("Finished download netvios levels");
			}
			UnityWebRequest www = null;
			this.Initialize();
			yield break;
			yield break;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003D90 File Offset: 0x00001F90
		private void Initialize()
		{
			if (Directory.Exists(Converter.oldFolderPath))
			{
				Converter.PrepareExistingLibrary();
				return;
			}
			this.RefreshSongs(true);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003DAC File Offset: 0x00001FAC
		internal void MenuLoaded()
		{
			if (Loader.AreSongsLoading && this._loadingTask != null)
			{
				this._loadingTask.Cancel();
				this._loadingCancelled = true;
				Loader.AreSongsLoading = false;
				Loader.LoadingProgress = 0f;
				base.StopAllCoroutines();
				this._progressBar.ShowMessage("Loading cancelled\n<size=80%>Press Ctrl+R to refresh</size>");
				Logging.Log("Loading was cancelled by player since they loaded another scene.");
			}
			Gamemode.Init();
			if (Loader._customLevelLoader == null)
			{
				Loader._customLevelLoader = Resources.FindObjectsOfTypeAll<CustomLevelLoader>().FirstOrDefault<CustomLevelLoader>();
				if (Loader._customLevelLoader)
				{
					Texture2D field = Loader._customLevelLoader.GetField("_defaultPackCoverTexture2D");
					Loader.defaultCoverImage = Sprite.Create(field, new Rect(0f, 0f, (float)field.width, (float)field.height), new Vector2(0.5f, 0.5f));
					Loader.cachedMediaAsyncLoaderSO = Loader._customLevelLoader.GetField("_cachedMediaAsyncLoaderSO");
					Loader.beatmapCharacteristicCollection = Loader._customLevelLoader.GetField("_beatmapCharacteristicCollection");
					return;
				}
				Texture2D blackTexture = Texture2D.blackTexture;
				Loader.defaultCoverImage = Sprite.Create(blackTexture, new Rect(0f, 0f, (float)blackTexture.width, (float)blackTexture.height), new Vector2(0.5f, 0.5f));
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003EE8 File Offset: 0x000020E8
		public void RefreshLevelPacks()
		{
			SongCoreCustomLevelCollection customLevelsCollection = Loader.CustomLevelsCollection;
			Dictionary<string, CustomPreviewBeatmapLevel> customLevels = Loader.CustomLevels;
			CustomPreviewBeatmapLevel[] array;
			if (customLevels == null)
			{
				array = null;
			}
			else
			{
				Dictionary<string, CustomPreviewBeatmapLevel>.ValueCollection values = customLevels.Values;
				if (values == null)
				{
					array = null;
				}
				else
				{
					array = values.OrderBy((CustomPreviewBeatmapLevel l) => l.songName).ToArray<CustomPreviewBeatmapLevel>();
				}
			}
			customLevelsCollection.UpdatePreviewLevels(array);
			SongCoreCustomLevelCollection wiplevelsCollection = Loader.WIPLevelsCollection;
			Dictionary<string, CustomPreviewBeatmapLevel> customWIPLevels = Loader.CustomWIPLevels;
			CustomPreviewBeatmapLevel[] array2;
			if (customWIPLevels == null)
			{
				array2 = null;
			}
			else
			{
				Dictionary<string, CustomPreviewBeatmapLevel>.ValueCollection values2 = customWIPLevels.Values;
				if (values2 == null)
				{
					array2 = null;
				}
				else
				{
					array2 = values2.OrderBy((CustomPreviewBeatmapLevel l) => l.songName).ToArray<CustomPreviewBeatmapLevel>();
				}
			}
			wiplevelsCollection.UpdatePreviewLevels(array2);
			SongCoreCustomLevelCollection cachedWIPLevelCollection = Loader.CachedWIPLevelCollection;
			if (cachedWIPLevelCollection != null)
			{
				Dictionary<string, CustomPreviewBeatmapLevel> cachedWIPLevels = Loader.CachedWIPLevels;
				CustomPreviewBeatmapLevel[] array3;
				if (cachedWIPLevels == null)
				{
					array3 = null;
				}
				else
				{
					Dictionary<string, CustomPreviewBeatmapLevel>.ValueCollection values3 = cachedWIPLevels.Values;
					if (values3 == null)
					{
						array3 = null;
					}
					else
					{
						array3 = values3.OrderBy((CustomPreviewBeatmapLevel l) => l.songName).ToArray<CustomPreviewBeatmapLevel>();
					}
				}
				cachedWIPLevelCollection.UpdatePreviewLevels(array3);
			}
			if (Loader.CachedWIPLevels.Count > 0 && Loader.CachedWIPLevelsPack != null && !Loader.CustomBeatmapLevelPackCollectionSO._customBeatmapLevelPacks.Contains(Loader.CachedWIPLevelsPack))
			{
				Loader.CustomBeatmapLevelPackCollectionSO.AddLevelPack(Loader.CachedWIPLevelsPack);
			}
			foreach (SeperateSongFolder seperateSongFolder in Loader.SeperateSongFolders)
			{
				if (seperateSongFolder.SongFolderEntry.Pack == FolderLevelPack.NewPack)
				{
					seperateSongFolder.LevelCollection.UpdatePreviewLevels(seperateSongFolder.Levels.Values.OrderBy((CustomPreviewBeatmapLevel l) => l.songName).ToArray<CustomPreviewBeatmapLevel>());
					if ((seperateSongFolder.Levels.Count > 0 || (seperateSongFolder is ModSeperateSongFolder && (seperateSongFolder as ModSeperateSongFolder).AlwaysShow)) && !Loader.CustomBeatmapLevelPackCollectionSO._customBeatmapLevelPacks.Contains(seperateSongFolder.LevelPack))
					{
						Loader.CustomBeatmapLevelPackCollectionSO.AddLevelPack(seperateSongFolder.LevelPack);
					}
				}
			}
			BeatmapLevelPackCollectionSO ostAndExtrasPackCollection = Loader.BeatmapLevelsModelSO.ostAndExtrasPackCollection;
			Loader.OstBeatmapLevelPackCollectionSO = SongCoreOstBeatmapLevelPackCollectionSO.CreateNew();
			foreach (IBeatmapLevelPack beatmapLevelPack in ostAndExtrasPackCollection.beatmapLevelPacks)
			{
				if (!(beatmapLevelPack.packID == "OstVol3") && !(beatmapLevelPack.packID == "Camellia") && !(null == beatmapLevelPack as BeatmapLevelPackSO))
				{
					Loader.OstBeatmapLevelPackCollectionSO.AddLevelPack((BeatmapLevelPackSO)beatmapLevelPack);
				}
			}
			foreach (CustomBeatmapLevelPack customBeatmapLevelPack in Loader.CustomBeatmapLevelPackCollectionSO._customBeatmapLevelPacks)
			{
				Loader.OstBeatmapLevelPackCollectionSO.AddLevelPack(customBeatmapLevelPack);
			}
			Loader.BeatmapLevelsModelSO.SetField("_ostAndExtrasPackCollection", Loader.OstBeatmapLevelPackCollectionSO);
			Loader.BeatmapLevelsModelSO.SetField("_customLevelPackCollection", Loader.CustomBeatmapLevelPackCollectionSO);
			Loader.BeatmapLevelsModelSO.UpdateAllLoadedBeatmapLevelPacks();
			Loader.BeatmapLevelsModelSO.UpdateLoadedPreviewLevels();
			LevelFilteringNavigationController levelFilteringNavigationController = Resources.FindObjectsOfTypeAll<LevelFilteringNavigationController>().FirstOrDefault<LevelFilteringNavigationController>();
			levelFilteringNavigationController.InitPlaylists();
			levelFilteringNavigationController.UpdatePlaylistsData();
			this.AttemptReselectCurrentLevelPack(levelFilteringNavigationController);
			Action onLevelPacksRefreshed = Loader.OnLevelPacksRefreshed;
			if (onLevelPacksRefreshed == null)
			{
				return;
			}
			onLevelPacksRefreshed();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004224 File Offset: 0x00002424
		internal void AttemptReselectCurrentLevelPack(LevelFilteringNavigationController controller)
		{
			TabBarViewController field = controller.GetField("_tabBarViewController");
			if (field == null || field.selectedCellNumber != 3)
			{
				return;
			}
			object[] field2 = controller.GetField("_tabBarDatas");
			if (field2 == null)
			{
				return;
			}
			int field3 = field2[field.selectedCellNumber].GetField("selectedItem");
			IAnnotatedBeatmapLevelCollection[] field4 = field2[field.selectedCellNumber].GetField("annotatedBeatmapLevelCollections");
			if (field4 == null)
			{
				return;
			}
			int num = field4.Length;
			if (field3 >= num)
			{
				return;
			}
			controller.SelectBeatmapLevelPackOrPlayList(field4[field3] as IBeatmapLevelPack, null);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000042A8 File Offset: 0x000024A8
		public void RefreshSongs(bool fullRefresh = true)
		{
			if (SceneManager.GetActiveScene().name == "GameCore")
			{
				return;
			}
			if (Loader.AreSongsLoading)
			{
				return;
			}
			Logging.Log(fullRefresh ? "Starting full song refresh" : "Starting song refresh");
			Loader.AreSongsLoaded = false;
			Loader.AreSongsLoading = true;
			Loader.LoadingProgress = 0f;
			this._loadingCancelled = false;
			if (Loader.LoadingStartedEvent != null)
			{
				try
				{
					Loader.LoadingStartedEvent(this);
				}
				catch (Exception ex)
				{
					Logging.Log("Some plugin is throwing exception from the LoadingStartedEvent!", IPA.Logging.Logger.Level.Error);
					Logging.Log(ex.ToString(), IPA.Logging.Logger.Level.Error);
				}
			}
			this.RetrieveAllSongs(fullRefresh);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000434C File Offset: 0x0000254C
		private void RetrieveAllSongs(bool fullRefresh)
		{
			Stopwatch stopwatch = new Stopwatch();
			if (fullRefresh)
			{
				Loader.hasLoadCustomLevelIds.Clear();
				Loader.CustomLevels.Clear();
				Loader.CustomWIPLevels.Clear();
				Loader.CachedWIPLevels.Clear();
				Collections.levelHashDictionary.Clear();
				Collections.hashLevelDictionary.Clear();
				foreach (SeperateSongFolder seperateSongFolder in Loader.SeperateSongFolders)
				{
					seperateSongFolder.Levels.Clear();
				}
			}
			HashSet<string> foundSongPaths = (fullRefresh ? new HashSet<string>() : new HashSet<string>(Hashing.cachedSongHashData.Keys));
			string baseProjectPath = CustomLevelPathHelper.baseProjectPath;
			string customLevelsPath = CustomLevelPathHelper.customLevelsDirectoryPath.Replace("CustomLevels", Plugin.costomLevelsDirName);
			Action action = delegate
			{
				try
				{
					string text = CustomLevelPathHelper.baseProjectPath;
					text = text.Replace('\\', '/');
					if (!Directory.Exists(customLevelsPath))
					{
						Directory.CreateDirectory(customLevelsPath);
					}
					if (!Directory.Exists(baseProjectPath + "/CustomWIPLevels"))
					{
						Directory.CreateDirectory(baseProjectPath + "/CustomWIPLevels");
					}
					if (fullRefresh)
					{
						try
						{
							string text2 = Path.Combine(text, "CustomWIPLevels");
							string text3 = Path.Combine(text, "CustomWIPLevels", "Cache");
							if (!Directory.Exists(text3))
							{
								Directory.CreateDirectory(text3);
							}
							DirectoryInfo directoryInfo = new DirectoryInfo(text3);
							FileInfo[] array = directoryInfo.GetFiles();
							int i;
							for (i = 0; i < array.Length; i++)
							{
								array[i].Delete();
							}
							DirectoryInfo[] array2 = directoryInfo.GetDirectories();
							for (i = 0; i < array2.Length; i++)
							{
								array2[i].Delete(true);
							}
							foreach (string text4 in Directory.GetFiles(text2, "*.zip", SearchOption.TopDirectoryOnly))
							{
								Unzip unzip = new Unzip(text4);
								try
								{
									unzip.ExtractToDirectory(text3 + "/" + new FileInfo(text4).Name);
								}
								catch (Exception ex)
								{
									IPA.Logging.Logger logger = Logging.logger;
									string text5 = "Failed to extract zip: ";
									string text6 = text4;
									string text7 = ": ";
									Exception ex2 = ex;
									logger.Warn(text5 + text6 + text7 + ((ex2 != null) ? ex2.ToString() : null));
								}
								unzip.Dispose();
							}
							string[] array3 = Directory.GetDirectories(text3).ToArray<string>();
							i = 0;
							while (i < array3.Length)
							{
								string text8 = array3[i];
								string[] files;
								try
								{
									files = Directory.GetFiles(text8, "info.dat", SearchOption.TopDirectoryOnly);
								}
								catch (DirectoryNotFoundException)
								{
									Logging.Log("Skipping missing or corrupt folder: '" + text8 + "'", IPA.Logging.Logger.Level.Warning);
									goto IL_02C7;
								}
								goto IL_01C7;
								IL_02C7:
								i++;
								continue;
								IL_01C7:
								if (files.Length == 0)
								{
									Logging.Log("Folder: '" + text8 + "' is missing info.dat files!", IPA.Logging.Logger.Level.Notice);
									goto IL_02C7;
								}
								foreach (string text9 in files)
								{
									try
									{
										string songPath2 = Path.GetDirectoryName(text9.Replace('\\', '/'));
										if (fullRefresh || !Loader.CachedWIPLevels.ContainsKey(songPath2) || Loader.CachedWIPLevels[songPath2] == null)
										{
											StandardLevelInfoSaveData saveData2 = Loader.GetStandardLevelInfoSaveData(songPath2);
											if (saveData2 != null)
											{
												PersistentSingleton<HMMainThreadDispatcher>.instance.Enqueue(delegate
												{
													if (this._loadingCancelled)
													{
														return;
													}
													string text28;
													CustomPreviewBeatmapLevel customPreviewBeatmapLevel3 = Loader.LoadSong(saveData2, songPath2, out text28, null);
													if (customPreviewBeatmapLevel3 != null)
													{
														Loader.CachedWIPLevels[songPath2] = customPreviewBeatmapLevel3;
													}
												});
											}
										}
									}
									catch (Exception ex3)
									{
										IPA.Logging.Logger logger2 = Logging.logger;
										string text10 = "Failed to load song from ";
										string text11 = text8;
										string text12 = ": ";
										Exception ex4 = ex3;
										logger2.Notice(text10 + text11 + text12 + ((ex4 != null) ? ex4.ToString() : null));
									}
								}
								goto IL_02C7;
							}
						}
						catch (Exception ex5)
						{
							IPA.Logging.Logger logger3 = Logging.logger;
							string text13 = "Failed To Load Cached WIP Levels: ";
							Exception ex6 = ex5;
							logger3.Error(text13 + ((ex6 != null) ? ex6.ToString() : null));
						}
					}
					stopwatch.Start();
					string text14 = Path.Combine(text, Plugin.costomLevelsDirName);
					string[] files2 = Directory.GetFiles(text14, "*.zip", SearchOption.TopDirectoryOnly);
					foreach (string text15 in files2)
					{
						Unzip unzip2 = new Unzip(text15);
						try
						{
							string text16 = new FileInfo(text15).Name;
							text16 = text16.TrimEnd(".zip");
							string text17 = text14 + "/" + text16;
							if (File.Exists(text17))
							{
								DirectoryInfo directoryInfo2 = new DirectoryInfo(text17);
								FileInfo[] array = directoryInfo2.GetFiles();
								for (int j = 0; j < array.Length; j++)
								{
									array[j].Delete();
								}
								DirectoryInfo[] array2 = directoryInfo2.GetDirectories();
								for (int j = 0; j < array2.Length; j++)
								{
									array2[j].Delete(true);
								}
							}
							unzip2.ExtractToDirectory(text17);
						}
						catch (Exception ex7)
						{
							IPA.Logging.Logger logger4 = Logging.logger;
							string text18 = "Failed to extract zip: ";
							string text19 = text15;
							string text20 = ": ";
							Exception ex8 = ex7;
							logger4.Warn(text18 + text19 + text20 + ((ex8 != null) ? ex8.ToString() : null));
						}
						unzip2.Dispose();
					}
					foreach (string text21 in files2)
					{
						File.SetAttributes(text21, FileAttributes.Normal);
						File.Delete(text21);
					}
					List<string> songFolders = Directory.GetDirectories(text + "/" + Plugin.costomLevelsDirName).ToList<string>().Concat(Directory.GetDirectories(text + "/CustomWIPLevels"))
						.ToList<string>();
					new List<string>();
					float num = 0f;
					float count = 0f;
					foreach (string text22 in songFolders)
					{
						num += 1f;
						string[] files3;
						try
						{
							files3 = Directory.GetFiles(text22, "info.dat", SearchOption.TopDirectoryOnly);
						}
						catch (DirectoryNotFoundException)
						{
							Logging.Log("Skipping missing or corrupt folder: '" + text22 + "'", IPA.Logging.Logger.Level.Warning);
							continue;
						}
						if (files3.Length == 0)
						{
							Logging.Log("Folder: '" + text22 + "' is missing info.dat files!", IPA.Logging.Logger.Level.Notice);
						}
						else
						{
							foreach (string text23 in files3)
							{
								try
								{
									string songPath3 = Path.GetDirectoryName(text23.Replace('\\', '/'));
									if (!(Directory.GetParent(songPath3).Name == "Backups"))
									{
										if (fullRefresh || !Loader.CustomLevels.ContainsKey(songPath3) || Loader.CustomLevels[songPath3] == null)
										{
											bool wip = songPath3.Contains("CustomWIPLevels");
											StandardLevelInfoSaveData saveData3 = Loader.GetStandardLevelInfoSaveData(songPath3);
											if (saveData3 != null)
											{
												PersistentSingleton<HMMainThreadDispatcher>.instance.Enqueue(delegate
												{
													if (this._loadingCancelled)
													{
														return;
													}
													string text29;
													CustomPreviewBeatmapLevel customPreviewBeatmapLevel4 = Loader.LoadSong(saveData3, songPath3, out text29, null);
													if (customPreviewBeatmapLevel4 != null)
													{
														float count4 = count;
														count = count4 + 1f;
														if (!Collections.levelHashDictionary.ContainsKey(customPreviewBeatmapLevel4.levelID))
														{
															Collections.levelHashDictionary.Add(customPreviewBeatmapLevel4.levelID, text29);
															List<string> list;
															if (Collections.hashLevelDictionary.TryGetValue(text29, out list))
															{
																list.Add(customPreviewBeatmapLevel4.levelID);
															}
															else
															{
																list = new List<string>();
																list.Add(customPreviewBeatmapLevel4.levelID);
																Collections.hashLevelDictionary.Add(text29, list);
															}
														}
														if (!wip && !Loader.hasLoadCustomLevelIds.Contains(customPreviewBeatmapLevel4.levelID))
														{
															Loader.CustomLevels[songPath3] = customPreviewBeatmapLevel4;
															Loader.hasLoadCustomLevelIds.Add(customPreviewBeatmapLevel4.levelID);
														}
														else
														{
															Loader.CustomWIPLevels[songPath3] = customPreviewBeatmapLevel4;
														}
														foundSongPaths.Add(songPath3);
													}
													Loader.LoadingProgress = count / (float)songFolders.Count;
												});
											}
										}
									}
								}
								catch (Exception ex9)
								{
									Logging.Log("Failed to load song folder: " + text23, IPA.Logging.Logger.Level.Error);
									Logging.Log(ex9.ToString(), IPA.Logging.Logger.Level.Error);
								}
							}
						}
					}
					for (int k = 0; k < Loader.SeperateSongFolders.Count; k++)
					{
						try
						{
							SeperateSongFolder entry = Loader.SeperateSongFolders[k];
							ProgressBar progressBar = Loader.Instance._progressBar;
							string text24 = "Loading ";
							int i = Loader.SeperateSongFolders.Count - k;
							progressBar.ShowMessage(text24 + i.ToString() + " Additional Song folders");
							if (Directory.Exists(entry.SongFolderEntry.Path))
							{
								List<string> entryFolders = Directory.GetDirectories(entry.SongFolderEntry.Path).ToList<string>();
								float num2 = 0f;
								float count2 = 0f;
								foreach (string text25 in entryFolders)
								{
									num2 += 1f;
									string[] files4;
									try
									{
										files4 = Directory.GetFiles(text25, "info.dat", SearchOption.TopDirectoryOnly);
									}
									catch (DirectoryNotFoundException)
									{
										Logging.Log("Skipping missing or corrupt folder: '" + text25 + "'", IPA.Logging.Logger.Level.Warning);
										continue;
									}
									if (files4.Length == 0)
									{
										Logging.Log("Folder: '" + text25 + "' is missing info.dat files!", IPA.Logging.Logger.Level.Notice);
									}
									else
									{
										foreach (string text26 in files4)
										{
											try
											{
												string songPath = Path.GetDirectoryName(text26.Replace('\\', '/'));
												if (fullRefresh || !entry.Levels.ContainsKey(songPath) || entry.Levels[songPath] == null)
												{
													if (entry.SongFolderEntry.Pack == FolderLevelPack.CustomLevels)
													{
														if (Loader.CustomLevels.ContainsKey(songPath) && Loader.CustomLevels[songPath] != null)
														{
															goto IL_09BA;
														}
													}
													else if (entry.SongFolderEntry.Pack == FolderLevelPack.CustomWIPLevels)
													{
														if (Loader.CustomWIPLevels.ContainsKey(songPath) && Loader.CustomWIPLevels[songPath] != null)
														{
															goto IL_09BA;
														}
													}
													else if (entry.SongFolderEntry.Pack == FolderLevelPack.CustomLevels || (entry.SongFolderEntry.Pack == FolderLevelPack.NewPack && !entry.SongFolderEntry.WIP))
													{
														if (Loader.CustomLevels.ContainsKey(songPath))
														{
															CustomPreviewBeatmapLevel customPreviewBeatmapLevel = Loader.CustomLevels[songPath];
															if (customPreviewBeatmapLevel != null)
															{
																entry.Levels[songPath] = customPreviewBeatmapLevel;
																goto IL_09BA;
															}
														}
														if (Loader.CustomWIPLevels.ContainsKey(songPath))
														{
															CustomPreviewBeatmapLevel customPreviewBeatmapLevel2 = Loader.CustomWIPLevels[songPath];
															if (customPreviewBeatmapLevel2 != null)
															{
																entry.Levels[songPath] = customPreviewBeatmapLevel2;
																goto IL_09BA;
															}
														}
													}
													StandardLevelInfoSaveData saveData = Loader.GetStandardLevelInfoSaveData(songPath);
													if (saveData == null)
													{
														Logging.Log("Null save data", IPA.Logging.Logger.Level.Notice);
													}
													else
													{
														PersistentSingleton<HMMainThreadDispatcher>.instance.Enqueue(delegate
														{
															if (this._loadingCancelled)
															{
																return;
															}
															string text30;
															CustomPreviewBeatmapLevel customPreviewBeatmapLevel5 = Loader.LoadSong(saveData, songPath, out text30, entry.SongFolderEntry);
															if (customPreviewBeatmapLevel5 == null)
															{
																Logging.Log("Null load song", IPA.Logging.Logger.Level.Notice);
															}
															if (customPreviewBeatmapLevel5 != null)
															{
																float count3 = count2;
																count2 = count3 + 1f;
																if (!Collections.levelHashDictionary.ContainsKey(customPreviewBeatmapLevel5.levelID))
																{
																	Collections.levelHashDictionary.Add(customPreviewBeatmapLevel5.levelID, text30);
																	List<string> list2;
																	if (Collections.hashLevelDictionary.TryGetValue(text30, out list2))
																	{
																		list2.Add(customPreviewBeatmapLevel5.levelID);
																	}
																	else
																	{
																		list2 = new List<string>();
																		list2.Add(customPreviewBeatmapLevel5.levelID);
																		Collections.hashLevelDictionary.Add(text30, list2);
																	}
																}
																entry.Levels[songPath] = customPreviewBeatmapLevel5;
																foundSongPaths.Add(songPath);
															}
															Loader.LoadingProgress = count2 / (float)entryFolders.Count;
														});
													}
												}
											}
											catch (Exception ex10)
											{
												Logging.Log("Failed to load song folder: " + text26, IPA.Logging.Logger.Level.Error);
												Logging.Log(ex10.ToString(), IPA.Logging.Logger.Level.Error);
											}
											IL_09BA:;
										}
									}
								}
							}
						}
						catch (Exception ex11)
						{
							string text27 = "Failed to load Seperate Folder";
							string name = Loader.SeperateSongFolders[k].SongFolderEntry.Name;
							Exception ex12 = ex11;
							Logging.Log(text27 + name + ((ex12 != null) ? ex12.ToString() : null), IPA.Logging.Logger.Level.Error);
						}
					}
				}
				catch (Exception ex13)
				{
					Logging.Log("RetrieveAllSongs failed:", IPA.Logging.Logger.Level.Error);
					Logging.Log(ex13.ToString(), IPA.Logging.Logger.Level.Error);
				}
			};
			Action action2 = delegate
			{
				stopwatch.Stop();
				int num3 = Loader.CustomLevels.Count + Loader.CustomWIPLevels.Count;
				int num4 = num3;
				foreach (SeperateSongFolder seperateSongFolder2 in Loader.SeperateSongFolders)
				{
					num3 += seperateSongFolder2.Levels.Count;
				}
				Logging.Log(string.Concat(new string[]
				{
					"Loaded ",
					num3.ToString(),
					" new songs (",
					num4.ToString(),
					" in ",
					Plugin.costomLevelsDirName,
					" | ",
					(num3 - num4).ToString(),
					" in seperate folders) in ",
					stopwatch.Elapsed.TotalSeconds.ToString(),
					" seconds"
				}));
				try
				{
					if (Loader.CustomBeatmapLevelPackCollectionSO == null)
					{
						Loader.CustomBeatmapLevelPackCollectionSO = SongCoreBeatmapLevelPackCollectionSO.CreateNew();
						foreach (SeperateSongFolder seperateSongFolder3 in Loader.SeperateSongFolders)
						{
							FolderLevelPack pack = seperateSongFolder3.SongFolderEntry.Pack;
							if (pack != FolderLevelPack.CustomLevels)
							{
								if (pack == FolderLevelPack.CustomWIPLevels)
								{
									Loader.CustomWIPLevels = Loader.CustomWIPLevels.Concat(seperateSongFolder3.Levels.Where((KeyValuePair<string, CustomPreviewBeatmapLevel> x) => !Loader.CustomWIPLevels.ContainsKey(x.Key))).ToDictionary((KeyValuePair<string, CustomPreviewBeatmapLevel> x) => x.Key, (KeyValuePair<string, CustomPreviewBeatmapLevel> x) => x.Value);
								}
							}
							else
							{
								Loader.CustomLevels = Loader.CustomLevels.Concat(seperateSongFolder3.Levels.Where((KeyValuePair<string, CustomPreviewBeatmapLevel> x) => !Loader.CustomLevels.ContainsKey(x.Key))).ToDictionary((KeyValuePair<string, CustomPreviewBeatmapLevel> x) => x.Key, (KeyValuePair<string, CustomPreviewBeatmapLevel> x) => x.Value);
							}
						}
						Loader.CustomLevelsCollection = new SongCoreCustomLevelCollection(Loader.CustomLevels.Values.ToArray<CustomPreviewBeatmapLevel>());
						Loader.WIPLevelsCollection = new SongCoreCustomLevelCollection(Loader.CustomWIPLevels.Values.ToArray<CustomPreviewBeatmapLevel>());
						Loader.CustomLevelsPack = new SongCoreCustomBeatmapLevelPack("custom_levelpack_" + Plugin.costomLevelsDirName, "下载歌曲", BasicUI.DownloadIcon, Loader.CustomLevelsCollection, "");
						Loader.WIPLevelsPack = new SongCoreCustomBeatmapLevelPack("custom_levelpack_CustomWIPLevels", "WIP Levels", BasicUI.WIPIcon, Loader.WIPLevelsCollection, "");
						Loader.CustomBeatmapLevelPackCollectionSO.AddLevelPack(Loader.CustomLevelsPack);
						if (Loader.CachedWIPLevels.Count > 0 && Loader.CachedWIPLevelCollection == null)
						{
							Loader.CachedWIPLevelCollection = new SongCoreCustomLevelCollection(Loader.CachedWIPLevels.Values.ToArray<CustomPreviewBeatmapLevel>());
							Loader.CachedWIPLevelsPack = new SongCoreCustomBeatmapLevelPack("custom_levelpack_CachedWIPLevels", "Cached WIP Levels", BasicUI.WIPIcon, Loader.CachedWIPLevelCollection, "");
							Loader.CustomBeatmapLevelPackCollectionSO.AddLevelPack(Loader.CachedWIPLevelsPack);
						}
						foreach (SeperateSongFolder seperateSongFolder4 in Loader.SeperateSongFolders)
						{
							if (seperateSongFolder4.SongFolderEntry.Pack == FolderLevelPack.NewPack)
							{
								seperateSongFolder4.LevelCollection.UpdatePreviewLevels(seperateSongFolder4.Levels.Values.OrderBy((CustomPreviewBeatmapLevel l) => l.songName).ToArray<CustomPreviewBeatmapLevel>());
								if (!Loader.CustomBeatmapLevelPackCollectionSO._customBeatmapLevelPacks.Contains(seperateSongFolder4.LevelPack))
								{
									Loader.CustomBeatmapLevelPackCollectionSO.AddLevelPack(seperateSongFolder4.LevelPack);
								}
							}
						}
						Loader.CustomBeatmapLevelPackCollectionSO.AddLevelPack(Loader.WIPLevelsPack);
					}
					this.RefreshLevelPacks();
				}
				catch (Exception ex14)
				{
					IPA.Logging.Logger logger5 = Logging.logger;
					string text31 = "Failed to Setup LevelPacks: ";
					Exception ex15 = ex14;
					logger5.Error(text31 + ((ex15 != null) ? ex15.ToString() : null));
				}
				Loader.AreSongsLoaded = true;
				Loader.AreSongsLoading = false;
				Loader.LoadingProgress = 1f;
				this._loadingTask = null;
				Action<Loader, Dictionary<string, CustomPreviewBeatmapLevel>> songsLoadedEvent = Loader.SongsLoadedEvent;
				if (songsLoadedEvent != null)
				{
					songsLoadedEvent(this, Loader.CustomLevels);
				}
				Hashing.UpdateCachedHashes(foundSongPaths);
				Collections.SaveExtraSongData();
			};
			this._loadingTask = new HMTask(action, action2);
			this._loadingTask.Run();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004480 File Offset: 0x00002680
		public static StandardLevelInfoSaveData GetStandardLevelInfoSaveData(string path)
		{
			return StandardLevelInfoSaveData.DeserializeFromJSONString(File.ReadAllText(path + "/info.dat"));
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004498 File Offset: 0x00002698
		public void DeleteSong(string folderPath, bool deleteFolder = true)
		{
			Action deletingSong = Loader.DeletingSong;
			if (deletingSong != null)
			{
				deletingSong();
			}
			try
			{
				CustomPreviewBeatmapLevel customPreviewBeatmapLevel = null;
				if (Loader.CustomLevels.TryGetValue(folderPath, out customPreviewBeatmapLevel))
				{
					Loader.CustomLevels.Remove(folderPath);
				}
				else if (Loader.CustomWIPLevels.TryGetValue(folderPath, out customPreviewBeatmapLevel))
				{
					Loader.CustomWIPLevels.Remove(folderPath);
				}
				else if (Loader.CachedWIPLevels.TryGetValue(folderPath, out customPreviewBeatmapLevel))
				{
					Loader.CachedWIPLevels.Remove(folderPath);
				}
				else
				{
					foreach (SeperateSongFolder seperateSongFolder in Loader.SeperateSongFolders)
					{
						if (seperateSongFolder.Levels.TryGetValue(folderPath, out customPreviewBeatmapLevel))
						{
							seperateSongFolder.Levels.Remove(folderPath);
						}
					}
				}
				if (customPreviewBeatmapLevel != null)
				{
					if (Collections.levelHashDictionary.ContainsKey(customPreviewBeatmapLevel.levelID))
					{
						string text = Collections.hashForLevelID(customPreviewBeatmapLevel.levelID);
						Collections.levelHashDictionary.Remove(customPreviewBeatmapLevel.levelID);
						if (Collections.hashLevelDictionary.ContainsKey(text))
						{
							Collections.hashLevelDictionary[text].Remove(customPreviewBeatmapLevel.levelID);
							if (Collections.hashLevelDictionary[text].Count == 0)
							{
								Collections.hashLevelDictionary.Remove(text);
							}
						}
					}
					Hashing.UpdateCachedHashes(new HashSet<string>(Loader.CustomLevels.Keys.Concat(Loader.CustomWIPLevels.Keys)));
				}
				if (deleteFolder && Directory.Exists(folderPath))
				{
					Directory.Delete(folderPath, true);
				}
				this.RefreshLevelPacks();
			}
			catch (Exception ex)
			{
				Logging.Log("Exception trying to Delete song: " + folderPath, IPA.Logging.Logger.Level.Error);
				Logging.Log(ex.ToString(), IPA.Logging.Logger.Level.Error);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004664 File Offset: 0x00002864
		public static CustomPreviewBeatmapLevel LoadSong(StandardLevelInfoSaveData saveData, string songPath, out string hash, SongFolderEntry folderEntry = null)
		{
			bool flag = songPath.Contains("CustomWIPLevels");
			if (folderEntry != null)
			{
				if (folderEntry.Pack == FolderLevelPack.CustomWIPLevels)
				{
					flag = true;
				}
				else if (folderEntry.WIP)
				{
					flag = true;
				}
			}
			hash = Hashing.GetCustomLevelHash(saveData, songPath);
			CustomPreviewBeatmapLevel customPreviewBeatmapLevel;
			try
			{
				string name = new DirectoryInfo(songPath).Name;
				string text = "custom_level_" + hash;
				if (flag)
				{
					text += " WIP";
				}
				string songName = saveData.songName;
				string songSubName = saveData.songSubName;
				string songAuthorName = saveData.songAuthorName;
				string levelAuthorName = saveData.levelAuthorName;
				float beatsPerMinute = saveData.beatsPerMinute;
				float songTimeOffset = saveData.songTimeOffset;
				float shuffle = saveData.shuffle;
				float shufflePeriod = saveData.shufflePeriod;
				float previewStartTime = saveData.previewStartTime;
				float previewDuration = saveData.previewDuration;
				EnvironmentInfoSO environmentInfoSO = Loader._customLevelLoader.LoadEnvironmentInfo(saveData.environmentName, false);
				EnvironmentInfoSO environmentInfoSO2 = Loader._customLevelLoader.LoadEnvironmentInfo(saveData.allDirectionsEnvironmentName, true);
				List<PreviewDifficultyBeatmapSet> list = new List<PreviewDifficultyBeatmapSet>();
				foreach (StandardLevelInfoSaveData.DifficultyBeatmapSet difficultyBeatmapSet in saveData.difficultyBeatmapSets)
				{
					BeatmapCharacteristicSO beatmapCharacteristicBySerializedName = Loader.beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(difficultyBeatmapSet.beatmapCharacteristicName);
					BeatmapDifficulty[] array = new BeatmapDifficulty[difficultyBeatmapSet.difficultyBeatmaps.Length];
					for (int j = 0; j < difficultyBeatmapSet.difficultyBeatmaps.Length; j++)
					{
						BeatmapDifficulty beatmapDifficulty;
						BeatmapDifficultySerializedMethods.BeatmapDifficultyFromSerializedName(difficultyBeatmapSet.difficultyBeatmaps[j].difficulty, ref beatmapDifficulty);
						array[j] = beatmapDifficulty;
					}
					list.Add(new PreviewDifficultyBeatmapSet(beatmapCharacteristicBySerializedName, array));
				}
				customPreviewBeatmapLevel = new CustomPreviewBeatmapLevel(Loader.defaultCoverImage.texture, saveData, songPath, Loader.cachedMediaAsyncLoaderSO, Loader.cachedMediaAsyncLoaderSO, text, songName, songSubName, songAuthorName, levelAuthorName, beatsPerMinute, songTimeOffset, shuffle, shufflePeriod, previewStartTime, previewDuration, environmentInfoSO, environmentInfoSO2, list.ToArray());
			}
			catch
			{
				Logging.Log("Failed to Load Song: " + songPath, IPA.Logging.Logger.Level.Error);
				customPreviewBeatmapLevel = null;
			}
			return customPreviewBeatmapLevel;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004840 File Offset: 0x00002A40
		private void Update()
		{
			if (Input.GetKeyDown(114))
			{
				this.InitNetviosLevels();
				this.RefreshSongs(Input.GetKey(306));
			}
		}

		// Token: 0x0400002F RID: 47
		public static HashSet<string> hasLoadCustomLevelIds = new HashSet<string>();

		// Token: 0x04000030 RID: 48
		public static Dictionary<string, CustomPreviewBeatmapLevel> CustomLevels = new Dictionary<string, CustomPreviewBeatmapLevel>();

		// Token: 0x04000031 RID: 49
		public static Dictionary<string, CustomPreviewBeatmapLevel> CustomWIPLevels = new Dictionary<string, CustomPreviewBeatmapLevel>();

		// Token: 0x04000032 RID: 50
		public static Dictionary<string, CustomPreviewBeatmapLevel> CachedWIPLevels = new Dictionary<string, CustomPreviewBeatmapLevel>();

		// Token: 0x04000033 RID: 51
		public static List<SeperateSongFolder> SeperateSongFolders = new List<SeperateSongFolder>();

		// Token: 0x0400003F RID: 63
		internal ProgressBar _progressBar;

		// Token: 0x04000040 RID: 64
		private HMTask _loadingTask;

		// Token: 0x04000041 RID: 65
		private bool _loadingCancelled;

		// Token: 0x04000042 RID: 66
		private static CustomLevelLoader _customLevelLoader;

		// Token: 0x04000043 RID: 67
		internal static BeatmapLevelsModel _beatmapLevelsModel;

		// Token: 0x04000044 RID: 68
		public static Sprite defaultCoverImage;

		// Token: 0x04000047 RID: 71
		public static Loader Instance;

		// Token: 0x04000048 RID: 72
		internal static string[] NetviosLevelHashs;

		// Token: 0x02000041 RID: 65
		[Serializable]
		public class ValidSongDownloadData
		{
			// Token: 0x040000CF RID: 207
			public string name;

			// Token: 0x040000D0 RID: 208
			public string hash;
		}
	}
}
