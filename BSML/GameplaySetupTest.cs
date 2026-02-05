using System;
using BeatSaberMarkupLanguage.Attributes;

namespace BeatSaberMarkupLanguage
{
	// Token: 0x02000006 RID: 6
	internal class GameplaySetupTest : PersistentSingleton<GameplaySetupTest>
	{
		// Token: 0x0400000A RID: 10
		[UIValue("test")]
		private bool checkbox1;

		// Token: 0x0400000B RID: 11
		[UIValue("test2")]
		private bool checkbox2;

		// Token: 0x0400000C RID: 12
		[UIValue("test3")]
		private bool checkbox3;

		// Token: 0x0400000D RID: 13
		[UIValue("test4")]
		private bool checkbox4 = true;
	}
}
