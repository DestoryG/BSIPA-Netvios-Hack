using System;

namespace PlayerDataPlugin.BSHandler
{
	// Token: 0x0200000D RID: 13
	public interface IHandler
	{
		// Token: 0x06000065 RID: 101
		void SetupBeforeGameSceneStart();

		// Token: 0x06000066 RID: 102
		void UpdateAtGameScene();
	}
}
