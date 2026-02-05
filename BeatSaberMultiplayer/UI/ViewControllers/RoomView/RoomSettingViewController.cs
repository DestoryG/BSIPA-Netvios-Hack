using System;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000060 RID: 96
	internal class RoomSettingViewController : BSMLResourceViewController
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0001C5DE File Offset: 0x0001A7DE
		public override string ResourceName
		{
			get
			{
				return string.Join(".", new string[]
				{
					base.GetType().Namespace,
					base.GetType().Name,
					"bsml"
				});
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001F18D File Offset: 0x0001D38D
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
		}
	}
}
