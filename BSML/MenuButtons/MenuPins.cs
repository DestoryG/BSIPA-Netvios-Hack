using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;

namespace BeatSaberMarkupLanguage.MenuButtons
{
	// Token: 0x02000087 RID: 135
	internal class MenuPins : PersistentSingleton<MenuPins>
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000D568 File Offset: 0x0000B768
		[UIValue("pin-buttons")]
		public List<object> pinButtons
		{
			get
			{
				return PersistentSingleton<MenuButtons>.instance.pinButtons;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000D574 File Offset: 0x0000B774
		private List<string> Pins
		{
			get
			{
				if (this.pins == null)
				{
					this.pins = Plugin.config.GetString("Pins", "Pinned Mods", "", false).Split(new char[] { ',' }).ToList<string>();
				}
				return this.pins;
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000D5C4 File Offset: 0x0000B7C4
		internal void PinButton(string text)
		{
			this.Pins.Add(text);
			this.Refresh();
			this.SavePins();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D5DE File Offset: 0x0000B7DE
		internal void UnPinButton(string text)
		{
			this.Pins.Remove(text);
			this.Refresh();
			this.SavePins();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000D5F9 File Offset: 0x0000B7F9
		internal bool IsPinned(string text)
		{
			return this.Pins.Contains(text);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000D607 File Offset: 0x0000B807
		internal void Refresh()
		{
			if (this.rootObject != null)
			{
				Object.Destroy(this.rootObject);
				this.Setup();
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000D628 File Offset: 0x0000B828
		internal void Setup()
		{
			MainMenuViewController mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First<MainMenuViewController>();
			PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatSaberMarkupLanguage.Views.main-menu-screen.bsml"), mainMenuViewController.gameObject, this);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000D661 File Offset: 0x0000B861
		internal void SavePins()
		{
			Plugin.config.SetString("Pins", "Pinned Mods", string.Join(",", this.Pins));
		}

		// Token: 0x04000093 RID: 147
		[UIObject("root-object")]
		internal GameObject rootObject;

		// Token: 0x04000094 RID: 148
		private List<string> pins;
	}
}
