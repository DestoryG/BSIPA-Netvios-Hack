using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IPA.Logging;
using IPA.Utilities;
using IPA.Utilities.Async;

namespace IPA.Config
{
	// Token: 0x0200005C RID: 92
	internal static class ConfigRuntime
	{
		// Token: 0x06000291 RID: 657 RVA: 0x0000D094 File Offset: 0x0000B294
		private static void TryStartRuntime()
		{
			if (ConfigRuntime.loadScheduler == null || !ConfigRuntime.loadScheduler.IsRunning)
			{
				ConfigRuntime.loadFactory = null;
				ConfigRuntime.loadScheduler = new SingleThreadTaskScheduler();
				ConfigRuntime.loadScheduler.Start();
			}
			if (ConfigRuntime.loadFactory == null)
			{
				ConfigRuntime.loadFactory = new TaskFactory(ConfigRuntime.loadScheduler);
			}
			if (ConfigRuntime.saveThread == null || !ConfigRuntime.saveThread.IsAlive)
			{
				ConfigRuntime.saveThread = new Thread(new ThreadStart(ConfigRuntime.SaveThread));
				ConfigRuntime.saveThread.Start();
			}
			AppDomain.CurrentDomain.ProcessExit -= ConfigRuntime.ShutdownRuntime;
			AppDomain.CurrentDomain.ProcessExit += ConfigRuntime.ShutdownRuntime;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000D143 File Offset: 0x0000B343
		private static void ShutdownRuntime(object sender, EventArgs e)
		{
			ConfigRuntime.ShutdownRuntime();
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000D14C File Offset: 0x0000B34C
		internal static void ShutdownRuntime()
		{
			try
			{
				ConfigRuntime.watcherTrackConfigs.Clear();
				KeyValuePair<DirectoryInfo, FileSystemWatcher>[] array = ConfigRuntime.watchers.ToArray();
				ConfigRuntime.watchers.Clear();
				foreach (KeyValuePair<DirectoryInfo, FileSystemWatcher> pair in array)
				{
					pair.Value.EnableRaisingEvents = false;
				}
				ConfigRuntime.loadScheduler.Join();
				ConfigRuntime.saveThread.Abort();
				ConfigRuntime.SaveAll();
			}
			catch
			{
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000D1CC File Offset: 0x0000B3CC
		public static void RegisterConfig(Config cfg)
		{
			ConcurrentBag<Config> concurrentBag = ConfigRuntime.configs;
			lock (concurrentBag)
			{
				if (ConfigRuntime.configs.ToArray().Contains(cfg))
				{
					throw new InvalidOperationException("Config already registered to runtime!");
				}
				ConfigRuntime.configs.Add(cfg);
			}
			ConfigRuntime.configsChangedWatcher.Set();
			ConfigRuntime.TryStartRuntime();
			ConfigRuntime.AddConfigToWatchers(cfg);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000D244 File Offset: 0x0000B444
		public static void ConfigChanged()
		{
			ConfigRuntime.configsChangedWatcher.Set();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000D254 File Offset: 0x0000B454
		private static void AddConfigToWatchers(Config config)
		{
			DirectoryInfo dir2 = config.File.Directory;
			FileSystemWatcher watcher;
			if (!ConfigRuntime.watchers.TryGetValue(dir2, out watcher))
			{
				watcher = ConfigRuntime.watchers.GetOrAdd(dir2, (DirectoryInfo dir) => new FileSystemWatcher(dir.FullName));
				watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime;
				watcher.Changed += ConfigRuntime.FileChangedEvent;
				watcher.Created += ConfigRuntime.FileChangedEvent;
				watcher.Renamed += new RenamedEventHandler(ConfigRuntime.FileChangedEvent);
				watcher.Deleted += ConfigRuntime.FileChangedEvent;
			}
			ConfigRuntime.TryStartRuntime();
			watcher.EnableRaisingEvents = false;
			ConfigRuntime.watcherTrackConfigs.GetOrAdd(watcher, (FileSystemWatcher w) => new ConcurrentBag<Config>()).Add(config);
			watcher.EnableRaisingEvents = true;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000D33C File Offset: 0x0000B53C
		private static void EnsureWritesSane(Config config)
		{
			for (int writes = config.Writes; writes < 0; writes = Interlocked.CompareExchange(ref config.Writes, 0, writes))
			{
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000D364 File Offset: 0x0000B564
		private static void FileChangedEvent(object sender, FileSystemEventArgs e)
		{
			FileSystemWatcher watcher = sender as FileSystemWatcher;
			ConcurrentBag<Config> bag;
			if (!ConfigRuntime.watcherTrackConfigs.TryGetValue(watcher, out bag))
			{
				return;
			}
			Config config = bag.FirstOrDefault((Config c) => c.File.FullName == e.FullPath);
			if (config != null && Interlocked.Decrement(ref config.Writes) + 1 <= 0)
			{
				ConfigRuntime.EnsureWritesSane(config);
				ConfigRuntime.TriggerFileLoad(config);
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000D3C8 File Offset: 0x0000B5C8
		public static Task TriggerFileLoad(Config config)
		{
			return ConfigRuntime.loadFactory.StartNew(delegate
			{
				ConfigRuntime.LoadTask(config);
			});
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000D3F8 File Offset: 0x0000B5F8
		public static Task TriggerLoadAll()
		{
			return Task.WhenAll(ConfigRuntime.configs.Select(new Func<Config, Task>(ConfigRuntime.TriggerFileLoad)));
		}

		/// <summary>
		/// this is synchronous, unlike <see cref="M:IPA.Config.ConfigRuntime.TriggerFileLoad(IPA.Config.Config)" />
		/// </summary>
		/// <param name="config"></param>
		// Token: 0x0600029B RID: 667 RVA: 0x0000D418 File Offset: 0x0000B618
		public static void Save(Config config)
		{
			IConfigStore store = config.Store;
			try
			{
				using (Synchronization.LockRead(store.WriteSyncObject))
				{
					ConfigRuntime.EnsureWritesSane(config);
					Interlocked.Increment(ref config.Writes);
					store.WriteTo(config.configProvider);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception e)
			{
				Logger.config.Error(string.Format("{0} for {1} errored while writing to disk", "IConfigStore", config.File));
				Logger.config.Error(e);
			}
		}

		/// <summary>
		/// this is synchronous, unlike <see cref="M:IPA.Config.ConfigRuntime.TriggerLoadAll" />
		/// </summary>
		// Token: 0x0600029C RID: 668 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
		public static void SaveAll()
		{
			foreach (Config config in ConfigRuntime.configs)
			{
				ConfigRuntime.Save(config);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000D50C File Offset: 0x0000B70C
		private static void LoadTask(Config config)
		{
			try
			{
				IConfigStore store = config.Store;
				using (Synchronization.LockWrite(store.WriteSyncObject))
				{
					store.ReadFrom(config.configProvider);
				}
			}
			catch (Exception e)
			{
				Logger.config.Error(string.Format("{0} for {1} errored while reading from the {2}", "IConfigStore", config.File, "IConfigProvider"));
				Logger.config.Error(e);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D598 File Offset: 0x0000B798
		private static void SaveThread()
		{
			try
			{
				for (;;)
				{
					Config[] configArr = ConfigRuntime.configs.Where((Config c) => c.Store != null).ToArray<Config>();
					int index = -1;
					try
					{
						index = WaitHandle.WaitAny(configArr.Select((Config c) => c.Store.SyncObject).Prepend(ConfigRuntime.configsChangedWatcher).ToArray<WaitHandle>());
					}
					catch (ThreadAbortException)
					{
						break;
					}
					catch (Exception e)
					{
						Logger.config.Error("Error waiting for in-memory updates");
						Logger.config.Error(e);
						Thread.Sleep(TimeSpan.FromSeconds(1.0));
					}
					if (index > 0)
					{
						ConfigRuntime.Save(configArr[index - 1]);
					}
				}
			}
			catch (ThreadAbortException)
			{
			}
		}

		// Token: 0x040000F4 RID: 244
		private static readonly ConcurrentBag<Config> configs = new ConcurrentBag<Config>();

		// Token: 0x040000F5 RID: 245
		private static readonly AutoResetEvent configsChangedWatcher = new AutoResetEvent(false);

		// Token: 0x040000F6 RID: 246
		private static readonly ConcurrentDictionary<DirectoryInfo, FileSystemWatcher> watchers = new ConcurrentDictionary<DirectoryInfo, FileSystemWatcher>(new ConfigRuntime.DirInfoEqComparer());

		// Token: 0x040000F7 RID: 247
		private static readonly ConcurrentDictionary<FileSystemWatcher, ConcurrentBag<Config>> watcherTrackConfigs = new ConcurrentDictionary<FileSystemWatcher, ConcurrentBag<Config>>();

		// Token: 0x040000F8 RID: 248
		private static SingleThreadTaskScheduler loadScheduler = null;

		// Token: 0x040000F9 RID: 249
		private static TaskFactory loadFactory = null;

		// Token: 0x040000FA RID: 250
		private static Thread saveThread = null;

		// Token: 0x02000122 RID: 290
		private class DirInfoEqComparer : IEqualityComparer<DirectoryInfo>
		{
			// Token: 0x060005B8 RID: 1464 RVA: 0x000174FA File Offset: 0x000156FA
			public bool Equals(DirectoryInfo x, DirectoryInfo y)
			{
				return ((x != null) ? x.FullName : null) == ((y != null) ? y.FullName : null);
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x00017519 File Offset: 0x00015719
			public int GetHashCode(DirectoryInfo obj)
			{
				if (obj == null)
				{
					return 0;
				}
				return obj.GetHashCode();
			}
		}
	}
}
