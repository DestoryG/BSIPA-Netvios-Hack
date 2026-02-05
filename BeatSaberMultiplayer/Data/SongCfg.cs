using System;
using Newtonsoft.Json;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x0200009C RID: 156
	public class SongCfg
	{
		// Token: 0x060009C3 RID: 2499 RVA: 0x00027230 File Offset: 0x00025430
		public SongCfg(string songId, string songName, string mode, string difficulty, string songCoverImg, int songDuration, string rules)
		{
			this.songId = songId;
			this.songName = songName;
			this.mode = mode;
			this.difficulty = difficulty;
			this.songCoverImg = songCoverImg;
			this.songDuration = songDuration;
			this.rules = rules;
			if (string.IsNullOrEmpty(rules))
			{
				this.rules = JsonConvert.SerializeObject(new GameplayModifiersStruct(new GameplayModifiers
				{
					noFail = true
				}));
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000272A4 File Offset: 0x000254A4
		public GameplayModifiers ToGameplayModifiers()
		{
			if (string.IsNullOrEmpty(this.rules))
			{
				GameplayModifiers gameplayModifiers = new GameplayModifiers();
				gameplayModifiers.noFail = true;
				this.rules = JsonConvert.SerializeObject(new GameplayModifiersStruct(gameplayModifiers));
				return gameplayModifiers;
			}
			return JsonConvert.DeserializeObject<GameplayModifiersStruct>(this.rules).ToGameplayModifiers();
		}

		// Token: 0x040004EA RID: 1258
		public string songId;

		// Token: 0x040004EB RID: 1259
		public string songName;

		// Token: 0x040004EC RID: 1260
		public string mode;

		// Token: 0x040004ED RID: 1261
		public string difficulty;

		// Token: 0x040004EE RID: 1262
		public string songCoverImg;

		// Token: 0x040004EF RID: 1263
		public int songDuration;

		// Token: 0x040004F0 RID: 1264
		public string rules;
	}
}
