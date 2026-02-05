using System;
using HarmonyLib;
using IPA;
using IPA.Logging;
using IPA.Netvios;
using UnityEngine;

namespace PlayerDataPlugin
{
	// Token: 0x02000007 RID: 7
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000256C File Offset: 0x0000076C
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002573 File Offset: 0x00000773
		internal static Plugin instance { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000257B File Offset: 0x0000077B
		internal static string Name
		{
			get
			{
				return "PlayerDataPlugin";
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002584 File Offset: 0x00000784
		[Init]
		public void Init(Logger logger)
		{
			try
			{
				if (!Utils.CheckIPA())
				{
					Application.Quit();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Application.Quit();
			}
			Plugin.instance = this;
			Logger.log = logger;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025C8 File Offset: 0x000007C8
		[OnStart]
		public void OnApplicationStart()
		{
			Logger.log.Debug("OnApplicationStart");
			this._harmony = new Harmony("com.netvios.beatsber.playerdataplugin");
			try
			{
				this._harmony.PatchAll();
			}
			catch (Exception ex)
			{
				Logger.log.Critical(string.Format("Failed to apply harmony patches! {0}", ex));
			}
			new GameObject("PlayerDataPluginController").AddComponent<PlayerDataPluginController>();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000263C File Offset: 0x0000083C
		[OnExit]
		public void OnApplicationQuit()
		{
			this._harmony.UnpatchAll(null);
		}

		// Token: 0x04000011 RID: 17
		internal Harmony _harmony;
	}
}
