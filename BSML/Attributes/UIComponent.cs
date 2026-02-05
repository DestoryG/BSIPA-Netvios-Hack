using System;

namespace BeatSaberMarkupLanguage.Attributes
{
	// Token: 0x020000BB RID: 187
	public class UIComponent : Attribute
	{
		// Token: 0x060003F7 RID: 1015 RVA: 0x00012680 File Offset: 0x00010880
		public UIComponent(string id)
		{
			this.id = id;
		}

		// Token: 0x04000139 RID: 313
		public string id;
	}
}
