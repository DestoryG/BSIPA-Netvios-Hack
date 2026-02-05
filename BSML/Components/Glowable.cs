using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x0200009F RID: 159
	public class Glowable : MonoBehaviour
	{
		// Token: 0x06000331 RID: 817 RVA: 0x0000F61C File Offset: 0x0000D81C
		public void SetGlow(string colorString)
		{
			if (this.image == null)
			{
				return;
			}
			if (colorString != "none")
			{
				Color color;
				if (!ColorUtility.TryParseHtmlString(colorString, ref color))
				{
					Logger.log.Warn("Invalid color: " + colorString);
				}
				this.image.color = color;
				this.image.gameObject.SetActive(true);
				return;
			}
			this.image.gameObject.SetActive(false);
		}

		// Token: 0x040000CF RID: 207
		public Image image;
	}
}
