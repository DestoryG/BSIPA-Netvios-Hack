using System;
using System.Runtime.InteropServices;
using NetViosCommon.Utility;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NetviosSdkPlugin
{
	// Token: 0x02000005 RID: 5
	public class NetviosSdkPluginController : SDKDataComponent
	{
		// Token: 0x0600000C RID: 12
		[DllImport("NetViosSDK")]
		public static extern void NetViosSDK_Init(string appID, string appSecret, NetviosSdkPluginController.InitCallback callback);

		// Token: 0x0600000D RID: 13
		[DllImport("NetViosSDK")]
		public static extern void NetViosSDK_Start();

		// Token: 0x0600000E RID: 14
		[DllImport("NetViosSDK")]
		public static extern void NetViosSDK_Close();

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002112 File Offset: 0x00000312
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002109 File Offset: 0x00000309
		private bool HasHealthWarningEntered { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000211A File Offset: 0x0000031A
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002121 File Offset: 0x00000321
		public static NetviosSdkPluginController instance { get; private set; }

		// Token: 0x06000013 RID: 19 RVA: 0x0000212C File Offset: 0x0000032C
		private void Awake()
		{
			if (NetviosSdkPluginController.instance != null)
			{
				Logger.log.Warn("Instance of " + base.GetType().Name + " already exists, destroying.");
				Object.DestroyImmediate(this);
				return;
			}
			Object.DontDestroyOnLoad(this);
			NetviosSdkPluginController.instance = this;
			Logger.log.Debug(base.name + ": Awake()");
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			NetviosSdkPluginController.sdkCallback = false;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000021B0 File Offset: 0x000003B0
		private void Start()
		{
			Logger.log.Debug(base.name + ": Start()");
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000021CC File Offset: 0x000003CC
		private void Update()
		{
			if (NetviosSdkPluginController.sdkCallback)
			{
				Logger.log.Debug(base.name + ": Update() with sdkCallback true");
				object obj = NetviosSdkPluginController.lockObj;
				lock (obj)
				{
					this.processSDKInitCallback();
					NetviosSdkPluginController.sdkCallback = false;
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002238 File Offset: 0x00000438
		private void processSDKInitCallback()
		{
			Logger.log.Debug("deltaTimeInMs: " + NetviosSdkPluginController.deltaTimeInMs.ToString());
			if (NetviosSdkPluginController.sdkCode == 0)
			{
				Logger.log.Debug("NetViosSDK_Start");
				JObject jobject = JObject.Parse(NetviosSdkPluginController.sdkJson);
				jobject.Remove("token");
				NetviosSdkPluginController.sdkJson = jobject.ToString();
				base.triggerEvent(NetviosSdkPluginController.sdkJson);
				NetviosSdkPluginController.NetViosSDK_Start();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000020F8 File Offset: 0x000002F8
		private void LateUpdate()
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000020F8 File Offset: 0x000002F8
		private void OnEnable()
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000020F8 File Offset: 0x000002F8
		private void OnDisable()
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000022B1 File Offset: 0x000004B1
		private void OnDestroy()
		{
			Logger.log.Debug(base.name + ": OnDestroy()");
			NetviosSdkPluginController.instance = null;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022D4 File Offset: 0x000004D4
		private void OnApplicationQuit()
		{
			Logger.log.Debug("Application ending after " + Time.time.ToString() + " seconds");
			NetviosSdkPluginController.NetViosSDK_Close();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000230C File Offset: 0x0000050C
		private void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
			Logger.log.Info("OnActiveSceneChanged prevScene:" + prevScene.name);
			Logger.log.Info("OnActiveSceneChanged nextScene:" + nextScene.name);
			if (nextScene.name == "HealthWarning" && !this.HasHealthWarningEntered)
			{
				this.HasHealthWarningEntered = true;
				this.SetupInHealthWarningScene();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002378 File Offset: 0x00000578
		private void SetupInHealthWarningScene()
		{
			Logger.log.Debug("Yeah, we can do NetviosSDK_Init() now ...");
			NetviosSdkPluginController.begin = DateTime.Now;
			try
			{
				NetviosSdkPluginController.NetViosSDK_Init(this.appID, this.appSecret, delegate(int code, string json)
				{
					Logger.log.Debug("code: " + code.ToString());
					Logger.log.Debug("json: " + json);
					NetviosSdkPluginController.deltaTimeInMs = (int)(DateTime.Now - NetviosSdkPluginController.begin).TotalMilliseconds;
					object obj = NetviosSdkPluginController.lockObj;
					lock (obj)
					{
						NetviosSdkPluginController.sdkCallback = true;
						NetviosSdkPluginController.sdkCode = code;
						NetviosSdkPluginController.sdkJson = json;
					}
				});
			}
			catch (Exception ex)
			{
				Logger.log.Error("NetviosSDK_Init error: " + ex.ToString());
				Application.Quit();
			}
		}

		// Token: 0x04000004 RID: 4
		public const string HEALTH_WARNING = "HealthWarning";

		// Token: 0x04000006 RID: 6
		private static object lockObj = new object();

		// Token: 0x04000007 RID: 7
		private static volatile int sdkCode;

		// Token: 0x04000008 RID: 8
		private static volatile string sdkJson;

		// Token: 0x04000009 RID: 9
		private static volatile bool sdkCallback;

		// Token: 0x0400000A RID: 10
		private static volatile int deltaTimeInMs;

		// Token: 0x0400000B RID: 11
		private static DateTime begin;

		// Token: 0x0400000C RID: 12
		private string appID = "f263055fc8584f8e94065fbf838e1000";

		// Token: 0x0400000D RID: 13
		private string appSecret = "24a54b16c4924c05a710d8a6a3bef806";

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000021 RID: 33
		public delegate void InitCallback(int code, string json);

		// Token: 0x02000007 RID: 7
		// (Invoke) Token: 0x06000025 RID: 37
		public delegate void StartCallback(int code, string json);
	}
}
