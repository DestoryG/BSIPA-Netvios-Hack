using System;
using System.Collections.Generic;
using System.IO;
using IPA;
using IPA.Logging;
using IPA.Netvios;
using NetViosCommon.Utility;
using Polyglot;
using UnityEngine;

namespace LocalizationPlugin
{
	// Token: 0x02000002 RID: 2
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		internal static Plugin instance { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		internal static string Name
		{
			get
			{
				return "LocalizationPlugin";
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002068 File Offset: 0x00000268
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
			Logger.log.Debug("Logger initialized.");
			Language privateField = Localization.Instance.GetPrivateField("fallbackLanguage");
			Logger.log.Debug("fallbackLanguage: " + privateField.ToString());
			List<Language> supportedLanguages = Localization.Instance.SupportedLanguages;
			foreach (Language language in supportedLanguages)
			{
				Logger.log.Debug("language: " + language.ToString());
			}
			supportedLanguages.Add(17);
			string text = File.ReadAllText(Path.Combine(Path.Combine(Path.Combine(Application.dataPath, ".."), "Plugins"), "Localization.csv"));
			object[] array = new object[] { text, 0 };
			ReflectionUtil.InvokePrivateStaticMethod(typeof(LocalizationImporter), "ImportTextFile", array);
			Localization.Instance.SelectedLanguage = 17;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021B8 File Offset: 0x000003B8
		[OnStart]
		public void OnApplicationStart()
		{
			Logger.log.Debug("OnApplicationStart");
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021C9 File Offset: 0x000003C9
		[OnExit]
		public void OnApplicationQuit()
		{
			Logger.log.Debug("OnApplicationQuit");
		}
	}
}
