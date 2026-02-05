using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Logging;
using IPA.Old;

namespace IPA.Loader.Composite
{
	// Token: 0x02000056 RID: 86
	internal class CompositeIPAPlugin : IPlugin
	{
		// Token: 0x0600026F RID: 623 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
		public CompositeIPAPlugin(IEnumerable<IPlugin> plugins)
		{
			this.plugins = plugins;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000CBEF File Offset: 0x0000ADEF
		public void OnApplicationStart()
		{
			this.Invoke(delegate(IPlugin plugin)
			{
				plugin.OnApplicationStart();
			}, "OnApplicationStart");
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000CC1B File Offset: 0x0000AE1B
		public void OnApplicationQuit()
		{
			this.Invoke(delegate(IPlugin plugin)
			{
				plugin.OnApplicationQuit();
			}, "OnApplicationQuit");
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000CC48 File Offset: 0x0000AE48
		private void Invoke(CompositeIPAPlugin.CompositeCall callback, [CallerMemberName] string member = "")
		{
			foreach (IPlugin plugin in this.plugins)
			{
				try
				{
					callback(plugin);
				}
				catch (Exception ex)
				{
					Logger.log.Error(string.Format("{0} {1}: {2}", plugin.Name, member, ex));
				}
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
		public void OnUpdate()
		{
			this.Invoke(delegate(IPlugin plugin)
			{
				plugin.OnUpdate();
			}, "OnUpdate");
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000CCF0 File Offset: 0x0000AEF0
		public void OnFixedUpdate()
		{
			this.Invoke(delegate(IPlugin plugin)
			{
				plugin.OnFixedUpdate();
			}, "OnFixedUpdate");
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000CD1C File Offset: 0x0000AF1C
		public string Name
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000CD23 File Offset: 0x0000AF23
		public string Version
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000CD2A File Offset: 0x0000AF2A
		public void OnLateUpdate()
		{
			this.Invoke(delegate(IPlugin plugin)
			{
				IEnhancedPlugin saberPlugin = plugin as IEnhancedPlugin;
				if (saberPlugin != null)
				{
					saberPlugin.OnLateUpdate();
				}
			}, "OnLateUpdate");
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000CD58 File Offset: 0x0000AF58
		public void OnLevelWasLoaded(int level)
		{
			this.Invoke(delegate(IPlugin plugin)
			{
				plugin.OnLevelWasLoaded(level);
			}, "OnLevelWasLoaded");
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000CD8C File Offset: 0x0000AF8C
		public void OnLevelWasInitialized(int level)
		{
			this.Invoke(delegate(IPlugin plugin)
			{
				plugin.OnLevelWasInitialized(level);
			}, "OnLevelWasInitialized");
		}

		// Token: 0x040000EC RID: 236
		private readonly IEnumerable<IPlugin> plugins;

		// Token: 0x0200011B RID: 283
		// (Invoke) Token: 0x060005A1 RID: 1441
		private delegate void CompositeCall(IPlugin plugin);
	}
}
