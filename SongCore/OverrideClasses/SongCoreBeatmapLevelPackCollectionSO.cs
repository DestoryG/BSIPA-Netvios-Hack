using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SongCore.OverrideClasses
{
	// Token: 0x0200001F RID: 31
	public class SongCoreBeatmapLevelPackCollectionSO : BeatmapLevelPackCollectionSO
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00007660 File Offset: 0x00005860
		public static SongCoreBeatmapLevelPackCollectionSO CreateNew()
		{
			SongCoreBeatmapLevelPackCollectionSO songCoreBeatmapLevelPackCollectionSO = ScriptableObject.CreateInstance<SongCoreBeatmapLevelPackCollectionSO>();
			songCoreBeatmapLevelPackCollectionSO._allBeatmapLevelPacks = new IBeatmapLevelPack[0];
			songCoreBeatmapLevelPackCollectionSO.UpdateArray();
			return songCoreBeatmapLevelPackCollectionSO;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007686 File Offset: 0x00005886
		public void AddLevelPack(CustomBeatmapLevelPack pack)
		{
			this._customBeatmapLevelPacks.Add(pack);
			this.UpdateArray();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000769C File Offset: 0x0000589C
		private void UpdateArray()
		{
			List<IBeatmapLevelPack> list = this._allBeatmapLevelPacks.ToList<IBeatmapLevelPack>();
			foreach (CustomBeatmapLevelPack customBeatmapLevelPack in this._customBeatmapLevelPacks)
			{
				if (!list.Contains(customBeatmapLevelPack))
				{
					list.Add(customBeatmapLevelPack);
				}
			}
			this._allBeatmapLevelPacks = list.ToArray();
		}

		// Token: 0x04000090 RID: 144
		internal List<CustomBeatmapLevelPack> _customBeatmapLevelPacks = new List<CustomBeatmapLevelPack>();
	}
}
