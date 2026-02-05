using System;
using System.Reflection;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Settings
{
	// Token: 0x0200006E RID: 110
	internal class SettingsMenu : CustomListTableData.CustomCellInfo
	{
		// Token: 0x060001DD RID: 477 RVA: 0x0000BE6C File Offset: 0x0000A06C
		public SettingsMenu(string name, string resource, object host, Assembly assembly)
			: base(name, null, null, null)
		{
			this.resource = resource;
			this.host = host;
			this.assembly = assembly;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000BE90 File Offset: 0x0000A090
		public void Setup()
		{
			this.viewController = BeatSaberUI.CreateViewController<ViewController>();
			SettingsMenu.SetupViewControllerTransform(this.viewController);
			this.parserParams = PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(this.assembly, this.resource), this.viewController.gameObject, this.host);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		public static void SetupViewControllerTransform(ViewController viewController)
		{
			viewController.rectTransform.sizeDelta = new Vector2(110f, 0f);
			viewController.rectTransform.anchorMin = new Vector2(0.5f, 0f);
			viewController.rectTransform.anchorMax = new Vector2(0.5f, 1f);
		}

		// Token: 0x04000046 RID: 70
		public ViewController viewController;

		// Token: 0x04000047 RID: 71
		public BSMLParserParams parserParams;

		// Token: 0x04000048 RID: 72
		public string resource;

		// Token: 0x04000049 RID: 73
		public object host;

		// Token: 0x0400004A RID: 74
		public Assembly assembly;
	}
}
