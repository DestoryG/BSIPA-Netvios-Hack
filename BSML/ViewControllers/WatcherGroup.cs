using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BeatSaberMarkupLanguage.ViewControllers
{
	// Token: 0x02000013 RID: 19
	internal class WatcherGroup
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004AD4 File Offset: 0x00002CD4
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00004ADC File Offset: 0x00002CDC
		internal FileSystemWatcher Watcher { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004AE5 File Offset: 0x00002CE5
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004AED File Offset: 0x00002CED
		internal string ContentDirectory { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004AF6 File Offset: 0x00002CF6
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004AFE File Offset: 0x00002CFE
		internal bool IsReloading { get; private set; }

		// Token: 0x0600009C RID: 156 RVA: 0x00004B07 File Offset: 0x00002D07
		internal WatcherGroup(string directory)
		{
			this.ContentDirectory = directory;
			this.CreateWatcher();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004B38 File Offset: 0x00002D38
		private void CreateWatcher()
		{
			if (this.Watcher != null)
			{
				return;
			}
			if (!Directory.Exists(this.ContentDirectory))
			{
				return;
			}
			this.Watcher = new FileSystemWatcher(this.ContentDirectory, "*.bsml")
			{
				NotifyFilter = NotifyFilters.LastWrite
			};
			this.Watcher.Changed += this.OnFileWasChanged;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004B91 File Offset: 0x00002D91
		private void DestroyWatcher()
		{
			this.Watcher.Dispose();
			this.Watcher = null;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004BA8 File Offset: 0x00002DA8
		private void OnFileWasChanged(object sender, FileSystemEventArgs e)
		{
			foreach (KeyValuePair<int, WeakReference<WatcherGroup.IHotReloadableController>> keyValuePair in this.BoundControllers.ToArray<KeyValuePair<int, WeakReference<WatcherGroup.IHotReloadableController>>>())
			{
				WatcherGroup.IHotReloadableController hotReloadableController;
				if (!keyValuePair.Value.TryGetTarget(out hotReloadableController))
				{
					this.UnbindController(keyValuePair.Key);
				}
				else if (e.FullPath == Path.GetFullPath(hotReloadableController.ContentFilePath))
				{
					hotReloadableController.MarkDirty();
					PersistentSingleton<HMMainThreadDispatcher>.instance.Enqueue(this.HotReloadCoroutine());
				}
			}
			if (this.BoundControllers.Count == 0)
			{
				this.DestroyWatcher();
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004C38 File Offset: 0x00002E38
		private IEnumerator<WaitForSeconds> HotReloadCoroutine()
		{
			if (this.IsReloading)
			{
				yield break;
			}
			this.IsReloading = true;
			yield return this.HotReloadDelay;
			foreach (KeyValuePair<int, WeakReference<WatcherGroup.IHotReloadableController>> keyValuePair in this.BoundControllers.ToArray<KeyValuePair<int, WeakReference<WatcherGroup.IHotReloadableController>>>())
			{
				WatcherGroup.IHotReloadableController hotReloadableController;
				if (!keyValuePair.Value.TryGetTarget(out hotReloadableController))
				{
					this.UnbindController(keyValuePair.Key);
				}
				else if (hotReloadableController.ContentChanged && hotReloadableController != null)
				{
					hotReloadableController.Refresh(false);
				}
			}
			this.IsReloading = false;
			yield break;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004C48 File Offset: 0x00002E48
		internal bool BindController(WatcherGroup.IHotReloadableController controller)
		{
			if (this.BoundControllers.ContainsKey(controller.GetInstanceID()))
			{
				return false;
			}
			this.BoundControllers.Add(controller.GetInstanceID(), new WeakReference<WatcherGroup.IHotReloadableController>(controller));
			this.CreateWatcher();
			this.Watcher.EnableRaisingEvents = true;
			return true;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004C94 File Offset: 0x00002E94
		internal bool UnbindController(int instanceId)
		{
			bool flag = this.BoundControllers.Remove(instanceId);
			if (this.BoundControllers.Count == 0)
			{
				this.DestroyWatcher();
			}
			return flag;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004CB5 File Offset: 0x00002EB5
		internal bool UnbindController(WatcherGroup.IHotReloadableController controller)
		{
			return controller != null && this.UnbindController(controller.GetInstanceID());
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public static bool RegisterViewController(WatcherGroup.IHotReloadableController controller)
		{
			string contentFilePath = controller.ContentFilePath;
			if (string.IsNullOrEmpty(contentFilePath))
			{
				return false;
			}
			string directoryName = Path.GetDirectoryName(contentFilePath);
			if (!Directory.Exists(directoryName))
			{
				return false;
			}
			WatcherGroup watcherGroup;
			if (!WatcherGroup.WatcherDictionary.TryGetValue(directoryName, out watcherGroup))
			{
				watcherGroup = new WatcherGroup(directoryName);
				WatcherGroup.WatcherDictionary.Add(directoryName, watcherGroup);
			}
			watcherGroup.BindController(controller);
			return true;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004D24 File Offset: 0x00002F24
		public static bool UnregisterViewController(WatcherGroup.IHotReloadableController controller)
		{
			string contentFilePath = controller.ContentFilePath;
			if (string.IsNullOrEmpty(contentFilePath))
			{
				return false;
			}
			bool flag = false;
			string directoryName = Path.GetDirectoryName(contentFilePath);
			WatcherGroup watcherGroup;
			if (WatcherGroup.WatcherDictionary.TryGetValue(directoryName, out watcherGroup))
			{
				flag = watcherGroup.UnbindController(controller);
			}
			return flag;
		}

		// Token: 0x0400002D RID: 45
		private readonly WaitForSeconds HotReloadDelay = new WaitForSeconds(0.5f);

		// Token: 0x0400002E RID: 46
		private readonly Dictionary<int, WeakReference<WatcherGroup.IHotReloadableController>> BoundControllers = new Dictionary<int, WeakReference<WatcherGroup.IHotReloadableController>>();

		// Token: 0x0400002F RID: 47
		private static readonly Dictionary<string, WatcherGroup> WatcherDictionary = new Dictionary<string, WatcherGroup>();

		// Token: 0x020000F1 RID: 241
		internal interface IHotReloadableController
		{
			// Token: 0x17000137 RID: 311
			// (get) Token: 0x0600053C RID: 1340
			bool ContentChanged { get; }

			// Token: 0x17000138 RID: 312
			// (get) Token: 0x0600053D RID: 1341
			string ContentFilePath { get; }

			// Token: 0x17000139 RID: 313
			// (get) Token: 0x0600053E RID: 1342
			string Name { get; }

			// Token: 0x0600053F RID: 1343
			void MarkDirty();

			// Token: 0x06000540 RID: 1344
			void Refresh(bool forceReload = false);

			// Token: 0x06000541 RID: 1345
			int GetInstanceID();
		}
	}
}
