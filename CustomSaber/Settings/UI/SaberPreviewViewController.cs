using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomSaber.Data;
using CustomSaber.Utilities;
using HMUI;

namespace CustomSaber.Settings.UI
{
	// Token: 0x02000018 RID: 24
	internal class SaberPreviewViewController : BSMLResourceViewController
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00005225 File Offset: 0x00003425
		public override string ResourceName
		{
			get
			{
				return "CustomSaber.Settings.UI.Views.saberPreview.bsml";
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000522C File Offset: 0x0000342C
		public void OnSaberWasChanged(CustomSaberData customSaber)
		{
			bool flag = !string.IsNullOrWhiteSpace((customSaber != null) ? customSaber.ErrorMessage : null);
			if (flag)
			{
				this.errorDescription.gameObject.SetActive(true);
				TextPageScrollView textPageScrollView = this.errorDescription;
				SaberDescriptor descriptor = customSaber.Descriptor;
				textPageScrollView.SetText(((descriptor != null) ? descriptor.SaberName : null) + ":\n\n" + Utils.SafeUnescape(customSaber.ErrorMessage));
			}
			else
			{
				this.errorDescription.gameObject.SetActive(false);
			}
		}

		// Token: 0x04000073 RID: 115
		[UIComponent("error-description")]
		public TextPageScrollView errorDescription;
	}
}
