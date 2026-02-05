using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAvatar.StereoRendering
{
	// Token: 0x0200002E RID: 46
	[DisallowMultipleComponent]
	internal class StereoRenderManager : MonoBehaviour
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00006CC0 File Offset: 0x00004EC0
		public static bool Active
		{
			get
			{
				return StereoRenderManager.instance != null;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00006CE0 File Offset: 0x00004EE0
		public static StereoRenderManager Instance
		{
			get
			{
				bool flag = StereoRenderManager.instance == null;
				if (flag)
				{
					StereoRenderManager.instance = new GameObject("StereoRenderManager").AddComponent<StereoRenderManager>();
					Plugin.logger.Info("Initialized StereoRenderManager");
				}
				return StereoRenderManager.instance;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006D2C File Offset: 0x00004F2C
		public void InvokeStereoRenderers(VRRenderEventDetector detector)
		{
			for (int i = 0; i < this.stereoRenderers.Count; i++)
			{
				StereoRenderer stereoRenderer = this.stereoRenderers[i];
				bool shouldRender = stereoRenderer.shouldRender;
				if (shouldRender)
				{
					stereoRenderer.Render(detector);
				}
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006D77 File Offset: 0x00004F77
		public void AddToManager(StereoRenderer stereoRenderer)
		{
			this.stereoRenderers.Add(stereoRenderer);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006D87 File Offset: 0x00004F87
		public void RemoveFromManager(StereoRenderer stereoRenderer)
		{
			this.stereoRenderers.Remove(stereoRenderer);
		}

		// Token: 0x0400016E RID: 366
		private static StereoRenderManager instance = null;

		// Token: 0x0400016F RID: 367
		public List<StereoRenderer> stereoRenderers = new List<StereoRenderer>();
	}
}
