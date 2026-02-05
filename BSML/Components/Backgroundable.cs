using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000AA RID: 170
	public class Backgroundable : MonoBehaviour
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00010CD8 File Offset: 0x0000EED8
		private static Dictionary<string, string> Backgrounds
		{
			get
			{
				return new Dictionary<string, string>
				{
					{ "round-rect-panel", "RoundRectPanel" },
					{ "panel-bottom", "PanelBottom" },
					{ "panel-top", "PanelTop" },
					{ "round-rect-panel-shadow", "RoundRectPanelShadow" }
				};
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00010D2C File Offset: 0x0000EF2C
		private static Dictionary<string, string> ObjectNames
		{
			get
			{
				return new Dictionary<string, string>
				{
					{ "round-rect-panel", "MinScoreInfo" },
					{ "panel-bottom", "BG" },
					{ "panel-top", "HeaderPanel" },
					{ "round-rect-panel-shadow", "Shadow" }
				};
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00010D80 File Offset: 0x0000EF80
		public void ApplyBackground(string name)
		{
			if (this.background != null)
			{
				throw new Exception("Cannot add multiple backgrounds");
			}
			string backgroundName;
			if (!Backgroundable.Backgrounds.TryGetValue(name, out backgroundName))
			{
				throw new Exception("Background type '" + name + "' not found");
			}
			this.background = base.gameObject.AddComponent(Resources.FindObjectsOfTypeAll<Image>().Last(delegate(Image x)
			{
				if (x.gameObject.name == Backgroundable.ObjectNames[name])
				{
					Sprite sprite = x.sprite;
					return ((sprite != null) ? sprite.name : null) == backgroundName;
				}
				return false;
			}));
			this.background.enabled = true;
		}

		// Token: 0x04000107 RID: 263
		public Image background;
	}
}
