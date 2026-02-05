using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using TMPro;

namespace BeatSaverDownloader.UI.ViewControllers
{
	// Token: 0x0200001B RID: 27
	public class SongDescriptionViewController : BSMLResourceViewController
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000519D File Offset: 0x0000339D
		public override string ResourceName
		{
			get
			{
				return "BeatSaverDownloader.UI.BSML.songDescription.bsml";
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000043AD File Offset: 0x000025AD
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000051A4 File Offset: 0x000033A4
		internal void ClearData()
		{
			if (this.songDescription)
			{
				this.songDescription.SetText("");
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000051C3 File Offset: 0x000033C3
		internal void Initialize(string description)
		{
			this.songDescription.SetText(description);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00002053 File Offset: 0x00000253
		[UIAction("#post-parse")]
		internal void Setup()
		{
		}

		// Token: 0x0400006A RID: 106
		[UIComponent("songDescription")]
		internal TextMeshProUGUI songDescription;
	}
}
