using System;

namespace BeatSaberMarkupLanguage.Attributes
{
	// Token: 0x020000BA RID: 186
	public class UIAction : Attribute
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x00012671 File Offset: 0x00010871
		public UIAction(string id)
		{
			this.id = id;
		}

		// Token: 0x04000138 RID: 312
		public string id;
	}
}
