using System;
using System.IO;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x02000003 RID: 3
	public class CameraPlusInstance
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002410 File Offset: 0x00000610
		public CameraPlusInstance(string configPath)
		{
			this.Config = new Config(configPath);
			GameObject gameObject = new GameObject("CamPlus_" + Path.GetFileName(configPath));
			this.Instance = gameObject.AddComponent<CameraPlusBehaviour>();
			this.Instance.Init(this.Config);
		}

		// Token: 0x0400000B RID: 11
		private readonly WaitForSecondsRealtime _waitForSecondsRealtime = new WaitForSecondsRealtime(0.1f);

		// Token: 0x0400000C RID: 12
		public Config Config;

		// Token: 0x0400000D RID: 13
		public CameraPlusBehaviour Instance;
	}
}
