using System;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000CD RID: 205
	public struct FrameInfo
	{
		// Token: 0x06000469 RID: 1129 RVA: 0x00013638 File Offset: 0x00011838
		public FrameInfo(int width, int height)
		{
			this.width = width;
			this.height = height;
			this.colors = new Color32[width * height];
			this.delay = 0;
		}

		// Token: 0x04000159 RID: 345
		public int width;

		// Token: 0x0400015A RID: 346
		public int height;

		// Token: 0x0400015B RID: 347
		public Color32[] colors;

		// Token: 0x0400015C RID: 348
		public int delay;
	}
}
