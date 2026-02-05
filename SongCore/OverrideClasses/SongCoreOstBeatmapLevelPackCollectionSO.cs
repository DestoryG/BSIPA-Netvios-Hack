using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SongCore.OverrideClasses
{
	// Token: 0x02000021 RID: 33
	public class SongCoreOstBeatmapLevelPackCollectionSO : BeatmapLevelPackCollectionSO
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00007738 File Offset: 0x00005938
		public static SongCoreOstBeatmapLevelPackCollectionSO CreateNew()
		{
			SongCoreOstBeatmapLevelPackCollectionSO songCoreOstBeatmapLevelPackCollectionSO = ScriptableObject.CreateInstance<SongCoreOstBeatmapLevelPackCollectionSO>();
			songCoreOstBeatmapLevelPackCollectionSO._allBeatmapLevelPacks = new IBeatmapLevelPack[0];
			songCoreOstBeatmapLevelPackCollectionSO.UpdateArray();
			return songCoreOstBeatmapLevelPackCollectionSO;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000775E File Offset: 0x0000595E
		public void AddLevelPack(BeatmapLevelPackSO pack)
		{
			this._customBeatmapLevelPacks.Add(pack);
			this.UpdateArray();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000775E File Offset: 0x0000595E
		public void AddLevelPack(CustomBeatmapLevelPack pack)
		{
			this._customBeatmapLevelPacks.Add(pack);
			this.UpdateArray();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007774 File Offset: 0x00005974
		private void UpdateArray()
		{
			List<IBeatmapLevelPack> list = this._allBeatmapLevelPacks.ToList<IBeatmapLevelPack>();
			foreach (IBeatmapLevelPack beatmapLevelPack in this._customBeatmapLevelPacks)
			{
				if (!list.Contains(beatmapLevelPack))
				{
					list.Add(beatmapLevelPack);
				}
			}
			this._allBeatmapLevelPacks = list.ToArray();
		}

		// Token: 0x04000091 RID: 145
		internal List<IBeatmapLevelPack> _customBeatmapLevelPacks = new List<IBeatmapLevelPack>();
	}
}
