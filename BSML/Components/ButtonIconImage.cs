using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x02000099 RID: 153
	public class ButtonIconImage : MonoBehaviour
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000F178 File Offset: 0x0000D378
		public void SetIcon(string path)
		{
			if (this.image == null)
			{
				return;
			}
			this.image.SetImage(path);
		}

		// Token: 0x040000B8 RID: 184
		public Image image;
	}
}
