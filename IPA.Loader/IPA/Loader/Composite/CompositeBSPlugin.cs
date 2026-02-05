using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Logging;
using UnityEngine.SceneManagement;

namespace IPA.Loader.Composite
{
	// Token: 0x02000055 RID: 85
	internal class CompositeBSPlugin
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000CAE8 File Offset: 0x0000ACE8
		public CompositeBSPlugin(IEnumerable<PluginExecutor> plugins)
		{
			this.plugins = plugins;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		private void Invoke(CompositeBSPlugin.CompositeCall callback, [CallerMemberName] string method = "")
		{
			foreach (PluginExecutor plugin in this.plugins)
			{
				try
				{
					if (plugin != null)
					{
						callback(plugin);
					}
				}
				catch (Exception ex)
				{
					Logger.log.Error(string.Format("{0} {1}: {2}", plugin.Metadata.Name, method, ex));
				}
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000CB7C File Offset: 0x0000AD7C
		public void OnEnable()
		{
			this.Invoke(delegate(PluginExecutor plugin)
			{
				plugin.Enable();
			}, "OnEnable");
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000CBA8 File Offset: 0x0000ADA8
		public void OnApplicationQuit()
		{
			this.Invoke(delegate(PluginExecutor plugin)
			{
				plugin.Disable();
			}, "OnApplicationQuit");
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000CBD4 File Offset: 0x0000ADD4
		public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000CBD6 File Offset: 0x0000ADD6
		public void OnSceneUnloaded(Scene scene)
		{
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000CBD8 File Offset: 0x0000ADD8
		public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000CBDA File Offset: 0x0000ADDA
		public void OnUpdate()
		{
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000CBDC File Offset: 0x0000ADDC
		public void OnFixedUpdate()
		{
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000CBDE File Offset: 0x0000ADDE
		public void OnLateUpdate()
		{
		}

		// Token: 0x040000EB RID: 235
		private readonly IEnumerable<PluginExecutor> plugins;

		// Token: 0x02000119 RID: 281
		// (Invoke) Token: 0x06000599 RID: 1433
		private delegate void CompositeCall(PluginExecutor plugin);
	}
}
