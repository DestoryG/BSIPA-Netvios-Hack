using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DynamicOpenVR.IO;
using DynamicOpenVR.Logging;
using HarmonyLib;
using IPA;
using IPA.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x0200000A RID: 10
	[Plugin(RuntimeOptions.SingleStartInit)]
	internal class Plugin
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002221 File Offset: 0x00000421
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002228 File Offset: 0x00000428
		public static global::IPA.Logging.Logger logger { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002230 File Offset: 0x00000430
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002237 File Offset: 0x00000437
		public static VectorInput leftTriggerValue { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000223F File Offset: 0x0000043F
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002246 File Offset: 0x00000446
		public static VectorInput rightTriggerValue { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000224E File Offset: 0x0000044E
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002255 File Offset: 0x00000455
		public static BooleanInput menu { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000225D File Offset: 0x0000045D
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002264 File Offset: 0x00000464
		public static HapticVibrationOutput leftSlice { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000226C File Offset: 0x0000046C
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002273 File Offset: 0x00000473
		public static HapticVibrationOutput rightSlice { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000227B File Offset: 0x0000047B
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002282 File Offset: 0x00000482
		public static PoseInput leftHandPose { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000228A File Offset: 0x0000048A
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002291 File Offset: 0x00000491
		public static PoseInput rightHandPose { get; private set; }

		// Token: 0x06000021 RID: 33 RVA: 0x00002299 File Offset: 0x00000499
		[Init]
		public Plugin(global::IPA.Logging.Logger logger)
		{
			Plugin.logger = logger;
			DynamicOpenVR.Logging.Logger.handler = new IPALogHandler();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000022CC File Offset: 0x000004CC
		[OnStart]
		public void OnStart()
		{
			Plugin.logger.Info("Starting " + typeof(Plugin).Namespace);
			try
			{
				OpenVRUtilities.Init();
			}
			catch (Exception ex)
			{
				Plugin.logger.Error("Failed to initialize OpenVR API; DynamicOpenVR will not run");
				Plugin.logger.Error(ex);
				return;
			}
			Plugin.logger.Info("Successfully initialized OpenVR API");
			try
			{
				this.AddManifestToSteamConfig();
			}
			catch (Exception ex2)
			{
				Plugin.logger.Error("Failed to update SteamVR manifest.");
				Plugin.logger.Error(ex2);
			}
			this.RegisterActionSet();
			this.ApplyHarmonyPatches();
			OpenVRActionManager.instance.Initialize(this._actionManifestPath);
			OpenVREventHandler openVREventHandler = new GameObject("OpenVREventHandler").AddComponent<OpenVREventHandler>();
			Object.DontDestroyOnLoad(openVREventHandler.gameObject);
			openVREventHandler.gamePaused += this.OnGamePaused;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023B8 File Offset: 0x000005B8
		[OnExit]
		public void OnExit()
		{
			VectorInput leftTriggerValue = Plugin.leftTriggerValue;
			if (leftTriggerValue != null)
			{
				leftTriggerValue.Dispose();
			}
			VectorInput rightTriggerValue = Plugin.rightTriggerValue;
			if (rightTriggerValue != null)
			{
				rightTriggerValue.Dispose();
			}
			BooleanInput menu = Plugin.menu;
			if (menu != null)
			{
				menu.Dispose();
			}
			HapticVibrationOutput leftSlice = Plugin.leftSlice;
			if (leftSlice != null)
			{
				leftSlice.Dispose();
			}
			HapticVibrationOutput rightSlice = Plugin.rightSlice;
			if (rightSlice != null)
			{
				rightSlice.Dispose();
			}
			PoseInput leftHandPose = Plugin.leftHandPose;
			if (leftHandPose != null)
			{
				leftHandPose.Dispose();
			}
			PoseInput rightHandPose = Plugin.rightHandPose;
			if (rightHandPose == null)
			{
				return;
			}
			rightHandPose.Dispose();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002434 File Offset: 0x00000634
		private void OnGamePaused()
		{
			PauseController pauseController = Resources.FindObjectsOfTypeAll<PauseController>().FirstOrDefault<PauseController>();
			if (pauseController == null)
			{
				return;
			}
			pauseController.Pause();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000244C File Offset: 0x0000064C
		private void AddManifestToSteamConfig()
		{
			string steamHomeDirectory = SteamUtilities.GetSteamHomeDirectory();
			string manifestPath = Path.Combine(Environment.CurrentDirectory, "beatsaber.vrmanifest");
			string text = Path.Combine(steamHomeDirectory, "config", "appconfig.json");
			string globalManifestPath = Path.Combine(steamHomeDirectory, "config", "steamapps.vrmanifest");
			JObject jobject = this.ReadBeatSaberManifest(globalManifestPath);
			jobject["action_manifest_path"] = this._actionManifestPath;
			JObject jobject2 = new JObject { 
			{
				"applications",
				new JArray { jobject }
			} };
			this.WriteBeatSaberManifest(manifestPath, jobject2);
			JObject jobject3 = this.ReadAppConfig(text);
			JArray jarray = jobject3["manifest_paths"].Value<JArray>();
			List<JToken> list = jarray.Where((JToken p) => p.Value<string>() == manifestPath).ToList<JToken>();
			if (jarray.IndexOf(list.FirstOrDefault<JToken>()) != 0 || list.Count > 1)
			{
				Plugin.logger.Info("Adding '" + manifestPath + "' to app config");
				foreach (JToken jtoken in list)
				{
					jarray.Remove(jtoken);
				}
				jarray.Insert(0, manifestPath);
			}
			else
			{
				Plugin.logger.Info("Manifest is already in app config");
			}
			if (!jarray.Any((JToken s) => s.Value<string>().Equals(globalManifestPath, StringComparison.InvariantCultureIgnoreCase)))
			{
				Plugin.logger.Info("Adding '" + globalManifestPath + "' to app config");
				jarray.Add(globalManifestPath);
				return;
			}
			Plugin.logger.Info("Global manifest is already in app config");
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002620 File Offset: 0x00000820
		private JObject ReadBeatSaberManifest(string globalManifestPath)
		{
			if (!File.Exists(globalManifestPath))
			{
				throw new FileNotFoundException("Could not find file " + globalManifestPath);
			}
			Plugin.logger.Trace("Reading '" + globalManifestPath + "'");
			JObject jobject2;
			using (StreamReader streamReader = new StreamReader(globalManifestPath))
			{
				JToken jtoken = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd())["applications"];
				JObject jobject;
				if (jtoken == null)
				{
					jobject = null;
				}
				else
				{
					JArray jarray = jtoken.Value<JArray>();
					if (jarray == null)
					{
						jobject = null;
					}
					else
					{
						JToken jtoken2 = jarray.FirstOrDefault(delegate(JToken a)
						{
							JToken jtoken3 = a["app_key"];
							return ((jtoken3 != null) ? jtoken3.Value<string>() : null) == "steam.app.000000";
						});
						jobject = ((jtoken2 != null) ? jtoken2.Value<JObject>() : null);
					}
				}
				jobject2 = jobject;
			}
			if (jobject2 == null)
			{
				throw new Exception("Beat Saber manifest not found in '" + globalManifestPath + "'");
			}
			return jobject2;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000026F8 File Offset: 0x000008F8
		private JObject ReadAppConfig(string configPath)
		{
			JObject jobject = new JObject();
			if (!File.Exists(configPath))
			{
				Plugin.logger.Warn("Could not find file '" + configPath + "'");
				jobject.Add("manifest_paths", new JArray());
				return jobject;
			}
			Plugin.logger.Trace("Reading '" + configPath + "'");
			using (StreamReader streamReader = new StreamReader(configPath))
			{
				jobject = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());
			}
			if (jobject == null)
			{
				Plugin.logger.Warn("File is empty");
				jobject = new JObject();
			}
			if (!jobject.ContainsKey("manifest_paths"))
			{
				Plugin.logger.Warn("manifest_paths is missing");
				jobject.Add("manifest_paths", new JArray());
			}
			return jobject;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027D0 File Offset: 0x000009D0
		private void WriteBeatSaberManifest(string manifestPath, JObject beatSaberManifest)
		{
			Plugin.logger.Info("Writing manifest to '" + manifestPath + "'");
			using (StreamWriter streamWriter = new StreamWriter(manifestPath))
			{
				streamWriter.Write(JsonConvert.SerializeObject(beatSaberManifest, Formatting.Indented));
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002828 File Offset: 0x00000A28
		private void WriteAppConfig(string configPath, JObject appConfig)
		{
			Plugin.logger.Info("Writing app config to '" + configPath + "'");
			using (StreamWriter streamWriter = new StreamWriter(configPath))
			{
				streamWriter.Write(JsonConvert.SerializeObject(appConfig, Formatting.Indented));
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002880 File Offset: 0x00000A80
		private void RegisterActionSet()
		{
			Plugin.logger.Info("Registering actions");
			Plugin.leftTriggerValue = new VectorInput("/actions/main/in/lefttriggervalue");
			Plugin.rightTriggerValue = new VectorInput("/actions/main/in/righttriggervalue");
			Plugin.menu = new BooleanInput("/actions/main/in/menu");
			Plugin.leftSlice = new HapticVibrationOutput("/actions/main/out/leftslice");
			Plugin.rightSlice = new HapticVibrationOutput("/actions/main/out/rightslice");
			Plugin.leftHandPose = new PoseInput("/actions/main/in/lefthandpose");
			Plugin.rightHandPose = new PoseInput("/actions/main/in/righthandpose");
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002905 File Offset: 0x00000B05
		private void ApplyHarmonyPatches()
		{
			Plugin.logger.Info("Applying input patches");
			this._harmonyInstance = new Harmony("com.nicoco007.dynamic-open-vr-beat-saber");
			this._harmonyInstance.PatchAll();
		}

		// Token: 0x0400003F RID: 63
		private readonly string _actionManifestPath = Path.Combine(Environment.CurrentDirectory, "DynamicOpenVR", "action_manifest.json");

		// Token: 0x04000040 RID: 64
		private Harmony _harmonyInstance;
	}
}
