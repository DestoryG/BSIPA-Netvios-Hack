using System;
using UnityEngine;

namespace SongCore.OverrideClasses
{
	// Token: 0x0200001E RID: 30
	public class SongCoreCustomBeatmapLevelPack : CustomBeatmapLevelPack
	{
		// Token: 0x0600017C RID: 380 RVA: 0x000075FC File Offset: 0x000057FC
		public SongCoreCustomBeatmapLevelPack(string packID, string packName, Sprite coverImage, CustomBeatmapLevelCollection customBeatmapLevelCollection, string shortPackName = "")
			: base(packID, packName, shortPackName, coverImage, customBeatmapLevelCollection)
		{
			coverImage = Sprite.Create(coverImage.texture, coverImage.rect, coverImage.pivot, (float)coverImage.texture.width);
			this._coverImage = coverImage;
			if (shortPackName == "")
			{
				this._shortPackName = packName;
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007657 File Offset: 0x00005857
		public void UpdateLevelCollection(CustomBeatmapLevelCollection newLevelCollection)
		{
			this._customBeatmapLevelCollection = newLevelCollection;
		}
	}
}
