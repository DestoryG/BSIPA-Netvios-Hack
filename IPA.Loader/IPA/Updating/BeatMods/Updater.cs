using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zip;
using IPA.Config;
using IPA.Loader;
using IPA.Loader.Features;
using IPA.Logging;
using IPA.Old;
using IPA.Utilities;
using IPA.Utilities.Async;
using Newtonsoft.Json;
using SemVer;
using UnityEngine;
using UnityEngine.Networking;

namespace IPA.Updating.BeatMods
{
	// Token: 0x0200000E RID: 14
	internal class Updater : MonoBehaviour
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000020B4 File Offset: 0x000002B4
		public void Awake()
		{
			try
			{
				if (Updater.Instance != null)
				{
					Object.Destroy(this);
				}
				else
				{
					Updater.Instance = this;
					Object.DontDestroyOnLoad(this);
					if (!Updater.ModListPresent && SelfConfig.Updates_.AutoCheckUpdates_)
					{
						this.CheckForUpdates(null);
					}
				}
			}
			catch (Exception e)
			{
				Logger.updater.Error(e);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002118 File Offset: 0x00000318
		public void CheckForUpdates(Updater.CheckUpdatesComplete onComplete = null)
		{
			base.StartCoroutine(this.CheckForUpdatesCoroutine(onComplete));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002128 File Offset: 0x00000328
		public static void ResetRequestCache()
		{
			Updater.requestCache.Clear();
			Updater.modCache.Clear();
			Updater.modVersionsCache.Clear();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002148 File Offset: 0x00000348
		private static IEnumerator GetBeatModsEndpoint(string url, Ref<string> result)
		{
			string value;
			if (Updater.requestCache.TryGetValue(url, out value))
			{
				result.Value = value;
			}
			else
			{
				using (UnityWebRequest request = UnityWebRequest.Get("https://beatmods.com/api/v1/mod" + url))
				{
					yield return request.SendWebRequest();
					if (request.isNetworkError)
					{
						result.Error = new NetworkException("Network error while trying to download: " + request.error);
						yield break;
					}
					if (request.isHttpError)
					{
						if (request.responseCode == 404L)
						{
							result.Error = new NetworkException("Not found");
							yield break;
						}
						result.Error = new NetworkException("Server returned error " + request.error + " while getting data");
						yield break;
					}
					else
					{
						result.Value = request.downloadHandler.text;
						Updater.requestCache[url] = result.Value;
					}
				}
				UnityWebRequest request = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000215E File Offset: 0x0000035E
		internal static IEnumerator GetModInfo(string modName, string ver, Ref<ApiEndpoint.Mod> result)
		{
			string uri = string.Format("?name={0}&version={1}", Uri.EscapeDataString(modName), Uri.EscapeDataString(ver));
			ApiEndpoint.Mod value;
			if (Updater.modCache.TryGetValue(uri, out value))
			{
				result.Value = value;
			}
			else
			{
				Ref<string> reqResult = new Ref<string>("");
				yield return Updater.GetBeatModsEndpoint(uri, reqResult);
				try
				{
					result.Value = JsonConvert.DeserializeObject<List<ApiEndpoint.Mod>>(reqResult.Value).First<ApiEndpoint.Mod>();
					Updater.modCache[uri] = result.Value;
				}
				catch (Exception e)
				{
					result.Error = new Exception("Error decoding response", e);
				}
				reqResult = null;
			}
			yield break;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000217B File Offset: 0x0000037B
		internal static IEnumerator GetModVersionsMatching(string modName, Range range, Ref<List<ApiEndpoint.Mod>> result)
		{
			string uri = string.Format("?name={0}", Uri.EscapeDataString(modName));
			List<ApiEndpoint.Mod> value;
			if (Updater.modVersionsCache.TryGetValue(uri, out value))
			{
				result.Value = value;
			}
			else
			{
				Ref<string> reqResult = new Ref<string>("");
				yield return Updater.GetBeatModsEndpoint(uri, reqResult);
				try
				{
					result.Value = (from m in JsonConvert.DeserializeObject<List<ApiEndpoint.Mod>>(reqResult.Value)
						where range.IsSatisfied(m.Version)
						select m).ToList<ApiEndpoint.Mod>();
					Updater.modVersionsCache[uri] = result.Value;
				}
				catch (Exception e)
				{
					result.Error = new Exception("Error decoding response", e);
				}
				reqResult = null;
			}
			yield break;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002198 File Offset: 0x00000398
		internal IEnumerator CheckForUpdatesCoroutine(Updater.CheckUpdatesComplete onComplete)
		{
			Ref<List<Updater.DependencyObject>> depList = new Ref<List<Updater.DependencyObject>>(new List<Updater.DependencyObject>());
			foreach (PluginExecutor plugin in PluginManager.BSMetas)
			{
				if (plugin.Metadata.Id != null)
				{
					PluginMetadata msinfo = plugin.Metadata;
					Updater.DependencyObject dep = new Updater.DependencyObject
					{
						Name = msinfo.Id,
						Version = msinfo.Version,
						Requirement = new Range(string.Format(">={0}", msinfo.Version), false),
						LocalPluginMeta = msinfo
					};
					if (msinfo.Features.FirstOrDefault((Feature f) => f is NoUpdateFeature) != null)
					{
						dep.Requirement = new Range(msinfo.Version.ToString(), false);
					}
					depList.Value.Add(dep);
				}
			}
			foreach (PluginMetadata meta in PluginLoader.ignoredPlugins.Keys)
			{
				if (meta.Id != null)
				{
					Updater.DependencyObject dep2 = new Updater.DependencyObject
					{
						Name = meta.Id,
						Version = meta.Version,
						Requirement = new Range(string.Format(">={0}", meta.Version), false),
						LocalPluginMeta = meta
					};
					if (meta.Features.FirstOrDefault((Feature f) => f is NoUpdateFeature) != null)
					{
						dep2.Requirement = new Range(meta.Version.ToString(), false);
					}
					depList.Value.Add(dep2);
				}
			}
			foreach (PluginMetadata meta2 in PluginManager.DisabledPlugins)
			{
				if (meta2.Id != null)
				{
					Updater.DependencyObject dep3 = new Updater.DependencyObject
					{
						Name = meta2.Id,
						Version = meta2.Version,
						Requirement = new Range(string.Format(">={0}", meta2.Version), false),
						LocalPluginMeta = meta2
					};
					if (meta2.Features.FirstOrDefault((Feature f) => f is NoUpdateFeature) != null)
					{
						dep3.Requirement = new Range(meta2.Version.ToString(), false);
					}
					depList.Value.Add(dep3);
				}
			}
			foreach (IPlugin plug in PluginManager.Plugins)
			{
				try
				{
					Updater.DependencyObject dep4 = new Updater.DependencyObject
					{
						Name = plug.Name,
						Version = new global::SemVer.Version(plug.Version, false),
						Requirement = new Range(">=" + plug.Version, false),
						IsLegacy = true,
						LocalPluginMeta = null
					};
					depList.Value.Add(dep4);
				}
				catch (Exception e)
				{
					Logger.updater.Warn("Error trying to add legacy plugin " + plug.Name + " to updater");
					Logger.updater.Warn(e);
				}
			}
			foreach (Updater.DependencyObject dep5 in depList.Value)
			{
				Logger.updater.Debug(string.Format("Phantom Dependency: {0}", dep5));
			}
			yield return this.ResolveDependencyRanges(depList);
			foreach (Updater.DependencyObject dep6 in depList.Value)
			{
				Logger.updater.Debug(string.Format("Dependency: {0}", dep6));
			}
			yield return this.ResolveDependencyPresence(depList);
			foreach (Updater.DependencyObject dep7 in depList.Value)
			{
				Logger.updater.Debug(string.Format("Dependency: {0}", dep7));
			}
			this.CheckDependencies(depList);
			if (onComplete != null)
			{
				onComplete(depList);
			}
			if (!Updater.ModListPresent && SelfConfig.Updates_.AutoUpdate_)
			{
				this.StartDownload(depList.Value, null, null, null, null, null, null);
			}
			yield break;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021AE File Offset: 0x000003AE
		internal IEnumerator ResolveDependencyRanges(Ref<List<Updater.DependencyObject>> list)
		{
			int i = 0;
			while (i < list.Value.Count)
			{
				Updater.<>c__DisplayClass15_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass15_0();
				CS$<>8__locals1.dep = list.Value[i];
				Ref<ApiEndpoint.Mod> mod = new Ref<ApiEndpoint.Mod>(null);
				yield return Updater.GetModInfo(CS$<>8__locals1.dep.Name, "", mod);
				try
				{
					mod.Verify();
				}
				catch (Exception e)
				{
					Logger.updater.Error("Error getting info for " + CS$<>8__locals1.dep.Name);
					if (SelfConfig.Debug_.ShowHandledErrorStackTraces_)
					{
						Logger.updater.Error(e);
					}
					CS$<>8__locals1.dep.MetaRequestFailed = true;
					goto IL_012C;
				}
				goto IL_00E8;
				IL_012C:
				int num = i;
				i = num + 1;
				continue;
				IL_00E8:
				list.Value.AddRange(mod.Value.Dependencies.Select((ApiEndpoint.Mod m) => new Updater.DependencyObject
				{
					Name = m.Name,
					Requirement = new Range(string.Format("^{0}", m.Version), false),
					Consumers = new HashSet<string> { CS$<>8__locals1.dep.Name }
				}));
				CS$<>8__locals1 = null;
				mod = null;
				goto IL_012C;
			}
			HashSet<string> depNames = new HashSet<string>();
			List<Updater.DependencyObject> final = new List<Updater.DependencyObject>();
			using (List<Updater.DependencyObject>.Enumerator enumerator = list.Value.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Updater.DependencyObject dep = enumerator.Current;
					if (!depNames.Contains(dep.Name))
					{
						depNames.Add(dep.Name);
						final.Add(dep);
					}
					else
					{
						Updater.DependencyObject toMod = final.First((Updater.DependencyObject d) => d.Name == dep.Name);
						if (dep.Requirement != null)
						{
							toMod.Requirement = toMod.Requirement.Intersect(dep.Requirement);
							foreach (string consume in dep.Consumers)
							{
								toMod.Consumers.Add(consume);
							}
						}
						if (dep.Conflicts != null)
						{
							toMod.Conflicts = ((toMod.Conflicts == null) ? dep.Conflicts : new Range(string.Format("{0} || {1}", toMod.Conflicts, dep.Conflicts), false));
						}
					}
				}
			}
			list.Value = final;
			yield break;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000021BD File Offset: 0x000003BD
		internal IEnumerator ResolveDependencyPresence(Ref<List<Updater.DependencyObject>> list)
		{
			using (List<Updater.DependencyObject>.Enumerator enumerator = list.Value.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Updater.<>c__DisplayClass16_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass16_0();
					CS$<>8__locals1.dep = enumerator.Current;
					CS$<>8__locals1.dep.Has = CS$<>8__locals1.dep.Version != null;
					if (CS$<>8__locals1.dep.MetaRequestFailed)
					{
						Logger.updater.Warn(CS$<>8__locals1.dep.Name + " info request failed, not trying again");
					}
					else
					{
						Ref<List<ApiEndpoint.Mod>> modsMatching = new Ref<List<ApiEndpoint.Mod>>(null);
						yield return Updater.GetModVersionsMatching(CS$<>8__locals1.dep.Name, CS$<>8__locals1.dep.Requirement, modsMatching);
						try
						{
							modsMatching.Verify();
						}
						catch (Exception e)
						{
							Logger.updater.Error("Error getting mod list for " + CS$<>8__locals1.dep.Name);
							if (SelfConfig.Debug_.ShowHandledErrorStackTraces_)
							{
								Logger.updater.Error(e);
							}
							CS$<>8__locals1.dep.MetaRequestFailed = true;
							continue;
						}
						global::SemVer.Version ver = (from versionCheck in modsMatching.Value.NonNull<ApiEndpoint.Mod>()
							where versionCheck.GameVersion == UnityGame.GameVersion
							select versionCheck into approvalCheck
							where approvalCheck.Status == "approved"
							select approvalCheck into conflictsCheck
							where CS$<>8__locals1.dep.Conflicts == null || !CS$<>8__locals1.dep.Conflicts.IsSatisfied(conflictsCheck.Version)
							select conflictsCheck into mod
							select mod.Version).Max<global::SemVer.Version>();
						CS$<>8__locals1.dep.Resolved = ver != null;
						if (CS$<>8__locals1.dep.Resolved)
						{
							CS$<>8__locals1.dep.ResolvedVersion = ver;
						}
						CS$<>8__locals1.dep.Has = CS$<>8__locals1.dep.Resolved && CS$<>8__locals1.dep.Version == CS$<>8__locals1.dep.ResolvedVersion;
						modsMatching = null;
						CS$<>8__locals1 = null;
					}
				}
			}
			List<Updater.DependencyObject>.Enumerator enumerator = default(List<Updater.DependencyObject>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000021CC File Offset: 0x000003CC
		internal void CheckDependencies(Ref<List<Updater.DependencyObject>> list)
		{
			List<Updater.DependencyObject> toDl = new List<Updater.DependencyObject>();
			foreach (Updater.DependencyObject dep in list.Value)
			{
				if (dep.Resolved)
				{
					Logger.updater.Debug(string.Format("Resolved: {0}", dep));
					if (!dep.Has)
					{
						Logger.updater.Debug(string.Format("To Download: {0}", dep));
						toDl.Add(dep);
					}
				}
				else if (!dep.Has)
				{
					if (dep.Version != null && dep.Requirement.IsSatisfied(dep.Version))
					{
						Logger.updater.Notice(string.Format("Mod {0} running a newer version than is on BeatMods ({1})", dep.Name, dep.Version));
					}
					else
					{
						Logger.updater.Warn(string.Format("Could not resolve dependency {0}", dep));
					}
				}
			}
			Logger.updater.Debug("To Download " + string.Join(", ", toDl.Select((Updater.DependencyObject d) => string.Format("{0}@{1}", d.Name, d.ResolvedVersion))));
			list.Value = toDl;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002314 File Offset: 0x00000514
		internal void StartDownload(IEnumerable<Updater.DependencyObject> download, Updater.DownloadStart downloadStart = null, Updater.DownloadProgress downloadProgress = null, Updater.DownloadFailed downloadFail = null, Updater.DownloadFinish downloadFinish = null, Updater.InstallFailed installFail = null, Updater.InstallFinish installFinish = null)
		{
			foreach (Updater.DependencyObject item in download)
			{
				base.StartCoroutine(Updater.UpdateModCoroutine(item, downloadStart, downloadProgress, downloadFail, downloadFinish, installFail, installFinish));
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000236C File Offset: 0x0000056C
		private static IEnumerator UpdateModCoroutine(Updater.DependencyObject item, Updater.DownloadStart downloadStart, Updater.DownloadProgress progress, Updater.DownloadFailed dlFail, Updater.DownloadFinish finish, Updater.InstallFailed installFail, Updater.InstallFinish installFinish)
		{
			Logger.updater.Debug(string.Format("Release: {0}", UnityGame.ReleaseType));
			Ref<ApiEndpoint.Mod> mod = new Ref<ApiEndpoint.Mod>(null);
			yield return Updater.GetModInfo(item.Name, item.ResolvedVersion.ToString(), mod);
			try
			{
				mod.Verify();
			}
			catch (Exception e)
			{
				Logger.updater.Error(string.Format("Error occurred while trying to get information for {0}", item));
				if (SelfConfig.Debug_.ShowHandledErrorStackTraces_)
				{
					Exception e2;
					Logger.updater.Error(e2);
				}
				yield break;
			}
			string releaseName = ((UnityGame.ReleaseType == UnityGame.Release.Steam) ? "steam" : "oculus");
			ApiEndpoint.Mod.DownloadsObject platformFile = mod.Value.Downloads.First((ApiEndpoint.Mod.DownloadsObject f) => f.Type == "universal" || f.Type == releaseName);
			string url = "https://beatmods.com" + platformFile.Path;
			Logger.updater.Debug("URL = " + url);
			int tries = 3;
			Action<int, int, double> <>9__1;
			while (tries > 0)
			{
				int num = tries;
				tries = num - 1;
				if (num != 3)
				{
					Logger.updater.Debug("Re-trying download...");
				}
				using (MemoryStream stream = new MemoryStream())
				{
					using (UnityWebRequest request = UnityWebRequest.Get(url))
					{
						using (CancellationTokenSource taskTokenSource = new CancellationTokenSource())
						{
							MemoryStream stream2 = stream;
							Action<int, int, double> action;
							if ((action = <>9__1) == null)
							{
								action = (<>9__1 = delegate(int i1, int i2, double d)
								{
									Updater.DownloadProgress progress2 = progress;
									if (progress2 == null)
									{
										return;
									}
									progress2(item, (long)i1, (long)i2, d);
								});
							}
							Updater.StreamDownloadHandler dlh = new Updater.StreamDownloadHandler(stream2, action);
							request.downloadHandler = dlh;
							if (downloadStart != null)
							{
								downloadStart(item);
							}
							Logger.updater.Debug("Sending request");
							yield return request.SendWebRequest();
							Logger.updater.Debug("Download finished");
							if (request.isNetworkError)
							{
								Logger.updater.Error("Network error while trying to update mod");
								Logger.updater.Error(request.error);
								if (dlFail != null)
								{
									dlFail(item, request.error);
								}
								taskTokenSource.Cancel();
							}
							else if (request.isHttpError)
							{
								Logger.updater.Error("Server returned an error code while trying to update mod");
								Logger.updater.Error(request.error);
								if (dlFail != null)
								{
									dlFail(item, request.error);
								}
								taskTokenSource.Cancel();
							}
							else
							{
								if (finish != null)
								{
									finish(item);
								}
								stream.Seek(0L, SeekOrigin.Begin);
								Task downloadTask = Task.Run(delegate
								{
									Updater.ExtractPluginAsync(stream, item, platformFile);
								}, taskTokenSource.Token);
								yield return Coroutines.WaitForTask(downloadTask);
								if (!downloadTask.IsFaulted)
								{
									break;
								}
								if (downloadTask.Exception != null)
								{
									if (downloadTask.Exception.InnerExceptions.Any((Exception e) => e is BeatmodsInterceptException))
									{
										Logger.updater.Error("BeatMods did not return expected data for " + item.Name);
										goto IL_04F3;
									}
								}
								Logger.updater.Error("Error downloading mod " + item.Name);
								IL_04F3:
								if (SelfConfig.Debug_.ShowHandledErrorStackTraces_)
								{
									Logger.updater.Error(downloadTask.Exception);
								}
								if (installFail != null)
								{
									installFail(item, downloadTask.Exception);
								}
							}
						}
						continue;
						break;
					}
					continue;
					break;
				}
				continue;
				break;
			}
			if (tries == 0)
			{
				Logger.updater.Warn(string.Format("Plugin download failed {0} times, not re-trying", 3));
				if (installFinish != null)
				{
					installFinish(item, true);
				}
			}
			else
			{
				Logger.updater.Debug("Download complete");
				if (installFinish != null)
				{
					installFinish(item, false);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023A8 File Offset: 0x000005A8
		private static void ExtractPluginAsync(MemoryStream stream, Updater.DependencyObject item, ApiEndpoint.Mod.DownloadsObject fileInfo)
		{
			Logger.updater.Debug("Extracting ZIP file for " + item.Name);
			string targetDir = Path.Combine(UnityGame.InstallPath, "IPA", Path.GetRandomFileName() + "_Pending");
			Directory.CreateDirectory(targetDir);
			string eventualOutput = Path.Combine(UnityGame.InstallPath, "IPA", "Pending");
			if (!Directory.Exists(eventualOutput))
			{
				Directory.CreateDirectory(eventualOutput);
			}
			try
			{
				PluginMetadata localPluginMeta = item.LocalPluginMeta;
				bool shouldDeleteOldFile = !((localPluginMeta != null) ? new bool?(localPluginMeta.IsSelf) : null).Unwrap();
				using (ZipFile zipFile = ZipFile.Read(stream))
				{
					Logger.updater.Debug("Streams opened");
					using (IEnumerator<ZipEntry> enumerator = zipFile.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ZipEntry entry = enumerator.Current;
							if (entry.IsDirectory)
							{
								Logger.updater.Debug("Creating directory " + entry.FileName);
								Directory.CreateDirectory(Path.Combine(targetDir, entry.FileName));
							}
							else
							{
								using (MemoryStream ostream = new MemoryStream((int)entry.UncompressedSize))
								{
									entry.Extract(ostream);
									ostream.Seek(0L, SeekOrigin.Begin);
									byte[] fileHash = new MD5CryptoServiceProvider().ComputeHash(ostream);
									try
									{
										if (!Utils.UnsafeCompare(fileHash, (from h in fileInfo.Hashes
											where h.File == entry.FileName
											select h.Hash).First<byte[]>()))
										{
											throw new Exception("The hash for the file doesn't match what is defined");
										}
									}
									catch (KeyNotFoundException)
									{
										throw new BeatmodsInterceptException("BeatMods did not send the hashes for the zip's content!");
									}
									ostream.Seek(0L, SeekOrigin.Begin);
									FileInfo targetFile = new FileInfo(Path.Combine(targetDir, entry.FileName));
									string directoryName = targetFile.DirectoryName;
									if (directoryName == null)
									{
										throw new InvalidOperationException();
									}
									Directory.CreateDirectory(directoryName);
									if (item.LocalPluginMeta != null)
									{
										string relativePath = Utils.GetRelativePath(targetFile.FullName, targetDir);
										PluginMetadata localPluginMeta2 = item.LocalPluginMeta;
										if (relativePath == Utils.GetRelativePath((localPluginMeta2 != null) ? localPluginMeta2.File.FullName : null, UnityGame.InstallPath))
										{
											shouldDeleteOldFile = false;
										}
									}
									Logger.updater.Debug("Extracting file " + targetFile.FullName);
									targetFile.Delete();
									using (FileStream fstream = targetFile.Create())
									{
										ostream.CopyTo(fstream);
									}
								}
							}
						}
					}
				}
				if (shouldDeleteOldFile && item.LocalPluginMeta != null)
				{
					string text = Path.Combine(targetDir, "$$delete");
					string[] array = new string[1];
					int num = 0;
					PluginMetadata localPluginMeta3 = item.LocalPluginMeta;
					array[num] = Utils.GetRelativePath((localPluginMeta3 != null) ? localPluginMeta3.File.FullName : null, UnityGame.InstallPath);
					File.AppendAllLines(text, array);
				}
			}
			catch (Exception)
			{
				Directory.Delete(targetDir, true);
				throw;
			}
			PluginMetadata localPluginMeta4 = item.LocalPluginMeta;
			if (((localPluginMeta4 != null) ? new bool?(localPluginMeta4.IsSelf) : null).Unwrap())
			{
				Updater.NeedsManualRestart = true;
				Utils.CopyAll(new DirectoryInfo(targetDir), new DirectoryInfo(UnityGame.InstallPath), "", null);
				string deleteFile = Path.Combine(UnityGame.InstallPath, "$$delete");
				if (File.Exists(deleteFile))
				{
					File.Delete(deleteFile);
				}
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				PluginMetadata localPluginMeta5 = item.LocalPluginMeta;
				string text2 = ((localPluginMeta5 != null) ? localPluginMeta5.File.FullName : null);
				if (text2 == null)
				{
					throw new InvalidOperationException();
				}
				processStartInfo.FileName = text2;
				processStartInfo.Arguments = string.Format("-nw={0}", Process.GetCurrentProcess().Id);
				processStartInfo.UseShellExecute = false;
				Process.Start(processStartInfo);
			}
			else
			{
				Utils.CopyAll(new DirectoryInfo(targetDir), new DirectoryInfo(eventualOutput), "$$delete", null);
			}
			Directory.Delete(targetDir, true);
			Logger.updater.Debug("Extractor exited");
		}

		// Token: 0x0400000C RID: 12
		internal const string SpecialDeletionsFile = "$$delete";

		// Token: 0x0400000D RID: 13
		public static Updater Instance;

		// Token: 0x0400000E RID: 14
		internal static bool ModListPresent = false;

		// Token: 0x0400000F RID: 15
		private static readonly Dictionary<string, string> requestCache = new Dictionary<string, string>();

		// Token: 0x04000010 RID: 16
		private static readonly Dictionary<string, ApiEndpoint.Mod> modCache = new Dictionary<string, ApiEndpoint.Mod>();

		// Token: 0x04000011 RID: 17
		private static readonly Dictionary<string, List<ApiEndpoint.Mod>> modVersionsCache = new Dictionary<string, List<ApiEndpoint.Mod>>();

		// Token: 0x04000012 RID: 18
		internal static bool NeedsManualRestart = false;

		// Token: 0x0200009F RID: 159
		// (Invoke) Token: 0x060003FA RID: 1018
		internal delegate void CheckUpdatesComplete(List<Updater.DependencyObject> toUpdate);

		// Token: 0x020000A0 RID: 160
		internal class DependencyObject
		{
			// Token: 0x170000AE RID: 174
			// (get) Token: 0x060003FD RID: 1021 RVA: 0x00013F1B File Offset: 0x0001211B
			// (set) Token: 0x060003FE RID: 1022 RVA: 0x00013F23 File Offset: 0x00012123
			public string Name { get; set; }

			// Token: 0x170000AF RID: 175
			// (get) Token: 0x060003FF RID: 1023 RVA: 0x00013F2C File Offset: 0x0001212C
			// (set) Token: 0x06000400 RID: 1024 RVA: 0x00013F34 File Offset: 0x00012134
			public global::SemVer.Version Version { get; set; }

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x06000401 RID: 1025 RVA: 0x00013F3D File Offset: 0x0001213D
			// (set) Token: 0x06000402 RID: 1026 RVA: 0x00013F45 File Offset: 0x00012145
			public global::SemVer.Version ResolvedVersion { get; set; }

			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x06000403 RID: 1027 RVA: 0x00013F4E File Offset: 0x0001214E
			// (set) Token: 0x06000404 RID: 1028 RVA: 0x00013F56 File Offset: 0x00012156
			public Range Requirement { get; set; }

			// Token: 0x170000B2 RID: 178
			// (get) Token: 0x06000405 RID: 1029 RVA: 0x00013F5F File Offset: 0x0001215F
			// (set) Token: 0x06000406 RID: 1030 RVA: 0x00013F67 File Offset: 0x00012167
			public Range Conflicts { get; set; }

			// Token: 0x170000B3 RID: 179
			// (get) Token: 0x06000407 RID: 1031 RVA: 0x00013F70 File Offset: 0x00012170
			// (set) Token: 0x06000408 RID: 1032 RVA: 0x00013F78 File Offset: 0x00012178
			public bool Resolved { get; set; }

			// Token: 0x170000B4 RID: 180
			// (get) Token: 0x06000409 RID: 1033 RVA: 0x00013F81 File Offset: 0x00012181
			// (set) Token: 0x0600040A RID: 1034 RVA: 0x00013F89 File Offset: 0x00012189
			public bool Has { get; set; }

			// Token: 0x170000B5 RID: 181
			// (get) Token: 0x0600040B RID: 1035 RVA: 0x00013F92 File Offset: 0x00012192
			// (set) Token: 0x0600040C RID: 1036 RVA: 0x00013F9A File Offset: 0x0001219A
			public HashSet<string> Consumers { get; set; } = new HashSet<string>();

			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x0600040D RID: 1037 RVA: 0x00013FA3 File Offset: 0x000121A3
			// (set) Token: 0x0600040E RID: 1038 RVA: 0x00013FAB File Offset: 0x000121AB
			public bool MetaRequestFailed { get; set; }

			// Token: 0x170000B7 RID: 183
			// (get) Token: 0x0600040F RID: 1039 RVA: 0x00013FB4 File Offset: 0x000121B4
			// (set) Token: 0x06000410 RID: 1040 RVA: 0x00013FBC File Offset: 0x000121BC
			public PluginMetadata LocalPluginMeta { get; set; }

			// Token: 0x170000B8 RID: 184
			// (get) Token: 0x06000411 RID: 1041 RVA: 0x00013FC5 File Offset: 0x000121C5
			// (set) Token: 0x06000412 RID: 1042 RVA: 0x00013FCD File Offset: 0x000121CD
			public bool IsLegacy { get; set; }

			// Token: 0x06000413 RID: 1043 RVA: 0x00013FD8 File Offset: 0x000121D8
			public override string ToString()
			{
				return string.Format("{0}@{1}{2} - ({3} ! {4}) {5}", new object[]
				{
					this.Name,
					this.Version,
					this.Resolved ? string.Format(" -> {0}", this.ResolvedVersion) : "",
					this.Requirement,
					this.Conflicts,
					this.Has ? " Already have" : ""
				});
			}
		}

		// Token: 0x020000A1 RID: 161
		// (Invoke) Token: 0x06000416 RID: 1046
		internal delegate void DownloadStart(Updater.DependencyObject obj);

		// Token: 0x020000A2 RID: 162
		// (Invoke) Token: 0x0600041A RID: 1050
		internal delegate void DownloadProgress(Updater.DependencyObject obj, long totalBytes, long currentBytes, double progress);

		// Token: 0x020000A3 RID: 163
		// (Invoke) Token: 0x0600041E RID: 1054
		internal delegate void DownloadFailed(Updater.DependencyObject obj, string error);

		// Token: 0x020000A4 RID: 164
		// (Invoke) Token: 0x06000422 RID: 1058
		internal delegate void DownloadFinish(Updater.DependencyObject obj);

		/// <summary>
		/// This will still be called even if there was an error. Called after all three download/install attempts, or after a successful installation.
		/// ALWAYS called.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="didError"></param>
		// Token: 0x020000A5 RID: 165
		// (Invoke) Token: 0x06000426 RID: 1062
		internal delegate void InstallFinish(Updater.DependencyObject obj, bool didError);

		/// <summary>
		/// This can be called multiple times
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="error"></param>
		// Token: 0x020000A6 RID: 166
		// (Invoke) Token: 0x0600042A RID: 1066
		internal delegate void InstallFailed(Updater.DependencyObject obj, Exception error);

		// Token: 0x020000A7 RID: 167
		internal class StreamDownloadHandler : DownloadHandlerScript
		{
			// Token: 0x170000B9 RID: 185
			// (get) Token: 0x0600042D RID: 1069 RVA: 0x00014065 File Offset: 0x00012265
			// (set) Token: 0x0600042E RID: 1070 RVA: 0x0001406D File Offset: 0x0001226D
			public MemoryStream Stream { get; set; }

			// Token: 0x0600042F RID: 1071 RVA: 0x00014076 File Offset: 0x00012276
			public StreamDownloadHandler(MemoryStream stream, Action<int, int, double> progress = null)
			{
				this.Stream = stream;
				this.progress = progress;
			}

			// Token: 0x06000430 RID: 1072 RVA: 0x0001408C File Offset: 0x0001228C
			[Obsolete("Use ReceiveContentLengthHeader")]
			protected override void ReceiveContentLength(int contentLength)
			{
				MemoryStream stream = this.Stream;
				this.length = contentLength;
				stream.Capacity = contentLength;
				this.cLen = 0;
				Logger.updater.Debug(string.Format("Got content length: {0}", contentLength));
			}

			// Token: 0x06000431 RID: 1073 RVA: 0x000140CF File Offset: 0x000122CF
			protected override void CompleteContent()
			{
				Logger.updater.Debug("Download complete");
			}

			// Token: 0x06000432 RID: 1074 RVA: 0x000140E0 File Offset: 0x000122E0
			protected override bool ReceiveData(byte[] rData, int dataLength)
			{
				if (rData == null || rData.Length < 1)
				{
					Logger.updater.Debug("CustomWebRequest :: ReceiveData - received a null/empty buffer");
					return false;
				}
				this.cLen += dataLength;
				this.Stream.Write(rData, 0, dataLength);
				Action<int, int, double> action = this.progress;
				if (action != null)
				{
					action(this.length, this.cLen, (double)this.cLen / (double)this.length);
				}
				return true;
			}

			// Token: 0x06000433 RID: 1075 RVA: 0x00014150 File Offset: 0x00012350
			protected override byte[] GetData()
			{
				return null;
			}

			// Token: 0x06000434 RID: 1076 RVA: 0x00014153 File Offset: 0x00012353
			protected override float GetProgress()
			{
				return 0f;
			}

			// Token: 0x06000435 RID: 1077 RVA: 0x0001415A File Offset: 0x0001235A
			public override string ToString()
			{
				return string.Format("{0} ({1})", base.ToString(), this.Stream);
			}

			// Token: 0x0400014D RID: 333
			internal int length;

			// Token: 0x0400014E RID: 334
			internal int cLen;

			// Token: 0x0400014F RID: 335
			internal Action<int, int, double> progress;
		}
	}
}
