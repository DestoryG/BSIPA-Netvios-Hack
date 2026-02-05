using System;
using System.Text;
using IPA.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetViosCommon.Utility
{
	// Token: 0x0200000A RID: 10
	public class UnityUtil
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000023E3 File Offset: 0x000005E3
		public static GameObject[] GetAllGameObject()
		{
			return Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023F9 File Offset: 0x000005F9
		public static Component[] GetAllComponents(GameObject obj)
		{
			return obj.GetComponents(typeof(Component));
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000240C File Offset: 0x0000060C
		public static void GetAllObjectInCurrentScene(Logger logger)
		{
			GameObject[] allGameObject = UnityUtil.GetAllGameObject();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("******* Current scene: " + SceneManager.GetActiveScene().name + "\n");
			foreach (GameObject gameObject in allGameObject)
			{
				stringBuilder.Append("obj.name: " + gameObject.name + "\n");
				foreach (Component component in UnityUtil.GetAllComponents(gameObject))
				{
					stringBuilder.Append("\t " + component.GetType().ToString() + "\n");
				}
			}
			logger.Debug(stringBuilder.ToString());
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000024CC File Offset: 0x000006CC
		public static void GetAllObjectInCurrentSceneExcludeTransform(Logger logger)
		{
			GameObject[] allGameObject = UnityUtil.GetAllGameObject();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("******* Current scene: " + SceneManager.GetActiveScene().name + "\n");
			foreach (GameObject gameObject in allGameObject)
			{
				stringBuilder.Append("obj.name: " + gameObject.name + "\n");
				foreach (Component component in UnityUtil.GetAllComponents(gameObject))
				{
					if (!(component as Transform))
					{
						stringBuilder.Append("\t " + component.GetType().ToString() + "\n");
					}
				}
			}
			logger.Debug(stringBuilder.ToString());
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002598 File Offset: 0x00000798
		public static void DumpScenesTransitionSetupDataSO(ScenesTransitionSetupDataSO data, Logger logger)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("******* Current scene: " + SceneManager.GetActiveScene().name + "\n");
			if (data == null)
			{
				stringBuilder.Append("data is null!!!\n");
				logger.Debug(stringBuilder.ToString());
				return;
			}
			stringBuilder.Append("scenes:[ ");
			foreach (SceneInfo sceneInfo in data.scenes)
			{
				if (sceneInfo == null)
				{
					stringBuilder.Append("null, ");
				}
				else
				{
					stringBuilder.Append(sceneInfo.sceneName + ", ");
				}
			}
			stringBuilder.Append(" ]\n");
			logger.Debug(stringBuilder.ToString());
		}
	}
}
