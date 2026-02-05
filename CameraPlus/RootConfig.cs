using System;
using System.IO;

namespace CameraPlus
{
	// Token: 0x0200000D RID: 13
	internal class RootConfig
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00007357 File Offset: 0x00005557
		public string FilePath { get; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600006D RID: 109 RVA: 0x00007360 File Offset: 0x00005560
		// (remove) Token: 0x0600006E RID: 110 RVA: 0x00007398 File Offset: 0x00005598
		public event Action<RootConfig> ConfigChangedEvent;

		// Token: 0x0600006F RID: 111 RVA: 0x000073D0 File Offset: 0x000055D0
		public RootConfig(string filePath)
		{
			this.FilePath = filePath;
			if (!Directory.Exists(Path.GetDirectoryName(this.FilePath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this.FilePath));
			}
			if (File.Exists(this.FilePath))
			{
				this.Load();
				File.ReadAllText(this.FilePath);
			}
			this.Save();
			this._configWatcher = new FileSystemWatcher(Path.GetDirectoryName(this.FilePath))
			{
				NotifyFilter = NotifyFilters.LastWrite,
				Filter = Path.GetFileName(this.FilePath),
				EnableRaisingEvents = true
			};
			this._configWatcher.Changed += this.ConfigWatcherOnChanged;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00007498 File Offset: 0x00005698
		~RootConfig()
		{
			this._configWatcher.Changed -= this.ConfigWatcherOnChanged;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000074D8 File Offset: 0x000056D8
		public void Save()
		{
			this._saving = true;
			ConfigSerializer.SaveConfig(this, this.FilePath);
			this._saving = false;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000074F4 File Offset: 0x000056F4
		public void Load()
		{
			ConfigSerializer.LoadConfig(this, this.FilePath);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00007503 File Offset: 0x00005703
		private void ConfigWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
		{
			if (this._saving)
			{
				this._saving = false;
				return;
			}
			this.Load();
			if (this.ConfigChangedEvent != null)
			{
				this.ConfigChangedEvent(this);
			}
		}

		// Token: 0x04000083 RID: 131
		public bool ProfileSceneChange;

		// Token: 0x04000084 RID: 132
		public string MenuProfile = "";

		// Token: 0x04000085 RID: 133
		public string GameProfile = "";

		// Token: 0x04000087 RID: 135
		private readonly FileSystemWatcher _configWatcher;

		// Token: 0x04000088 RID: 136
		private bool _saving;
	}
}
