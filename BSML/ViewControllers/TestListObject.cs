using System;
using BeatSaberMarkupLanguage.Attributes;

namespace BeatSaberMarkupLanguage.ViewControllers
{
	// Token: 0x02000012 RID: 18
	public class TestListObject
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00004AA2 File Offset: 0x00002CA2
		public TestListObject(string title, bool shouldGlow)
		{
			this.title = title;
			this.shouldGlow = shouldGlow;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004AB8 File Offset: 0x00002CB8
		[UIAction("button-click")]
		private void ClickedButton()
		{
			Logger.log.Info("Button - " + this.title);
		}

		// Token: 0x04000028 RID: 40
		[UIValue("title")]
		public string title;

		// Token: 0x04000029 RID: 41
		[UIValue("should-glow")]
		public bool shouldGlow;
	}
}
