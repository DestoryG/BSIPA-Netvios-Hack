using System;
using System.Collections;
using System.Linq;
using CustomSaber.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xft;

namespace CustomSaber.Utilities
{
	// Token: 0x0200000E RID: 14
	public class DefaultSaberGrabber : MonoBehaviour
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002864 File Offset: 0x00000A64
		// (set) Token: 0x0600002D RID: 45 RVA: 0x0000286B File Offset: 0x00000A6B
		public static bool isCompleted { get; private set; } = false;

		// Token: 0x0600002E RID: 46 RVA: 0x00002874 File Offset: 0x00000A74
		private void Awake()
		{
			Object.DontDestroyOnLoad(this);
			bool flag = !DefaultSaberGrabber.isCompleted;
			if (flag)
			{
				base.StartCoroutine(this.PreloadDefaultSabers());
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000028A4 File Offset: 0x00000AA4
		private IEnumerator PreloadDefaultSabers()
		{
			bool isSceneLoaded = false;
			string sceneName = "GameCore";
			try
			{
				Logger.log.Debug("Loading " + sceneName + " scene");
				AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, 1);
				while (!loadScene.isDone)
				{
					yield return null;
				}
				isSceneLoaded = true;
				Logger.log.Debug("Loaded!");
				BasicSaberModelController saber = Resources.FindObjectsOfTypeAll<BasicSaberModelController>().FirstOrDefault<BasicSaberModelController>();
				DefaultSaberGrabber.trail = saber.GetComponent<XWeaponTrail>();
				Logger.log.Debug("Got sabers!");
				Logger.log.Debug(string.Concat(new string[]
				{
					"Saber: ",
					saber.name,
					", GameObj: ",
					saber.gameObject.name,
					", ",
					saber.ToString()
				}));
				DefaultSaberGrabber.defaultLeftSaber = Object.Instantiate<BasicSaberModelController>(saber).gameObject;
				Object.DestroyImmediate(DefaultSaberGrabber.defaultLeftSaber.GetComponent<BasicSaberModelController>());
				Object.DestroyImmediate(DefaultSaberGrabber.defaultLeftSaber.GetComponentInChildren<ConditionalMaterialSwitcher>());
				foreach (SetSaberGlowColor c in DefaultSaberGrabber.defaultLeftSaber.GetComponentsInChildren<SetSaberGlowColor>())
				{
					Object.DestroyImmediate(c);
					c = null;
				}
				SetSaberGlowColor[] array = null;
				Object.DontDestroyOnLoad(DefaultSaberGrabber.defaultLeftSaber);
				DefaultSaberGrabber.defaultLeftSaber.transform.SetParent(base.transform);
				DefaultSaberGrabber.defaultLeftSaber.gameObject.name = "LeftSaber";
				DefaultSaberGrabber.defaultLeftSaber.transform.localPosition = Vector3.zero;
				DefaultSaberGrabber.defaultLeftSaber.transform.localRotation = Quaternion.identity;
				DefaultSaberGrabber.defaultLeftSaber.AddComponent<DummySaber>();
				DefaultSaberGrabber.defaultRightSaber = Object.Instantiate<BasicSaberModelController>(saber).gameObject;
				Object.DestroyImmediate(DefaultSaberGrabber.defaultRightSaber.GetComponent<BasicSaberModelController>());
				Object.DestroyImmediate(DefaultSaberGrabber.defaultRightSaber.GetComponentInChildren<ConditionalMaterialSwitcher>());
				foreach (SetSaberGlowColor c2 in DefaultSaberGrabber.defaultRightSaber.GetComponentsInChildren<SetSaberGlowColor>())
				{
					Object.DestroyImmediate(c2);
					c2 = null;
				}
				SetSaberGlowColor[] array2 = null;
				Object.DontDestroyOnLoad(DefaultSaberGrabber.defaultRightSaber);
				DefaultSaberGrabber.defaultRightSaber.transform.SetParent(base.transform);
				DefaultSaberGrabber.defaultRightSaber.gameObject.name = "RightSaber";
				DefaultSaberGrabber.defaultRightSaber.transform.localPosition = Vector3.zero;
				DefaultSaberGrabber.defaultRightSaber.transform.localRotation = Quaternion.identity;
				DefaultSaberGrabber.defaultRightSaber.AddComponent<DummySaber>();
				Logger.log.Debug("Finished! Got default sabers! Setting active state");
				bool flag = DefaultSaberGrabber.defaultLeftSaber;
				if (flag)
				{
					Logger.log.Debug("Found default left saber");
					DefaultSaberGrabber.defaultLeftSaber.SetActive(false);
				}
				bool flag2 = DefaultSaberGrabber.defaultRightSaber;
				if (flag2)
				{
					Logger.log.Debug("Found default right saber");
					DefaultSaberGrabber.defaultRightSaber.SetActive(false);
				}
				bool flag3 = DefaultSaberGrabber.defaultLeftSaber && DefaultSaberGrabber.defaultRightSaber;
				if (flag3)
				{
					CustomSaberData defaultSabers = new CustomSaberData(DefaultSaberGrabber.defaultLeftSaber.gameObject, DefaultSaberGrabber.defaultRightSaber.gameObject);
					SaberAssetLoader.CustomSabers[0] = defaultSabers;
					DefaultSaberGrabber.isCompleted = true;
					defaultSabers = null;
				}
				loadScene = null;
				saber = null;
			}
			finally
			{
				bool flag4 = isSceneLoaded;
				if (flag4)
				{
					Logger.log.Debug("Unloading " + sceneName);
					SceneManager.UnloadSceneAsync(sceneName);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04000034 RID: 52
		public static GameObject defaultLeftSaber = null;

		// Token: 0x04000035 RID: 53
		public static GameObject defaultRightSaber = null;

		// Token: 0x04000036 RID: 54
		public static XWeaponTrail trail = null;
	}
}
