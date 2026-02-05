using System;
using System.Reflection;
using HarmonyLib;
using IPA;
using IPA.Logging;
using UnityEngine;

namespace NetviosSdkPlugin
{
	// Token: 0x02000003 RID: 3
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205B File Offset: 0x0000025B
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002062 File Offset: 0x00000262
		internal static Plugin instance { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206A File Offset: 0x0000026A
		internal static string Name
		{
			get
			{
				return "NetviosSdkPlugin";
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002071 File Offset: 0x00000271
		[Init]
		public void Init(Logger logger)
		{
			Plugin.instance = this;
			Logger.log = logger;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002080 File Offset: 0x00000280
		[OnStart]
		public void OnApplicationStart()
		{
			Logger.log.Debug("OnApplicationStart");
			this._harmony = new Harmony("com.netvios.beatsber.netviossdkplugin");
			try
			{
				this._harmony.PatchAll(Assembly.GetExecutingAssembly());
			}
			catch (Exception ex)
			{
				Logger.log.Critical(string.Format("Failed to apply harmony patches! {0}", ex));
			}
			new GameObject("SdkPluginController").AddComponent<NetviosSdkPluginController>();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020F8 File Offset: 0x000002F8
		[OnExit]
		public void OnApplicationQuit()
		{
		}

		// Token: 0x04000002 RID: 2
		private Harmony _harmony;
	}
}
