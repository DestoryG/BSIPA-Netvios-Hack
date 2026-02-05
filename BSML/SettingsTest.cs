using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage
{
	// Token: 0x0200000B RID: 11
	public class SettingsTest : PersistentSingleton<SettingsTest>
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00003B70 File Offset: 0x00001D70
		[UIAction("#apply")]
		public void OnApply()
		{
			Logger.log.Info(string.Format("{0}", this.sliderValue));
			Logger.log.Info(this.testString ?? "");
			Logger.log.Info(string.Format("Bool Test: {0}", this.boolTest));
			Logger.log.Info("List Test: " + this.listChoice);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003BEE File Offset: 0x00001DEE
		[UIAction("format")]
		public string Format(int number)
		{
			return number.ToString() + "x";
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000263A File Offset: 0x0000083A
		public void Update()
		{
		}

		// Token: 0x04000015 RID: 21
		[UIParams]
		private BSMLParserParams parserParams;

		// Token: 0x04000016 RID: 22
		[UIValue("list-options")]
		private List<object> options = new object[] { "1", "Something", "Kapow", "Yeet" }.ToList<object>();

		// Token: 0x04000017 RID: 23
		[UIValue("list-choice")]
		private string listChoice = "Something";

		// Token: 0x04000018 RID: 24
		[UIValue("bool-test")]
		private bool boolTest = true;

		// Token: 0x04000019 RID: 25
		[UIValue("slider-value")]
		private int sliderValue = 5;

		// Token: 0x0400001A RID: 26
		[UIValue("string-value")]
		private string testString = "Shazam";

		// Token: 0x0400001B RID: 27
		[UIValue("color-value")]
		private Color testColor = Color.yellow;
	}
}
