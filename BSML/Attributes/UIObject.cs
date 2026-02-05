using System;

namespace BeatSaberMarkupLanguage.Attributes
{
	// Token: 0x020000BC RID: 188
	public class UIObject : Attribute
	{
		// Token: 0x060003F8 RID: 1016 RVA: 0x0001268F File Offset: 0x0001088F
		public UIObject(string id)
		{
			this.id = id;
		}

		// Token: 0x0400013A RID: 314
		public string id;
	}
}
