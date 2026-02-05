using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IPA.Logging;
using IPA.Utilities;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x02000006 RID: 6
	public class CameraUtilities
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0000296C File Offset: 0x00000B6C
		public static bool CameraExists(string cameraName)
		{
			return Plugin.Instance.Cameras.Keys.Where((string c) => c == cameraName + ".cfg").Count<string>() > 0;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000029B0 File Offset: 0x00000BB0
		public static void AddNewCamera(string cameraName, Config CopyConfig = null, bool meme = false)
		{
			string text = Path.Combine(UnityGame.UserDataPath, Plugin.Name, cameraName + ".cfg");
			if (!File.Exists(text))
			{
				if (cameraName == Plugin.MainCamera)
				{
					string text2 = Path.Combine(Environment.CurrentDirectory, Plugin.MainCamera + ".cfg");
					if (File.Exists(text2))
					{
						if (!Directory.Exists(Path.GetDirectoryName(text)))
						{
							Directory.CreateDirectory(Path.GetDirectoryName(text));
						}
						File.Move(text2, text);
						Logger.Log(string.Concat(new string[]
						{
							"Copied old ",
							Plugin.MainCamera,
							".cfg into new ",
							Plugin.Name,
							" folder in UserData"
						}), Logger.Level.Info);
					}
				}
				Config config = null;
				if (CopyConfig != null)
				{
					File.Copy(CopyConfig.FilePath, text, true);
				}
				config = new Config(text);
				foreach (CameraPlusInstance cameraPlusInstance in Plugin.Instance.Cameras.Values.OrderBy((CameraPlusInstance i) => i.Config.layer))
				{
					if (cameraPlusInstance.Config.layer > config.layer)
					{
						config.layer += cameraPlusInstance.Config.layer - config.layer;
					}
					else if (cameraPlusInstance.Config.layer == config.layer)
					{
						config.layer++;
					}
				}
				if (cameraName == Plugin.MainCamera)
				{
					config.fitToCanvas = true;
				}
				if (meme)
				{
					config.screenWidth = (int)Random.Range(200f, (float)Screen.width / 1.5f);
					config.screenHeight = (int)Random.Range(200f, (float)Screen.height / 1.5f);
					config.screenPosX = Random.Range(-200, Screen.width - config.screenWidth + 200);
					config.screenPosY = Random.Range(-200, Screen.height - config.screenHeight + 200);
					config.thirdPerson = Random.Range(0, 2) == 0;
					config.renderScale = Random.Range(0.1f, 1f);
					config.posx += (float)Random.Range(-5, 5);
					config.posy += (float)Random.Range(-2, 2);
					config.posz += (float)Random.Range(-5, 5);
					config.angx = (float)Random.Range(0, 360);
					config.angy = (float)Random.Range(0, 360);
					config.angz = (float)Random.Range(0, 360);
				}
				else if (CopyConfig == null && cameraName != Plugin.MainCamera)
				{
					config.screenHeight /= 4;
					config.screenWidth /= 4;
				}
				config.Position = config.DefaultPosition;
				config.Rotation = config.DefaultRotation;
				config.FirstPersonPositionOffset = config.DefaultFirstPersonPositionOffset;
				config.FirstPersonRotationOffset = config.DefaultFirstPersonRotationOffset;
				config.Save();
				Logger.Log("Success creating new camera \"" + cameraName + "\"", Logger.Level.Info);
				return;
			}
			Logger.Log("Camera \"" + cameraName + "\" already exists!", Logger.Level.Info);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002D0C File Offset: 0x00000F0C
		public static string GetNextCameraName()
		{
			int num = 1;
			string text = string.Empty;
			for (;;)
			{
				text = "customcamera" + num.ToString();
				if (!CameraUtilities.CameraExists(text))
				{
					break;
				}
				num++;
			}
			return text;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002D44 File Offset: 0x00000F44
		public static bool RemoveCamera(CameraPlusBehaviour instance, bool delete = true)
		{
			try
			{
				if (Path.GetFileName(instance.Config.FilePath) != Plugin.MainCamera + ".cfg")
				{
					ConcurrentDictionary<string, CameraPlusInstance> cameras = Plugin.Instance.Cameras;
					IEnumerable<KeyValuePair<string, CameraPlusInstance>> enumerable = Plugin.Instance.Cameras.Where((KeyValuePair<string, CameraPlusInstance> c) => c.Value.Instance == instance && c.Key != Plugin.MainCamera + ".cfg");
					CameraPlusInstance cameraPlusInstance;
					if (cameras.TryRemove((enumerable != null) ? enumerable.First<KeyValuePair<string, CameraPlusInstance>>().Key : null, out cameraPlusInstance))
					{
						if (delete && File.Exists(cameraPlusInstance.Config.FilePath))
						{
							File.Delete(cameraPlusInstance.Config.FilePath);
						}
						GL.Clear(false, true, Color.black, 0f);
						Object.Destroy(cameraPlusInstance.Instance.gameObject);
						return true;
					}
				}
				else
				{
					Logger.Log("One does not simply remove the main camera!", Logger.Level.Warning);
				}
			}
			catch (Exception ex)
			{
				string text = ((instance != null && instance.Config != null && instance.Config.FilePath != null) ? ("Could not remove camera with configuration: '" + Path.GetFileName(instance.Config.FilePath) + "'.") : "Could not remove camera.");
				Logger.Log(string.Concat(new string[] { text, " CameraUtilities.RemoveCamera() threw an exception: ", ex.Message, "\n", ex.StackTrace }), Logger.Level.Error);
			}
			return false;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002ED8 File Offset: 0x000010D8
		public static void SetAllCameraCulling()
		{
			try
			{
				CameraPlusInstance[] array = Plugin.Instance.Cameras.Values.ToArray<CameraPlusInstance>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Instance.SetCullingMask();
				}
			}
			catch (Exception ex)
			{
				Logger.Log("Exception cameras culling! Exception: " + ex.Message + "\n" + ex.StackTrace, Logger.Level.Error);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002F4C File Offset: 0x0000114C
		public static void ReloadCameras()
		{
			try
			{
				if (!Directory.Exists(Path.Combine(UnityGame.UserDataPath, Plugin.Name)))
				{
					Directory.CreateDirectory(Path.Combine(UnityGame.UserDataPath, Plugin.Name));
				}
				foreach (string text in Directory.GetFiles(Path.Combine(UnityGame.UserDataPath, Plugin.Name)))
				{
					string fileName = Path.GetFileName(text);
					if (fileName.EndsWith(".cfg") && !Plugin.Instance.Cameras.ContainsKey(fileName))
					{
						Logger.Log("Found config " + text + "!", Logger.Level.Info);
						Plugin.Instance.Cameras.TryAdd(fileName, new CameraPlusInstance(text));
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log("Exception while reloading cameras! Exception: " + ex.Message + "\n" + ex.StackTrace, Logger.Level.Error);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003038 File Offset: 0x00001238
		public static IEnumerator Spawn38Cameras()
		{
			ConcurrentDictionary<string, CameraPlusInstance> concurrentDictionary = Plugin.Instance.Cameras;
			lock (concurrentDictionary)
			{
				int num;
				for (int i = 0; i < 38; i = num + 1)
				{
					CameraUtilities.AddNewCamera(CameraUtilities.GetNextCameraName(), null, true);
					CameraUtilities.ReloadCameras();
					yield return null;
					num = i;
				}
			}
			concurrentDictionary = null;
			yield break;
			yield break;
		}
	}
}
