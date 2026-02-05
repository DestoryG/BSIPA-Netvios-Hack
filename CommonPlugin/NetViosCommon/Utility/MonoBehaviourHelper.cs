using System;
using System.Collections;
using UnityEngine;

namespace NetViosCommon.Utility
{
	// Token: 0x02000007 RID: 7
	public class MonoBehaviourHelper : MonoBehaviour
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002224 File Offset: 0x00000424
		public static MonoBehaviourHelper Instance
		{
			get
			{
				if (MonoBehaviourHelper.mInstance == null)
				{
					GameObject gameObject = new GameObject("Plugin_MonoBehaviourHelper");
					Object.DontDestroyOnLoad(gameObject);
					MonoBehaviourHelper.mInstance = gameObject.AddComponent<MonoBehaviourHelper>();
				}
				return MonoBehaviourHelper.mInstance;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002252 File Offset: 0x00000452
		public Coroutine MonoStartCoroutine(IEnumerator routine)
		{
			return base.StartCoroutine(routine);
		}

		// Token: 0x0400000D RID: 13
		private static MonoBehaviourHelper mInstance;
	}
}
