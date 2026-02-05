using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A6 RID: 166
	public class Strokable : MonoBehaviour
	{
		// Token: 0x06000366 RID: 870 RVA: 0x000107CC File Offset: 0x0000E9CC
		public void SetType(Strokable.StrokeType strokeType)
		{
			if (this.image == null)
			{
				return;
			}
			switch (strokeType)
			{
			case Strokable.StrokeType.None:
				this.image.enabled = false;
				return;
			case Strokable.StrokeType.Clean:
				this.image.enabled = true;
				this.image.sprite = Resources.FindObjectsOfTypeAll<Sprite>().Last((Sprite x) => x.name == "RoundRectSmallStroke");
				return;
			case Strokable.StrokeType.Regular:
				this.image.enabled = true;
				this.image.sprite = Resources.FindObjectsOfTypeAll<Sprite>().Last((Sprite x) => x.name == "RoundRectBigStroke");
				return;
			default:
				return;
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0001088C File Offset: 0x0000EA8C
		public void SetColor(string strokeColor)
		{
			if (this.image == null)
			{
				return;
			}
			Color color;
			if (!ColorUtility.TryParseHtmlString(strokeColor, ref color))
			{
				Logger.log.Warn("Invalid color: " + strokeColor);
			}
			this.image.color = color;
			this.image.enabled = true;
		}

		// Token: 0x040000F7 RID: 247
		public Image image;

		// Token: 0x02000146 RID: 326
		public enum StrokeType
		{
			// Token: 0x040002D1 RID: 721
			None,
			// Token: 0x040002D2 RID: 722
			Clean,
			// Token: 0x040002D3 RID: 723
			Regular
		}
	}
}
