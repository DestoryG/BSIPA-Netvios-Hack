using System;
using NetViosCommon.Utility;
using UnityEngine;

namespace PlayerDataPlugin.BSHandler
{
	// Token: 0x02000012 RID: 18
	internal class StandardLevelGameplayManager_Handler : Handler<StandardLevelGameplayManager_Handler>
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003128 File Offset: 0x00001328
		// (set) Token: 0x06000081 RID: 129 RVA: 0x0000311F File Offset: 0x0000131F
		public StandardLevelGameplayManager GameplayManager { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003139 File Offset: 0x00001339
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00003130 File Offset: 0x00001330
		public GameEnergyCounter EnergyCounter { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000086 RID: 134 RVA: 0x0000314A File Offset: 0x0000134A
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003141 File Offset: 0x00001341
		public GameSongController SongController { get; private set; }

		// Token: 0x06000087 RID: 135 RVA: 0x00003154 File Offset: 0x00001354
		public override void UpdateAtGameScene()
		{
			this.GameplayManager = Object.FindObjectOfType<StandardLevelGameplayManager>();
			if (this.GameplayManager == null)
			{
				Logger.log.Error("[Handler] GameplayManager is null.");
				return;
			}
			this.EnergyCounter = this.GameplayManager.GetPrivateField("_gameEnergyCounter");
			if (this.EnergyCounter == null)
			{
				Logger.log.Error("[Handler] EnergyCounter is null.");
				return;
			}
			this.SongController = this.GameplayManager.GetPrivateField("_gameSongController");
			if (this.SongController == null)
			{
				Logger.log.Error("[Handler] SongController is null.");
				return;
			}
		}
	}
}
