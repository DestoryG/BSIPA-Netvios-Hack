using System;
using Newtonsoft.Json;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x0200008C RID: 140
	public struct GameplayModifiersStruct
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x000267E4 File Offset: 0x000249E4
		public GameplayModifiersStruct(GameplayModifiers modifiers)
		{
			this.songSpeed = modifiers.songSpeed;
			this.noFail = modifiers.noFail;
			this.noObstacles = modifiers.noObstacles;
			this.noBombs = modifiers.noBombs;
			this.noArrows = modifiers.noArrows;
			this.instaFail = modifiers.instaFail;
			this.batteryEnergy = modifiers.batteryEnergy;
			this.disappearingArrows = modifiers.disappearingArrows;
			this.ghostNotes = modifiers.ghostNotes;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00026860 File Offset: 0x00024A60
		public GameplayModifiersStruct(string rules)
		{
			if (string.IsNullOrEmpty(rules))
			{
				Logger.log.Warn("rules is Empty");
				GameplayModifiers gameplayModifiers = new GameplayModifiers();
				this.songSpeed = gameplayModifiers.songSpeed;
				this.noFail = true;
				this.noObstacles = gameplayModifiers.noObstacles;
				this.noBombs = gameplayModifiers.noBombs;
				this.noArrows = gameplayModifiers.noArrows;
				this.instaFail = gameplayModifiers.instaFail;
				this.batteryEnergy = gameplayModifiers.batteryEnergy;
				this.disappearingArrows = gameplayModifiers.disappearingArrows;
				this.ghostNotes = gameplayModifiers.ghostNotes;
				return;
			}
			GameplayModifiersStruct gameplayModifiersStruct = JsonConvert.DeserializeObject<GameplayModifiersStruct>(rules);
			this.songSpeed = gameplayModifiersStruct.songSpeed;
			this.noFail = gameplayModifiersStruct.noFail;
			this.noObstacles = gameplayModifiersStruct.noObstacles;
			this.noBombs = gameplayModifiersStruct.noBombs;
			this.noArrows = gameplayModifiersStruct.noArrows;
			this.instaFail = gameplayModifiersStruct.instaFail;
			this.batteryEnergy = gameplayModifiersStruct.batteryEnergy;
			this.disappearingArrows = gameplayModifiersStruct.disappearingArrows;
			this.ghostNotes = gameplayModifiersStruct.ghostNotes;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00026968 File Offset: 0x00024B68
		public GameplayModifiers ToGameplayModifiers()
		{
			return new GameplayModifiers
			{
				songSpeed = this.songSpeed,
				noFail = this.noFail,
				noObstacles = this.noObstacles,
				noBombs = this.noBombs,
				noArrows = this.noArrows,
				instaFail = this.instaFail,
				batteryEnergy = this.batteryEnergy,
				disappearingArrows = this.disappearingArrows,
				ghostNotes = this.ghostNotes
			};
		}

		// Token: 0x0400048F RID: 1167
		public GameplayModifiers.SongSpeed songSpeed;

		// Token: 0x04000490 RID: 1168
		public bool noFail;

		// Token: 0x04000491 RID: 1169
		public bool noObstacles;

		// Token: 0x04000492 RID: 1170
		public bool noBombs;

		// Token: 0x04000493 RID: 1171
		public bool noArrows;

		// Token: 0x04000494 RID: 1172
		public bool instaFail;

		// Token: 0x04000495 RID: 1173
		public bool batteryEnergy;

		// Token: 0x04000496 RID: 1174
		public bool disappearingArrows;

		// Token: 0x04000497 RID: 1175
		public bool ghostNotes;
	}
}
