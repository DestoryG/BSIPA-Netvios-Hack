using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using IPA.Logging;

namespace IPA.Loader
{
	// Token: 0x0200003E RID: 62
	internal class DisabledConfig
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00006554 File Offset: 0x00004754
		// (set) Token: 0x06000183 RID: 387 RVA: 0x0000655B File Offset: 0x0000475B
		public static Config Disabled { get; set; }

		// Token: 0x06000184 RID: 388 RVA: 0x00006563 File Offset: 0x00004763
		public static void Load()
		{
			DisabledConfig.Disabled = Config.GetConfigFor("Disabled Mods", new string[] { "json" });
			DisabledConfig.Instance = DisabledConfig.Disabled.Generated(true);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006592 File Offset: 0x00004792
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000659A File Offset: 0x0000479A
		public virtual bool Reset { get; set; } = true;

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000187 RID: 391 RVA: 0x000065A3 File Offset: 0x000047A3
		// (set) Token: 0x06000188 RID: 392 RVA: 0x000065AB File Offset: 0x000047AB
		[NonNullable]
		[UseConverter(typeof(CollectionConverter<string, HashSet<string>>))]
		public virtual HashSet<string> DisabledModIds { get; set; } = new HashSet<string>();

		// Token: 0x06000189 RID: 393 RVA: 0x000065B4 File Offset: 0x000047B4
		protected internal virtual void Changed()
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000065B6 File Offset: 0x000047B6
		protected internal virtual IDisposable ChangeTransaction()
		{
			return null;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000065BC File Offset: 0x000047BC
		protected virtual void OnReload()
		{
			DisabledConfig.<>c__DisplayClass18_0 CS$<>8__locals1 = new DisabledConfig.<>c__DisplayClass18_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.DisabledModIds == null || this.Reset)
			{
				this.DisabledModIds = new HashSet<string>();
				this.Reset = false;
			}
			if (!PluginLoader.IsFirstLoadComplete)
			{
				return;
			}
			DisabledConfig.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
			int num = this.updateState + 1;
			this.updateState = num;
			CS$<>8__locals2.referToState = num;
			CS$<>8__locals1.copy = this.DisabledModIds.ToArray<string>();
			if (this.disableUpdateTask == null || this.disableUpdateTask.IsCompleted)
			{
				this.disableUpdateTask = this.UpdateDisabledMods(CS$<>8__locals1.copy);
				return;
			}
			this.disableUpdateTask = this.disableUpdateTask.ContinueWith<Task>(delegate(Task t)
			{
				if (CS$<>8__locals1.referToState != CS$<>8__locals1.<>4__this.updateState)
				{
					return Task.WhenAll(Array.Empty<Task>());
				}
				return CS$<>8__locals1.<>4__this.UpdateDisabledMods(CS$<>8__locals1.copy);
			});
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006670 File Offset: 0x00004870
		private Task UpdateDisabledMods(string[] updateWithDisabled)
		{
			Func<PluginMetadata, bool> <>9__1;
			Task task;
			for (;;)
			{
				using (StateTransitionTransaction transaction = PluginManager.PluginStateTransaction())
				{
					foreach (PluginMetadata plugin in transaction.DisabledPlugins.ToArray<PluginMetadata>())
					{
						transaction.Enable(plugin, true);
					}
					IEnumerable<PluginMetadata> enumerable = transaction.EnabledPlugins.ToArray<PluginMetadata>();
					Func<PluginMetadata, bool> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = (PluginMetadata m) => updateWithDisabled.Contains(m.Id));
					}
					foreach (PluginMetadata plugin2 in enumerable.Where(func))
					{
						transaction.Disable(plugin2, true);
					}
					try
					{
						if (transaction.WillNeedRestart)
						{
							Logger.loader.Warn("Runtime disabled config reload will need game restart to apply");
						}
						task = transaction.Commit().ContinueWith(delegate(Task t)
						{
							if (t.IsFaulted)
							{
								Logger.loader.Error("Error changing disabled plugins");
								Logger.loader.Error(t.Exception);
							}
						});
					}
					catch (InvalidOperationException)
					{
						continue;
					}
				}
				break;
			}
			return task;
		}

		// Token: 0x0400008F RID: 143
		public static DisabledConfig Instance;

		// Token: 0x04000092 RID: 146
		private Task disableUpdateTask;

		// Token: 0x04000093 RID: 147
		private int updateState;
	}
}
