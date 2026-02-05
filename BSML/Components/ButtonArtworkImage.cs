using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x02000098 RID: 152
	public class ButtonArtworkImage : MonoBehaviour
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0000F108 File Offset: 0x0000D308
		public void SetArtwork(string path)
		{
			if (this.image == null)
			{
				this.image = base.GetComponentsInChildren<Image>().FirstOrDefault((Image x) => x.name == "BGArtwork");
			}
			if (this.image == null)
			{
				throw new Exception("Unable to find BG artwork image!");
			}
			this.image.SetImage(path);
		}

		// Token: 0x040000B7 RID: 183
		public Image image;
	}
}
