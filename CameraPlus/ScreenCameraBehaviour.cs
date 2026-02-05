using System;
using IPA.Logging;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x0200000E RID: 14
	public class ScreenCameraBehaviour : MonoBehaviour
	{
		// Token: 0x06000074 RID: 116 RVA: 0x0000752F File Offset: 0x0000572F
		public void SetRenderTexture(RenderTexture renderTexture)
		{
			this._renderTexture = renderTexture;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00007538 File Offset: 0x00005738
		public void SetCameraInfo(Vector2 position, Vector2 size, int layer)
		{
			this._cam.pixelRect = new Rect(position, size);
			this._cam.depth = (float)layer;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000755C File Offset: 0x0000575C
		public void Awake()
		{
			Logger.Log("Created new screen camera behaviour component!", Logger.Level.Info);
			Object.DontDestroyOnLoad(base.gameObject);
			this._cam = base.gameObject.AddComponent<Camera>();
			this._cam.clearFlags = 4;
			this._cam.cullingMask = 0;
			this._cam.stereoTargetEye = 0;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000075B4 File Offset: 0x000057B4
		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (this._renderTexture == null)
			{
				return;
			}
			Graphics.Blit(this._renderTexture, dest);
		}

		// Token: 0x04000089 RID: 137
		private Camera _cam;

		// Token: 0x0400008A RID: 138
		private RenderTexture _renderTexture;
	}
}
