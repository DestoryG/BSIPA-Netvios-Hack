using System;

namespace BS_Utils.Gameplay
{
	// Token: 0x0200000B RID: 11
	public class LevelData
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000443B File Offset: 0x0000263B
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00004443 File Offset: 0x00002643
		public GameplayCoreSceneSetupData GameplayCoreSceneSetupData { get; internal set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000444C File Offset: 0x0000264C
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00004454 File Offset: 0x00002654
		public bool IsSet { get; internal set; }

		// Token: 0x060000B9 RID: 185 RVA: 0x0000445D File Offset: 0x0000265D
		internal void Clear()
		{
			this.IsSet = false;
			this.GameplayCoreSceneSetupData = null;
		}
	}
}
