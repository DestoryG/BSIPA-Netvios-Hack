using System;
using UnityEngine;

namespace PlayerDataPlugin.BSHandler
{
	// Token: 0x0200000C RID: 12
	internal class GameNoteController_Handler : Handler<GameNoteController_Handler>
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002F12 File Offset: 0x00001112
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002F09 File Offset: 0x00001109
		public GameNoteController[] GameNoteControllers { get; private set; }

		// Token: 0x06000063 RID: 99 RVA: 0x00002F1A File Offset: 0x0000111A
		public override void UpdateAtGameScene()
		{
			this.GameNoteControllers = Resources.FindObjectsOfTypeAll<GameNoteController>();
		}
	}
}
