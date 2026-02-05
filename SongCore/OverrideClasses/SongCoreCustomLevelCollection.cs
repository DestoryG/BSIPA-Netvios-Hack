using System;

namespace SongCore.OverrideClasses
{
	// Token: 0x02000020 RID: 32
	public class SongCoreCustomLevelCollection : CustomBeatmapLevelCollection
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00007723 File Offset: 0x00005923
		public SongCoreCustomLevelCollection(CustomPreviewBeatmapLevel[] customPreviewBeatmapLevels)
			: base(customPreviewBeatmapLevels)
		{
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000772C File Offset: 0x0000592C
		public void UpdatePreviewLevels(CustomPreviewBeatmapLevel[] levels)
		{
			this._customPreviewBeatmapLevels = levels;
		}
	}
}
