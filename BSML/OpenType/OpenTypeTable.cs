using System;

namespace BeatSaberMarkupLanguage.OpenType
{
	// Token: 0x0200007E RID: 126
	public abstract class OpenTypeTable
	{
		// Token: 0x06000254 RID: 596
		public abstract void ReadFrom(OpenTypeReader reader, uint length);
	}
}
