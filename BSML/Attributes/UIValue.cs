using System;

namespace BeatSaberMarkupLanguage.Attributes
{
	// Token: 0x020000BE RID: 190
	public class UIValue : Attribute
	{
		// Token: 0x060003FA RID: 1018 RVA: 0x000126A6 File Offset: 0x000108A6
		public UIValue(string id)
		{
			this.id = id;
		}

		// Token: 0x0400013B RID: 315
		public string id;
	}
}
