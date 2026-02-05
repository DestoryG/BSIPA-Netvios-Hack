using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000C4 RID: 196
	public class AnimationControllerData
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00012939 File Offset: 0x00010B39
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x00012941 File Offset: 0x00010B41
		public bool IsPlaying { get; set; } = true;

		// Token: 0x0600040F RID: 1039 RVA: 0x0001294C File Offset: 0x00010B4C
		public AnimationControllerData(Texture2D tex, Rect[] uvs, float[] delays)
		{
			this.sprites = new Sprite[uvs.Length];
			float num = -1f;
			for (int i = 0; i < uvs.Length; i++)
			{
				this.sprites[i] = Sprite.Create(tex, new Rect(uvs[i].x * (float)tex.width, uvs[i].y * (float)tex.height, uvs[i].width * (float)tex.width, uvs[i].height * (float)tex.height), new Vector2(0f, 0f), 100f);
				if (i == 0)
				{
					num = delays[i];
				}
				if (delays[i] != num)
				{
					this._isDelayConsistent = false;
				}
			}
			this.sprite = Utilities.LoadSpriteFromTexture(tex, 100f);
			this.uvs = uvs;
			this.delays = delays;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00012A58 File Offset: 0x00010C58
		internal void CheckFrame(DateTime now)
		{
			if (this.activeImages.Count == 0)
			{
				return;
			}
			double totalMilliseconds = (now - this.lastSwitch).TotalMilliseconds;
			if (totalMilliseconds < (double)this.delays[this.uvIndex])
			{
				return;
			}
			if (this._isDelayConsistent && this.delays[this.uvIndex] <= 10f && totalMilliseconds < 100.0)
			{
				return;
			}
			this.lastSwitch = now;
			do
			{
				this.uvIndex++;
				if (this.uvIndex >= this.uvs.Length)
				{
					this.uvIndex = 0;
				}
			}
			while (!this._isDelayConsistent && this.delays[this.uvIndex] == 0f);
			foreach (Image image in this.activeImages)
			{
				image.sprite = this.sprites[this.uvIndex];
			}
		}

		// Token: 0x04000145 RID: 325
		public Sprite sprite;

		// Token: 0x04000146 RID: 326
		public int uvIndex;

		// Token: 0x04000147 RID: 327
		public DateTime lastSwitch = DateTime.UtcNow;

		// Token: 0x04000148 RID: 328
		public Rect[] uvs;

		// Token: 0x04000149 RID: 329
		public float[] delays;

		// Token: 0x0400014A RID: 330
		public Sprite[] sprites;

		// Token: 0x0400014C RID: 332
		public Material animMaterial;

		// Token: 0x0400014D RID: 333
		private bool _isDelayConsistent = true;

		// Token: 0x0400014E RID: 334
		public List<Image> activeImages = new List<Image>();
	}
}
