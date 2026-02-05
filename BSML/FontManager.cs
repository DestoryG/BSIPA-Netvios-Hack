using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.OpenType;
using IPA.Utilities.Async;
using Microsoft.Win32;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage
{
	// Token: 0x02000005 RID: 5
	public static class FontManager
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000263C File Offset: 0x0000083C
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002643 File Offset: 0x00000843
		public static Task SystemFontLoadTask { get; private set; }

		// Token: 0x0600001F RID: 31 RVA: 0x0000264C File Offset: 0x0000084C
		public static Task AsyncLoadSystemFonts()
		{
			if (FontManager.IsInitialized)
			{
				return Task.CompletedTask;
			}
			if (FontManager.SystemFontLoadTask != null)
			{
				return FontManager.SystemFontLoadTask;
			}
			Task<ValueTuple<Dictionary<string, List<FontManager.FontInfo>>, Dictionary<string, FontManager.FontInfo>>> task = Task.Factory.StartNew<Task<ValueTuple<Dictionary<string, List<FontManager.FontInfo>>, Dictionary<string, FontManager.FontInfo>>>>(new Func<Task<ValueTuple<Dictionary<string, List<FontManager.FontInfo>>, Dictionary<string, FontManager.FontInfo>>>>(FontManager.LoadSystemFonts)).Unwrap<ValueTuple<Dictionary<string, List<FontManager.FontInfo>>, Dictionary<string, FontManager.FontInfo>>>();
			FontManager.SystemFontLoadTask = task.ContinueWith<Task>(delegate([TupleElementNames(new string[] { "families", "fulls" })] Task<ValueTuple<Dictionary<string, List<FontManager.FontInfo>>, Dictionary<string, FontManager.FontInfo>>> t)
			{
				Logger.log.Debug("Font loading complete");
				Interlocked.CompareExchange<Dictionary<string, FontManager.FontInfo>>(ref FontManager.fontInfoLookupFullName, t.Result.Item2, null);
				Interlocked.CompareExchange<Dictionary<string, List<FontManager.FontInfo>>>(ref FontManager.fontInfoLookup, t.Result.Item1, null);
				return Task.CompletedTask;
			}, TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously).Unwrap();
			task.ContinueWith(delegate([TupleElementNames(new string[] { "families", "fulls" })] Task<ValueTuple<Dictionary<string, List<FontManager.FontInfo>>, Dictionary<string, FontManager.FontInfo>>> t)
			{
				Logger.log.Error(string.Format("Font loading errored: {0}", t.Exception));
			}, TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
			return FontManager.SystemFontLoadTask;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026F4 File Offset: 0x000008F4
		internal static Task Destroy()
		{
			FontManager.fontInfoLookup = null;
			FontManager.fontInfoLookupFullName = null;
			return UnityMainThreadTaskScheduler.Factory.StartNew<Task>(() => FontManager.DestroyObjects(FontManager.loadedFontsCache.Select((KeyValuePair<string, Font> p) => p.Value))).Unwrap().ContinueWith(delegate(Task _)
			{
				FontManager.loadedFontsCache.Clear();
			})
				.ContinueWith<Task>((Task _) => FontManager.DestroyObjects(FontManager.tmpFontCache.Select(([TupleElementNames(new string[] { "font", "hasFallbacks" })] KeyValuePair<ValueTuple<Font, bool>, TMP_FontAsset> p) => p.Value)), UnityMainThreadTaskScheduler.Default)
				.Unwrap()
				.ContinueWith(delegate(Task _)
				{
					FontManager.tmpFontCache.Clear();
				});
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027B4 File Offset: 0x000009B4
		private static async Task DestroyObjects(IEnumerable<Object> objects)
		{
			foreach (Object @object in objects)
			{
				Object.Destroy(@object);
				await Task.Yield();
			}
			IEnumerator<Object> enumerator = null;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027F8 File Offset: 0x000009F8
		[return: TupleElementNames(new string[] { "families", "fulls" })]
		private static async Task<ValueTuple<Dictionary<string, List<FontManager.FontInfo>>, Dictionary<string, FontManager.FontInfo>>> LoadSystemFonts()
		{
			string[] pathsToOSFonts = Font.GetPathsToOSFonts();
			Dictionary<string, List<FontManager.FontInfo>> families = new Dictionary<string, List<FontManager.FontInfo>>(pathsToOSFonts.Length, StringComparer.InvariantCultureIgnoreCase);
			Dictionary<string, FontManager.FontInfo> fullNames = new Dictionary<string, FontManager.FontInfo>(pathsToOSFonts.Length, StringComparer.InvariantCultureIgnoreCase);
			foreach (string text in pathsToOSFonts)
			{
				try
				{
					FontManager.AddFontFileToCache(families, fullNames, text);
				}
				catch (Exception ex)
				{
					Logger.log.Error(ex);
				}
				await Task.Yield();
			}
			string[] array = null;
			return new ValueTuple<Dictionary<string, List<FontManager.FontInfo>>, Dictionary<string, FontManager.FontInfo>>(families, fullNames);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002834 File Offset: 0x00000A34
		private static IEnumerable<FontManager.FontInfo> AddFontFileToCache(Dictionary<string, List<FontManager.FontInfo>> cache, Dictionary<string, FontManager.FontInfo> fullCache, string path)
		{
			FontManager.<>c__DisplayClass13_0 CS$<>8__locals1 = new FontManager.<>c__DisplayClass13_0();
			CS$<>8__locals1.path = path;
			CS$<>8__locals1.cache = cache;
			CS$<>8__locals1.fullCache = fullCache;
			IEnumerable<FontManager.FontInfo> enumerable;
			using (FileStream fileStream = new FileStream(CS$<>8__locals1.path, FileMode.Open, FileAccess.Read))
			{
				using (OpenTypeReader openTypeReader = OpenTypeReader.For(fileStream, null, false))
				{
					OpenTypeCollectionReader openTypeCollectionReader = openTypeReader as OpenTypeCollectionReader;
					if (openTypeCollectionReader != null)
					{
						enumerable = new OpenTypeCollection(openTypeCollectionReader, false).Select(new Func<OpenTypeFont, FontManager.FontInfo>(CS$<>8__locals1.<AddFontFileToCache>g__AddFont|0)).ToList<FontManager.FontInfo>();
					}
					else
					{
						OpenTypeFontReader openTypeFontReader = openTypeReader as OpenTypeFontReader;
						if (openTypeFontReader != null)
						{
							OpenTypeFont openTypeFont = new OpenTypeFont(openTypeFontReader, false);
							enumerable = CS$<>8__locals1.<AddFontFileToCache>g__AddFont|0(openTypeFont).SingleEnumerable<FontManager.FontInfo>();
						}
						else
						{
							Logger.log.Warn("Font file '" + CS$<>8__locals1.path + "' is not an OpenType file");
							enumerable = Enumerable.Empty<FontManager.FontInfo>();
						}
					}
				}
			}
			return enumerable;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002920 File Offset: 0x00000B20
		private static List<FontManager.FontInfo> GetListForFamily(Dictionary<string, List<FontManager.FontInfo>> cache, string family)
		{
			List<FontManager.FontInfo> list;
			if (!cache.TryGetValue(family, out list))
			{
				cache.Add(family, list = new List<FontManager.FontInfo>());
			}
			return list;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002948 File Offset: 0x00000B48
		public static Font AddFontFile(string path)
		{
			FontManager.ThrowIfNotInitialized();
			Dictionary<string, List<FontManager.FontInfo>> dictionary = FontManager.fontInfoLookup;
			Font fontFromCacheOrLoad;
			lock (dictionary)
			{
				IEnumerable<FontManager.FontInfo> enumerable = FontManager.AddFontFileToCache(FontManager.fontInfoLookup, FontManager.fontInfoLookupFullName, path);
				if (!enumerable.Any<FontManager.FontInfo>())
				{
					throw new ArgumentException("File is not an OpenType font or collection", "path");
				}
				fontFromCacheOrLoad = FontManager.GetFontFromCacheOrLoad(enumerable.First<FontManager.FontInfo>());
			}
			return fontFromCacheOrLoad;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000029BC File Offset: 0x00000BBC
		public static bool IsInitialized
		{
			get
			{
				return FontManager.fontInfoLookup != null;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029C6 File Offset: 0x00000BC6
		private static void ThrowIfNotInitialized()
		{
			if (!FontManager.IsInitialized)
			{
				throw new InvalidOperationException("FontManager not initialized");
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000029DC File Offset: 0x00000BDC
		public static bool TryGetFontByFamily(string family, out Font font, string subfamily = null, bool fallbackIfNoSubfamily = false)
		{
			FontManager.FontInfo fontInfo;
			if (FontManager.TryGetFontInfoByFamily(family, out fontInfo, subfamily, fallbackIfNoSubfamily))
			{
				font = FontManager.GetFontFromCacheOrLoad(fontInfo);
				return true;
			}
			font = null;
			return false;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A04 File Offset: 0x00000C04
		private static bool TryGetFontInfoByFamily(string family, out FontManager.FontInfo info, string subfamily = null, bool fallbackIfNoSubfamily = false)
		{
			FontManager.ThrowIfNotInitialized();
			if (subfamily == null)
			{
				fallbackIfNoSubfamily = true;
			}
			if (subfamily == null)
			{
				subfamily = "Regular";
			}
			Dictionary<string, List<FontManager.FontInfo>> dictionary = FontManager.fontInfoLookup;
			bool flag2;
			lock (dictionary)
			{
				List<FontManager.FontInfo> list;
				if (FontManager.fontInfoLookup.TryGetValue(family, out list))
				{
					info = list.FirstOrDefault((FontManager.FontInfo p) => ((p != null) ? p.Info.Subfamily : null) == subfamily);
					if (info == null)
					{
						if (!fallbackIfNoSubfamily)
						{
							return false;
						}
						info = list.First<FontManager.FontInfo>();
					}
					flag2 = true;
				}
				else
				{
					info = null;
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public static bool TryGetFontByFullName(string fullName, out Font font)
		{
			FontManager.FontInfo fontInfo;
			if (FontManager.TryGetFontInfoByFullName(fullName, out fontInfo))
			{
				font = FontManager.GetFontFromCacheOrLoad(fontInfo);
				return true;
			}
			font = null;
			return false;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002ADC File Offset: 0x00000CDC
		private static bool TryGetFontInfoByFullName(string fullname, out FontManager.FontInfo info)
		{
			FontManager.ThrowIfNotInitialized();
			Dictionary<string, List<FontManager.FontInfo>> dictionary = FontManager.fontInfoLookup;
			bool flag2;
			lock (dictionary)
			{
				flag2 = FontManager.fontInfoLookupFullName.TryGetValue(fullname, out info);
			}
			return flag2;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B28 File Offset: 0x00000D28
		private static Font GetFontFromCacheOrLoad(FontManager.FontInfo info)
		{
			Dictionary<string, Font> dictionary = FontManager.loadedFontsCache;
			Font font2;
			lock (dictionary)
			{
				Font font;
				if (!FontManager.loadedFontsCache.TryGetValue(info.Path, out font))
				{
					font = new Font(info.Path);
					font.name = info.Info.FullName;
					FontManager.loadedFontsCache.Add(info.Path, font);
				}
				font2 = font;
			}
			return font2;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002BA8 File Offset: 0x00000DA8
		public static IEnumerable<string> GetOSFontFallbackList(string fullname)
		{
			IEnumerable<string> enumerable;
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\FontLink\\SystemLink"))
			{
				if (registryKey == null)
				{
					enumerable = Enumerable.Empty<string>();
				}
				else
				{
					string[] array = registryKey.GetValue(fullname) as string[];
					if (array != null)
					{
						enumerable = from s in array.Select((string s) => s.Split(new char[] { ',' })).Select(delegate(string[] a)
							{
								if (a.Length <= 1)
								{
									return null;
								}
								return a[1];
							})
							where s != null
							select s;
					}
					else
					{
						enumerable = Enumerable.Empty<string>();
					}
				}
			}
			return enumerable;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C78 File Offset: 0x00000E78
		public static bool TryGetTMPFontByFamily(string family, out TMP_FontAsset font, string subfamily = null, bool fallbackIfNoSubfamily = false, bool setupOsFallbacks = true)
		{
			FontManager.FontInfo fontInfo;
			if (!FontManager.TryGetFontInfoByFamily(family, out fontInfo, subfamily, fallbackIfNoSubfamily))
			{
				font = null;
				return false;
			}
			font = FontManager.GetOrSetupTMPFontFor(fontInfo, setupOsFallbacks);
			return true;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002CA4 File Offset: 0x00000EA4
		public static bool TryGetTMPFontByFullName(string fullname, out TMP_FontAsset font, bool setupOsFallbacks = true)
		{
			FontManager.FontInfo fontInfo;
			if (!FontManager.TryGetFontInfoByFullName(fullname, out fontInfo))
			{
				font = null;
				return false;
			}
			font = FontManager.GetOrSetupTMPFontFor(fontInfo, setupOsFallbacks);
			return true;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002CCC File Offset: 0x00000ECC
		private static TMP_FontAsset GetOrSetupTMPFontFor(FontManager.FontInfo info, bool setupOsFallbacks)
		{
			Font fontFromCacheOrLoad = FontManager.GetFontFromCacheOrLoad(info);
			TMP_FontAsset tmp_FontAsset;
			if (!FontManager.tmpFontCache.TryGetValue(new ValueTuple<Font, bool>(fontFromCacheOrLoad, setupOsFallbacks), out tmp_FontAsset))
			{
				tmp_FontAsset = BeatSaberUI.CreateTMPFont(fontFromCacheOrLoad, info.Info.FullName);
				if (setupOsFallbacks)
				{
					foreach (string text in FontManager.GetOSFontFallbackList(info.Info.FullName))
					{
						TMP_FontAsset tmp_FontAsset2;
						if (FontManager.TryGetTMPFontByFullName(text, out tmp_FontAsset2, false))
						{
							if (tmp_FontAsset.fallbackFontAssetTable == null)
							{
								tmp_FontAsset.fallbackFontAssetTable = new List<TMP_FontAsset>();
							}
							tmp_FontAsset.fallbackFontAssetTable.Add(tmp_FontAsset2);
						}
						else
						{
							Logger.log.Debug(text + "-> Not found");
						}
					}
				}
				FontManager.tmpFontCache.Add(new ValueTuple<Font, bool>(fontFromCacheOrLoad, setupOsFallbacks), tmp_FontAsset);
			}
			return tmp_FontAsset;
		}

		// Token: 0x04000005 RID: 5
		private static Dictionary<string, List<FontManager.FontInfo>> fontInfoLookup;

		// Token: 0x04000006 RID: 6
		private static Dictionary<string, FontManager.FontInfo> fontInfoLookupFullName;

		// Token: 0x04000007 RID: 7
		private static readonly Dictionary<string, Font> loadedFontsCache = new Dictionary<string, Font>();

		// Token: 0x04000008 RID: 8
		[TupleElementNames(new string[] { "font", "hasFallbacks" })]
		private static readonly Dictionary<ValueTuple<Font, bool>, TMP_FontAsset> tmpFontCache = new Dictionary<ValueTuple<Font, bool>, TMP_FontAsset>();

		// Token: 0x020000E0 RID: 224
		private class FontInfo
		{
			// Token: 0x06000501 RID: 1281 RVA: 0x00015098 File Offset: 0x00013298
			public FontInfo(string path, OpenTypeFont info)
			{
				this.Path = path;
				this.Info = info;
			}

			// Token: 0x0400019A RID: 410
			public string Path;

			// Token: 0x0400019B RID: 411
			public OpenTypeFont Info;
		}
	}
}
