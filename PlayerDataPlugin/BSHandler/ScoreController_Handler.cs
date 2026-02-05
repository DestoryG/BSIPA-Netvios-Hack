using System;
using UnityEngine;

namespace PlayerDataPlugin.BSHandler
{
	// Token: 0x02000011 RID: 17
	internal class ScoreController_Handler : Handler<ScoreController_Handler>
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000030E5 File Offset: 0x000012E5
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000030DC File Offset: 0x000012DC
		public ScoreController ScoreController { get; private set; }

		// Token: 0x0600007E RID: 126 RVA: 0x00002078 File Offset: 0x00000278
		public override void SetupBeforeGameSceneStart()
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000030ED File Offset: 0x000012ED
		public override void UpdateAtGameScene()
		{
			this.ScoreController = Object.FindObjectOfType<ScoreController>();
			if (this.ScoreController == null)
			{
				Logger.log.Error("ScoreController is null");
			}
		}
	}
}
