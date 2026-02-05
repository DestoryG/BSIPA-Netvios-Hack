using System;
using NetViosCommon.Utility;
using UnityEngine;

namespace PlayerDataPlugin.BSHandler
{
	// Token: 0x02000010 RID: 16
	internal class MainFlowCoordinator_Handler : Handler<MainFlowCoordinator_Handler>
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002F64 File Offset: 0x00001164
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00002F5B File Offset: 0x0000115B
		public MainFlowCoordinator MainFlowCoordinator { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002F75 File Offset: 0x00001175
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002F6C File Offset: 0x0000116C
		public SoloFreePlayFlowCoordinator SoloFreePlayFlowCoordinator { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002F86 File Offset: 0x00001186
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00002F7D File Offset: 0x0000117D
		public ResultsViewController ResultsViewController { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002F97 File Offset: 0x00001197
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002F8E File Offset: 0x0000118E
		public bool IsInSoloMode { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002FA8 File Offset: 0x000011A8
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002F9F File Offset: 0x0000119F
		public LevelCompletionResults LevelCompletionResults { get; private set; }

		// Token: 0x06000077 RID: 119 RVA: 0x00002FB0 File Offset: 0x000011B0
		public override void SetupBeforeGameSceneStart()
		{
			GameObject gameObject = GameObject.Find("MainFlowCoordinator");
			if (gameObject == null)
			{
				Logger.log.Error("mainFlowCordinatorObj is null");
				return;
			}
			this.MainFlowCoordinator = gameObject.GetComponent<MainFlowCoordinator>();
			if (this.MainFlowCoordinator == null)
			{
				Logger.log.Error("MainFlowCoordinator is null");
				return;
			}
			this.SoloFreePlayFlowCoordinator = this.MainFlowCoordinator.GetPrivateField("_soloFreePlayFlowCoordinator");
			if (this.SoloFreePlayFlowCoordinator == null)
			{
				Logger.log.Error("SoloFreePlayFlowCoordinator is null");
				return;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000303F File Offset: 0x0000123F
		public override void UpdateAtGameScene()
		{
			if (this.MainFlowCoordinator == null)
			{
				Logger.log.Error("UpdateAtGameScene: MainFlowCoordinator is null.");
				return;
			}
			this.IsInSoloMode = this.MainFlowCoordinator.childFlowCoordinator == this.SoloFreePlayFlowCoordinator;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000307B File Offset: 0x0000127B
		private void UpdateRootComponents()
		{
			this.ResultsViewController = this.SoloFreePlayFlowCoordinator.GetPrivateField("_resultsViewController");
			if (this.ResultsViewController == null)
			{
				Logger.log.Error("ResultsViewController is null");
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000030B0 File Offset: 0x000012B0
		public LevelCompletionResults UpdateLevelCompletionResults()
		{
			this.UpdateRootComponents();
			this.LevelCompletionResults = this.ResultsViewController.GetPrivateField("_levelCompletionResults");
			return this.LevelCompletionResults;
		}
	}
}
