using System;

namespace PlayerDataPlugin.BSHandler
{
	// Token: 0x0200000F RID: 15
	public class Handler<T> : Singleton<T>, IHandler where T : class, new()
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00002F4C File Offset: 0x0000114C
		public virtual void SetupBeforeGameSceneStart()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002F4C File Offset: 0x0000114C
		public virtual void UpdateAtGameScene()
		{
			throw new NotImplementedException();
		}
	}
}
